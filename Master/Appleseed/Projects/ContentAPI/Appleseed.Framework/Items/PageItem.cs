using System;
using System.Collections.Generic;
using System.Text;
using Content.API;

namespace Appleseed.Framework.Content.Items
{
    public class PageItem : AppleseedItem
    {
        private ModuleCollection _moduleCollection;
        private long _pageID = 0;
        private long _portalID = 0;
        private PortalItem _portal = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:PageItem"/> class.
        /// </summary>
        public PageItem()
            : base()
        {

        }

        /// <summary>
        /// Gets the modules.
        /// </summary>
        /// <value>The modules.</value>
        public ModuleCollection Modules
        {
            get { return _moduleCollection; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:PageItem"/> class.
        /// </summary>
        /// <param name="pageID">The page ID.</param>
        public PageItem(long pageID)
            : base(pageID)
        {
            _pageID = pageID;
        }

        /// <summary>
        /// Gets the parent page ID.
        /// </summary>
        /// <value>The parent page ID.</value>
        public long ParentPageID
        {
            get
            {
                return base.ParentItemID;
            }
        }

        /// <summary>
        /// Gets the portal ID.
        /// </summary>
        /// <value>The portal ID.</value>
        public long PortalID
        {
            get
            {
                return _portalID;
            }
            set
            {
                _portalID = value;
                _portal = null;
            }
        }

        /// <summary>
        /// Gets the portal.
        /// </summary>
        /// <value>The portal.</value>
        public PortalItem Portal
        {
            get { return _portal; }
        }
    }
}
