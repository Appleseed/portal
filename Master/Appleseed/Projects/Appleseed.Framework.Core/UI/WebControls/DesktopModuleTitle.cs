using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.UI;

namespace Appleseed.Framework.Web.UI.WebControls
{
    /// <summary>
    /// New 'stub' class for backward compatibility. Modules that add the ModuleTitle in 
    /// their init event will be adding this. New PortalModuleControl class pulls needed 
    /// values from this class. New modules should not use this class at all, setting 
    /// title properties directly on PortalModuleContol instead. Jes1111.
    /// </summary>
    [
        DefaultProperty("Title"),
            ToolboxData("<{0}:DesktopModuleTitle runat=server></{0}:DesktopModuleTitle>"),
            Designer("Appleseed.Framework.UI.Design.DesktopModuleTitleDesigner")
        ]
    public class DesktopModuleTitle : Control // WebControl - only needs generic Control now
    {
        /// <summary>
        /// Constructor
        /// </summary>
        [Obsolete("Use the corresponding properties in PortalModuleControl")]
        public DesktopModuleTitle()
        {
            EnableViewState = false; // No need for viewstate
        }

        /// <summary>
        /// Init Event 
        /// </summary>
        protected override void OnInit(EventArgs e)
        {
            Visible = false;
        }

        // GG: added 08/04/2004 by groskrg@versifit.com	to support custom buttons in the title bar
        private ArrayList _CustomButtons = new ArrayList(3);

        /// <summary>
        /// CustomButtons class allows modules to add their own buttons from Code.
        /// </summary>
        public ArrayList CustomButtons
        {
            get { return _CustomButtons; }
            set { _CustomButtons = value; }
        }

        private string _EditText = "EDIT";
        private string _EditUrl = string.Empty;
        private string _EditTarget = string.Empty;

        /// <summary>
        /// Text for Edit Link
        /// </summary>
        public string EditText
        {
            get { return _EditText; }
            set { _EditText = value; }
        }

        /// <summary>
        /// Url for Edit Link
        /// </summary>
        public string EditUrl
        {
            get { return _EditUrl; }
            set { _EditUrl = value; }
        }

        /// <summary>
        /// Target frame/page for Edit Link
        /// </summary>
        public string EditTarget
        {
            get { return _EditTarget; }
            set { _EditTarget = value; }
        }

        private string _AddText = "ADD";
        private string _AddUrl = string.Empty;
        private string _AddTarget = string.Empty;

        /// <summary>
        /// Text for Add Link
        /// </summary>
        public string AddText
        {
            get { return _AddText; }
            set { _AddText = value; }
        }

        /// <summary>
        /// Url for Add Link
        /// </summary>
        public string AddUrl
        {
            get { return _AddUrl; }
            set { _AddUrl = value; }
        }

        /// <summary>
        /// Target frame/page for Add Link
        /// </summary>
        public string AddTarget
        {
            get { return _AddTarget; }
            set { _AddTarget = value; }
        }

        private string _PropertiesText = "PROPERTIES";
        private string _PropertiesUrl = "~/DesktopModules/CoreModules/Admin/PropertyPage.aspx";
        private string _PropertiesTarget = string.Empty;

        /// <summary>
        /// Text for Properties Link
        /// </summary>
        public string PropertiesText
        {
            get { return _PropertiesText; }
            set { _PropertiesText = value; }
        }

        /// <summary>
        /// Url for Properties Link
        /// </summary>
        public string PropertiesUrl
        {
            get { return _PropertiesUrl; }
            set { _PropertiesUrl = value; }
        }

        /// <summary>
        /// Target frame/page for Properties Link
        /// </summary>
        public string PropertiesTarget
        {
            get { return _PropertiesTarget; }
            set { _PropertiesTarget = value; }
        }

        private string _SecurityText = "SECURITY";
        private string _SecurityUrl = "~/DesktopModules/CoreModules/Admin/ModuleSettings.aspx";
        private string _SecurityTarget = string.Empty;

        /// <summary>
        /// Text for Security Link
        /// </summary>
        public string SecurityText
        {
            get { return _SecurityText; }
            set { _SecurityText = value; }
        }

        /// <summary>
        /// Url for Security Link
        /// </summary>
        public string SecurityUrl
        {
            get { return _SecurityUrl; }
            set { _SecurityUrl = value; }
        }

        /// <summary>
        /// Target frame/page for Security Link
        /// </summary>
        public string SecurityTarget
        {
            get { return _SecurityTarget; }
            set { _SecurityTarget = value; }
        }


        private string title = string.Empty;

        /// <summary>
        /// Title
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private int _tabID = 0;

        /// <summary>
        /// Stores current linked module ID if applicable
        /// </summary>
        public int PageID
        {
            get
            {
                if (_tabID == 0)
                {
                    // Determine PageID if specified
                    if (HttpContext.Current != null && HttpContext.Current.Request.Params["PageID"] != null)
                        _tabID = Int32.Parse(HttpContext.Current.Request.Params["PageID"]);
                    else if (HttpContext.Current != null && HttpContext.Current.Request.Params["TabID"] != null)
                        _tabID = Int32.Parse(HttpContext.Current.Request.Params["TabID"]);
                }
                return _tabID;
            }
        }
    }
}