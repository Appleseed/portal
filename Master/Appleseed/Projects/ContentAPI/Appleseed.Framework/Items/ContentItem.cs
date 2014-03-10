using System;
using System.Collections.Generic;
using System.Text;
using Content.API;
using Content.API.Types;
namespace Appleseed.Framework.Content.Items
{
    public class ContentItem : Item
    {
        private int _moduleID = 0;
        private int _pageID = 0;
        private ItemType _itemType;

        public ContentItem() : base() { }
        public ContentItem(long itemID) : base(itemID) { }
    }
}
