// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UploadedFileDataType.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   Allows upload a file on current portal folder
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.DataTypes
{
    using System;
    using System.Web.UI.WebControls;

    using Appleseed.Framework.Web.UI.WebControls;

    /// <summary>
    /// Allows upload a file on current portal folder
    /// </summary>
    public class UploadedFileDataType : PortalUrlDataType
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "UploadedFileDataType" /> class.
        /// </summary>
        public UploadedFileDataType()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadedFileDataType"/> class.
        /// </summary>
        /// <param name="portalPath">
        /// The portal path.
        /// </param>
        public UploadedFileDataType(string portalPath)
            : base(portalPath)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the description.
        /// </summary>
        public override string Description
        {
            get
            {
                return "A file that can be uploaded to server";
            }
        }

        /// <summary>
        ///   Gets or sets the edit control.
        /// </summary>
        /// <value>
        ///   The edit control.
        /// </value>
        public override TextBox EditControl
        {
            get
            {
                if (this.InnerControl == null)
                {
                    this.InitializeComponents();
                }

                // Update value in control
                var upload = (UploadDialogTextBox)this.InnerControl;
                if (upload != null)
                {
                    upload.UploadDirectory = this.PortalPathPrefix;
                    upload.Text = this.Value;
                }

                // Return control
                return this.InnerControl;
            }

            set
            {
                if (value.GetType().Name == "UploadDialogTextBox")
                {
                    this.InnerControl = value;

                    // Update value from control
                    var upload = (UploadDialogTextBox)this.InnerControl;
                    this.Value = upload.Text;
                }
                else
                {
                    throw new ArgumentException(
                        "EditControl", 
                        string.Format("A UploadDialogTextBox values is required, a '{0}' is given.", value.GetType().Name));
                }
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
                // Remove portal path if present
                base.Value = value.StartsWith(this.PortalPathPrefix)
                                 ? value.Substring(this.PortalPathPrefix.Length)
                                 : value;
                base.Value = base.Value.TrimStart('/');
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the components.
        /// </summary>
        protected override void InitializeComponents()
        {
            // UploadDialogTextBox
            using (var upload = new UploadDialogTextBox())
            {
                upload.AllowEditTextBox = true;
                upload.Width = new Unit(this.ControlWidth);
                upload.CssClass = "NormalTextBox";

                this.InnerControl = upload;
            }
        }

        #endregion
    }
}