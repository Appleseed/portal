// Esperantus - The Web translator
// Copyright (C) 2003 Emmanuele De Andreis
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//
// Emmanuele De Andreis (manu-dea@hotmail dot it)

using System;
using System.ComponentModel;
using System.Globalization;

namespace Appleseed.Framework.Web.UI.WebControls
{
    /// <summary>
    /// LanguageCultureList converter.
    /// </summary>
    public class TypeConverterLanguageCultureCollection : TypeConverter
    {
        /// <summary>
        /// Overrides the CanConvertFrom method of TypeConverter.
        /// The ITypeDescriptorContext interface provides the context for the
        /// conversion. Typically this interface is used at design time to 
        /// provide information about the design-time container.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof (string))
            {
                return true;
            }
            if (sourceType == typeof (LanguageCultureCollection))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Overrides the CanConverTo method of TypeConverter.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof (string))
            {
                return true;
            }
            if (sourceType == typeof (LanguageCultureCollection))
            {
                return true;
            }
            return base.CanConvertTo(context, sourceType);
        }

        private readonly char[] itemsSeparator = {';'};
        private readonly char[] keyValueSeparator = {'='};

        private LanguageCultureCollection GetLanguagesCultureList(string keyValueList)
        {
            //Trim last separator if found
            keyValueList = keyValueList.TrimEnd(itemsSeparator);

            string[] nameValues = keyValueList.Split(itemsSeparator);
            LanguageCultureCollection nameValueColl = new LanguageCultureCollection();

            foreach (string nameValue in nameValues)
            {
                if (nameValue != string.Empty)
                {
                    string[] arrNameValue = nameValue.Split(keyValueSeparator);

                    CultureInfo language;
                    CultureInfo culture;

                    language = new CultureInfo(arrNameValue[0]);

                    if (arrNameValue.Length == 2)
                    {
                        culture = new CultureInfo(arrNameValue[1]);
                        nameValueColl.Add(new LanguageCultureItem(language, culture));
                    }
                    if (arrNameValue.Length == 1)
                    {
                        culture = new CultureInfo(arrNameValue[0]);
                        if (culture.IsNeutralCulture) //we cannot allow this
                            culture = CultureInfo.CreateSpecificCulture(culture.Name); //creates a specific culture
                        nameValueColl.Add(new LanguageCultureItem(language, culture));
                    }
                }
            }
            return nameValueColl;
        }

        /// <summary>
        /// Overrides the ConvertFrom method of TypeConverter.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                return GetLanguagesCultureList((string) value);
            }
            if (value is LanguageCultureCollection)
            {
                return value.ToString();
            }
            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// Overrides the ConvertTo method of TypeConverter.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                         Type destinationType)
        {
            if (destinationType == typeof (string))
            {
                return value.ToString();
            }
            if (destinationType == typeof (LanguageCultureCollection))
            {
                return GetLanguagesCultureList((string) value);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}