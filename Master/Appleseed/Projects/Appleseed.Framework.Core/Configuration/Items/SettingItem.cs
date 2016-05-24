// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingItem.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   This class holds a single setting in the hash table,
//   providing information about data type, constraints.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework
{
    using System;
    using System.Web.UI;

    using Appleseed.Framework.DataTypes;

    /// <summary>
    /// This class holds a single setting in the hash table,
    ///   providing information about data type, constraints.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the setting item.
    /// </typeparam>
    /// <typeparam name="TEditControl">
    /// The edit control for the value.
    /// </typeparam>
    /// <author>
    ///   by Manu
    /// </author>
    public class SettingItem<T, TEditControl> : ISettingItem<T, TEditControl>
        where TEditControl : class
    {
        #region Constants and Fields

        /// <summary>
        ///   The data type.
        /// </summary>
        private readonly BaseDataType<T, TEditControl> datatype;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingItem{T,TEditControl}"/> class. 
        /// </summary>
        /// <param name="dataType">
        /// Type of the data.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public SettingItem(BaseDataType<T, TEditControl> dataType, T value)
        {
            this.EnglishName = string.Empty;
            this.Description = string.Empty;
            this.Group = SettingItemGroup.MODULE_SPECIAL_SETTINGS;
            this.datatype = dataType;
            this.datatype.Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingItem{T,TEditControl}"/> class. 
        /// </summary>
        /// <param name="dataType">
        /// Type of the data.
        /// </param>
        public SettingItem(BaseDataType<T, TEditControl> dataType)
        {
            this.EnglishName = string.Empty;
            this.Description = string.Empty;
            this.Group = SettingItemGroup.MODULE_SPECIAL_SETTINGS;
            this.datatype = dataType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingItem{T,TEditControl}"/> class. 
        /// </summary>
        /// <remarks>
        /// </remarks>
        public SettingItem()
        {
            this.EnglishName = string.Empty;
            this.Description = string.Empty;
            this.Group = SettingItemGroup.MODULE_SPECIAL_SETTINGS;
            this.datatype = new BaseDataType<T, TEditControl>();
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the data source.
        /// </summary>
        public object DataSource
        {
            get
            {
                return this.datatype.DataSource;
            }
        }

        /// <summary>
        ///   Gets or sets Provide help for parameter.
        ///   Should be a brief, descriptive text that explains what
        ///   this setting should do.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        ///   Gets or sets the edit control.
        /// </summary>
        /// <value>
        ///   The edit control.
        /// </value>
        public TEditControl EditControl
        {
            get
            {
                return this.datatype.EditControl;
            }

            set
            {
                this.datatype.EditControl = value;
            }
        }

        /// <summary>
        ///   Gets or sets the name of the parameter in plain English.
        /// </summary>
        /// <value>The name of the English.</value>
        public string EnglishName { get; set; }

        /// <summary>
        ///   Gets the full path.
        /// </summary>
        public string FullPath
        {
            get
            {
                return this.datatype.FullPath;
            }
        }

        /// <summary>
        ///   Gets or sets Allows grouping of settings in SettingsTable - use
        ///   Appleseed.Framework.Configuration.SettingItemGroup enum (convert to string)
        /// </summary>
        /// <value>The group.</value>
        /// <author>
        ///   Jes1111
        /// </author>
        public SettingItemGroup Group { get; set; }

        /// <summary>
        ///   Gets a description in plain English for
        ///   Group Key (read only)
        /// </summary>
        /// <value>The group description.</value>
        public string GroupDescription
        {
            get
            {
                switch (this.Group)
                {
                    case SettingItemGroup.NONE:
                        return "Generic settings";

                    case SettingItemGroup.THEME_LAYOUT_SETTINGS:
                        return "Theme and layout settings";

                    case SettingItemGroup.SECURITY_USER_SETTINGS:
                        return "Users and Security settings";

                    case SettingItemGroup.CULTURE_SETTINGS:
                        return "Culture settings";

                    case SettingItemGroup.BUTTON_DISPLAY_SETTINGS:
                        return "Buttons and Display settings";

                    case SettingItemGroup.MODULE_SPECIAL_SETTINGS:
                        return "Specific Module settings";

                    case SettingItemGroup.META_SETTINGS:
                        return "Meta settings";

                    case SettingItemGroup.MISC_SETTINGS:
                        return "Miscellaneous settings";

                    case SettingItemGroup.NAVIGATION_SETTINGS:
                        return "Navigation settings";

                    case SettingItemGroup.CUSTOM_USER_SETTINGS:
                        return "Custom User Settings";
                }

                return "Settings";
            }
        }

        /// <summary>
        ///   Gets or sets the max value.
        /// </summary>
        /// <value>
        ///   The max value.
        /// </value>
        public int MaxValue { get; set; }

        /// <summary>
        ///   Gets or sets the min value.
        /// </summary>
        /// <value>
        ///   The min value.
        /// </value>
        public int MinValue { get; set; }

        /// <summary>
        ///   Gets or sets the Display Order - use Appleseed.Framework.Configuration.SettingItemGroup enum
        ///   (add integer in range 1-999)
        /// </summary>
        /// <value>The order.</value>
        public int Order { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether this <see cref = "SettingItem&lt;T, TEditControl&gt;" /> is required.
        /// </summary>
        /// <value>
        ///   <c>true</c> if required; otherwise, <c>false</c>.
        /// </value>
        public bool Required { get; set; }

        /// <summary>
        ///   Gets or sets the value.
        /// </summary>
        /// <value>
        ///   The value.
        /// </value>
        public T Value
        {
            get
            {
                return this.datatype.Value;
            }

            set
            {
                this.datatype.Value = value;
            }
        }

        /// <summary>
        ///   Gets or sets the edit control.
        /// </summary>
        /// <value>The edit control.</value>
        /// <remarks>
        /// </remarks>
        Control ISettingItem.EditControl
        {
            get
            {
                return this.EditControl as Control;
            }

            set
            {
                this.EditControl = value as TEditControl;
            }
        }

        /// <summary>
        ///   Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        /// <remarks>
        /// </remarks>
        object ISettingItem.Value
        {
            get
            {
                return this.Value;
            }

            set
            {
                // WLF: If setting null...
                if (value == null)
                {
                    this.Value = default(T);
                }
                else
                {
                    // WLF: Special case for Uri type.
                    if (typeof(T) == typeof(Uri) && value.GetType() != typeof(Uri))
                    {
                        Uri url;
                        if (Uri.TryCreate(value.ToString(), UriKind.RelativeOrAbsolute, out url))
                        {
                            this.Value = (T)Convert.ChangeType(url, typeof(T));
                        }
                        else
                        {
                            throw new UriFormatException(string.Format("Not a valid Uri: {0}", value));
                        }
                    }
                    else
                    {
                        // WLF: Convert is required because some values come in as strings and need to be parsed.
                        try
                        {
                            this.Value = (T)Convert.ChangeType(value, typeof(T));
                        }
                        catch (Exception ex)
                        {
                            ErrorHandler.Publish(LogLevel.Warn, "Error Occured while Loding Setting Item value - "+ value + ".  Execption - " + ex.Message, ex);
                        }
                    }
                }
            }
        }

        #endregion

        #region Operators

        /// <summary>
        ///   ToString converter operator
        /// </summary>
        /// <param name = "value">The value.</param>
        /// <returns></returns>
        public static implicit operator string(SettingItem<T, TEditControl> value)
        {
            return value.ToString();
        }

        #endregion

        #region Implemented Interfaces

        #region IComparable

        /// <summary>
        /// Public comparer
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The compare to.
        /// </returns>
        public int CompareTo(object value)
        {
            if (value == null)
            {
                return 1;
            }

            // Modified by Hongwei Shen(hongwei.shen@gmail.com) 10/9/2005
            // the "value" should be casted to SettingItem instead of ModuleItem 
            // int compareOrder = ((ModuleItem) value).Order;
            var compareOrder = ((SettingItem<T, TEditControl>)value).Order;

            // end of modification            
            return this.Order != compareOrder
                       ? (this.Order < compareOrder ? -1 : (this.Order > compareOrder ? 1 : 0))
                       : 0;
        }

        #endregion

        #region IConvertible

        /// <summary>
        /// Returns the <see cref="T:System.TypeCode"/> for this instance.
        /// </summary>
        /// <returns>
        /// The enumerated constant that is the <see cref="T:System.TypeCode"/> of the class or value type that implements this interface.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public TypeCode GetTypeCode()
        {
            return Type.GetTypeCode(typeof(T));
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent Boolean value using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// A Boolean value equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        /// An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. 
        /// </param>
        /// <filterpriority>2</filterpriority>
        public bool ToBoolean(IFormatProvider provider)
        {
            return Convert.ToBoolean(this.Value);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 8-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// An 8-bit unsigned integer equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        /// An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. 
        /// </param>
        /// <filterpriority>2</filterpriority>
        public byte ToByte(IFormatProvider provider)
        {
            return Convert.ToByte(this.Value);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent Unicode character using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// A Unicode character equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        /// An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. 
        /// </param>
        /// <filterpriority>2</filterpriority>
        public char ToChar(IFormatProvider provider)
        {
            return Convert.ToChar(this.Value);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent <see cref="T:System.DateTime"/> using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.DateTime"/> instance equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        /// An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. 
        /// </param>
        /// <filterpriority>2</filterpriority>
        public DateTime ToDateTime(IFormatProvider provider)
        {
            return Convert.ToDateTime(this.Value);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent <see cref="T:System.Decimal"/> number using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Decimal"/> number equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        /// An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. 
        /// </param>
        /// <filterpriority>2</filterpriority>
        public decimal ToDecimal(IFormatProvider provider)
        {
            return Convert.ToDecimal(this.Value);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent double-precision floating-point number using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// A double-precision floating-point number equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        /// An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. 
        /// </param>
        /// <filterpriority>2</filterpriority>
        public double ToDouble(IFormatProvider provider)
        {
            return Convert.ToDouble(this.Value);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 16-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// An 16-bit signed integer equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        /// An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. 
        /// </param>
        /// <filterpriority>2</filterpriority>
        public short ToInt16(IFormatProvider provider)
        {
            return Convert.ToInt16(this.Value);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 32-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// An 32-bit signed integer equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        /// An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. 
        /// </param>
        /// <filterpriority>2</filterpriority>
        public int ToInt32(IFormatProvider provider)
        {
            return Convert.ToInt32(this.Value);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 64-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// An 64-bit signed integer equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        /// An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. 
        /// </param>
        /// <filterpriority>2</filterpriority>
        public long ToInt64(IFormatProvider provider)
        {
            return Convert.ToInt64(this.Value);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 8-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// An 8-bit signed integer equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        /// An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. 
        /// </param>
        /// <filterpriority>2</filterpriority>
        public sbyte ToSByte(IFormatProvider provider)
        {
            return Convert.ToSByte(this.Value);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent single-precision floating-point number using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// A single-precision floating-point number equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        /// An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. 
        /// </param>
        /// <filterpriority>2</filterpriority>
        public float ToSingle(IFormatProvider provider)
        {
            return Convert.ToSingle(this.Value);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent <see cref="T:System.String"/> using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> instance equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        /// An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. 
        /// </param>
        /// <filterpriority>2</filterpriority>
        public string ToString(IFormatProvider provider)
        {
            return Convert.ToString(this.Value);
        }

        /// <summary>
        /// Converts the value of this instance to an <see cref="T:System.Object"/> of the specified <see cref="T:System.Type"/> that has an equivalent value, using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Object"/> instance of type <paramref name="conversionType"/> whose value is equivalent to the value of this instance.
        /// </returns>
        /// <param name="conversionType">
        /// The <see cref="T:System.Type"/> to which the value of this instance is converted. 
        /// </param>
        /// <param name="provider">
        /// An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. 
        /// </param>
        /// <filterpriority>2</filterpriority>
        public object ToType(Type conversionType, IFormatProvider provider)
        {
            return Convert.ChangeType(this.Value, conversionType, provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 16-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// An 16-bit unsigned integer equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        /// An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. 
        /// </param>
        /// <filterpriority>2</filterpriority>
        public ushort ToUInt16(IFormatProvider provider)
        {
            return Convert.ToUInt16(this.Value);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 32-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// An 32-bit unsigned integer equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        /// An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. 
        /// </param>
        /// <filterpriority>2</filterpriority>
        public uint ToUInt32(IFormatProvider provider)
        {
            return Convert.ToUInt32(this.Value);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 64-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// An 64-bit unsigned integer equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        /// An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. 
        /// </param>
        /// <filterpriority>2</filterpriority>
        public ulong ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64(this.Value);
        }

        #endregion

        #region ISettingItem

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Convert.ToString(this.Value);
        }

        #endregion

        #endregion
    }
}