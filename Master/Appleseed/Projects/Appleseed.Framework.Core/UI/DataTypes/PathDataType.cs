// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PathDataType.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   Path Data Type
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.DataTypes
{
    using System.Web.UI.WebControls;

    /// <summary>
    /// Path Data Type
    /// </summary>
    public class PathDataType : BaseDataType<string, TextBox>
    {
        #region Properties

        /// <summary>
        ///   Gets the description.
        /// </summary>
        public override string Description
        {
            get
            {
                return "File System path";
            }
        }

        /// <summary>
        ///   Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public override string Value
        {
            get
            {
                return base.Value;
            }

            set
            {
                value = value.Replace("/", "\\");
                base.Value = value;
            }
        }

        #endregion
    }
}