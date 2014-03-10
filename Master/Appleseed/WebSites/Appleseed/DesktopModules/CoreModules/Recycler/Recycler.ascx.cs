using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;
using Appleseed.Framework.Site.Data;
using Appleseed.Framework.Web.UI.WebControls;

namespace Appleseed.Content.Web.Modules
{
    /// <summary>
    ///		Summary description for Backlog.
    /// </summary>
    public partial class Recycler : PortalModuleControl
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData("ModuleTitle");
            }
        }

        protected void BindData(string SortField)
        {
            DataTable dt = RecyclerDB.GetModulesInRecycler(this.PortalSettings.PortalID, SortField);
            DataGrid1.DataSource = dt;

            DataGrid1.DataBind();
        }

        /// <summary>
        /// The DeleteModuleButton_Click server event handler on this page is
        /// used to delete a portal module
        /// This method is copied directly from PortalModuleControl (exists in
        /// both places!! ugh.)
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void DeleteModuleButton_Click(Object sender, EventArgs e)
        {
            ModulesDB admin = new ModulesDB();

            //admin.DeleteModule(sender.ToString());
            Response.Write("Sending module is " + sender.ToString());
            // Redirect to the same page to pick up changes
            Page.Response.Redirect(Page.Request.RawUrl);
        }

        /// <summary>
        /// The DataGrid1_SortCommand server event handler on this page is
        /// used to sort data in the datagrid based upon the SortCommand method of the
        /// DataGrid.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.Web.UI.WebControls.DataGridSortCommandEventArgs"/> instance containing the event data.</param>
        private void DataGrid1_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
            string SortField = e.SortExpression.ToString();
            BindData(SortField);
        }

        /// <summary>
        /// Admin Module
        /// </summary>
        /// <value></value>
        public override bool AdminModule
        {
            get { return true; }
        }

        /// <summary>
        /// GUID of module (mandatory)
        /// </summary>
        /// <value></value>
        public override Guid GuidID
        {
            get { return new Guid("{E928F47B-A131-4a33-88D5-D5D6E7A94B36}"); }
        }

        #region Web Form Designer generated code

        /// <summary>
        /// Raises OnInit event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            this.DataGrid1.SortCommand += new DataGridSortCommandEventHandler(this.DataGrid1_SortCommand);
            this.Load += new EventHandler(this.Page_Load);
            base.OnInit(e);
        }

        #endregion

        #region Install / Uninstall Implementation

        /// <summary>
        /// Unknown
        /// </summary>
        /// <param name="stateSaver"></param>
        public override void Install(IDictionary stateSaver)
        {
//			string currentScriptName = Server.MapPath(this.TemplateSourceDirectory + "/Install.sql");
//			ArrayList errors = Appleseed.Framework.Data.DBHelper.ExecuteScript(currentScriptName, true);
//			if (errors.Count > 0)
//			{
//				// Call rollback
//				throw new Exception("Error occurred:" + errors[0].ToString());
//			}
        }

        /// <summary>
        /// Unknown
        /// </summary>
        /// <param name="stateSaver"></param>
        public override void Uninstall(IDictionary stateSaver)
        {
//			string currentScriptName = Server.MapPath(this.TemplateSourceDirectory + "/Uninstall.sql");
//			ArrayList errors = Appleseed.Framework.Data.DBHelper.ExecuteScript(currentScriptName, true);
//			if (errors.Count > 0)
//			{
//				// Call rollback
//				throw new Exception("Error occurred:" + errors[0].ToString());
//			}
        }

        #endregion
    }
}