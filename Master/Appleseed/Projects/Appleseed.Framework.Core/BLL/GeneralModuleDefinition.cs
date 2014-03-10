// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeneralModuleDefinition.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The general module definition.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
namespace Appleseed.Framework.Core.BLL
{
    /// <summary>
    /// The general module definition.
    /// </summary>
    public class GeneralModuleDefinition
    {
        #region Constants and Fields

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether Admin.
        /// </summary>
        public bool Admin { get; set; }

        /// <summary>
        /// Gets or sets AssemblyName.
        /// </summary>
        public string AssemblyName { get; set; }

        /// <summary>
        /// Gets or sets ClassName.
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// Gets or sets DesktopSource.
        /// </summary>
        public string DesktopSource { get; set; }

        /// <summary>
        /// Gets or sets FriendlyName.
        /// </summary>
        public string FriendlyName { get; set; }

        /// <summary>
        /// Gets or sets GeneralModDefID.
        /// </summary>
        public Guid GeneralModDefID { get; set; }

        /// <summary>
        /// Gets or sets MobileSource.
        /// </summary>
        public string MobileSource { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Searchable.
        /// </summary>
        public bool Searchable { get; set; }

        #endregion
    }
}