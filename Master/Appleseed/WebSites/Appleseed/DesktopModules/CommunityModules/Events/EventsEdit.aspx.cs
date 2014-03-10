using System;
using System.Collections;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using Appleseed.Framework;
using Appleseed.Framework.Content.Data;
using Appleseed.Framework.DataTypes;
using Appleseed.Framework.Site.Configuration;
using Appleseed.Framework.Web.UI;
using Appleseed.Framework.Web.UI.WebControls;

namespace Appleseed.Content.Web.Modules
{
    using System.Collections.Generic;

    ///	<summary>
    ///	Edit events
    ///	</summary>
    [History("devsolution", "2003/6/17", "Added items for calendar control")]
    [History("Jes1111", "2003/03/04", "Cache flushing now handled by inherited page")]
    [History("devsolution", "devsolution@yahoo.com", "", "2003/06/17", "Added Calendar capability")]
    public partial class EventsEdit : AddEditItemPage
    {
        #region Declarations

        /// <summary>
        /// 
        /// </summary>
        protected IHtmlEditor DescriptionField;

        #endregion

        /// <summary>
        /// The	Page_Load event	on this	Page is	used to	obtain the ModuleID
        /// and	ItemID of the event	to edit.
        /// It then	uses the Appleseed.EventsDB()	data component
        /// to populate	the	page's edit	controls with the event	details.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            // Added EsperantusKeys for Localization 
            // Mario Endara mario@softworks.com.uy 11/05/2004 

            foreach (ListItem item in AllDay.Items)
            {
                switch (AllDay.Items.IndexOf(item))
                {
                    case 0:
                        item.Text = General.GetString("EVENTS_ALLDAY");
                        break;
                    case 1:
                        item.Text = General.GetString("EVENTS_STARTAT");
                        break;
                }
            }

            //Change Indah Fuldner indah@die-seitenweber.de
            HtmlEditorDataType h = new HtmlEditorDataType();
            h.Value = this.ModuleSettings["Editor"].ToString();
            DescriptionField =
                h.GetEditor(PlaceHolderHTMLEditor, ModuleID, bool.Parse(this.ModuleSettings["ShowUpload"].ToString()),
                            this.PortalSettings);

            DescriptionField.Width = new Unit(this.ModuleSettings["Width"].ToString());
            DescriptionField.Height = new Unit(this.ModuleSettings["Height"].ToString());
            //End Change Indah Fuldner indah@die-seitenweber.de

            // If the page is being	requested the first	time, determine	if an
            // event itemID	value is specified,	and	if so populate page
            // contents	with the event details

            if (Page.IsPostBack == false)
            {
                if (ItemID != 0)
                {
                    // Obtain a	single row of event	information
                    EventsDB events = new EventsDB();
                    SqlDataReader dr = events.GetSingleEvent(ItemID, WorkFlowVersion.Staging);

                    try
                    {
                        // Read	first row from database
                        if (dr.Read())
                        {
                            TitleField.Text = (string) dr["Title"];
                            DescriptionField.Text = (string) dr["Description"];

                            // devsolution 2003/6/17: Added items for calendar control
                            if ((bool) dr["AllDay"])
                            {
                                AllDay.SelectedIndex = 0;
                            }
                            else
                            {
                                int hour = 0;
                                int minute = 0;

                                AllDay.SelectedIndex = 1;
                                StartHour.Enabled = StartMinute.Enabled = StartAMPM.Enabled = true;

                                string[] TimeParts = dr["StartTime"].ToString().Split(new Char[] {':'});

                                try
                                {
                                    if (TimeParts[0].Length > 0) hour = int.Parse(TimeParts[0]);
                                    if (TimeParts.Length > 1) minute = int.Parse(TimeParts[1]);
                                }
                                catch
                                {
                                }

                                if (hour > 11)
                                {
                                    StartAMPM.SelectedIndex = 1;
                                    if (hour > 12) hour -= 12;
                                }
                                else
                                {
                                    if (hour == 0) hour = 12;
                                    StartAMPM.SelectedIndex = 0;
                                }

                                StartHour.SelectedIndex = hour - 1;
                                StartMinute.SelectedIndex = minute/5;
                            }
                            if (dr["StartDate"] != DBNull.Value)
                                StartDate.Text = ((DateTime) dr["StartDate"]).ToShortDateString();
                            else
                                StartDate.Text = string.Empty;
                            // devsolution 2003/6/17: Finished - Added items for calendar control

                            ExpireField.Text = ((DateTime) dr["ExpireDate"]).ToShortDateString();
                            CreatedBy.Text = (string) dr["CreatedByUser"];
                            WhereWhenField.Text = (string) dr["WhereWhen"];
                            CreatedDate.Text = ((DateTime) dr["CreatedDate"]).ToShortDateString();
                            // 15/7/2004 added localization by Mario Endara mario@softworks.com.uy
                            if (CreatedBy.Text == "unknown")
                            {
                                CreatedBy.Text = General.GetString("UNKNOWN", "unknown");
                            }
                        }
                    }
                    finally
                    {
                        dr.Close();
                    }
                }
                else
                {
                    ExpireField.Text =
                        DateTime.Now.AddDays(Int32.Parse(this.ModuleSettings["DelayExpire"].ToString())).ToShortDateString();
                    this.DeleteButton.Visible = false; // Cannot	delete an unexsistent item
                }
            }
        }

        /// <summary>
        /// Set the module guids with free access to this page
        /// </summary>
        /// <value>The allowed modules.</value>
        protected override List<string> AllowedModules
        {
            get
            {
                List<string> al = new List<string>();
                al.Add("EF9B29C5-E481-49A6-9383-8ED3AB42DDA0");
                return al;
            }
        }

        /// <summary>
        /// DevSolution 2003/6/17
        /// AllDay_SelectedIndexChanged fired when AllDay or Specific Time selected
        /// so that the appropriate combo boxes can be disabled for useability
        /// </summary>
        /// <param name="sender">Who is sending the request</param>
        /// <param name="e">Standard EventArgs</param>
        private void AllDay_SelectedIndexChanged(object sender, EventArgs e)
        {
            StartMinute.Enabled = StartAMPM.Enabled = StartHour.Enabled = (AllDay.SelectedItem.Value == "0");
        }

        /// <summary>
        /// The	UpdateBtn_Click	event handler on this Page is used to either
        /// create or update an	event.	It uses	the	Appleseed.EventsDB()
        /// data component to encapsulate all data functionality.
        /// </summary>
        /// <param name="e">Standard EventArgs</param>
        protected override void OnUpdate(EventArgs e)
        {
            base.OnUpdate(e);

            // Only	Update if the Entered Data is Valid
            if (Page.IsValid == true)
            {
                // Create an instance of the Event DB component
                EventsDB events = new EventsDB();

                // devsolution 2003/6/17: Added items for calendar control
                string StartTime = string.Empty;
                bool IsAllDay = (AllDay.SelectedItem.Value == "1");

                if (IsAllDay)
                {
                    int hour = int.Parse(StartHour.SelectedItem.Text);
                    int minute = int.Parse(StartMinute.SelectedItem.Text);

                    if (StartAMPM.SelectedItem.Value == "PM")
                    {
                        if (hour < 12) hour += 12;
                    }
                    else
                    {
                        if (hour == 12) hour -= 12;
                    }
                    StartTime = string.Format("{0:00}:{1:00}:00", hour, minute);
                }
                // devsolution 2003/6/17: Finished - Added items for calendar control

                if (ItemID == 0)
                {
                    // Add the event within	the	Events table
                    events.AddEvent(ModuleID, ItemID, PortalSettings.CurrentUser.Identity.UserName, TitleField.Text,
                                    DateTime.Parse(ExpireField.Text), DescriptionField.Text, WhereWhenField.Text,
                                    IsAllDay, StartDate.Text, StartTime);
                }
                else
                {
                    // Update the event	within the Events table
                    events.UpdateEvent(ModuleID, ItemID, PortalSettings.CurrentUser.Identity.UserName, TitleField.Text,
                                       DateTime.Parse(ExpireField.Text), DescriptionField.Text, WhereWhenField.Text,
                                       IsAllDay, StartDate.Text, StartTime);
                }

                // Redirect	back to	the	portal home	page
                RedirectBackToReferringPage();
            }
        }

        /// <summary>
        /// The	DeleteBtn_Click	event handler on this Page is used to delete an
        /// an event.  It  uses	the	Appleseed.EventsDB() data	component to
        /// encapsulate	all	data functionality.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected override void OnDelete(EventArgs e)
        {
            base.OnDelete(e);

            // Only	attempt	to delete the item if it is	an existing	item
            // (new	items will have	"ItemID" of	0)

            if (ItemID != 0)
            {
                EventsDB events = new EventsDB();
                events.DeleteEvent(ItemID);
            }

            // Redirect	back to	the	portal home	page
            RedirectBackToReferringPage();
        }

        #region	Web	Form Designer generated	code

        /// <summary>
        /// Raises OnInitEvent
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            //Translate
            RequiredTitle.ErrorMessage = General.GetString("EVENTS_VALID_TITLE");
            // RequiredDescription.ErrorMessage = Esperantus.General.GetString("EVENTS_VALID_DESCRIPTION");
            RequiredWhereWhen.ErrorMessage = General.GetString("EVENTS_VALID_WHERE-WHEN");
            RequiredExpireDate.ErrorMessage = General.GetString("EVENTS_VALID_EXPIRE");
            VerifyExpireDate.ErrorMessage = General.GetString("EVENTS_VALID_EXPIRE");

            AllDay.SelectedIndexChanged += new EventHandler(AllDay_SelectedIndexChanged);
            Load += new EventHandler(Page_Load);

            base.OnInit(e);
        }

        #endregion
    }
}