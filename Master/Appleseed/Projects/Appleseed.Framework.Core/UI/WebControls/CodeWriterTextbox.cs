using Appleseed.Framework.Web.UI.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace Appleseed.Framework.UI.WebControls
{
    /// <summary>
    /// CodeWriterTextbox is a simple implementation for an html editor. 
    /// </summary>
    public class CodeWriterTextbox : TextBox, IHtmlEditor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CodeWriterTextbox"/> class.
        /// </summary>
        public CodeWriterTextbox()
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
            set {; }
        }

    }
}
