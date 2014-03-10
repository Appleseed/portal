using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using Appleseed.Framework.Site.Configuration;

namespace Appleseed.Framework.Web.UI.WebControls
{
    /// <summary>
    /// The CachedPortalModuleControl class is a custom server control that
    /// the Portal framework uses to optionally enable output caching of 
    /// individual portal module's content.<br />
    /// If a CacheTime value greater than 0 seconds is specified within the 
    /// Portal.Config configuration file, then the CachePortalModuleControl
    /// will automatically capture the output of the Portal Module User Control
    /// it wraps. It will then store this captured output within the ASP.NET
    /// Cache API. On subsequent requests (either by the same browser -- or
    /// by other browsers visiting the same portal page), the CachedPortalModuleControl
    /// will attempt to resolve the cached output out of the cache.
    /// </summary>
    /// <remarks>
    /// In the event that previously cached output can't be found in the
    /// ASP.NET Cache, the CachedPortalModuleControl will automatically instatiate
    /// the appropriate portal module user control and place it within the
    /// portal page.
    /// </remarks>
    [History("Jes1111", "2003/04/24", "Added PortalAlias to cachekey")]
    [History("Jes1111", "2003/04/24", "Improved cache behaviour for CacheTime=-1")]
    public class CachedPortalModuleControl : Control
    {
        // Private field variables
        private ModuleSettings _moduleConfiguration;
        private string _cachedOutput = string.Empty;
        private int _portalID = 0;

        /// <summary>
        /// ModuleConfiguration
        /// </summary>
        /// <value>The module configuration.</value>
        public ModuleSettings ModuleConfiguration
        {
            get { return _moduleConfiguration; }
            set { _moduleConfiguration = value; }
        }

        /// <summary>
        /// ModuleID
        /// </summary>
        /// <value>The module ID.</value>
        public int ModuleID
        {
            get { return _moduleConfiguration.ModuleID; }
        }

        /// <summary>
        /// PortalID
        /// </summary>
        /// <value>The portal ID.</value>
        public int PortalID
        {
            get { return _portalID; }
            set { _portalID = value; }
        }

        /// <summary>
        /// The CacheKey property is used to calculate a "unique" cache key
        /// entry to be used to store/retrieve the portal module's content
        /// from the ASP.NET Cache.
        /// </summary>
        /// <value>The cache key.</value>
        public string CacheKey
        {
            get
            {
                // Change 8/April/2003 Jes1111
                // changes to Language behaviour require addition of culture names to cache key
                // Jes1111 - 24/April/2003 - added portal alias to cachekey (to facilitate identification
                // when examining cache contents)
                PortalSettings PortalSettings = (PortalSettings) HttpContext.Current.Items["PortalSettings"];
                StringBuilder sb = new StringBuilder();
                sb.Append("rb_");
                sb.Append(PortalSettings.PortalAlias.ToLower());
                sb.Append("_mid");
                sb.Append(ModuleID.ToString());
                sb.Append("[");
                sb.Append(PortalSettings.PortalContentLanguage);
                sb.Append("+");
                sb.Append(PortalSettings.PortalUILanguage);
                sb.Append("+");
                sb.Append(PortalSettings.PortalDataFormattingCulture);
                sb.Append("]");

                return sb.ToString();
            }
        }

        /// <summary>
        /// The CreateChildControls method is called when the ASP.NET Page Framework
        /// determines that it is time to instantiate a server control.<br/>
        /// The CachedPortalModuleControl control overrides this method and attempts
        /// to resolve any previously cached output of the portal module from the ASP.NET cache.
        /// If it doesn't find cached output from a previous request, then the
        /// CachedPortalModuleControl will instantiate and add the portal module's
        /// User Control instance into the page tree.
        /// </summary>
        protected override void CreateChildControls()
        {
            // Attempt to resolve previously cached content from the ASP.NET Cache
            if (_moduleConfiguration.CacheTime > 0)
            {
                _cachedOutput = (string) Context.Cache[CacheKey];
            }

            // If no cached content is found, then instantiate and add the portal
            // module user control into the portal's page server control tree
            if (_cachedOutput == null)
            {
                base.CreateChildControls();

                PortalModuleControl module = (PortalModuleControl) Page.LoadControl(this._moduleConfiguration.DesktopSrc);

                module.ModuleConfiguration = ModuleConfiguration;
                module.PortalID = PortalID;

                Controls.Add(module);
            }
        }

        /// <summary>
        /// The Render method is called when the ASP.NET Page Framework
        /// determines that it is time to render content into the page output stream.
        /// The CachedPortalModuleControl control overrides this method and captures
        /// the output generated by the portal module user control. It then
        /// adds this content into the ASP.NET Cache for future requests.
        /// </summary>
        /// <param name="output">The output.</param>
        protected override void Render(HtmlTextWriter output)
        {
            // If no caching is specified, render the child tree and return 
            //if (_moduleConfiguration.CacheTime == 0) // Jes1111
            if (_moduleConfiguration.CacheTime <= 0)
            {
                base.Render(output);
                return;
            }

            // If no cached output was found from a previous request, render
            // child controls into a TextWriter, and then cache the results
            // in the ASP.NET Cache for future requests.
            if (_cachedOutput == null)
            {
                using (TextWriter tempWriter = new StringWriter())
                {
                    base.Render(new HtmlTextWriter(tempWriter));
                    _cachedOutput = tempWriter.ToString();
                }

                // change 28/Feb/2003 - Jeremy Esland - Cache
                // added file dependencies for cache insert
                if (this._moduleConfiguration.CacheDependency != null)
                {
                    string[] dependencyList = new string[this._moduleConfiguration.CacheDependency.Count];
                    int i = 0;
                    foreach (string thisfile in this._moduleConfiguration.CacheDependency)
                    {
                        dependencyList[i] = thisfile;
                        i++;
                    }
                    using (CacheDependency _cacheDependency = new CacheDependency(dependencyList))
                    {
                        Context.Cache.Insert(CacheKey, _cachedOutput, _cacheDependency,
                                             DateTime.Now.AddSeconds(_moduleConfiguration.CacheTime), TimeSpan.Zero);
                    }
                    Debug.WriteLine("************** Insert Render1" + CacheKey);
                }
                else
                {
                    Context.Cache.Insert(CacheKey, _cachedOutput, null,
                                         DateTime.Now.AddSeconds(_moduleConfiguration.CacheTime), TimeSpan.Zero);
                    Debug.WriteLine("************** Insert Render2" + CacheKey);
                }
            }

            // Output the user control's content
            output.Write(_cachedOutput);
        }
    }
}