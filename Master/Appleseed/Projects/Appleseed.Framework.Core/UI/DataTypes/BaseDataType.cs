// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseDataType.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   Base Data Type
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.DataTypes
{
    using System;
    using System.ComponentModel;
    using System.Web.UI.WebControls;

    /// <summary>
    /// Base Data Type
    /// </summary>
    /// <typeparam name="T">
    /// The type of data.
    /// </typeparam>
    /// <typeparam name="TEditControl">
    /// The edit control for the data type.
    /// </typeparam>
    public class BaseDataType<T, TEditControl>
        where TEditControl : class
    {
        #region Constants and Fields

        /// <summary>
        /// The inner data source.
        /// </summary>
        protected object InnerDataSource;

        /// <summary>
        /// The control width.
        /// </summary>
        protected int ControlWidth = 350;

        /// <summary>
        /// The inner control.
        /// </summary>
        protected TEditControl InnerControl;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the data source.
        /// </summary>
        /// <value>
        /// The data source.
        /// </value>
        public virtual object DataSource
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
        /// <value>
        ///   The data text field.
        /// </value>
        public virtual string DataTextField { get; set; }

        /// <summary>
        ///   Gets or sets the data value field.
        /// </summary>
        /// <value>
        ///   The data value field.
        /// </value>
        public virtual string DataValueField { get; set; }

        /// <summary>
        ///   Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public virtual string Description
        {
            get
            {
                return typeof(T).Name;
            }
        }

        /// <summary>
        /// Gets or sets the edit control.
        /// </summary>
        /// <value>
        /// The edit control.
        /// </value>
        public virtual TEditControl EditControl
        {
            get
            {
                if (this.InnerControl == null)
                {
                    this.InitializeComponents();
                }

                if (typeof(TEditControl) == typeof(TextBox))
                {
                    var txt = this.InnerControl as TextBox;
                    if (txt != null)
                    {
                        txt.Text = Convert.ToString(this.Value);
                    }
                }
                else
                {
                    if (typeof(TEditControl) == typeof(CheckBox))
                    {
                        var cbx = this.InnerControl as CheckBox;
                        if (cbx != null)
                        {
                            cbx.Checked = Convert.ToBoolean(this.Value);
                        }
                    }
                    else
                    {
                        if (typeof(TEditControl) == typeof(ListControl))
                        {
                            var lc = this.InnerControl as ListControl;
                            if (lc != null)
                            {
                                lc.SelectedValue = Convert.ToString(this.Value);
                            }
                        }
                    }
                }

                // Return control
                return this.InnerControl;
            }

            set
            {
                this.InnerControl = value;

                if (typeof(TEditControl) == typeof(TextBox))
                {
                    var txt = this.InnerControl as TextBox;
                    if (txt != null)
                    {
                        if (typeof(T).Name == "Uri")
                            this.Value = (T)Convert.ChangeType((new System.Uri(txt.Text)),typeof(T));
                        else
                            this.Value = (T)Convert.ChangeType(txt.Text, typeof(T));
                    }
                }
                else
                {
                    if (typeof(TEditControl) == typeof(CheckBox))
                    {
                        var cbx = this.InnerControl as CheckBox;
                        if (cbx != null)
                        {
                            this.Value = (T)Convert.ChangeType(cbx.Checked, typeof(T));
                        }
                    }
                    else
                    {
                        if (typeof(TEditControl) == typeof(ListControl))
                        {
                            var lc = this.InnerControl as ListControl;
                            if (lc != null)
                            {
                                this.Value = (T)Convert.ChangeType(lc.SelectedValue, typeof(T));
                            }
                        }
                        else
                        {
                            throw new NotImplementedException("Unknown editor type. Please implement a value setter here.");
                        }
                    }
                }
            }
        }

        /// <summary>
        ///   Gets the full path.
        /// </summary>
        public virtual string FullPath { get; }

        /// <summary>
        ///   Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public virtual T Value { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the components.
        /// </summary>
        protected virtual void InitializeComponents()
        {
            if (typeof(TEditControl) == typeof(TextBox))
            {
                // Text box
                // changed max value to 1500 since most of settings are string
                var tx =
                    new TextBox
                        {
                            CssClass = "NormalTextBox",
                            Columns = 30,
                            Width = new Unit(this.ControlWidth),
                            MaxLength = 1500
                        }

                    as TEditControl;

                this.InnerControl = tx;
            }
            else
            {
                this.InnerControl = (TEditControl)Activator.CreateInstance(typeof(TEditControl));
            }
        }

        #endregion
    }
}