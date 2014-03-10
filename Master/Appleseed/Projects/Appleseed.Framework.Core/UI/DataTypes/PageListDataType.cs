// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageListDataType.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   Page List Data Type
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.DataTypes
{
    using System.Collections.Generic;
    using System.Web;

    using Appleseed.Framework.Site.Configuration;
    using Appleseed.Framework.Site.Data;

    /// <summary>
    /// Page List Data Type
    /// </summary>
    public class PageListDataType : CustomListDataType
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PageListDataType" /> class. 
        ///   The page list data type.
        /// </summary>
        public PageListDataType()
            : base(GetPageList(), "Name", "Id")
        {
            // this.Type = PropertiesDataType.PageList;
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
                return "Pages List";
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the default value.
        /// </summary>
        /// <returns>
        /// The default value.
        /// </returns>
        public string GetDefaultValue()
        {
            return ((List<PageItem>)this.InnerDataSource)[0].ID.ToString();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the page list.
        /// </summary>
        /// <returns>
        /// The page list.
        /// </returns>
        private static List<PageItem> GetPageList()
        {
            var PortalSettings = (PortalSettings)HttpContext.Current.Items["PortalSettings"];
            return new PagesDB().GetPagesFlat(PortalSettings.PortalID);
        }

        #endregion
    }
}