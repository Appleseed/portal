namespace Appleseed.Content.Web.Modules
{
    /// <summary>
    /// Class for storing a collection of table types
    /// </summary>
    public class UserDefinedTableType
    {
        private string typeText;
        private string typeValue;

        /// <summary>
        /// Gets or sets the type text.
        /// </summary>
        /// <value>The type text.</value>
        public string TypeText
        {
            get { return typeText; }
            set { typeText = value; }
        }

        /// <summary>
        /// Gets or sets the type value.
        /// </summary>
        /// <value>The type value.</value>
        public string TypeValue
        {
            get { return typeValue; }
            set { typeValue = value; }
        }
    }
}