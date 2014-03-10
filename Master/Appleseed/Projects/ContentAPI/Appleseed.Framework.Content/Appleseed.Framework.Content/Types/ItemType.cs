/* 
-- =============================================
-- Author:		Jonathan F. Minond
-- Create date: April 2006
-- Description:	Item Type is the basic type that all type definitions will inherit from
-- =============================================
*/

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Content.API.Types
{
    [Serializable]
    public class ItemType
    {
        #region Fields
        private int _typeID = 0;
        private string _title = string.Empty;
        private string _description = string.Empty;
        private Guid _typeGUID = Guid.Empty;
        private bool _GUIDSET = false;
        private ItemBaseType _itemBaseType = ItemBaseType.User;
        /// <summary>
        /// The xml document is used for loading and storing, while the type is an object
        /// the settings will be handled by the list.
        /// upon item initialiazation the settings will be put into _typeSettings
        /// </summary>
        private XmlDocument _typeSettingXML = null;
        private Settings _typeSettings = new Settings();
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ItemType"/> class.
        /// </summary>
        public ItemType() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ItemType"/> class.
        /// </summary>
        /// <param name="typeGuid">The type GUID.</param>
        public ItemType(Guid typeGuid)
        {
            _typeGUID = typeGuid;
            LoadItemByGUID();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ItemType"/> class.
        /// </summary>
        /// <param name="typeID">The type ID.</param>
        public ItemType(int typeID)
        {
            // TODO: GetGUID
            //_typeGUID = typeGuid;
            LoadItemByGUID();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ContentItemType"/> class.
        /// </summary>
        /// <param name="typeGuid">The type GUID.</param>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        /// <param name="xmlSettings">The XML settings.</param>
        /// <param name="baseType">Type of the base.</param>
        public ItemType(Guid typeGuid, string title, string description, string xmlSettings, int baseType)
        {
            _typeGUID = typeGuid;
            LoadItemByGUID();

            _itemBaseType = (ItemBaseType)baseType;

            FetchTypeSettings(xmlSettings);
            // TODO: Convert settings to list, and clear xml document.

            //_typeID = typeID;
            
            _title = title;
            _description = description;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ContentItemType"/> class.
        /// </summary>
        /// <param name="typeGuid">The type GUID.</param>
        /// <param name="typeID">The type ID.</param>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        /// <param name="xmlSettings">The XML settings.</param>
        /// <param name="baseType">Type of the base.</param>
        public ItemType(Guid typeGuid, int typeID,string title, string description, string xmlSettings, int baseType)
        {
            LoadItemByGUID();

            _itemBaseType = (ItemBaseType)baseType;

            FetchTypeSettings(xmlSettings);
            // TODO: Convert settings to list, and clear xml document.

            _typeID = typeID;
            _typeGUID = typeGuid;
            _title = title;
            _description = description;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ContentItemType"/> class.
        /// </summary>
        /// <param name="typeID">The type ID.</param>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        /// <param name="xmlSettings">The XML settings.</param>
        /// <param name="baseType">Type of the base.</param>
        public ItemType(int typeID, string title, string description, string xmlSettings, int baseType)
        {
            // TODO: GetGUID
            //_typeGUID = typeGuid;
            LoadItemByGUID();

            _itemBaseType = (ItemBaseType)baseType;

            FetchTypeSettings(xmlSettings);
            // TODO: Convert settings to list, and clear xml document.

            _typeID = typeID;
            _title = title;
            _description = description;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the type ID.
        /// Note, this is an applciation local ID, and you may want to consider using TypeGUID to ensure 
        /// cross portal / application uniqueness.
        /// </summary>
        /// <value>The type ID.</value>
        [Bindable(true)]
        public int TypeID
        {
            get { return _typeID; }
        }

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>The title.</value>
        [Bindable(true)]
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        [Bindable(true)]
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        /// <summary>
        /// Gets or sets the type GUID.
        /// </summary>
        /// <value>The type GUID.</value>
        public Guid TypeGUID
        {
            get { return _typeGUID; }
            set {
                if (_GUIDSET == false)
                {
                    _typeGUID = value;
                    _GUIDSET = true;
                }
                else
                    throw new Exception("Item Type GUID cannot be changed once it has been set.");
            }
        }
        /// <summary>
        /// Gets the type settings.
        /// </summary>
        /// <value>The type settings.</value>
        [Bindable(true)]
        public Settings TypeSettings
        {
            get
            {
                return _typeSettings;
            }
        }


        /// <summary>
        /// Gets or sets the type of the base.
        /// </summary>
        /// <value>The type of the base.</value>
        [Bindable(true)]
        public ItemBaseType BaseType
        {
            get { return _itemBaseType; }
            set { _itemBaseType = value; }
        }
        #endregion

        /// <summary>
        /// Loads the item by GUID.
        /// </summary>
        protected virtual void LoadItemByGUID()
        {
            throw new System.NotImplementedException();
        }


        private List<string> _SettingKeys;
        /// <summary>
        /// Gets the item setting keys.
        /// </summary>
        /// <value>The item setting keys.</value>
        public List<string> SettingKeys
        {
            get { return _SettingKeys; }
        }


        /// <summary>
        /// Fetches the type settings.
        /// </summary>
        /// <param name="xmlSettings">The XML settings.</param>
        protected virtual void FetchTypeSettings(string xmlSettings)
        {
            throw new System.NotImplementedException();
            // TODO: This is an important method :-)
            //TH

            XmlSerializer serializer = new XmlSerializer(typeof(Settings));

            using (StringReader reader = new StringReader(xmlSettings))
            {
                _typeSettings = serializer.Deserialize(reader) as Settings;
            }
        }

       



        
    }
}
