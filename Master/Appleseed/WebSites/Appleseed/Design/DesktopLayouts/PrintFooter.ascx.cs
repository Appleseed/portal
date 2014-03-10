using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Appleseed.Framework.Site.Configuration;

namespace Appleseed.Content.Web.Modules
{
	/// <summary>
	///	Footer for Print page
	/// </summary>
	public partial  class PrintFooter : UserControl
	{
        /// <summary>
        /// Placeholder for current control
        /// </summary>
        protected PlaceHolder LayoutPlaceHolder;

		private void PrintFooter_Load(object sender, EventArgs e)
		{
            string LayoutBasePage = "PrintFooter.ascx";
			
            // Obtain PortalSettings from Current Context
			PortalSettings portalSettings = (PortalSettings) HttpContext.Current.Items["PortalSettings"];
			
			try
			{
				LayoutPlaceHolder.Controls.Add(Page.LoadControl(portalSettings.PortalLayoutPath + LayoutBasePage));
			}
			catch
			{
				//No footer available
			}
		}

		#region Web Form Designer generated code
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
            this.Load += new EventHandler(this.PrintFooter_Load);

        }
		#endregion
	}
}
