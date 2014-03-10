using System;
using Appleseed.Framework.Web.UI;

namespace Appleseed.Content.Web.ModulesVersion
{
    /// <summary>
    /// Summary description for RbVersion.
    /// </summary>
    public partial class RbVersion : Page
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            // Put user code to initialize the page here
        }

        #region Web Form Designer generated code

        /// <summary>
        /// Handles the OnInit event at Page level<br/>
        /// Performs OnInit events that are common to all Pages<br/>
        /// Can be overridden
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(this.Page_Load);
            base.OnInit(e);
        }

        #endregion
    }
}