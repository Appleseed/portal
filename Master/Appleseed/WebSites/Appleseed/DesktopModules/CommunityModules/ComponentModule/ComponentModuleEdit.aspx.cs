using System;
using System.Collections;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI.WebControls;
using Appleseed.Framework;
using Appleseed.Framework.Content.Data;
using Appleseed.Framework.DataTypes;
using Appleseed.Framework.Site.Configuration;
using Appleseed.Framework.Web.UI;
using Appleseed.Framework.Web.UI.WebControls;
using History=Appleseed.Framework.History;

namespace Appleseed.Content.Web.Modules
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    [History("jminond", "2006/2/23", "Converted to partial class")]
    public partial class ComponentModuleEdit : AddEditItemPage
    {

        protected IHtmlEditor DesktopText;

        /// <summary>
        /// The Page_Load event on this Page is used to obtain the ModuleID
        /// and ItemID of the event to edit.
        /// It then uses the Appleseed.ComponentModuleDB() data component
        /// to populate the page's edit controls with the control details.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                // Obtain a single row of event information
                ComponentModuleDB comp = new ComponentModuleDB();
                SqlDataReader dr = comp.GetComponentModule(ModuleID);

                try
                {
                    // Read first row from database
                    if (dr.Read())
                    {
                        TitleField.Text = (string) dr["Title"];
                        //ComponentField.Text = (string) dr["Component"];
                        CreatedBy.Text = (string) dr["CreatedByUser"];
                        DesktopText.Text = (string)dr["Component"];
                        CreatedDate.Text = ((DateTime) dr["CreatedDate"]).ToShortDateString();
                        // 15/7/2004 added localization by Mario Endara mario@softworks.com.uy
                        if (CreatedBy.Text == "unknown" || CreatedBy.Text == string.Empty)
                            CreatedBy.Text = General.GetString("UNKNOWN", "unknown");
                    }
                }
                finally
                {
                    dr.Close();
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
                al.Add("2B113F51-FEA3-499A-98E7-7B83C192FDBC");
                return al;
            }
        }

        /// <summary>
        /// The UpdateBtn_Click event handler on this Page is used to either
        /// create or update an event.  It uses the Appleseed.EventsDB()
        /// data component to encapsulate all data functionality.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected override void OnUpdate(EventArgs e)
        {
            base.OnUpdate(e);

            // Only Update if the Entered Data is Valid
            if (Page.IsValid)
            {
                // Create an instance of the Event DB component
                var comp = new ComponentModuleDB();

                comp.UpdateComponentModule(ModuleID, PortalSettings.CurrentUser.Identity.UserName, TitleField.Text,
                                           DesktopText.Text);

                if (Request.QueryString.GetValues("ModalChangeMaster") != null)
                    Response.Write("<script type=\"text/javascript\">window.parent.location = window.parent.location.href;</script>");
                else
                    RedirectBackToReferringPage();
            }
        }

        #region Web Form Designer generated code

        /// <summary>
        /// Raises OnInitEvent
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {

            var editor = ModuleSettings["Editor"].ToString();
            var width = this.ModuleSettings["Width"].ToString();
            var height = this.ModuleSettings["Height"].ToString();
            var showUpload = this.ModuleSettings["ShowUpload"].ToBoolean(CultureInfo.InvariantCulture);

            var h = new HtmlEditorDataType { Value = editor };
            this.DesktopText = h.GetEditor(
                this.PlaceHolderComponentEditor,
                this.ModuleID,
                showUpload,
                this.PortalSettings);

            this.DesktopText.Width = new Unit(width);
            this.DesktopText.Height = new Unit(height);


            //Translate
            // Added EsperantusKeys for Localization 
            // Mario Endara mario@softworks.com.uy june-1-2004 
            RequiredTitle.ErrorMessage = General.GetString("ERROR_VALID_TITLE");
            //RequiredComponent.ErrorMessage = General.GetString("ERROR_VALID_DESCRIPTION");

            this.Load += new EventHandler(this.Page_Load);
            this.UpdateButton.Click += new EventHandler(updateButton_Click);

            base.OnInit(e);
        }

        /// <summary>
        /// Handles the Click event of the updateButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void updateButton_Click(object sender, EventArgs e)
        {
            OnUpdate(e);
        }

        #endregion
    }
}