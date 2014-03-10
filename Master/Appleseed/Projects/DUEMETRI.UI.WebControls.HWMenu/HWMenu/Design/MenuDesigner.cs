using System;
using System.Text;
using System.Web.UI.Design;

namespace DUEMETRI.UI.WebControls.HWMenu.Design
{
	/// <summary>
	/// Summary description for MenuDesigner.
	/// </summary>
	public class MenuDesigner : ControlDesigner
	{
        /// <summary>
        /// GetDesignTimeHtml
        /// </summary>
        /// <returns></returns>
        public override string GetDesignTimeHtml()
		{
			Menu menu = (Menu) this.Component;
			if (menu.Childs.Count == 0)
				return this.CreatePlaceHolderDesignTimeHtml("Add menu items throug the Childs collection in property pane");
			else
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("|");				
				foreach(MenuTreeNode mn in menu.Childs)
				{
					sb.Append(mn.Text);				
					sb.Append("|");				
				}
				return sb.ToString();			
			}
		}
	}
}