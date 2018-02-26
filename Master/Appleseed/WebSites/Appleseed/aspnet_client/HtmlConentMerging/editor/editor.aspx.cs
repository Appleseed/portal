namespace TestCompare.editor
{
    using Appleseed.Framework;
    using Appleseed.Framework.Content.Data;
    using System;
    using System.Data.SqlClient;

    /// <summary>
    /// HtmlText Compare and merge editor
    /// </summary>
    
    [History("Ashish.patel@haptix.biz", "2014/11/20", "HtmlText versions Compare and merging")]
    public partial class editor : System.Web.UI.Page 
    {
        /// <summary>
        /// Get data for compare and merge
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["mID"].ToString() != null)
                {
                    HtmlTextDB text = new HtmlTextDB();
                    SqlDataReader sqlLeftverText = text.GetHtmlText(Convert.ToInt32(Request.QueryString["mID"]), WorkFlowVersion.Staging, Convert.ToInt32(Request.QueryString["lvn"]));
                    if (sqlLeftverText.HasRows)
                    {
                        while (sqlLeftverText.Read())
                        {
                            hdnLvn.Value = sqlLeftverText["DesktopHtml"].ToString();
                            hdnLvnPublished.Value = sqlLeftverText["Published"].ToString();
                        }
                    }
                    sqlLeftverText.Close();

                    SqlDataReader sqlRightverText = text.GetHtmlText(Convert.ToInt32(Request.QueryString["mID"]), WorkFlowVersion.Staging, Convert.ToInt32(Request.QueryString["rvn"]));
                    if (sqlRightverText.HasRows)
                    {
                        while (sqlRightverText.Read())
                        {
                            hdnRvn.Value = sqlRightverText["DesktopHtml"].ToString();
                            hdnRvnPublished.Value = sqlRightverText["Published"].ToString();
                        }
                    }
                    sqlRightverText.Close();
                }
            }
        }

        /// <summary>
        /// Method is used to save give htmltext data
        /// </summary>
        /// <param name="moduleID">moduleID</param>
        /// <param name="publishedData">published (1/0)</param>
        /// <param name="HtmlText">DesktopHtmlText</param>
        /// <param name="version">HtmlText version no</param>
        private static void SaveMergeData(string moduleID, string publishedData, string HtmlText, string version)
        {
            string mobSummary = string.Empty;
            string mobDetails = string.Empty;

            HtmlTextDB saveText = new HtmlTextDB();
            SqlDataReader textData = saveText.GetHtmlText(Convert.ToInt32(moduleID), WorkFlowVersion.Staging, Convert.ToInt32(version));
            if (textData.HasRows)
            {
                while (textData.Read())
                {
                    mobSummary = textData["MobileSummary"].ToString();
                    mobDetails = textData["MobileDetails"].ToString();
                }
            }
            textData.Close(); //Added by Ashish - Connection pool issue fixed
            saveText.UpdateHtmlText(
                Convert.ToInt32(moduleID),
                HtmlText,
                 mobSummary,
                  mobDetails,
                  Convert.ToInt32(version),
                  Convert.ToBoolean(publishedData),
                  DateTime.Now, Appleseed.Framework.Site.Configuration.PortalSettings.CurrentUser.Identity.UserName, DateTime.Now, Appleseed.Framework.Site.Configuration.PortalSettings.CurrentUser.Identity.UserName
                  );

        }

        /// <summary>
        /// Cancle will redirect to the parent page
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.Response.Redirect("/DesktopModules/CoreModules/HTMLDocument/HtmlVersonHistory.aspx?mID=" + Request.QueryString["mID"].ToString() + "&ModalChangeMaster=true", true);
        }

        /// <summary>
        /// Save Compare and Merge HtmlText
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string[] VrsionData = hdnVersionData.Value.TrimEnd(',').Split(',');
            string[] publishedData = hdnPublishedData.Value.TrimEnd(',').Split(',');

            SaveMergeData(Request.QueryString["mID"].ToString(), publishedData[0], hdnLhsText.Value, VrsionData[0]);
            SaveMergeData(Request.QueryString["mID"].ToString(), publishedData[1], hdnRhsText.Value, VrsionData[1]);

            this.Response.Redirect("/DesktopModules/CoreModules/HTMLDocument/HtmlEdit.aspx?mID=" + Request.QueryString["mID"].ToString() + "&ModalChangeMaster=true", true);
        }
    }
}

