// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlModule.ascx.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   Xml Module
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Content.Web.Modules
{
    using System;
    using System.IO;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Appleseed.Framework;
    using Appleseed.Framework.DataTypes;
    using Appleseed.Framework.Web.UI.WebControls;
    using System.Net;
    using System.Xml;
    using System.Xml.Xsl;

    /// <summary>
    /// Xml Module
    /// </summary>
    public partial class XmlModule : PortalModuleControl
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "XmlModule" /> class.
        /// </summary>
        public XmlModule()
        {
            var xmlSrc = new SettingItem<string, TextBox>(new PortalUrlDataType()) { Required = true, Order = 1 };
            this.BaseSettings.Add("XMLsrc", xmlSrc);

            var xslSrc = new SettingItem<string, TextBox>(new PortalUrlDataType()) { Required = true, Order = 2 };
            this.BaseSettings.Add("XSLsrc", xslSrc);
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
                return new Guid("{BE224332-03DE-42B7-B127-AE1F1BD0FADC}");
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The Page_Load event handler on this User Control uses
        ///   the Portal configuration system to obtain an xml document
        ///   and xsl/t transform file location.  It then sets these
        ///   properties on an &lt;asp:Xml&gt; server control.
        /// </summary>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var xmlSetting = this.Settings["XMLsrc"].ToString();

            if (xmlSetting.StartsWith("http://") || xmlSetting.StartsWith("https://")) {
                try {
                    var wr = (HttpWebRequest)WebRequest.Create(xmlSetting);

                    // set the HTTP properties
                    wr.Timeout = 300 * 1000; // milliseconds to seconds

                    // Read the response
                    var resp = wr.GetResponse();

                    // Stream read the response
                    var stream = resp.GetResponseStream();
                    if (stream != null) {
                        // Read XML data from the stream
                        // ignore the DTD (resolver null)
                        var reader = new XmlTextReader(stream) { XmlResolver = null };

                        // Create a new document object
                        var doc = new XmlDocument();

                        // Create the content of the XML Document from the XML data stream
                        doc.Load(reader);

                        // the XML control to hold the generated XML document
                        this.xml1.Document = doc; 
                    }
                }
                catch (Exception ex) {
                    this.Controls.Add(
                            new LiteralControl(
                                string.Format(
                                    "<br /><span class='Error'>{0}<br />",
                                    General.GetString("FILE_NOT_FOUND").Replace("%1%", xmlSetting))));
                    ErrorHandler.Publish(LogLevel.Error, ex);
                }
            }
            else {
                var pt = new PortalUrlDataType { Value = this.Settings["XMLsrc"].ToString() };
                var xmlsrc = pt.FullPath;

                if (!string.IsNullOrEmpty(xmlsrc)) {
                    if (File.Exists(this.Server.MapPath(xmlsrc))) {
                        this.xml1.DocumentSource = xmlsrc;

                        // Change - 28/Feb/2003 - Jeremy Esland
                        // Builds cache dependency files list
                        this.ModuleConfiguration.CacheDependency.Add(this.Server.MapPath(xmlsrc));
                    }
                    else {
                        this.Controls.Add(
                            new LiteralControl(
                                string.Format(
                                    "<br /><span class='Error'>{0}<br />",
                                    General.GetString("FILE_NOT_FOUND").Replace("%1%", xmlsrc))));
                    }
                }
            }

            var xslSetting = this.Settings["XSLsrc"].ToString();
            if (xslSetting.StartsWith("http://") || xslSetting.StartsWith("https://")) {
                try {                    

                    var t = new XslTransform();
                    t.Load(xslSetting);
                    xml1.Transform = t;

                }
                catch (Exception ex) {

                    this.Controls.Add(
                        new LiteralControl(
                            string.Format(
                                "<br /><span class='Error'>{0}<br />",
                                General.GetString("FILE_NOT_FOUND").Replace("%1%", xslSetting))));
                    ErrorHandler.Publish(LogLevel.Error, ex);
                }
            
            
            
            }
            else {
                var pt = new PortalUrlDataType { Value = this.Settings["XSLsrc"].ToString() };
                var xslsrc = pt.FullPath;

                if (string.IsNullOrEmpty(xslsrc)) {
                    return;
                }

                if (File.Exists(this.Server.MapPath(xslsrc))) {
                    this.xml1.TransformSource = xslsrc;

                    // Change - 28/Feb/2003 - Jeremy Esland
                    // Builds cache dependency files list
                    this.ModuleConfiguration.CacheDependency.Add(this.Server.MapPath(xslsrc));
                }
                else {
                    this.Controls.Add(
                        new LiteralControl(
                            string.Format(
                                "<br /><span class='Error'>{0}<br />",
                                General.GetString("FILE_NOT_FOUND").Replace("%1%", xslsrc))));
                }
            }
        }

        #endregion
    }
}