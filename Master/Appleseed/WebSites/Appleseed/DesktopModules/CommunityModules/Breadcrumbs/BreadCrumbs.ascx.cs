using System;
using Appleseed.Framework.Web.UI.WebControls;

namespace Appleseed.Content.Web.Modules
{
    /// <summary>
    /// BreadCrumbs Module
    /// </summary>
    public partial class BreadCrumbs : PortalModuleControl
    {
        /// <summary>
        /// Public constructor. Sets base settings for module.
        /// </summary>
        public BreadCrumbs()
        {
        }

        /// <summary>
        /// The Page_Load event handler on this User Control is used to
        /// databind the used BreadCrumbsControl.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            //BreadCrumbs1.DataBind();
        }

        #region general Module Implementation

        /// <summary>
        /// Gets the GUID ID.
        /// </summary>
        /// <value>The GUID ID.</value>
        public override Guid GuidID
        {
            get { return new Guid("{D3182CD6-DAFF-4E72-AD9E-0B28CB44F007}"); }
        }

        #endregion

        #region Web Form Designer generated code

        /// <summary>
        /// Raises the init event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(this.Page_Load);
            base.OnInit(e);
        }

        #endregion
    }
}