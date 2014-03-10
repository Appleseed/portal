// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewItemPage.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   The view item page.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Web.UI
{
    using System;

    using Appleseed.Framework.Security;

    /// <summary>
    /// The view item page.
    /// </summary>
    [History("jviladiu@portalServices.net", "2004/07/22", 
        "Added Security Access. Now inherits from Appleseed.Framework.UI.SecurePage")]
    [History("jviladiu@portalServices.net", "2004/07/22", "Clean Methods that only call to base")]
    public class ViewItemPage : SecurePage
    {
        #region Methods

        /// <summary>
        /// Load settings
        /// </summary>
        protected override void LoadSettings()
        {
            // Verify that the current user has access to view this module
            if (PortalSecurity.HasViewPermissions(this.ModuleID) == false)
            {
                // Removed by Mario Endara <mario@softworks.com.uy> (2004/11/04)
                // && PortalSecurity.IsInRoles("Admins") == false)
                PortalSecurity.AccessDenied();
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
            // Verify that the current user has access to add in this module
            if (PortalSecurity.HasPropertiesPermissions(this.ModuleID) == false)
            {
                // Removed by Mario Endara <mario@softworks.com.uy> (2004/11/04)
                // && PortalSecurity.IsInRoles("Admins") == false)
                PortalSecurity.AccessDeniedEdit();
            }

            base.OnUpdate(e);
        }

        #endregion
    }
}