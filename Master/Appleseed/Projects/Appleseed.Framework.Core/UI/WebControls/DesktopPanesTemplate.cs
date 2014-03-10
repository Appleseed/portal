using System;
using System.Web.UI;

namespace Appleseed.Framework.Web.UI.WebControls
{
    /// <summary>
    /// DesktopPanes design support class for Visual Studio. Pane Template.
    /// </summary>
	public class DesktopPanesTemplate : Control, INamingContainer
	{
        private DesktopPanesBase parent;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="parent"></param>
		public DesktopPanesTemplate(DesktopPanesBase parent)
		{
			this.parent = parent;
		}
	}
}