using System;

namespace Appleseed.Content.Web.Modules
{
    /// <summary>
    /// Placeable Registration module
    /// </summary>
    public partial class Register : RegisterFull
    {
        /// <summary>
        /// Gets the GUID ID.
        /// </summary>
        /// <value>The GUID ID.</value>
        public override Guid GuidID
        {
            get { return new Guid("{09C7351B-C9A1-454e-953F-E17E6E6EF092}"); }
        }
    }
}