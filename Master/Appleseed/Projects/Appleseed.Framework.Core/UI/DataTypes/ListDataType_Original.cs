// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListDataType_Original.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   List Data Type
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.DataTypes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Web.UI.WebControls;

    /// <summary>
    /// List Data Type
    /// </summary>
    /// <typeparam name="T">
    /// The value type.
    /// </typeparam>
    /// <typeparam name="TEditor">
    /// The editor control type.
    /// </typeparam>
    public class ListDataType<T, TEditor> : BaseDataType<T, TEditor>
        where TEditor : ListControl
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ListDataType&lt;T, TEditor&gt;"/> class.
        /// </summary>
        public ListDataType()
        {
            // this.Type = PropertiesDataType.List;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListDataType&lt;T, TEditor&gt;"/> class.
        /// </summary>
        /// <param name="csvList">The CSV list.</param>
        public ListDataType(string csvList)
        {
            // this.Type = PropertiesDataType.List;
            this.InnerDataSource = csvList;

            // InitializeComponents();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListDataType&lt;T, TEditor&gt;"/> class.
        /// </summary>
        /// <param name="fileList">The file list.</param>
        public ListDataType(IEnumerable<FileInfo> fileList)
        {
            // this.Type = PropertiesDataType.List;
            this.InnerDataSource = fileList;

            // this._dataTextField = "Name";
            // this._dataValueField = "Name";
            // InitializeComponents();
            // innerControl.DataBind();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListDataType&lt;T, TEditor&gt;"/> class.
        /// </summary>
        /// <param name="dataSource">The data source.</param>
        /// <param name="textField">The text field.</param>
        /// <param name="dataField">The data field.</param>
        public ListDataType(object dataSource, string textField, string dataField)
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
                return this.InnerDataSource == null
                           ? null
                           : (this.InnerDataSource is string
                                  ? this.InnerDataSource.ToString().Split(';')
                                  : this.InnerDataSource);
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

        /*
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public override string Value
        {
            get { return (this.Value); }
            set
            {
                this.Value = value;

                // DropDownList dd = (DropDownList) innerControl;
                // if (dd.Items.FindByValue(value) != null)
                // {
                //     dd.ClearSelection();
                //     dd.Items.FindByValue(value).Selected = true;
                //     innerValue = value;
                // }
                // else
                // {
                //     //Invalid value
                // }            
            }
        }
        */

        /// <summary>
        ///   Gets or sets the edit control.
        /// </summary>
        /// <value>
        ///   The edit control.
        /// </value>
        public override TEditor EditControl
        {
            get
            {
                if (this.InnerControl == null)
                {
                    this.InitializeComponents();
                }

                // Update value in control
                var dd = this.InnerControl;
                dd.ClearSelection();
                if (this.Value != null)
                {
                    if (dd.Items.FindByValue(this.Value.ToString()) != null)
                    {
                        dd.Items.FindByValue(this.Value.ToString()).Selected = true;
                    }
                }

                // Return control
                return this.InnerControl;
            }

            set
            {
                if (value.GetType().Name != "DropDownList")
                {
                    throw new ArgumentException(
                        "EditControl", 
                        string.Format("A DropDownList values is required, a '{0}' is given.", value.GetType().Name));
                }

                this.InnerControl = value;

                // Update value from control
                var conv = TypeDescriptor.GetConverter(typeof(T));
                if (conv != null)
                {
                    var conv2 = TypeDescriptor.GetConverter(typeof(TEditor));
                    if (conv2 != null)
                    {
                        var dd = this.InnerControl as DropDownList;
                        if (dd != null)
                        {
                            //var val = conv.ConvertFrom(dd.SelectedItem.Value);
                            this.Value = (dd.SelectedItem != null ? (conv.ConvertFrom(dd.SelectedItem.Value)) : string.Empty) is T ? 
                                (T)(dd.SelectedItem != null ? (conv.ConvertFrom(dd.SelectedItem.Value)) : string.Empty) : default(T);
                        }
                    }
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the components.
        /// </summary>
        protected override void InitializeComponents()
        {
            // Dropdown list
            var dd = new DropDownList
                {
                    CssClass = "NormalTextBox", 
                    Width = new Unit(this.ControlWidth), 
                    DataSource = this.DataSource, 
                    DataValueField = this.DataValueField, 
                    DataTextField = this.DataTextField
                };
            dd.DataBind();

            this.InnerControl = dd as TEditor;
        }

        #endregion
    }
}