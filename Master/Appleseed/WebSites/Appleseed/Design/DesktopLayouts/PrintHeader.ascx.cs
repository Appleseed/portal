using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Appleseed.Framework.Site.Configuration;

namespace Appleseed.Content.Web.Modules
{
	/// <summary>
	///	Header for Print page
	/// </summary>
	public partial  class PrintHeader : UserControl
	{
        /// <summary>
        /// Placeholder for current control
        /// </summary>
        protected PlaceHolder LayoutPlaceHolder;

		private void PrintHeader_Load(object sender, EventArgs e)
		{
            string LayoutBasePage = "PrintHeader.ascx";
			
            // Obtain PortalSettings from Current Context
			PortalSettings portalSettings = (PortalSettings) HttpContext.Current.Items["PortalSettings"];
			
			try
			{
				LayoutPlaceHolder.Controls.Add(Page.LoadControl(portalSettings.PortalLayoutPath + LayoutBasePage));
			}
			catch
			{
				//No header available
			}
		}

		#region Web Form Designer generated code
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///	Required method for Designer support - do not modify
		///	the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.Load += new EventHandler(this.PrintHeader_Load);

        }
		#endregion
	}
}
