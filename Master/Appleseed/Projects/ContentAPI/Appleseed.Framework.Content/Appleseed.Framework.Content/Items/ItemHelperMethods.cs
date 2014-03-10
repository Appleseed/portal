/* 
-- =============================================
-- Author:		Jonathan F. Minond
-- Create date: April 2006
-- Description:	Partial class containing helper methods used in ItemObjects
-- This class works in conjucntion with ItemHelpers.cs and Item.cs
-- =============================================
*/

using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Xml;
using System.Reflection;
using Content.API.Data;
using Content.API.Properties;

namespace Content.API
{
    public partial class Item 
    {
        /// <summary>
        /// Encrypts the content.
        /// </summary>
        /// <returns></returns>
        public virtual bool EncryptContent()
        {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// Saves the state.
        /// </summary>
        /// <returns></returns>
        public virtual bool SaveState()
        {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// Resets this instance.
        /// </summary>
        /// <returns></returns>
        public virtual bool Reset()
        {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// Recycles the item.
        /// </summary>
        /// <returns></returns>
        public virtual bool RecycleItem()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Adds the new item.
        /// </summary>
        /// <returns></returns>
        public virtual long AddNewItem()
        {
            return 0;
        }

        /// <summary>
        /// Removes the item.
        /// </summary>
        /// <returns></returns>
        public virtual bool RemoveItem()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Updates the item.
        /// </summary>
        /// <returns></returns>
        public virtual bool UpdateItem()
        {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// Resets the settings.
        /// </summary>
        public virtual void ResetSettings()
        {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// Changes the version.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="targetVersion">The target version.</param>
        public virtual void SwitchVersion(int targetVersion)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Changes the culture.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="targetCulture">The target culture.</param>
        public virtual void SwitchCulture(Culture targetCulture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Changes the culture.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="targetCulture">The target culture.</param>
        public virtual void SwitchCultureVersion(int targetVersion, Culture targetCulture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fetches the item settings.
        /// </summary>
        protected virtual void FetchItemSettings()
        {
            throw new System.NotImplementedException();

            XmlSerializer serializer = new XmlSerializer(typeof(Settings));

            using (StreamReader reader = new StreamReader("sitesettings.config"))
            {
                _originalSettings = serializer.Deserialize(reader) as Settings;
            }
        }



        /// <summary>
        /// Loads the item Statuses.
        /// </summary>
        protected virtual void LoadItemStatuses()
        {
            ItemData id = DAL.Items;
            id.Connection = new DatabaseHelper().Connection;
            DataTable dtstatuses = id.GetItemStatuses(ItemID);
            if (dtstatuses != null && dtstatuses.Rows.Count > 0)
            {
                foreach (DataRow row in dtstatuses.Rows)
                {
                    Status st = new Status(Convert.ToInt32(row["SatusID"]), row["Title"].ToString(), row["Description"].ToString());
                    st.CategoryID = Convert.ToInt32(row["CategoryID"]);
                    this.Statuses.Add(st.ID, st);
                }
            }
        }

        /// <summary>
        /// Loads the item Categories.
        /// </summary>
        protected virtual void LoadItemCategories()
        {
            ItemData id = DAL.Items;
            id.Connection = new DatabaseHelper().Connection;
            DataTable dtcategories = id.GetItemCategories(ItemID);
            if (dtcategories != null && dtcategories.Rows.Count > 0)
            {
                foreach (DataRow row in dtcategories.Rows)
                {
                    this.Categories.Add(new Category(Convert.ToInt32(row["CategoryID"]), row["Title"].ToString(), row["Description"].ToString()));
                }
            }
        }
    }
}
