namespace Appleseed.DesktopModules.CoreModules.XMLImportExport
{
    using Appleseed.Framework;
    using Appleseed.Framework.DAL;
    using Appleseed.Framework.Site.Data;
    using Appleseed.Framework.Web.UI.WebControls;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Xml;

    /// <summary>
    /// Selected page export as XML
    /// XML page can import
    /// </summary>
    [History("Ashish Patel", "2014/10/02", "Export/Import page")]
    public partial class XmlImportExport : PortalModuleControl
    {
        XmlDocument xmlDoc = new XmlDocument();
        XMLImportExportDB xmlFile = new XMLImportExportDB();

        /// <summary>
        /// Create a tree for pages
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            pagesTree.Attributes.Add("OnClick", "client_OnTreeNodeChecked(event)");
            XMLImportExportDB pageTreeSource = new XMLImportExportDB();
            DataTable dtTree = pageTreeSource.GetPageTree(0).Tables[0];

            if (!IsPostBack)
            {

                for (int i = 0; i < dtTree.Rows.Count; i++)
                {
                    int parentId = 0;
                    int.TryParse(dtTree.Rows[i]["ParentPageID"].ToString(), out parentId);

                    if (parentId == 0)
                    {
                        TreeNode newNode = new TreeNode();
                        newNode.Text = dtTree.Rows[i]["PageName"].ToString();
                        newNode.NavigateUrl = "/" + dtTree.Rows[i]["PageID"].ToString();
                        newNode.Value = dtTree.Rows[i]["PageID"].ToString();
                        newNode.SelectAction = TreeNodeSelectAction.None;
                        pagesTree.Nodes.Add(newNode);

                        int pageId = 0;
                        int.TryParse(dtTree.Rows[i]["PageID"].ToString(), out pageId);
                        if (HasChildren(dtTree, pageId))
                        {
                            CreatePageTree(dtTree, pageId, newNode);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check wether the node has a child or not 
        /// </summary>
        /// <param name="dtTree">Table</param>
        /// <param name="pageId">Parent PageID</param>
        /// <returns></returns>
        private bool HasChildren(DataTable dtTree, int pageId)
        {
            foreach (DataRow item in dtTree.Rows)
            {
                int parentId = 0;
                int.TryParse(item["ParentPageID"].ToString(), out parentId);
                if (parentId == pageId)
                {
                    return true;
                }
            }
            return false;
        }

        private void CreatePageTree(DataTable dtTree, int ParentID, TreeNode perentnode)
        {
            for (int i = 0; i < dtTree.Rows.Count; i++)
            {
                int parentpageId = 0;
                int.TryParse(dtTree.Rows[i]["ParentPageID"].ToString(), out parentpageId);

                if (parentpageId == ParentID)
                {
                    TreeNode newNode = new TreeNode();
                    newNode.Text = dtTree.Rows[i]["PageName"].ToString();
                    newNode.NavigateUrl = "/" + dtTree.Rows[i]["PageID"].ToString();
                    newNode.Value = dtTree.Rows[i]["PageID"].ToString();
                    newNode.SelectAction = TreeNodeSelectAction.None;
                    perentnode.ChildNodes.Add(newNode);

                    int pageId = 0;
                    int.TryParse(dtTree.Rows[i]["PageID"].ToString(), out pageId);
                    if (HasChildren(dtTree, pageId))
                    {
                        CreatePageTree(dtTree, pageId, newNode);
                    }
                }
            }
        }

        /// <summary>
        /// Import XML and get log table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnImportXml_Click(object sender, EventArgs e)
        {

            if (fileUploadXML.HasFile)
            {
                try
                {
                    string xmlContent;
                    using (StreamReader xmlStremReader = new StreamReader(fileUploadXML.PostedFile.InputStream))
                    {
                        xmlContent = xmlStremReader.ReadToEnd();
                        SqlDataReader sqlDrLog = xmlFile.ImportXmlFile(xmlContent);
                        rptLogTable.DataSource = sqlDrLog;
                        rptLogTable.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}