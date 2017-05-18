using Appleseed.Framework.Settings;
using Appleseed.Framework.Site.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appleseed.Framework.Site.Data
{
    public class UserPagePermission
    {
        public int Id { get; set; }
        public int PageId { get; set; }
        public Guid UserId { get; set; }
        public int Permission { get; set; }
    }

    public class UserInfo
    {
        public Guid UserId { get; set; }
        public Guid ApplicationId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Permission { get; set; }
        public int PageId { get; set; }
    }

    public class UserPagePermissionDB
    {
        public List<UserPagePermission> GetPermissionsByPageId(int pageid)
        {

            return Appleseed.Framework.Data.DBHelper.DataReaderToObjectList<UserPagePermission>("select * from rb_userPagePermission WHERE PAGEID = " + pageid);

        }

        public List<UserInfo> GetAllUsers(int pageId, Guid userId)
        {
            string query = string.Format("SELECT m.ApplicationId, m.UserId, m.Email, cp.Name, ISNULL(upp.Permission,1) as Permission, {1} PageId " +
                                         "FROM aspnet_Membership as m LEFT JOIN aspnet_CustomProfile as cp " +
                                         "ON m.UserId = cp.UserId " +
                                         "inner join aspnet_Applications as a " +
                                         "on a.ApplicationName = '{0}'and a.ApplicationId = m.ApplicationId " +
                                         "left join rb_userPagePermission upp on upp.UserId = m.UserId and upp.PageId = {1} Where m.UserId != '{2}'" +
                                         "order by cp.Name", Portal.UniqueID, pageId, userId);

            return Appleseed.Framework.Data.DBHelper.DataReaderToObjectList<UserInfo>(query);
        }

        public static UserPagePermission GetUserPagePermission(int pageid, Guid UserId)
        {
            var pp = Appleseed.Framework.Data.DBHelper.DataReaderToObjectList<UserPagePermission>("select * from rb_userPagePermission WHERE PAGEID = " + pageid + " AND UserId = '" + UserId + "'");
            if (pp.Count > 0)
            {
                return pp[0];
            }

            return new UserPagePermission() { Permission = 0, UserId = UserId, PageId = pageid };
        }

        public static bool HasEditPermission(int pageid, Guid userId)
        {
            var upp = GetUserPagePermission(pageid, userId);
            if (upp.Permission == 2 || System.Web.HttpContext.Current.User.IsInRole("Admins"))
            {
                return true;
            }
            return false;
        }

        private static PortalSettings PortalSettings
        {
            get
            {
                return (PortalSettings)System.Web.HttpContext.Current.Items["PortalSettings"];
            }
        }

        public static bool HasCurrentPageEditPermission(ModuleSettings moduleSettings = null)
        {
            if (System.Web.HttpContext.Current.User.IsInRole("Admins"))
            {
                return true;
            }

            var upp = GetUserPagePermission(PortalSettings.ActivePage.PageID, Guid.Parse(System.Web.Security.Membership.GetUser().ProviderUserKey.ToString()));
            if (upp.Permission == 2)
            {
                return true;
            }
            else if((upp.Permission == 0 && System.Web.HttpContext.Current.User.IsInRole("Builder") && moduleSettings != null && !moduleSettings.Admin) 
                || ((upp.Permission == 0 && System.Web.HttpContext.Current.User.IsInRole("Builder") && moduleSettings == null)))
            {
                return true;
            }

            return false;
        }

        public void UpdatePagePermissions(List<UserPagePermission> permissions)
        {
            using (var connection = Config.SqlConnectionString)
            {
                connection.Open();
                foreach (var permis in permissions)
                {
                    using (var sqlCommand = new SqlCommand("UPDATE rb_userPagePermission SET Permission = " + permis.Permission + " WHERE PageId = " + permis.PageId + " and UserId = '" + permis.UserId + "'", connection))
                    {
                        // Mark the Command as a SPROC
                        sqlCommand.CommandType = CommandType.Text;

                        var up = Appleseed.Framework.Data.DBHelper.DataReaderToObjectList<UserPagePermission>("select * from rb_userPagePermission WHERE PAGEID = " + permis.PageId + " AND UserId = '" + permis.UserId + "'");
                        if (up.Count == 0)
                        {
                            sqlCommand.CommandText = "INSERT INTO rb_userPagePermission (PageId, UserId, Permission) VALUES (" + permis.PageId + ", '" + permis.UserId + "', " + permis.Permission + ")";
                        }

                        try
                        {
                            sqlCommand.ExecuteNonQuery();
                        }
                        finally
                        {
                        }
                    }
                }
            }
        }
    }
}
