using Appleseed.Framework.Configuration.Items;
using Appleseed.Framework.Data;
using Appleseed.Framework.DataTypes;
using Appleseed.Framework.Site.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace Appleseed.Framework.UI.DataTypes
{
    /// <summary>
    /// Slider Data type
    /// </summary>
    public class SliderDataType : ListDataType<string, DropDownList>
    {
        #region Constructors and Destructors
        /// <summary>
        ///   Initializes a new instance of the <see cref = "SliderDataType" /> class.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public SliderDataType()
        {
            this.InitializeComponents();
        }
        #endregion

        #region Properties

        /// <summary>
        ///   Gets the data source.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public override object DataSource
        {
            get
            {
                SliderDB sldb = new SliderDB();
                return sldb.AllSliders();
            }
        }

        /// <summary>
        /// Data text field
        /// </summary>
        public override string DataTextField
        {
            get
            {
                return "SliderName";
            }
        }

        /// <summary>
        /// Data value field
        /// </summary>
        public override string DataValueField
        {
            get
            {
                return "SliderID";
            }
        }

        /// <summary>
        ///   Gets the description.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public override string Description
        {
            get
            {
                return "Sliders";
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initializes the components.
        /// </summary>
        protected override void InitializeComponents()
        {
            base.InitializeComponents();

        }
        #endregion
    }
}
