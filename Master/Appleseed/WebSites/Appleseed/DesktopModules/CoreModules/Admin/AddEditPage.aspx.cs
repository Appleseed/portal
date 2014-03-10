using System;
using System.Web.UI;
using Appleseed.Framework.Security;
using Appleseed.Framework.Settings;
using Appleseed.Framework.Web.UI;
using Appleseed.Framework.Web.UI.WebControls;
using Appleseed.KickStarter.CommonClasses;

namespace Appleseed.Content.Web.Modules
{
    /// <summary>
    /// Summary description for Property Page
    /// </summary>
    public partial class AddEditPage : AddEditItemPage
    {
        protected IEditModule AddEditControl;

        #region Web Form Designer generated code

        /// <summary>
        /// On init
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
            InitializeControl();
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Load += new EventHandler(this.Page_Load);
        }

        /// <summary>
        /// Purpose: Method to initialize the control.
        /// </summary>
        private void InitializeControl()
        {
            if (ViewState["AddEditControl"] == null)
            {
                PortalModuleControl myControl =
                    (PortalModuleControl) this.LoadControl(Path.ApplicationRoot + "/" + this.Module.DesktopSrc);
                ViewState["AddEditControl"] = (IEditModule) this.LoadControl(myControl.AddModuleControl);
                AddEditControlPlaceHolder.Controls.Add((Control) ViewState["AddEditControl"]);
            }
            AddEditControl = (IEditModule) ViewState["AddEditControl"];
            //Attach events
            AddEditControl.DataActionStart += new DataChangeEventHandler(this.EditControl_DataActionStart);
            AddEditControl.DataActionEnd += new DataChangeEventHandler(this.EditControl_DataActionEnd);
            AddEditControl.CancelEdit += new EventHandler(this.EditControl_CancelEdit);
        }

        #endregion

        private void Page_Load(object sender, EventArgs e)
        {
            //Check permissions and enable/disable buttons accordingly
            if (!PortalSecurity.IsInRoles("Admins"))
            {
                AddEditControl.AllowAdd = PortalSecurity.HasAddPermissions(ModuleID);
                AddEditControl.AllowDelete = PortalSecurity.HasDeletePermissions(ModuleID);
                AddEditControl.AllowUpdate = PortalSecurity.HasEditPermissions(ModuleID);
            }

            if (!IsPostBack)
            {
                if (AddEditControl.AllowUpdate && ItemID > 0) //If editing 
                    AddEditControl.StartEdit(ItemID.ToString());
            }
        }

        /// <summary>
        /// Purpose: Method to Handle the EditControl actions events.
        /// </summary>
        /// <param name="sender" type="Appleseed.Content.Web.ModulesWTS.BusinessLayer.Games"></param>
        /// <param name="eventArgs" type="DataChangeEventArgs"></param>
        protected void EditControl_DataActionStart(object sender, DataChangeEventArgs eventArgs)
        {
        }

        /// <summary>
        /// Purpose: Method to Handle the EditControl actions events.
        /// </summary>
        /// <param name="sender" type="Appleseed.Content.Web.ModulesWTS.BusinessLayer.Games"></param>
        /// <param name="eventArgs" type="DataChangeEventArgs"></param>
        protected void EditControl_DataActionEnd(object sender, DataChangeEventArgs eventArgs)
        {
            OnUpdate(null);

            // Redirect back to the portal home page
            this.RedirectBackToReferringPage();
        }

        /// <summary>
        /// Cancel
        /// </summary>
        /// <param name="sender" type="object"></param>
        /// <param name="eventArgs" type="EventArgs"></param>
        protected void EditControl_CancelEdit(object sender, EventArgs eventArgs)
        {
            OnCancel(null);
        }
    }
}