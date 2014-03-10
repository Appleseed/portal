using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Appleseed.Framework.Data;
using Appleseed.Framework.DataTypes;
using Appleseed.Framework.Site.Configuration;
using Appleseed.Framework.Users.Data;
using Appleseed.Framework.Web.UI.WebControls;

namespace Appleseed.Framework.Helpers
{

	/// <summary>
	/// Lists the possible types of data sources for the MDF system
	/// </summary>
	public enum DataSourceType 
	{
		/// <summary>The current module</summary>
		This, 
		/// <summary>All modules in the portal</summary>
		All, 
		/// <summary>The specified list of modules (for the portal)</summary>
		List
	}

	/// <summary>
	/// MDFHelper file by Jakob Hansen.
	/// MDF = Module Data Filter
	/// This class Represents all settings used by the MDF settings system
	/// </summary>
	public class MDFSettings
    {
        #region Public Fields
        /// <summary>
        /// 
        /// </summary>
        public const string NameApplyMDF     = "MDF_APPLY_MDF";
        /// <summary>
        /// 
        /// </summary>
		public const string NameDataSource   = "MDF_DATA_SOURCE";
        /// <summary>
        /// 
        /// </summary>
		public const string NameMaxHits      = "MDF_MAX_HITS";
        /// <summary>
        /// 
        /// </summary>
		public const string NameModuleList   = "MDF_MODULE_LIST";
        /// <summary>
        /// 
        /// </summary>
		public const string NameAllNotInList = "MDF_ALL_NOT_IN_LIST";
        /// <summary>
        /// 
        /// </summary>
		public const string NameSortField    = "MDF_SORT_FIELD";
        /// <summary>
        /// 
        /// </summary>
		public const string NameSortDirection= "MDF_SORT_DIRECTION";
        /// <summary>
        /// 
        /// </summary>
		public const string NameSearchString = "MDF_SEARCH_STRING";
        /// <summary>
        /// 
        /// </summary>
		public const string NameSearchField  = "MDF_SEARCH_FIELD";
        /// <summary>
        /// 
        /// </summary>
		public const string NameMobileOnly   = "MDF_MOBILE_ONLY";

        /// <summary>
        /// 
        /// </summary>
		public const bool DefaultValueApplyMDF = false;
        /// <summary>
        /// 
        /// </summary>
		public const DataSourceType DefaultValueDataSource = DataSourceType.This;
        /// <summary>
        /// 
        /// </summary>
		public const int DefaultValueMaxHits = 20;
        /// <summary>
        /// 
        /// </summary>
		public const string DefaultValueModuleList = "";
        /// <summary>
        /// 
        /// </summary>
		public const bool DefaultValueAllNotInList = false;
        /// <summary>
        /// 
        /// </summary>
		public const string DefaultValueSortField = "";
        /// <summary>
        /// 
        /// </summary>
		public const string DefaultValueSortDirection = "ASC";
        /// <summary>
        /// 
        /// </summary>
		public const string DefaultValueSearchString = "";
        /// <summary>
        /// 
        /// </summary>
		public const string DefaultValueSearchField = "";
        /// <summary>
        /// 
        /// </summary>
		public const bool DefaultValueMobileOnly = false;
        #endregion

        #region Private fields
        bool _applyMDF = DefaultValueApplyMDF;
		DataSourceType _dataSource = DefaultValueDataSource;
		int _maxHits = DefaultValueMaxHits;
		string _moduleList = DefaultValueModuleList;   //Module ID. Can be a list: "23,45,56"
		bool _allNotInList = DefaultValueAllNotInList;
		string _sortField = DefaultValueSortField;
		string _sortDirection = DefaultValueSortDirection;
		string _searchString = DefaultValueSearchString;
		string _searchField = DefaultValueSearchField;
		bool _mobileOnly = DefaultValueMobileOnly;

		bool _supportsWorkflow = false;
		WorkFlowVersion _workflowVersion = WorkFlowVersion.Production;

		string _itemTableName = "";
		string _titleFieldName = "";
		string _selectFieldList = "";
		string _searchFieldList = "";

		int _portalID = -1;
		int _userID = -1;
        #endregion

        #region Public Properties
        /// <summary>
        /// Controls if the MDF system should be used.
        /// Default value: false
        /// </summary>
        /// <value><c>true</c> if [apply MDF]; otherwise, <c>false</c>.</value>
		public bool ApplyMDF
		{
			get {return _applyMDF;}
			set {_applyMDF = value;}
		}

        /// <summary>
        /// Controls the data displyed in the module
        /// Default value: DataSourceType.This;
        /// </summary>
        /// <value>The data source.</value>
		public DataSourceType DataSource
		{
			get {return _dataSource;}
			set {_dataSource = value;}
		}

        /// <summary>
        /// Represents the number of items returned by the service.
        /// Value 0 means no hit limit (all found items are displayed).
        /// Default value: 20
        /// </summary>
        /// <value>The max hits.</value>
		public int MaxHits
		{
			get {return _maxHits;}
			set {_maxHits = value;}
		}

        /// <summary>
        /// Comma separated list of module ID's. e.g.: 1234,234,5454.
        /// Only data for these ID's are listed. (see also AllNotInList!)
        /// Default value: string.Empty
        /// </summary>
        /// <value>The module list.</value>
		public string ModuleList
		{
			get {return _moduleList;}
			set {_moduleList = value;}
		}

        /// <summary>
        /// If DataSource is All or List this can exclude modules listed in ModuleList
        /// Default value: false
        /// </summary>
        /// <value><c>true</c> if [all not in list]; otherwise, <c>false</c>.</value>
		public bool AllNotInList
		{
			get {return _allNotInList;}
			set {_allNotInList = value;}
		}

        /// <summary>
        /// Sort list on this field
        /// Valid values: You must set this in the module constructor.
        /// Must be a existing field in the module core item table.
        /// </summary>
        /// <value>The sort field.</value>
		public string SortField
		{
			get {return _sortField;}
			set {_sortField = value;}
		}

        /// <summary>
        /// Sort Ascending or Descending
        /// Valid values: ASC;DESC
        /// Default value: ASC
        /// </summary>
        /// <value>The sort direction.</value>
		public string SortDirection
		{
			get {return _sortDirection;}
			set {_sortDirection = value;}
		}

        /// <summary>
        /// Search string. An empty string means no search (same as off).
        /// Default value: string.Empty
        /// </summary>
        /// <value>The search string.</value>
		public string SearchString
		{
			get {return _searchString;}
			set {_searchString = value;}
		}

        /// <summary>
        /// Set this if only a single field should be searched e.g.: "Title"
        /// Default value: string.Empty
        /// </summary>
        /// <value>The search field.</value>
		public string SearchField
		{
			get {return _searchField;}
			set {_searchField = value;}
		}

        /// <summary>
        /// Default value: false
        /// </summary>
        /// <value><c>true</c> if [supports workflow]; otherwise, <c>false</c>.</value>
		public bool SupportsWorkflow
		{
			get {return _supportsWorkflow;}
			set {_supportsWorkflow = value;}
		}

        /// <summary>
        /// Default value: Production
        /// </summary>
        /// <value>The workflow version.</value>
		public WorkFlowVersion WorkflowVersion
		{
			get {return _workflowVersion;}
			set {_workflowVersion = value;}
		}

        /// <summary>
        /// The name of the modules core item table e.g. "rb_Links" for module Links
        /// Default value: string.Empty
        /// </summary>
        /// <value>The name of the item table.</value>
		public string ItemTableName
		{
			get {return _itemTableName;}
			set {_itemTableName = value;}
		}

        /// <summary>
        /// The name of the field in the item table that is considered the item title. Typical value is "Title"
        /// Default value: string.Empty
        /// </summary>
        /// <value>The name of the title field.</value>
		public string TitleFieldName
		{
			get {return _titleFieldName;}
			set {_titleFieldName = value;}
		}

        /// <summary>
        /// The list of fields in the SQL select, separated with comma.
        /// NOTE: must be prefixed with itm., e.g.: "itm.ItemID,itm.CreatedByUser,itm.CreatedDate,itm.Title"
        /// Default value: string.Empty
        /// </summary>
        /// <value>The select field list.</value>
		public string SelectFieldList
		{
			get {return _selectFieldList;}
			set {_selectFieldList = value;}
		}

        /// <summary>
        /// Fields to search - must be of nvarchar or ntext type.
        /// NOTE: must be seperated with semicolon, e.g.: "Title;Url;MobileUrl;Description;CreatedByUser"
        /// Default value: string.Empty
        /// </summary>
        /// <value>The search field list.</value>
		public string SearchFieldList
		{
			get {return _searchFieldList;}
			set {_searchFieldList = value;}
		}

        /// <summary>
        /// When true only data for mobile devices are displyed
        /// Default value: false
        /// </summary>
        /// <value><c>true</c> if [mobile only]; otherwise, <c>false</c>.</value>
		public bool MobileOnly
		{
			get {return _mobileOnly;}
			set {_mobileOnly = value;}
		}

        /// <summary>
        /// The portal id
        /// Default value: -1
        /// </summary>
        /// <value>The portal ID.</value>
		public int PortalID
		{
			get {return _portalID;}
			set {_portalID = value;}
		}

        /// <summary>
        /// When the value of UserID is 0 the user has not signed in (it's a guest!)
        /// Default value: -1
        /// </summary>
        /// <value>The user ID.</value>
		public int UserID
		{
			get {return _userID;}
			set {_userID = value;}
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills all MDF settings. Returns true if no problems reading and
        /// parsing all MDF settings.
        /// </summary>
        /// <param name="pmc">The PMC.</param>
        /// <param name="itemTableName">Name of the item table.</param>
        /// <param name="titleFieldName">Name of the title field.</param>
        /// <param name="selectFieldList">The select field list.</param>
        /// <param name="searchFieldList">The search field list.</param>
        /// <returns></returns>
        public bool Populate(PortalModuleControl pmc, string itemTableName, string titleFieldName, string selectFieldList, string searchFieldList)
        {
            bool PopulateDone;
            try
            {
                _applyMDF = bool.Parse(pmc.Settings[NameApplyMDF].ToString());

                string ds = pmc.Settings[NameDataSource].ToString();
                if (ds == DataSourceType.This.ToString())
                    _dataSource = DataSourceType.This;
                else if (ds == DataSourceType.All.ToString())
                    _dataSource = DataSourceType.All;
                else if (ds == DataSourceType.List.ToString())
                    _dataSource = DataSourceType.List;

                _maxHits = int.Parse(pmc.Settings[NameMaxHits].ToString());
                _moduleList = pmc.Settings[NameModuleList].ToString();
                _allNotInList = bool.Parse(pmc.Settings[NameAllNotInList].ToString());
                _sortField = pmc.Settings[NameSortField].ToString();
                _sortDirection = pmc.Settings[NameSortDirection].ToString();
                _searchString = pmc.Settings[NameSearchString].ToString();
                _searchField = pmc.Settings[NameSearchField].ToString();
                _mobileOnly = bool.Parse(pmc.Settings[NameMobileOnly].ToString());

                if (_dataSource == DataSourceType.This)
                    _moduleList = pmc.ModuleID.ToString();

                if (_moduleList == "" && _dataSource == DataSourceType.List)
                {
                    // Create data to lazy user that forgot to enter data in field Module List
                    _moduleList = pmc.ModuleID.ToString();
                }

                if (pmc.SupportsWorkflow)
                {
                    _supportsWorkflow = pmc.SupportsWorkflow;
                    _workflowVersion = pmc.Version;
                }

                _itemTableName = itemTableName;
                _titleFieldName = titleFieldName;
                _selectFieldList = selectFieldList;
                _searchFieldList = searchFieldList;

                _portalID = pmc.PortalID;
                UsersDB u = new UsersDB();
                SqlDataReader dr = u.GetSingleUser(PortalSettings.CurrentUser.Identity.Email);
                if (dr.Read())
                    _userID = Int32.Parse(dr["UserID"].ToString());

                PopulateDone = true;
            }
            catch (Exception)
            {
                PopulateDone = false;
            }
            return PopulateDone;
        }

        /// <summary>
        /// Initializes a new instance of the MDFSetting object with default settings.
        /// </summary>
        public MDFSettings()
        {
            //Nada code! 
        }

        /// <summary>
        /// Initializes a new instance of the MDFSetting object with real settings
        /// </summary>
        /// <param name="pmc">The PMC.</param>
        /// <param name="itemTableName">Name of the item table.</param>
        /// <param name="titleFieldName">Name of the title field.</param>
        /// <param name="selectFieldList">The select field list.</param>
        /// <param name="searchFieldList">The search field list.</param>
        public MDFSettings(PortalModuleControl pmc, string itemTableName, string titleFieldName, string selectFieldList, string searchFieldList)
        {
            Populate(pmc, itemTableName, titleFieldName, selectFieldList, searchFieldList);
        }

        /// <summary>
        /// Retuns true if the module is using MDF.
        /// </summary>
        /// <param name="pmc">The PMC.</param>
        /// <returns>
        /// 	<c>true</c> if [is MDF applied] [the specified PMC]; otherwise, <c>false</c>.
        /// </returns>
        static public bool IsMDFApplied(PortalModuleControl pmc)
        {
            return bool.Parse(pmc.Settings[NameApplyMDF].ToString());
        }

        /// <summary>
        /// Max apply mdf
        /// </summary>
        /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
        /// <returns></returns>
        static public SettingItem MakeApplyMDF(bool defaultValue)
        {
            SettingItem si = new SettingItem(new BooleanDataType());
            si.Group = SettingItemGroup.MDF_SETTINGS;
            si.Order = 1;
            si.Value = defaultValue.ToString();
            si.EnglishName = "Apply MDF";
            si.Description = "Controls if the MDF system is activated or not";
            return si;
        }

        /// <summary>
        /// Make data source
        /// </summary>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        static public SettingItem MakeDataSource(DataSourceType defaultValue)
        {
            SettingItem si = new SettingItem(
                new ListDataType(DataSourceType.This.ToString() + ";" + DataSourceType.All.ToString() + ";" + DataSourceType.List.ToString()));
            si.Group = SettingItemGroup.MDF_SETTINGS;
            si.Order = 2;
            si.Required = true;
            si.Value = defaultValue.ToString();
            si.EnglishName = "DataSource";
            si.Description = "Controls where data displyed in the module is comming from. " +
                "'This' is the current module, 'All' is all modules in the current portal " +
                "and with 'List' you must specify a list of module id's, e.g.: 20242,10243";
            return si;
        }

        /// <summary>
        /// Makes the max hits.
        /// </summary>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        static public SettingItem MakeMaxHits(int defaultValue)
        {
            SettingItem si = new SettingItem(new IntegerDataType());
            si.Group = SettingItemGroup.MDF_SETTINGS;
            si.Order = 3;
            si.Required = true;
            si.Value = defaultValue.ToString();
            si.MinValue = 0;
            si.MaxValue = int.MaxValue;
            si.EnglishName = "Max Hits";
            si.Description = "Represents the number of items returned by MDF. Value 0 means no hit limit (all found items are displayed)";
            return si;
        }

        /// <summary>
        /// Makes the module list.
        /// </summary>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        static public SettingItem MakeModuleList(string defaultValue)
        {
            SettingItem si = new SettingItem(new StringDataType());
            si.Group = SettingItemGroup.MDF_SETTINGS;
            si.Order = 4;
            si.Required = false;
            si.Value = defaultValue;
            si.EnglishName = "Module List";
            si.Description = "Comma separated list of module ID's. e.g.: 20242,10243. Only data for these ID's are listed. (see also AllNotInList!)";
            return si;
        }

        /// <summary>
        /// Makes all not in list.
        /// </summary>
        /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
        /// <returns></returns>
        static public SettingItem MakeAllNotInList(bool defaultValue)
        {
            SettingItem si = new SettingItem(new BooleanDataType());
            si.Group = SettingItemGroup.MDF_SETTINGS;
            si.Order = 5;
            si.Value = defaultValue.ToString();
            si.EnglishName = "All Not In List";
            si.Description = "If DataSource is 'All' or 'List' this can exclude modules listed in Module List";
            return si;
        }

        /// <summary>
        /// Makes the sort field list.
        /// </summary>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="fieldList">The field list.</param>
        /// <returns></returns>
        static public SettingItem MakeSortFieldList(string defaultValue, string fieldList)
        {
            SettingItem si = new SettingItem(new ListDataType(fieldList));
            si.Group = SettingItemGroup.MDF_SETTINGS;
            si.Order = 6;
            si.Required = true;
            si.Value = defaultValue;
            si.EnglishName = "Sort Field";
            si.Description = "A list of all fields from the core item table you want to sort on";
            return si;
        }

        /// <summary>
        /// Makes the sort direction.
        /// </summary>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        static public SettingItem MakeSortDirection(string defaultValue)
        {
            SettingItem si = new SettingItem(new ListDataType("ASC;DESC"));
            si.Group = SettingItemGroup.MDF_SETTINGS;
            si.Order = 7;
            si.Required = true;
            si.Value = defaultValue;
            si.EnglishName = "Sort Direction";
            si.Description = "Sort Ascending or Descending";
            return si;
        }

        /// <summary>
        /// Makes the search string.
        /// </summary>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        static public SettingItem MakeSearchString(string defaultValue)
        {
            SettingItem si = new SettingItem(new StringDataType());
            si.Group = SettingItemGroup.MDF_SETTINGS;
            si.Order = 8;
            si.Required = false;
            si.Value = defaultValue;
            si.EnglishName = "Search string";
            si.Description = "An empty string means no search (same as off)";
            return si;
        }

        /// <summary>
        /// Makes the search field list.
        /// </summary>
        /// <param name="fieldList">The field list.</param>
        /// <returns></returns>
        static public SettingItem MakeSearchFieldList(string fieldList)
        {
            SettingItem si = new SettingItem(new ListDataType("All;" + fieldList));
            si.Group = SettingItemGroup.MDF_SETTINGS;
            si.Order = 9;
            si.Required = true;
            si.Value = "All";
            si.EnglishName = "Search field";
            si.Description = "Search all fields or a single named field in the item record. " +
                "The list of possible search fields are different for different modules";
            return si;
        }

        /// <summary>
        /// Makes the mobile only.
        /// </summary>
        /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
        /// <returns></returns>
        static public SettingItem MakeMobileOnly(bool defaultValue)
        {
            SettingItem si = new SettingItem(new BooleanDataType());
            si.Group = SettingItemGroup.MDF_SETTINGS;
            si.Order = 10;
            si.Value = defaultValue.ToString();
            si.EnglishName = "Mobile Only";
            si.Description = "When true only data for mobile devices are displyed";
            return si;
        }

        /// <summary>
        /// Gets the SQL select.
        /// </summary>
        /// <returns></returns>
        public string GetSqlSelect()
        {
            StringBuilder select = new StringBuilder("", 1000);

            select.Append(" SELECT");
            if (MaxHits > 0)
                select.Append(" TOP " + MaxHits);
            select.Append(" ");
            select.Append(SelectFieldList);
            if (SupportsWorkflow && WorkflowVersion == WorkFlowVersion.Staging)
                select.Append(" FROM " + ItemTableName + "_st itm");
            else
                select.Append(" FROM " + ItemTableName + " itm");

            if (DataSource == DataSourceType.This)
            {
                // Note that there at this point are only one single moduleid in ModuleList!
                select.Append(" WHERE itm.ModuleID = " + ModuleList + "");
            }
            else
            {
                select.Append(", rb_Modules mod, rb_ModuleDefinitions modDef");
                if (_userID > -1)
                    select.Append(", rb_Roles, rb_UserRoles");

                if (DataSource == DataSourceType.List)
                    if (AllNotInList)
                        select.Append(" WHERE itm.ModuleID NOT IN (" + ModuleList + ")");
                    else
                        select.Append(" WHERE itm.ModuleID IN (" + ModuleList + ")");
                else
                    if (AllNotInList)
                        select.Append(" WHERE itm.ModuleID NOT IN (" + ModuleList + ")");
                    else
                        select.Append(" WHERE 1=1");

                if (MobileOnly)
                    select.Append(" AND Mod.ShowMobile=1");

                select.Append(" AND itm.ModuleID = mod.ModuleID");
                select.Append(" AND mod.ModuleDefID = modDef.ModuleDefID");
                select.Append(" AND modDef.PortalID = " + PortalID.ToString());
                if (_userID > -1)
                {
                    select.Append(" AND rb_UserRoles.UserID = " + _userID.ToString());
                    select.Append(" AND rb_UserRoles.RoleID = rb_Roles.RoleID");
                    select.Append(" AND rb_Roles.PortalID = " + _portalID.ToString());
                    select.Append(" AND ((mod.AuthorizedViewRoles LIKE '%All Users%') OR (mod.AuthorizedViewRoles LIKE '%'+rb_Roles.RoleName+'%'))");
                }
                else
                {
                    select.Append(" AND (mod.AuthorizedViewRoles LIKE '%All Users%')");
                }
            }

            // Why this? Because some Rb modules inserts a record containing null value fields! :o(
            select.Append(" AND itm." + TitleFieldName + " IS NOT NULL");

            if (SearchString != "")
            {
                select.Append(" AND (");
                if (SearchField == "All")
                {
                    string[] arrField = _searchFieldList.Split(';');
                    bool firstField = true;
                    foreach (string field in arrField)
                    {
                        if (firstField)
                            firstField = false;
                        else
                            select.Append(" OR ");
                        select.Append("itm." + field + " like '%" + SearchString + "%'");
                    }
                }
                else
                {
                    select.Append("itm." + SearchField + " like '%" + SearchString + "%'");
                }
                select.Append(")");
            }
            select.Append(" ORDER BY itm." + SortField + " " + SortDirection);

            return select.ToString();
        }


        /// <summary>
        /// Get the item list data as a SqlDataReader
        /// </summary>
        /// <returns></returns>
        public SqlDataReader GetDataReader()
        {
            string sqlSelect = GetSqlSelect();
            return DBHelper.GetDataReader(sqlSelect);
        }

        /// <summary>
        /// Get the item list data as a DataSet
        /// </summary>
        /// <returns></returns>
        public DataSet GetDataSet()
        {
            string sqlSelect = GetSqlSelect();
            return DBHelper.GetDataSet(sqlSelect);
        } 
        #endregion
	}
  
}
