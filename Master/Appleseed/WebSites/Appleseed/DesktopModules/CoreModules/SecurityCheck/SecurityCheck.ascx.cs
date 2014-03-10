using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI.WebControls;
using Appleseed.Framework;
using Appleseed.Framework.Settings;
using Appleseed.Framework.Web.UI.WebControls;
using ImageButton=Appleseed.Framework.Web.UI.WebControls.ImageButton;
using Appleseed.Framework.Providers.AppleseedRoleProvider;
using System.Collections.Generic;

namespace Appleseed.Content.Web.Modules
{
    public partial class SecurityCheck : PortalModuleControl
    {
        protected ImageButton RoleDeleteBtn;
        protected DataSet dsModules;
        protected DataView myDataView;
        protected string sortField;
        protected string sortDirection;

        /// <summary>
        /// Admin Module
        /// </summary>
        /// <value></value>
        public override bool AdminModule
        {
            get { return true; }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            // If this is the first visit to the page, bind the role data to the DropDownList
            if (Page.IsPostBack == false)
            {
                BindRoles();
                sortField = "PageName";
                sortDirection = "ASC";
                ViewState["SORTFIELD"] = sortField;
                ViewState["SORTDIRECTION"] = sortDirection;
            }
            else
            {
                sortField = (string) ViewState["SORTFIELD"];
                sortDirection = (string) ViewState["SORTDIRECTION"];
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public SecurityCheck()
        {
        }

        /// <summary>
        /// Guid
        /// </summary>
        /// <value></value>
        public override Guid GuidID
        {
            get { return new Guid("{8F74C9C4-543A-48fa-AB73-1C07D219899A}"); }
        }

        #region Web Form Designer generated code

        /// <summary>
        /// Raises the Init event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            this.dgModules.PageIndexChanged += new DataGridPageChangedEventHandler(this.dgModules_PageIndexChanged);
            this.dgModules.SortCommand += new DataGridSortCommandEventHandler(this.dgModules_SortCommand);
            this.Load += new EventHandler(this.Page_Load);
            base.OnInit(e);
        }

        #endregion

        # region Install / Uninstall Implementation

        /// <summary>
        /// Unknown
        /// </summary>
        /// <param name="stateSaver"></param>
        public override void Install(IDictionary stateSaver)
        {
            // Don't do Anything
        }

        /// <summary>
        /// Unknown
        /// </summary>
        /// <param name="stateSaver"></param>
        public override void Uninstall(IDictionary stateSaver)
        {
            // Don't do Anything
        }

        #endregion

        /// <summary>
        /// Handles the Click event of the btnSearch control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            BindData();
            BindGrid();
        }

        /// <summary>
        /// Handles the PageIndexChanged event of the dgModules control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.Web.UI.WebControls.DataGridPageChangedEventArgs"/> instance containing the event data.</param>
        private void dgModules_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            dgModules.CurrentPageIndex = e.NewPageIndex;
            BindData();
            BindGrid();
        }

        /// <summary>
        /// Handles the SortCommand event of the dgModules control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.Web.UI.WebControls.DataGridSortCommandEventArgs"/> instance containing the event data.</param>
        private void dgModules_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
            if (sortField == e.SortExpression && sortDirection == "ASC")
                sortDirection = "DESC";
            else
                sortDirection = "ASC";

            ViewState["SORTFIELD"] = e.SortExpression;
            ViewState["SORTDIRECTION"] = sortDirection;
            BindData();
            BindGrid();
        }

        /// <summary>
        /// This method is used to bind the list of
        /// security roles for this portal to an asp:dropdownlist server control
        /// </summary>
        private void BindRoles()
        {
            // Get the portal's roles from the database
            Appleseed.Framework.Users.Data.UsersDB users = new Appleseed.Framework.Users.Data.UsersDB();

            IList<AppleseedRole> roles = users.GetPortalRoles(this.PortalSettings.PortalAlias);
            ddlRoles.DataSource = roles;
            ddlRoles.DataBind();

            ListItem noAdminItem = new ListItem();
            noAdminItem.Text = General.GetString("SECURITYCHECK_NOADMIN", "No Admin");
            noAdminItem.Value = "-1";
            ddlRoles.Items.Add(noAdminItem);
        }

        /// <summary>
        /// The BindData method on this User Control is used to obtain a DataSet of Modules'security information
        /// from the rb_Modules table
        /// </summary>
        private void BindData()
        {
            StringBuilder select = new StringBuilder(string.Empty, 2048);

            if (ddlRoles.SelectedValue == "-1")
            {
                // Search for all non Admin roles
                select.Append("SELECT ModuleID, TabName, ModuleTitle, FriendlyName, IsAdmin = \n");
                select.Append("Case Admin\n");
                select.Append("When 1 Then 'X'\n");
                select.Append("Else ' '\n");
                select.Append("End,\n");
                select.Append("CanView = \n");
                select.Append("Case AuthorizedViewRoles\n");
                select.Append("When 'Admins' then ' '\n");
                select.Append("When 'Admins;' then ' '\n");
                select.Append("Else 'X'\n");
                select.Append("End,\n");
                select.Append("CanEdit = \n");
                select.Append("Case AuthorizedEditRoles\n");
                select.Append("When 'Admins' then ' '\n");
                select.Append("When 'Admins;' then ' '\n");
                select.Append("Else 'X'\n");
                select.Append("End,\n");
                select.Append("CanAdd = \n");
                select.Append("Case AuthorizedAddRoles\n");
                select.Append("When 'Admins' then ' '\n");
                select.Append("When 'Admins;' then ' '\n");
                select.Append("Else 'X'\n");
                select.Append("End,\n");
                select.Append("CanDelete = \n");
                select.Append("Case AuthorizedDeleteRoles\n");
                select.Append("When 'Admins' then ' '\n");
                select.Append("When 'Admins;' then ' '\n");
                select.Append("Else 'X'\n");
                select.Append("End,\n");
                select.Append("CanProperties = \n");
                select.Append("Case AuthorizedPropertiesRoles\n");
                select.Append("When 'Admins' then ' '\n");
                select.Append("When 'Admins;' then ' '\n");
                select.Append("Else 'X'\n");
                select.Append("End,\n");
                select.Append("CanMove = \n");
                select.Append("Case AuthorizedMoveModuleRoles\n");
                select.Append("When 'Admins' then ' '\n");
                select.Append("When 'Admins;' then ' '\n");
                select.Append("Else 'X'\n");
                select.Append("End,\n");
                select.Append("CanDeleteModule = \n");
                select.Append("Case AuthorizedDeleteModuleRoles\n");
                select.Append("When 'Admins' then ' '\n");
                select.Append("When 'Admins;' then ' '\n");
                select.Append("Else 'X'\n");
                select.Append("End\n");
                select.Append("from rb_Modules\n");
                select.Append("Inner Join rb_Pages on rb_Modules.TabID = rb_Pages.PageID and PortalID=" +
                              PortalID.ToString() + "\n");
                select.Append(
                    "Inner Join rb_ModuleDefinitions on rb_Modules.ModuleDefID = rb_ModuleDefinitions.ModuleDefID\n");
                select.Append(
                    "Inner Join rb_GeneralModuleDefinitions on rb_ModuleDefinitions.GeneralModDefID = rb_GeneralModuleDefinitions.GeneralModDefID\n");
            }
            else
            {
                // Search for a specific Role
                string roleName = ddlRoles.SelectedItem.Text;

                select.Append("SELECT ModuleID, TabName, ModuleTitle, FriendlyName, IsAdmin = \n");
                select.Append("Case Admin\n");
                select.Append("When 1 Then 'X'\n");
                select.Append("Else ' '\n");
                select.Append("End,\n");
                select.Append("CanView = \n");
                select.Append("Case PATINDEX('%" + roleName +
                              "%', Replace(Replace(Replace(AuthorizedViewRoles, 'Authenticated Users', '" +
                              roleName + "'), 'Unauthenticated Users', '" +
                              roleName + "'), 'All Users', '" + roleName + "'))\n");
                select.Append("When 0 then ' '\n");
                select.Append("Else 'X'\n");
                select.Append("End,\n");
                select.Append("CanEdit = \n");
                select.Append("Case PATINDEX('%" + roleName +
                              "%', Replace(Replace(Replace(AuthorizedEditRoles, 'Authenticated Users', '" +
                              roleName + "'), 'Unauthenticated Users', '" +
                              roleName + "'), 'All Users', '" + roleName + "'))\n");
                select.Append("When 0 then ' '\n");
                select.Append("Else 'X'\n");
                select.Append("End,\n");
                select.Append("CanAdd = \n");
                select.Append("Case PATINDEX('%" + roleName +
                              "%', Replace(Replace(Replace(AuthorizedAddRoles, 'Authenticated Users', '" +
                              roleName + "'), 'Unauthenticated Users', '" +
                              roleName + "'), 'All Users', '" + roleName + "'))\n");
                select.Append("When 0 then ' '\n");
                select.Append("Else 'X'\n");
                select.Append("End,\n");
                select.Append("CanDelete = \n");
                select.Append("Case PATINDEX('%" + roleName +
                              "%', Replace(Replace(Replace(AuthorizedDeleteRoles, 'Authenticated Users', '" +
                              roleName + "'), 'Unauthenticated Users', '" +
                              roleName + "'), 'All Users', '" + roleName + "'))\n");
                select.Append("When 0 then ' '\n");
                select.Append("Else 'X'\n");
                select.Append("End,\n");
                select.Append("CanProperties = \n");
                select.Append("Case PATINDEX('%" + roleName +
                              "%', Replace(Replace(Replace(AuthorizedPropertiesRoles, 'Authenticated Users', '" +
                              roleName + "'), 'Unauthenticated Users', '" +
                              roleName + "'), 'All Users', '" + roleName + "'))\n");
                select.Append("When 0 then ' '\n");
                select.Append("Else 'X'\n");
                select.Append("End,\n");
                select.Append("CanMove = \n");
                select.Append("Case PATINDEX('%" + roleName +
                              "%', Replace(Replace(Replace(AuthorizedMoveModuleRoles, 'Authenticated Users', '" +
                              roleName + "'), 'Unauthenticated Users', '" +
                              roleName + "'), 'All Users', '" + roleName + "'))\n");
                select.Append("When 0 then ' '\n");
                select.Append("Else 'X'\n");
                select.Append("End,\n");
                select.Append("CanDeleteModule = \n");
                select.Append("Case PATINDEX('%" + roleName +
                              "%', Replace(Replace(Replace(AuthorizedDeleteModuleRoles, 'Authenticated Users', '" +
                              roleName + "'), 'Unauthenticated Users', '" +
                              roleName + "'), 'All Users', '" + roleName + "'))\n");
                select.Append("When 0 then ' '\n");
                select.Append("Else 'X'\n");
                select.Append("End\n");
                select.Append("from rb_Modules\n");
                select.Append("Inner Join rb_Pages on rb_Modules.TabID = rb_Pages.PageID and PortalID=" +
                              PortalID.ToString() + "\n");
                select.Append(
                    "Inner Join rb_ModuleDefinitions on rb_Modules.ModuleDefID = rb_ModuleDefinitions.ModuleDefID\n");
                select.Append(
                    "Inner Join rb_GeneralModuleDefinitions on rb_ModuleDefinitions.GeneralModDefID = rb_GeneralModuleDefinitions.GeneralModDefID\n");
            }

            if (chkAdmin.Checked)
            {
                select.Append("Where Admin = 1\n");
            }

            select.Append("order by TabName");
            string selectSQL = select.ToString();

            SqlConnection sqlConnection = Config.SqlConnectionString;
            SqlDataAdapter sqlCommand = new SqlDataAdapter(selectSQL, sqlConnection);

            try
            {
                sqlConnection.Open();
                dsModules = new DataSet();
                sqlCommand.Fill(dsModules);
                sqlConnection.Close();
            }
            catch (Exception e)
            {
                Appleseed.Framework.ErrorHandler.Publish(Appleseed.Framework.LogLevel.Error,
                                                       "Error in Search: " + e.ToString() + " " + select.ToString(), e);
                throw new Exception("Error in Search selection.");
            }

            myDataView = new DataView();
            myDataView = dsModules.Tables[0].DefaultView;
        }

        /// <summary>
        /// Binds the grid.
        /// </summary>
        private void BindGrid()
        {
            myDataView.Sort = sortField + " " + sortDirection;
            dgModules.DataSource = myDataView;
            dgModules.DataBind();
        }
    }
}