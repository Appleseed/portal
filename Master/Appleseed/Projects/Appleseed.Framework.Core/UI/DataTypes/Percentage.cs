using System;

namespace Appleseed.Framework.DataTypes
{
    /// <summary>
    /// Structure that contains percentage values (0 to 100)
    /// </summary>
    public struct Percentage
    {
        private byte _Percent;

        /// <summary>
        /// Percentage constructor
        /// </summary>
        /// <param name="percent">Byte value from 0 to 100</param>
        public Percentage(byte percent)
        {
            //Initialize value of struct
            _Percent = 0;
            //Sets the required value
            //If it is invalid the struct contains 0
            Value = percent;
        }

        /// <summary>
        /// Explicit conversion from int (it may fail)
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static explicit operator Percentage(int value)
        {
            Percentage retval;
            retval = new Percentage(Convert.ToByte(value));
            return (retval);
        }

        /// <summary>
        /// Explicit conversion from byte (it may fail)
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static explicit operator Percentage(byte value)
        {
            Percentage retval;
            retval = new Percentage(Convert.ToByte(value));
            return (retval);
        }

        /// <summary>
        /// Implicit conversion to int
        /// </summary>
        /// <param name="perc">The perc.</param>
        /// <returns></returns>
        public static implicit operator int(Percentage perc)
        {
            return (Convert.ToInt32(perc.Value));
        }

        /// <summary>
        /// Implicit conversion to byte
        /// </summary>
        /// <param name="perc">The perc.</param>
        /// <returns></returns>
        public static implicit operator byte(Percentage perc)
        {
            return (perc.Value);
        }

        /// <summary>
        /// Implicit conversion to string
        /// </summary>
        /// <param name="perc">The perc.</param>
        /// <returns></returns>
        public static implicit operator string(Percentage perc)
        {
            return (perc.Value.ToString() + "%");
        }

        /// <summary>
        /// The value of the Percentage
        /// </summary>
        /// <value>The value.</value>
        public byte Value
        {
            get { return _Percent; }
            set
            {
                if (value >= 0 & value <= 100)
                {
                    _Percent = value;
                }
                else
                {
                    throw (new ArgumentOutOfRangeException("Percentage", value, "Percentage must be from 0 to 100"));
                }
            }
        }

        /// <summary>
        /// Overridden method ToString
        /// </summary>
        /// <returns>The value of percentage in string format</returns>
        public override string ToString()
        {
            return _Percent.ToString() + "%";
        }
    }
}