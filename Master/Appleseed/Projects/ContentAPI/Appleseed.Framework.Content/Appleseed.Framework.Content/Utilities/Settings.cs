using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace Content.API
{
    public struct Setting
    {
        private string _key;
        private object _value;
        private string _dataType;
        private System.Xml.XmlDocument _xmlSettingValue;

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>
        /// true if obj and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {


            if (obj == null) return false;
            if (Object.ReferenceEquals(this, obj)) return true;
            if (this.GetType() != obj.GetType()) return false;

            Setting y = (Setting)obj;
            if ((this.Key.ToLower() == y.Key.ToLower()) && (this.Value == y.Value))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> containing a fully qualified type name.
        /// </returns>
        public override string ToString()
        {
            return "Key = " + this.Key + " : value = " + this.Value;
        }

        /// <summary>
        /// Operator == check if setting x is equal to y.
        /// <remarks>checks the key and value to be equal</remarks>
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public static bool operator ==(Setting x, Setting y)
        {
            return x.Equals(y);
        }
        /// <summary>
        /// Operator !=s the specified x.
        /// <remarks>checks the key and value to be equal</remarks>
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public static bool operator !=(Setting x, Setting y)
        {
            if(!x.Equals(y))
                return true;
            else
                return false;
        }
 

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }
        /// <summary>
        /// Gets or sets the type of the data.
        /// </summary>
        /// <value>The type of the data.</value>
        public string DataType
        {
            get { return _dataType; }
            set { _dataType = value; }
        }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }
        /// <summary>
        /// Gets or sets the XML setting value.
        /// </summary>
        /// <value>The XML setting value.</value>
        public System.Xml.XmlDocument XmlSettingValue
        {
            get { return _xmlSettingValue; }
            set { XmlSettingValue = value; }
        }
    }

    [Serializable]
    public class Settings : Dictionary<string, Setting>
    {
        private bool _settingsDirty = false;
        private Dictionary<string, Setting> _originalValues = null;
        private Dictionary<string, bool> _isChanged = null;

        /// <summary>
        /// Gets a value indicating whether this instance is dirty.
        /// </summary>
        /// <value><c>true</c> if this instance is dirty; otherwise, <c>false</c>.</value>
        public bool IsDirty
        {
            get { return _settingsDirty; }
            set { _settingsDirty = value; }
        }


        /// <summary>
        /// Resets this instance.
        /// </summary>
        public virtual void Reset()
        {
            throw new System.NotImplementedException();
        }


        /// <summary>
        /// Sets the specified setting.
        /// <remarks>recomended to use Add(string key, Setting setting)</remarks>
        /// </summary>
        /// <param name="setting">The setting.</param>
        public void Set(Setting setting)
        {
            this.Add(setting.Key, setting);
        }

        /// <summary>
        /// Adds the specified setting.
        /// <remarks>recomended to use Add(string key, Setting setting)</remarks>
        /// </summary>
        /// <param name="setting">The setting.</param>
        public void Add(Setting setting)
        {
            this.Add(setting.Key, setting);
        }

        public void Add(string key, Setting setting) 
        {
            Add(key, setting, true);
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="setting">The setting.</param>
        public void Add(string key, Setting setting, bool isDirty)
        {
            string k = setting.Key;
            if (base.ContainsKey(k))
            {
                base.Remove(k);
                if (_originalValues.ContainsKey(k))
                    _originalValues.Remove(k);

                _originalValues.Add(k, base[k]);

                base.Add(k, setting);

                _isChanged.Add(k, isDirty);
            }
            else
            {
                base.Add(k, setting);
                _isChanged.Add(k, isDirty);
            }
 
        }


        /// <summary>
        /// Removes the specified setting.
        /// </summary>
        /// <param name="setting">The setting.</param>
        public void Remove(Setting setting)
        {
            if (this.ContainsKey(setting.Key))
                Remove(setting.Key);
        }





        /// <summary>
        /// Updates the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="setting">The setting.</param>
        public void Update(string key, Setting setting)
        {
            if (base.ContainsKey(key) && (setting != base[key]))
            {
                Add(key, setting, true);
            }
            else if (base.ContainsKey(key))
            {
                Add(key, setting, true);
            }
            else
            {
                throw new Exception("key does not exist, perhaps you want to Add a new Setting?");
            }
        }

        /// <summary>
        /// Updates the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Update(string key, object value)
        {
            if (base.ContainsKey(key))
                Remove(key);

            Setting setting = new Setting();
            setting.Key = key;
            setting.Value = value;
            Add(key, setting, true);
        }


        /// <summary>
        /// Saves this settings to the data store.
        /// only settings which are labeled dirty are persisted.
        /// </summary>
        /// <returns></returns>
        public virtual void Save(long ItemID)
        {
            DataTable settingsToSave = new DataTable();
            
            DataColumn columnItemID = new DataColumn("ItemID", typeof(int));
            settingsToSave.Columns.Add(columnItemID);

            DataColumn columnName = new DataColumn("Name", typeof(string));
            settingsToSave.Columns.Add(columnName);

            DataColumn columnValue = new DataColumn("Value", typeof(string));
            settingsToSave.Columns.Add(columnValue);
            
            DataColumn columnXmlSettings = new DataColumn("XmlSettings", typeof(System.Xml.XmlDocument));
            settingsToSave.Columns.Add(columnXmlSettings);

            DataColumn columnDataType = new DataColumn("DataType", typeof(string));
            settingsToSave.Columns.Add(columnDataType);

            DataRow dr;

            foreach (KeyValuePair<string, bool> kv in _isChanged)
            {
                if (kv.Value == true)
                {
                    dr = settingsToSave.NewRow();
                    dr["ItemID"] = ItemID;
                    dr["Name"] = base[kv.Key].Key;
                    dr["Value"] = base[kv.Key].Value;
                    dr["XmlSettings"] = base[kv.Key].XmlSettingValue;
                    dr["DataType"] = base[kv.Key].DataType;
                    settingsToSave.Rows.Add(kv.Key, base[kv.Key]);
                }
            }

            // TODO: Bulk update / insert settings.
        }

        /// <summary>
        /// Saves the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public virtual void Save(long ItemID, string key, string value)
        {
            throw new System.NotImplementedException();
        }
    }
}
