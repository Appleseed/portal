// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomListDataType.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   Custom List Data Type
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.DataTypes
{
    using System;
    using System.Web.UI.WebControls;

    /// <summary>
    /// Custom List Data Type
    /// </summary>
    public class CustomListDataType : ListDataType<string, ListControl>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomListDataType"/> class.
        /// </summary>
        /// <param name="dataSource">
        /// The data source.
        /// </param>
        /// <param name="dataTextField">
        /// The data text field.
        /// </param>
        /// <param name="dataValueField">
        /// The data value field.
        /// </param>
        public CustomListDataType(object dataSource, string dataTextField, string dataValueField)
        {
            // this.Type = PropertiesDataType.List;
            this.InnerDataSource = dataSource;
            this.DataValueField = dataValueField;
            this.DataTextField = dataTextField;

            // InitializeComponents();
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets DataSource
        ///   Should be overridden from inherited classes
        /// </summary>
        /// <value>The data source.</value>
        public override object DataSource
        {
            get
            {
                var result = this.InnerDataSource;
                if (this.InnerDataSource is Delegate)
                {
                    result = ((Delegate)this.InnerDataSource).DynamicInvoke(null);
                }

                return result;
            }
        }

        /// <summary>
        ///   Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public override string Description
        {
            get
            {
                return "Custom List";
            }
        }

        #endregion
    }
}