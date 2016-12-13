// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HtmlEdit.aspx.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The html edit.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.DesktopModules.CoreModules.HTMLDocument
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
    using System.Web.Services;
    using System.IO;
    using System.Web.Hosting;

    /// <summary>
    /// The html edit.
    /// </summary>
    [History("Jes1111", "2003/03/04", "Cache flushing now handled by inherited page")]
    public partial class HtmlEdit : EditItemPage
    {
        #region Constants and Fields

        /// <summary>
        /// The desktop text.
        /// </summary>
        protected IHtmlEditor DesktopText;

        public bool IsCodeWriter { get { return this.ModuleSettings["Editor"].ToString().ToLower() == "codewriter"; } }
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


        private string CustomTheme = "CustomTheme";
        private string CustomThemeAlt = "CustomThemeAlt";

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
        protected override void OnInit(EventArgs e)
        {
            // Controls must be created here
            this.UpdateButton = new LinkButton();
            this.CancelButton = new LinkButton();

            // Add the setting
            var editor = this.ModuleSettings["Editor"].ToString();
            var width = this.ModuleSettings["Width"].ToString();
            var height = this.ModuleSettings["Height"].ToString();
            var showUpload = this.ModuleSettings["ShowUpload"].ToBoolean(CultureInfo.InvariantCulture);
            var showMobile = this.ModuleSettings["ShowMobile"].ToBoolean(CultureInfo.InvariantCulture);

            if (this.IsCodeWriter && !Request.Url.PathAndQuery.ToLower().Contains("modalchangemaster"))
                Response.Redirect(Request.Url.PathAndQuery + "&ModalChangeMaster=true", true);

            plcCodewriter.Visible = editor.ToLower() == "codewriter";
            plcNoCodeWriter.Visible = editor.ToLower() != "codewriter";

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
            //Added by Ashish - Connection pool Issue
            if (drList != null)
            {
                drList.Close();
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

        private void LoadHTMLText()
        {

            // Obtain a single row of text information
            var text = new HtmlTextDB();

            // Change by Geert.Audenaert@Syntegra.Com - Date: 7/2/2003
            // Original: SqlDataReader dr = text.GetHtmlText(ModuleID);
            var dr = text.GetHtmlText(this.ModuleID, WorkFlowVersion.Staging, Convert.ToInt32(drpVirsionList.SelectedItem.Value));

            this.hdnModuleId.Value = this.ModuleID.ToString();
            this.hdnPageId.Value = this.PageID.ToString();
            this.hdnDefaultJSCSS.Value = $"<link type='text/css' rel='stylesheet' href='/Design/Themes/{this.GetCurrentTheme()}/default.css'></link><script src='/aspnet_client/jQuery/jquery-1.8.3.js'></script>";

            // End Change Geert.Audenaert@Syntegra.Com
            try
            {
                if (dr.Read())
                {
                    if (this.IsCodeWriter)
                    {
                        this.cwCSS.InnerText = this.Server.HtmlDecode((string)dr["CWCSS"]);
                        this.cwHTML.InnerText = this.Server.HtmlDecode((string)dr["CWHTML"]);
                        this.cwJS.InnerText = this.Server.HtmlDecode((string)dr["CWJS"]);
                        this.cwJSCSSRef.InnerText = this.Server.HtmlDecode((string)dr["CWJSCSSREF"]);
                    }
                    else
                    {
                        this.DesktopText.Text = this.Server.HtmlDecode((string)dr["DesktopHtml"]);
                        this.MobileSummary.Text = this.Server.HtmlDecode((string)dr["MobileSummary"]);
                        this.MobileDetails.Text = this.Server.HtmlDecode((string)dr["MobileDetails"]);
                    }
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
        protected override void OnUpdate(EventArgs e)
        {
            base.OnUpdate(e);

            // Create an instance of the HtmlTextDB component
            var text = new HtmlTextDB();
            if (this.IsCodeWriter)
            {
                this.DesktopText.Text = string.Format("<style type='text/css'>{0}</style>{1}<script type='text/javascript'>{2}</script>", this.cwCSS.InnerText, this.cwHTML.InnerText, this.cwJS.InnerText);
            }

            // Update the text within the HtmlText table
            text.UpdateHtmlText(
                this.ModuleID,
                this.Server.HtmlEncode(this.DesktopText.Text),
                this.Server.HtmlEncode(this.MobileSummary.Text),
                this.Server.HtmlEncode(this.MobileDetails.Text),
                Convert.ToInt32(drpVirsionList.SelectedItem.Value),
                Convert.ToBoolean(drpVirsionList.SelectedItem.Text.Contains("Published") ? 1 : 0),
                DateTime.Now, Appleseed.Framework.Site.Configuration.PortalSettings.CurrentUser.Identity.UserName, DateTime.Now, Appleseed.Framework.Site.Configuration.PortalSettings.CurrentUser.Identity.UserName,
                this.cwCSS.InnerText, this.cwJS.InnerText, this.cwHTML.InnerText, this.cwJSCSSRef.InnerText
                );

            if (Request.QueryString.GetValues("ModalChangeMaster") != null)
            {
                if (this.IsCodeWriter)
                {
                    Response.Redirect(this.PageID.ToString());
                }
                else
                {
                    Response.Write("<script type=\"text/javascript\">window.parent.location = window.parent.location.href;</script>");
                }
            }
            else
                this.RedirectBackToReferringPage();
        }

        protected override void OnCancel(EventArgs e)
        {
            base.OnCancel(e);
            if (this.IsCodeWriter)
            {
                Response.Redirect(this.PageID.ToString());
            }
        }


        private string GetCurrentTheme()
        {
            Dictionary<string, ISettingItem> pageSettings = new Appleseed.Framework.Site.Configuration.PageSettings().GetPageCustomSettings(this.Module.PageID);

            string pageTheme = pageSettings[this.CustomTheme]?.Value?.ToString() ?? pageSettings[this.CustomThemeAlt]?.Value?.ToString();

            if (string.IsNullOrEmpty(pageTheme))
                pageTheme = this.PortalSettings.CurrentLayout;

            return pageTheme;
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
        protected void btnCreateNewVersion_Click(object sender, EventArgs e)
        {
            HtmlTextDB versionDB = new HtmlTextDB();
            int maxVersion = drpVirsionList.Items.Count + 1;
            if (this.IsCodeWriter)
            {
                this.DesktopText.Text = string.Format("<style type='text/css'>{0}</style>{1}<script type='text/javascript'>{2}</script>", this.cwCSS.InnerText, this.cwHTML.InnerText, this.cwJS.InnerText);
            }

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
                , this.cwCSS.InnerText, this.cwJS.InnerText, this.cwHTML.InnerText, this.cwJSCSSRef.InnerText);
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
        protected void btnPsVersion_Click(object sender, EventArgs e)
        {
            HtmlTextDB versionDB = new HtmlTextDB();
            int maxVersion = Convert.ToInt32(drpVirsionList.SelectedItem.Value);
            if (this.IsCodeWriter)
            {
                this.DesktopText.Text = string.Format("<style type='text/css'>{0}</style>{1}<script type='text/javascript'>{2}</script>", this.cwCSS.InnerText, this.cwHTML.InnerText, this.cwJS.InnerText);
            }
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
               , this.cwCSS.InnerText, this.cwJS.InnerText, this.cwHTML.InnerText, this.cwJSCSSRef.InnerText);

            if (Request.QueryString.GetValues("ModalChangeMaster") != null)
            {
                if (IsCodeWriter)
                {
                    Response.Redirect(this.PageID.ToString());
                }
                else
                {
                    Response.Write("<script type=\"text/javascript\">window.parent.location = window.parent.location.href;</script>");
                }
            }
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
        protected void btnHsVersion_Click(object sender, EventArgs e)
        {
            this.Response.Redirect("/DesktopModules/CoreModules/HTMLDocument/HtmlVersonHistory.aspx?mID=" + this.ModuleID + "&ModalChangeMaster=", true);
        }

        [WebMethod]
        public static string SaveData(string css, string html, string js, string pageId, string moduleId, string jscssref)
        {
            string rootPath = HostingEnvironment.ApplicationPhysicalPath + @"/DesktopModules/CoreModules/HTMLDocument/preview.html";
            try
            {
                string content = $"<!DOCTYPE html><html><head><title></title>{jscssref}<style type='text/css'>{css}</style></head><body>{html}</body><script type='text/javascript'>{js}</script></html>";

                rootPath = HostingEnvironment.ApplicationPhysicalPath + @"/DesktopModules/CoreModules/HTMLDocument/PageModulePreview/P" + pageId + "M" + moduleId + ".html";
                string folderPath = System.Web.HttpContext.Current.Server.MapPath("/DesktopModules/CoreModules/HTMLDocument/PageModulePreview");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                System.IO.File.WriteAllText(rootPath, content);
                return "/DesktopModules/CoreModules/HTMLDocument/PageModulePreview/P" + pageId + "M" + moduleId + ".html";
            }
            catch (Exception e)
            {
                return rootPath;
            }
        }
    }
}