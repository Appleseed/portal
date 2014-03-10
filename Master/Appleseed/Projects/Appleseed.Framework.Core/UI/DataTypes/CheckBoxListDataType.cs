// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckBoxListDataType.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   CheckBoxListDataType
//   Implements a checkboxlist that allows multiple selections
//   (returns a colon-delimited string)
//   by Mike Stone
//   Mike Stone 23/01/2005 based on John Bowens Multiselectlist
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.DataTypes
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Web.UI.WebControls;

    /// <summary>
    /// CheckBoxListDataType
    ///   Implements a checkboxlist that allows multiple selections 
    ///   (returns a colon-delimited string)
    ///   by Mike Stone
    ///   Mike Stone 23/01/2005 based on John Bowens Multiselectlist
    /// </summary>
    public class CheckBoxListDataType : BaseDataType<string, CheckBoxList>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckBoxListDataType"/> class.
        /// </summary>
        /// <param name="dataSource">
        /// The data source.
        /// </param>
        /// <param name="textField">
        /// The text field.
        /// </param>
        /// <param name="dataField">
        /// The data field.
        /// </param>
        public CheckBoxListDataType(object dataSource, string textField, string dataField)
        {
            // this.Type = PropertiesDataType.List;
            this.InnerDataSource = dataSource;
            this.DataTextField = textField;
            this.DataValueField = dataField;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets DataSource
        ///   Should be overridden from inherited classes
        /// </summary>
        /// <value>The data source.</value>
        public override object DataSource
        {
            get
            {
                return this.InnerDataSource;
            }

            set
            {
                this.InnerDataSource = value;
            }
        }

        /// <summary>
        ///   Gets or sets the data text field.
        /// </summary>
        /// <value>The data text field.</value>
        public override string DataTextField { get; set; }

        /// <summary>
        ///   Gets or sets the data value field.
        /// </summary>
        /// <value>The data value field.</value>
        public override string DataValueField { get; set; }

        /// <summary>
        ///   Gets or sets the edit control.
        /// </summary>
        /// <value>
        ///   The edit control.
        /// </value>
        public override CheckBoxList EditControl
        {
            get
            {
                if (this.InnerControl == null)
                {
                    this.InitializeComponents();
                }

                // Update value in control
                var cbl = this.InnerControl;
                if (cbl != null)
                {
                    cbl.ClearSelection();

                    // Store in string array
                    var values = this.Value.Split(new[] { ';' });
                    foreach (var value in values.Where(value => cbl.Items.FindByValue(value) != null))
                    {
                        cbl.Items.FindByValue(value).Selected = true;
                    }
                }

                // Return control
                return this.InnerControl;
            }

            set
            {
                if (value.GetType().Name != "CheckBoxList")
                {
                    throw new ArgumentException(
                        "EditControl", 
                        string.Format("A CheckBoxList value is required, a '{0}' is given.", value.GetType().Name));
                }

                this.InnerControl = value;

                // Update value from control
                var cbl = this.InnerControl;
                var sb = new StringBuilder();

                foreach (var item in cbl.Items.Cast<ListItem>().Where(item => item.Selected))
                {
                    sb.Append(item.Value);
                    sb.Append(";");
                }

                this.Value = sb.ToString();
            }
        }

        /// <summary>
        ///   Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public override string Value
        {
            get
            {
                return base.Value;
            }

            set
            {
                // Remove trailing ';'
                base.Value = value.TrimEnd(new[] { ';' });
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the components.
        /// </summary>
        protected override void InitializeComponents()
        {
            var cbl = new CheckBoxList
                {
                    Width = new Unit("100%"), 
                    RepeatColumns = 2, 
                    DataSource = this.DataSource, 
                    DataValueField = this.DataValueField, 
                    DataTextField = this.DataTextField
                };

            // cbl.CssClass = "NormalTextBox";
            cbl.DataBind();

            this.InnerControl = cbl;
        }

        #endregion
    }
}