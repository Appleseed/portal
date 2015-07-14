// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserDefinedTableXSL.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   The user defined table xml.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Content.Web.Modules
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.IO;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Xml;

    using Appleseed.Framework;

    using Path = Appleseed.Framework.Settings.Path;
    using System.Xml.XPath;

    /// <summary>
    /// The user defined table xml.
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:UserDefinedTableXSLctl runat=server></{0}:UserDefinedTableXSLctl>")]
    public class UserDefinedTableXML : Xml, IPostBackEventHandler
    {
        #region Constants and Fields

        /// <summary>
        ///   The data view.
        /// </summary>
        private readonly DataView dv;

        /// <summary>
        ///   The page id.
        /// </summary>
        private readonly int pageId;

        /// <summary>
        ///   The show detail id.
        /// </summary>
        private int showdetailId;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDefinedTableXML"/> class.
        /// </summary>
        /// <param name="xmlDataview">
        /// The xml data view.
        /// </param>
        /// <param name="pageId">
        /// The page id.
        /// </param>
        /// <param name="moduleId">
        /// The module id.
        /// </param>
        /// <param name="editable">
        /// The editable.
        /// </param>
        /// <param name="sortField">
        /// The sort field.
        /// </param>
        /// <param name="sortOrder">
        /// The sort order.
        /// </param>
        public UserDefinedTableXML(
            DataView xmlDataview, int pageId, int moduleId, bool editable, string sortField, string sortOrder)
        {
            this.dv = xmlDataview;
            this.pageId = pageId;
            this.ModuleID = moduleId;
            this.IsEditable = editable;
            this.SortField = sortField;
            this.SortOrder = sortOrder;
        }

        #endregion

        #region Events

        /// <summary>
        ///   Esperantus.Esperantus.Localize. Sort event is defined using Esperantus.Esperantus.Localize. event keyword.
        /// </summary>
        public event UDTXSLSortEventHandler SortCommand;

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the inner XML.
        /// </summary>
        public string InnerXml
        {
            get
            {
                //this.Document = this.XmlData();
                //return this.Document.InnerXml;
                this.DocumentContent = (new XPathDocument(new XmlNodeReader(this.XmlData()))).CreateNavigator().OuterXml;
                return this.DocumentContent;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is editable.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is editable; otherwise, <c>false</c>.
        /// </value>
        public bool IsEditable { get; private set; }

        /// <summary>
        /// Gets the module ID.
        /// </summary>
        public int ModuleID { get; private set; }

        /// <summary>
        ///   Gets or sets the sort field.
        /// </summary>
        /// <value>
        ///   The sort field.
        /// </value>
        public string SortField { get; set; }

        /// <summary>
        /// Gets or sets the sort order.
        /// </summary>
        /// <value>
        /// The sort order.
        /// </value>
        public string SortOrder { get; set; }

        /// <summary>
        /// Gets the UDT data.
        /// </summary>
        public DataView UDTdata
        {
            get
            {
                return this.dv;
            }
        }

        #endregion

        #region Implemented Interfaces

        #region IPostBackEventHandler

        /// <summary>
        /// Implement Esperantus.Esperantus.Localize. RaisePostBackEvent method from Esperantus.Esperantus.Localize. IPostBackEventHandler interface.
        ///   Define Esperantus.Esperantus.Localize. method of IPostBackEventHandler that raises change events.
        ///   To capture Sorting request issued from XML/XSL data
        /// </summary>
        /// <param name="eventArgument">
        /// A <see cref="T:System.String"/> that represents an optional event argument to be passed to the event handler.
        /// </param>
        public void RaisePostBackEvent(string eventArgument)
        {
            var strEvent = eventArgument.Split('|');
            switch (strEvent[0])
            {
                case "Sort":
                    {
                        var newEvent = new UDTXSLSortEventArgs(strEvent[1], strEvent[2]);
                        this.OnSort(newEvent);
                    }

                    break;
                case "ShowDetail":
                    this.showdetailId = int.Parse(strEvent[1]);

                    // UDTXSLShowDetailEventArgs newEvent = new UDTXSLShowDetailEventArgs(strEvent[1]);
                    // OnShowDetail(newEvent);
                    break;
            }
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Raises the see - cref= Sort event.
        /// </summary>
        /// <param name="e">
        /// The <see cref="Appleseed.Content.Web.Modules.UDTXSLSortEventArgs"/> instance containing the event data.
        /// </param>
        protected virtual void OnSort(UDTXSLSortEventArgs e)
        {
            if (this.SortCommand != null)
            {
                this.SortCommand(this, e); // Invokes Esperantus.Esperantus.Localize. delegates
            }
        }

        /// <summary>
        /// Render this control to Esperantus.Esperantus.Localize. output parameter specified.
        /// </summary>
        /// <param name="output">
        /// Esperantus.Esperantus.Localize. HTML writer to write out to 
        /// </param>
        protected override void Render(HtmlTextWriter output)
        {
            // *** Write it back to Esperantus.Esperantus.Localize. server
            output.Write(this.RenderedXml());
        }

        /// <summary>
        /// Show Detail link creator
        /// </summary>
        /// <param name="idnr">
        /// The id nr.
        /// </param>
        /// <returns>
        /// The get show detail url.
        /// </returns>
        private string GetShowDetailUrl(int idnr)
        {
            return string.Format("javascript:{0}", this.Page.ClientScript.GetPostBackEventReference(this, string.Format("ShowDetail|{0}", idnr)));
        }

        /// <summary>
        /// ShowDetail link creator
        /// </summary>
        /// <param name="order">
        /// The order.
        /// </param>
        /// <returns>
        /// The get sort order image.
        /// </returns>
        private string GetSortOrderImg(string order)
        {
            var s = string.Format(
                @"<img src='{0}' width='10' height='9' border='0'>", 
                order == "DESC"
                    ? Path.WebPathCombine(Path.ApplicationRoot, "DesktopModules/UserDefinedTable/sortdescending.gif")
                    : Path.WebPathCombine(Path.ApplicationRoot, "DesktopModules/UserDefinedTable/sortascending.gif"));

            return s;
        }

        /// <summary>
        /// Sort link creator
        /// </summary>
        /// <param name="field">
        /// The field.
        /// </param>
        /// <param name="order">
        /// The order.
        /// </param>
        /// <returns>
        /// The sorting url.
        /// </returns>
        private string GetSortingUrl(string field, string order)
        {
            return string.Format(
                "javascript:{0}", this.Page.ClientScript.GetPostBackEventReference(this, "Sort|" + field + "|" + order));
        }

        /// <summary>
        /// Renders the XML.
        /// </summary>
        /// <returns>
        /// The output.
        /// </returns>
        private string RenderedXml()
        {
            //this.Document = this.XmlData();
            this.DocumentContent = (new XPathDocument(new XmlNodeReader(this.XmlData()))).CreateNavigator().OuterXml;
            
            // *** Write Esperantus.Esperantus.Localize. HTML into this string builder
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);

            var htmlTextWriter = new HtmlTextWriter(sw);
            base.Render(htmlTextWriter);

            // *** insert Sorting links (if minimal one is used in output)
            if (sb.ToString().IndexOf("@@sort.") > -1)
            {
                foreach (DataColumn fldname in this.dv.Table.Columns)
                {
                    if (fldname.ColumnName == this.SortField)
                    {
                        if (this.SortOrder == "ASC")
                        {
                            sb.Replace(string.Format("@@sort.{0}@@", this.SortField), this.GetSortingUrl(this.SortField, "DESC"));
                            sb.Replace(string.Format("@@imgsortorder.{0}@@", this.SortField), this.GetSortOrderImg("ASC"));
                        }
                        else
                        {
                            sb.Replace(string.Format("@@sort.{0}@@", this.SortField), this.GetSortingUrl(this.SortField, "ASC"));
                            sb.Replace(string.Format("@@imgsortorder.{0}@@", this.SortField), this.GetSortOrderImg("DESC"));
                        }
                    }
                    else
                    {
                        sb.Replace(string.Format("@@sort.{0}@@", fldname.ColumnName), this.GetSortingUrl(fldname.ColumnName, "ASC"));
                        sb.Replace(string.Format("@@imgsortorder.{0}@@", fldname.ColumnName), string.Empty);
                    }
                }
            }

            // *** insert ShowDetail links 
            var cmdPos = sb.ToString().IndexOf("@@ShowDetail");
            while (cmdPos > -1)
            {
                var s = sb.ToString().Substring(cmdPos + 12);
                var p2 = s.IndexOf("@");
                s = s.Substring(0, p2);
                var idnr = int.Parse(s);
                sb.Replace("@@ShowDetail" + idnr + "@@", this.GetShowDetailUrl(idnr));
                cmdPos = sb.ToString().IndexOf("@@ShowDetail");
            }

            // *** Localize
            cmdPos = sb.ToString().ToUpper().IndexOf("@@LOCALIZE");
            while (cmdPos > -1)
            {
                var s = sb.ToString().Substring(cmdPos + 11);
                var p2 = s.IndexOf("@");
                s = s.Substring(0, p2);
                var lkey = s.ToUpper();
                var srepl = sb.ToString().Substring(cmdPos, 11 + p2 + 2);
                sb.Replace(srepl, General.GetString(lkey));
                cmdPos = sb.ToString().ToUpper().IndexOf("@@LOCALIZE");
            }

            // *** SortOrder images
            // cmdPos = sb.ToString().ToUpper().IndexOf("@@IMGSORTORDER");
            // while (cmdPos>-1)
            // {
            //     string s = sb.ToString().Substring(cmdPos + 15);
            //     int p2 = s.IndexOf("@");
            //     s = s.Substring(0,p2);
            //     string lkey = s.ToUpper();
            //     string srepl = sb.ToString().Substring(cmdPos,15+p2+2);
            //     sb.Replace(srepl,Esperantus.General.GetString(lkey));
            //     cmdPos = sb.ToString().ToUpper().IndexOf("@@IMGSORTORDER");
            // }
            return sb.ToString();
        }

        /// <summary>
        /// The xml data.
        /// </summary>
        /// <returns>
        /// </returns>
        private XmlDocument XmlData()
        {
            // sort data view
            if (this.SortField.Length != 0)
            {
                this.dv.Sort = string.Format("{0} {1}", this.SortField, this.SortOrder);
            }

            var xmlDocument = new XmlDocument();

            XmlNode rootElement = xmlDocument.CreateElement("UserDefinedTable");

            // Add root Attributes 
            // ModuleID
            var newAttribute = xmlDocument.CreateAttribute("ModuleID");
            newAttribute.Value = this.ModuleID.ToString();
            rootElement.Attributes.Append(newAttribute);

            // ShowDetail ID
            newAttribute = xmlDocument.CreateAttribute("ShowDetail");
            newAttribute.Value = this.showdetailId.ToString();
            rootElement.Attributes.Append(newAttribute);

            // Language
            newAttribute = xmlDocument.CreateAttribute("xml:lang");
            newAttribute.Value = "en-us"; // Esperantus.Localize.GetCurrentNeutralCultureName();
            rootElement.Attributes.Append(newAttribute);

            /*
                        newAttribute = null;
            */
            xmlDocument.AppendChild(rootElement);

            var iterator = this.dv.GetEnumerator();
            var i = 0;
            while (iterator.MoveNext())
            {
                var drv = (DataRowView)iterator.Current;
                i++;
                if (this.showdetailId != 0 && this.showdetailId.ToString() != drv["UserDefinedRowID"].ToString())
                {
                    continue;
                }

                // Don't boEsperantus.Esperantus.Localize.r to make xml data for ever record if we only want one 
                XmlNode rowNode = xmlDocument.CreateElement("Row");

                var rowIdAttribute = xmlDocument.CreateAttribute("ID");
                rowIdAttribute.Value = drv["UserDefinedRowID"].ToString();
                rowNode.Attributes.Append(rowIdAttribute);

                foreach (DataColumn dc in this.dv.Table.Columns)
                {
                    if (dc.ColumnName != "UserDefinedRowID")
                    {
                        XmlNode fieldNode = xmlDocument.CreateElement(dc.ColumnName);
                        fieldNode.InnerText = drv[dc.ColumnName].ToString();
                        rowNode.AppendChild(fieldNode);
                    }
                }

                // Rob Siera - 04 nov 2004 - Add EditURL to XML output
                XmlNode extraNode = xmlDocument.CreateElement("EditURL");
                if (this.IsEditable)
                {
                    extraNode.InnerText =
                        HttpUrlBuilder.BuildUrl(
                            "~/DesktopModules/UserDefinedTable/UserDefinedTableEdit.aspx", 
                            this.pageId, 
                            string.Format("&mID={0}&UserDefinedRowID={1}", this.ModuleID, drv["UserDefinedRowID"]));
                }
                else
                {
                    extraNode.InnerText = string.Empty;
                }

                rowNode.AppendChild(extraNode);

                // Rob Siera - 11 dec 2004 - Add ShowDetailURL to XML output
                extraNode = xmlDocument.CreateElement("ShowDetailURL");
                extraNode.InnerText = "@@ShowDetail" + drv["UserDefinedRowID"] + "@@";
                rowNode.AppendChild(extraNode);

                rootElement.AppendChild(rowNode);
            }

            return xmlDocument;
        }

        #endregion
    }

    /// <summary>
    /// UDTXSLSortEventHandler
    /// </summary>
    public delegate void UDTXSLSortEventHandler(object sender, UDTXSLSortEventArgs e);

    /// <summary>
    /// The UDT XSL sort event arguments.
    /// </summary>
    public class UDTXSLSortEventArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UDTXSLSortEventArgs"/> class.
        /// </summary>
        /// <param name="sortField">The sort field.</param>
        /// <param name="sortOrder">The sort order.</param>
        public UDTXSLSortEventArgs(string sortField, string sortOrder)
        {
            this.SortField = sortField;
            this.SortOrder = sortOrder;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the sort field.
        /// </summary>
        public string SortField { get; private set; }

        /// <summary>
        /// Gets the sort order.
        /// </summary>
        public string SortOrder { get; private set; }

        #endregion
    }
}