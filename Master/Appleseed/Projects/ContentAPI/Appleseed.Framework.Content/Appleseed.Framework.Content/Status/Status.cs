using System;
using System.Collections.Generic;
using System.Text;

namespace Content.API
{
    [Serializable]
    public class Status
    {
        private int _statusid = 0;
        private string _name = string.Empty;
        private string _description = string.Empty;
        private int _category = 0;
        private bool statusIsDirty = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Status"/> class.
        /// </summary>
        public Status()
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Status"/> class.
        /// </summary>
        /// <param name="stautsID">The stauts ID.</param>
        public Status(int stautsID)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Status"/> class.
        /// </summary>
        /// <param name="stautsID">The stauts ID.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        public Status(int stautsID, string name, string description)
        {
            _statusid = stautsID;
            _name = name;
            _description = description;
        }

        /// <summary>
        /// Gets the ID.
        /// </summary>
        /// <value>The ID.</value>
        public int ID
        {
            get { return _statusid; }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get { return _description; }
        }

        /// <summary>
        /// Gets the category.
        /// </summary>
        /// <value>The category.</value>
        public int CategoryID
        {
            get { return _category; }
            set { 
                _category = value;
                statusIsDirty = true;
            }
        }

        /// <summary>
        /// Gets the category.
        /// </summary>
        /// <value>The category.</value>
        public Category Category
        {
            get
            {
                return new Category(_category);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is dirty.
        /// </summary>
        /// <value><c>true</c> if this instance is dirty; otherwise, <c>false</c>.</value>
        public bool IsDirty
        {
            get
            {
                return statusIsDirty;
            }
        }


        /// <summary>
        /// Updates the category.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        public bool UpdateCategory(string name, string description, int category)
        {
            _name = name;
            _description = description;
            _category = category;

            Save();
            return true;
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        protected void Save()
        {
            throw new System.NotImplementedException();
        }
    }
}
