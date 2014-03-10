// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PortalUrlDataType.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   Portal URL Data Type
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.DataTypes
{
    using System.Web;
    using System.Web.UI.WebControls;

    using Appleseed.Framework.Settings;
    using Appleseed.Framework.Site.Configuration;

    /// <summary>
    /// Portal URL Data Type
    /// </summary>
    public class PortalUrlDataType : BaseDataType<string, TextBox>
    {
        #region Constants and Fields

        /// <summary>
        ///   The inner full path.
        /// </summary>
        private string innerFullPath;

        /// <summary>
        /// The the value.
        /// </summary>
        private string theValue;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PortalUrlDataType" /> class.
        /// </summary>
        public PortalUrlDataType()
        {
            this.PortalPathPrefix = string.Empty;

            if (HttpContext.Current.Items["PortalSettings"] == null)
            {
                return;
            }

            // Obtain PortalSettings from Current Context
            var PortalSettings = (PortalSettings)HttpContext.Current.Items["PortalSettings"];
            this.PortalPathPrefix = PortalSettings.PortalFullPath;
            if (!this.PortalPathPrefix.EndsWith("/"))
            {
                this.PortalPathPrefix += "/";
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PortalUrlDataType"/> class. 
        ///   Use this on portal setting or when you want turn off automatic discovery
        /// </summary>
        /// <param name="portalFullPath">
        /// The portal full path.
        /// </param>
        public PortalUrlDataType(string portalFullPath)
        {
            this.PortalPathPrefix = portalFullPath;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the description.
        /// </summary>
        public override string Description
        {
            get
            {
                return "URL relative to Portal";
            }
        }

        /// <summary>
        ///   Gets the full path.
        /// </summary>
        public override string FullPath
        {
            get
            {
                if (this.innerFullPath == null)
                {
                    this.innerFullPath = Path.WebPathCombine(this.PortalPathPrefix, this.Value);

                    // Removes trailing /
                    this.innerFullPath = this.innerFullPath.TrimEnd('/');
                }

                return this.innerFullPath;
            }
        }

        /// <summary>
        ///   Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public override string Value
        {
            get
            {
                return this.theValue;
            }

            set
            {
                // Remove portal path if present
                this.theValue = value.StartsWith(this.PortalPathPrefix)
                                    ? value.Substring(this.PortalPathPrefix.Length)
                                    : value;

                // Reset innerFullPath
                this.innerFullPath = null;
            }
        }

        /// <summary>
        ///   Gets the portal path prefix.
        /// </summary>
        /// <value>The portal path prefix.</value>
        protected string PortalPathPrefix { get; private set; }

        #endregion
    }
}