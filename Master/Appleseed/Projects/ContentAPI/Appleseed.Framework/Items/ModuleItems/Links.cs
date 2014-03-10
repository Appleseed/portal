using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

using Content.API;

using Appleseed.Framework.Content;
using Appleseed.Framework.Content.Items;

using Content.API.Data;
using Content.API.Types;

namespace Appleseed.Framework.Content.ItemTypes
{
    public class Links : ModuleItem
    {
        #region Fields
        private Guid _typeGUID = new Guid("{5E97E7D6-F9AE-45c2-A88D-5665E4A28103}");
        private WorkFlowVersions _workFlow = WorkFlowVersions.Post;
        private long _moduleID = 0;
        private ModuleItem _containerModule = null;
        //public ContentItemCollection _links;
        private int _category = 0;
        #endregion

        /// <summary>
        /// Fills the by category.
        /// </summary>
        protected void FillByCategory()
        {
            ItemData id = DAL.Items;
            id.Connection = new DatabaseHelper().Connection;
            DataTable dtLinks = id.GetItemsByCategoryTypeGroup(_category, this.ItemType, this.ModuleID);
            if (dtLinks != null && dtLinks.Rows.Count > 0)
            {
                foreach (DataRow row in dtLinks.Rows)
                {
                    ContentItem link = new ContentItem(Convert.ToInt64(row["ItemId"]));
                    base.ContentItems.Add(link);
                }
            }
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>The items.</value>
        public ContentItemCollection Items
        {
            get 
            {
                return base.ContentItems;
            } 
        }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        public int Category
        {
            get { return _category; }
            set
            {
                // TODO: Refil _links when catgory is changed;
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the module ID.
        /// </summary>
        /// <value>The module ID.</value>
        public long ModuleID
        {
            get { return _moduleID; }
        }

        /// <summary>
        /// Gets the module.
        /// </summary>
        /// <value>The module.</value>
        public ModuleItem Module
        {
            get
            {
                if (_containerModule == null && _moduleID > 0)
                    _containerModule = new ModuleItem(_moduleID);

                return _containerModule;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Links"/> class.
        /// </summary>
        /// <param name="ModuleID">The module ID.</param>
        public Links(long ModuleID)
        {
            _moduleID = ModuleID;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ModuleType"/> class.
        /// </summary>
        public Links()
            : base()
        {
            base.ItemType = _typeGUID;
        }

        public void AddLink(ContentItem link) 
        {
            link.GroupItemID = this.ModuleID;
            link.Categories.Add(new Category(this.Category));

            base.ContentItems.Add(link);
        }
        /// <summary>
        /// Adds the link.
        /// </summary>
        /// <param name="linkID">The link ID.</param>
        public void AddLink(long linkID) 
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Adds the link.
        /// </summary>
        /// <param name="href">The href.</param>
        /// <param name="text">The text.</param>
        public void AddLink(string href, string text) { AddLink(href, "", "", text); }
        /// <summary>
        /// Adds the link.
        /// </summary>
        /// <param name="href">The href.</param>
        /// <param name="tooltip">The tooltip.</param>
        /// <param name="text">The text.</param>
        public void AddLink(string href, string tooltip, string text) { AddLink(href, "", tooltip, text); }
        /// <summary>
        /// Adds the link.
        /// </summary>
        /// <param name="href">The href.</param>
        /// <param name="target">The target.</param>
        /// <param name="tooltip">The tooltip.</param>
        /// <param name="text">The text.</param>
        public void AddLink(string href, string target, string tooltip, string text)
        {
            ContentItem l = new ContentItem();
            l.Title = text;
            l.GroupItemID = this.ModuleID;
            l.Categories.Add(new Category(this.Category));
            l.ItemSettings.Update("Href", href);
            l.ItemSettings.Update("Target", target);
            l.ItemSettings.Update("Tooltip", tooltip);

            base.ContentItems.Add(l);
        }
        /// <summary>
        /// Removes the link.
        /// </summary>
        /// <param name="linkID">The link ID.</param>
        public void RemoveLink(long linkID) 
        {
            base.ContentItems.Remove(linkID);        
        }

        
    }
}
