// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SolutionModuleDefinition.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   The Solution Module Definition.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Site.Data
{
    using System;

    /// <summary>
    /// The Solution Module Definition.
    /// </summary>
    public class SolutionModuleDefinition
    {
        #region Properties

        /// <summary>
        ///   Gets or sets the general module definition id.
        /// </summary>
        /// <value>
        ///   The general module definition id.
        /// </value>
        public Guid GeneralModuleDefinitionId { get; set; }

        /// <summary>
        ///   Gets or sets the solution module definition id.
        /// </summary>
        /// <value>
        ///   The solution module definition id.
        /// </value>
        public int SolutionModuleDefinitionId { get; set; }

        /// <summary>
        ///   Gets or sets the solutions id.
        /// </summary>
        /// <value>
        ///   The solutions id.
        /// </value>
        public int SolutionsId { get; set; }

        #endregion
    }
}