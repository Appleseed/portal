// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TinyMCETextBox.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The tiny MCE text box.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.UI.WebControls.TinyMCE
{
    using System;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Appleseed.Framework.Site.Configuration;
    using Appleseed.Framework.Web.UI.WebControls;
    using System.Web.UI.HtmlControls;

    /// <summary>
    /// The tiny MCE text box.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class TinyMCETextBox : TextBox, IHtmlEditor
    {
        #region Constants and Fields

        /// <summary>
        /// The image folder.
        /// </summary>
        private string imageFolder = string.Empty;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "TinyMCETextBox" /> class.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public TinyMCETextBox()
        {
            // in order to render the textbox as a textarea
            this.TextMode = TextBoxMode.MultiLine;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the Control Image Folder
        /// </summary>
        /// <value>The image folder.</value>
        /// <remarks>
        /// </remarks>
        public string ImageFolder
        {
            get
            {
                if (this.imageFolder == string.Empty)
                {
                    var pS = (PortalSettings)HttpContext.Current.Items["PortalSettings"];
                    if (pS.CustomSettings != null)
                    {
                        if (pS.CustomSettings["SITESETTINGS_DEFAULT_IMAGE_FOLDER"] != null)
                        {
                            this.imageFolder = pS.CustomSettings["SITESETTINGS_DEFAULT_IMAGE_FOLDER"].ToString();
                        }
                    }
                }

                return "/images/" + this.imageFolder;
            }

            set
            {
                this.imageFolder = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Raised when the pages is loading. Here the TinyMCETextBox will register its scripts references.
        /// </summary>
        /// <param name="e">
        /// The <see cref="T:System.EventArgs"/> object that contains the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected override void OnLoad(EventArgs e)
        {

            if (!this.Page.ClientScript.IsClientScriptIncludeRegistered(this.GetType(), "TinyMCE"))
            {
                this.Page.ClientScript.RegisterClientScriptInclude(
                    this.GetType(), "TinyMCE", HttpUrlBuilder.BuildUrl("~/aspnet_client/tiny_mce/tinymce.min.js"));
            }

            HtmlGenericControl colorboxCss = new HtmlGenericControl("link");
            colorboxCss.Attributes.Add("href", "/aspnet_client/tiny_mce/skins/lightgray/content.min.css");
            colorboxCss.Attributes.Add("type", "text/css");
            colorboxCss.Attributes.Add("rel", "stylesheet");

            Page.Header.Controls.Add(colorboxCss);

            base.OnLoad(e);
        }

        /// <summary>
        /// Renders the <see cref="T:System.Web.UI.WebControls.TextBox"/> control to the specified <see cref="T:System.Web.UI.HtmlTextWriter"/> object.
        /// </summary>
        /// <param name="writer">
        /// The <see cref="T:System.Web.UI.HtmlTextWriter"/> that receives the rendered output.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected override void Render(HtmlTextWriter writer)
        {
            var specificEditorClass = string.Concat(this.ClientID, "_tinymce");
            this.Attributes.Add("class", specificEditorClass);
            writer.Write("<script type=\"text/javascript\">");

            var width = this.Width.IsEmpty ? string.Empty : string.Concat("width : \"", this.Width.Value + 3, "\",");
            var height = this.Height.IsEmpty ? string.Empty : string.Concat("height : \"", this.Height.Value, "\",");

            writer.Write(
               string.Concat(
              " tinymce.init({ " +
       " selector: \"textarea\", " +
       " plugins: \"advlist,anchor,autolink,autoresize,autosave,bbcode,charmap,code,colorpicker,contextmenu,directionality,emoticons,fullpage,fullscreen,hr,image,importcss,insertdatetime,layer,legacyoutput,link,lists,media,nonbreaking,noneditable,pagebreak,paste,preview,print,save,searchreplace,spellchecker,tabfocus,table,template,textcolor,textpattern,visualblocks,visualchars,wordcount\", " +
       " toolbar1: \"insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image\", " +
       " toolbar2: \"print preview media | forecolor backcolor emoticons\", " +
       width +
       height +
   " }); "));

            writer.Write("</script>");
            base.Render(writer);
        }

        #endregion
    }
}