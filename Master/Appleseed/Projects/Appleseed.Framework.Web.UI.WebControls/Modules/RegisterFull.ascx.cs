// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegisterFull.ascx.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   Placeable Registration (Full) module
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Content.Web.Modules
{
    using System;
    using System.Data.SqlClient;
    using System.Security.Principal;
    using System.Text;
    using System.Web;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    using Appleseed.Framework;
    using Appleseed.Framework.Helpers;
    using Appleseed.Framework.Providers.Geographic;
    using Appleseed.Framework.Security;
    using Appleseed.Framework.Settings;
    using Appleseed.Framework.Site.Configuration;
    using Appleseed.Framework.Users.Data;
    using Appleseed.Framework.Web.UI.WebControls;

    using Label = Appleseed.Framework.Web.UI.WebControls.Label;
    using LinkButton = Appleseed.Framework.Web.UI.WebControls.LinkButton;

    /// <summary>
    /// Placeable Registration (Full) module
    /// </summary>
    [History("jminond", "march 2005", "Changes for moving Tab to Page")]
    [History("john.mandia@whitelightsolutions.com", "2005/02/12", 
        "Adding handling for unique constraint violation and showed friendlier message.")]
    [History("bill@improvdesign.com", "2004/10/23", "Added email to admin on registration")]
    [History("john.mandia@whitelightsolutions.com", "2003/10/31", 
        "Fixed bug where old state list would remain even though new country with no states had been selected.")]
    [History("Mario Hartmann", "mario@hartmann.net", "1.2", "2003/10/08", "moved to seperate folder")]
    [History("Jes1111", "2003/03/10", "Modified from original to be fully placeable module")]
    [History("Jes1111", "2003/03/10", "Updated to use Globalized controls")]
    // TODO still needs Globalized field validators (Compare and Regex)
    public class RegisterFull : PortalModuleControl, IEditUserProfile
    {
        #region Constants and Fields

        /// <summary>
        ///     The accept.
        /// </summary>
        protected CheckBox Accept;

        /// <summary>
        ///     The account label.
        /// </summary>
        protected Label AccountLabel;

        /// <summary>
        ///     The address field.
        /// </summary>
        protected TextBox AddressField;

        /// <summary>
        ///     The address label.
        /// </summary>
        protected Label AddressLabel;

        /// <summary>
        ///     The check id.
        /// </summary>
        protected CompareValidator CheckID;

        /// <summary>
        ///     The check terms validator.
        /// </summary>
        protected CustomValidator CheckTermsValidator;

        /// <summary>
        ///     The city field.
        /// </summary>
        protected TextBox CityField;

        /// <summary>
        ///     The city label.
        /// </summary>
        protected Label CityLabel;

        /// <summary>
        ///     The company field.
        /// </summary>
        protected TextBox CompanyField;

        /// <summary>
        ///     The company label.
        /// </summary>
        protected Label CompanyLabel;

        /// <summary>
        ///     The compare passwords.
        /// </summary>
        protected CompareValidator ComparePasswords;

        /// <summary>
        ///     The conditions label.
        /// </summary>
        protected Label ConditionsLabel;

        /// <summary>
        ///     The conditions row.
        /// </summary>
        protected HtmlTableRow ConditionsRow;

        /// <summary>
        ///     The confirm password field.
        /// </summary>
        protected TextBox ConfirmPasswordField;

        /// <summary>
        ///     The confirm password label.
        /// </summary>
        protected Label ConfirmPasswordLabel;

        /// <summary>
        ///     The country field.
        /// </summary>
        protected DropDownList CountryField;

        /// <summary>
        ///     The country label.
        /// </summary>
        protected Label CountryLabel;

        /// <summary>
        ///     The edit password row.
        /// </summary>
        protected HtmlTableRow EditPasswordRow;

        /// <summary>
        ///     The email field.
        /// </summary>
        protected TextBox EmailField;

        /// <summary>
        ///     The email label.
        /// </summary>
        protected Label EmailLabel;

        /// <summary>
        ///     The fax field.
        /// </summary>
        protected TextBox FaxField;

        /// <summary>
        ///     The fax label.
        /// </summary>
        protected Label FaxLabel;

        /// <summary>
        ///     The field conditions.
        /// </summary>
        protected TextBox FieldConditions;

        /// <summary>
        ///     The full profile information.
        /// </summary>
        protected Panel FullProfileInformation;

        /// <summary>
        ///     The in label.
        /// </summary>
        protected Label InLabel;

        /// <summary>
        ///     The label 1.
        /// </summary>
        protected Label Label1;

        /// <summary>
        ///     The message.
        /// </summary>
        protected Label Message;

        /// <summary>
        ///     The name field.
        /// </summary>
        protected TextBox NameField;

        /// <summary>
        ///     The name label.
        /// </summary>
        protected Label NameLabel;

        /// <summary>
        ///     The page title label.
        /// </summary>
        protected Label PageTitleLabel;

        /// <summary>
        ///     The password field.
        /// </summary>
        protected TextBox PasswordField;

        /// <summary>
        ///     The password label.
        /// </summary>
        protected Label PasswordLabel;

        /// <summary>
        ///     The phone field.
        /// </summary>
        protected TextBox PhoneField;

        /// <summary>
        ///     The phone label.
        /// </summary>
        protected Label PhoneLabel;

        /// <summary>
        ///     The register btn.
        /// </summary>
        protected LinkButton RegisterBtn;

        /// <summary>
        ///     The required confirm.
        /// </summary>
        protected RequiredFieldValidator RequiredConfirm;

        /// <summary>
        ///     The required email.
        /// </summary>
        protected RequiredFieldValidator RequiredEmail;

        /// <summary>
        ///     The required name.
        /// </summary>
        protected RequiredFieldValidator RequiredName;

        /// <summary>
        ///     The required password.
        /// </summary>
        protected RequiredFieldValidator RequiredPassword;

        /// <summary>
        ///     The save changes btn.
        /// </summary>
        protected LinkButton SaveChangesBtn;

        /// <summary>
        ///     The send newsletter.
        /// </summary>
        protected CheckBox SendNewsletter;

        /// <summary>
        ///     The send newsletter label.
        /// </summary>
        protected Label SendNewsletterLabel;

        /// <summary>
        ///     The state field.
        /// </summary>
        protected DropDownList StateField;

        /// <summary>
        ///     The state label.
        /// </summary>
        protected Label StateLabel;

        /// <summary>
        ///     The state row.
        /// </summary>
        protected HtmlTableRow StateRow;

        /// <summary>
        ///     The this country label.
        /// </summary>
        protected Label ThisCountryLabel;

        /// <summary>
        ///     The valid email.
        /// </summary>
        protected RegularExpressionValidator ValidEmail;

        /// <summary>
        ///     The zip field.
        /// </summary>
        protected TextBox ZipField;

        /// <summary>
        ///     The zip label.
        /// </summary>
        protected Label ZipLabel;

        /// <summary>
        ///     The cancel button.
        /// </summary>
        protected LinkButton cancelButton;

        /// <summary>
        ///     The redirect page.
        /// </summary>
        private string redirectPage;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets a value indicating whether control is on edit mode
        /// </summary>
        /// <value></value>
        public bool EditMode
        {
            get
            {
                return this.UserName.Length != 0;
            }
        }

        /// <summary>
        ///     GUID of module (mandatory)
        /// </summary>
        /// <value></value>
        public override Guid GuidID
        {
            get
            {
                return new Guid("{AE419DCC-B890-43ba-B77C-54955F182041}");
            }
        }

        /// <summary>
        ///     Gets or sets the redirect page.
        /// </summary>
        public string RedirectPage
        {
            get
            {
                if (this.redirectPage == null)
                {
                    // changed by Mario Endara <mario@softworks.com.uy> (2004/11/05)
                    // it's necessary the ModuleID in the URL to apply security checking in the target
                    return string.Format(
                        "{0}?TabID={1}&mID={2}&username={3}", 
                        this.Request.Url.Segments[this.Request.Url.Segments.Length - 1], 
                        this.PageID, 
                        this.ModuleID, 
                        this.EmailField.Text);
                }

                return this.redirectPage;
            }

            set
            {
                this.redirectPage = value;
            }
        }

        /// <summary>
        ///     Gets CurrentPortalSettings.
        /// </summary>
        private static PortalSettings CurrentPortalSettings
        {
            get
            {
                return (PortalSettings)HttpContext.Current.Items["PortalSettings"];
            }
        }

        /// <summary>
        ///     Gets or sets originalPassword.
        /// </summary>
        private string OriginalPassword
        {
            get
            {
                return this.ViewState["originalPassword"] != null
                           ? (string)this.ViewState["originalPassword"]
                           : string.Empty;
            }

            set
            {
                this.ViewState["originalPassword"] = value;
            }
        }

        /// <summary>
        ///     Gets or sets originalUserID.
        /// </summary>
        private Guid OriginalUserId
        {
            get
            {
                return this.ViewState["originalUserID"] != null ? (Guid)this.ViewState["originalUserID"] : Guid.Empty;
            }

            set
            {
                this.ViewState["originalUserID"] = value;
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether selfEdit.
        /// </summary>
        private bool SelfEdit
        {
            get
            {
                return this.ViewState["selfEdit"] != null && (bool)this.ViewState["selfEdit"];
            }

            set
            {
                this.ViewState["selfEdit"] = value;
            }
        }

        /// <summary>
        ///     Gets userName.
        /// </summary>
        private string UserName
        {
            get
            {
                var uid = string.Empty;

                if (this.Request.Params["userName"] != null)
                {
                    uid = this.Request.Params["userName"];
                }

                if (uid.Length == 0 && HttpContext.Current.Items["userName"] != null)
                {
                    uid = HttpContext.Current.Items["userName"].ToString();
                }

#if DEBUG

                // TODO: Remove this.
                if (uid.Length == 0)
                {
                    HttpContext.Current.Response.Write("username is empty");
                }

#endif

                return uid;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sends registration information to portal administrator.
        /// </summary>
        public void SendRegistrationNoticeToAdmin()
        {
            var sb = new StringBuilder();

            sb.Append("New User Registration\n");
            sb.Append("---------------------\n");
            sb.Append("PORTAL         : " + this.PortalSettings.PortalTitle + "\n");
            sb.Append("Name           : " + this.NameField.Text + "\n");
            sb.Append("Company        : " + this.CompanyField.Text + "\n");
            sb.Append("Address        : " + this.AddressField.Text + "\n");
            sb.Append("                 " + this.CityField.Text + ", ");
            if (this.StateField.SelectedItem != null)
            {
                sb.Append(this.StateField.SelectedItem.Text + "  ");
            }

            sb.Append(this.ZipField.Text + "\n");
            sb.Append("                 " + this.CountryField.SelectedItem.Text + "\n");
            sb.Append("                 " + this.PhoneField.Text + "\n");
            sb.Append("Fax            : " + this.FaxField.Text + "\n");
            sb.Append("Email          : " + this.EmailField.Text + "\n");
            sb.Append("Send Newsletter: " + this.SendNewsletter.Checked + "\n");

            MailHelper.SendMailNoAttachment(
                this.PortalSettings.CustomSettings["SITESETTINGS_ON_REGISTER_SEND_TO"].ToString(), 
                this.PortalSettings.CustomSettings["SITESETTINGS_ON_REGISTER_SEND_TO"].ToString(), 
                "New User Registration for " + this.PortalSettings.PortalAlias, 
                sb.ToString(), 
                string.Empty, 
                string.Empty, 
                Config.SmtpServer);
        }

        #endregion

        #region Implemented Interfaces

        #region IEditUserProfile

        /// <summary>
        /// Save user data
        /// </summary>
        /// <returns>
        /// The user id
        /// </returns>
        public Guid SaveUserData()
        {
            var returnId = Guid.Empty;

            if (this.PasswordField.Text.Length > 0 || this.ConfirmPasswordField.Text.Length > 0)
            {
                if (this.PasswordField.Text != this.ConfirmPasswordField.Text)
                {
                    this.ComparePasswords.IsValid = false;
                }
            }

            // Only attempt a login if all form fields on the page are valid
            if (this.Page.IsValid)
            {
                var accountSystem = new UsersDB();

                var countryId = string.Empty;
                if (this.CountryField.SelectedItem != null)
                {
                    countryId = this.CountryField.SelectedItem.Value;
                }

                var stateId = 0;
                if (this.StateField.SelectedItem != null)
                {
                    stateId = Convert.ToInt32(this.StateField.SelectedItem.Value);
                }

                try
                {
                    if (this.UserName == string.Empty)
                    {
                        // Add New User to Portal User Database
                        returnId = accountSystem.AddUser(
                            this.NameField.Text, 
                            this.CompanyField.Text, 
                            this.AddressField.Text, 
                            this.CityField.Text, 
                            this.ZipField.Text, 
                            countryId, 
                            stateId, 
                            this.PhoneField.Text, 
                            this.FaxField.Text, 
                            this.PasswordField.Text, 
                            this.EmailField.Text, 
                            this.SendNewsletter.Checked, 
                            CurrentPortalSettings.PortalAlias);
                    }
                    else
                    {
                        // Update user
                        if (this.PasswordField.Text.Equals(this.ConfirmPasswordField.Text) &&
                            this.PasswordField.Text.Equals(string.Empty))
                        {
                            accountSystem.UpdateUser(
                                this.OriginalUserId, 
                                this.NameField.Text, 
                                this.CompanyField.Text, 
                                this.AddressField.Text, 
                                this.CityField.Text, 
                                this.ZipField.Text, 
                                countryId, 
                                stateId, 
                                this.PhoneField.Text, 
                                this.FaxField.Text, 
                                this.EmailField.Text, 
                                this.SendNewsletter.Checked);
                        }
                        else
                        {
                            accountSystem.UpdateUser(
                                this.OriginalUserId, 
                                this.NameField.Text, 
                                this.CompanyField.Text, 
                                this.AddressField.Text, 
                                this.CityField.Text, 
                                this.ZipField.Text, 
                                countryId, 
                                stateId, 
                                this.PhoneField.Text, 
                                this.FaxField.Text, 
                                this.PasswordField.Text, 
                                this.EmailField.Text, 
                                this.SendNewsletter.Checked, 
                                this.PortalSettings.PortalAlias);
                        }

                        // If we are here no error occurred
                    }
                }
                catch (Exception ex)
                {
                    this.Message.Text = General.GetString("REGISTRATION_FAILED", "Registration failed", this.Message) +
                                        " - ";

                    if (ex is SqlException)
                    {
                        if (((SqlException)ex).Number == 2627)
                        {
                            this.Message.Text = General.GetString(
                                "REGISTRATION_FAILED_EXISTING_EMAIL_ADDRESS", 
                                "Registration has failed. This email address has already been registered. Please use a different email address or use the 'Send Password' button on the login page.", 
                                this.Message);
                        }
                    }

                    ErrorHandler.Publish(LogLevel.Error, "Error registering user", ex);
                }
            }

            return returnId;
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Handles the Click event of the cancelButton control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        protected void CancelButtonClick(object sender, EventArgs e)
        {
            this.Page.RedirectBackToReferringPage();
        }

        /// <summary>
        /// Handles the ServerValidate event of the CheckTermsValidator control.
        /// </summary>
        /// <param name="source">
        /// The source of the event.
        /// </param>
        /// <param name="args">
        /// The <see cref="System.Web.UI.WebControls.ServerValidateEventArgs"/> instance containing the event data.
        /// </param>
        protected void CheckTermsValidatorServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = this.Accept.Checked;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the CountryField control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        protected void CountryFieldSelectedIndexChanged(object sender, EventArgs e)
        {
            this.BindState();
        }

        /// <summary>
        /// Raises OnInit event.
        /// </summary>
        /// <param name="e">
        /// Event arguments.
        /// </param>
        protected override void OnInit(EventArgs e)
        {
            this.CountryField.SelectedIndexChanged += this.CountryFieldSelectedIndexChanged;
            this.CheckTermsValidator.ServerValidate += this.CheckTermsValidatorServerValidate;
            this.RegisterBtn.Click += this.RegisterBtnClick;
            this.SaveChangesBtn.Click += this.SaveChangesBtnClick;
            this.cancelButton.Click += this.CancelButtonClick;

            // Remove validation for Windows users
            if (HttpContext.Current != null && this.Context.User is WindowsPrincipal)
            {
                this.ValidEmail.Visible = false;
                this.EmailLabel.TextKey = "WINDOWS_USER_NAME";
                this.EmailLabel.Text = "Windows User Name";
            }

            // TODO: Jonathan - need to bring in country functionality from esperantus or new somehow?
            this.BindCountry();

            // TODO: Fix this
            // More esperanuts country stuff...
            // CountryInfo country = CountryInfo.CurrentCountry;
            // if (country != null && CountryField.Items.FindByValue(country.Name) != null)
            // CountryField.Items.FindByValue(country.Name).Selected = true;
            this.BindState();

            var termsOfService = this.PortalSettings.GetTermsOfService;

            // Verify if we have to show conditions
            if (termsOfService.Length != 0)
            {
                // Shows conditions
                this.FieldConditions.Text = termsOfService;
                this.ConditionsRow.Visible = true;
            }
            else
            {
                // Hides conditions
                this.ConditionsRow.Visible = false;
            }

            base.OnInit(e);
        }

        /// <summary>
        /// The on load.
        /// </summary>
        /// <param name="e">
        /// Event arguments.
        /// </param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!this.Page.IsPostBack)
            {
                // Edit check
                if (this.EditMode)
                {
                    // Someone requested edit this record
                    // True is use is editing himself, false if is edited by an admin
                    this.SelfEdit = this.UserName == PortalSettings.CurrentUser.Identity.UserName;

                    // Removed by Mario Endara <mario@softworks.com.uy> (2004/11/04)
                    // if (PortalSecurity.IsInRoles("Admins") || selfEdit)
                    if (PortalSecurity.HasEditPermissions(this.ModuleID) ||
                        PortalSecurity.HasAddPermissions(this.ModuleID) || this.SelfEdit)
                    {
                        // We can edit

                        // Hide
                        this.RequiredPassword.Visible = false;
                        this.RequiredConfirm.Visible = false;
                        this.EditPasswordRow.Visible = true;
                        this.SaveChangesBtn.Visible = true;
                        this.RegisterBtn.Visible = false;

                        // Obtain a single row of event information
                        var accountSystem = new UsersDB();

                        var memberUser = accountSystem.GetSingleUser(this.UserName, this.PortalSettings.PortalAlias);

                        try
                        {
                            this.NameField.Text = memberUser.Name;
                            this.EmailField.Text = memberUser.Email;
                            this.CompanyField.Text = memberUser.Company;
                            this.AddressField.Text = memberUser.Address;
                            this.ZipField.Text = memberUser.Zip;
                            this.CityField.Text = memberUser.City;

                            this.CountryField.ClearSelection();
                            if (this.CountryField.Items.FindByValue(memberUser.CountryID) != null)
                            {
                                this.CountryField.Items.FindByValue(memberUser.CountryID).Selected = true;
                            }

                            this.BindState();
                            this.StateField.ClearSelection();
                            if (this.StateField.Items.Count > 0 &&
                                this.StateField.Items.FindByValue(memberUser.StateID.ToString()) != null)
                            {
                                this.StateField.Items.FindByValue(memberUser.StateID.ToString()).Selected = true;
                            }

                            this.FaxField.Text = memberUser.Fax;
                            this.PhoneField.Text = memberUser.Phone;
                            this.SendNewsletter.Checked = memberUser.SendNewsletter;

                            // stores original password for later check
                            this.OriginalPassword = memberUser.GetPassword();
                            this.OriginalUserId = memberUser.ProviderUserKey;
                        }
                        catch (ArgumentNullException)
                        {
                            // user doesn't exist
                        }
                    }
                    else
                    {
                        // We do not have rights to do it!
                        PortalSecurity.AccessDeniedEdit();
                    }
                }
                else
                {
                    this.BindState();

                    // No edit
                    this.RequiredPassword.Visible = true;
                    this.RequiredConfirm.Visible = true;
                    this.EditPasswordRow.Visible = false;
                    this.SaveChangesBtn.Visible = false;
                    this.RegisterBtn.Visible = true;
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the RegisterBtn control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        protected void RegisterBtnClick(object sender, EventArgs e)
        {
            var returnId = this.SaveUserData();

            if (returnId == Guid.Empty)
            {
                return;
            }

            if (this.PortalSettings.CustomSettings["SITESETTINGS_ON_REGISTER_SEND_TO"].ToString().Length > 0)
            {
                this.SendRegistrationNoticeToAdmin();
            }

            // Full signon
            PortalSecurity.SignOn(this.EmailField.Text, this.PasswordField.Text, false, this.RedirectPage);
        }

        /// <summary>
        /// Handles the Click event of the SaveChangesBtn control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        protected void SaveChangesBtnClick(object sender, EventArgs e)
        {
            this.Page.Validate();
            if (this.Page.IsValid)
            {
                var returnId = this.SaveUserData();

                if (returnId == Guid.Empty)
                {
                    if (this.SelfEdit)
                    {
                        // All should be ok now
                        // Try logoff user
                        PortalSecurity.SignOut(string.Empty, true);

                        // Logon user again with new settings
                        var actualPassword = this.PasswordField.Text.Length != 0
                                                 ? this.PasswordField.Text
                                                 : this.OriginalPassword;

                        // Full signon
                        PortalSecurity.SignOn(this.EmailField.Text, actualPassword, false, this.RedirectPage);
                    }
                    else if (this.RedirectPage == string.Empty)
                    {
                        // Redirect browser back to home page
                        PortalSecurity.PortalHome();
                    }
                    else
                    {
                        this.Response.Redirect(this.RedirectPage);
                    }
                }
            }
        }

        /// <summary>
        /// The bind country.
        /// </summary>
        private void BindCountry()
        {
            this.CountryField.DataSource = GeographicProvider.Current.GetCountries(CountryFields.Name);
            this.CountryField.DataBind();
        }

        /// <summary>
        /// The bind state.
        /// </summary>
        private void BindState()
        {
            this.StateRow.Visible = false;
            if (this.CountryField.SelectedItem != null)
            {
                var selectedCountry = GeographicProvider.Current.GetCountry(this.CountryField.SelectedValue);

                // added next line to clear the list. 
                // The stateField seems to remember it's values even when you set the 
                // DataSource to null
                // Michel Barneveld Appleseed@MichelBarneveld.Com
                this.StateField.Items.Clear();
                this.StateField.DataSource = GeographicProvider.Current.GetCountryStates(selectedCountry.CountryID);
                this.StateField.DataBind();

                this.StateLabel.Text = selectedCountry.AdministrativeDivisionName;

                if (this.StateField.Items.Count > 0)
                {
                    this.StateRow.Visible = true;
                    this.ThisCountryLabel.Text = this.CountryField.SelectedItem.Text;
                }
                else
                {
                    this.StateRow.Visible = false;
                }
            }
        }

        #endregion
    }
}