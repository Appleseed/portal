using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Appleseed.Framework.Web.UI;
using System.Globalization;
using Appleseed.Framework.DataTypes;
using Appleseed.Framework.Content.Data;
using Appleseed.Framework;
using Appleseed.Framework.Web.UI.WebControls;
using LinkButton = Appleseed.Framework.Web.UI.WebControls.LinkButton;

namespace Appleseed.DesktopModules.CoreModules.HTMLDocument
{
    public partial class HtmlEditModal : EditItemPage
    {
        #region Constants and Fields

        /// <summary>
        /// The desktop text.
        /// </summary>
        protected IHtmlEditor DesktopText;

        #endregion

        #region Properties

        /// <summary>
        ///   Set the module guids with free access to this page
        /// </summary>
        /// <value>The allowed modules.</value>
        protected override List<string> AllowedModules
        {
            get
            {
                var al = new List<string> { "0B113F51-FEA3-499A-98E7-7B83C192FDBB" };
                return al;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles OnInit event
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        /// <remarks>
        /// The Page_Load event on this Page is used to obtain the ModuleID
        ///   of the xml module to edit.
        ///   It then uses the Appleseed.HtmlTextDB() data component
        ///   to populate the page's edit controls with the text details.
        /// </remarks>
        protected override void OnInit(EventArgs e)
        {
            // Controls must be created here
            this.UpdateButton = new LinkButton();
            this.CancelButton = new LinkButton();

            // Add the setting
            var editor = this.ModuleSettings["Editor"].ToString();
            var width = this.ModuleSettings["Width"].ToString();
            var height = this.ModuleSettings["Height"].ToString();
            var showUpload = this.ModuleSettings["ShowUpload"].ToBoolean(CultureInfo.InvariantCulture);
            

            var h = new HtmlEditorDataType { Value = editor };
            this.DesktopText = h.GetEditor(
                this.PlaceHolderHTMLEditor,
                this.ModuleID,
                showUpload,
                this.PortalSettings);

            this.DesktopText.Width = new Unit(width);
            this.DesktopText.Height = new Unit(height);
            

            // Construct the page
            // Added css Styles by Mario Endara <mario@softworks.com.uy> (2004/10/26)
            this.UpdateButton.CssClass = "CommandButton";
            this.PlaceHolderButtons.Controls.Add(this.UpdateButton);
            this.PlaceHolderButtons.Controls.Add(new LiteralControl("&#160;"));
            this.CancelButton.CssClass = "CommandButton";
            this.CancelButton.ID = "EditHtmlCancelButton";
            this.PlaceHolderButtons.Controls.Add(this.CancelButton);

            // Obtain a single row of text information
            var text = new HtmlTextDB();

            // Change by Geert.Audenaert@Syntegra.Com - Date: 7/2/2003
            // Original: SqlDataReader dr = text.GetHtmlText(ModuleID);
            var dr = text.GetHtmlText(this.ModuleID, WorkFlowVersion.Staging);

            // End Change Geert.Audenaert@Syntegra.Com
            try {
                if (dr.Read()) {
                    this.DesktopText.Text = this.Server.HtmlDecode((string)dr["DesktopHtml"]);
                    
                } else {
                    this.DesktopText.Text = General.GetString(
                        "HTMLDOCUMENT_TODO_ADDCONTENT", "Todo: Add Content...", null);
                    
                }
            } finally {
                dr.Close();
            }

            this.dialogclass.Attributes.Add("class", this.ModuleID.ToString());

            base.OnInit(e);
        }

        protected override void OnUpdate(EventArgs e)
        {
            base.OnUpdate(e);

            // Create an instance of the HtmlTextDB component
            var text = new HtmlTextDB();

            // Update the text within the HtmlText table
            text.UpdateHtmlText(
                this.ModuleID,
                this.Server.HtmlEncode(this.DesktopText.Text),
                this.Server.HtmlEncode(""),
                this.Server.HtmlEncode(""));

            Response.Write("<script type=\"text/javascript\">window.parent.location = window.parent.location.href;</script>");
            
        }

        protected override void OnCancel(EventArgs e)
        {
           
            //base.OnCancel(e);

            
        }

        #endregion
    }
    
        
}