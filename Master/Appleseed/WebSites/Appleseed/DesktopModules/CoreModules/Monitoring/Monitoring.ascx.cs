using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI.WebControls;
using Appleseed.Framework;
using Appleseed.Framework.Data;
using Appleseed.Framework.DataTypes;
using Appleseed.Framework.Monitoring;
using Appleseed.Framework.Scheduler;
using Appleseed.Framework.Settings;
using Appleseed.Framework.Web.UI.WebControls;
using Label=Appleseed.Framework.Web.UI.WebControls.Label;
using Path=System.IO.Path;

namespace Appleseed.Content.Web.Modules
{
    using System.Collections.Generic;

    /// <summary>
    /// Appleseed Monitoring Module - Shows website usage stats
    /// Written by: Paul Yarrow, paul@paulyarrow.com
    /// </summary>
    public partial class Monitoring : PortalModuleControl, ISchedulable
    {
        protected DataView myDataView;
        protected string sortField;
        protected string sortDirection;

        protected CheckBox CheckBoxIncludeAdmin;
        protected Label lblMessage;
        protected SqlCommand sqlComm1;
        protected SqlDataAdapter sqlDA1;


        /// <summary>
        /// Initial Revision by Paul Yarrow, paul@paulyarrow.com, 2003-07-13
        /// </summary>
        public Monitoring()
        {
            // modified by Hongwei Shen
            SettingItemGroup group = SettingItemGroup.MODULE_SPECIAL_SETTINGS;
            int groupBase = (int) group;

            var setSortField =
                new SettingItem<string, ListControl>(
                    new ListDataType<string, ListControl>("ActivityTime;ActivityType;Name;PortalName;TabName;UserHostAddress;UserAgent"))
                    { Required = true, Value = "ActivityTime", Group = group, Order = groupBase + 20 };

            // 1;
            this.BaseSettings.Add("SortField", setSortField);
        }

        /// <summary>
        /// The Page_Load event handler on this User Control is used to
        /// determine sort field and order, and then databind the required
        /// monitoring table rows, generating a graph if necessary.
        /// Initial Revision by Paul Yarrow, paul@paulyarrow.com, 2003-07-13
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void Monitoring_Load(object sender, EventArgs e)
        {
            if (Config.EnableMonitoring)
            {
                // Set the variables correctly depending
                // on whether this is the first page view
                if (Page.IsPostBack == false)
                {
                    sortField = Settings["SortField"].ToString();
                    sortDirection = "ASC";
                    if (sortField == "ActivityTime")
                    {
                        sortDirection = "DESC";
                    }
                    ViewState["SortField"] = sortField;
                    ViewState["sortDirection"] = sortDirection;

                    txtStartDate.Text = DateTime.Now.AddDays(-6).ToShortDateString();
                    txtEndDate.Text = DateTime.Now.ToShortDateString();
                }
                else
                {
                    sortField = (string) ViewState["SortField"];
                    sortDirection = (string) ViewState["sortDirection"];
                }

                BindGrid();

                MonitoringPanel.Visible = true;
                ErrorLabel.Visible = false;
            }
            else
            {
                ErrorLabel.Text = "Monitoring is disabled. Put EnableMonitoring to true in web.config.";
                MonitoringPanel.Visible = false;
                ErrorLabel.Visible = true;
            }
        }

        /// <summary>
        /// The SortTasks event handler sorts the monitoring list (a DataGrid control)
        /// Initial Revision by Paul Yarrow, paul@paulyarrow.com, 2003-07-13
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="e">The <see cref="T:System.Web.UI.WebControls.DataGridSortCommandEventArgs"/> instance containing the event data.</param>
        protected void SortTasks(Object source, DataGridSortCommandEventArgs e)
        {
            if (sortField == e.SortExpression)
            {
                if (sortDirection == "ASC")
                {
                    sortDirection = "DESC";
                }
                else
                {
                    sortDirection = "ASC";
                }
            }
            else
            {
                if (e.SortExpression == "DueDate")
                {
                    sortDirection = "DESC";
                }
            }

            ViewState["SortField"] = e.SortExpression;
            ViewState["sortDirection"] = sortDirection;

            BindGrid();
        }

        /// <summary>
        /// Initial Revision by Paul Yarrow, paul@paulyarrow.com, 2003-07-13
        /// </summary>
        protected void BindGrid()
        {
            if (txtStartDate.Text.Length > 0 &&
                txtEndDate.Text.Length > 0)
            {
                // Read in the data regardless
                //MonitoringDB monitorDB = new MonitoringDB();

                DateTime startDate = DateTime.Parse(txtStartDate.Text);
                DateTime endDate = DateTime.Parse(txtEndDate.Text);

                bool showChart = true;
                string chartType = string.Empty;

                switch (cboReportType.SelectedItem.Value)
                {
                    case "Detailed Site Log":
                        sortField = "ActivityTime";
                        sortDirection = "DESC";
                        ViewState["SortField"] = sortField;
                        ViewState["sortDirection"] = sortDirection;
                        showChart = false;
                        break;

                    case "Page Popularity":
                        sortField = "Requests";
                        sortDirection = "DESC";
                        ViewState["SortField"] = sortField;
                        ViewState["sortDirection"] = sortDirection;
                        chartType = "pie";
                        break;

                    case "Most Active Users":
                        sortField = "Actions";
                        sortDirection = "DESC";
                        ViewState["SortField"] = sortField;
                        ViewState["sortDirection"] = sortDirection;
                        chartType = "pie";
                        break;

                    case "Page Views By Day":
                        sortField = "[Date]";
                        sortDirection = "ASC";
                        ViewState["SortField"] = sortField;
                        ViewState["sortDirection"] = sortDirection;
                        chartType = "bar";
                        break;

                    case "Page Views By Browser Type":
                        sortField = "[Views]";
                        sortDirection = "DESC";
                        ViewState["SortField"] = sortField;
                        ViewState["sortDirection"] = sortDirection;
                        chartType = "pie";
                        break;
                }

                DataSet monitorData = Utility.GetMonitoringStats(startDate,
                                                                 endDate,
                                                                 cboReportType.SelectedItem.Value,
                                                                 this.PortalSettings.ActivePage.PageID,
                                                                 CheckBoxIncludeMonitorPage.Checked,
                                                                 CheckBoxPageRequests.Checked,
                                                                 CheckBoxLogons.Checked,
                                                                 CheckBoxLogouts.Checked,
                                                                 CheckBoxIncludeMyIPAddress.Checked,
                                                                 this.PortalSettings.PortalID);
                myDataView = monitorData.Tables[0].DefaultView;
                myDataView.Sort = sortField + " " + sortDirection;
                myDataGrid.DataSource = myDataView;
                myDataGrid.DataBind();

                if (monitorData.Tables[0].Rows.Count > 0)
                {
                    myDataGrid.Visible = true;
                    LabelNoData.Visible = false;
                }
                else
                {
                    myDataGrid.Visible = false;
                    LabelNoData.Visible = true;
                }

                if (showChart)
                {
                    StringBuilder xValues = new StringBuilder();
                    StringBuilder yValues = new StringBuilder();

                    foreach (DataRow dr in monitorData.Tables[0].Rows)
                    {
                        xValues.Append(dr[0]);
                        yValues.Append(dr[1]);
                        xValues.Append("|");
                        yValues.Append("|");
                    }

                    if (xValues.Length > 0 && yValues.Length > 0)
                    {
                        xValues.Remove(xValues.Length - 1, 1);
                        yValues.Remove(yValues.Length - 1, 1);

                        ChartImage.ImageUrl =
                            HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/Monitoring/ChartGenerator.aspx?" +
                                                    "xValues=" + xValues.ToString() +
                                                    "&yValues=" + yValues.ToString() +
                                                    "&ChartType=" + chartType);

                        ChartImage.Visible = true;
                    }
                    else
                    {
                        ChartImage.Visible = false;
                    }
                }
                else
                {
                    ChartImage.Visible = false;
                }
            }
        }

        /// <summary>
        /// Initial Revision by Paul Yarrow, paul@paulyarrow.com, 2003-07-13
        /// </summary>
        /// <value></value>
        public override Guid GuidID
        {
            get { return new Guid("{3B8E3585-58B7-4f56-8AB6-C04A2BFA6589}"); }
        }

        /// <summary>
        /// Admin Module
        /// </summary>
        /// <value></value>
        public override bool AdminModule
        {
            get { return true; }
        }

        # region Install / Uninstall Implementation

        /// <summary>
        /// Unknown
        /// </summary>
        /// <param name="stateSaver"></param>
        public override void Install(IDictionary stateSaver)
        {
            string currentScriptName = Path.Combine(Server.MapPath(TemplateSourceDirectory), "install.sql");
            List<string> errors = DBHelper.ExecuteScript(currentScriptName, true);
            if (errors.Count > 0)
            {
                // Call rollback
                throw new Exception("Error occurred:" + errors[0].ToString());
            }
        }

        /// <summary>
        /// Unknown
        /// </summary>
        /// <param name="stateSaver"></param>
        public override void Uninstall(IDictionary stateSaver)
        {
            string currentScriptName = Path.Combine(Server.MapPath(TemplateSourceDirectory), "uninstall.sql");
            List<string> errors = DBHelper.ExecuteScript(currentScriptName, true);
            if (errors.Count > 0)
            {
                // Call rollback
                throw new Exception("Error occurred:" + errors[0].ToString());
            }
        }

        #endregion

        #region Web Form Designer generated code

        /// <summary>
        /// Raises Init event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            this.cboReportType.SelectedIndexChanged += new EventHandler(this.cboReportType_SelectedIndexChanged);
            this.cmdDisplay.Click += new EventHandler(this.cmdDisplay_Click);
            this.Load += new EventHandler(this.Monitoring_Load);
            base.OnInit(e);
        }

        #endregion

        /// <summary>
        /// Handles the Click event of the cmdDisplay control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void cmdDisplay_Click(object sender, EventArgs e)
        {
            if (Page.IsValid == true)
            {
                BindGrid();
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the cboReportType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void cboReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// Called after ScheduleDo if it doesn't throw any exception
        /// </summary>
        /// <param name="task"></param>
        public void ScheduleCommit(SchedulerTask task)
        {
            // TODO:  Add Monitoring.ScheduleCommit implementation
        }

        /// <summary>
        /// Called when a task occurs
        /// </summary>
        /// <param name="task"></param>
        public void ScheduleDo(SchedulerTask task)
        {
            // TODO:  Add Monitoring.ScheduleDo implementation
        }

        /// <summary>
        /// Called after ScheduleDo if it throws an exception
        /// </summary>
        /// <param name="task"></param>
        public void ScheduleRollback(SchedulerTask task)
        {
            // TODO:  Add Monitoring.ScheduleRollback implementation
        }
    }
}