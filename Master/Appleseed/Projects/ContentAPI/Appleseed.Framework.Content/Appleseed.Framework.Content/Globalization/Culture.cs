using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Content.API
{
    [Serializable]
    public class Culture : CultureInfo
    {
        private string cultureCode = string.Empty;
        private string neutralCode = string.Empty;
        private string countryCode = string.Empty;
        private string description = string.Empty;
        private int identifier = 0;
        private string direction = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Culture"/> class.
        /// </summary>
        /// <param name="culturename">The culturename.</param>
        public Culture(string culturename) : base(culturename)
        {
        }

        /// <summary>
        /// Gets the culture code.
        /// </summary>
        /// <value>The culture code.</value>
        public string CultureCode
        {
            get { return cultureCode; }
        }

        /// <summary>
        /// Gets the neutral code.
        /// </summary>
        /// <value>The neutral code.</value>
        public string NeutralCode
        {
            get { return neutralCode; }
        }

        /// <summary>
        /// Gets the country code.
        /// </summary>
        /// <value>The country code.</value>
        public string CountryCode
        {
            get { return countryCode; }
        }

        /// <summary>
        /// Gets the direction.
        /// </summary>
        /// <value>The direction.</value>
        public string Direction
        {
            get { return direction; }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get { return description; }
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Identifier
        {
            get { return identifier; }
        }
    }
}
