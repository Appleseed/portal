using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Appleseed.Framework;
using Appleseed.Framework.Settings;
using Appleseed.Framework.Users.Data;
using Trirand.Web.Mvc;
using UserManager.Massive;
using UserManager.Models;
using aspnet_CustomProfile = UserManager.Massive.aspnet_CustomProfile;
using Appleseed.Framework.Web.UI.WebControls;
using Appleseed.Framework.Site.Configuration;

namespace UserManager.Controllers
{

    public class UserManagerController : Controller
    {
        public ActionResult Module()
        {
            Session["sKeywords"] = null;

            var segment = Request.Url.Segments;
            var pagenumber = getPageId(segment);
            var mid = (int)ControllerContext.RouteData.Values["moduleId"];
            var urlCreate = Path.ApplicationRoot + "/DesktopModules/CoreModules/Users/UsersManage.aspx?pageId=" + pagenumber +
                            "&mID=" + mid;
            var model = new UserManagerModel { UserEmail = urlCreate };

            return View(model);
        }

        private string getPageId(string[] segment)
        {
            string pagenumber = 0.ToString();

            // if friendly url is enabled 
            if (PortalSettings.HasEnablePageFriendlyUrl(Portal.PageID, Config.DefaultPortal))
            {
                string urlsegment = string.Empty;
                foreach (string seg in segment)
                {
                    urlsegment = urlsegment + seg;
                }
                pagenumber = Appleseed.Framework.UrlRewriting.UrlRewritingFriendlyUrl.GetPageIDFromPageName(urlsegment);
            }
            else
            {
                // else get the pageid based on url
                foreach (var seg in segment)
                {
                    int num;
                    bool isNum = int.TryParse(seg.Split('/').First(), out num);
                    if (isNum)
                    {
                        pagenumber = seg.Split('/').First();
                    }
                }
            }
            return pagenumber;
        }
        public string Builddir(string email)
        {
            string userName = Membership.GetUserNameByEmail(email);
            var segment = Request.UrlReferrer.Segments;
            string pagenumber = getPageId(segment);
            string redurl = Path.ApplicationRoot + "/DesktopModules/CoreModules/Users/UsersManage.aspx?mid=" + pagenumber +
                            "&username=" + userName;
            return redurl;
        }

        public JsonResult Delete(Guid userID)
        {
            var users = new UsersDB();
            users.DeleteUser(userID);
            return Json("ok");
        }

        public void jqxSetSearchKeyword(string keyword)
        {
            Session["sKeywords"] = keyword;
        }

        public JsonResult jqxSearch(int pagenum, int pagesize, string sortorder, string sortdatafield)
        {
            return GridUser(pagenum + 1, pagesize, "", sortorder, sortdatafield);
        }

        public JsonResult Search(string text, int page, int rows, List<UserManagerModel> data)
        {

            var result = new List<UserManagerModel>();
            var words = text.Split(' ');
            int i = 1;
            foreach (var user in data)
            {
                var name = user.UserName ?? "";
                var mail = user.UserEmail;
                var role = user.UserRol ?? "";
                foreach (var word in words)
                {
                    var userMail = mail.Split('@');
                    if (name.ToUpper().Contains(word.ToUpper()) || (role.ToUpper().Contains(word.ToUpper())) || (userMail[0].ToUpper().Contains(word.ToUpper())) || (mail.ToLower().Contains(word.ToLower())))
                    {
                        user.id = i;
                        result.Add(user);
                        i++;
                        break;
                    }
                }

            }
            return GetRowsFromList(result.AsQueryable(), rows, page);
        }
        public string getSearchCondition(string search)
        {
            string whereClase = "";
            if (!string.IsNullOrEmpty(search))
            {
                whereClase = " where 1=2 ";
                foreach (var kw in search.Split(' '))
                {
                    whereClase += " OR " + string.Format("cp.name like '%{0}%' or cp.email like '%{0}%'", kw);
                }
            }
            return whereClase;
        }

        public class UserFormatedModel
        {
            public string UserName { get; set; }
            public string Email { get; set; }
            public string Role { get; set; }
            public string Edit { get; set; }
            public Guid UserId { get; set; }
            public string EditId { get; set; }
            public string Delete { get; set; }
            public long TotalRows { get; set; }
        }

        public class Result
        {
            public int total { get; set; }
            public int page { get; set; }
            public int currentPage { get; set; }
            public long records { get; set; }
            public IQueryable<UserFormatedModel> rows { get; set; }
        }

        public JsonResult GridUser(int page, int rows, string search, string sortorder, string sortdatafield)
        {
            if (Session["sKeywords"] != null && !string.IsNullOrEmpty(Session["sKeywords"].ToString()) && string.IsNullOrEmpty(search))
            {
                search = Session["sKeywords"].ToString();
            }
            else if (!string.IsNullOrEmpty(search))
            {
                Session["sKeywords"] = search;
            }
            var start = (page * rows) - rows + 1;
            var end = start + rows - 1;
            string orderBY = "";
            if (!string.IsNullOrEmpty(sortorder) && !string.IsNullOrEmpty(sortdatafield))
            {
                orderBY = " Order By " + (sortdatafield.ToUpper() == "ROLE" ? "UserRol" : sortdatafield) + " " + sortorder + " ";
            }

            var queryUsers = "WITH YourCTE AS " +
   "(" +
   "SELECT ROW_NUMBER() OVER(ORDER BY cp.Name) AS RowIndx,au.UserName, m.ApplicationId, m.UserId, m.Email, cp.Name, (SELECT ',' + ar.RoleName FROM aspnet_Roles ar where ar.RoleId in (select RoleId from[dbo].[aspnet_UsersInRoles] ur where ur.UserId = m.UserId) FOR XML PATH('')) as UserRol " +
                                          " FROM aspnet_Membership as m LEFT JOIN aspnet_CustomProfile as cp ON m.UserId = cp.UserId " +
                                          " inner join aspnet_users au on au.UserId = m.UserId " +
                                          " inner join aspnet_Applications as a " +
                                          " on a.ApplicationName = '" + Portal.UniqueID + "' and a.ApplicationId = m.ApplicationId " +
                                          "  " + getSearchCondition(search) + " " +
   " ) " +
   " Select *, (SELECT MAX(RowIndx) FROM YourCTE) AS 'TotalRows' from YourCTE   where RowIndx between " + start + " and " + end + orderBY;

            //ErrorHandler.Publish(LogLevel.Info, "User Query - " + queryUsers);

            var tbl = new DynamicModel("ConnectionString", "aspnet_CustomProfile", "UserId");
            dynamic searchedUsers = tbl.Query(queryUsers);
            var dataFormted = new List<UserFormatedModel>();
            foreach (var m in searchedUsers)
            {
                var usr = new UserFormatedModel()
                {
                    UserName = m.UserName,
                    Email = m.Email,
                    Role = m.UserRol,
                    Edit = General.GetString("EDIT_USER"),
                    UserId = m.UserId,
                    EditId = Builddir(m.Email),
                    Delete = General.GetString("DELETE_USER"),
                    TotalRows = m.TotalRows
                };
                if (!string.IsNullOrEmpty(usr.Role))
                {
                    usr.Role = usr.Role.TrimStart(',').TrimEnd(',');
                }
                dataFormted.Add(usr);
            }

            var totalRecords = dataFormted[0].TotalRows;
            var totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
            List<Result> rusts = new List<Result>();
            var jsonData = new Result
            {
                total = totalPages,
                page = page,
                currentPage = page,
                records = totalRecords,
                rows = dataFormted.AsQueryable()
            };
            rusts.Add(jsonData);
            return Json(rusts, JsonRequestBehavior.AllowGet);


            /*
            var data = new List<UserManagerModel>();

            dynamic iduser = tbl.Query("SELECT m.ApplicationId, m.UserId, m.Email, cp.Name " +
                                       "FROM aspnet_Membership as m LEFT JOIN aspnet_CustomProfile as cp " +
                                       "ON m.UserId = cp.UserId " +
                                       "inner join aspnet_Applications as a " +
                                       "on a.ApplicationName = @0 and a.ApplicationId = m.ApplicationId " +
                                       "order by cp.Name", Portal.UniqueID);
            var table = new DynamicModel("ConnectionString", "aspnet_Roles", "RoleId");
            dynamic roles = table.Query("SELECT r.RoleName, ur.UserId " +
                                        "FROM aspnet_Roles r, aspnet_UsersInRoles ur " +
                                        "WHERE r.RoleId = ur.RoleId");
            var iRoles = (IEnumerable<dynamic>)roles;
            var i = 1;
            foreach (var user in iduser)
            {
                var m = new UserManagerModel();
                m.id = i;
                m.UserId = user.UserId;
                m.UserName = user.Name;
                m.UserEmail = user.Email;
                var userrolid = Guid.Parse(user.UserId.ToString());

                try
                {
                    var roleName = iRoles.Where(r => r.UserId == userrolid);
                    m.UserRol = roleName.Single().RoleName;

                }
                catch
                {
                    m.UserRol = "";
                }
                m.Edit = Builddir(m.UserEmail);
                data.Add(m);
                i++;
            }
            if (!string.IsNullOrEmpty(search))
            {
                return Search(search, page, rows, data);
            }
            var result = GetRowsFromList(data.AsQueryable(), rows, page);
            return result;
            */
        }


        private JsonResult GetRowsFromList(IQueryable<UserManagerModel> result, int rows, int page)
        {
            var names = from m in result
                        where m.id > (rows * (page - 1))
                        select new
                        {
                            UserName = m.UserName,
                            Email = m.UserEmail,
                            Role = m.UserRol,
                            Edit = General.GetString("EDIT_USER"),
                            UserId = m.UserId,
                            EditId = m.Edit,
                            Delete = General.GetString("DELETE_USER"),
                        };
            var totalRecords = result.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
            var jsonData = new
            {
                total = totalPages,
                page = page,
                currentPage = page,
                records = totalRecords,
                rows = names.AsQueryable()
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

    }
}
