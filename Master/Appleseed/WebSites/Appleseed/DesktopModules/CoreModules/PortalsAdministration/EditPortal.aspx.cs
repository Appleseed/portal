using System;
using System.Collections;
using Appleseed.Framework;
using Appleseed.Framework.Settings.Cache;
using Appleseed.Framework.Site.Configuration;
using Appleseed.Framework.Site.Data;
using Appleseed.Framework.Web.UI;
using Appleseed.Framework.Web.UI.WebControls;

namespace Appleseed.AdminAll
{
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    /// <summary>
    /// EditPortal
    /// </summary>
    public partial class EditPortal : EditItemPage
    {
        private int currentPortalID = -1;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            // Get portalID from querystring
            if (Request.Params["portalID"] != null)
            {
                currentPortalID = Int32.Parse(Request.Params["portalID"]);
            }

            if (currentPortalID != -1)
            {
                // Remove cache for reload settings
                if (!Page.IsPostBack)
                    CurrentCache.Remove(Key.PortalSettings());

                // Obtain PortalSettings of this Portal
                PortalSettings currentPortalSettings = PortalSettings.GetPortalSettings(currentPortalID);

                // If this is the first visit to the page, populate the site data
                if (!Page.IsPostBack)
                {
                    PortalIDField.Text = currentPortalID.ToString();
                    TitleField.Text = currentPortalSettings.PortalName;
                    AliasField.Text = currentPortalSettings.PortalAlias;
                    PathField.Text = currentPortalSettings.PortalPath;
                }
                EditTable.DataSource =
                    new SortedList(
                        PortalSettings.GetPortalCustomSettings(currentPortalSettings.PortalID,
                                                               PortalSettings.GetPortalBaseSettings(null)));
                EditTable.DataBind();
                EditTable.ObjectID = currentPortalID;
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
                al.Add("366C247D-4CFB-451D-A7AE-649C83B05841");
                return al;
            }
        }

        /// <summary>
        /// OnUpdate
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected override void OnUpdate(EventArgs e)
        {
            base.OnUpdate(e);

            if (Page.IsValid)
            {
                //Update main settings and Tab info in the database
                new PortalsDB().UpdatePortalInfo(currentPortalID, TitleField.Text, PathField.Text, false);

                // Update custom settings in the database
                EditTable.ObjectID = currentPortalID;
                EditTable.UpdateControls();

                // Remove cache for reload settings before redirect
                CurrentCache.Remove(Key.PortalSettings());
                // Redirect back to calling page
                RedirectBackToReferringPage();
            }
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            CurrentCache.Remove(Key.PortalSettings());
        }

        /// <summary>
        /// Handles the UpdateControl event of the EditTable control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:Appleseed.Framework.Web.UI.WebControls.SettingsTableEventArgs"/> instance containing the event data.</param>
        private void EditTable_UpdateControl(object sender, SettingsTableEventArgs e)
        {
            SettingsTable edt = (SettingsTable) sender;
            PortalSettings.UpdatePortalSetting(edt.ObjectID, ((ISettingItem)e.CurrentItem).EditControl.ID, ((ISettingItem)e.CurrentItem).Value.ToString());
        }

        #region Web Form Designer generated code

        /// <summary>
        /// Raises the Init event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            this.EditTable.UpdateControl +=
                new Appleseed.Framework.Web.UI.WebControls.UpdateControlEventHandler(this.EditTable_UpdateControl);
            this.Load += new EventHandler(this.Page_Load);

            //Translations
            RequiredTitle.ErrorMessage = General.GetString("VALID_FIELD");

            base.OnInit(e);
        }

        #endregion
    }
}