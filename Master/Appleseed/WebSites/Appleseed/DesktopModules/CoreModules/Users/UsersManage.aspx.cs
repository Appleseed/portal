using System;
using System.Collections;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Appleseed.Framework;
using Appleseed.Framework.Security;
using Appleseed.Framework.Settings;
using Appleseed.Framework.Settings.Cache;
using Appleseed.Framework.Site.Configuration;
using Appleseed.Framework.Users.Data;
using Appleseed.Framework.Web.UI;
using Appleseed.Framework.Web.UI.WebControls;
using History=Appleseed.Framework.History;
using Localize=Appleseed.Framework.Web.UI.WebControls.Localize;
using System.Web.Security;
using Appleseed.Framework.Providers.AppleseedRoleProvider;
using System.Collections.Generic;

namespace Appleseed.Content.Web.Modules
{
    /// <summary>
    /// User manager
    /// </summary>
    [History("jminond", "march 2005", "Changes for moving Tab to Page")]
    [
        History("gman3001", "2004/10/06",
            "Add GetCurrentProfileControl method to properly obtain a custom register control as specified by the 'Register Module ID' setting."
            )]
    public partial class UsersManage : EditItemPage
    {
        private Guid userID = Guid.Empty;
        private string userName = string.Empty;
        //        int tabIndex = 0;
        protected Localize name;
        protected IEditUserProfile EditControl;

        private PortalSettings CurrentPortalSettings
        {
            get
            {
                return (PortalSettings)HttpContext.Current.Items["PortalSettings"];
            }
        }

        /// <summary>
        /// The Page_Load server event handler on this page is used
        /// to populate the role information for the page.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            // Verify that the current user has access to access this page
            // Removed by Mario Endara <mario@softworks.com.uy> (2004/11/04)
            //			if (PortalSecurity.IsInRoles("Admins") == false)
            //				PortalSecurity.AccessDeniedEdit();

            //Code no longer needed here, gman3001 10/06/2004
            /*string RegisterPage;

            //Select the actual register page
            if (portalSettings.CustomSettings["SITESETTINGS_REGISTER_TYPE"] != null &&
                    portalSettings.CustomSettings["SITESETTINGS_REGISTER_TYPE"].ToString() != "register.aspx" )
                RegisterPage = portalSettings.CustomSettings["SITESETTINGS_REGISTER_TYPE"].ToString();
            else
                RegisterPage = "register.aspx";
            */

            // Calculate userid
            if (Request.Params["userid"] != null)
            {
                userID = new Guid(Request.Params["userid"]);
            } 

            string newUserEmail = string.Empty;
            if (Request.Params["username"] != null) {
                userName = (string)Request.Params["username"];
                if (userID == Guid.Empty)
                {
                    userID = (Guid)Membership.GetUser(userName).ProviderUserKey;
                }

            } else {
                allRoles.Visible = false;
                addExisting.Visible = false;
                
                if (Request.Params["email"] != null) {
                    newUserEmail = Convert.ToString(Request.Params["email"]);
                }
            }
            bool outer = false;
            if (Request.Params["outer"] != null) {
                outer = Convert.ToInt32(Request.Params["outer"]) == 1 ? true : false;
            }

            //Control myControl = this.LoadControl("../DesktopModules/Register/" + RegisterPage);
            //Control myControl = this.LoadControl(Appleseed.Framework.Settings.Path.WebPathCombine(Appleseed.Framework.Settings.Path.ApplicationRoot, "DesktopModules/Register", RegisterPage));
            // Line Added by gman3001 10/06/2004, to support proper loading of a register module specified by 'Register Module ID' setting in the Portal Settings admin page
            Control myControl = GetCurrentProfileControl();

            EditControl = ((IEditUserProfile)myControl);
            //EditControl.RedirectPage = HttpUrlBuilder.BuildUrl("~/Admin/UsersManage.aspx", TabID, "username=" + userName + AllowEditUserID);
            register.Controls.Add(myControl);

            // If this is the first visit to the page, bind the role data to the datalist
            if (Page.IsPostBack == false)
            {
                // new user?
                if (userName == string.Empty)
                {
                    userName = newUserEmail;
                    /* COMENTO LA CREACION DE UN USUARIO A PARTIR DE SOLAMENTE EL EMAIL */

                    //try
                    //{
                    //    UsersDB users = new UsersDB();

                    //    // make a unique new user record
                    //    Guid uid = Guid.Empty;
                    //    int i = 0;

                    //    Exception lastException = null;
                    //    while (uid == Guid.Empty && i < 10) //Avoid infinite loop
                    //    {
                    //        string friendlyName = "New User created " + DateTime.Now.ToString();                            
                    //        try
                    //        {

                    //            uid = users.AddUser(friendlyName, userName, string.Empty, CurrentPortalSettings.PortalAlias);
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            uid = Guid.Empty;
                    //            lastException = ex;
                    //        }
                    //        i++;
                    //    }
                    //    if (uid == Guid.Empty)
                    //        throw new Exception("New user creation failed after " + i.ToString() + " retries.",
                    //                            lastException);

                    //    // redirect to this page with the corrected querystring args
                    //    string outerStr = outer == true ? "1" : "0";
                    //    Response.Redirect(
                    //        HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/Users/UsersManage.aspx", PageID,
                    //        "mID=" + ModuleID + "&username=" + userName + "&userid=" + uid.ToString() + "&outer=" + outerStr));
                    //}
                    //catch (Exception ex)
                    //{
                    //    ErrorHandler.Publish(LogLevel.Error, "Error creating new user", ex);
                    //    ErrorLabel.Text = ex.Message;
                    //    ErrorLabel.Visible = true;
                    //}
                }

                BindData();
            }
        }

        // Method Added by gman3001 10/06/2004, to support proper loading of a register module specified by 'Register Module ID' setting in the Portal Settings admin page
        /// <summary>
        /// Gets the current profile control.
        /// </summary>
        /// <returns></returns>
        private Control GetCurrentProfileControl()
        {
            //default
            string RegisterPage = "register.aspx";
            if (HttpContext.Current != null)
            {
                PortalSettings portalSettings = (PortalSettings)HttpContext.Current.Items["PortalSettings"];

                //Select the actual register page
                if (portalSettings.CustomSettings["SITESETTINGS_REGISTER_TYPE"] != null &&
                    portalSettings.CustomSettings["SITESETTINGS_REGISTER_TYPE"].ToString() != "register.aspx")
                {
                    RegisterPage = portalSettings.CustomSettings["SITESETTINGS_REGISTER_TYPE"].ToString();
                }

                int moduleID = int.Parse(portalSettings.CustomSettings["SITESETTINGS_REGISTER_MODULEID"].ToString());
                string moduleDesktopSrc = string.Empty;
                if (moduleID > 0)
                    moduleDesktopSrc = Framework.Site.Configuration.ModuleSettings.GetModuleDesktopSrc(moduleID);
                if (moduleDesktopSrc.Length == 0)
                    moduleDesktopSrc = RegisterPage;
                Control myControl = LoadControl(moduleDesktopSrc);

                PortalModuleControl p = ((PortalModuleControl)myControl);

                // changed by Mario Endara <mario@softworks.com.uy> (2004/11/05)
                // if there's no custom register module, take actual ModuleID, else take the custom ModuleID
                if (moduleID == 0)
                {
                    p.ModuleID = ModuleID;
                    ((SettingItem<bool, CheckBox>)p.Settings["MODULESETTINGS_SHOW_TITLE"]).Value = false;
                }
                else
                    p.ModuleID = moduleID;

                return p;
            }

            return (null);
        }

        /// <summary>
        /// Set the module guids with free access to this page
        /// </summary>
        /// <value>The allowed modules.</value>
        protected override List<string> AllowedModules
        {
            get
            {
                var al = new List<string>
                    {
                        "B6A48596-9047-4564-8555-61E3B31D7272", "399D5138-5A1F-4131-9B8C-0AAF0682CFD4" 
                    };
                return al;
            }
        }

        //		string AllowEditUserID
        //		{
        //			get
        //			{
        //				return (Request.Params["AllowEditUserID"] != null) ? "&AllowEditUserID=true" : string.Empty;
        //			}	
        //		}

        /// <summary>
        /// The Save_Click server event handler on this page is used
        /// to save the current security settings to the configuration system
        /// </summary>
        /// <param name="Sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Save_Click(Object Sender, EventArgs e)
        {
            // Persists user data
            var userId = EditControl.SaveUserData();
            // if userId is empty, then something was wrong trying to save the user, so we stay in the page (the control should display the error message)
            if (userId != Guid.Empty)
            {
                // remove cache before redirect
                Context.Cache.Remove(Key.ModuleSettings(ModuleID));

                if (Request.QueryString.GetValues("ModalChangeMaster") != null) {
                    // Close the dialog and reload the page.
                    Response.Write("<script type=\"text/javascript\">window.parent.location = window.parent.location.href;</script>");
                } else
                    // Navigate back to admin page
                    RedirectBackToReferringPage();
                    //Response.Redirect(HttpUrlBuilder.BuildUrl(PageID));
            }
        }

        /// <summary>
        /// The AddRole_Click server event handler is used to add
        /// the user to this security role.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void AddRole_Click(Object sender, EventArgs e)
        {
            Guid roleID;

            //get user id from dropdownlist of existing users
            roleID = new Guid(allRoles.SelectedItem.Value);

            // Add a new userRole to the database
            UsersDB users = new UsersDB();
                   
            users.AddUserRole(roleID, userID, this.PortalSettings.PortalAlias);

            // Rebind list
            BindData();
        }

        /// <summary>
        /// The UserRoles_ItemCommand server event handler on this page
        /// is used to handle deleting the user from roles
        /// from the userRoles asp:datalist control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.DataListCommandEventArgs"/> instance containing the event data.</param>
        private void UserRoles_ItemCommand(object sender, DataListCommandEventArgs e)
        {
            UsersDB users = new UsersDB();
            Guid roleID = (Guid)userRoles.DataKeys[e.Item.ItemIndex];

            // update database
            users.DeleteUserRole(roleID, userID, this.PortalSettings.PortalAlias);

            // Ensure that item is not editable
            userRoles.EditItemIndex = -1;

            // Repopulate list
            BindData();
        }

        /// <summary>
        /// The BindData helper method is used to bind the list of
        /// security roles for this portal to an asp:datalist server control
        /// </summary>
        private void BindData()
        {
            // Bind the Email and Password
            UsersDB users = new UsersDB();

            Guid currentUserID = this.userID;// PortalSettings.CurrentUser.Identity.ProviderUserKey;
            // bind users in role to DataList
            IList<AppleseedRole> roles = new List<AppleseedRole>();
            try {
                roles = users.GetRolesByUser(currentUserID, this.PortalSettings.PortalAlias);
            } catch (Exception exc) {
                ErrorHandler.Publish(LogLevel.Error, exc);
            }
            userRoles.DataKeyField = "Id";
            userRoles.DataSource = roles;
            userRoles.DataBind();

            // bind all portal roles to dropdownlist
            IList<AppleseedRole> allRolesList = users.GetPortalRoles(this.PortalSettings.PortalAlias);


            // remove "All Users", "Authenticated Users" and "Unauthenticated Users" pseudo-roles
            AppleseedRole pseudoRole = new AppleseedRole(AppleseedRoleProvider.AllUsersGuid, AppleseedRoleProvider.AllUsersRoleName);
            
            if (allRolesList.Contains(pseudoRole))
            {
                allRolesList.Remove(pseudoRole);
            }
            pseudoRole = new AppleseedRole(AppleseedRoleProvider.AuthenticatedUsersGuid, AppleseedRoleProvider.AuthenticatedUsersRoleName);
            if (allRolesList.Contains(pseudoRole))
            {
                allRolesList.Remove(pseudoRole);
            }
            pseudoRole = new AppleseedRole(AppleseedRoleProvider.UnauthenticatedUsersGuid, AppleseedRoleProvider.UnauthenticatedUsersRoleName);
            if (allRolesList.Contains(pseudoRole))
            {
                allRolesList.Remove(pseudoRole);
            }

            allRoles.DataSource = allRolesList;
            allRoles.DataBind();
        }

        #region Web Form Designer generated code

        /// <summary>
        /// Raises the Init event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            this.addExisting.Click += new EventHandler(this.AddRole_Click);
            this.userRoles.ItemCommand += new DataListCommandEventHandler(this.UserRoles_ItemCommand);
            this.saveBtn.Click += new EventHandler(this.Save_Click);
            this.Load += new EventHandler(this.Page_Load);
            base.OnInit(e);
        }

        #endregion
    }
}