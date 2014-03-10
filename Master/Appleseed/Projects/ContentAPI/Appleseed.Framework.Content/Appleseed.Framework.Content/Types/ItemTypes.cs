/* 
-- =============================================
-- Author:		Jonathan F. Minond
-- Create date: April 2006
-- Description:	Item Type helpers
-- =============================================
*/

using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using Content.API.Data;

namespace Content.API.Types
{
    public enum ItemBaseType
    {
        Application = 0,
        User = 1
    }

    /// <summary>
    /// This is a single instance for the application, it will load once with a collection of all types
    /// avvaiable to the application.
    /// </summary>
    public class ItemTypes
    {
        private static TypeCollection _types = new TypeCollection();
        // Lock synchronization object 
        private static object syncLock = new object();
        private static ItemTypes instance;

        /// <summary>
        /// Gets the types.
        /// </summary>
        /// <value>The types.</value>
        public TypeCollection Types
        {
            get { return _types; }
        }

        /// <summary>
        /// Gets the load balancer.
        /// </summary>
        /// <returns></returns>
        public static ItemTypes GetItemTypes()
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
                        instance = new ItemTypes();
                }
            }

            return instance;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CategoryCache"/> class.
        /// </summary>
        protected ItemTypes()
        {
            FillTypeList();
        }

        /// <summary>
        /// Fills the type table.
        /// </summary>
        private void FillTypeList()
        {
            // TODO: _types should be stored in cache, and managed there
            // TODO: We will need some sort of Cache Manager, that can be extended
            // TO Support distributed caching

            ItemData id = DAL.Items;
            id.Connection = new DatabaseHelper().Connection;
            DataTable dtTypes = id.GetItemTypes();
            if (dtTypes != null && dtTypes.Rows.Count > 0)
            {
                foreach (DataRow row in dtTypes.Rows)
                {
                    // TODO: should be using type guid
                    _types.Add(new ItemType(Convert.ToInt32(row["ItemTypeId"]), row["Name"].ToString(), row["Description"].ToString(), row["typeSettings"].ToString(), Convert.ToInt32(row["BaseType"])));
                }
            }
        }
    }



    public class ApplicationTypes
    {


       

    }
}
