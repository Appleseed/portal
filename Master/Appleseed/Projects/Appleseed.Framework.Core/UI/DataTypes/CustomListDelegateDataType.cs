// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomListDelegateDataType.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   The custom list delegate data type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.DataTypes
{
    using System;
    using System.Collections;
    using System.Web.UI.WebControls;

    /// <summary>
    /// The custom list delegate data type.
    /// </summary>
    public class CustomListDelegateDataType : ListDataType<string, ListControl>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomListDelegateDataType"/> class.
        /// </summary>
        /// <param name="function">
        /// The function.
        /// </param>
        /// <param name="dataTextField">
        /// The data text field.
        /// </param>
        /// <param name="dataValueField">
        /// The data value field.
        /// </param>
        public CustomListDelegateDataType(
            InitializeCustomSettingsDelegate function, string dataTextField, string dataValueField)
        {
            // this.Type = PropertiesDataType.List;
            this.InnerDataSource = function;
            this.DataValueField = dataValueField;
            this.DataTextField = dataTextField;
        }

        #endregion

        #region Delegates

        /// <summary>
        /// The initialize custom settings delegate.
        /// </summary>
        /// <returns>A sorted list of settings.</returns>
        public delegate SortedList InitializeCustomSettingsDelegate();

        #endregion

        #region Properties

        /// <summary>
        /// Gets DataSource.
        /// </summary>
        public override object DataSource
        {
            get
            {
                return ((Delegate)this.InnerDataSource).DynamicInvoke(null);
            }
        }

        /// <summary>
        /// Gets Description.
        /// </summary>
        public override string Description
        {
            get
            {
                return "Custom List with Delegate";
            }
        }

        #endregion
    }
}