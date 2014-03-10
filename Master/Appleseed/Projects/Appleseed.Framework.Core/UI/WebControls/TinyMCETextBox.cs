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
                    this.GetType(), "TinyMCE", HttpUrlBuilder.BuildUrl("~/aspnet_client/tiny_mce/tiny_mce.js"));
            }

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
                    @"
                tinyMCE.init({
                    // General options
                    mode : ""specific_textareas"",
                    editor_selector : """, 
                    specificEditorClass, 
                    @""",
                    theme : ""advanced"",
                    plugins : ""pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template,wordcount,advlist,autosave"",

                    // Theme options
                    theme_advanced_buttons1 : ""newdocument,|,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,styleselect,formatselect,fontselect,fontsizeselect,|,help"",
                    theme_advanced_buttons2 : ""cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,cleanup,code,|,insertdate,inserttime,preview,|,forecolor,backcolor"",
                    theme_advanced_buttons3 : ""tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,emotions,iespell,media,advhr,|,print,|,ltr,rtl,|,fullscreen"",
                    theme_advanced_buttons4 : ""insertlayer,moveforward,movebackward,absolute,|,styleprops,|,cite,abbr,acronym,del,ins,attribs,|,visualchars,nonbreaking,template,pagebreak,restoredraft"",
                    theme_advanced_toolbar_location : ""top"",
                    theme_advanced_toolbar_align : ""left"",
                    theme_advanced_statusbar_location : ""bottom"",
                    theme_advanced_resizing : false,
                    ", 
                    width, 
                    @"
                    ", 
                    height, 
                    @"
                    // Example content CSS (should be your site CSS)
                    content_css : ""css/content.css"",

                    // Drop lists for link/image/media/template dialogs
                    template_external_list_url : ""lists/template_list.js"",
                    external_link_list_url : ""lists/link_list.js"",
                    external_image_list_url : ""lists/image_list.js"",
                    media_external_list_url : ""lists/media_list.js"",

                    // Style formats
                    style_formats : [
                        {title : 'Normal', inline : 'span', classes : 'Normal'},
                        {title : 'Red text', inline : 'span', styles : {color : '#ff0000'}}
                    ]
                });
                "));

            writer.Write("</script>");
            base.Render(writer);
        }

        #endregion
    }
}