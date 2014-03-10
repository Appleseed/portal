using System;
using System.Collections.Generic;
using System.Text;

namespace Content.API
{
    [Serializable]
    public class Category
    {
        private int _categoryid = 0;
        private string _name = string.Empty;
        private string _description = string.Empty;
        private int[] _categoryParents;
        private int[] _categoryChildren;

        /// <summary>
        /// Gets or sets the category parents.
        /// </summary>
        /// <value>The category parents.</value>
        public int[] CategoryParents
        {
            get { return _categoryParents; }
            set { _categoryParents = value; }
        }

        /// <summary>
        /// Gets or sets the category children.
        /// </summary>
        /// <value>The category children.</value>
        public int[] CategoryChildren
        {
            get { return _categoryChildren; }
            set { _categoryChildren = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Category"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        public Category(int id)
        {
            _categoryid = id;
            LoadCategoryData();
        }

        /// <summary>
        /// Loads the category data.
        /// </summary>
        private void LoadCategoryData()
        {
            _name = "Fetch Category Name not implemented yet";
            _description = "Fetch of description not implemented, maybe should be lazy load";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Category"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        public Category(int id, string name, string description)
        {
            _categoryid = id;
            _name = name;
            _description = description;
        }

        /// <summary>
        /// Gets the ID.
        /// </summary>
        /// <value>The ID.</value>
        public int ID
        {
            get { return _categoryid; }
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
        /// Updates the category.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <returns></returns>
        public bool UpdateCategory(string name, string description)
        {
            _name = name;
            _description = description;
            
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
