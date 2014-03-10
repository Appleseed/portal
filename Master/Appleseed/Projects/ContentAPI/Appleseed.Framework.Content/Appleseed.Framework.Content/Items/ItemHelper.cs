/* 
-- =============================================
-- Author:		Jonathan F. Minond
-- Create date: April 2006
-- Description:	Sealed class of helper methods and utility methods for items
-- =============================================
*/

using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using Content.API.Data;

namespace Content.API
{
    public sealed class ItemHelper
    {
        /// <summary>
        /// Items by category.
        /// Returns Items of all types matching category.
        /// <remarks>This can be a slow running method. 
        /// It is likely you will want to use one of the overloads
        /// to trim the result set down a bit
        /// </remarks>
        /// </summary>
        /// <param name="categoryID">The category ID.</param>
        /// <returns></returns>
        public static ItemCollection ItemsByCategory(int categoryID)
        {
            // TODO: Need overloads to limit range
            // Date range, top count, bytype, bystatus, byparenttype....
            ItemCollection items = null;
            // i.ItemId, i.ParentItemId
            ItemData id = DAL.Items;
            id.Connection = new DatabaseHelper().Connection;
            DataTable dt = id.GetItemsByCategoryandType(categoryID, Guid.Empty);
            if (dt != null && dt.Rows.Count > 0)
            {
                items = new ItemCollection();
                foreach (DataRow row in dt.Rows)
                {
                    long itemID = Convert.ToInt64(row["ItemID"]);
                    items.Add(new Item(itemID));
                }
            }
            return items;
        }

        /// <summary>
        /// Items by status.
        /// </summary>
        /// <param name="statusID">The status ID.</param>
        /// <returns></returns>
        public static ItemCollection ItemsByStatus(int statusID)
        {
            throw new System.NotImplementedException("Need to implement get by status method in DAL Layer.");
            // TODO: Need to implement get by status method in DAL Layer.
            // Date range, top count, bytype, bystatus, byparenttype....
            ItemCollection items = null;
            // i.ItemId, i.ParentItemId
            ItemData id = DAL.Items;
            id.Connection = new DatabaseHelper().Connection;
            DataTable dt = id.GetItemsByCategoryandType(statusID, Guid.Empty);
            if (dt != null && dt.Rows.Count > 0)
            {
                items = new ItemCollection();
                foreach (DataRow row in dt.Rows)
                {
                    long itemID = Convert.ToInt64(row["ItemID"]);
                    items.Add(new Item(itemID));
                }
            }
            return items;
        }


        /// <summary>
        /// Counts the items by category.
        /// </summary>
        /// <param name="categoryID">The category ID.</param>
        /// <returns></returns>
        public static int CountItemsByCategory(int categoryID)
        {
            ItemData id = DAL.Items;
            id.Connection = new DatabaseHelper().Connection;
            return id.CountItemsByCategory(categoryID);
        }


        /// <summary>
        /// Counts the items by status.
        /// </summary>
        /// <param name="statusID">The status ID.</param>
        /// <returns></returns>
        public static int CountItemsByStatus(int statusID)
        {
            ItemData id = DAL.Items;
            id.Connection = new DatabaseHelper().Connection;
            return id.CountItemsByStatus(statusID);
        }

        #region Subscriptions
        /// <summary>
        /// Subscribes to item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void SubscribeToItem(Item item)
        {

        }

        /// <summary>
        /// Subscribes to item.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        public void SubscribeToItem(long itemID)
        {

        }
        #endregion


    }
}
