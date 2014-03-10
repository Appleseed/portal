// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LanguageSwitcherEventArgs.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
// Language Switcher Event Arguments
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Web.UI.WebControls
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Language Switcher Event Arguments
    /// </summary>
    public class LanguageSwitcherEventArgs : EventArgs
    {
        #region Constants and Fields

        /// <summary>
        /// The culture item.
        /// </summary>
        private readonly LanguageCultureItem cultureItem;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageSwitcherEventArgs"/> class.
        /// </summary>
        /// <param name="uiCulture">The UI culture.</param>
        /// <param name="culture">The culture.</param>
        public LanguageSwitcherEventArgs(CultureInfo uiCulture, CultureInfo culture)
        {
            this.cultureItem = new LanguageCultureItem(uiCulture, culture);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageSwitcherEventArgs"/> class.
        /// </summary>
        /// <param name="cultureItem">The culture item.</param>
        public LanguageSwitcherEventArgs(LanguageCultureItem cultureItem)
        {
            this.cultureItem = cultureItem;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the culture item.
        /// </summary>
        public LanguageCultureItem CultureItem
        {
            get
            {
                return this.cultureItem;
            }
        }

        #endregion
    }
}