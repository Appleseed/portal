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
                BindData("DateDeleted", true);
                BindPagesData("DateDeleted", true);
            }
        }

        protected void BindData(string SortField, bool IsLoad = false)
        {
            DataTable dt = RecyclerDB.GetModulesInRecycler(this.PortalSettings.PortalID, SortField);

            DataView dv = new DataView(dt);

            if (GridViewSortDirection == SortDirection.Descending && !IsLoad)
            {
                GridViewSortDirection = SortDirection.Ascending;
                dv.Sort = SortField + ASCENDING;
            }
            else
            {
                GridViewSortDirection = SortDirection.Descending;
                dv.Sort = SortField + DESCENDING;
            }

            DataGrid1.DataSource = dv;
            DataGrid1.DataBind();
        }

        public void BindPagesData(string SortField, bool IsLoad = false)
        {
            DataTable dt1 = RecyclerDB.GetPagesInRecycler(this.PortalSettings.PortalID, SortField);
            DataView dv1 = new DataView(dt1);

            if (GridViewSortDirectionPages == SortDirection.Descending && !IsLoad)
            {
                GridViewSortDirectionPages = SortDirection.Ascending;
                dv1.Sort = SortField + ASCENDING;
            }
            else
            {
                GridViewSortDirectionPages = SortDirection.Descending;
                dv1.Sort = SortField + DESCENDING;
            }

            dtgPages.DataSource = dv1;
            dtgPages.DataBind();
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

            int i = 0;
            foreach (DataGridColumn col in DataGrid1.Columns)
            {
                if (col.SortExpression == e.SortExpression)
                    DataGrid1.Columns[i].HeaderStyle.CssClass =
                      "gridHeaderSort" + (GridViewSortDirection == SortDirection.Ascending ? ASCENDING.Trim() : DESCENDING.Trim());
                else
                    DataGrid1.Columns[i].HeaderStyle.CssClass = "gridHeader";
                i++;
            }
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

        private const string ASCENDING = " ASC";
        private const string DESCENDING = " DESC";

        public SortDirection GridViewSortDirection
        {
            get
            {
                if (ViewState["sortDirection"] == null)
                    ViewState["sortDirection"] = SortDirection.Descending;

                return (SortDirection)ViewState["sortDirection"];
            }
            set { ViewState["sortDirection"] = value; }
        }

        public SortDirection GridViewSortDirectionPages
        {
            get
            {
                if (ViewState["sortDirectionPages"] == null)
                    ViewState["sortDirectionPages"] = SortDirection.Descending;

                return (SortDirection)ViewState["sortDirectionPages"];
            }
            set { ViewState["sortDirectionPages"] = value; }
        }


        #endregion

        protected void dtgPages_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
            string SortField = e.SortExpression.ToString();
            BindPagesData(SortField);

            int i = 0;
            foreach (DataGridColumn col in dtgPages.Columns)
            {
                if (col.SortExpression == e.SortExpression)
                    dtgPages.Columns[i].HeaderStyle.CssClass =
                      "gridHeaderSort" + (GridViewSortDirectionPages == SortDirection.Ascending ? ASCENDING.Trim() : DESCENDING.Trim());
                else
                    dtgPages.Columns[i].HeaderStyle.CssClass = "gridHeader";
                i++;
            }
        }
    }
}