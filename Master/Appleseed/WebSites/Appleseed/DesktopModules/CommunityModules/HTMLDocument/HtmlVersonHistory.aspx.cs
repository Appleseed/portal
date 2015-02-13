namespace Appleseed.DesktopModules.CommunityModules.HTMLDocument
{
    using Appleseed.Framework.Content.Data;
    using Appleseed.Framework.Web.UI;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Appleseed.Framework.Security;
    using Appleseed.Framework;

    /// <summary>
    /// Display the perticular module's history
    /// </summary>
    [History("Ashish.patel@haptix.biz", "2014/11/20", "HtmlText Version History")]
    public partial class HtmlVersonHistory : EditItemPage 
    {
        int lvn, rvn = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HtmlTextDB versionDB = new HtmlTextDB();
         
                DataTable versionTable = versionDB.GetVersionHistory(Convert.ToInt32(Request.QueryString["mID"].ToString())).Tables[0];

                RptVersionHistory.DataSource = versionTable;
                RptVersionHistory.DataBind();
            }
        }

        /// <summary>
        /// Send back to parent page
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        [History("Ashish.patel@haptix.biz", "2014/11/20", "Send back to parent page")]
        protected void btnBack_Click(object sender, EventArgs e)
        {
            this.Response.Redirect("/DesktopModules/CommunityModules/HTMLDocument/HtmlEdit.aspx?mID=" + this.ModuleID + "&ModalChangeMaster=", true);
        }

        /// <summary>
        /// Pick two selected versions and redirect to editor with selected version
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        [History("Ashish.patel@haptix.biz", "2014/11/20", "Display editor for Compare and merge code")]
        protected void btnCompareAndMarge_Click(object sender, EventArgs e)
        {
           var idList = string.Empty;
           foreach (RepeaterItem item in RptVersionHistory.Items)
            {
                CheckBox box = item.FindControl("chkVersion") as CheckBox;
                HiddenField hd = item.FindControl("hdnVersionNo") as HiddenField;
                if (box != null && box.Checked)
                {
                    idList = idList + ',' + hd.Value;
                }
            }

           string[] vrLists;

           vrLists = idList.TrimStart(',').Split(',');

           lvn = Convert.ToInt32( vrLists[0]);
           rvn = Convert.ToInt32( vrLists[1]);
           this.Response.Redirect("/aspnet_client/HtmlConentMerging/editor/editor.aspx?mID=" + this.ModuleID + "&lvn=" + lvn + "&rvn=" + rvn + "&ModalChangeMaster=", true);
        }
    }
}