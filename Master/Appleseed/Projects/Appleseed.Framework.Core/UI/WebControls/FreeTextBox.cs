using System.Web;
using Appleseed.Framework.Site.Configuration;

namespace Appleseed.Framework.Web.UI.WebControls
{
    /// <summary>
    ///  FreeTextBox is a wrapper for FreeTextBoxControls.FreeTextBox
    /// </summary>
    public class FreeTextBox : FreeTextBoxControls.FreeTextBox, IHtmlEditor
    {
        private string _imageFolder = string.Empty;

        /// <summary>
        /// Control Image Folder
        /// </summary>
        /// <value></value>
        public string ImageFolder
        {
            get
            {
                if (_imageFolder == string.Empty)
                {
                    PortalSettings pS = (PortalSettings) HttpContext.Current.Items["PortalSettings"];
                    if (pS.CustomSettings != null)
                    {
                        if (pS.CustomSettings["SITESETTINGS_DEFAULT_IMAGE_FOLDER"] != null)
                        {
                            _imageFolder = pS.CustomSettings["SITESETTINGS_DEFAULT_IMAGE_FOLDER"].ToString();
                        }
                    }
                }
                return "/images/" + _imageFolder;
            }
            set { _imageFolder = value; }
        }

/* Unused in FTB 3.0. Commented for optional use of staticdust image gallery replacing the included in ftb library
		public string ImageGalleryUrl
		{
			get
			{
				string JavascriptLink;

				Type UploadDialogType = Type.GetType("StaticDust.Web.UI.Controls.UploadDialogButton, StaticDust.Web.UI.Controls.UploadDialog");
				if (UploadDialogType == null) return string.Empty;

				object UploadDialog = UploadDialogType.InvokeMember(null, (BindingFlags.CreateInstance | (BindingFlags.NonPublic | (BindingFlags.Public | (BindingFlags.Instance | BindingFlags.DeclaredOnly)))), null, null, new object[0]);
				if (UploadDialog == null) return string.Empty;

				object[] objArray = new object[1];
				objArray[0] = this.ImageGalleryPath;
				UploadDialogType.InvokeMember("UploadDirectory", (BindingFlags.SetProperty | (BindingFlags.NonPublic | (BindingFlags.Public | (BindingFlags.Instance | BindingFlags.DeclaredOnly)))), null, UploadDialog, objArray);

				objArray[0] = "FTB_ReturnImageFromGallery()";
				UploadDialogType.InvokeMember("ReturnFunction", (BindingFlags.SetProperty | (BindingFlags.NonPublic | (BindingFlags.Public | (BindingFlags.Instance | BindingFlags.DeclaredOnly)))), null, UploadDialog, objArray);

				JavascriptLink = ((string) UploadDialogType.InvokeMember("JavascriptLink", (BindingFlags.GetProperty | (BindingFlags.NonPublic | (BindingFlags.Public | (BindingFlags.Instance | BindingFlags.DeclaredOnly)))), null, UploadDialog, null));
				Match match1 = Regex.Match(JavascriptLink, "\'(?<link>[^\']+)\'", RegexOptions.IgnoreCase);
				if (match1.Success)
				{
					return match1.Groups["link"].Value;
				}
				return string.Empty;
			}
		}
*/
    }
}