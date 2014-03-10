using System;
using System.Collections.Generic;
using System.Text;

namespace Content.API
{
    public class Cultures
    {
        private Dictionary<string, Culture> allCultures = null;
        private Dictionary<string, Culture> applicationCultures = null;
        private List<string> allowedCultures = null;

        /// <summary>
        /// Gets the allowed cultures.
        /// </summary>
        /// <value>The allowed cultures.</value>
        public List<string> AllowedCultures
        {
            get
            {
                return allowedCultures;
            }
        }

        /// <summary>
        /// Gets the application cultures.
        /// </summary>
        /// <value>The application cultures.</value>
        public Dictionary<string, Culture> ApplicationCultures
        {
            get
            {
                return applicationCultures;
            }
        }

        /// <summary>
        /// Gets all avaialable cultures.
        /// </summary>
        /// <value>All avaialable cultures.</value>
        public Dictionary<string, Culture> AllAvaialableCultures
        {
            get
            {
                return allCultures;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Cultures"/> class.
        /// </summary>
        public Cultures()
        {
            foreach (KeyValuePair<string, Culture> culture in allCultures)
            {
                if (allowedCultures.Contains(culture.Key))
                    applicationCultures.Add(culture.Key, culture.Value);
            }
        }
    }
}
