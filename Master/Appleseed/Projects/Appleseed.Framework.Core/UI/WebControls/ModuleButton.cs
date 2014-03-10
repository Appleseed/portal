using System;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Appleseed.Framework;
using Appleseed.Framework.Settings;

namespace Appleseed.Framework.Web.UI.WebControls
{
	/// <summary>
	///  ModuleButton. Custom control for Module buttons ... derives from HtmlAnchor. 
	///  Allows text, image or both. Can be url hyperlink or postback button.
	///  Use ServerClick event to pick up on postback.
	/// </summary>
	public class ModuleButton : HtmlAnchor
	{
		/// <summary>
		/// 
		/// </summary>
		protected HtmlAnchor moduleButton;

		#region Constructor
		/// <summary>
		/// Constructor
		/// </summary>
		public ModuleButton()
		{
		}
		#endregion

		#region Events
		/// <summary>
		/// Init event handler.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
		override protected void OnInit(EventArgs e)
		{
			this.EnableViewState = false;
			base.OnInit(e);
		}

		/// <summary>
		/// Load event handler
		/// </summary>
		/// <param name="e">The <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
		protected override void OnLoad(EventArgs e)
		{
			if (this.TranslationKey.Length != 0)
				this.InnerText = General.GetString(this.TranslationKey, this.EnglishName, this);
			if (this.InnerText == string.Empty)
				this.InnerText = this.EnglishName;

			this.HRef = HttpContext.Current.Server.HtmlEncode(this.HRef);

			switch (this.RenderAs)
			{
				case RenderOptions.TextOnly:
					this.Style.Add("display", "block");
					this.Attributes.Add("class", this.CssClass);
					this.InnerHtml = string.Concat("<div class=\"btn-txt-only\">", this.InnerText, "</div>");
					// Jes1111 - 27/Nov/2004 - changed popup window handling
					if (this.PopUp)
					{
						this.Attributes.Add("onclick", GetPopupCommand());
						this.Attributes.Add("target", this.Target);
						this.Attributes.Add("class", string.Concat(this.CssClass, " btn-is-popup btn-txt-only"));
					}
					else
						this.Attributes.Add("class", string.Concat(this.CssClass, " btn-txt-only"));
					break;


				case RenderOptions.ImageAndTextCSS:
					this.Style.Add("display", "block");
					this.Style.Add("background-image", string.Concat("url(", this.Image.ImageUrl, ")"));
					this.Style.Add("background-repeat", "no-repeat");
					this.Style.Add("height", this.Image.Height.ToString());
					this.Style.Add("width", this.Image.Width.ToString());
					this.InnerHtml = string.Concat("<div class=\"btn-img-txt\">", this.InnerText, "</div>");
					// Jes1111 - 27/Nov/2004 - changed popup window handling
					if (this.PopUp)
					{
						this.Attributes.Add("onclick", GetPopupCommand());
						this.Attributes.Add("target", this.Target);
						this.Attributes.Add("class", string.Concat(this.CssClass, " btn-is-popup btn-img-txt"));
					}
					else
						this.Attributes.Add("class", string.Concat(this.CssClass, " btn-img-txt"));

					break;


				case RenderOptions.ImageOnlyCSS:
					this.Title = string.Concat(this.InnerText, "...");
					this.Style.Add("display", "block");
					this.Style.Add("background-image", string.Concat("url(", this.Image.ImageUrl, ")"));
					this.Style.Add("background-repeat", "no-repeat");
					this.Style.Add("height", this.Image.Height.ToString());
					this.Style.Add("width", this.Image.Width.ToString());
					this.InnerHtml = string.Concat("<div class=\"btn-img-only-css\">", this.InnerText, "</div>");
					// Jes1111 - 27/Nov/2004 - changed popup window handling
					if (this.PopUp)
					{
						this.Attributes.Add("onclick", GetPopupCommand());
						this.Attributes.Add("target", this.Target);
						this.Attributes.Add("class", string.Concat(this.CssClass, " btn-is-popup btn-img-only-css"));
					}
					else
						this.Attributes.Add("class", string.Concat(this.CssClass, " btn-img-only-css"));
					break;

				case RenderOptions.ImageOnly:
				default:
					//this.Title = string.Concat(this.InnerText,"...");

					//Manu FIX: Image caption was not showed in Image only style
					//this.Image.AlternateText = string.Concat(this.InnerText,"...");
					// fixed again by Jes1111 - 18/01/2004 - tooltips were not showing in Gecko browsers
					this.Attributes.Add("title", string.Concat(this.InnerText, "..."));

					this.Style.Add("display", "block");
					// Jes1111 - 27/Nov/2004 - changed popup window handling
					if (this.PopUp)
					{
						this.Attributes.Add("onclick", GetPopupCommand());
						this.Attributes.Add("target", this.Target);
						this.Attributes.Add("class", string.Concat(this.CssClass, " btn-is-popup btn-img-only"));
					}
					else
						this.Attributes.Add("class", string.Concat(this.CssClass, " btn-img-only"));
					this.Image.BorderStyle = BorderStyle.None;
					// Hongwei Shen(hongwei.shen@gmail.com) 11/9/2005 
					// use the AlternateText to display the tool tip for the button
					this.image.AlternateText = this.InnerText;
					// end of modification
					this.InnerText = string.Empty;
					this.Controls.Add(this.Image);
					break;
			}
			this.EnsureChildControls();

			base.OnLoad(e);
		}

		// Jes1111 - 27/Nov/2004 - changed popup window handling
		/// <summary>
		/// Builds Javascript popup command
		/// </summary>
		/// <returns>popup command as a string</returns>
		protected string GetPopupCommand()
		{
			// make sure the popup script is registered
			if (!((Page)this.Page).IsClientScriptRegistered("rb-popup"))
				((Page)this.Page).RegisterClientScript("rb-popup", Path.ApplicationRoot + "/aspnet_client/popupHelper/popup.js");

			// build the popup command
			StringBuilder sb = new StringBuilder();
			sb.Append("link_popup(this");
			if (this.PopUpOptions.Length != 0)
			{
				sb.Append(", '");
				sb.Append(this.PopUpOptions);
				sb.Append("'");
			}
			sb.Append(");return false;");

			return sb.ToString();
		}

		// Jes1111 - 27/Nov/2004 - changed popup window handling
		//		protected override void OnPreRender(EventArgs e)
		//		{
		//			//popup removed until fixed	
		//			if ( this.PopUp )
		//			{
		//				if ( !((Appleseed.Framework.Web.UI.Page)this.Page).IsClientScriptRegistered("rb-popup") )
		//				((Appleseed.Framework.Web.UI.Page)this.Page).RegisterClientScript("rb-popup",Appleseed.Framework.Settings.Path.ApplicationRoot + "/aspnet_client/popupHelper/popup.js");
		//
		//				if ( !((Appleseed.Framework.Web.UI.Page)this.Page).IsClientPopUpEventListenerRegistered(this.Target) )
		//				{
		//					StringBuilder sb = new StringBuilder();
		//					sb.Append("mlisten('click', getElementsByClass('");
		//					sb.Append(this.Target);
		//					sb.Append("','a')");
		//					if ( this.PopUpOptions.Length != 0 )
		//					{
		//						sb.Append(", event_popup_features('");
		//						sb.Append(this.PopUpOptions);
		//						sb.Append("')");
		//					}
		//					sb.Append(");");
		//					((Appleseed.Framework.Web.UI.Page)this.Page).RegisterClientPopUpEventListener(this.Target, sb.ToString());
		//				}
		//			}
		//			//popup removed until fixed
		//			base.OnPreRender(e);
		//		}

		#endregion

		#region Properties

		private string targetID = string.Empty;
		/// <summary>
		/// Gets the target ID.
		/// </summary>
		/// <value>The target ID.</value>
		private string TargetID
		{
			get
			{
				if (HttpContext.Current != null)
					targetID = string.Concat(this.Target, ((Page)this.Page).PageID);
				return targetID;
			}
		}

		private bool popUp = false;
		/// <summary>
		/// Gets or sets a value indicating whether [pop up].
		/// </summary>
		/// <value><c>true</c> if [pop up]; otherwise, <c>false</c>.</value>
		public bool PopUp
		{
			get { return popUp; }
			set { popUp = value; }
		}

		private string popUpOptions = string.Empty;
		/// <summary>
		/// Gets or sets the pop up options.
		/// </summary>
		/// <value>The pop up options.</value>
		public string PopUpOptions
		{
			get { return popUpOptions; }
			set { popUpOptions = value; }
		}

		private string name = string.Empty;
		/// <summary>
		/// Hides inherited Name
		/// </summary>
		/// <value></value>
		/// <returns>The bookmark name.</returns>
		new private string Name
		{
			get { return name; }
			set { name = value; }
		}

		private string cssClass = "rb_mod_btn";
		/// <summary>
		/// Sets CSS Class on control
		/// </summary>
		/// <value>The CSS class.</value>
		public string CssClass
		{
			get { return cssClass; }
			set { cssClass = value; }
		}

		private ButtonGroup group = ButtonGroup.User;
		/// <summary>
		/// Holds Button group enum: User, Admin or Custom
		/// </summary>
		/// <value>The group.</value>
		public ButtonGroup Group
		{
			get { return group; }
			set { group = value; }
		}

		private Image image = new Image();
		/// <summary>
		/// Button image
		/// </summary>
		/// <value>The image.</value>
		public Image Image
		{
			get { return image; }
			set { image = value; }
		}

		private string translation = string.Empty;
		/// <summary>
		/// Esperantus translation
		/// </summary>
		/// <value>The name of the english.</value>
		public string EnglishName
		{
			get { return translation; }
			set { translation = value; }
		}

		private string translationKey = string.Empty;
		/// <summary>
		/// Esperantus translation key
		/// </summary>
		/// <value>The translation key.</value>
		public string TranslationKey
		{
			get { return translationKey; }
			set { translationKey = value; }
		}

		private RenderOptions renderAs = RenderOptions.ImageOnly;
		/// <summary>
		/// Active RenderAs option
		/// </summary>
		/// <value>The render as.</value>
		public RenderOptions RenderAs
		{
			get { return renderAs; }
			set { renderAs = value; }
		}
		#endregion

		#region Enums
		/// <summary>
		/// Rendering options
		/// </summary>
		public enum RenderOptions : int
		{
			/// <summary>
			/// 
			/// </summary>
			ImageOnly = 0,
			/// <summary>
			/// 
			/// </summary>
			ImageOnlyCSS = 1,
			/// <summary>
			/// 
			/// </summary>
			TextOnly = 2,
			/// <summary>
			/// 
			/// </summary>
			ImageAndTextCSS = 3

		}

		/// <summary>
		/// Group options
		/// </summary>
		public enum ButtonGroup : int
		{
			/// <summary>
			/// 
			/// </summary>
			User = 0,
			/// <summary>
			/// 
			/// </summary>
			Admin = 1,
			/// <summary>
			/// 
			/// </summary>
			Custom = 2
		}
		#endregion

	}
}
