using System.Web;
using FredCK.FCKeditorV2;
using Appleseed.Framework.Site.Configuration;

namespace Appleseed.Framework.Web.UI.WebControls
{
    /// <summary>
    /// FCKTextBoxV2 is a wrapper for FredCK.FCKeditorV2.
    /// </summary>
    [History("jviladiu@portalservices.net", "2004/11/09", "Implementation of FCKEditor Version 2 in Appleseed")]
    public class FCKTextBoxV2 : FCKeditor, IHtmlEditor
    {
        /// <summary>
        /// Control Text
        /// </summary>
        /// <value></value>
        public string Text
        {
            get { return Value; }
            set { Value = value; }
        }

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
    }
}