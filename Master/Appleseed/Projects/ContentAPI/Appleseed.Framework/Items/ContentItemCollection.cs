using System;
using System.Collections.Generic;
using System.Text;

namespace Appleseed.Framework.Content.Items
{
    public class ContentItemCollection : Dictionary<long, ContentItem>
    {
        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(ContentItem item)
        {
            base.Add(item.ItemID, item);
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void remove(ContentItem item)
        {
            base.Remove(item.ItemID);
        }
    }

    public class PageCollection : Dictionary<long, PageItem>
    {
        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(PageItem item)
        {
            base.Add(item.ItemID, item);
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void remove(PageItem item)
        {
            base.Remove(item.ItemID);
        }
    }

    public class PortalCollection : Dictionary<long, PortalItem>
    {
        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(PortalItem item)
        {
            base.Add(item.ItemID, item);
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void remove(PortalItem item)
        {
            base.Remove(item.ItemID);
        }
    }

    public class ModuleCollection : Dictionary<long, ModuleItem>
    {
        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(ModuleItem item)
        {
            base.Add(item.ItemID, item);
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void remove(ModuleItem item)
        {
            base.Remove(item.ItemID);
        }
    }
}
