// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddItemPage.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   AddItemPage inherits from Appleseed.Framework.UI.SecurePage <br />
//   Used for add pages<br />
//   Can be inherited
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Web.UI
{
    using System;

    using Appleseed.Framework.Security;

    /// <summary>
    /// AddItemPage inherits from Appleseed.Framework.UI.SecurePage <br/>
    ///     Used for add pages<br/>
    ///     Can be inherited
    /// </summary>
    [History("jminond", "2005/03/10", "Tab to page conversion")]
    [History("jviladiu@portalServices.net", "2004/07/22", 
        "Added Security Access. Now inherits from Appleseed.Framework.UI.SecurePage")]
    [History("jviladiu@portalServices.net", "2004/07/22", "Clean Methods that only call to base")]
    [History("Jes1111", "2003/03/04", 
        "Smoothed out page event inheritance hierarchy - placed security checks and cache flushing")]
    public class AddItemPage : SecurePage
    {
        #region Methods

        /// <summary>
        /// Load settings
        /// </summary>
        protected override void LoadSettings()
        {
            // Verify that the current user has access to edit this module
            // Removed by Mario Endara <mario@softworks.com.uy> (2004/11/04)
            // if (PortalSecurity.HasAddPermissions(ModuleID) == false && PortalSecurity.IsInRoles("Admins") == false)
            if (PortalSecurity.HasAddPermissions(this.ModuleID) == false)
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
            // Removed by Mario Endara <mario@softworks.com.uy> (2004/11/04)
            // if (PortalSecurity.HasAddPermissions(ModuleID) == false && PortalSecurity.IsInRoles("Admins") == false)
            if (PortalSecurity.HasAddPermissions(this.ModuleID) == false)
            {
                PortalSecurity.AccessDeniedEdit();
            }

            base.OnUpdate(e);
        }

        #endregion
    }
}