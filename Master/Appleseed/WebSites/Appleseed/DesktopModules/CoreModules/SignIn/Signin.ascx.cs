// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Signin.ascx.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The SignIn User Control enables clients to authenticate themselves using
//   the ASP.NET Forms based authentication system.
//   When a client enters their username/password within the appropriate
//   textboxes and clicks the "Login" button, the LoginBtn_Click event
//   handler executes on the server and attempts to validate their
//   credentials against a SQL database.
//   If the password check succeeds, then the LoginBtn_Click event handler
//   sets the customers username in an encrypted cookieID and redirects
//   back to the portal home page.
//   If the password check fails, then an appropriate error message
//   is displayed.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.DesktopModules.CoreModules.SignIn
{
    using System;
    using System.Data.SqlClient;
    using System.Text;
    using System.Web.Security;
    using System.Web.UI.WebControls;

    using Appleseed.Framework;
    using Appleseed.Framework.Content.Security;
    using Appleseed.Framework.DataTypes;
    using Appleseed.Framework.Helpers;
    using Appleseed.Framework.Providers.AppleseedMembershipProvider;
    using Appleseed.Framework.Security;
    using Appleseed.Framework.Settings;
    using Appleseed.Framework.Users.Data;
    using Appleseed.Framework.Web.UI.WebControls;

    using Resources;
    using System.Web.Profile;
    using System.Data;
    using System.Net.Mail;

    using Localize = Appleseed.Framework.Web.UI.WebControls.Localize;
    using System.Web;
    using System.Configuration;

    /// <summary>
    /// The SignIn User Control enables clients to authenticate themselves using
    ///   the ASP.NET Forms based authentication system.
    ///   When a client enters their username/password within the appropriate
    ///   textboxes and clicks the "Login" button, the LoginBtn_Click event
    ///  handler executes on the server and attempts to validate their
    ///  credentials against a SQL database.
    ///  If the password check succeeds, then the LoginBtn_Click event handler
    ///  sets the customers username in an encrypted cookieID and redirects
    ///  back to the portal home page.
    ///   If the password check fails, then an appropriate error message
    ///   is displayed.
    /// </summary>
    public partial class Signin : PortalModuleControl
    {
        #region Constants and Fields

        /// <summary>
        /// The login title.
        /// </summary>
        protected Localize LoginTitle;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "Signin" /> class.
        /// </summary>
        public Signin()
        {
            var hideAutomatically = new SettingItem<bool, CheckBox>()
                {
                    Value = true,
                    EnglishName = "Hide automatically",
                    Order = 20
                };
            this.BaseSettings.Add("SIGNIN_AUTOMATICALLYHIDE", hideAutomatically);

            // 1.2.8.1743b - 09/10/2003
            // New setting on Sign-in to disable IE auto-complete by Mike Stone
            // If you uncheck this setting IE will not remember user name and passwords. 
            // Note that users who have memorized passwords will not be effected until their computer 
            // is reset, only new users and/or computers will honor this. 
            var autoComplete = new SettingItem<bool, CheckBox>()
                {
                    Value = true,
                    EnglishName = "Allow IE Autocomplete",
                    Description = "If Checked IE Will try to remember logins",
                    Order = 30
                };
            this.BaseSettings.Add("SIGNIN_ALLOW_AUTOCOMPLETE", autoComplete);

            var rememberLogin = new SettingItem<bool, CheckBox>()
                {
                    Value = true,
                    EnglishName = "Allow Remember Login",
                    Description = "If Checked allows to remember logins",
                    Order = 40
                };
            this.BaseSettings.Add("SIGNIN_ALLOW_REMEMBER_LOGIN", rememberLogin);

            var sendPassword = new SettingItem<bool, CheckBox>()
                {
                    Value = true,
                    EnglishName = "Allow Send Password",
                    Description = "If Checked allows user to ask to get password by email if he forgotten",
                    Order = 50
                };
            this.BaseSettings.Add("SIGNIN_ALLOW_SEND_PASSWORD", sendPassword);
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Overrides ModuleSetting to render this module type un-cacheable
        /// </summary>
        /// <value><c>true</c> if cacheable; otherwise, <c>false</c>.</value>
        /// <remarks>
        /// </remarks>
        public override bool Cacheable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        ///   GUID of module (mandatory)
        /// </summary>
        /// <value></value>
        public override Guid GuidID
        {
            get
            {
                return new Guid("{A0F1F62B-FDC7-4de5-BBAD-A5DAF31D960A}");
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">
        /// An <see cref="T:System.EventArgs"/> object that contains the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected override void OnInit(EventArgs e)
        {
            // use "View = Unauthenticated Users" instead
            // //Hide control if not needed
            // if (Request.IsAuthenticated)
            //     this.Visible = false;

            if (Request.Cookies["userid"] != null)
                email.Text = Request.Cookies["email"].Value;
            if (Request.Cookies["pwd"] != null)
                password.Attributes.Add("value", Request.Cookies["pwd"].Value);
            if (Request.Cookies["userid"] != null && Request.Cookies["pwd"] != null)
                RememberCheckBox.Checked = true;

            this.LoginBtn.Click += this.LoginBtnClick;
            //this.SendPasswordBtn.Click += this.SendPasswordBtnClick;
            this.RegisterBtn.Click += this.RegisterBtnClick;
            this.Load += this.SigninLoad;
            this.SendPasswordBtn.Click += SendPasswordBtnClickLink;

            base.OnInit(e);
        }

        /// <summary>
        /// Handles the Click event of the LoginBtn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void LoginBtnClick(object sender, EventArgs e)
        {
            this.Session["PersistedUser"] = this.RememberCheckBox.Checked;

            int timeout = Convert.ToInt32(ConfigurationManager.AppSettings["RememberMe.Session.TimeOut"]);
            //HttpCookie myCookie = new HttpCookie("MyTestCookie");
            //DateTime now = DateTime.Now;
            if (RememberCheckBox.Checked)
            {
                Response.Cookies["email"].Value = email.Text;
                Response.Cookies["password"].Value = password.Text;
                Response.Cookies["email"].Expires = DateTime.MaxValue;
                Response.Cookies["password"].Expires = DateTime.MaxValue;
            }
            else
            {
                Response.Cookies["email"].Expires = DateTime.Now.AddDays(timeout);
                Response.Cookies["password"].Expires = DateTime.Now.AddDays(timeout);
            }


            if (PortalSecurity.SignOn(this.email.Text.Trim(), this.password.Text, this.RememberCheckBox.Checked) == null)
            {
                this.Message.Text = Appleseed.Signin_LoginBtnClick_Login_failed;
                this.Message.TextKey = "LOGIN_FAILED";
            }
        }

        /// <summary>
        /// Handles the Click event of the RegisterBtn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void RegisterBtnClick(object sender, EventArgs e)
        {
            this.Response.Redirect(HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/Register/Register.aspx"));
        }

        private void SendPasswordBtnClickLink(object sender, EventArgs e)
        {
            var url = HttpUrlBuilder.BuildUrl("~/Password/ForgotPassword");

            if (!string.IsNullOrEmpty(this.email.Text))
            {
                url += "?email=" + this.email.Text;
            }

            this.Response.Redirect(url);
        }

        /// <summary>
        /// Handles the Click event of the SendPasswordBtn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void SendPasswordBtnClick(object sender, EventArgs e)
        {
            if (this.email.Text == string.Empty)
            {
                this.Message.Text = Appleseed.Signin_SendPasswordBtnClick_Please_enter_you_email_address;
                this.Message.TextKey = "SIGNIN_ENTER_EMAIL_ADDR";
                return;
            }

            Membership.ApplicationName = this.PortalSettings.PortalAlias;
            var membership = (AppleseedMembershipProvider)Membership.Provider;

            // Obtain single row of User information
            var memberUser = membership.GetUser(this.email.Text, false);

            if (memberUser == null)
            {
                this.Message.Text = General.GetString(
                    "SIGNIN_PWD_MISSING_IN_DB", "The email you specified does not exists on our database", this);
                this.Message.TextKey = "SIGNIN_PWD_MISSING_IN_DB";
                return;
            }

            var userId = (Guid)(memberUser.ProviderUserKey ?? Guid.Empty);

            // generate Token for user
            var token = membership.CreateResetPasswordToken(userId);

            var changePasswordUrl = string.Concat(
                Path.ApplicationFullPath,
                "DesktopModules/CoreModules/Admin/ChangePassword.aspx?usr=",
                userId.ToString("N"),
                "&tok=",
                token.ToString("N"));

            var mail = new MailMessage();

            // we check the PortalSettings in order to get if it has an sender registered 
            if (this.PortalSettings.CustomSettings["SITESETTINGS_ON_REGISTER_SEND_FROM"] != null)
            {
                var sf = this.PortalSettings.CustomSettings["SITESETTINGS_ON_REGISTER_SEND_FROM"];
                var mailFrom = sf.ToString();
                try
                {
                    mail.From = new MailAddress(mailFrom);
                }
                catch
                {
                    // if the address is not well formed, a warning is logged.
                    LogHelper.Logger.Log(
                        LogLevel.Warn,
                        string.Format(
                            @"This is the current email address used as sender when someone want to retrieve his/her password: '{0}'. 
Is not well formed. Check the setting SITESETTINGS_ON_REGISTER_SEND_FROM of portal '{1}' in order to change this value (it's a portal setting).",
                            mailFrom,
                            this.PortalSettings.PortalAlias));
                }
            }

            // if there is not a correct email in the portalSettings, we use the default sender specified on the web.config file in the mailSettings tag.
            mail.To.Add(new MailAddress(this.email.Text));
            mail.Subject = string.Format(
                "{0} - {1}",
                this.PortalSettings.PortalName,
                General.GetString("SIGNIN_PWD_LOST", "I lost my password", this));

            var sb = new StringBuilder();

            sb.Append(memberUser.UserName);
            sb.Append(",");
            sb.Append("\r\n\r\n");
            sb.Append(
                General.GetString(
                    "SIGNIN_PWD_LOST_REQUEST_RECEIVED",
                    "We received your request regarding the loss of your password.",
                    this));
            sb.Append("\r\n");
            sb.Append(
                General.GetString(
                    "SIGNIN_SET_NEW_PWD_MSG",
                    "You can set a new password for your account going to the following link:",
                    this));
            sb.Append(" ");
            sb.Append(changePasswordUrl);
            sb.Append("\r\n\r\n");
            sb.Append(General.GetString("SIGNIN_THANK_YOU", "Thanks for your visit.", this));
            sb.Append(" ");
            sb.Append(this.PortalSettings.PortalName);
            sb.Append("\r\n\r\n");
            sb.Append(
                General.GetString(
                    "SIGNIN_URL_WARNING",
                    "NOTE: The address above may not show up on your screen as one line. This would prevent you from using the link to access the web page. If this happens, just use the 'cut' and 'paste' options to join the pieces of the URL.",
                    this));

            mail.Body = sb.ToString();
            mail.IsBodyHtml = false;

            using (var client = new SmtpClient())
            {
                try
                {
                    client.Send(mail);

                    this.Message.Text = General.GetString(
                        "SIGNIN_PWD_WAS_SENT", "Your password was sent to the address you provided", this);
                    this.Message.TextKey = "SIGNIN_PWD_WAS_SENT";
                }
                catch (Exception exception)
                {
                    this.Message.Text = General.GetString(
                        "SIGNIN_SMTP_SENDING_PWD_MAIL_ERROR",
                        "We can't send you your password. There were problems while trying to do so.");
                    this.Message.TextKey = "SIGNIN_SMTP_SENDING_PWD_MAIL_ERROR";
                    LogHelper.Logger.Log(
                        LogLevel.Error,
                        string.Format(
                            "Error while trying to send the password to '{0}'. Perhaps you should check your SMTP server configuration in the web.config.",
                            this.email.Text),
                        exception);
                }
            }
        }

        /// <summary>
        /// Handles the Load event of the Sign in control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void SigninLoad(object sender, EventArgs e)
        {
            var hide = true;
            var autocomplete = false;
            if (this.ModuleID == 0)
            {
                ((SettingItem<bool, CheckBox>)this.Settings["MODULESETTINGS_SHOW_TITLE"]).Value = false;
            }

            if (this.Settings.ContainsKey("SITESETTINGS_ALLOW_NEW_REGISTRATION") && !this.Settings["SITESETTINGS_ALLOW_NEW_REGISTRATION"].ToString().Equals(string.Empty))
            //if (this.PortalSettings.CustomSettings["SITESETTINGS_ALLOW_NEW_REGISTRATION"] != null)
            {
                if (!bool.Parse(this.PortalSettings.CustomSettings["SITESETTINGS_ALLOW_NEW_REGISTRATION"].ToString()))
                {
                    this.RegisterBtn.Visible = false;
                }
            }

            if (this.Settings.ContainsKey("SIGNIN_AUTOMATICALLYHIDE") && !this.Settings["SIGNIN_AUTOMATICALLYHIDE"].ToString().Equals(string.Empty))
            //if (this.Settings["SIGNIN_AUTOMATICALLYHIDE"] != null)
            {
                hide = bool.Parse(this.Settings["SIGNIN_AUTOMATICALLYHIDE"].ToString());
            }

            if (this.Settings.ContainsKey("SIGNIN_ALLOW_AUTOCOMPLETE") && !this.Settings["SIGNIN_ALLOW_AUTOCOMPLETE"].ToString().Equals(string.Empty))
            //if (this.Settings["SIGNIN_ALLOW_AUTOCOMPLETE"] != null)
            {
                autocomplete = bool.Parse(this.Settings["SIGNIN_ALLOW_AUTOCOMPLETE"].ToString());
            }

            if (this.Settings.ContainsKey("SIGNIN_ALLOW_REMEMBER_LOGIN") && !this.Settings["SIGNIN_ALLOW_REMEMBER_LOGIN"].ToString().Equals(string.Empty))
            //if (this.Settings["SIGNIN_ALLOW_REMEMBER_LOGIN"] != null)
            {
                this.RememberCheckBox.Visible = bool.Parse(this.Settings["SIGNIN_ALLOW_REMEMBER_LOGIN"].ToString());
            }

            if (this.Settings.ContainsKey("SIGNIN_ALLOW_SEND_PASSWORD") && !this.Settings["SIGNIN_ALLOW_SEND_PASSWORD"].ToString().Equals(string.Empty))
            //if (this.Settings["SIGNIN_ALLOW_SEND_PASSWORD"] != null)
            {
                this.SendPasswordBtn.Visible = bool.Parse(this.Settings["SIGNIN_ALLOW_SEND_PASSWORD"].ToString());
            }

            if (hide && this.Request.IsAuthenticated)
            {
                this.Visible = false;
            }
            else if (!autocomplete)
            {
                // New setting on Signin fo disable IE autocomplete by Mike Stone
                this.password.Attributes.Add("autocomplete", "off");
            }
        }

        #endregion
    }
}