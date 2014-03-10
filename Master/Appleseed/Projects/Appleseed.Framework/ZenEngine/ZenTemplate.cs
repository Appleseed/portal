using System.Web.UI;

namespace Appleseed.Framework.Web.UI.WebControls
{
    /// <summary>
    /// DesktopPanes design support class for Visual Studio. Pane Template.
    /// </summary>
    public class ZenTemplate : Control, INamingContainer
    {
        private ZenLayout parent;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="parent">The parent.</param>
        public ZenTemplate(ZenLayout parent)
        {
            this.parent = parent;
        }
    }
}