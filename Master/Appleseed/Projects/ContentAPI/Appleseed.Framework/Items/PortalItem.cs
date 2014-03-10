using System;
using System.Collections.Generic;
using System.Text;
using Content.API;

namespace Appleseed.Framework.Content.Items
{
    public class PortalItem : AppleseedItem
    {
        private PageCollection _pageCollection;
        private long _portalID = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:PortalItem"/> class.
        /// </summary>
        public PortalItem()
            : base()
        {

        }

        /// <summary>
        /// Gets the pages.
        /// </summary>
        /// <value>The pages.</value>
        public PageCollection Pages
        {
            get
            {
                return _pageCollection;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:PortalItem"/> class.
        /// </summary>
        /// <param name="portalID">The portal ID.</param>
        public PortalItem(long portalID)
            : base(portalID)
        {
            _portalID = portalID;
        }

       
    }
}
