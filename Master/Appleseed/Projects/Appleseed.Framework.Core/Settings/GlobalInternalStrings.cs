	//===============================================================================
	//
	// Globalized Internal Strings (Portal Related )
	//	
	//
	//===============================================================================
	//
	// This class holds commonly used internal strings. Place strings, constants, etc.
	// here to better reuse and manage the data.
	//===============================================================================



namespace Appleseed.Framework
{
	/// <summary>
	/// Summary description for GlobalInternalStrings.
	/// </summary>
	sealed public class /* struct*/ GlobalInternalStrings
	{
	    /// <summary>
		/// The role of all  users
		/// </summary>
		public static readonly string AllUsers = "All Users";
		/// <summary>
		/// The name of the admin role
		/// </summary>
		public static readonly string Admins ="Admins";
		/// <summary>
		/// The Persistent/DB delimter
		/// </summary>
		public static readonly string RoleDelimiter = ";";
		/// <summary>
		/// Anonymous user name
		/// </summary>
		public static readonly string Anonymous = "anonymous";
		/// <summary>
		/// the portal prefix
		/// </summary>
		public static readonly string PortalPrefix = "Appleseed_";
		/// <summary>
		/// the cookie id used for getting the uid ( dependency above !)
		/// </summary>
        public static readonly string UserWinMgmtIndex = "Appleseed_WinMgmt";
		/// <summary>
		/// the cookie path used for window informaton ( dependency above !)
		/// </summary>
		public static readonly string CookiePath  = "/" ; 
		/// <summary>
		/// the CVS directory
		/// </summary>
		public static readonly string CVS = "CVS";
		/// <summary>
		/// left pane
		/// </summary>
		public static readonly string LeftPane = "leftpane";
		/// <summary>
		/// right pane
		/// </summary>
		public static readonly string RightPane = "rightpane";
		/// <summary>
		/// context pane
		/// </summary>
		public static readonly string ContentPane = "contentpane";
		/// <summary>
		/// header pane [FUTURE?]
		/// </summary>
		public static readonly string HeaderPane = "headerpane";
		/// <summary>
		/// footer pane [FUTURE?]
		/// </summary>
		public static readonly string FooterPane = "footerpane";
		/// <summary>
		/// current supported panes
		/// </summary>
		public static string []CurrentPanes = {LeftPane,ContentPane,RightPane};
		/// <summary>
		/// Standardize text PageID
		/// </summary>
		public static readonly string str_PageID = "PageID";
		/// <summary>
		/// Support for old TabID
		/// </summary>
		public static readonly string str_TabID = "TabID";
		/// <summary>
		/// Non-breaking space
		/// </summary>
		public  const string HTML_SPACE = "&nbsp;";


	} // end of GlobalInternalStrings
}