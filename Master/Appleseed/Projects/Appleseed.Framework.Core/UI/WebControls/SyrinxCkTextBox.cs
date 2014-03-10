using System.Web;
using Appleseed.Framework.Site.Configuration;


namespace Appleseed.Framework.Web.UI.WebControls
{
    /// <summary>
    /// 
    /// </summary>
    public class SyrinxCkTextBox : Syrinx.Gui.AspNet.CkEditor, IHtmlEditor
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

        /// <summary>
        /// Returns the html as a string that will be rendered to the client
        /// </summary>
        /// <returns>The html that is going to be rendered</returns>
        protected override string getTextForRender()
        {
            var result = base.getTextForRender();
            //this is a hack in order to allow the SyrinxCkTextBox to render scripts.
            return result.Replace("</script>", "</scr\" + \"ipt>");
        }


    }
}
