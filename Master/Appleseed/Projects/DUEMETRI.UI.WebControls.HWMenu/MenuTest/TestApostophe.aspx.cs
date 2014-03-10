using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace MenuTest
{
	/// <summary>
	/// Descrizione di riepilogo per WebForm1.
	/// </summary>
	public class WebForm1 : System.Web.UI.Page
	{
		protected DUEMETRI.UI.WebControls.HWMenu.Menu Menu1;
		protected DUEMETRI.UI.WebControls.HWMenu.MenuTreeNode menuTreeNode1;
		protected DUEMETRI.UI.WebControls.HWMenu.MenuTreeNode menuTreeNode2;
		protected DUEMETRI.UI.WebControls.HWMenu.MenuTreeNode menuTreeNode3;
		protected DUEMETRI.UI.WebControls.HWMenu.MenuTreeNode menuTreeNode4;
		protected DUEMETRI.UI.WebControls.HWMenu.MenuTreeNode menuTreeNode5;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Inserire qui il codice utente necessario per inizializzare la pagina.
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: questa chiamata è richiesta da Progettazione Web Form ASP.NET.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Metodo necessario per il supporto della finestra di progettazione. Non modificare
		/// il contenuto del metodo con l'editor di codice.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
