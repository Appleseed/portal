using System;
using System.Web;
using System.Web.UI;
using Appleseed.Framework.Site.Configuration;

namespace Appleseed.Framework.Web.UI.WebControls
{
    /// <summary>
    /// Summary description for ZenHeaderTitle.
    /// </summary>
    public class ZenHeaderTitle : HeaderTitle
    {
        /// <summary>
        /// 
        /// </summary>
        protected string _imageUrl = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        protected bool _showImage = true;

        /// <summary>
        /// Gets or sets a value indicating whether [show image].
        /// </summary>
        /// <value><c>true</c> if [show image]; otherwise, <c>false</c>.</value>
        public virtual bool ShowImage
        {
            get { return _showImage; }
            set { _showImage = value; }
        }

        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        /// <value>The image URL.</value>
        public virtual string ImageUrl
        {
            get { return _imageUrl; }
            set { _imageUrl = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZenHeaderTitle"/> class.
        /// </summary>
        public ZenHeaderTitle()
        {
            EnableViewState = false;
            Load += new EventHandler(LoadControl);
        }

        /// <summary>
        /// Loads the control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public void LoadControl(object sender, EventArgs e)
        {
            if (HttpContext.Current != null)
            {
                // Obtain PortalSettings from Current Context
                PortalSettings PortalSettings = (PortalSettings) HttpContext.Current.Items["PortalSettings"];

                // PortalTitle                
                Text = PortalSettings.PortalName;
            }
        }

        /// <summary>
        /// Overrides Render to produce structure suitable for Zen
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object that receives the control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (ShowImage)
                // show image
                //<h1 id="portaltitle" class="portaltitle">Appleseed Portal<span></span></h1>
            {
                writer.Write("<h1 id=\"portaltitle\" class=\"portaltitle\">");
                writer.Write(Text);
                writer.Write("<span></span></h1>");
            }
            else
                // show text 
                //<h1 class="portaltitle">Appleseed Portal</h1>
            {
                writer.Write("<h1 class=\"portaltitle\">");
                writer.Write(Text);
                writer.Write("</h1>");
            }
        }
    }
}