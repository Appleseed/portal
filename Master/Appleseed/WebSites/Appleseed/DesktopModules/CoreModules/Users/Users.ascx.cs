using System;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Appleseed.Framework.Security;
using Appleseed.Framework.Settings;
using Appleseed.Framework.Users.Data;
using Appleseed.Framework.Web.UI.WebControls;
using History=Appleseed.Framework.History;
using LinkButton=Appleseed.Framework.Web.UI.WebControls.LinkButton;
using Localize=Appleseed.Framework.Web.UI.WebControls.Localize;
using System.Web.Security;

namespace Appleseed.Content.Web.Modules
{
    [History("jminond", "march 2005", "Changes for moving Tab to Page")]
    public partial class Users : PortalModuleControl
    {
        /// <summary>
        /// 
        /// </summary>
        protected Localize Message;

        /// <summary>
        /// 
        /// </summary>
        protected LinkButton addNew;

        /// <summary>
        /// 
        /// </summary>
        protected Localize name;

        /// <summary>
        /// 
        /// </summary>
        protected IEditUserProfile EditControl;

        /// <summary>
        /// Admin Module
        /// </summary>
        /// <value></value>
        public override bool AdminModule
        {
            get { return true; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Users"/> class.
        /// </summary>
        public Users()
        {
            // TODO: Break this class up into user controls, 
            // UserDetails, UserList
        }

        /// <summary>
        /// The Page_Load server event handler on this user control is used
        /// to populate the current roles settings from the configuration system
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            // If this is the first visit to the page, bind the role data to the datalist
            if (Page.IsPostBack == false)
            {
                BindData();
            }
        }

        /// <summary>
        /// The DeleteUser_Click server event handler
        /// is used to delete an user for this portal
        /// </summary>
        protected override void OnDelete()
        {
            base.OnDelete();

            // Rebind list
            BindData();
        }

        //		string AllowEditUserID
        //		{
        //			get
        //			{
        //				return bool.Parse(Settings["AllowEditUserID"].ToString()) ? "&AllowEditUserID=true" : string.Empty;
        //			}	
        //		}


        /// <summary>
        /// The BindData helper method is used to bind the list of
        /// users for this portal to an asp:DropDownList server control
        /// </summary>
        private void BindData()
        {
            // change the message between Windows and Forms authentication
            if (Context.User.Identity.AuthenticationType == "Forms")
            {
                UserDomain.Visible = false;
                UserForm.Visible = true;
            }
            else
            {
                UserDomain.Visible = true;
                UserForm.Visible = false;
            }

            // Get the list of registered users from the database
            allUsers.DataSource = new UsersDB().GetUsers(this.PortalSettings.PortalAlias);
            // bind all portal users to dropdownlist
            allUsers.DataBind();
        }


        /// <summary>
        /// GuidID
        /// </summary>
        /// <value></value>
        public override Guid GuidID
        {
            get { return new Guid("{B6A48596-9047-4564-8555-61E3B31D7272}"); }
        }

        #region Web Form Designer generated code

        /// <summary>
        /// Raises the Init event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            this.allUsers.RowEditing += new GridViewEditEventHandler(allUsers_RowEditing);
            this.allUsers.RowDeleting += new GridViewDeleteEventHandler(allUsers_RowDeleting);
            this.allUsers.PageIndexChanging += new GridViewPageEventHandler( allUsers_PageIndexChanging );
            this.AddUrl = "~/DesktopModules/CoreModules/Users/UsersManage.aspx";
            this.AddText = "ADD_NEW_USER";
            this.Load += new EventHandler(this.Page_Load);
            base.OnInit(e);
        }

        void allUsers_PageIndexChanging( object sender, GridViewPageEventArgs e ) {
            allUsers.PageIndex = e.NewPageIndex;
            BindData();
        }

        /// <summary>
        /// Handles the RowEditing event of the allUsers control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.Web.UI.WebControls.GridViewEditEventArgs"/> instance containing the event data.</param>
        protected void allUsers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // User the sender parameter to retrieve the GridView control
            // that raised the event.
            GridView usersGrid = (GridView) sender;
            GridViewRow row = usersGrid.Rows[e.NewEditIndex];
            HttpContext.Current.Items["userName"] = "";
            string _email = ((HtmlAnchor) row.FindControl("lnkUser")).InnerText.Trim();
            string _userName = Membership.GetUserNameByEmail(_email);
            string redurl = Path.ApplicationRoot + "/DesktopModules/CoreModules/Users/UsersManage.aspx?mid=" + ModuleID +
                            "&username=" + _userName;
            Response.Redirect(redurl);
        }

        /// <summary>
        /// deletex
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.Web.UI.WebControls.GridViewDeleteEventArgs"/> instance containing the event data.</param>
        protected void allUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView usersGrid = (GridView) sender;
            GridViewRow row = usersGrid.Rows[e.RowIndex];
            Guid _userID = new Guid(((Appleseed.Framework.Web.UI.WebControls.LinkButton) row.FindControl("DeleteBtn")).CommandArgument);

            // TODO: Fix this
            UsersDB users = new UsersDB();
            users.DeleteUser(_userID);

            OnDelete();
        }

        public string Builddir(string _email) {
            string _userName = Membership.GetUserNameByEmail(_email);
            string redurl = Path.ApplicationRoot + "/DesktopModules/CoreModules/Users/UsersManage.aspx?mid=" + ModuleID +
                            "&username=" + _userName;


            return redurl;
        }

        #endregion
    }
}