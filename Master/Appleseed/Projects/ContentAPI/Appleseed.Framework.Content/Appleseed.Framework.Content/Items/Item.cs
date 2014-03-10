/* 
-- =============================================
-- Author:		Jonathan F. Minond
-- Create date: April 2006
-- Description:	Partial class containing the basic parts of an Item Object.
-- This class works in conjucntion with ItemHelpers.cs and ItemBase.cs
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
using Content.API.ItemEvents;

namespace Content.API
{
    [Serializable]
    public partial class Item : IFormattable
    {


        // These should be lazy loaded probably
        #region Private version dependant fields are dependant on current version / culture
        private object _content = null;
        private bool _contentLoaded = false;
        // TODO: Public property, and db field.
        private bool _enableApplicationSearch = true;

        private Culture _culture;
        private bool _isDirty = false;
        private string _description = null;
        private string _title = "";

        private bool _descriptionLoaded = false;
        
        private string _lastupdatedUser = string.Empty;
        private DateTime _lastUpdated;
        private List<string> _keywords = null;
        private string _fullText = string.Empty;
        private Catagories _categories = null;
        private Statuses _statuses = null;
        private DateTime _datePublished;
        
        #endregion

        #region Public Properties
        
        /// <summary>
        /// Gets the full text.
        /// </summary>
        /// <value>The full text.</value>
        public string FullText
        {
            get { return _fullText; }
            set
            {
                _fullText = value; 
            }
        }
       /// <summary>
        /// Gets the keywords.
        /// a list of words that are stored for searches. these are indexed keywords which can be used
        /// by the application to provide targeted searching. this is an extension of the description/abstract proeprty.
        /// in the case of a page type for example, the keywords could be used to generate metatag keywords, 
        /// and a description would be the description
        /// </summary>
        /// <value>The keywords.</value>
        public List<string> Keywords
        {
            get
            {
                return _keywords;
            }
            set
            {
                throw new NotImplementedException("The method or operation is not implemented.");
                //_itemID = value; 
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable workflow].
        /// </summary>
        /// <value><c>true</c> if [enable workflow]; otherwise, <c>false</c>.</value>
        public bool EnableWorkflow
        {
            get
            {
                return _enableWorkflow;
            }
            set
            {
                _enableWorkflow = value;
            }
        }
        /// <summary>
        /// Gets or sets the work flow version.
        /// </summary>
        /// <value>The work flow version.</value>
        public WorkFlowVersions WorkFlowVersion
        {
            get
            {
                return _workflowVersion;
            }
            set
            {
                _workflowVersion = value;
            }
        }

        /// <summary>
        /// Gets or sets the last updated by user.
        /// <remarks>this is the last updated user for the current version of an item.</remarks>
        /// </summary>
        /// <value>The last updated by user.</value>
        public string LastUpdatedByUser
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
        /// Gets or sets the last updated date.
        /// <remarks>this is the last updated user for the current version of an item.</remarks>
        /// </summary>
        /// <value>The last updated date.</value>
        public DateTime LastUpdatedDate
        {
            get
            {
                return _lastUpdated;
            }
            set
            {
                if (DateCreated < value)
                    _lastUpdated = value;
            }
        }
        /// <summary>
        /// Gets or sets the statuses.
        /// A collection of statuses for the item. perhaps based on multiple status groups.
        /// you will likely be looking for a specifc set of statuses or single satus. use the status collection
        /// to filter the status by category
        /// <remarks>statuses per version</remarks>
        /// </summary>
        /// <value>The statuses.</value>
        public Statuses Statuses
        {
            get
            {
                // TODO: Statuses collection (methods for category filters)
                return _statuses;
            }
            set
            {
                _statuses = value;
            }
        }
        /// <summary>
        /// Gets or sets the categories.
        /// <remarks>categories per version</remarks>
        /// </summary>
        /// <value>The categories.</value>
        public Catagories Categories
        {
            get
            {
                return _categories;
            }
            set
            {
                _categories = value;
            }
        }
        /// <summary>
        /// Gets or sets the type of the item.
        /// </summary>
        /// <value>The type of the item.</value>
        public Guid ItemType
        {
            get
            {
                return _typeID;
            }
            set
            {
                _typeID = value;
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is stored in DB.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is stored in DB; otherwise, <c>false</c>.
        /// </value>
        public bool IsStoredInDB
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
        /// Gets a value indicating whether this instance is encrypted.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is encrypted; otherwise, <c>false</c>.
        /// </value>
        public bool IsEncrypted
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new NotImplementedException("The method or operation is not implemented.");
                //_itemID = value; 
            }
        }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [Bindable(true)]
        public string Description
        {
            get
            {
                if (!_descriptionLoaded)
                {
                    // ToDO: Load Description
                    _description = "Description fetch method not implemented yet";
                    _descriptionLoaded = true;
                }

                return _description;
            }
            set
            {
                _description = value;
            }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        [Bindable(true)]
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        /// <value>The culture.</value>
        [Bindable(true)]
        public Culture Culture
        {
            get { return _culture; }
            set { _culture = value; }
        }
        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        [Bindable(true)]
        public virtual object Content
        {
            get
            {
                if (!_contentLoaded)
                {
                    // ToDO: Load Description
                    _content = "Content fetch method not implemented yet";
                    _content = true;
                }
                return _content;
            }
            set
            {
                _content = value;
            }
        }
        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        [Bindable(true)]
        public virtual int Version
        {
            get
            {
                return _version;
            }
            set
            {
                if (value >= 0)
                    _version = value;
                else
                    throw new Exception("Version may not be less than 0");
            }
        }

        /// <summary>
        /// Gets or sets the date published.
        /// </summary>
        /// <value>The date published.</value>
        [System.ComponentModel.Bindable(true)]
        public virtual DateTime DatePublished
        {
            get
            {
                return _datePublished;
            }
            set
            {
                _datePublished = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is dirty.
        /// </summary>
        /// <value><c>true</c> if this instance is dirty; otherwise, <c>false</c>.</value>
        public virtual bool IsDirty
        {
            get
            {
                return _isDirty;
            }
            set
            {
                _isDirty = false;
            }
        }
        #endregion



        #region Events and Delegats - needs work, not sure about this
        public delegate void HandlerItemStatePersisted(ItemHandlerEventArgs arg);
        public delegate void HandlerItemChildAdded(ItemHandlerEventArgs arg);
        public delegate void HandlerItemParentChanged(ItemHandlerEventArgs arg);
        public event HandlerItemStatePersisted OnStatePersisted;
        public event HandlerItemChildAdded OnChildAdded;
        public event HandlerItemParentChanged OnParentChanged;
        /// <summary>
        /// Items the parent changed.
        /// </summary>
        /// <param name="arg">The <see cref="T:Content.API.ItemHandlerEventArgs"/> instance containing the event data.</param>
        public virtual void ItemParentChanged(ItemHandlerEventArgs arg)
        {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// Items the child added.
        /// </summary>
        /// <param name="arg">The <see cref="T:Content.API.ItemHandlerEventArgs"/> instance containing the event data.</param>
        public virtual void ItemChildAdded(ItemHandlerEventArgs arg)
        {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// Items the state persisted.
        /// </summary>
        /// <param name="arg">The <see cref="T:Content.API.ItemHandlerEventArgs"/> instance containing the event data.</param>
        public virtual void ItemStatePersisted(ItemHandlerEventArgs arg)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region Public Methods
        
        #endregion

        /// <summary>
        /// Assigns the events.
        /// </summary>
        private void assignEvents()
        {
            // TODO: Events needs work, I dont know if i am doing a good job with them.
            this.OnStatePersisted += new HandlerItemStatePersisted(this.ItemStatePersisted);
            this.OnChildAdded += new HandlerItemChildAdded(this.ItemChildAdded);
            this.OnParentChanged += new HandlerItemParentChanged(this.ItemParentChanged);
        }

        /// <summary>
        /// Subscribes to item.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        public static void SubscribeToItem(long itemID)
        {
            // TODO: Persist subscription to event
            // TODO: need tables for subscriptions.
            throw new NotImplementedException();
        }



        #region IFormattable Members

        /// <summary>
        /// Formats the value of the current instance using the specified format.
        /// </summary>
        /// <param name="format">The <see cref="T:System.String"></see> specifying the format to use.-or- null to use the default format defined for the type of the <see cref="T:System.IFormattable"></see> implementation.</param>
        /// <param name="formatProvider">The <see cref="T:System.IFormatProvider"></see> to use to format the value.-or- null to obtain the numeric format information from the current locale setting of the operating system.</param>
        /// <returns>
        /// A <see cref="T:System.String"></see> containing the value of the current instance in the specified format.
        /// </returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            throw new Exception("The method or operation is not implemented.");

            // TODO: Default formating
            if (format == null) format = "G";

            if (formatProvider != null)
            {
                ICustomFormatter formatter = formatProvider.GetFormat(
                    this.GetType())
                    as ICustomFormatter;

                if (formatter != null)
                    return formatter.Format(format, this, formatProvider);
            }
            /*
            switch (format)
            {
                case "f": return _firstname;
                case "l": return _lastname;
                case "na": return string.Format("{0} {1} Age= {2}", _firstname, _lastname, _age.ToString());
                case "n":
                case "G":
                default: return string.Format("{0} {1}", _firstname, _lastname);
            }
             * */
            StringBuilder sb = new StringBuilder();
            sb.Append("ItemID = ").Append(_itemID).Append(Environment.NewLine);
            sb.Append("Parent ItemID = ").Append(_parentItemID).Append(Environment.NewLine);
            sb.Append("Title = ").Append(_title).Append(Environment.NewLine);
            sb.Append("Version = ").Append(_version).Append(Environment.NewLine);
            sb.Append("Date Created = ").Append(_dateCreated).Append(Environment.NewLine);
            sb.Append("Date Published = ").Append(_datePublished).Append(Environment.NewLine);
            sb.Append("Culture = ").Append(_culture).Append(Environment.NewLine);
            sb.Append("Content = ").Append(_content).Append(Environment.NewLine);
            return sb.ToString();
        }
        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return ToString("G", null);
        }

        /// <summary>
        /// Toes the string.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public string ToString(string format)
        {
            return ToString(format, null);
        }

        /// <summary>
        /// Toes the string.
        /// </summary>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns></returns>
        public string ToString(IFormatProvider formatProvider)
        {
            return ToString(null, formatProvider);
        }


        #endregion

        #region Equality
        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (Object.ReferenceEquals(this, obj)) return true;
            if (this.GetType() != obj.GetType()) return false;

            Item objItem = (Item)obj;
            return this.Equals(objItem);

            return false;
        }

        /// <summary>
        /// Equalses the specified obj.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public virtual bool Equals(Item obj)
        {
            if (_itemID.Equals(obj.ItemID) && _version.Equals(obj.Version)) return true;

            return false;
        }

        /// <summary>
        /// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }

    
}
