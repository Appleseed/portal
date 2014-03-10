// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleMenuType.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   SimpleMenuType
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Content.Web.Modules
{
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Appleseed.Framework;
    using Appleseed.Framework.Site.Configuration;
    using Appleseed.Framework.Web.UI.WebControls;

    /// <summary>
    /// Simple Menu Type
    /// </summary>
    public class SimpleMenuType : UserControl
    {
        #region Constants and Fields

        /// <summary>
        /// The menu bind option.
        /// </summary>
        private BindOption menuBindOption = BindOption.BindOptionNone;

        /// <summary>
        /// The menu repeat direction.
        /// </summary>
        private RepeatDirection menuRepeatDirection;

        /// <summary>
        /// The parent tab id.
        /// </summary>
        private int parentTabId;

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the global portal settings.
        /// </summary>
        /// <value>The global portal settings.</value>
        public PortalSettings GlobalPortalSettings { get; set; }

        /// <summary>
        ///   Gets the menu bind option.
        /// </summary>
        /// <value>The menu bind option.</value>
        public BindOption MenuBindOption
        {
            get
            {
                if (this.ModuleSettings["sm_MenuBindingType"] != null)
                {
                    this.menuBindOption =
                        (BindOption)int.Parse("0" + this.ModuleSettings["sm_MenuBindingType"].ToString());
                }

                return this.menuBindOption;
            }
        }

        /// <summary>
        ///   Gets the menu repeat direction.
        /// </summary>
        /// <value>The menu repeat direction.</value>
        public RepeatDirection MenuRepeatDirection
        {
            get
            {
                if (this.ModuleSettings["sm_Menu_RepeatDirection"] != null &&
                    this.ModuleSettings["sm_Menu_RepeatDirection"].ToString() == "0")
                {
                    this.menuRepeatDirection = RepeatDirection.Horizontal;
                }
                else
                {
                    this.menuRepeatDirection = RepeatDirection.Vertical;
                }

                return this.menuRepeatDirection;
            }
        }

        /// <summary>
        ///   Gets or sets the module settings.
        /// </summary>
        /// <value>The module settings.</value>
        public Dictionary<string, ISettingItem> ModuleSettings { get; set; }

        /// <summary>
        ///   Gets the parent page ID.
        /// </summary>
        /// <value>The parent page ID.</value>
        public int ParentPageID
        {
            get
            {
                if (this.ModuleSettings["sm_ParentPageID"] != null)
                {
                    this.parentTabId = int.Parse(this.ModuleSettings["sm_ParentPageID"].ToString());
                }

                return this.parentTabId;
            }
        }

        #endregion
    }
}