// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UrlDataType.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   URL Data Type
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.DataTypes
{
    using System;
    using System.Web.UI.WebControls;

    /// <summary>
    /// URL Data Type
    /// </summary>
    public class UrlDataType : BaseDataType<Uri, TextBox>
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "UrlDataType" /> class.
        /// </summary>
        public UrlDataType()
        {
            // this.Type = PropertiesDataType.String;
            this.Value = new Uri("http://localhost");

            // InitializeComponents();
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public override string Description
        {
            get
            {
                return "Full valid URI";
            }
        }

        #endregion
    }
}