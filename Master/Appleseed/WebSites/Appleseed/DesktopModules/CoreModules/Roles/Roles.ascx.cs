using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Appleseed.Framework;
using Appleseed.Framework.Security;
using Appleseed.Framework.Users.Data;
using Appleseed.Framework.Web.UI.WebControls;
using HyperLink = Appleseed.Framework.Web.UI.WebControls.HyperLink;
using ImageButton = Appleseed.Framework.Web.UI.WebControls.ImageButton;
using Label = Appleseed.Framework.Web.UI.WebControls.Label;
using System.Collections;
using Appleseed.Framework.Providers.AppleseedRoleProvider;
using Appleseed.Framework.Site.Configuration;
using System.Web;

namespace Appleseed.Content.Web.Modules
{
    public partial class Roles : PortalModuleControl
    {
        protected ImageButton RoleDeleteBtn;

        /// <summary>
        /// Admin Module
        /// </summary>
        /// <value></value>
        public override bool AdminModule
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // If this is the first visit to the page, bind the role data to the datalist
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }

        /// <summary>
        /// Handles the ItemDataBound event of the RolesList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.Web.UI.WebControls.DataListItemEventArgs"/> instance containing the event data.</param>
        protected void RolesList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            // 20/7/2004 changed by Mario Endara mario@softworks.com.uy
            // don't let the user to edit or delete the role "Admins"
            // the rolename is an hyperlink to the list of users of the role
            Control dl = e.Item.FindControl("ImageButton1");
            Control d2 = e.Item.FindControl("ImageButton2");
            HyperLink d3 = (HyperLink)e.Item.FindControl("Name");

            AppleseedRole role = ((AppleseedRole)e.Item.DataItem);

            // Added by Mario Endara <mario@softworks.com.uy> 2004/11/04
            // if the user is not member of the "Admins" role, he can´t access to the members of the Admins role
            // added mID by Mario Endara <mario@softworks.com.uy> to support security check (2004/11/27)
            if ((d3 != null) && (PortalSecurity.IsInRoles("Admins") == true || role.Name != "Admins"))
            {
                d3.NavigateUrl = HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/Roles/SecurityRoles.aspx", PageID,
                    "mID=" + ModuleID + "&roleID=" + role.Id.ToString());
            }

            if (dl != null)
            {
                if (role.Name.Equals("Admins"))
                    dl.Visible = false;
                ((ImageButton)dl).Attributes.Add("OnClick", "return confirmDelete()");
            }
            if (d2 != null)
            {
                if (role.Name.Equals("Admins"))
                    d2.Visible = false;
            }
        }

        private PortalSettings CurrentPortalSettings
        {
            get
            {
                return (PortalSettings)HttpContext.Current.Items["PortalSettings"];
            }
        }


        /// <summary>
        /// Handles the Click event of the AddRole control.
        /// </summary>
        /// <param name="Sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected void AddRole_Click(Object Sender, EventArgs e)
        {
            //http://sourceforge.net/tracker/index.php?func=detail&aid=828580&group_id=66837&atid=515929
            try
            {
                // Add a new role to the database
                new UsersDB().AddRole(CurrentPortalSettings.PortalAlias, txtNewRole.Text);

                txtNewRole.Text = string.Empty;
            }
            catch (Exception ex)
            {
                // new role is already present more than likely
                ErrorHandler.Publish(LogLevel.Error, "AddRole_Click error: new role is already present more than likely",
                                     ex);
            }

            // Rebind list
            BindData();
        }

        /// <summary>
        /// The RolesList_ItemCommand server event handler on this page
        /// is used to handle the user editing and deleting roles
        /// from the RolesList asp:datalist control
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.Web.UI.WebControls.DataListCommandEventArgs"/> instance containing the event data.</param>
        protected void rolesList_ItemCommand(object source, DataListCommandEventArgs e)
        {
            //http://sourceforge.net/tracker/index.php?func=detail&aid=828580&group_id=66837&atid=515929
            UsersDB users = new UsersDB();

            bool enable = true; // enable add - bja

            if (e.CommandName == "edit")
            {
                // Set editable list item index if "edit" button clicked next to the item
                rolesList.EditItemIndex = e.Item.ItemIndex;
                // disable the add function
                enable = false;
                // Repopulate the datalist control
                BindData();
            }

            else if (e.CommandName == "apply")
            {

                var _roleName = ((TextBox)e.Item.FindControl("roleName")).Text;
                var _roleId = ((System.Web.UI.WebControls.Label)e.Item.FindControl("roleId")).Text;

                // update database
                users.UpdateRole(new Guid(_roleId), _roleName, this.PortalSettings.PortalAlias);

                // Disable editable list item access
                rolesList.EditItemIndex = -1;

                // Repopulate the datalist control
                BindData();
            }
            else if (e.CommandName == "delete")
            {
                // john.mandia@whitelightsolutions.com: 30th May 2004: Added Try And Catch To Delete Role
                // update database
                try
                {
                    users.DeleteRole(new Guid(e.CommandArgument.ToString()), this.PortalSettings.PortalAlias);
                }
                catch
                {
                    labelError.Visible = true;
                }
                // End of john.mandia@whitelightsolutions.com Update

                // Ensure that item is not editable
                rolesList.EditItemIndex = -1;

                // Repopulate list
                BindData();
            }
            else if (e.CommandName == "members")
            {

                string _roleId = ((System.Web.UI.WebControls.Label)e.Item.FindControl("roleId")).Text;

                // Role names shouldn't be editable, it's not supported by the Roles Provider API
                //// Save role name changes first
                //users.UpdateRole( selectedRole.Id, _roleName, portalSettings.PortalAlias );

                // redirect to edit page
                Response.Redirect(
                    HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/Roles/SecurityRoles.aspx", PageID,
                                            "mID=" + ModuleID.ToString() + "&roleID=" + _roleId));
            }
            // reset the enable state of the add
            // set add button -- bja
            AddRoleBtn.Enabled = enable;
        }

        /// <summary>
        /// The BindData helper method is used to bind the list of
        /// security roles for this portal to an asp:datalist server control
        /// </summary>
        private void BindData()
        {
            // Get the portal's roles from the database
            UsersDB users = new UsersDB();

            IList<AppleseedRole> roles = users.GetPortalRoles(this.PortalSettings.PortalAlias);

            // remove "All Users", "Authenticated Users" and "Unauthenticated Users" pseudo-roles
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

            rolesList.DataSource = roles;
            rolesList.DataBind();
        }

        /// <summary>
        /// Guid
        /// </summary>
        /// <value></value>
        public override Guid GuidID
        {
            get
            {
                return new Guid("{A406A674-76EB-4BC1-BB35-50CD2C251F9C}");
            }
        }

        #region Web Form Designer generated code

        /// <summary>
        /// On init
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Load += new EventHandler(this.Page_Load);
        }

        #endregion
    }
}