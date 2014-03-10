// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmailForm.ascx.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   The email form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Content.Web.Modules
{
    using System;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Appleseed.Framework;
    using Appleseed.Framework.DataTypes;
    using Appleseed.Framework.Site.Configuration;
    using Appleseed.Framework.Web.UI.WebControls;

    using Label = Appleseed.Framework.Web.UI.WebControls.Label;
    using Localize = Appleseed.Framework.Web.UI.WebControls.Localize;

    /// <summary>
    /// The email form.
    /// </summary>
    public class EmailForm : UserControl
    {
        #region Constants and Fields

        /// <summary>
        ///     The literal 1.
        /// </summary>
        protected Localize Literal1;

        /// <summary>
        ///     The literal 2.
        /// </summary>
        protected Localize Literal2;

        /// <summary>
        ///     The literal 3.
        /// </summary>
        protected Localize Literal3;

        /// <summary>
        ///     The literal 4.
        /// </summary>
        protected Localize Literal4;

        /// <summary>
        ///     The place holder html editor.
        /// </summary>
        protected PlaceHolder PlaceHolderHTMLEditor;

        /// <summary>
        ///     The lbl email addresses not ok.
        /// </summary>
        protected Label lblEmailAddressesNotOk;

        /// <summary>
        ///     BCC List
        /// </summary>
        protected TextBox txtBcc;

        /// <summary>
        ///     BOdy Area
        /// </summary>
        protected IHtmlEditor txtBody;

        /// <summary>
        ///     CC List
        /// </summary>
        protected TextBox txtCc;

        /// <summary>
        ///     Subject Textbox
        /// </summary>
        protected TextBox txtSubject;

        /// <summary>
        ///     To List
        /// </summary>
        protected TextBox txtTo;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref = "EmailForm" /> class.
        /// </summary>
        public EmailForm()
        {
            this.AllEmailAddressesOk = true;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets a value indicating whether [all email addresses ok].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [all email addresses ok]; otherwise, <c>false</c>.
        /// </value>
        public bool AllEmailAddressesOk { get; private set; }

        /// <summary>
        ///     Gets a collection containing all bcc email addresses
        /// </summary>
        public EmailAddressList Bcc { get; private set; }

        /// <summary>
        ///     Gets or sets text for the body of the email in plain text format
        /// </summary>
        public string BodyText
        {
            get
            {
                return this.txtBody.Text;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value", "BodyText can not contain null values!");
                }

                this.txtBody.Text = value;
            }
        }

        /// <summary>
        ///     Gets a collection containing all cc email addresses
        /// </summary>
        public EmailAddressList Cc { get; private set; }

        /// <summary>
        ///     Gets or sets text for the body of the email in html format
        /// </summary>
        public string HtmlBodyText
        {
            get
            {
                return this.txtBody.Text;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value", "HtmlBodyText can not contain null values!");
                }

                this.txtBody.Text = value;
            }
        }

        /// <summary>
        ///     Gets or sets subject
        /// </summary>
        public string Subject
        {
            get
            {
                return this.txtSubject.Text;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value", "Subject can not contain null values!");
                }

                this.txtSubject.Text = value;
            }
        }

        /// <summary>
        ///     Gets a collection containing all to email addresses
        /// </summary>
        public EmailAddressList To { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">
        /// An <see cref="T:System.EventArgs"/> object that contains the event data.
        /// </param>
        protected override void OnInit(EventArgs e)
        {
            this.To = new EmailAddressList();
            this.Cc = new EmailAddressList();
            this.Bcc = new EmailAddressList();

            var h = new HtmlEditorDataType();
            var pS = (PortalSettings)HttpContext.Current.Items["PortalSettings"];
            try
            {
                h.Value = pS.CustomSettings["SITESETTINGS_DEFAULT_EDITOR"].ToString();
                this.txtBody = h.GetEditor(
                    this.PlaceHolderHTMLEditor, 
                    int.Parse(this.Context.Request["mID"]), 
                    bool.Parse(pS.CustomSettings["SITESETTINGS_SHOWUPLOAD"].ToString()), 
                    pS);
            }
            catch
            {
                this.txtBody = h.GetEditor(this.PlaceHolderHTMLEditor, int.Parse(this.Context.Request["mID"]), true, pS);
            }

            this.lblEmailAddressesNotOk.Text = General.GetString(
                "EMF_ADDRESSES_NOT_OK", "The emailaddresses are not ok.", this.lblEmailAddressesNotOk);

            this.txtTo.Text = string.Join(";", (string[])this.To.ToArray(typeof(string)));
            this.txtCc.Text = string.Join(";", (string[])this.Cc.ToArray(typeof(string)));
            this.txtBcc.Text = string.Join(";", (string[])this.Bcc.ToArray(typeof(string)));

            base.OnInit(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">
        /// The <see cref="T:System.EventArgs"/> object that contains the event data.
        /// </param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (this.IsPostBack)
            {
                this.AllEmailAddressesOk = true;

                // Initialize To addresses
                foreach (var em in this.txtTo.Text.Split(";".ToCharArray()))
                {
                    try
                    {
                        if (em.Trim().Length != 0)
                        {
                            this.To.Add(em);
                        }
                    }
                    catch (ArgumentException)
                    {
                        this.AllEmailAddressesOk = false;
                    }
                }

                // Initialize Cc addresses
                foreach (var em in this.txtCc.Text.Split(";".ToCharArray()))
                {
                    try
                    {
                        if (em.Trim().Length != 0)
                        {
                            this.Cc.Add(em);
                        }
                    }
                    catch (ArgumentException)
                    {
                        this.AllEmailAddressesOk = false;
                    }
                }

                // Initialize To addresses
                foreach (var em in this.txtBcc.Text.Split(";".ToCharArray()))
                {
                    try
                    {
                        if (em.Trim().Length != 0)
                        {
                            this.Bcc.Add(em);
                        }
                    }
                    catch (ArgumentException)
                    {
                        this.AllEmailAddressesOk = false;
                    }
                }

                // Show error
                this.lblEmailAddressesNotOk.Visible = ! this.AllEmailAddressesOk;
            }
        }

        #endregion
    }
}