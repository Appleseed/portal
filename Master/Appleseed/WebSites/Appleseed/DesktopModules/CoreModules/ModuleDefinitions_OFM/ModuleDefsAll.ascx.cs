using System;
using System.Web.UI.WebControls;
using Appleseed.Framework;
using Appleseed.Framework.Data;
using Appleseed.Framework.Web.UI.WebControls;

namespace Appleseed.Content.Web.Modules
{
    /// <summary>
    /// Control to show/edit portals modules (AdminAll)
    /// </summary>
    public partial class ModuleDefsAll_OFM : PortalModuleControl
    {
        /// <summary>
        /// Admin Module
        /// </summary>
        /// <value></value>
        public override bool AdminModule
        {
            get { return true; }
        }

        /// <summary>
        /// The Page_Load server event handler on this user control is used
        /// to populate the current defs settings from the configuration system
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                BindData();
        }

        /// <summary>
        /// The BindData helper method is used to bind the list of
        /// module definitions for this portal to an asp:datalist server control
        /// </summary>
        private void BindData()
        {
            // Get the portal's defs from the database
            string sql =
                "SELECT GeneralModDefID, FriendlyName, DesktopSrc " +
                "FROM rb_GeneralModuleDefinitions " +
                "WHERE AssemblyName = 'Appleseed.Modules.OneFileModule.dll'" +
                "ORDER BY FriendlyName";

            defsList.DataSource = DBHelper.GetDataReader(sql);
            defsList.DataBind();
        }

        /// <summary>
        /// GUID of module (mandatory)
        /// </summary>
        /// <value></value>
        public override Guid GuidID
        {
            get { return new Guid("{D04BB5EA-A792-4E87-BFC7-7D0ED3AD1234}"); }
        }

        #region Web Form Designer generated code

        /// <summary>
        /// Raises OnInit event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            this.defsList.ItemCommand += new DataListCommandEventHandler(this.defsList_ItemCommand);
            this.Load += new EventHandler(this.Page_Load);
            this.AddUrl = "~/DesktopModules/CoreModules/ModuleDefinitions_OFM/ModuleDefinitions.aspx";
            base.OnInit(e);
        }

        #endregion

        /// <summary>
        /// The DefsList_ItemCommand server event handler on this page
        /// is used to handle the user editing module definitions
        /// from the DefsList &lt;asp:datalist&gt; control
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.Web.UI.WebControls.DataListCommandEventArgs"/> instance containing the event data.</param>
        private void defsList_ItemCommand(object source, DataListCommandEventArgs e)
        {
            Guid GeneralModDefID = new Guid(defsList.DataKeys[e.Item.ItemIndex].ToString());

            // Go to edit page
            Response.Redirect(
                HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/ModuleDefinitions_OFM/ModuleDefinitions.aspx", PageID,
                                        "DefID=" + GeneralModDefID + "&Mid=" + ModuleID));
        }
    }
}