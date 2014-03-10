// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IModuleSettings.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The module settings interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Site.Configuration
{
    using System;
    using System.Collections;

    /// <summary>
    /// The module settings interface.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public interface IModuleSettings
    {
        #region Properties

        /// <summary>
        ///   Gets or sets a value indicating whether this <see cref = "IModuleSettings" /> is admin.
        /// </summary>
        /// <value><c>true</c> if admin; otherwise, <c>false</c>.</value>
        /// <remarks>
        /// </remarks>
        bool Admin { get; set; }

        /// <summary>
        ///   Gets or sets the authorized add roles.
        /// </summary>
        /// <value>The authorized add roles.</value>
        /// <remarks>
        /// </remarks>
        string AuthorizedAddRoles { get; set; }

        /// <summary>
        ///   Gets or sets the authorized approve roles.
        /// </summary>
        /// <value>The authorized approve roles.</value>
        /// <remarks>
        /// </remarks>
        string AuthorizedApproveRoles { get; set; }

        /// <summary>
        ///   Gets or sets the authorized delete module roles.
        /// </summary>
        /// <value>The authorized delete module roles.</value>
        /// <remarks>
        /// </remarks>
        string AuthorizedDeleteModuleRoles { get; set; }

        /// <summary>
        ///   Gets or sets the authorized delete roles.
        /// </summary>
        /// <value>The authorized delete roles.</value>
        /// <remarks>
        /// </remarks>
        string AuthorizedDeleteRoles { get; set; }

        /// <summary>
        ///   Gets or sets the authorized edit roles.
        /// </summary>
        /// <value>The authorized edit roles.</value>
        /// <remarks>
        /// </remarks>
        string AuthorizedEditRoles { get; set; }

        /// <summary>
        ///   Gets or sets the authorized move module roles.
        /// </summary>
        /// <value>The authorized move module roles.</value>
        /// <remarks>
        /// </remarks>
        string AuthorizedMoveModuleRoles { get; set; }

        /// <summary>
        ///   Gets or sets the authorized properties roles.
        /// </summary>
        /// <value>The authorized properties roles.</value>
        /// <remarks>
        /// </remarks>
        string AuthorizedPropertiesRoles { get; set; }

        /// <summary>
        ///   Gets or sets the authorized publishing roles.
        /// </summary>
        /// <value>The authorized publishing roles.</value>
        /// <remarks>
        /// </remarks>
        string AuthorizedPublishingRoles { get; set; }

        /// <summary>
        ///   Gets or sets the authorized view roles.
        /// </summary>
        /// <value>The authorized view roles.</value>
        /// <remarks>
        /// </remarks>
        string AuthorizedViewRoles { get; set; }

        /// <summary>
        ///   Gets or sets the cache dependency.
        /// </summary>
        /// <value>The cache dependency.</value>
        /// <remarks>
        /// </remarks>
        ArrayList CacheDependency { get; set; }

        /// <summary>
        ///   Gets or sets the cache time.
        /// </summary>
        /// <value>The cache time.</value>
        /// <remarks>
        /// </remarks>
        int CacheTime { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether this <see cref = "IModuleSettings" /> is cacheable.
        /// </summary>
        /// <value><c>true</c> if cacheable; otherwise, <c>false</c>.</value>
        /// <remarks>
        /// </remarks>
        bool Cacheable { get; set; }

        /// <summary>
        ///   Gets or sets the desktop SRC.
        /// </summary>
        /// <value>The desktop SRC.</value>
        /// <remarks>
        /// </remarks>
        string DesktopSrc { get; set; }

        /// <summary>
        ///   Gets or sets the GUID ID.
        /// </summary>
        /// <value>The GUID ID.</value>
        /// <remarks>
        /// </remarks>
        Guid GuidID { get; set; }

        /// <summary>
        ///   Gets or sets the mobile SRC.
        /// </summary>
        /// <value>The mobile SRC.</value>
        /// <remarks>
        /// </remarks>
        string MobileSrc { get; set; }

        /// <summary>
        ///   Gets or sets the module def ID.
        /// </summary>
        /// <value>The module def ID.</value>
        /// <remarks>
        /// </remarks>
        int ModuleDefID { get; set; }

        /// <summary>
        ///   Gets or sets the module ID.
        /// </summary>
        /// <value>The module ID.</value>
        /// <remarks>
        /// </remarks>
        int ModuleID { get; set; }

        /// <summary>
        ///   Gets or sets the module order.
        /// </summary>
        /// <value>The module order.</value>
        /// <remarks>
        /// </remarks>
        int ModuleOrder { get; set; }

        /// <summary>
        ///   Gets or sets the module title.
        /// </summary>
        /// <value>The module title.</value>
        /// <remarks>
        /// </remarks>
        string ModuleTitle { get; set; }

        /// <summary>
        ///   Gets or sets the page ID.
        /// </summary>
        /// <value>The page ID.</value>
        /// <remarks>
        /// </remarks>
        int PageID { get; set; }

        /// <summary>
        ///   Gets or sets the name of the pane.
        /// </summary>
        /// <value>The name of the pane.</value>
        /// <remarks>
        /// </remarks>
        string PaneName { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether [show every where].
        /// </summary>
        /// <value><c>true</c> if [show every where]; otherwise, <c>false</c>.</value>
        /// <remarks>
        /// </remarks>
        bool ShowEveryWhere { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether [show mobile].
        /// </summary>
        /// <value><c>true</c> if [show mobile]; otherwise, <c>false</c>.</value>
        /// <remarks>
        /// </remarks>
        bool ShowMobile { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether [support collapsible].
        /// </summary>
        /// <value><c>true</c> if [support collapsible]; otherwise, <c>false</c>.</value>
        /// <remarks>
        /// </remarks>
        bool SupportCollapsable { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether [support workflow].
        /// </summary>
        /// <value><c>true</c> if [support workflow]; otherwise, <c>false</c>.</value>
        /// <remarks>
        /// </remarks>
        bool SupportWorkflow { get; set; }

        /// <summary>
        ///   Gets or sets the workflow status.
        /// </summary>
        /// <value>The workflow status.</value>
        /// <remarks>
        /// </remarks>
        WorkflowState WorkflowStatus { get; set; }

        #endregion
    }
}