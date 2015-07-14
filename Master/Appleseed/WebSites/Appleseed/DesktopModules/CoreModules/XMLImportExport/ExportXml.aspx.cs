namespace Appleseed.DesktopModules.CoreModules.XMLImportExport
{
    using Appleseed.Framework.DAL;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Xml;

    /// <summary>
    /// Create XMl for selected page and module of page
    /// with their respective settings
    /// </summary>
    public partial class ExportXml : System.Web.UI.Page
    {
        XmlDocument xmlDoc = new XmlDocument();

        /// <summary>
        /// Export the xml of selected page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

            CultureInfo currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            string pLanguage = currentCulture.IetfLanguageTag.Split('-').GetValue(0).ToString().ToLower();
            GetPageDetails(Convert.ToInt32(Request.QueryString["pid"]), pLanguage);
        }


        private void AddAttribute(DataRow dr, String columnName, XmlNode rootnode)
        {
            XmlAttribute attribute = xmlDoc.CreateAttribute(columnName);
            attribute.Value = dr[columnName].ToString();
            rootnode.Attributes.Append(attribute);
        }

       /// <summary>
       /// Create XML 
       /// </summary>
       /// <param name="PageID">Selected page id</param>
       /// <param name="lang">Current language</param>
        public void GetPageDetails(int PageID, string lang)
        {
            XMLImportExportDB pageDetail = new XMLImportExportDB();

            DataSet dsPageModuleDetails = pageDetail.GetPageByID(PageID, lang);

            XmlNode pageNode = xmlDoc.CreateElement("Page");
            xmlDoc.AppendChild(pageNode);
            int i = 0;
            //Page Attribures
            AddAttribute(dsPageModuleDetails.Tables[0].Rows[i], "PageID", pageNode);
            AddAttribute(dsPageModuleDetails.Tables[0].Rows[i], "ParentPageID", pageNode);
            AddAttribute(dsPageModuleDetails.Tables[0].Rows[i], "PageOrder", pageNode);
            AddAttribute(dsPageModuleDetails.Tables[0].Rows[i], "PortalID", pageNode);
            AddAttribute(dsPageModuleDetails.Tables[0].Rows[i], "PageName", pageNode);
            AddAttribute(dsPageModuleDetails.Tables[0].Rows[i], "MobilePageName", pageNode);
            AddAttribute(dsPageModuleDetails.Tables[0].Rows[i], "ShowMobile", pageNode);
            AddAttribute(dsPageModuleDetails.Tables[0].Rows[i], "PageLayout", pageNode);

            //Page Long text fields
            XmlNode longTextNode = xmlDoc.CreateElement("PageDescription");
            longTextNode.AppendChild(xmlDoc.CreateCDataSection(dsPageModuleDetails.Tables[0].Rows[i]["PageDescription"].ToString()));
            pageNode.AppendChild(longTextNode);

            longTextNode = xmlDoc.CreateElement("AuthorizedRoles");
            longTextNode.AppendChild(xmlDoc.CreateCDataSection(dsPageModuleDetails.Tables[0].Rows[i]["AuthorizedRoles"].ToString()));
            pageNode.AppendChild(longTextNode);

            //page settings
            XmlNode pageSettingRoot = xmlDoc.CreateElement("PageSettings");
            pageNode.AppendChild(pageSettingRoot);

            foreach (DataRow item in dsPageModuleDetails.Tables[1].Rows)
            {
                XmlNode pageSetting = xmlDoc.CreateElement("PageSetting");
                pageSettingRoot.AppendChild(pageSetting);

                XmlAttribute attribute = xmlDoc.CreateAttribute("Name");
                attribute.Value = item["SettingName"].ToString();
                pageSetting.Attributes.Append(attribute);

                pageSetting.AppendChild(xmlDoc.CreateCDataSection(item["SettingValue"].ToString()));
            }


            //Module
            XmlNode ModuleRoot = xmlDoc.CreateElement("Modules");
            pageNode.AppendChild(ModuleRoot);

            DateTime lstModDt = new DateTime();
           
            foreach (DataRow item in dsPageModuleDetails.Tables[2].Rows)
            {
                XmlNode ModuleDetail = xmlDoc.CreateElement("Module");
                ModuleRoot.AppendChild(ModuleDetail);

                AddAttribute(item, "ModuleID", ModuleDetail);
                AddAttribute(item, "ModuleDefID", ModuleDetail);
                AddAttribute(item, "ModuleOrder", ModuleDetail);
                AddAttribute(item, "CacheTime", ModuleDetail);
                AddAttribute(item, "ShowMobile", ModuleDetail);
                AddAttribute(item, "NewVersion", ModuleDetail);
                AddAttribute(item, "SupportWorkflow", ModuleDetail);
                AddAttribute(item, "WorkflowState", ModuleDetail);
                AddAttribute(item, "StagingLastModified", ModuleDetail);
                AddAttribute(item, "SupportCollapsable", ModuleDetail);
                AddAttribute(item, "ShowEveryWhere", ModuleDetail);
                AddAttribute(item, "WorkflowState", ModuleDetail);
                AddAttribute(item, "WorkflowState", ModuleDetail);
                AddAttribute(item, "WorkflowState", ModuleDetail);

                DateTime.TryParse(item["LastModified"].ToString(), out lstModDt);
                XmlAttribute Dateattribute = xmlDoc.CreateAttribute("LastModified");
                Dateattribute.Value = lstModDt.ToString("dd-MMM-yyyyTHH:mm:ss");
                ModuleDetail.Attributes.Append(Dateattribute);

                DateTime.TryParse(item["LastModified"].ToString(), out lstModDt);
                Dateattribute = xmlDoc.CreateAttribute("StagingLastModified");
                Dateattribute.Value = lstModDt.ToString("dd-MMM-yyyyTHH:mm:ss");
                ModuleDetail.Attributes.Append(Dateattribute);

                longTextNode = xmlDoc.CreateElement("PaneName");
                longTextNode.AppendChild(xmlDoc.CreateCDataSection(item["PaneName"].ToString()));
                ModuleDetail.AppendChild(longTextNode);

                longTextNode = xmlDoc.CreateElement("ModuleTitle");
                longTextNode.AppendChild(xmlDoc.CreateCDataSection(item["ModuleTitle"].ToString()));
                ModuleDetail.AppendChild(longTextNode);

                longTextNode = xmlDoc.CreateElement("AuthorizedEditRoles");
                longTextNode.AppendChild(xmlDoc.CreateCDataSection(item["AuthorizedEditRoles"].ToString()));
                ModuleDetail.AppendChild(longTextNode);

                longTextNode = xmlDoc.CreateElement("AuthorizedAddRoles");
                longTextNode.AppendChild(xmlDoc.CreateCDataSection(item["AuthorizedAddRoles"].ToString()));
                ModuleDetail.AppendChild(longTextNode);

                longTextNode = xmlDoc.CreateElement("AuthorizedDeleteRoles");
                longTextNode.AppendChild(xmlDoc.CreateCDataSection(item["AuthorizedDeleteRoles"].ToString()));
                ModuleDetail.AppendChild(longTextNode);

                longTextNode = xmlDoc.CreateElement("AuthorizedViewRoles");
                longTextNode.AppendChild(xmlDoc.CreateCDataSection(item["AuthorizedViewRoles"].ToString()));
                ModuleDetail.AppendChild(longTextNode);

                longTextNode = xmlDoc.CreateElement("AuthorizedPropertiesRoles");
                longTextNode.AppendChild(xmlDoc.CreateCDataSection(item["AuthorizedPropertiesRoles"].ToString()));
                ModuleDetail.AppendChild(longTextNode);

                longTextNode = xmlDoc.CreateElement("AuthorizedPublishingRoles");
                longTextNode.AppendChild(xmlDoc.CreateCDataSection(item["AuthorizedPublishingRoles"].ToString()));
                ModuleDetail.AppendChild(longTextNode);

                longTextNode = xmlDoc.CreateElement("AuthorizedApproveRoles");
                longTextNode.AppendChild(xmlDoc.CreateCDataSection(item["AuthorizedApproveRoles"].ToString()));
                ModuleDetail.AppendChild(longTextNode);

                longTextNode = xmlDoc.CreateElement("LastEditor");
                longTextNode.AppendChild(xmlDoc.CreateCDataSection(item["LastEditor"].ToString()));
                ModuleDetail.AppendChild(longTextNode);

                longTextNode = xmlDoc.CreateElement("StagingLastEditor");
                longTextNode.AppendChild(xmlDoc.CreateCDataSection(item["StagingLastEditor"].ToString()));
                ModuleDetail.AppendChild(longTextNode);

                longTextNode = xmlDoc.CreateElement("AuthorizedMoveModuleRoles");
                longTextNode.AppendChild(xmlDoc.CreateCDataSection(item["AuthorizedMoveModuleRoles"].ToString()));
                ModuleDetail.AppendChild(longTextNode);

                longTextNode = xmlDoc.CreateElement("AuthorizedDeleteModuleRoles");
                longTextNode.AppendChild(xmlDoc.CreateCDataSection(item["AuthorizedDeleteModuleRoles"].ToString()));
                ModuleDetail.AppendChild(longTextNode);

                XmlNode ModuleSettings = xmlDoc.CreateElement("ModuleSettings");
                ModuleDetail.AppendChild(ModuleSettings);

                foreach (DataRow moduleSetting in dsPageModuleDetails.Tables[3].Rows)
                {
                    if (item["ModuleID"].ToString() == moduleSetting["ModuleID"].ToString())
                    {
                        XmlNode ModuleSettingNode = xmlDoc.CreateElement("ModuleSetting");
                        ModuleSettings.AppendChild(ModuleSettingNode);

                        XmlAttribute attribute = xmlDoc.CreateAttribute("SettingName");
                        attribute.Value = moduleSetting["SettingName"].ToString();
                        ModuleSettingNode.Attributes.Append(attribute);
                        ModuleSettingNode.AppendChild(xmlDoc.CreateCDataSection(moduleSetting["SettingValue"].ToString()));
                    }
                }
            }

            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(stream, System.Text.Encoding.UTF8);

            xmlDoc.WriteTo(writer);
            writer.Flush();
            Response.Clear();
            byte[] byteArray = stream.ToArray();

            string pagename = dsPageModuleDetails.Tables[0].Rows[i]["PageName"].ToString();
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            pagename = rgx.Replace(pagename, "");
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + pagename + ".xml");
            Response.AppendHeader("Content-Length", byteArray.Length.ToString());
            Response.ContentType = "application/octet-stream";
            Response.BinaryWrite(byteArray);
            writer.Close();
            Response.End();
        }
    }
}