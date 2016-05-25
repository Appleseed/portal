using Appleseed.Framework.Site.Configuration;
using Appleseed.Framework.Web.UI.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Appleseed.Framework.UI.WebControls
{
    /// <summary>
    /// class CKEditor
    /// </summary>
    public class CKEditorTextbox : CKEditor.NET.CKEditorControl, IHtmlEditor
    {
        /// <summary>
        /// Control Text
        /// </summary>
        /// <value></value>
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private string _imageFolder = string.Empty;

        /// <summary>
        /// Image Folder string
        /// </summary>
        public string ImageFolder
        {
            get
            {
                if (_imageFolder == string.Empty)
                {
                    PortalSettings pS = (PortalSettings)HttpContext.Current.Items["PortalSettings"];
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
