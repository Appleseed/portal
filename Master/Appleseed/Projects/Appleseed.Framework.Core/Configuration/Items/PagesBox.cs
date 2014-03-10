// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PagesBox.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   Pages Box is a collection of PageStripDetails.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Site.Configuration
{
    using System;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Pages Box is a collection of PageStripDetails.
    /// </summary>
    /// <remarks>
    /// </remarks>
    [History("jminond", "2005/03/10", "Tab to page conversion")]
    [Obsolete("Use Collection<PageStripDetails> instead.")]
    public class PagesBox : Collection<PageStripDetails>
    {
    }
}