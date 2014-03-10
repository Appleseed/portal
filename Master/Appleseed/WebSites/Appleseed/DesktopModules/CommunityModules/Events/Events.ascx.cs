// devsolution 2003/6/17: Added items for calendar control
//						must have Devsolution.Portal.dll in /bin for generating of the calendar
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using DevSolution.Portal;
using Appleseed.Framework;
using Appleseed.Framework.Content.Data;
using Appleseed.Framework.Data;
using Appleseed.Framework.DataTypes;
using Appleseed.Framework.Helpers;
using Appleseed.Framework.Web.UI.WebControls;
using History=Appleseed.Framework.History;
// devsolution 2003/6/17: Finish - Added items for calendar control

namespace Appleseed.Content.Web.Modules
{
    using System.Collections.Generic;

    ///	<summary>
    ///	Event list
    ///	</summary>
    [History("Mario Hartmann", "mario@hartmann.net", "1.3", "2003/10/01", "moved to seperate folder")]
    [History("devsolution", "devsolution@yahoo.com", "", "2003/06/17", "Added Calendar capability")]
    [
        History("devsolution", "devsolution@yahoo.com", "", "2003/06/23",
            "Fixed when another control on page has postback, calendar would not show")]
    public partial class Events : PortalModuleControl
    {
        /// <summary>
        /// 
        /// </summary>
        private DataSet dsEventData;

        // devsolution 2003/6/17: Finished - Added items for calendar control

        /// <summary>
        /// The	Page_Load event	handler	on this	User Control is	used to
        /// obtain a DataReader	of event information from the Events
        /// table, and then	databind the results to	a templated	DataList
        /// server control.	 It	uses the Appleseed.EventDB()
        /// data component to encapsulate all data functionality.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            // Obtain the list of events from the Events table
            // and bind	to the DataList	Control

            // devsolution 2003/6/23: Fix for when another control is on
            // page and a postback from that occurs calendar doesn't draw
            // now it doesn't care if postback -- always draw it
            // Test Case: To reproduce add picture module that has to page, page through
            // picture module data and calendar will go away
            int Month = (txtDisplayMonth.Text == string.Empty ? DateTime.Now.Month : int.Parse(txtDisplayMonth.Text));
            int Year = (txtDisplayYear.Text == string.Empty ? DateTime.Now.Year : int.Parse(txtDisplayYear.Text));
            RenderEvents(Month, Year);
            // devsolution 2003/6/23: Fix
        }

        /// <summary>
        /// Public constructor.	Sets base settings for module.
        /// </summary>
        public Events()
        {
            // Set Editor Settings jviladiu@portalservices.net 2004/07/30 (added by Jakob Hansen)
            // Modified by Hongwei Shen 2005/09/24
            SettingItemGroup group = SettingItemGroup.MODULE_SPECIAL_SETTINGS;
            int groupBase = (int) group;
            HtmlEditorDataType.HtmlEditorSettings(this.BaseSettings, group);

            //Indah	Fuldner
            var RepeatDirection = new SettingItem<string, ListControl>(new ListDataType<string,ListControl>("Vertical;Horizontal"));
            RepeatDirection.Group = group; //SettingItemGroup.MODULE_SPECIAL_SETTINGS;
            RepeatDirection.Required = true;
            RepeatDirection.Value = "Vertical";
            RepeatDirection.Order = groupBase + 20; //10;
            this.BaseSettings.Add("RepeatDirectionSetting", RepeatDirection);

            var RepeatColumn = new SettingItem<int, TextBox>();
            RepeatColumn.Group = group; // SettingItemGroup.MODULE_SPECIAL_SETTINGS;
            RepeatColumn.Required = true;
            RepeatColumn.Value = 1;
            RepeatColumn.MinValue = 1;
            RepeatColumn.MaxValue = 10;
            RepeatColumn.Order = groupBase + 25; // 20;
            this.BaseSettings.Add("RepeatColumns", RepeatColumn);

            var showItemBorder = new SettingItem<bool, CheckBox>();
            showItemBorder.Group = group; //SettingItemGroup.MODULE_SPECIAL_SETTINGS;
            showItemBorder.Order = groupBase + 30;
            showItemBorder.Value = false;
            this.BaseSettings.Add("ShowBorder", showItemBorder);
            //End Indah	Fuldner

            var DelayExpire = new SettingItem<int, TextBox>();
            DelayExpire.Group = group; //SettingItemGroup.MODULE_SPECIAL_SETTINGS;
            DelayExpire.Order = groupBase + 35; // 40;
            DelayExpire.Value = 365; // 1	year
            DelayExpire.MinValue = 0;
            DelayExpire.MaxValue = 3650; //10 years
            this.BaseSettings.Add("DelayExpire", DelayExpire);

            // devsolution 2003/6/17: Added items for calendar control
            // Show Calendar -	Show a visual calendar with 
            //					Default is false for backward compatibility
            //					Must edit collection properties and set to true
            //					to show calendar
            var ShowCalendar = new SettingItem<bool, CheckBox>();
            ShowCalendar.Group = group; //SettingItemGroup.MODULE_SPECIAL_SETTINGS;
            ShowCalendar.Order = groupBase + 40; // 50;
            ShowCalendar.Value = false;
            this.BaseSettings.Add("ShowCalendar", ShowCalendar);
            // devsolution 2003/6/17: Finished - Added items for calendar control

            // Change by Geert.Audenaert@Syntegra.Com
            // Date: 27/2/2003
            SupportsWorkflow = true;
            // End Change Geert.Audenaert@Syntegra.Com
        }


        /// <summary>
        /// PreviousMonth_Click is fired when previous is clicked
        /// this function goes back one month by passing in -1 (add -1 month)
        /// to calendar
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void PreviousMonth_Click(object sender, EventArgs e)
        {
            ChangeMonth(-1);
        }

        /// <summary>
        /// Main routine to handle changing the month to display relative
        /// to current display month and render it
        /// </summary>
        /// <param name="AddMonth">number of months to add (positive or negative allowed)</param>
        private void ChangeMonth(int AddMonth)
        {
            int DisplayMonth = int.Parse(txtDisplayMonth.Text);
            int DisplayYear = int.Parse(txtDisplayYear.Text);

            DisplayMonth = DisplayMonth + AddMonth;
            if (DisplayMonth < 1)
            {
                DisplayYear--;
                DisplayMonth = 12;
            }
            else if (DisplayMonth > 12)
            {
                DisplayYear++;
                DisplayMonth = 1;
            }

            txtDisplayMonth.Text = DisplayMonth.ToString();
            txtDisplayYear.Text = DisplayYear.ToString();

            RenderEvents(DisplayMonth, DisplayYear);
        }

        /// <summary>
        /// NextMonth_Click fired when next month link is clicked
        /// Addes 1 month to current month by calling ChangeMonth
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void NextMonth_Click(object sender, EventArgs e)
        {
            ChangeMonth(1);
        }

        /// <summary>
        /// Wrapper to call Devsolution.Portal dll
        /// If all day should not return anything
        /// if time passed and not allday should display in HH:MM AM or HH:MM PM format
        /// </summary>
        /// <param name="AllDay">if set to <c>true</c> [all day].</param>
        /// <param name="StartTime">The start time.</param>
        /// <returns></returns>
        public string DisplayTime(bool AllDay, object StartTime)
        {
            EventCalendar eventcalendar = new EventCalendar();
            return eventcalendar.DisplayTime(AllDay, StartTime);
        }

        // devsolution 2003/6/17: Finished - Added items for calendar control

        /// <summary>
        /// devsolution 2003/6/17:
        /// Change to make a RenderEvents for modularity
        /// Routine to add show calendar logic
        /// And Clean up code as now the calendar next and previous
        /// controls must re-render the display and get the data again
        /// e.g. modularize the code
        /// </summary>
        /// <param name="DisplayMonth">Month to display 1=Jan, 2=Feb, etc</param>
        /// <param name="DisplayYear">Year to display YYYY, 2003 for 2003</param>
        private void RenderEvents(int DisplayMonth, int DisplayYear)
        {
            EventsDB events = new EventsDB();

            myDataList.RepeatDirection = (Settings["RepeatDirectionSetting"].ToString() == "Horizontal"
                                              ? RepeatDirection.Horizontal
                                              : RepeatDirection.Vertical);
            myDataList.RepeatColumns = Int32.Parse(Settings["RepeatColumns"].ToString());

            if (bool.Parse(Settings["ShowBorder"].ToString()))
            {
                //myDataList.BorderWidth=Unit.Pixel(1);
                myDataList.ItemStyle.BorderWidth = Unit.Pixel(1);
            }
            dsEventData = events.GetEvents(ModuleID, Version);
            myDataList.DataSource = dsEventData;
            myDataList.DataBind();

            // devsolution 2003/6/17: Added items for calendar control
            if (bool.Parse(Settings["ShowCalendar"].ToString()))
            {
                CalendarPanel.Visible = true;
                string DisplayDate = string.Empty;
                // devsolution 2003/6/17: Must have Devsolution.Portal.dll in \bin for calendar display functionality
                EventCalendar eventcalendar = new EventCalendar();
                lblCalendar.Text =
                    eventcalendar.GenerateCalendar(ModuleID, DisplayMonth, DisplayYear, out DisplayDate, dsEventData);
                lblDisplayDate.Text = DisplayDate;
            }
            // devsolution 2003/6/17: Finished - Added items for calendar control

            myDataList.DataSource = dsEventData;
            myDataList.DataBind();
        }

        #region General Implementation

        /// <summary>
        /// GUID of module (mandatory)
        /// </summary>
        /// <value></value>
        /// <remarks>
        /// </remarks>
        public override Guid GuidID
        {
            get { return new Guid("{EF9B29C5-E481-49A6-9383-8ED3AB42DDA0}"); }
        }

        #region Search Implementation

        /// <summary>
        /// Searchable module
        /// </summary>
        /// <value></value>
        public override bool Searchable
        {
            get { return true; }
        }

        /// <summary>
        /// Searchable module implementation
        /// </summary>
        /// <param name="portalID">The portal ID</param>
        /// <param name="userID">ID	of the user	is searching</param>
        /// <param name="searchString">The text	to search</param>
        /// <param name="searchField">The fields where perfoming the search</param>
        /// <returns>
        /// The SELECT	sql	to perform a search	on the current module
        /// </returns>
        public override string SearchSqlSelect(int portalID, int userID, string searchString, string searchField)
        {
            SearchDefinition s =
                new SearchDefinition("rb_Events", "Title", "Description", "CreatedByUser", "CreatedDate", searchField);

            //Add extra	search fields here,	this way
            s.ArrSearchFields.Add("itm.WhereWhen");

            return s.SearchSqlSelect(portalID, userID, searchString);
        }

        #endregion

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

        # endregion

        #endregion

        #region	Web	Form Designer generated	code

        /// <summary>
        /// Raises Init	event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            // devsolution 2003/6/17: Added items for calendar control
            PreviousMonth.Click += new EventHandler(PreviousMonth_Click);
            NextMonth.Click += new EventHandler(NextMonth_Click);
            // devsolution 2003/6/17: Finish - Added items for calendar control
            Load += new EventHandler(Page_Load);

            // Create a	new	Title the control
//			ModuleTitle	= new DesktopModuleTitle();
            // Set here	title properties
            // Add support for the edit	page
            AddText = "EVENTS_ADD";
            AddUrl = "~/DesktopModules/CommunityModules/Events/EventsEdit.aspx";
            // Add title ad	the	very beginning of 
            // the control's controls collection
//			Controls.AddAt(0, ModuleTitle);

            // Mark	Brown: Added for Calendar
            if (txtDisplayMonth.Text.Length == 0)
            {
                txtDisplayMonth.Text = DateTime.Now.Month.ToString();
                txtDisplayYear.Text = DateTime.Now.Year.ToString();
            }
            // Mark	Brown: End = Added for Calendar

            base.OnInit(e);
        }

        #endregion
    }
}