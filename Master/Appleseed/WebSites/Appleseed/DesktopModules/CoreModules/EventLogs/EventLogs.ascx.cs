using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Web.UI.WebControls;
using Appleseed.Framework;
using Appleseed.Framework.DataTypes;
using Appleseed.Framework.Settings;
using Appleseed.Framework.Web.UI.WebControls;

namespace Appleseed.Content.Web.Modules
{
    /// <summary>
    /// EventLogs - Windows Event viewer
    /// Written by: Hervé LE ROY (www.hleroy.com)
    /// Moved into Appleseed by Jakob Hansen
    /// </summary>
    public partial class EventLogs : PortalModuleControl
    {
        protected string sortField;
        protected string sortDirection;

        /// <summary>
        /// The Page_Load server event handler is used to initialize the sort column
        /// and populate the list of event logs
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            if (! Page.IsPostBack)
            {
                MachineName.Text = Settings["MachineName"].ToString();
                sortField = Settings["SortField"].ToString();
                sortDirection = Settings["SortDirection"].ToString();
                ViewState["SortField"] = sortField;
                ViewState["sortDirection"] = sortDirection;

                PopulateListOfLogs();
            }
            else
            {
                sortField = (string) ViewState["SortField"];
                sortDirection = (string) ViewState["sortDirection"];
            }
        }


        /// <summary>
        /// The GetEntryTypeImage function is used to get the image path
        /// for the different event log entry types (warning, error, info)
        /// </summary>
        /// <param name="EntryType">Type of the entry.</param>
        /// <returns></returns>
        public string GetEntryTypeImage(EventLogEntryType EntryType)
        {
            string strImage;
            switch (EntryType)
            {
                case EventLogEntryType.Warning:
                    strImage = "EventLogs_Warning.png";
                    break;
                case EventLogEntryType.Error:
                    strImage = "EventLogs_Error.png";
                    break;
                default:
                    strImage = "EventLogs_Info.png";
                    break;
            }
            return Path.WebPathCombine(Path.ApplicationRoot, "DesktopModules/EventLogs", strImage);
        }


        /// <summary>
        /// The PopulateListOfLogs sub is used to fill the LogName drop down list
        /// with the event logs found on the machine (Application, Security, System
        /// are the default logs you may found)
        /// </summary>
        private void PopulateListOfLogs()
        {
            LogName.Items.Clear();
            Message.Text = string.Empty;
            try
            {
                // Browse event logs for machine name
                foreach (EventLog myEventLog in EventLog.GetEventLogs(MachineName.Text))
                {
                    LogName.Items.Add(myEventLog.LogDisplayName);
                } //
                // Populate list of sources
                PopulateListOfSources();
            }
            catch
            {
                // Probably wrong machine name
                Message.Text = "Error while browsing event logs on machine " + MachineName.Text +
                               ". Probably wrong machine name";
                LogName.Items.Clear();
                LogSource.Items.Clear();
            }
        }


        /// <summary>
        /// The PopulateListOfSources sub is used to fill the LogSource drop down list
        /// with the different sources found in the selected event log
        /// </summary>
        private void PopulateListOfSources()
        {
            Message.Text = string.Empty;
            try
            {
                EventLog myEventLog = new EventLog(LogName.SelectedItem.Text, MachineName.Text);
                EventLogEntryCollection myLogEntryCollection = myEventLog.Entries;

                ArrayList mySourceArray = new ArrayList();
                    // Array used to sort strings before populating the drop down list;
                // Browse event entries for different source name
                foreach (EventLogEntry myLogEntry in myLogEntryCollection)
                {
                    if ((mySourceArray.IndexOf(myLogEntry.Source) < 0))
                    {
                        mySourceArray.Add(myLogEntry.Source);
                    }
                } //
                // Sort the source array
                mySourceArray.Sort();
                // Add the source names to the drop down list
                LogSource.Items.Clear();
                LogSource.Items.Add("(all)");
                foreach (string Source in mySourceArray)
                {
                    LogSource.Items.Add(Source);
                } //
                // Bind grid
                BindGrid();
            }
            catch
            {
                // An error as happened. Mostly permissions problems (when accessing security log for example)
                Message.Text = "Error while browsing source entries for " + LogName.SelectedItem.Text +
                               ". Probably insufficient permissions";
                LogSource.Items.Clear();
            }
        }


        /// <summary>
        /// The BindGrid sub is used to bind the event log entries with the data grid
        /// This could be done directly but we chose to use an intermediate data view
        /// filled with the event log entries. This allows us to filter on the source
        /// the entries we pass to the data view and ultimately to sort the dataview
        /// </summary>
        private void BindGrid()
        {
            Message.Text = string.Empty;
            try
            {
                DataTable myDataTable;
                DataRow myDataRow;
                EventLog myEventLog = new EventLog();
                string myEventLogSource;
                myEventLog.MachineName = MachineName.Text;
                myEventLog.Log = LogName.SelectedItem.Text;
                myEventLogSource = LogSource.SelectedItem.Text;

                myDataTable = new DataTable();
                myDataTable.Columns.Add(new DataColumn("EntryType", typeof (EventLogEntryType)));
                myDataTable.Columns.Add(new DataColumn("TimeGenerated", typeof (DateTime)));
                myDataTable.Columns.Add(new DataColumn("Source", typeof (string)));
                myDataTable.Columns.Add(new DataColumn("EventID", typeof (int)));
                myDataTable.Columns.Add(new DataColumn("Message", typeof (string)));
                // Fill the data table with the event log entries
                foreach (EventLogEntry myEventLogEntry in myEventLog.Entries)
                {
                    if ((myEventLogSource == "(all)") || (myEventLogSource == myEventLogEntry.Source))
                    {
                        myDataRow = myDataTable.NewRow();
                        myDataRow[0] = myEventLogEntry.EntryType;
                        myDataRow[1] = myEventLogEntry.TimeGenerated;
                        myDataRow[2] = myEventLogEntry.Source;
                        myDataRow[3] = myEventLogEntry.InstanceId;
                        myDataRow[4] = myEventLogEntry.Message;
                        myDataTable.Rows.Add(myDataRow);
                    }
                } //
                // return a data view of the data table
                DataView myDataView = new DataView(myDataTable);
                // Sort the data view on specified column
                myDataView.Sort = sortField + " " + sortDirection;
                // Bind the data view with the data grid
                LogGrid.DataSource = myDataView;
                LogGrid.DataBind();
            }
            catch
            {
                Message.Text = "Unknown error while binding event log entries to the data grid";
            }
        }


        /// <summary>
        /// Handles the Change event of the MachineName control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        public void MachineName_Change(object sender, EventArgs e) //MachineName.TextChanged 
        {
            PopulateListOfLogs();
        }

        /// <summary>
        /// Handles the Change event of the LogName control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        public void LogName_Change(object sender, EventArgs e) //LogName.SelectedIndexChanged 
        {
            PopulateListOfSources();
        }


        /// <summary>
        /// Handles the Change event of the LogSource control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        public void LogSource_Change(object sender, EventArgs e) //LogSource.SelectedIndexChanged 
        {
            LogGrid.CurrentPageIndex = 0;
            BindGrid();
        }

        /// <summary>
        /// Handles the Change event of the LogGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.Web.UI.WebControls.DataGridPageChangedEventArgs"/> instance containing the event data.</param>
        public void LogGrid_Change(object sender, DataGridPageChangedEventArgs e) // LogGrid.PageIndexChanged 
        {
            LogGrid.CurrentPageIndex = e.NewPageIndex;
            BindGrid();
        }

        /// <summary>
        /// Handles the Sort event of the LogGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.Web.UI.WebControls.DataGridSortCommandEventArgs"/> instance containing the event data.</param>
        public void LogGrid_Sort(object sender, DataGridSortCommandEventArgs e) // LogGrid.SortCommand
        {
            sortField = e.SortExpression;
            BindGrid();
        }

        /// <summary>
        /// Public constructor. Sets base settings for module.
        /// </summary>
        public EventLogs()
        {
            // Modified by Hongwei Shen(hongwei.shen@gmail.com) to group the settings
            // 13/9/2005
            SettingItemGroup group = SettingItemGroup.MODULE_SPECIAL_SETTINGS;
            int groupBase = (int) group;
            // end of modification

            var setMachineName = new SettingItem<string, TextBox>();
            setMachineName.Required = true;
            setMachineName.Value = ".";
            // Modified by Hongwei Shen
            // setMachineName.Order = 1;
            setMachineName.Group = group;
            setMachineName.Order = groupBase + 20;
            setMachineName.EnglishName = "Machine Name";
            // end of modification
            this.BaseSettings.Add("MachineName", setMachineName);

            var setSortField =
                new SettingItem<string, ListControl>(new ListDataType<string, ListControl>("EntryType;TimeGenerated;Source;EventID;Message"));
            setSortField.Required = true;
            setSortField.Value = "TimeGenerated";
            // Modified by Hongwei Shen
            // setSortField.Order = 2;
            setSortField.Group = group;
            setSortField.Order = groupBase + 25;
            setSortField.EnglishName = "Sort Field";
            // end of modification
            this.BaseSettings.Add("SortField", setSortField);

            var setSortDirection = new SettingItem<string, ListControl>(new ListDataType<string, ListControl>("ASC;DESC"));
            setSortDirection.Required = true;
            setSortDirection.Value = "DESC";
            // Modified by Hongwei Shen
            // setSortDirection.Order = 3;
            setSortDirection.Group = group;
            setSortDirection.Order = groupBase + 30;
            setSortDirection.EnglishName = "Sort Direction";
            // end of modification
            this.BaseSettings.Add("SortDirection", setSortDirection);
        }

        /// <summary>
        /// GUID of module (mandatory)
        /// </summary>
        /// <value></value>
        public override Guid GuidID
        {
            get { return new Guid("{2502DB18-B580-4F90-8CB4-C15E6E531051}"); }
        }

        /// <summary>
        /// Admin Module
        /// </summary>
        /// <value></value>
        public override bool AdminModule
        {
            get { return true; }
        }

        #region Web Form Designer generated code

        /// <summary>
        /// Raises OnInit event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(this.Page_Load);
//			ModuleTitle = new DesktopModuleTitle();
//			Controls.AddAt(0, ModuleTitle);
            base.OnInit(e);
        }

        #endregion
    }
}