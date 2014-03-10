using System;
using System.Collections;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Appleseed.Framework.Security;
using Appleseed.Framework.Settings;
using Appleseed.Framework.Site.Configuration;

namespace Appleseed.Framework.Web.UI.WebControls
{
    /// <summary>
    /// ZenContent class, supports ZenLayout
    /// </summary>
    public class ZenContent : WebControl, INamingContainer
    {
        private ArrayList innerDataSource;
        private bool _autoBind = true;
        private string _content;

        /// <summary>
        /// Constructor
        /// </summary>
        public ZenContent()
        {
            Load += new EventHandler(LoadControl);
        }

        /// <summary>
        /// Loads control
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public void LoadControl(object sender, EventArgs e)
        {
            if (AutoBind)
            {
                DataBind();
            }
        }

        /// <summary>
        /// Indicates if control should bind when it loads
        /// </summary>
        /// <value><c>true</c> if [auto bind]; otherwise, <c>false</c>.</value>
        public bool AutoBind
        {
            get { return _autoBind; }
            set { _autoBind = value; }
        }

        /// <summary>
        /// The DataSource
        /// </summary>
        /// <value>The data source.</value>
        public ArrayList DataSource
        {
            get
            {
                if (innerDataSource == null)
                {
                    InitializeDataSource();
                }
                return innerDataSource;
            }
        }

        /// <summary>
        /// The layout position for this content
        /// </summary>
        /// <value>The content.</value>
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        /// <summary>
        /// Binds a data source to the invoked server control and all its child controls
        /// </summary>
        public override void DataBind()
        {
            foreach (Control c in DataSource)
                Controls.Add(c);
        }

        /// <summary>
        /// Initialize internal data source
        /// </summary>
        public void InitializeDataSource()
        {
            innerDataSource = new ArrayList();

            // Obtain PortalSettings from Current Context
            PortalSettings PortalSettings = (PortalSettings) HttpContext.Current.Items["PortalSettings"];

            // Loop through each entry in the configuration system for this tab
            // Ensure that the visiting user has access to view the module
            foreach (ModuleSettings _moduleSettings in PortalSettings.ActivePage.Modules)
            {
                if (_moduleSettings.PaneName.ToLower() == Content.ToLower()
                    && PortalSecurity.IsInRoles(_moduleSettings.AuthorizedViewRoles))
                {
                    //Cache. If == 0 then override with default cache in web.config
                    if (ConfigurationManager.AppSettings["ModuleOverrideCache"] != null
                        && !_moduleSettings.Admin
                        && _moduleSettings.CacheTime == 0)
                    {
                        int mCache = Int32.Parse(ConfigurationManager.AppSettings["ModuleOverrideCache"]);
                        if (mCache > 0)
                            _moduleSettings.CacheTime = mCache;
                    }

                    // added security settings to condition test so that a user who has 
                    // edit or properties permission will not cause the module output to be cached. 
                    if (
                        ((_moduleSettings.CacheTime) <= 0)
                        || (PortalSecurity.HasEditPermissions(_moduleSettings.ModuleID))
                        || (PortalSecurity.HasPropertiesPermissions(_moduleSettings.ModuleID))
                        || (PortalSecurity.HasAddPermissions(_moduleSettings.ModuleID))
                        || (PortalSecurity.HasDeletePermissions(_moduleSettings.ModuleID))
                        )
                    {
                        try
                        {
                            string portalModuleName =
                                string.Concat(Path.ApplicationRoot, "/", _moduleSettings.DesktopSrc);
                            PortalModuleControl portalModule = (PortalModuleControl) Page.LoadControl(portalModuleName);

                            portalModule.PortalID = PortalSettings.PortalID;
                            portalModule.ModuleConfiguration = _moduleSettings;

                            //TODO: This is not the best place: should be done early
                            if ((portalModule.Cultures != null && portalModule.Cultures.Length == 0) ||
                                (portalModule.Cultures + ";").IndexOf(PortalSettings.PortalContentLanguage.Name + ";") >=
                                0)
                                innerDataSource.Add(portalModule);
                        }
                        catch (Exception ex)
                        {
                            ErrorHandler.Publish(LogLevel.Error,
                                                 "ZenLayout: Unable to load control '" + _moduleSettings.DesktopSrc +
                                                 "'!", ex);
                            innerDataSource.Add(
                                new LiteralControl("<br><span class=\"NormalRed\">" +
                                                   "ZenLayout: Unable to load control '" + _moduleSettings.DesktopSrc +
                                                   "'!"));
                        }
                    }
                    else
                    {
                        try
                        {
                            CachedPortalModuleControl portalModule = new CachedPortalModuleControl();

                            portalModule.PortalID = PortalSettings.PortalID;
                            portalModule.ModuleConfiguration = _moduleSettings;

                            innerDataSource.Add(portalModule);
                        }
                        catch (Exception ex)
                        {
                            ErrorHandler.Publish(LogLevel.Error,
                                                 "ZenLayout: Unable to load cached control '" +
                                                 _moduleSettings.DesktopSrc + "'!", ex);
                            innerDataSource.Add(
                                new LiteralControl("<br><span class=\"NormalRed\">" +
                                                   "ZenLayout: Unable to load cached control '" +
                                                   _moduleSettings.DesktopSrc + "'!"));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This member overrides Control.OnDataBinding
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnDataBinding(EventArgs e)
        {
            EnsureChildControls();
            base.OnDataBinding(e);
        }

        /// <summary>
        /// This member overrides Control.Render
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object that receives the control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            RenderContents(writer);
        }
    }
}