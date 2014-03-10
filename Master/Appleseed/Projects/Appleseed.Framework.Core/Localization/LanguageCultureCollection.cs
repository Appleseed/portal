// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LanguageCultureCollection.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   LanguageCultureCollection
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Web.UI.WebControls
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Language Culture Collection
    /// </summary>
    [TypeConverter(typeof(TypeConverterLanguageCultureCollection))]
    public class LanguageCultureCollection : CollectionBase, ICollection
    {
        #region Constants and Fields

        /// <summary>
        ///   The items separator.
        /// </summary>
        private readonly char[] itemsSeparator = { ';' };

        /// <summary>
        ///   The key value separator.
        /// </summary>
        private readonly char[] keyValueSeparator = { '=' };

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "LanguageCultureCollection" /> class.
        /// </summary>
        public LanguageCultureCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageCultureCollection"/> class.
        /// </summary>
        /// <param name="languageCultureCollection">
        /// The language culture collection.
        /// </param>
        public LanguageCultureCollection(string languageCultureCollection)
        {
            var mylist = (LanguageCultureCollection)languageCultureCollection;

            foreach (LanguageCultureItem l in mylist)
            {
                this.Add(l);
            }
        }

        #endregion

        #region Indexers

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <returns>The element at the specified index.</returns>
        /// <param name="i">The index of the element.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">Index is not a valid index in the <see cref="T:System.Collections.IList"/>.</exception>
        /// <exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.IList"/> is read-only.</exception>
        public LanguageCultureItem this[int i]
        {
            get
            {
                return (LanguageCultureItem)this.InnerList[i];
            }

            // set{InnerList[i] = value;}
        }

        #endregion

        #region Operators

        /// <summary>
        ///   Explicitly converts String to LanguageCultureCollection value
        /// </summary>
        /// <returns></returns>
        public static explicit operator LanguageCultureCollection(string languageList)
        {
            var converter = TypeDescriptor.GetConverter(typeof(LanguageCultureCollection));
            if (converter == null)
            {
                throw new ArgumentOutOfRangeException("languageList", "Cannot load type converter.");
            }

            return (LanguageCultureCollection)converter.ConvertTo(languageList, typeof(LanguageCultureCollection));
        }

        /// <summary>
        ///   Explicitly converts LanguageCultureCollection to String value
        /// </summary>
        /// <returns></returns>
        public static explicit operator string(LanguageCultureCollection langList)
        {
            var converter = TypeDescriptor.GetConverter(typeof(LanguageCultureCollection));
            if (converter == null)
            {
                throw new ArgumentOutOfRangeException("langList", "Cannot load type converter.");
            }

            return (string)converter.ConvertTo(langList, typeof(string));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        public void Add(LanguageCultureItem item)
        {
            this.InnerList.Add(item);
        }

        /// <summary>
        /// Determines whether [contains] [the specified item].
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// <c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool Contains(LanguageCultureItem item)
        {
            return this.InnerList.Contains(item);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">
        /// The array.
        /// </param>
        /// <param name="index">
        /// The index.
        /// </param>
        public void CopyTo(LanguageCultureItem[] array, int index)
        {
            ((ICollection)this).CopyTo(array, index);
        }

        /// <summary>
        /// Returns the best possible LanguageCultureItem
        ///   matching the provided culture
        /// </summary>
        /// <param name="culture">
        /// The culture.
        /// </param>
        /// <returns>
        /// </returns>
        public LanguageCultureItem GetBestMatching(CultureInfo culture)
        {
            return this.GetBestMatching(new[] { culture });
        }

        /// <summary>
        /// Returns the best possible LanguageCultureItem
        ///   matching cultures in provided list
        /// </summary>
        /// <param name="cultures">
        /// The cultures.
        /// </param>
        /// <returns>
        /// </returns>
        public LanguageCultureItem GetBestMatching(CultureInfo[] cultures)
        {
            // If null return default
            if (cultures == null || cultures.Length == 0 || cultures[0] == null)
            {
                return (LanguageCultureItem)this.InnerList[0];
            }

            // First pass, exact match
            foreach (var t in
                cultures.SelectMany(
                    culture =>
                    this.InnerList.Cast<object>().Where(t => culture.Name == ((LanguageCultureItem)t).Culture.Name)))
            {
                // switched from UICulture to culture
                return (LanguageCultureItem)t;
            }

            // Second pass, we may accept a parent match
            return
                cultures.SelectMany(
                    culture =>
                    this.InnerList.Cast<LanguageCultureItem>().Where(
                        t =>
                        (culture.Name == t.Culture.Parent.Name) || (culture.Parent.Name == t.Culture.Name) ||
                        (culture.Parent.Name == t.Culture.Parent.Name))).FirstOrDefault();
        }

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The index of.
        /// </returns>
        public virtual int IndexOf(LanguageCultureItem item)
        {
            return this.InnerList.IndexOf(item);
        }

        /// <summary>
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <param name="item">
        /// The item.
        /// </param>
        public void Insert(int index, LanguageCultureItem item)
        {
            this.InnerList.Insert(index, item);
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        public void Remove(LanguageCultureItem item)
        {
            this.InnerList.Remove(item);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var s = new StringBuilder();
            foreach (LanguageCultureItem item in this.InnerList)
            {
                s.Append(item.UICulture.Name);
                s.Append(this.keyValueSeparator);
                s.Append(item.Culture.Name);
                s.Append(this.itemsSeparator);
            }

            return s.ToString();
        }

        /// <summary>
        /// Returns a CultureInfo list matching language property
        /// </summary>
        /// <param name="addInvariantCulture">
        /// If true adds a row containing invariant culture
        /// </param>
        /// <returns>
        /// </returns>
        public CultureInfo[] ToUICultureArray(bool addInvariantCulture)
        {
            var cultures = new ArrayList();
            if (addInvariantCulture)
            {
                cultures.Add(CultureInfo.InvariantCulture);
            }

            foreach (var t in this.InnerList.Cast<LanguageCultureItem>())
            {
                cultures.Add(t.UICulture);
            }

            return (CultureInfo[])cultures.ToArray(typeof(CultureInfo));
        }

        /// <summary>
        /// Returns a CultureInfo list matching language property
        /// </summary>
        /// <returns>
        /// </returns>
        public CultureInfo[] ToUICultureArray()
        {
            return this.ToUICultureArray(false);
        }

        #endregion

        #region Implemented Interfaces

        #region ICollection

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">
        /// The array.
        /// </param>
        /// <param name="arrayIndex">
        /// Index of the array.
        /// </param>
        void ICollection.CopyTo(Array array, int arrayIndex)
        {
            this.InnerList.CopyTo(array, arrayIndex);
        }

        #endregion

        #endregion
    }
}