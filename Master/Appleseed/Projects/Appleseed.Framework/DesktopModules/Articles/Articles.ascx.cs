// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Articles.ascx.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   Articles
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Content.Web.Modules
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Web.UI.WebControls;

    using Appleseed.Framework;
    using Appleseed.Framework.Content.Data;
    using Appleseed.Framework.Data;
    using Appleseed.Framework.DataTypes;
    using Appleseed.Framework.Helpers;
    using Appleseed.Framework.Security;
    using Appleseed.Framework.Users.Data;
    using Appleseed.Framework.Web.UI.WebControls;

    /// <summary>
    /// Articles module
    /// </summary>
    public class Articles : PortalModuleControl
    {
        #region Constants and Fields

        /// <summary>
        /// My data list.
        /// </summary>
        protected DataList MyDataList;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "Articles" /> class.
        /// </summary>
        public Articles()
        {
            this.SupportsWorkflow = true;

            if (this.PortalSettings == null)
            {
                return;
            }

            // check for avoid design time errors

            // modified by Hongwei Shen(hongwei.shen@gmail.com) 12/9/2005
            const SettingItemGroup Group = SettingItemGroup.MODULE_SPECIAL_SETTINGS;
            const int GroupBase = (int)Group;

            // end of modification

            // Set Editor Settings jviladiu@portalservices.net 2004/07/30
            // modified by Hongwei Shen
            // HtmlEditorDataType.HtmlEditorSettings (this._baseSettings, SettingItemGroup.MODULE_SPECIAL_SETTINGS);
            HtmlEditorDataType.HtmlEditorSettings(this.BaseSettings, Group);

            // end of modification

            // Switches date display on/off
            var showDate = new SettingItem<bool, CheckBox>
                {
                    Value = true, EnglishName = "Show Date", Group = Group, Order = GroupBase + 20 
                };

            // modified by Hongwei Shen
            // ShowDate.Group = SettingItemGroup.MODULE_SPECIAL_SETTINGS;
            // ShowDate.Order = 10;

            // end of modification
            this.BaseSettings.Add("ShowDate", showDate);

            // Added by Rob Siera
            var defaultVisibleDays = new SettingItem<int, TextBox>
                {
                    Value = 90, EnglishName = "Default Days Visible", Group = Group, Order = GroupBase + 25 
                };

            // modified by Hongwei Shen
            // DefaultVisibleDays.Group = SettingItemGroup.MODULE_SPECIAL_SETTINGS;
            // DefaultVisibleDays.Order = 20;

            // end of modification
            this.BaseSettings.Add("DefaultVisibleDays", defaultVisibleDays);

            var richAbstract = new SettingItem<bool, CheckBox>
                {
                    Value = true,
                    EnglishName = "Rich Abstract",
                    Description = "User rich editor for abstract",
                    Group = Group,
                    Order = GroupBase + 30
                };

            // modified by Hongwei Shen
            // RichAbstract.Group = SettingItemGroup.MODULE_SPECIAL_SETTINGS;
            // RichAbstract.Order = 30;

            // end of modification
            this.BaseSettings.Add("ARTICLES_RICHABSTRACT", richAbstract);

            var users = new UsersDB();
            var rolesViewExpiredItems =
                new SettingItem<string, CheckBoxList>(
                    new CheckBoxListDataType(
                        users.GetPortalRoles(this.PortalSettings.PortalAlias), "RoleName", "RoleName"))
                    {
                        Value = "Admins",
                        EnglishName = "Expired items visible to",
                        Description = "Role that can see expire items",
                        Group = Group,
                        Order = GroupBase + 40
                    };

            // modified by Hongwei Shen
            // RolesViewExpiredItems.Group = SettingItemGroup.MODULE_SPECIAL_SETTINGS;
            // RolesViewExpiredItems.Order = 40;

            // end of modification
            this.BaseSettings.Add("EXPIRED_PERMISSION_ROLE", rolesViewExpiredItems);
        }

        #endregion

        #region Properties

        /// <summary>
        ///   GUID of module (mandatory)
        /// </summary>
        /// <value></value>
        public override Guid GuidID
        {
            get
            {
                return new Guid("{87303CF7-76D0-49B1-A7E7-A5C8E26415BA}");
            }
        }

        /// <summary>
        ///   Searchable module
        /// </summary>
        /// <value></value>
        public override bool Searchable
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether [show date].
        /// </summary>
        /// <value><c>true</c> if [show date]; otherwise, <c>false</c>.</value>
        protected bool ShowDate
        {
            get
            {
                // Hide/show date
                return bool.Parse(this.Settings["ShowDate"].ToString());
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Installs the specified state saver.
        /// </summary>
        /// <param name="stateSaver">The state saver.</param>
        /// <remarks></remarks>
        public override void Install(IDictionary stateSaver)
        {
            var currentScriptName = Path.Combine(this.Server.MapPath(this.TemplateSourceDirectory), "install.sql");
            var errors = DBHelper.ExecuteScript(currentScriptName, true);
            if (errors.Count > 0)
            {
                // Call rollback
                throw new Exception("Error occurred:" + errors[0]);
            }
        }

        /// <summary>
        /// Searchable module implementation
        /// </summary>
        /// <param name="portalId">
        /// The portal ID
        /// </param>
        /// <param name="userId">
        /// ID of the user is searching
        /// </param>
        /// <param name="searchString">
        /// The text to search
        /// </param>
        /// <param name="searchField">
        /// The fields where performing the search
        /// </param>
        /// <returns>
        /// The SELECT SQL to perform a search on the current module
        /// </returns>
        public override string SearchSqlSelect(int portalId, int userId, string searchString, string searchField)
        {
            var s = new SearchDefinition(
                "rb_Articles", "Title", "Abstract", "CreatedByUser", "CreatedDate", searchField);

            // Add extra search fields here, this way
            s.ArrSearchFields.Add("itm.Description");

            return s.SearchSqlSelect(portalId, userId, searchString);
        }

        /// <summary>
        /// Uninstalls the specified state saver.
        /// </summary>
        /// <param name="stateSaver">The state saver.</param>
        /// <remarks></remarks>
        public override void Uninstall(IDictionary stateSaver)
        {
            var currentScriptName = Path.Combine(this.Server.MapPath(this.TemplateSourceDirectory), "uninstall.sql");
            var errors = DBHelper.ExecuteScript(currentScriptName, true);
            if (errors.Count > 0)
            {
                // Call rollback
                throw new Exception("Error occurred:" + errors[0]);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Raises OnInit event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnInit(EventArgs e)
        {
            // View state is not needed here, so we are disabling. - jminond
            this.MyDataList.EnableViewState = false;

            // Add support for the edit page
            this.AddText = "ADD_ARTICLE";
            this.AddUrl = "~/DesktopModules/CommunityModules/Articles/ArticlesEdit.aspx";

            // Obtain Articles information from the Articles table
            // and bind to the data list control
            var articles = new ArticlesDB();

            this.MyDataList.DataSource = PortalSecurity.IsInRoles(this.Settings["EXPIRED_PERMISSION_ROLE"].ToString())
                                             ? articles.GetArticlesAll(this.ModuleID, this.Version)
                                             : articles.GetArticles(this.ModuleID, this.Version);
            this.MyDataList.DataBind();

            base.OnInit(e);
        }

        #endregion
    }
}