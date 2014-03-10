// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditItemPage.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   EditItemPage inherits from Appleseed.Framework.UI.SecurePage <br />
//   Used for edit pages<br />
//   Can be inherited
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Web.UI
{
    using System;

    using Appleseed.Framework.Helpers;
    using Appleseed.Framework.Security;
    using Appleseed.Framework.Site.Data;

    /// <summary>
    /// EditItemPage inherits from Appleseed.Framework.UI.SecurePage <br/>
    ///     Used for edit pages<br/>
    ///     Can be inherited
    /// </summary>
    [History("jminond", "2005/03/10", "Tab to page conversion")]
    [History("jviladiu@portalServices.net", "2004/07/22", 
        "Added Security Access. Now inherits from Appleseed.Framework.UI.SecurePage")]
    [History("jviladiu@portalServices.net", "2004/07/22", "Clean Methods that only call to base")]
    [History("Jes1111", "2003/03/04", 
        "Smoothed out page event inheritance hierarchy - placed security checks and cache flushing")]
    public class EditItemPage : SecurePage
    {
        #region Methods

        /// <summary>
        /// Load settings
        /// </summary>
        protected override void LoadSettings()
        {
            // Verify that the current user has access to edit this module
            // Removed by Mario Endara <mario@softworks.com.uy> (2004/11/04)
            // if (PortalSecurity.HasEditPermissions(ModuleID) == false && PortalSecurity.IsInRoles("Admins") == false)
            if (PortalSecurity.HasEditPermissions(this.ModuleID) == false)
            {
                PortalSecurity.AccessDeniedEdit();
            }

            base.LoadSettings();
        }

        /// <summary>
        /// Handles OnCancel event
        /// </summary>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        protected override void OnCancel(EventArgs e)
        {
            base.OnCancel(e);
        }

        /// <summary>
        /// Handles OnDelete
        /// </summary>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        protected override void OnDelete(EventArgs e)
        {
            WorkFlowDB.SetLastModified(this.ModuleID, MailHelper.GetCurrentUserEmailAddress());
            base.OnDelete(e);
        }

        /// <summary>
        /// Handles OnInit event
        /// </summary>
        /// <param name="e">
        /// An <see cref="T:System.EventArgs"></see> that contains the event data.
        /// </param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        /// <summary>
        /// Handles OnUpdate event
        /// </summary>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        protected override void OnUpdate(EventArgs e)
        {
            WorkFlowDB.SetLastModified(this.ModuleID, MailHelper.GetCurrentUserEmailAddress());
            base.OnUpdate(e);
        }

        #endregion
    }
}