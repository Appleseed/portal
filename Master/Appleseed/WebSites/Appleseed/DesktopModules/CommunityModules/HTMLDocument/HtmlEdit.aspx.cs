// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HtmlEdit.aspx.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The html edit.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.DesktopModules.CommunityModules.HTMLDocument
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Appleseed.Framework;
    using Appleseed.Framework.Content.Data;
    using Appleseed.Framework.DataTypes;
    using Appleseed.Framework.Web.UI;
    using Appleseed.Framework.Web.UI.WebControls;

    using LinkButton = Appleseed.Framework.Web.UI.WebControls.LinkButton;
    using System.Data.SqlClient;
    using Appleseed.Framework.Security;

    /// <summary>
    /// The html edit.
    /// </summary>
    [History("Jes1111", "2003/03/04", "Cache flushing now handled by inherited page")]
    [History("Ashish.patel@haptix.biz", "2014/11/20", "Create new version/published and compare/merge functionality")]
    public partial class HtmlEdit : EditItemPage
    {
        #region Constants and Fields

        /// <summary>
        /// The desktop text.
        /// </summary>
        protected IHtmlEditor DesktopText;

        #endregion

        #region Properties

        /// <summary>
        ///   Set the module guids with free access to this page
        /// </summary>
        /// <value>The allowed modules.</value>
        protected override List<string> AllowedModules
        {
            get
            {
                var al = new List<string> { "0B113F51-FEA3-499A-98E7-7B83C192FDBB" };
                return al;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles OnInit event
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        /// <remarks>
        /// The Page_Load event on this Page is used to obtain the ModuleID
        ///   of the xml module to edit.
        ///   It then uses the Appleseed.HtmlTextDB() data component
        ///   to populate the page's edit controls with the text details.
        /// </remarks>
       [History("Ashish.patel@haptix.biz", "2014/11/20", "Get verson list and fill it into dropdown list")]
        protected override void OnInit(EventArgs e)
        {
            // Controls must be created here
            this.UpdateButton = new LinkButton();
            this.CancelButton = new LinkButton();
            //this.VersionHistory = new LinkButton();

            // Add the setting
            var editor = this.ModuleSettings["Editor"].ToString();
            var width = this.ModuleSettings["Width"].ToString();
            var height = this.ModuleSettings["Height"].ToString();
            var showUpload = this.ModuleSettings["ShowUpload"].ToBoolean(CultureInfo.InvariantCulture);
            var showMobile = this.ModuleSettings["ShowMobile"].ToBoolean(CultureInfo.InvariantCulture);

            var h = new HtmlEditorDataType { Value = editor };
            this.DesktopText = h.GetEditor(
                this.PlaceHolderHTMLEditor,
                this.ModuleID,
                showUpload,
                this.PortalSettings);

            this.DesktopText.Width = new Unit(width);
            this.DesktopText.Height = new Unit(height);
            if (showMobile)
            {
                this.MobileRow.Visible = true;
                this.MobileSummary.Width = new Unit(width);
                this.MobileDetails.Width = new Unit(width);
            }
            else
            {
                this.MobileRow.Visible = false;
            }

            // Construct the page
            // Added css Styles by Mario Endara <mario@softworks.com.uy> (2004/10/26)
            this.UpdateButton.CssClass = "CommandButton";
            this.PlaceHolderButtons.Controls.Add(this.UpdateButton);

            this.PlaceHolderButtons.Controls.Add(new LiteralControl("&#160;"));
            this.CancelButton.CssClass = "CommandButton";
            this.PlaceHolderButtons.Controls.Add(this.CancelButton);

            //Get versionList
            //["Ashish.patel@haptix.biz", "2014/11/20", "Get HtmlText versions by ModuleID"]
            HtmlTextDB versionDB = new HtmlTextDB();
            SqlDataReader drList = versionDB.GetHtmlTextRecord(this.ModuleID);
            ListItem item = new ListItem();
            if (drList.HasRows)
            {
                while (drList.Read())
                {
                    item = new ListItem();
                    if (Convert.ToBoolean(drList["Published"]))
                    {
                        item.Text = drList["VersionNo"].ToString() + " [Published]";
                        item.Value = drList["VersionNo"].ToString();
                        item.Selected = true;
                    }
                    else
                    {
                        item.Text = drList["VersionNo"].ToString();
                        item.Value = drList["VersionNo"].ToString();
                    }
                    drpVirsionList.Items.Add(item);
                }

            }

            if (drpVirsionList.Items.Count == 0)
            {
                item.Text = "1 [Published]";
                item.Value = "1";
                item.Selected = true;
                drpVirsionList.Items.Add(item);
            }

            LoadHTMLText();

            base.OnInit(e);
        }

        /// <summary>
        /// Load html text
        /// </summary>
        [History("Ashish.patel@haptix.biz", "2014/11/20", "Get HtmlText")]
        private void LoadHTMLText()
        {
            // Obtain a single row of text information
            var text = new HtmlTextDB();

            // Change by Geert.Audenaert@Syntegra.Com - Date: 7/2/2003
            // Original: SqlDataReader dr = text.GetHtmlText(ModuleID);
            var dr = text.GetHtmlText(this.ModuleID, WorkFlowVersion.Staging, Convert.ToInt32(drpVirsionList.SelectedItem.Value));

            // End Change Geert.Audenaert@Syntegra.Com
            try
            {
                if (dr.Read())
                {
                    this.DesktopText.Text = this.Server.HtmlDecode((string)dr["DesktopHtml"]);
                    this.MobileSummary.Text = this.Server.HtmlDecode((string)dr["MobileSummary"]);
                    this.MobileDetails.Text = this.Server.HtmlDecode((string)dr["MobileDetails"]);
                }
                else
                {
                    this.DesktopText.Text = General.GetString(
                        "HTMLDOCUMENT_TODO_ADDCONTENT", "Todo: Add Content...", null);
                    this.MobileSummary.Text = General.GetString(
                        "HTMLDOCUMENT_TODO_ADDCONTENT", "Todo: Add Content...", null);
                    this.MobileDetails.Text = General.GetString(
                        "HTMLDOCUMENT_TODO_ADDCONTENT", "Todo: Add Content...", null);
                }
            }
            finally
            {
                dr.Close();
            }
        }

        /// <summary>
        /// The UpdateBtn_Click event handler on this Page is used to save
        ///   the text changes to the database.
        /// </summary>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        [History("Ashish.patel@haptix.biz", "2014/11/20", "Add parameter in UpdateHtmltext")]
        protected override void OnUpdate(EventArgs e)
        {
            base.OnUpdate(e);

            // Create an instance of the HtmlTextDB component
            var text = new HtmlTextDB();

            // Update the text within the HtmlText table
            text.UpdateHtmlText(
                this.ModuleID,
                this.Server.HtmlEncode(this.DesktopText.Text),
                this.Server.HtmlEncode(this.MobileSummary.Text),
                this.Server.HtmlEncode(this.MobileDetails.Text),
                Convert.ToInt32(drpVirsionList.SelectedItem.Value),
               Convert.ToBoolean(drpVirsionList.SelectedItem.Text.Contains("Published") ? 1 : 0),
                DateTime.Now, Appleseed.Framework.Site.Configuration.PortalSettings.CurrentUser.Identity.UserName, DateTime.Now, Appleseed.Framework.Site.Configuration.PortalSettings.CurrentUser.Identity.UserName
                );

            if (Request.QueryString.GetValues("ModalChangeMaster") != null)
                Response.Write("<script type=\"text/javascript\">window.parent.location = window.parent.location.href;</script>");
            else
                this.RedirectBackToReferringPage();
        }

        protected override void OnCancel(EventArgs e)
        {
            base.OnCancel(e);
        }
        #endregion

        /// <summary>
        /// Create new version
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        [History("Ashish.patel@haptix.biz", "2014/11/20", "Creating versions of HtmlText")]
        protected void btnCreateNewVersion_Click(object sender, EventArgs e)
        {
            HtmlTextDB versionDB = new HtmlTextDB();
            int maxVersion = drpVirsionList.Items.Count + 1;

            versionDB.UpdateHtmlText(
                this.ModuleID,
                this.Server.HtmlEncode(this.DesktopText.Text),
                this.Server.HtmlEncode(this.MobileSummary.Text),
                this.Server.HtmlEncode(this.MobileDetails.Text),
                maxVersion,
                 Convert.ToBoolean(0),
                DateTime.Now,
                Appleseed.Framework.Site.Configuration.PortalSettings.CurrentUser.Identity.UserName,
                DateTime.Now,
                Appleseed.Framework.Site.Configuration.PortalSettings.CurrentUser.Identity.UserName
                );
            Response.Redirect(Request.Url.PathAndQuery, true);
        }

        /// <summary>
        /// It will Published selected version
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        [History("Ashish.patel@haptix.biz", "2014/11/20", "Publish Selected HtmlText Version")]
        protected void btnPsVersion_Click(object sender, EventArgs e)
        {
            HtmlTextDB versionDB = new HtmlTextDB();
            int maxVersion = Convert.ToInt32(drpVirsionList.SelectedItem.Value);

            versionDB.UpdateHtmlText(
                this.ModuleID,
                this.Server.HtmlEncode(this.DesktopText.Text),
                this.Server.HtmlEncode(this.MobileSummary.Text),
                this.Server.HtmlEncode(this.MobileDetails.Text),
                maxVersion,
                 Convert.ToBoolean(1),
                DateTime.Now,
                Appleseed.Framework.Site.Configuration.PortalSettings.CurrentUser.Identity.UserName,
                DateTime.Now,
                Appleseed.Framework.Site.Configuration.PortalSettings.CurrentUser.Identity.UserName
                );

            if (Request.QueryString.GetValues("ModalChangeMaster") != null)
                Response.Write("<script type=\"text/javascript\">window.parent.location = window.parent.location.href;</script>");
            else
                this.RedirectBackToReferringPage();
        }

        /// <summary>
        /// Change the blog by selected version
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        [History("Ashish.patel@haptix.biz", "2014/11/20", "HtmlText will be display as per selected version")]
        protected void drpVirsionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadHTMLText();
        }

        /// <summary>
        /// Check HtmlText Version History by ModuleID
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        [History("Ashish.patel@haptix.biz", "2014/11/20", "Display version history of selected module")]
        protected void btnHsVersion_Click(object sender, EventArgs e)
        {
            this.Response.Redirect("/DesktopModules/CommunityModules/HTMLDocument/HtmlVersonHistory.aspx?mID=" + this.ModuleID + "&ModalChangeMaster=", true);
        }
    }
}