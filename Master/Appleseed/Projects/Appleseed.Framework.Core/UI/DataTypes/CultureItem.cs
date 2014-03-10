using System;
using System.Globalization;

namespace Appleseed.Framework
{
    /// <summary>
    /// Single item in list. Language culture pair.
    /// </summary>
    public class LanguageCultureItem
    {
        private CultureInfo m_UICulture;
        private CultureInfo m_culture;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:LanguageCultureItem"/> class.
        /// </summary>
        /// <param name="uiCulture">The ui culture.</param>
        /// <param name="culture">The culture.</param>
        public LanguageCultureItem(CultureInfo uiCulture, CultureInfo culture)
        {
            if (uiCulture == null)
                UICulture = CultureInfo.InvariantCulture;
            else
                UICulture = uiCulture;

            if (culture == null)
                Culture = CultureInfo.InvariantCulture;
            else
                Culture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:LanguageCultureItem"/> class.
        /// </summary>
        public LanguageCultureItem()
        {
            UICulture = CultureInfo.InvariantCulture;
            Culture = CultureInfo.CreateSpecificCulture(CultureInfo.InvariantCulture.Name);
        }

        /// <summary>
        /// Gets or sets the UI culture.
        /// </summary>
        /// <value>The UI culture.</value>
        public CultureInfo UICulture
        {
            get { return m_UICulture; }
            set { m_UICulture = value; }
        }

        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        /// <value>The culture.</value>
        public CultureInfo Culture
        {
            get { return m_culture; }
            set
            {
                if (value.IsNeutralCulture)
                    throw new ArgumentException("Culture value cannot be neutral", "Culture");

                m_culture = value;
            }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return UICulture.Name + "/" + Culture.Name;
        }

        /// <summary>
        /// Implicit operators the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public static implicit operator string(LanguageCultureItem item)
        {
            return item.ToString();
        }

        //		public static bool operator==(LanguageCultureItem a, LanguageCultureItem b) 
        //		{
        //			return LanguageCultureItem.Equals(a, b);
        //		}

        //		public static bool operator!=(LanguageCultureItem a, LanguageCultureItem b) 
        //		{
        //			return !LanguageCultureItem.Equals(a, b);
        //		}

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
        /// </returns>
        public override bool Equals(Object obj)
        {
            if (obj.GetType() == typeof (LanguageCultureItem))
            {
                return Equals(this, (LanguageCultureItem) obj);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Equalses the specified a.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public static bool Equals(LanguageCultureItem a, LanguageCultureItem b)
        {
            if ((a == null) || (b == null))
            {
                return false;
            }
            if (a.ToString() == b.ToString())
            {
                return true;
            }
            return a.Equals(b);
        }

        /// <summary>
        /// We must override GetHashCode when we override Equals
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override Int32 GetHashCode()
        {
            return (Int32) ((UICulture.LCID*5000) + Culture.LCID);
        }
    }
}