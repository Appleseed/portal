using System.Web.UI.WebControls;

namespace Appleseed.Framework.Web.UI.WebControls
{
	/// <summary>
	/// TextEditor is a simple implementation for an html editor. 
	/// Currently implements text only.
	/// </summary>
	public class TextEditor : TextBox, IHtmlEditor
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TextEditor"/> class.
		/// </summary>
		public TextEditor()
		{
			TextMode = TextBoxMode.MultiLine;
			CssClass = "NormalTextBox";
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
	}
}