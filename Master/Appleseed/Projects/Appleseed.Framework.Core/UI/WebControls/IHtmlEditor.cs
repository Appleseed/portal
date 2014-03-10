using System.Web.UI.WebControls;

namespace Appleseed.Framework.Web.UI.WebControls
{
    /// <summary>
    /// IHtmlEditor is a simple interface that defines 
    /// some minimal common features between different html editors.
    /// </summary>
    public interface IHtmlEditor
    {
        /// <summary>
        /// Control Text
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Control Image Folder
        /// </summary>
        string ImageFolder { get; set; }

        /// <summary>
        /// Control Height
        /// </summary>
        Unit Height { get; set; }

        /// <summary>
        /// Control Width
        /// </summary>
        Unit Width { get; set; }
    }
}