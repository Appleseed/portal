// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultiSelectListDataType.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   Multi Select List Data Type
//   Implements a list box that allows multiple selections
//   (returns a colon-delimited string)
//   by John Bowen
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.DataTypes
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Web.UI.WebControls;

    /// <summary>
    /// Multi Select List Data Type
    ///   Implements a list box that allows multiple selections 
    ///   (returns a colon-delimited string)
    ///   by John Bowen
    /// </summary>
    public class MultiSelectListDataType : BaseDataType<string, ListControl>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiSelectListDataType"/> class.
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
        public MultiSelectListDataType(object dataSource, string textField, string dataField)
        {
            // this.Type = PropertiesDataType.List;
            this.InnerDataSource = dataSource;
            this.DataTextField = textField;
            this.DataValueField = dataField;

            // InitializeComponents();
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
                // if(InnerDataSource != null)
                return this.InnerDataSource;

                // else
                // return null;
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
        public override ListControl EditControl
        {
            get
            {
                if (this.InnerControl == null)
                {
                    this.InitializeComponents();
                }

                // Update value in control
                var lb = this.InnerControl;
                lb.ClearSelection();

                // Store in string array
                var values = this.Value.Split(new[] { ';' });
                foreach (var value in values.Where(value => lb.Items.FindByValue(value) != null))
                {
                    lb.Items.FindByValue(value).Selected = true;
                }

                // Return control
                return this.InnerControl;
            }

            set
            {
                if (value.GetType().Name != "ListBox")
                {
                    throw new ArgumentException(
                        "EditControl",
                        string.Format("A ListBox value is required, a '{0}' is given.", value.GetType().Name));
                }
                
                this.InnerControl = value;

                // Update value from control
                var lb = this.InnerControl;
                var sb = new StringBuilder();
                foreach (var item in lb.Items.Cast<ListItem>().Where(item => item.Selected))
                {
                    sb.Append(item.Value);
                    sb.Append(";");
                }

                this.Value = sb.ToString();
            }
        }

        private string theValue;

        /// <summary>
        ///   Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public override string Value
        {
            get
            {
                return this.theValue;
            }

            set
            {
                // Remove trailing ';'
                this.theValue = value.TrimEnd(new[] { ';' });

                // //Fix by manu          
                // ListBox lb = (ListBox)innerControl;
                // lb.SelectionMode = ListSelectionMode.Multiple;
                // lb.ClearSelection();
                // //Clear inner value
                // innerValue = string.Empty;
                // if (value != null)
                // {
                // //Remove trailing ;
                // value = value.TrimEnd(new char[] { ';' });
                // // Store in string array
                // string[] values = value.Split(new char[] { ';' });
                // foreach (string _value in values)
                // {
                // if (lb.Items.FindByValue(_value) != null)
                // {
                // lb.Items.FindByValue(_value).Selected = true;
                // innerValue += value + ";";
                // }
                // }
                // }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the components.
        /// </summary>
        protected override void InitializeComponents()
        {
            var lb = new ListBox
                {
                    CssClass = "multiselect", 
                    SelectionMode = ListSelectionMode.Multiple, 
                    //Width = new Unit(this.ControlWidth), 
                    //Height = new Unit(400),
                    //Style = "width "
                    DataSource = this.DataSource, 
                    DataValueField = this.DataValueField, 
                    DataTextField = this.DataTextField,
                    Rows = 20
                };
            lb.DataBind();

            // Provide a smart height depending on items number
            if (lb.Items.Count > 5)
            {
                lb.Rows = 5;
            }

            if (lb.Items.Count > 10)
            {
                lb.Rows = 10;
            }

            if (lb.Items.Count > 20)
            {
                lb.Rows = 15;
            }

            this.InnerControl = lb;
        }

        #endregion
    }
}