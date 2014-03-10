// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModuleListDataType.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   Module List Data Type
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.DataTypes
{
    using System;
    using System.Web;
    using System.Web.UI.WebControls;

    using Appleseed.Framework.Site.Configuration;
    using Appleseed.Framework.Site.Data;

    /// <summary>
    /// Module List Data Type
    /// </summary>
    public class ModuleListDataType : ListDataType<string, ListControl>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleListDataType"/> class.
        /// </summary>
        /// <param name="moduleType">
        /// The Module name
        /// </param>
        public ModuleListDataType(string moduleType)
        {
            this.InnerDataSource = moduleType;

            // InitializeComponents();
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets DataSource
        ///   Should be overridden from inherited classes
        /// </summary>
        /// <value>
        ///   The data source.
        /// </value>
        public override object DataSource
        {
            get
            {
                // Obtain PortalSettings from Current Context
                var PortalSettings = (PortalSettings)HttpContext.Current.Items["PortalSettings"];
                return new ModulesDB().GetModulesByName(this.InnerDataSource.ToString(), PortalSettings.PortalID);
            }
        }

        /// <summary>
        ///   Gets or sets the data text field.
        /// </summary>
        /// <value>The data text field.</value>
        public override string DataTextField
        {
            get
            {
                return "ModuleTitle";
            }

            set
            {
                throw new ArgumentException("ModuleTitle cannot be set", "value");
            }
        }

        /// <summary>
        ///   Gets or sets the data value field.
        /// </summary>
        /// <value>The data value field.</value>
        public override string DataValueField
        {
            get
            {
                return "ModuleID";
            }

            set
            {
                throw new ArgumentException("ModuleID cannot be set", "value");
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
                return "Module List";
            }
        }

        #endregion
    }
}