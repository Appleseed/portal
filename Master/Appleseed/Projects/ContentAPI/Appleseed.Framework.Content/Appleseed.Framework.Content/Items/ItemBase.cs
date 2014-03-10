/* 
-- =============================================
-- Author:		Jonathan F. Minond
-- Create date: April 2006
-- Description:	Partial class containing the basic parts of an Item Object.
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
        // This should all be prefetched i suppose
        #region provate Standard Item Data fields
        private long _itemID = 0;
        private DateTime _dateCreated;
        
        private bool _enablePersmissioning = false;
        private int _version = 0;
        private List<long> _versionItemIds; // not implemented yet
        private List<long> _currentVersionCultureItemIds; // not implemented yet
        private long _parentItemID = 0;
        private Guid _typeID;

        private Settings _itemSettings = new Settings();
        private Settings _originalSettings = new Settings();
        private WorkFlowVersions _workflowVersion = WorkFlowVersions.Draft;
        private bool _enableWorkflow = false;
        private string _createdByUser = string.Empty;
        private ItemCollection _childItems = null;
        private bool _enableComments = false;
        private bool _enableRatings = false;
        private long _groupItemID = 0;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Item"/> class.
        /// </summary>
        public Item()
        {
            assignEvents();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Item"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        public Item(long id)
        {
            if (id >= 0)
            {
                _itemID = id;
                FillInitialData();
            }
            assignEvents();
        }
        #endregion

        /// <summary>
        /// Fills the initial data.
        /// Prefech data for the item should be centralized to here as much as possible.
        /// Inherited objects can override thi method to fill additional properties
        /// they may need on aditional access.
        /// Some things like cihld nodes are lazy loaded.
        /// </summary>
        /// <remarks>
        /// Properties which are initially loaded at instance time of the itembase
        /// <list type="string">
        /// 		<item>_itemID</item>
        /// 		<item>_title</item>
        /// 		<item>_dateCreated</item>
        /// 		<item>_datePublished</item>
        /// 		<item>_enablePersmissioning</item>
        /// 		<item>_isDirty</item>
        /// 		<item>_parentItemID</item>
        /// 		<item></item>
        /// 	</list>
        /// </remarks>
        protected virtual void FillInitialData()
        {
            #region ItemHelperMethods
            LoadItemStatuses();
            LoadItemCategories();
            FetchItemSettings();
            #endregion
        }

        #region Public Properties
        /// <summary>
        /// Gets or sets a value indicating whether [enable permissioning].
        /// </summary>
        /// <value><c>true</c> if [enable permissioning]; otherwise, <c>false</c>.</value>
        public virtual bool EnablePermissioning
        {
            get
            {
                return _enablePersmissioning;
            }
            set
            {
                _enablePersmissioning = value;
            }
        }
        /// <summary>
        /// Gets or sets the date created.
        /// </summary>
        /// <value>The date created.</value>
        [Bindable(true)]
        public virtual DateTime DateCreated
        {
            get
            {
                return _dateCreated;
            }
            set
            {
                _dateCreated = value;
            }
        }
        /// <summary>
        /// Gets or sets the parent item ID.
        /// </summary>
        /// <value>The parent item ID.</value>
        [Bindable(true)]
        public long ParentItemID
        {
            get
            {
                return _parentItemID;
            }
            set
            {
                _parentItemID = value;
            }
        }
        /// <summary>
        /// Gets the item ID.
        /// </summary>
        /// <value>The item ID.</value>
        [Bindable(true)]
        public long ItemID
        {
            get
            {
                return _itemID;
            }
            set
            {
                if (value > 0)
                    _itemID = value;
                else
                    throw new Exception("Not allowed to set ItemID to 0 or negative.");


            }
        }
        /// <summary>
        /// Gets or sets the created by user.
        /// <remarks>this is the user who created the original version/culture of an item. there is
        /// only one created user for an item, even for multiple versions, the original creator stays 
        /// the same.</remarks>
        /// </summary>
        /// <value>The created by user.</value>
        public string CreatedByUser
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }
        /// <summary>
        /// Gets the item settings.
        /// Collection of settings for the item, stored as Setting types. Which provide
        /// name, value, datatype, and a bit more.
        /// </summary>
        /// <value>The item settings.</value>
        public Settings ItemSettings
        {
            get { return _itemSettings; }
            set
            {
                throw new NotImplementedException("The method or operation is not implemented.");
                //_itemID = value; 
            }
        }
        /// <summary>
        /// Gets or sets the group item ID.
        /// The group Item ID is the id of the item that groups a bunch of things. 
        /// it is different from a parent id. 
        /// for example pages are grouped by a portal id, but there parent is another pageid
        /// </summary>
        /// <value>The group item ID.</value>
        public long GroupItemID
        {
            get { return _groupItemID; }
            set { _groupItemID = value; }
        }

        /// <summary>
        /// Gets a value indicating whether [comments enabled].
        /// if enabled, features such as adding items of type comment will be enabled.
        /// there will be built in data mehods for easily making ui controls to deal with comments
        /// <remarks>some applications have the potential to use the comments as a full fledged
        /// type, for example in a discussion scenarion, you might use comments to make a single level
        /// reply. the comment type does not have have a lot of features, but they would have a parent
        /// for example, text, author, date.. and some other settings which UI could use.</remarks>
        /// </summary>
        /// <value><c>true</c> if [comments enabled]; otherwise, <c>false</c>.</value>
        public bool CommentsEnabled
        {
            get { return _enableComments; }
            set { _enableComments = value; }
        }
        /// <summary>
        /// Gets a value indicating whether [ratings enabled].
        /// if enabled, features such as adding items of type rating will be enabled.
        /// there will be built in data mehods for easily making ui controls to deal with ratings
        /// such as top rated by category, or most rated items...etc.. this is mainly helper functionality
        /// that a lot of conetnt is likely to use.
        /// <remarks>applications could potentially use ratings on differnt types</remarks>
        /// </summary>
        /// <value><c>true</c> if [ratings enabled]; otherwise, <c>false</c>.</value>
        public bool RatingsEnabled
        {
            get { return _enableRatings; }
            set { _enableRatings = value; }
        }
        /// <summary>
        /// Gets or sets the child items.
        /// This is a collection of items that would be lazy loaded if the items are asked for.
        /// parents who have this instance itemid as a parent will show up here.
        /// </summary>
        /// <value>The child items.</value>
        public ItemCollection ChildItems
        {
            get
            {
                return _childItems;
            }
            set
            {
                _childItems = value;
            }
        }

        #endregion
    }

    


}
