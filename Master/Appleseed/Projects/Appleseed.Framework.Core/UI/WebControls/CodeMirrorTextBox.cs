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
                Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "CodeMirror", HttpUrlBuilder.BuildUrl("~/aspnet_client/CodeMirror/js/codemirror.js"));
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
            writer.Write(string.Concat("<div style=\"border: 1px solid grey; padding: 3px 0px 3px 3px; width: ", new Unit(this.Width.Value + 3, UnitType.Pixel), "\">"));
            base.Render(writer);
            var rootPath = string.Concat(Appleseed.Framework.Settings.Path.ApplicationRoot, @"/aspnet_client/CodeMirror/");
            var jsToAdd = string.Concat(@"
            <script type=""text/javascript""> 
                var editor", this.ClientID, @" = CodeMirror.fromTextArea('", this.ClientID, @"', {
                    height: """, this.Height, @""",
                    parserfile: [""parsexml.js"", ""parsecss.js"", ""tokenizejavascript.js"", ""parsejavascript.js"", ""parsehtmlmixed.js""],
                    stylesheet: [""", rootPath, @"css/xmlcolors.css"", """, rootPath, @"css/jscolors.css"", """, rootPath, @"css/csscolors.css""],
                    path: """, rootPath, @"js/""
                });
            </script>");
            writer.Write(jsToAdd);
            writer.Write("</div>");
        }
    }
}
