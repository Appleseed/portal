namespace Appleseed.Framework.DataTypes
{
    /// <summary>
    /// Structure used for list settings
    /// </summary>
    public struct SettingOption
    {
        private int val;
        private string name;

        /// <summary>
        /// Gets or sets the val.
        /// </summary>
        /// <value>The val.</value>
        public int Val
        {
            get { return val; }
            set { val = value; }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingOption"/> class.
        /// </summary>
        /// <param name="aVal">A val.</param>
        /// <param name="aName">A name.</param>
        public SettingOption(int aVal, string aName)
        {
            val = aVal;
            name = aName;
        }
    }
}