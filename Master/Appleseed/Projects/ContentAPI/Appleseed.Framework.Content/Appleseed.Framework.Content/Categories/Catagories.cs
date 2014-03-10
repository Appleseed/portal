using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using Content.API.Data;
namespace Content.API
{
    /// <summary>
    /// There should only be one instance of this class in the applciation.
    /// Its size in controled by app settings, so different sites, with different
    /// resources can control the level of there cach resources
    /// There is only one instance of the caregory cache
    /// </summary>
    public class CategoryCache
    {
        private static Dictionary<int, Category> _categoryCache = new Dictionary<int, Category>();
        private static int _maxCacheItems = 100; // TODO Should be application setting
        private static int[] keys;
        // Lock synchronization object 
        private static object syncLock = new object();
        private static CategoryCache instance;

        /// <summary>
        /// Gets the load balancer.
        /// </summary>
        /// <returns></returns>
        public static CategoryCache GetCategoryCache()
        {
            // Support multithreaded applications through 
            // 'Double checked locking' pattern which (once 
            // the instance exists) avoids locking each 
            // time the method is invoked 
            if (instance == null)
            {
                lock (syncLock)
                {
                    if (instance == null)
                        instance = new CategoryCache();
                }
            }

            return instance;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CategoryCache"/> class.
        /// </summary>
        protected CategoryCache()
        {
            // TODO: The same class concept needs to be created for StatusCache as well
            _categoryCache = new Dictionary<int, Category>(_maxCacheItems);
        }

        /// <summary>
        /// Adds the category.
        /// </summary>
        /// <param name="categoryID">The category ID.</param>
        public static void AddCategory(int categoryID)
        {
            if (!_categoryCache.ContainsKey(categoryID))
            {
                if (_categoryCache.Count == _maxCacheItems)
                    ClearLastItem();

                _categoryCache.Add(categoryID, new Category(categoryID));
            }
        }

        /// <summary>
        /// Clears the last item.
        /// </summary>
        private static void ClearLastItem()
        {
            _categoryCache.Remove(keys[(_maxCacheItems - 1)]);
        }

        /// <summary>
        /// Adds the category.
        /// </summary>
        /// <param name="category">The category.</param>
        public static void AddCategory(Category category)
        {
            if (!_categoryCache.ContainsKey(category.ID))
            {
                if (_categoryCache.Count == _maxCacheItems)
                    ClearLastItem();

                _categoryCache.Add(category.ID, category);
            }
        }
    }

    public class Catagories : Dictionary<int, Category>
    {
        /// <summary>
        /// Adds the specified category.
        /// </summary>
        /// <param name="category">The category.</param>
        public void Add(Category category)
        {
            this.Add(category.ID, category);
        }

        /// <summary>
        /// Application Categories Not Yet Implemented
        /// </summary>
        /// <param name="applicationID">The application ID.</param>
        /// <returns></returns>
        public static Catagories ApplicationCategories(int applicationID)
        {
            throw new System.NotImplementedException("Get Application Categories Not Yet Implemented");
        }

        /// <summary>
        /// Items categories.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <returns></returns>
        public static Catagories ItemCategories(long itemID)
        {

            Catagories theList = new Catagories();

            ItemData id = DAL.Items;
            id.Connection = new DatabaseHelper().Connection;
            DataTable dtcategories = id.GetItemCategories(itemID);
            if (dtcategories != null && dtcategories.Rows.Count > 0)
            {
                foreach (DataRow row in dtcategories.Rows)
                {
                    theList.Add(new Category(Convert.ToInt32(row["CategoryID"]), row["Title"].ToString(), row["Description"].ToString()));
                }
            }

            return theList;
        }
    }
}
