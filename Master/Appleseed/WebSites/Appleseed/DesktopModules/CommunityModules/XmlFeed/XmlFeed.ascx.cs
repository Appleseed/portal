// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlFeed.ascx.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   XmlFeed Module
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Content.Web.Modules
{
    using System;
    using System.IO;
    using System.Net;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Xml;

    using Appleseed.Framework;
    using Appleseed.Framework.DataTypes;
    using Appleseed.Framework.Settings;
    using Appleseed.Framework.Web.UI.WebControls;

    using Path = Appleseed.Framework.Settings.Path;
    using System.Xml.XPath;

    /// <summary>
    /// XmlFeed Module
    /// </summary>
    public partial class XmlFeed : PortalModuleControl
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "XmlFeed" /> class.
        /// </summary>
        public XmlFeed()
        {
            const SettingItemGroup Group = SettingItemGroup.MODULE_SPECIAL_SETTINGS;
            const int GroupOrderBase = (int)SettingItemGroup.MODULE_SPECIAL_SETTINGS;

            var xmlSrcType = new SettingItem<string, ListControl>(new ListDataType<string, ListControl>("URL;File"))
                {
                   Required = true, Value = "URL", Group = Group, Order = GroupOrderBase + 1 
                };
            this.BaseSettings.Add("XML Type", xmlSrcType);

            var xmlSrcUrl = new SettingItem<Uri, TextBox>(new UrlDataType())
                {
                   Required = false, Group = Group, Order = GroupOrderBase + 2 
                };
            this.BaseSettings.Add("XML URL", xmlSrcUrl);

            var xmlSrcFile = new SettingItem<string, TextBox>(new PortalUrlDataType())
                {
                   Required = false, Group = Group, Order = GroupOrderBase + 3 
                };
            this.BaseSettings.Add("XML File", xmlSrcFile);

            var xslSrcType = new SettingItem<string, ListControl>(new ListDataType<string, ListControl>("Predefined;File"))
                {
                   Required = true, Value = "Predefined", Order = GroupOrderBase + 4, Group = Group 
                };
            this.BaseSettings.Add("XSL Type", xslSrcType);

            var xsltFileList = new ListDataType<string, ListControl>(this.GetXSLListForFeedTransformations());
            var xslSrcPredefined = new SettingItem<string, ListControl>(xsltFileList)
                {
                   Required = true, Value = "RSS91", Group = Group, Order = GroupOrderBase + 5 
                };
            this.BaseSettings.Add("XSL Predefined", xslSrcPredefined);

            var xslSrcFile = new SettingItem<string, TextBox>(new PortalUrlDataType())
                {
                   Required = false, Group = Group, Order = GroupOrderBase + 6 
                };
            this.BaseSettings.Add("XSL File", xslSrcFile);

            var timeout = new SettingItem<int, TextBox>()
                {
                   Required = true, Group = Group, Order = GroupOrderBase + 7, Value = 15 
                };
            this.BaseSettings.Add("Timeout", timeout);
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
                return new Guid("{2502DB18-B580-4F90-8CB4-C15E6E531020}");
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Author: Joe Audette
        ///   Added:  7/31/2003
        ///   Allows you to add style sheets for new feed formats without recompiling.
        ///   Any XSLT style sheets placed in the folder specified in the web.config will show up
        ///   in the dropdown list
        ///   Patch 11/11/2003 by Manu: Errors are logged.
        /// </summary>
        /// <returns>
        /// FileInfo[]
        /// </returns>
        public FileInfo[] GetXSLListForFeedTransformations()
        {
            // jes1111 - if (ConfigurationSettings.AppSettings["XMLFeedXSLFolder"] != null && ConfigurationSettings.AppSettings["XMLFeedXSLFolder"].Length > 0)
            // true then jes1111 - xsltPath = ConfigurationSettings.AppSettings["XMLFeedXSLFolder"];
            // false then this will default to the xml feed folder where the .xslt files are located by default
            var xsltPath = Config.XMLFeedXSLFolder.Length != 0
                               ? Config.XMLFeedXSLFolder
                               : HttpContext.Current.Server.MapPath(this.TemplateSourceDirectory);

            try
            {
                if (Directory.Exists(xsltPath))
                {
                    var dir = new DirectoryInfo(xsltPath);
                    return dir.GetFiles("*.xslt");
                }

                ErrorHandler.Publish(LogLevel.Warn, string.Format("Default XSLT location not found: '{0}'", xsltPath));
            }
            catch (Exception ex)
            {
                ErrorHandler.Publish(LogLevel.Error, string.Format("XSLT location not found: {0}", xsltPath), ex);
            }

            return null;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">
        /// The <see cref="T:System.EventArgs"/> object that contains the event data.
        /// </param>
        /// <remarks>
        /// The Page_Load event handler on this User Control obtains
        ///   an xml document and xsl/t transform file location.
        ///   It then sets these properties on an &lt;asp:Xml&gt; server control.
        ///   Patch 11/11/2003 by Manu: Errors are logged.
        /// </remarks>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            var xmlsrcType = this.Settings["XML Type"].ToString();
            var xmlsrc = xmlsrcType == "File"
                             ? this.Settings["XML File"].ToString()
                             : this.Settings["XML URL"].ToString();

            var xslsrcType = this.Settings["XSL Type"].ToString();
            var xslsrc = xslsrcType == "File"
                             ? this.Settings["XSL File"].ToString()
                             : this.Settings["XSL Predefined"].ToString();

            // Timeout
            var timeout = int.Parse(this.Settings["Timeout"].ToString());

            if (xmlsrc.Length != 0)
            {
                if (xmlsrcType == "File")
                {
                    var pathXml = new PortalUrlDataType { Value = xmlsrc };
                    xmlsrc = pathXml.FullPath;

                    if (File.Exists(this.Server.MapPath(xmlsrc)))
                    {
                        this.xml1.DocumentSource = xmlsrc;
                    }
                    else
                    {
                        this.Controls.Add(
                            new LiteralControl(
                                string.Format("<br /><div class='error'>File {0} not found.<br /></div>", xmlsrc)));
                    }
                }
                else
                {
                    try
                    {
                        ErrorHandler.Publish(
                            LogLevel.Warn, 
                            string.Format("XMLFeed - This should not done more than once in 30 minutes: '{0}'", xmlsrc));

                        // handle on the remote resource
                        var wr = (HttpWebRequest)WebRequest.Create(xmlsrc);

                        // jes1111 - not needed: global proxy is set in Global class Application Start
                        // if (ConfigurationSettings.AppSettings.Get("UseProxyServerForServerWebRequests") == "true")
                        // wr.Proxy = PortalSettings.GetProxy();

                        // set the HTTP properties
                        wr.Timeout = timeout * 1000; // milliseconds to seconds

                        // Read the response
                        var resp = wr.GetResponse();

                        // Stream read the response
                        var stream = resp.GetResponseStream();
                        if (stream != null)
                        {
                            // Read XML data from the stream
                            // ignore the DTD (resolver null)
                            var reader = new XmlTextReader(stream) { XmlResolver = null };

                            // Create a new document object
                           // var doc = new XmlDocument();

                            // Create the content of the XML Document from the XML data stream
                           // doc.Load(reader);

                            // the XML control to hold the generated XML document
                            //this.xml1.Document = doc;
                            this.xml1.DocumentContent = (new XPathDocument(reader)).CreateNavigator().OuterXml;	
                        }
                    }
                    catch (Exception ex)
                    {
                        // connectivity issues
                        this.Controls.Add(
                            new LiteralControl(
                                string.Format(
                                    "<br /><div class='error'>Error loading: {0}.<br />{1}</div>", xmlsrc, ex.Message)));
                        ErrorHandler.Publish(LogLevel.Error, string.Format("Error loading: {0}.", xmlsrc), ex);
                    }
                }
            }

            if (xslsrcType == "File")
            {
                var pathXsl = new PortalUrlDataType { Value = xslsrc };
                xslsrc = pathXsl.FullPath;
            }
            else
            {
                // if (ConfigurationSettings.AppSettings.Get("XMLFeedXSLFolder") != null)
                // {
                // if (ConfigurationSettings.AppSettings.Get("XMLFeedXSLFolder").ToString().Length > 0)
                //     xslsrc = ConfigurationSettings.AppSettings.Get("XMLFeedXSLFolder").ToString() + xslsrc;
                // else
                //     xslsrc = "~/DesktopModules/CommunityModules/XmlFeed/" + xslsrc;
                // }
                // else
                // {
                //     xslsrc = "~/DesktopModules/CommunityModules/XmlFeed/" + xslsrc;
                // }
                xslsrc =
                    Path.WebPathCombine(
                        Config.XMLFeedXSLFolder.Length == 0 ? this.TemplateSourceDirectory : Config.XMLFeedXSLFolder, 
                        xslsrc);

                if (!xslsrc.EndsWith(".xslt"))
                {
                    xslsrc += ".xslt";
                }
            }

            if (!string.IsNullOrEmpty(xslsrc))
            {
                if (File.Exists(this.Server.MapPath(xslsrc)))
                {
                    this.xml1.TransformSource = xslsrc;
                }
                else
                {
                    this.Controls.Add(
                        new LiteralControl(
                            string.Format("<br /><div class='error'>File {0} not found.<br /></div>", xslsrc)));
                }
            }
        }

        #endregion
    }
}