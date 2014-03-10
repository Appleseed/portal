// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchDefinition.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   This struct stores custom parameters needed by
//   the search helper for do the search string.
//   This make the search string consistent and easy
//   to change without modify all the searchable modules
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Helpers
{
    using System;
    using System.Collections;
    using System.Text;

    /// <summary>
    /// This struct stores custom parameters needed by
    ///   the search helper for do the search string.
    ///   This make the search string consistent and easy
    ///   to change without modify all the searchable modules
    /// </summary>
    public struct SearchDefinition
    {
        #region Constants and Fields

        /// <summary>
        /// The abstract field.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public string AbstractField;

        /// <summary>
        /// The arr search fields.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public ArrayList ArrSearchFields;

        /// <summary>
        /// The created by user field.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public string CreatedByUserField;

        /// <summary>
        /// The created date field.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public string CreatedDateField;

        /// <summary>
        /// The item id field.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public string ItemIDField;

        /// <summary>
        /// The page id field.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public string PageIDField;

        /// <summary>
        /// The table name.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public string TableName;

        /// <summary>
        /// The title field.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public string TitleField;

        /// <summary>
        /// The string "itm."
        /// </summary>
        private const string StringItmDot = "itm.";

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchDefinition"/> class.
        /// </summary>
        /// <param name="tableName">
        /// Name of the table.
        /// </param>
        /// <param name="titleField">
        /// The title field.
        /// </param>
        /// <param name="abstractField">
        /// The abstract field.
        /// </param>
        /// <param name="searchField">
        /// The search field.
        /// </param>
        /// <returns>
        /// A void value...
        /// </returns>
        public SearchDefinition(string tableName, string titleField, string abstractField, string searchField)
        {
            this.TableName = tableName;
            this.PageIDField = "mod.TabID";
            this.ItemIDField = "ItemID";
            this.TitleField = titleField;
            this.AbstractField = abstractField;
            this.CreatedByUserField = "''";
            this.CreatedDateField = "''";
            this.ArrSearchFields = new ArrayList();

            if (searchField == string.Empty)
            {
                this.ArrSearchFields.Add(StringItmDot + this.TitleField);
                this.ArrSearchFields.Add(StringItmDot + this.AbstractField);
            }
            else
            {
                if (searchField == "Title")
                {
                    this.ArrSearchFields.Add(StringItmDot + this.TitleField);
                }
                else
                {
                    this.ArrSearchFields.Add(StringItmDot + searchField);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchDefinition"/> class.
        /// </summary>
        /// <param name="tableName">
        /// Name of the table.
        /// </param>
        /// <param name="titleField">
        /// The title field.
        /// </param>
        /// <param name="abstractField">
        /// The abstract field.
        /// </param>
        /// <param name="createdByUserField">
        /// The created by user field.
        /// </param>
        /// <param name="createdDateField">
        /// The created date field.
        /// </param>
        /// <param name="searchField">
        /// The search field.
        /// </param>
        /// <returns>
        /// A void value...
        /// </returns>
        public SearchDefinition(
            string tableName, 
            string titleField, 
            string abstractField, 
            string createdByUserField, 
            string createdDateField, 
            string searchField)
        {
            this.TableName = tableName;
            this.PageIDField = "mod.TabID";
            this.ItemIDField = "ItemID";
            this.TitleField = titleField;
            this.AbstractField = abstractField;
            this.CreatedByUserField = createdByUserField;
            this.CreatedDateField = createdDateField;
            this.ArrSearchFields = new ArrayList();

            if (searchField == string.Empty)
            {
                this.ArrSearchFields.Add(StringItmDot + this.TitleField);
                this.ArrSearchFields.Add(StringItmDot + this.AbstractField);
            }
            else
            {
                if (searchField == "Title")
                {
                    this.ArrSearchFields.Add(StringItmDot + this.TitleField);
                }
                else
                {
                    this.ArrSearchFields.Add(StringItmDot + searchField);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchDefinition"/> class. 
        /// </summary>
        /// <param name="tableName">
        /// Name of the table.
        /// </param>
        /// <param name="tabIdField">
        /// The tab ID field.
        /// </param>
        /// <param name="itemIdField">
        /// The item ID field.
        /// </param>
        /// <param name="titleField">
        /// The title field.
        /// </param>
        /// <param name="abstractField">
        /// The abstract field.
        /// </param>
        /// <param name="createdByUserField">
        /// The created by user field.
        /// </param>
        /// <param name="createdDateField">
        /// The created date field.
        /// </param>
        /// <param name="searchField">
        /// The search field.
        /// </param>
        public SearchDefinition(
            string tableName, 
            string tabIdField, 
            string itemIdField, 
            string titleField, 
            string abstractField, 
            string createdByUserField, 
            string createdDateField, 
            string searchField)
        {
            this.TableName = tableName;
            this.PageIDField = tabIdField;
            this.ItemIDField = itemIdField;
            this.TitleField = titleField;
            this.AbstractField = abstractField;
            this.CreatedByUserField = createdByUserField;
            this.CreatedDateField = createdDateField;
            this.ArrSearchFields = new ArrayList();

            if (searchField == string.Empty)
            {
                this.ArrSearchFields.Add(StringItmDot + this.TitleField);
                this.ArrSearchFields.Add(StringItmDot + this.AbstractField);
            }
            else
            {
                if (searchField == "Title")
                {
                    this.ArrSearchFields.Add(StringItmDot + this.TitleField);
                }
                else
                {
                    this.ArrSearchFields.Add(StringItmDot + searchField);
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Searches the SQL select.
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <param name="userId">
        /// The user ID.
        /// </param>
        /// <param name="searchStr">
        /// The search STR.
        /// </param>
        /// <returns>
        /// A string value...
        /// </returns>
        public string SearchSqlSelect(int portalId, int userId, string searchStr)
        {
            return this.SearchSqlSelect(portalId, userId, searchStr, true);
        }

        /// <summary>
        /// Builds a SELECT query using given parameters
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <param name="userId">
        /// The user ID.
        /// </param>
        /// <param name="searchStr">
        /// The search STR.
        /// </param>
        /// <param name="hasItemId">
        /// if set to <c>true</c> [has item ID].
        /// </param>
        /// <returns>
        /// The search SQL select.
        /// </returns>
        public string SearchSqlSelect(int portalId, int userId, string searchStr, bool hasItemId)
        {
            if (string.IsNullOrEmpty(this.CreatedByUserField))
            {
                this.CreatedByUserField = "''";
            }

            if (string.IsNullOrEmpty(this.CreatedDateField))
            {
                this.CreatedDateField = "''";
            }

            // SQL injection filter
            searchStr = this.FilterString(searchStr);

            if (searchStr.Length < 3)
            {
                throw new ArgumentException(
                    "Please use a word with at least 3 valid chars (invalid chars were removed).");
            }

            // special extended search feature (used by RSS/Community Service). Added by Jakob Hansen
            var extraSql = string.Empty;

            if (searchStr.StartsWith("AddExtraSQL:"))
            {
                var posSs = searchStr.IndexOf("SearchString:");

                if (posSs > 0)
                {
                    // Get the added search string if any
                    extraSql = posSs > 12 ? searchStr.Substring(12, posSs - 12).Trim() : string.Empty;

                    searchStr = searchStr.Substring(posSs + 14).Trim();
                }
                else
                {
                    // There are no added search string
                    extraSql = searchStr.Substring(12).Trim();
                    searchStr = string.Empty;
                }

                // Are the required "AND " missing? (then add it!)
                if (extraSql.Length != 0 && !extraSql.StartsWith("AND"))
                {
                    extraSql = string.Format("AND {0}", extraSql);
                }
            }

            var select = new StringBuilder();
            select.Append("SELECT TOP 50 ");
            select.Append("genModDef.FriendlyName AS ModuleName, ");
            select.Append("CAST (itm.");
            select.Append(this.TitleField);
            select.Append(" AS NVARCHAR(100)) AS Title, ");
            select.Append("CAST (itm.");
            select.Append(this.AbstractField);
            select.Append(" AS NVARCHAR(100)) AS Abstract, ");
            select.Append("itm.ModuleID AS ModuleID, ");

            if (hasItemId)
            {
                select.Append(StringItmDot + this.ItemIDField + " AS ItemID, ");
            }
            else
            {
                select.Append("itm.ModuleID AS ItemID, ");
            }

            if (!this.CreatedByUserField.StartsWith("'"))
            {
                select.Append(StringItmDot); // Add itm only if not a constant value
            }

            select.Append(this.CreatedByUserField);
            select.Append(" AS CreatedByUser, ");

            if (!this.CreatedDateField.StartsWith("'"))
            {
                select.Append(StringItmDot); // Add itm only if not a constant value
            }

            select.Append(this.CreatedDateField);
            select.Append(" AS CreatedDate, ");
            select.Append(this.PageIDField + " AS TabID, ");
            select.Append("tab.TabName AS TabName, ");
            select.Append("genModDef.GeneralModDefID AS GeneralModDefID, ");
            select.Append("mod.ModuleTitle AS ModuleTitle ");
            select.Append("FROM ");
            select.Append(this.TableName);
            select.Append(" itm INNER JOIN ");
            select.Append("rb_Modules mod ON itm.ModuleID = mod.ModuleID INNER JOIN ");
            select.Append("rb_ModuleDefinitions modDef ON mod.ModuleDefID = modDef.ModuleDefID INNER JOIN ");
            select.Append("rb_Pages tab ON mod.TabID = tab.PageID INNER JOIN ");
            select.Append(
                "rb_GeneralModuleDefinitions genModDef ON modDef.GeneralModDefID = genModDef.GeneralModDefID ");

            // 			if (topicName.Length != 0)
            // 				select.Append("INNER JOIN rb_ModuleSettings modSet ON mod.ModuleID = modSet.ModuleID");
            select.Append("%TOPIC_PLACEHOLDER_JOIN%");
            SearchHelper.AddSharedSQL(portalId, userId, ref select, this.TitleField);

            // 			if (topicName.Length != 0)
            // 				select.Append(" AND (modSet.SettingName = 'TopicName' AND modSet.SettingValue='" + topicName + "')");
            select.Append("%TOPIC_PLACEHOLDER%");

            if (searchStr.Length != 0)
            {
                select.AppendFormat(" AND {0}", SearchHelper.CreateTestSQL(this.ArrSearchFields, searchStr, true));
            }

            if (extraSql.Length != 0)
            {
                select.Append(extraSql);
            }

            return select.ToString();
        }

        #endregion

        #region Methods

        /// <summary>
        /// SQL injection prevention
        /// </summary>
        /// <param name="toClean">
        /// To clean.
        /// </param>
        /// <returns>
        /// The filter string.
        /// </returns>
        private string FilterString(string toClean)
        {
            var c = new StringBuilder(toClean);
            string[] knownbad = { "select", "insert", "update", "delete", "drop", "--", "'", "char", ";" };

            foreach (var t in knownbad)
            {
                c.Replace(t, string.Empty);
            }

            return c.ToString();
        }

        #endregion

        /* Jakob Hansen, 20 may: Before the RSS/Web Service community release

                /// <summary>
                /// Builds a SELECT query using given parameters
                /// </summary>
                /// <param name="portalID"></param>
                /// <param name="userID"></param>
                /// <param name="searchString"></param>
                /// <returns></returns>
                public string SearchSqlSelect(int portalID, int userID, string 
        searchString, bool hasItemID)
                {
                    System.Text.StringBuilder select = new System.Text.StringBuilder();
                    select.Append("SELECT ");
                    select.Append("genModDef.FriendlyName AS ModuleName, ");
                    select.Append("CAST (itm.");
                    select.Append(TitleField);
                    select.Append(" AS NVARCHAR(100)) AS Title, ");
                    select.Append("CAST (itm.");
                    select.Append(AbstractField);
                    select.Append(" AS NVARCHAR(100)) AS Abstract, ");
                    select.Append("itm.ModuleID AS ModuleID, ");

                    if (hasItemID)
                        select.Append("itm.ItemID AS ItemID, ");

                    else
                        select.Append("itm.ModuleID AS ItemID, ");

                    if (!CreatedByUserField.StartsWith("'"))
                        select.Append(strItm);   // Add itm only if not a constant value
                    select.Append(CreatedByUserField);
                    select.Append(" AS CreatedByUser, ");

                    if (!CreatedDateField.StartsWith("'"))
                        select.Append(strItm);   // Add itm only if not a constant value
                    select.Append(CreatedDateField);
                    select.Append(" AS CreatedDate, ");
                    select.Append("mod.TabID AS TabID, ");
                    select.Append("tab.TabName AS TabName, ");
                    select.Append("genModDef.GeneralModDefID AS GeneralModDefID, ");
                    select.Append("mod.ModuleTitle AS ModuleTitle ");
                    select.Append("FROM ");
                    select.Append(TableName);
                    select.Append(" itm INNER JOIN ");
                    select.Append("rb_Modules mod ON itm.ModuleID = mod.ModuleID INNER JOIN ");
                    select.Append("rb_ModuleDefinitions modDef ON mod.ModuleDefID = modDef.ModuleDefID INNER JOIN ");
                    select.Append("rb_Tabs tab ON mod.TabID = tab.TabID INNER JOIN ");
                    select.Append("rb_GeneralModuleDefinitions genModDef ON modDef.GeneralModDefID = genModDef.GeneralModDefID ");
                    Helpers.SearchHelper.AddSharedSQL(portalID, userID, ref select, TitleField);
                    select.Append(" AND " + Appleseed.Framework.Helpers.SearchHelper.CreateTestSQL(ArrSearchFields, searchString, true));
                    return select.ToString();
                }
        */
    }
}