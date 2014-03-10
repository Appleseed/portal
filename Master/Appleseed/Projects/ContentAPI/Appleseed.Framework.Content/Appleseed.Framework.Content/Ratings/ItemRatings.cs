using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Content.API.Ratings
{
    public class ItemRatings
    {
        /// <summary>
        /// Rates the item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="rating">The rating.</param>
        public static void RateItem(Item item, Double rating)
        {
            RateItem(item.ItemID, rating);
        }


        /// <summary>
        /// Rates the item.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <param name="rating">The rating.</param>
        public static void RateItem(long itemID, Double rating)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Gets the item rating.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <returns></returns>
        public static Double GetItemRating(long itemID)
        {
            throw new System.NotImplementedException();
        }



        /// <summary>
        /// Gets the category ratings.
        /// </summary>
        /// <param name="CategoryID">The category ID.</param>
        /// <returns></returns>
        public static DataTable GetCategoryRatings(int CategoryID)
        {
            return GetCategoryRatings(CategoryID, false);
        }

        /// <summary>
        /// Gets the category ratings.
        /// </summary>
        /// <param name="CategoryID">The category ID.</param>
        /// <param name="includeChildCategories">if set to <c>true</c> [include child categories].</param>
        /// <returns></returns>
        public static DataTable GetCategoryRatings(int CategoryID, bool includeChildCategories)
        {
            throw new System.NotImplementedException();
        }


    }
}
