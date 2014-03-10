// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationUrlDataType.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   Application URL Data Type
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.DataTypes
{
    /// <summary>
    /// Application URL Data Type
    /// </summary>
    public class ApplicationUrlDataType : UrlDataType
    {
        #region Properties

        /// <summary>
        ///   URL relative to Application
        /// </summary>
        /// <value>The description.</value>
        public override string Description
        {
            get
            {
                return "URL relative to Application";
            }
        }

        #endregion
    }
}