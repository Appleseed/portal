// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FolderDataType.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   Implements a DropDownList with a list of directories
//   and a TextBox for add and create a new directory.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.DataTypes
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    /// Implements a DropDownList with a list of directories 
    ///   and a TextBox for add and create a new directory.
    /// </summary>
    public class FolderDataType : BaseDataType<string, Panel>
    {
        #region Constants and Fields

        /// <summary>
        /// The base directory.
        /// </summary>
        private readonly string baseDirectory;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FolderDataType"/> class.
        /// </summary>
        /// <param name="baseDirectory">
        /// The base directory.
        /// </param>
        /// <param name="defaultDirectory">
        /// The default directory.
        /// </param>
        public FolderDataType(string baseDirectory, string defaultDirectory)
        {
            this.baseDirectory = baseDirectory;
            
            // this.Type = PropertiesDataType.List;
            if (defaultDirectory != null)
            {
                try
                {
                    if (!Directory.Exists(string.Format("{0}/{1}", baseDirectory, defaultDirectory)))
                    {
                        if (!Directory.Exists(baseDirectory))
                        {
                            Directory.CreateDirectory(baseDirectory);
                        }

                        Directory.CreateDirectory(string.Format("{0}/{1}", baseDirectory, defaultDirectory));
                    }
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(
                        string.Format("Cannot create the default directory '{0}'", defaultDirectory), 
                        "defaultDirectory", 
                        ex);
                }
            }

            var result =
                new DirectoryInfo(baseDirectory).GetDirectories().Where(di => di.Name != "CVS" && di.Name != "_svn").
                    Select(di => new ListItem(di.Name, di.Name)).ToList();
            this.InnerDataSource = result;
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
        public override Panel EditControl
        {
            get
            {
                if (this.InnerControl == null)
                {
                    this.InitializeComponents();
                }

                var panel = this.InnerControl;
                DropDownList dd = null;
                if (panel != null)
                {
                    foreach (var c in panel.Controls.OfType<DropDownList>())
                    {
                        // Update value in control
                        dd = c;
                        dd.ClearSelection();
                        if (dd.Items.FindByValue(this.Value) != null)
                        {
                            dd.Items.FindByValue(this.Value).Selected = true;
                        }
                    }
                }

                // Return control
                return this.InnerControl;
            }

            set
            {
                this.InnerControl = value;

                // Update value from control
                DropDownList dd = null;
                foreach (Control c in value.Controls)
                {
                    if (c is DropDownList)
                    {
                        dd = (DropDownList)c;
                        this.Value = dd.SelectedItem != null ? dd.SelectedItem.Value : string.Empty;
                    }

                    if (c is TextBox)
                    {
                        var tb = (TextBox)c;
                        if (tb.Text != General.GetString("NEW_FOLDER", "New Folder ?"))
                        {
                            try
                            {
                                if (!Directory.Exists(string.Format("{0}/{1}", this.baseDirectory, tb.Text)))
                                {
                                    Directory.CreateDirectory(string.Format("{0}/{1}", this.baseDirectory, tb.Text));
                                    if (dd != null)
                                    {
                                        dd.Items.Add(new ListItem(tb.Text, tb.Text));
                                        dd.Items.FindByValue(tb.Text).Selected = true;
                                        dd.SelectedIndex = dd.Items.Count - 1;
                                        if (dd.SelectedItem != null)
                                        {
                                            this.Value = dd.SelectedItem.Value;
                                        }
                                    }
                                }
                            }
                            catch
                            {
                            }

                            tb.Text = General.GetString("NEW_FOLDER", "New Folder ?");
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
            var dd = new DropDownList
                {
                    CssClass = "NormalTextBox", 
                    Width = new Unit(this.ControlWidth), 
                    DataSource = this.DataSource, 
                    DataValueField = this.DataValueField, 
                    DataTextField = this.DataTextField
                };

            dd.DataBind();
            dd.Width = (this.ControlWidth / 2) - 1;

            var panel = new Panel();

            dd.ID = string.Format("{0}dd", panel.ID);

            panel.Controls.Add(dd);

            var tb = new TextBox
                {
                    CssClass = "NormalTextBox", 
                    Text = General.GetString("NEW_FOLDER", "New Folder ?"), 
                    Columns = 30, 
                    Width = (base.ControlWidth / 2) - 1, 
                    ID = string.Format("{0}tb", panel.ID), 
                    MaxLength = 1500
                };

            panel.Controls.Add(tb);

            this.InnerControl = panel;
        }

        #endregion
    }
}