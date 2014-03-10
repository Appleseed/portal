using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Xsl;
using Appleseed.Framework.Helpers;
using Appleseed.Framework.Site.Configuration;
using Path=Appleseed.Framework.Settings.Path;

namespace Appleseed.Framework.Web.UI.WebControls
{
    /// <summary>
    /// Summary description for ZenNavigation.
    /// </summary>
    public class ZenNavigation : WebControl //, INavigation
    {
        /// <summary>
        /// 
        /// </summary>
        protected PortalSettings PortalSettings;

        /// <summary>
        /// 
        /// </summary>
        protected XmlDocument PortalPagesXml;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZenNavigation"/> class.
        /// </summary>
        public ZenNavigation()
        {
            EnableViewState = false;
            Load += new EventHandler(LoadControl);
        }

        private string _containerCssClass = string.Empty;

        /// <summary>
        /// Gets or sets the container CSS class.
        /// </summary>
        /// <value>The container CSS class.</value>
        public virtual string ContainerCssClass
        {
            get { return _containerCssClass; }
            set { _containerCssClass = value; }
        }

        //		private string _urlStyle = string.Empty;
        //		public virtual string UrlStyle
        //		{
        //			get{return _urlStyle;}
        //			set{_urlStyle = value;}
        //		}

        private string _xsltFile = "BindAll";

        /// <summary>
        /// Gets or sets the XSLT file.
        /// </summary>
        /// <value>The XSLT file.</value>
        public virtual string XsltFile
        {
            get { return _xsltFile; }
            set { _xsltFile = value; }
        }

        private BindOption _bind = BindOption.BindOptionTop;

        /// <summary>
        /// Gets or sets the bind.
        /// </summary>
        /// <value>The bind.</value>
        public virtual BindOption Bind
        {
            get { return _bind; }
            set { _bind = value; }
        }

        private bool _usePageNameInUrl = true;

        /// <summary>
        /// Gets or sets a value indicating whether [use page name in URL].
        /// </summary>
        /// <value><c>true</c> if [use page name in URL]; otherwise, <c>false</c>.</value>
        public virtual bool UsePageNameInUrl
        {
            get { return _usePageNameInUrl; }
            set { _usePageNameInUrl = value; }
        }

        private bool _usePathTraceInUrl = true;

        /// <summary>
        /// Gets or sets a value indicating whether [use path trace in URL].
        /// </summary>
        /// <value><c>true</c> if [use path trace in URL]; otherwise, <c>false</c>.</value>
        public virtual bool UsePathTraceInUrl
        {
            get { return _usePathTraceInUrl; }
            set { _usePathTraceInUrl = value; }
        }

        /// <summary>
        /// Loads the control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void LoadControl(object sender, EventArgs e)
        {
            // Obtain PortalSettings from Current Context
            PortalSettings = (PortalSettings) HttpContext.Current.Items["PortalSettings"];

            PortalPagesXml = PortalSettings.PortalPagesXml;

            //base.DataBind();
        }

        /// <summary>
        /// Renders the control to the specified HTML writer.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object that receives the control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            // get XSLT stylesheet
            XslCompiledTransform xslDoc = new XslCompiledTransform();
            try
            {
                string myFilePath =
                    HttpContext.Current.Server.MapPath(
                        string.Concat(Path.ApplicationRoot, "/app_support/ZenNavigation/", XsltFile, ".xslt"));
                xslDoc.Load(myFilePath);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Cannot load specified XsltFile: " + e.Message);
            }

            try
            {
                // build parameter list to pass to stylesheet
                XsltArgumentList xslArg = new XsltArgumentList();
                xslArg.AddParam("ActivePageId", "", PortalSettings.ActivePage.PageID);
                xslArg.AddParam("ContainerCssClass", "", ContainerCssClass);
                xslArg.AddParam("UsePageNameInUrl", "", UsePageNameInUrl.ToString().ToLower());
                xslArg.AddParam("UsePathTraceInUrl", "", UsePathTraceInUrl.ToString().ToLower());
                // add the helper object
                XslHelper xslHelper = new XslHelper();
                xslArg.AddExtensionObject("urn:Appleseed", xslHelper);
                // do the transform
                StringWriter result = new StringWriter();
                xslDoc.Transform(PortalPagesXml, xslArg, result);

                string myResult = result.ToString();
                writer.Write(myResult);
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("ZenNavigation :: Failed to transform menu:" + ex.Message);
                writer.Write("ZenNavigation :: Failed to transform menu:" + ex.Message);
            }
        }

        /// <summary>
        /// Cleans the name of the page.
        /// </summary>
        /// <param name="targetPage">The target page.</param>
        /// <returns></returns>
        private string CleanPageName(string targetPage)
        {
            string splitter = ConfigurationManager.AppSettings["HandlerDefaultSplitter"];
            if (splitter == string.Empty || splitter == null)
                splitter = "__";
            targetPage = Regex.Replace(targetPage, @"[\.\$\^\{\[\(\|\)\*\+\?!'""]", splitter);
            targetPage = targetPage.Replace(" ", splitter).ToLower();
            return targetPage;
        }

        #region INavigation implementation

        //		private BindOption _bind = BindOption.BindOptionTop; 
        private int _definedParentTab = -1;
        //		private bool _autoBind = true; 
        //		/// <summary> 
        //		/// Indicates if control should bind when loads 
        //		/// </summary> 
        //		[ 
        //		Category("Data"), 
        //		PersistenceMode(PersistenceMode.Attribute) 
        //		] 
        //		public bool AutoBind 
        //		{ 
        //			get{return _autoBind;} 
        //			set{_autoBind = value;} 
        //		} 
        //
        //		/// <summary> 
        //		/// Describes how this control should bind to db data 
        //		/// </summary> 
        //		[ 
        //		Category("Data"), 
        //		PersistenceMode(PersistenceMode.Attribute) 
        //		] 
        //		public BindOption Bind 
        //		{ 
        //			get {return _bind;} 
        //			set 
        //			{ 
        //				if(_bind != value) 
        //				{ 
        //					_bind = value; 
        //				} 
        //			} 
        //		} 
        /// <summary>
        /// defines the parentTabID when using BindOptionDefinedParent
        /// </summary>
        /// <value>The parent tab ID.</value>
        [
            Category("Data"),
                PersistenceMode(PersistenceMode.Attribute)
            ]
        public int ParentTabID
        {
            get { return _definedParentTab; }
            set
            {
                if (_definedParentTab != value)
                {
                    _definedParentTab = value;
                }
            }
        }

        #endregion
    }
}