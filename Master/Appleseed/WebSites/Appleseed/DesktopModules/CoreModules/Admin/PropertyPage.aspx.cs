namespace Appleseed.Content.Web.Modules
{
    using System;
    using System.Collections;
    using System.Web.UI.WebControls;

    using Appleseed.Framework;
    using Appleseed.Framework.Settings.Cache;
    using Appleseed.Framework.Site.Configuration;
    using Appleseed.Framework.Web.UI;
    using Appleseed.Framework.Web.UI.WebControls;

    using History = Appleseed.Framework.History;
    using HyperLink = Appleseed.Framework.Web.UI.WebControls.HyperLink;
    using LinkButton = Appleseed.Framework.Web.UI.WebControls.LinkButton;

    /// <summary>
    /// Summary description for Property Page
    /// </summary>
    [History("Jes1111", "2003/03/04", "Cache flushing now handled by inherited page")]
    public partial class PagePropertyPage : PropertyPage
    {
        protected Panel EditPanel;
        protected HyperLink adminPropertiesButton;
        protected PlaceHolder AddEditControl;
        protected LinkButton saveAndCloseButton;

        #region Web Form Designer generated code

        /// <summary>
        /// On init
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            this.PlaceHolderButtons.EnableViewState = false;
            this.PlaceholderButtons2.EnableViewState = false;

            //Controls must be created here
            this.UpdateButton = new LinkButton();
            this.UpdateButton.CssClass = "CommandButton";

            PlaceHolderButtons.Controls.Add(this.UpdateButton);

            // jminond added to top of property page so no need to scroll for save
            LinkButton update2 = new LinkButton();
            update2.CssClass = "CommandButton";
            update2.TextKey = "Apply";
            update2.Text = "Apply";
            update2.Click += new EventHandler(UpdateButton_Click);
            PlaceholderButtons2.Controls.Add(update2);

//			PlaceHolderButtons.Controls.Add(new LiteralControl("&nbsp;"));
//			PlaceholderButtons2.Controls.Add(new LiteralControl("&nbsp;"));

            saveAndCloseButton = new LinkButton();
            saveAndCloseButton.TextKey = "SAVE_AND_CLOSE";
            saveAndCloseButton.Text = "Save and close";
            saveAndCloseButton.CssClass = "CommandButton";
            PlaceHolderButtons.Controls.Add(saveAndCloseButton);

            // jminond added to top of property page so no need to scroll for save
            LinkButton saveAndCloseButton2 = new LinkButton();
            saveAndCloseButton2.TextKey = "SAVE_AND_CLOSE";
            saveAndCloseButton2.Text = "Save and close";
            saveAndCloseButton2.CssClass = "CommandButton";
            saveAndCloseButton2.Click += new EventHandler(this.saveAndCloseButton_Click);
            PlaceholderButtons2.Controls.Add(saveAndCloseButton2);

            this.saveAndCloseButton.Click += new EventHandler(this.saveAndCloseButton_Click);

//			PlaceHolderButtons.Controls.Add(new LiteralControl("&nbsp;"));
//			PlaceholderButtons2.Controls.Add(new LiteralControl("&nbsp;"));

            // Removed by Mario Endara <mario@softworks.com.uy> (2004/11/04)
//			if (Appleseed.Security.PortalSecurity.IsInRoles("Admins"))
//			{

            string NavigateUrlPropertyPage = Appleseed.Framework.HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/Admin/ModuleSettings.aspx", PageID, ModuleID);

            if (Request.QueryString.GetValues("ModalChangeMaster") != null) {
                NavigateUrlPropertyPage += "&ModalChangeMaster=true";
                if (Request.QueryString.GetValues("camefromEditPage") != null)
                    NavigateUrlPropertyPage += "&camefromEditPage=true";
            }

            adminPropertiesButton = new HyperLink();
            adminPropertiesButton.TextKey = "MODULESETTINGS_BASE_SETTINGS";
            adminPropertiesButton.Text = "Edit base settings";
            adminPropertiesButton.CssClass = "CommandButton";
            adminPropertiesButton.NavigateUrl = NavigateUrlPropertyPage;

            if (Framework.Security.UserProfile.HasPortalAdministrationAccess() || Framework.Security.UserProfile.HasModuleAddEditAccess())
            {
                PlaceHolderButtons.Controls.Add(adminPropertiesButton);
            }

            // jminond added to top of property page so no need to scroll for save
            HyperLink adminPropertiesButton2 = new HyperLink();
            adminPropertiesButton2.TextKey = "MODULESETTINGS_BASE_SETTINGS";
            adminPropertiesButton2.Text = "Edit base settings";
            adminPropertiesButton2.CssClass = "CommandButton";
            adminPropertiesButton2.NavigateUrl = NavigateUrlPropertyPage;

            if (Framework.Security.UserProfile.HasPortalAdministrationAccess() || Framework.Security.UserProfile.HasModuleAddEditAccess())
            {
                PlaceholderButtons2.Controls.Add(adminPropertiesButton2);
            }

//			PlaceHolderButtons.Controls.Add(new LiteralControl("&nbsp;"));
//			PlaceholderButtons2.Controls.Add(new LiteralControl("&nbsp;"));
//			}

            // jminond added to top of property page so no need to scroll for save
            LinkButton cancel2 = new LinkButton();
            cancel2.CssClass = "CommandButton";
            cancel2.TextKey = "Cancel";
            cancel2.Text = "Cancel";
            cancel2.Click += new EventHandler(CancelButton_Click);
            PlaceholderButtons2.Controls.Add(cancel2);

            this.CancelButton = new LinkButton();
            this.CancelButton.CssClass = "CommandButton";
            PlaceHolderButtons.Controls.Add(this.CancelButton);

//			if(((UI.Page)this.Page).IsCssFileRegistered("tabsControl") == false)
//			{
//				string themePath = Path.WebPathCombine(this.CurrentTheme.WebPath, "/tabControl.css");
//				((UI.Page)this.Page).RegisterCssFile("tabsControl", themePath);
//			}
// Modified by Hongwei Shen 10/72005-- the css file will be inject with the main theme
//			if(!((UI.Page)this.Page).IsCssFileRegistered("TabControl"))
//				((UI.Page)this.Page).RegisterCssFile("TabControl");


            this.EditTable.UpdateControl += new UpdateControlEventHandler(this.EditTable_UpdateControl);
            this.Load += new EventHandler(this.PagePropertyPage_Load);
            base.OnInit(e);
        }
        #endregion

        private void PagePropertyPage_Load(object sender, EventArgs e)
        {
            //We reset cache before display page to ensure dropdown shows actual data
            //by Pekka Ylenius
            CurrentCache.Remove(Key.ModuleSettings(ModuleID));
            // Using settings grouping tabs or not is set in config file. --Hongwei Shen
            EditTable.UseGroupingTabs = Appleseed.Framework.Settings.Config.UseSettingsGroupingTabs;
            // The width and height will take effect only when using grouping tabs.
            // When not using grouping tabs, width and height should be set in css 
            // class -- Hongwei Shen
            EditTable.Width = 1000;
            //EditTable.Height = Appleseed.Framework.Settings.Config.SettingsGroupingHeight;
            EditTable.CssClass = "st_control";
            EditTable.DataSource = new SortedList(this.ModuleSettings);
            EditTable.DataBind();
        }

        private void saveAndCloseButton_Click(object sender, EventArgs e)
        {
            OnUpdate(e);
            if (Page.IsValid == true && Request.QueryString.GetValues("ModalChangeMaster") == null)
                Response.Redirect(HttpUrlBuilder.BuildUrl("~/" + HttpUrlBuilder.DefaultPage, PageID));
        }

        /// <summary>
        /// Persists the changes to database
        /// </summary>
        protected override void OnUpdate(EventArgs e)
        {
            base.OnUpdate(e);

            // Only Update if Input Data is Valid
            if (Page.IsValid == true)
            {
                // Update settings in the database
                EditTable.UpdateControls();
            }

            if (Request.QueryString.GetValues("ModalChangeMaster") != null)
                Response.Write("<script type=\"text/javascript\">window.parent.location = window.parent.location.href;</script>");
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            this.OnUpdate(e);
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.OnCancel(e);
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            this.OnDelete(e);
        }

        protected override void OnCancel(EventArgs e)
        {
            base.OnCancel(e);
            //Response.Redirect(HttpUrlBuilder.BuildUrl("~/" + HttpUrlBuilder.DefaultPage, PageID));
        }

        private void EditTable_UpdateControl(object sender,
                                             Appleseed.Framework.Web.UI.WebControls.SettingsTableEventArgs e)
        {
            Framework.Site.Configuration.ModuleSettings.UpdateModuleSetting(ModuleID, ((ISettingItem)e.CurrentItem).EditControl.ID, ((ISettingItem)e.CurrentItem).Value.ToString());
        }
    }
}
