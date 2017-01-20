using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Appleseed.Framework.Web.UI.WebControls;

namespace Appleseed.Framework.UI.WebControls.CodeMirror
{
    /// <summary>
    /// CodeMirrorTextBox is a simple implementation for an html editor. 
    /// It implements text only, but in a nice and colored way.
    /// </summary>
    public class CodeMirrorTextBox : TextBox, IHtmlEditor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CodeMirrorTextBox"/> class.
        /// </summary>
        public CodeMirrorTextBox()
        {
            TextMode = TextBoxMode.MultiLine;
        }

        /// <summary>
        /// Control Image Folder
        /// </summary>
        /// <value></value>
        public string ImageFolder
        {
            get { return string.Empty; }
            set { ;}
        }


        /// <summary>
        /// Raised when the pages is loading. Here the CodeMirrorTextBox register its scripts.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            if (!Page.ClientScript.IsClientScriptIncludeRegistered(this.GetType(), "CodeMirror"))
            {
                Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "CodeMirror", HttpUrlBuilder.BuildUrl("~/aspnet_client/CodeMirrorV5.12/js/codemirror.js"));
                Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "CodeMirror_mode_xml", HttpUrlBuilder.BuildUrl("~/aspnet_client/CodeMirrorV5.12/mode/xml/xml.js"));
                Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "CodeMirror_mode_js", HttpUrlBuilder.BuildUrl("~/aspnet_client/CodeMirrorV5.12/mode/javascript/javascript.js"));
                Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "CodeMirror_mode_css", HttpUrlBuilder.BuildUrl("~/aspnet_client/CodeMirrorV5.12/mode/css/css.js"));
                Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "CodeMirror_mode_htmlmixed", HttpUrlBuilder.BuildUrl("~/aspnet_client/CodeMirrorV5.12/mode/htmlmixed/htmlmixed.js"));

                Literal cssFile = new Literal() { Text = @"<link href=""" + HttpUrlBuilder.BuildUrl("~/aspnet_client/CodeMirrorV5.12/css/docs.css") + @""" type=""text/css"" rel=""stylesheet"" />" };
                Page.Header.Controls.Add(cssFile);
                cssFile = new Literal() { Text = @"<link href=""" + HttpUrlBuilder.BuildUrl("~/aspnet_client/CodeMirrorV5.12/css/codemirror.css") + @""" type=""text/css"" rel=""stylesheet"" />" };
                Page.Header.Controls.Add(cssFile);

                var jsToAdd = "<script type=\"text/javascript\"> $(document).ready(function(){ var editor = CodeMirror.fromTextArea(document.getElementById('" + this.ClientID + "'), { mode: \"text/html\", extraKeys: {\"Ctrl-Space\": \"autocomplete\"},value: document.getElementById('" + this.ClientID + "').innerHTML,lineNumbers: true,indentWithTabs: true }); }); </script>";


                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CM_load", jsToAdd);

            }
            base.OnLoad(e);
        }

        /// <summary>
        /// It writes the javascript necessary for the rendering of the CodeMirrorTextBox. 
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            this.Attributes["id"] = this.ClientID;
            this.Style.Add("width", "100%");
            writer.Write(string.Concat("<div style=\"border: 1px solid grey;\" >"));
            base.Render(writer);
            writer.Write("</div>");
        }
    }
}
