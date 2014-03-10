using System;
using System.Collections;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI.WebControls;
using Appleseed.Framework;
using Appleseed.Framework.Content.Data;
using Appleseed.Framework.DataTypes;
using Appleseed.Framework.Site.Configuration;
using Appleseed.Framework.Site.Data;
using Appleseed.Framework.Web.UI;
using Appleseed.Framework.Web.UI.WebControls;
using History=Appleseed.Framework.History;
using LanguageSwitcher=Appleseed.Framework.Localization.LanguageSwitcher;

namespace Appleseed.Content.Web.Modules
{
    using System.Collections.Generic;

    /// <summary>
    /// Appleseed EnhancedHtml Module EditItemPage
    /// Written by: José Viladiu, jviladiu@portalServices.net
    /// </summary>
    [History("jminond", "march 2005", "Changes for moving Tab to Page")]
    [
        History("jviladiu@portalServices.net", "2004/12/28",
            "Add support for include contents from others modules in a page")]
    [
        History("jviladiu@portalServices.net", "2004/07/05",
            "Changed SelectedValue for SelectedItem.Value in many places for compatibility with 1.0")]
    public partial class EnhancedHtmlEdit : EditItemPage
    {
        /// <summary>
        /// Html Text editor for the control
        /// </summary>
        protected IHtmlEditor DesktopText;

        private readonly string tokenModule = "#MODULE#";
        private readonly string tokenPortalModule = "#PORTALMODULE#";

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            HtmlEditorDataType h = new HtmlEditorDataType();
            h.Value = this.ModuleSettings["Editor"].ToString();
            DesktopText =
                h.GetEditor(PlaceHolderHTMLEditor, ModuleID, bool.Parse(this.ModuleSettings["ShowUpload"].ToString()),
                            this.PortalSettings);
            DesktopText.Width = new Unit(this.ModuleSettings["Width"].ToString());
            DesktopText.Height = new Unit(this.ModuleSettings["Height"].ToString());

            if (!Page.IsPostBack)
            {
                if (bool.Parse(this.ModuleSettings["ENHANCEDHTML_GET_CONTENTS_FROM_PORTALS"].ToString()))
                {
                    kindOfContent.Items.Add(new ListItem("External Module", "Portal"));
                    CustomListDataType cldtAll =
                        new CustomListDataType(new ModulesDB().GetModulesAllPortals(), "ModuleTitle", "ModuleID");
                    listAllModules.CssClass = "NormalTextBox";
                    listAllModules.DataSource = cldtAll.DataSource;
                    listAllModules.DataValueField = cldtAll.DataValueField;
                    listAllModules.DataTextField = cldtAll.DataTextField;
                    listAllModules.DataBind();
                }
                CustomListDataType cldt =
                    new CustomListDataType(new ModulesDB().GetModulesSinglePortal(this.PortalSettings.PortalID),
                                           "ModuleTitle", "ModuleID");
                listModules.CssClass = "NormalTextBox";
                listModules.DataSource = cldt.DataSource;
                listModules.DataValueField = cldt.DataValueField;
                listModules.DataTextField = cldt.DataTextField;
                listModules.DataBind();

                string comando =
                    General.GetString("ENHANCEDHTML_CONFIRMDELETEMESSAGE", "Are You Sure You Wish To Delete This Item ?");
                cmdDeletePage.Attributes.Add("onClick", "javascript:return confirm('" + comando + "');");
                ViewState["UrlReferrer"] = HttpUrlBuilder.BuildUrl(PageID);

                CultureInfo[] listaLang = LanguageSwitcher.GetLanguageList(true);

                lstLanguages.Items.Add(new ListItem(General.GetString("ENHANCEDHTML_SHOWALLPAGES", "All Pages"), "0"));
                foreach (CultureInfo ci in listaLang)
                {
                    lstLanguages.Items.Add(new ListItem(ci.DisplayName, (ci.LCID).ToString()));
                    listLanguages.Items.Add(new ListItem(ci.DisplayName, (ci.LCID).ToString()));
                }
                lstLanguages.SelectedIndex = 0;
                listLanguages.SelectedIndex = 0;
                ShowList();
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
                var al = new List<string> { "875254B7-2471-491f-BAF8-4AFC261CC224" };
                return al;
            }
        }

        /// <summary>
        /// Shows the list.
        /// </summary>
        private void ShowList()
        {
            bool showAllPages = (int.Parse(lstLanguages.SelectedItem.Value) < 1);
            CultureInfo[] listaLang = LanguageSwitcher.GetLanguageList(true);
            EnhancedHtmlDB ehdb = new EnhancedHtmlDB();
            SqlDataReader dr;
            lstPages.Items.Clear();

            if (showAllPages)
            {
                dr = ehdb.GetAllPages(ModuleID, WorkFlowVersion.Staging);
            }
            else
            {
                dr =
                    ehdb.GetLocalizedPages(ModuleID, int.Parse(lstLanguages.SelectedItem.Value), WorkFlowVersion.Staging);
            }


            try
            {
                while (dr.Read())
                {
                    if (showAllPages)
                    {
                        int ccode = int.Parse(dr["CultureCode"].ToString());
                        string texto = string.Empty;

                        foreach (CultureInfo ci in listaLang)
                        {
                            if (ccode == ci.LCID) texto = ci.DisplayName;
                        }
                        if (texto.Length != 0)
                        {
                            lstPages.Items.Add(
                                new ListItem((string) dr["Title"] + " (" + texto + ")", dr["ItemID"].ToString()));
                        }
                    }
                    else
                    {
                        lstPages.Items.Add(new ListItem((string) dr["Title"], dr["ItemID"].ToString()));
                    }
                }
            }
            finally
            {
                dr.Close();
            }
            if (lstPages.Items.Count > 0)
            {
                lstPages.SelectedIndex = 0;
                cmdEditPage.Enabled = true;
                cmdDeletePage.Enabled = true;
            }
            else
            {
                cmdEditPage.Enabled = false;
                cmdDeletePage.Enabled = false;
            }
        }

        #region Web Form Designer generated code

        /// <summary>
        /// Raises OnInitEvent
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            this.lstLanguages.SelectedIndexChanged += new EventHandler(this.selectLocalizedPages);
            this.cmdNewPage.Click += new EventHandler(this.cmdNewPage_Click);
            this.cmdEditPage.Click += new EventHandler(this.cmdEditPage_Click);
            this.cmdDeletePage.Click += new EventHandler(this.cmdDeletePage_Click);
            this.cmdReturn.Click += new EventHandler(this.cmdReturn_Click);
            this.kindOfContent.SelectedIndexChanged += new EventHandler(this.kindOfContents_Click);
            this.cmdPageUpdate.Click += new EventHandler(this.cmdPageUpdate_Click);
            this.cmdPageCancel.Click += new EventHandler(this.cmdPageCancel_Click);
            this.listModules.SelectedIndexChanged += new EventHandler(this.kindOfContents_Click);
            this.Load += new EventHandler(this.Page_Load);

            // Call base first
            base.OnInit(e);
        }

        #endregion

        #region Events

        /// <summary>
        /// Handles the Click event of the cmdReturn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected void cmdReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect(((string) ViewState["UrlReferrer"]), true);
        }

        /// <summary>
        /// Handles the Click event of the cmdPageCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected void cmdPageCancel_Click(object sender, EventArgs e)
        {
            pnlSelectPage.Visible = true;
            pnlEditPage.Visible = false;
            if (lstPages.SelectedItem == null) // Cancel a new page
            {
                if (lstPages.Items.Count > 0)
                {
                    lstPages.SelectedIndex = 0;
                    cmdEditPage.Enabled = true;
                    cmdDeletePage.Enabled = true;
                }
                else
                {
                    lstPages.SelectedIndex = -1;
                    cmdEditPage.Enabled = false;
                    cmdDeletePage.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the cmdDeletePage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected void cmdDeletePage_Click(object sender, EventArgs e)
        {
            if ((lstPages.SelectedItem.Value != null) && (lstPages.SelectedItem.Value != "-1"))
            {
                EnhancedHtmlDB tdb1 = new EnhancedHtmlDB();
                tdb1.DeletePage(int.Parse(this.lstPages.SelectedItem.Value));
                if (lstPages.Items.Count > 0)
                {
                    lstPages.SelectedIndex = 0;
                    cmdEditPage.Enabled = true;
                    cmdDeletePage.Enabled = true;
                }
                else
                {
                    lstPages.SelectedIndex = -1;
                    cmdEditPage.Enabled = false;
                    cmdDeletePage.Enabled = false;
                }
                Response.Redirect(Request.Url.ToString(), true);
            }
        }

        /// <summary>
        /// Handles the Click event of the cmdEditPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected void cmdEditPage_Click(object sender, EventArgs e)
        {
            pnlSelectPage.Visible = false;
            pnlEditPage.Visible = true;
            txtPageName.Text = string.Empty;
            txtViewOrder.Text = "100";

            EnhancedHtmlDB ehdb = new EnhancedHtmlDB();
            SqlDataReader dr =
                ehdb.GetSinglePage(int.Parse(lstPages.SelectedItem.Value), Appleseed.Framework.WorkFlowVersion.Staging);

            try
            {
                if (dr.Read())
                {
                    txtPageName.Text = ((string) dr["Title"]);
                    txtViewOrder.Text = dr["ViewOrder"].ToString();
                    foreach (ListItem li in listLanguages.Items)
                    {
                        li.Selected = li.Value.Equals(dr["CultureCode"].ToString());
                    }
                    DesktopText.Text = Server.HtmlDecode(((string) dr["DesktopHtml"]));
                    CreatedBy.Text = (string) dr["CreatedByUser"];
                    CreatedDate.Text = ((DateTime) dr["CreatedDate"]).ToShortDateString();
                }
            }
            finally
            {
                dr.Close();
            }
            if (DesktopText.Text.StartsWith(tokenModule))
            {
                kindOfContent.SelectedIndex = 1;
                int module = int.Parse(DesktopText.Text.Substring(tokenModule.Length));
                foreach (ListItem li in listModules.Items)
                    if (int.Parse(li.Value) == module)
                        li.Selected = true;
            }
            else if (DesktopText.Text.StartsWith(tokenPortalModule))
            {
                kindOfContent.SelectedIndex = 2;
                int module = int.Parse(DesktopText.Text.Substring(tokenPortalModule.Length));
                foreach (ListItem li in listAllModules.Items)
                    if (int.Parse(li.Value) == module)
                        li.Selected = true;
            }
            else
                kindOfContent.SelectedIndex = 0;
            kindOfContents_Click(sender, e);
        }

        /// <summary>
        /// Handles the Click event of the cmdNewPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected void cmdNewPage_Click(object sender, EventArgs e)
        {
            pnlSelectPage.Visible = false;
            pnlEditPage.Visible = true;
            txtPageName.Text = string.Empty;
            txtViewOrder.Text = "100";
            DesktopText.Text = string.Empty;
            lstPages.SelectedIndex = -1;
        }

        /// <summary>
        /// Handles the Click event of the cmdPageUpdate control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected void cmdPageUpdate_Click(object sender, EventArgs e)
        {
            string titlePage = txtPageName.Text;
            if (((titlePage == null) || (titlePage.Length < 1)) || (titlePage.Trim().Length < 1))
            {
                titlePage = "Page";
            }
            string orderView = txtViewOrder.Text;
            int order = 0;
            try
            {
                order = int.Parse(orderView);
            }
            catch
            {
            }
            if (order < 1)
            {
                orderView = "100";
                txtViewOrder.Text = orderView;
                order = 100;
            }
            string text = string.Empty;
            int i;

            switch (kindOfContent.SelectedValue)
            {
                case "Editor":
                    text = Server.HtmlEncode(DesktopText.Text);
                    if (text.StartsWith(tokenModule) || text.StartsWith(tokenPortalModule))
                        text = string.Empty;
                    break;
                case "Module":
                    i = int.Parse(listModules.SelectedValue);
                    if (i == 0 || i == this.ModuleID) return; // Cannot select this module
                    text = tokenModule + listModules.SelectedValue;
                    break;
                case "Portal":
                    i = int.Parse(listAllModules.SelectedValue);
                    if (i == 0 || i == this.ModuleID) return; // Cannot select this module
                    text = tokenPortalModule + listAllModules.SelectedValue;
                    break;
            }

            string user = PortalSettings.CurrentUser.Identity.UserName;
            EnhancedHtmlDB tdb1 = new EnhancedHtmlDB();
            if (lstPages.SelectedIndex >= 0)
            {
                int itemID = int.Parse(lstPages.SelectedItem.Value);
                tdb1.UpdatePage(ModuleID, itemID, user, titlePage, order, int.Parse(listLanguages.SelectedItem.Value),
                                text);
            }
            else
            {
                tdb1.AddPage(ModuleID, 0, user, titlePage, order, int.Parse(listLanguages.SelectedItem.Value), text);
            }
            pnlSelectPage.Visible = true;
            pnlEditPage.Visible = false;
            ShowList();
        }

        /// <summary>
        /// Selects the localized pages.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void selectLocalizedPages(object sender, EventArgs e)
        {
            ShowList();
        }

        /// <summary>
        /// Handles the Click event of the kindOfContents control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void kindOfContents_Click(object sender, EventArgs e)
        {
            switch (kindOfContent.SelectedValue)
            {
                case "Editor":
                    Literal2.TextKey = "HTML_DESKTOP_CONTENT";
                    Literal2.Text = "Desktop HTML Content";
                    PlaceHolderHTMLEditor.Visible = true;
                    listModules.Visible = false;
                    listAllModules.Visible = false;
                    string text = Server.HtmlEncode(DesktopText.Text);
                    if (text.StartsWith(tokenModule) || text.StartsWith(tokenPortalModule))
                        DesktopText.Text = string.Empty;
                    break;
                case "Module":
                    Literal2.TextKey = "ENHANCEDHTML_SELECT_MODULE";
                    Literal2.Text = "Select a module from this portal for show the contents in this page";
                    PlaceHolderHTMLEditor.Visible = false;
                    listModules.Visible = true;
                    listAllModules.Visible = false;
                    break;
                case "Portal":
                    Literal2.TextKey = "ENHANCEDHTML_SELECT_MODULE_PORTAL";
                    Literal2.Text = "Select a module in database portals for show the contents in this page";
                    PlaceHolderHTMLEditor.Visible = false;
                    listModules.Visible = false;
                    listAllModules.Visible = true;
                    break;
            }
        }

        #endregion
    }
}