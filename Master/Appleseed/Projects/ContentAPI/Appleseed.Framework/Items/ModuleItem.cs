using System;
using System.Collections.Generic;
using System.Text;
using Content.API;

namespace Appleseed.Framework.Content.Items
{
    public class ModuleItem : AppleseedItem
    {
        private ContentItemCollection _contentItems;
        private long _moduleID = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ModuleItem"/> class.
        /// </summary>
        public ModuleItem()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ModuleItem"/> class.
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        public ModuleItem(long moduleID)
            : base(moduleID)
        {
            _moduleID = moduleID;
        }

        /// <summary>
        /// Gets the content items.
        /// </summary>
        /// <value>The content items.</value>
        public ContentItemCollection ContentItems
        {
            get { return _contentItems; }
        }

        /// <summary>
        /// Gets the module ID.
        /// </summary>
        /// <value>The module ID.</value>
        public long ModuleID
        {
            get { return _moduleID; }
        }
    }
}
