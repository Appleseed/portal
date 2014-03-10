/* 
-- =============================================
-- Author:		Jonathan F. Minond
-- Create date: April 2006
-- Description:	Starter classes for the content data api
-- =============================================
*/

using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using Content.API.Data;
using System.Reflection;

namespace Content.API.Data
{
    public sealed class DAL : DataAccessLayerBase
    {
        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>The items.</value>
        public static ItemData Items { get { return (ItemData)WrapFactory.Create(typeof(ItemData)); } }
        /// <summary>
        /// Gets the item properties.
        /// </summary>
        /// <value>The item properties.</value>
        public static ItemProperties ItemProperties { get { return (ItemProperties)WrapFactory.Create(typeof(ItemProperties)); } }
    }


    public abstract class ItemProperties : SqlWrapperBase
    {
        public abstract DataSet GetItemBaseProperties(long ItemID);
    }

    public abstract class ItemData : SqlWrapperBase
    {
        
        /// <summary>
        /// Gets the statuses.
        /// </summary>
        /// <returns></returns>
        [SWCommand("select StatusId, Description from Status")]
        public abstract DataTable GetStatuses();

        /// <summary>
        /// Gets the item statuses.
        /// </summary>
        /// <param name="ItemID">The item ID.</param>
        /// <returns></returns>
        [SWCommand("select is.StatusID, s.Title, s.Description, sc.CategoryID from ItemStatuses is left join Status s on is.StatusId=s.StatusID left join StatusCategories sc on is.StatusId = sc.StatusID where is.itemId=@ItemID")]
        public abstract DataTable GetItemStatuses(long ItemID);

        /// <summary>
        /// Gets the item categories.
        /// </summary>
        /// <param name="ItemID">The item ID.</param>
        /// <returns></returns>
        [SWCommand("select ic.CategoryId, c.Title, c.Description from ItemCategories ic left join Categories c on ic.CategoryId=c.CategoryId where ic.ItemId=@ItemID")]
        public abstract DataTable GetItemCategories(long ItemID);

        /// <summary>
        /// Gets the type of the item by categoryand.
        /// </summary>
        /// <param name="CategoryID">The category ID.</param>
        /// <param name="TypeID">The type ID.</param>
        /// <returns></returns>
        [SWCommand("select i.ItemId, i.ParentItemId from Items i left join ItemCategories ic on i.ItemID = ic.ItemID and ic.Categoryid = CategoryId=@CategoryID Where i.ItemTypeID = @TypeID")]
        public abstract DataTable GetItemsByCategoryandType(int CategoryID, Guid TypeID);


        /// <summary>
        /// Gets the item by category type group.
        /// </summary>
        /// <param name="CategoryID">The category ID.</param>
        /// <param name="TypeID">The type ID.</param>
        /// <param name="GroupItemId">The group item id.</param>
        /// <returns></returns>
        [SWCommand("select i.ItemId, i.ParentItemId from Items i left join ItemCategories ic on i.ItemID = ic.ItemID and ic.Categoryid = CategoryId=@CategoryID Where i.ItemTypeID = @TypeID AND i.GroupItemId = @GroupItemId")]
        public abstract DataTable GetItemsByCategoryTypeGroup(int CategoryID, Guid TypeID, long GroupItemId);

        /// <summary>
        /// Gets the item types.
        /// </summary>
        /// <returns></returns>
        [SWCommand("select ItemTypeId as TypeID, Name, Description, BaseType, typeSettings from ItemTypes")]
        public abstract DataTable GetItemTypes();

        /// <summary>
        /// Counts the items by category.
        /// </summary>
        /// <param name="CategoryID">The category ID.</param>
        /// <returns></returns>
        [SWCommand("select count(ItemID) from ItemCategories where CategoryId=@CategoryID")]
        public abstract int CountItemsByCategory(int CategoryID);

        /// <summary>
        /// Counts the items by status.
        /// </summary>
        /// <param name="StatusID">The status ID.</param>
        /// <returns></returns>
        [SWCommand("select count(ItemID) from ItemCategories where StatusId=@StatusID")]
        public abstract int CountItemsByStatus(int StatusID);
    }
}
