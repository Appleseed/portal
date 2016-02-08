using Appleseed.Content.Web.Modules;
using Appleseed.Framework.DAL;
using Appleseed.Framework.Providers.AppleseedRoleProvider;
using Appleseed.Framework.Security;
using Appleseed.Framework.Users.Data;
using Appleseed.Framework.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Appleseed.DesktopModules.CoreModules.Roles
{
    public partial class AccessPermissionViewer : Appleseed.Framework.Web.UI.SecurePage
    {
        private void generateTable()
        {
            PermissionsDB permissions = new PermissionsDB();
            List<PermissionInfo> PermissionList = permissions.Permissions();

            var assignedPermissions = permissions.AssignedPermissions();

            UsersDB users = new UsersDB();
            IList<AppleseedRole> roles = users.GetPortalRoles(this.PortalSettings.PortalAlias);

            AppleseedRole pseudoRole = new AppleseedRole(AppleseedRoleProvider.AllUsersGuid, AppleseedRoleProvider.AllUsersRoleName);
            if (roles.Contains(pseudoRole))
            {
                roles.Remove(pseudoRole);
            }

            pseudoRole = new AppleseedRole(AppleseedRoleProvider.AuthenticatedUsersGuid, AppleseedRoleProvider.AuthenticatedUsersRoleName);
            if (roles.Contains(pseudoRole))
            {
                roles.Remove(pseudoRole);
            }

            pseudoRole = new AppleseedRole(AppleseedRoleProvider.UnauthenticatedUsersGuid, AppleseedRoleProvider.UnauthenticatedUsersRoleName);
            if (roles.Contains(pseudoRole))
            {
                roles.Remove(pseudoRole);
            }

            roles = roles.Where(X => X.Name.ToLower() != "admins").ToList();
            if (RoleID != Guid.Empty)
            {
                roles = roles.Where(role => role.Id == RoleID).ToList();
            }

            StringBuilder s = new StringBuilder();
            s.Append("<div id='accessviewer'><table border='1'>");
            for (int i = 0; i < PermissionList.Count + 1; i++)
            {
                s.Append("<tr>");
                if (i == 0)
                {
                    s.Append("<td class='headcol'>Permission / Role</td>");
                }
                else
                {
                    s.Append("<td class='headcol'>" + PermissionList[i - 1].PermissionName + "</td>");
                }

                for (int j = 0; j < roles.Count; j++)
                {
                    if (i == 0)
                    {
                        s.Append("<td class='long'>" + roles[j].Name + "</td>");
                    }
                    else
                    {
                        var permis = assignedPermissions.FirstOrDefault(per => per.RoleID == roles[j].Id && per.PermissionID == PermissionList[i - 1].PermissionID);
                        if (permis == null)
                        {
                            s.Append("<td class='long'><input type='checkbox' id='" + PermissionList[i - 1].PermissionID + '#' + roles[j].Id + "'/></td>");
                        }
                        else
                        {
                            s.Append("<td class='long'><input type='checkbox' checked='checked' id='" + PermissionList[i - 1].PermissionID + '#' + roles[j].Id + "'/></td>");
                        }
                    }
                }

                s.Append("</tr>");
            }
            s.Append("</table></div>");
            ltrAccessViewer.Text = s.ToString();
        }

        private Guid RoleID
        {
            get
            {
                Guid rID = Guid.Empty;
                Guid.TryParse(Request.QueryString["rid"], out rID);
                return rID;
            }
        }

        /// <summary>
        /// Set the module guids with free access to this page
        /// </summary>
        /// <value>The allowed modules.</value>
        protected override List<string> AllowedModules
        {
            get
            {
                var al = new List<string> { "A406A674-76EB-4BC1-BB35-50CD2C251F9C" };
                return al;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.UrlReferrer != null)
                    ReturnUrl.Value = Request.UrlReferrer.PathAndQuery;
                generateTable();
            }
        }

        protected void btnSaveAccess_Click(object sender, EventArgs e)
        {
            string selectedPermissionIDRoleID = selectedPermissions.Value;
            PermissionsDB permision = new PermissionsDB();
            permision.AssignPermissions(selectedPermissionIDRoleID, RoleID);
            if (!string.IsNullOrEmpty(ReturnUrl.Value))
                Response.Redirect(ReturnUrl.Value);
        }

        protected void btnCancelAccess_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ReturnUrl.Value))
                Response.Redirect(ReturnUrl.Value);
        }
    }
}