using System;
using System.Data.SqlClient;
using System.Text;
//using System.Web.Mail;
using System.Web.UI;
using Appleseed.Framework;
using Appleseed.Framework.Content.Security;
using Appleseed.Framework.DataTypes;
using Appleseed.Framework.Helpers;
using Appleseed.Framework.Security;
using Appleseed.Framework.Settings;
using Appleseed.Framework.Users.Data;
using Appleseed.Framework.Web.UI.WebControls;
using Appleseed.Framework.Providers.AppleseedMembershipProvider;

namespace Appleseed.Content.Web.Modules
{
    using System.Web.UI.WebControls;

    using Localize = Appleseed.Framework.Web.UI.WebControls.Localize;
    using System.Net.Mail;

    /// <summary>
    /// The SignIn User Control enables clients to authenticate themselves using 
    /// the ASP.NET Forms based authentication system.
    ///
    /// When a client enters their username/password within the appropriate
    /// textboxes and clicks the "Login" button, the LoginBtn_Click event
    /// handler executes on the server and attempts to validate their
    /// credentials against a SQL database.
    ///
    /// If the password check succeeds, then the LoginBtn_Click event handler
    /// sets the customers username in an encrypted cookieID and redirects
    /// back to the portal home page.
    /// 
    /// If the password check fails, then an appropriate error message
    /// is displayed.
    /// This cool version is placed orizontally and allows a descriptive text on the right
    /// </summary>
    public partial class SigninCool : PortalModuleControl
    {
        #region Controls

        /// <summary>
        /// 
        /// </summary>
        protected Localize LoginTitle;

        #endregion

        /// <summary>
        /// Handles the Click event of the LoginBtn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void LoginBtn_Click(Object sender, EventArgs e)
        {
            if (PortalSecurity.SignOn(email.Text, password.Text, RememberCheckBox.Checked) == null)
            {
                Message.Text = "Login failed";
                Message.TextKey = "LOGIN_FAILED";
            }
        }

        /// <summary>
        /// Handles the Click event of the RegisterBtn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void RegisterBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect(HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/Register/Register.aspx"));
        }

        private void SendPasswordBtnClickLink(object sender, EventArgs e) {
            var url = HttpUrlBuilder.BuildUrl("~/Password/ForgotPassword");

            if (!string.IsNullOrEmpty(this.email.Text)) {
                url += "?email=" + this.email.Text;
            }

            this.Response.Redirect(url);
        }

        /// <summary>
        /// Handles the Click event of the SendPasswordBtn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void SendPasswordBtn_Click( object sender, EventArgs e ) {
            if ( email.Text == string.Empty ) {
                Message.Text = "Please enter you email address";
                Message.TextKey = "SIGNIN_ENTER_EMAIL_ADDR";
                return;
            }
            // generate random password
            string randomPassword = RandomPassword.Generate( 8, 10 );

            CryptoHelper crypthelp = new CryptoHelper();
            UsersDB usersDB = new UsersDB();

            //Obtain single row of User information
            AppleseedUser user = usersDB.GetSingleUser( email.Text, this.PortalSettings.PortalAlias );

            if ( user != null ) {

                string Pswrd;
                string AppName = this.PortalSettings.PortalName;
                bool encrypted = Config.EncryptPassword;
                string Name = user.Email;
                if ( encrypted ) {
                    Pswrd = randomPassword;
                    crypthelp.ResetPassword( Name, randomPassword );
                }
                else {
                    Pswrd = user.GetPassword();
                }
                crypthelp.ResetPassword( Name, randomPassword );
                string LoginUrl = Path.ApplicationFullPath + "DesktopModules/Admin/Logon.aspx?Usr=" + Name + "&Pwd=" +
                                  Pswrd + "&Alias=" + this.PortalSettings.PortalAlias;
               // MailMessage mail = new MailMessage();

                // Geert.Audenaert@Syntegra.Com
                // Date 19 March 2003
                // We have to use a correct sender address, 
                // because most SMTP servers reject it otherwise
                //jes1111 - mail.From = ConfigurationSettings.AppSettings["EmailFrom"].ToString();
                //mail.From = Config.EmailFrom;
                //mail.To = email.Text;
                //mail.Subject = AppName + " - " + General.GetString( "SIGNIN_SEND_PWD", "Send me password", this );

                StringBuilder sb = new StringBuilder();

                sb.Append( Name );
                sb.Append( "," );
                sb.Append( "\r\n\r\n" );
                sb.Append( General.GetString( "SIGNIN_PWD_REQUESTED", "This is the password you requested", this ) );
                sb.Append( " " );
                sb.Append( Pswrd );
                sb.Append( "\r\n\r\n" );
                sb.Append( General.GetString( "SIGNIN_THANK_YOU", "Thanks for your visit.", this ) );
                sb.Append( " " );
                sb.Append( AppName );
                sb.Append( "\r\n\r\n" );
                sb.Append( General.GetString( "SIGNIN_YOU_CAN_LOGIN_FROM", "You can login from", this ) );
                sb.Append( ":" );
                sb.Append( "\r\n" );
                sb.Append( Path.ApplicationFullPath );
                sb.Append( "\r\n\r\n" );
                sb.Append( General.GetString( "SIGNIN_USE_DIRECT_URL", "Or using direct url", this ) );
                sb.Append( "\r\n" );
                sb.Append( LoginUrl );
                sb.Append( "\r\n\r\n" );
                sb.Append(
                    General.GetString( "SIGNIN_URL_WARNING",
                                      "NOTE: The address above may not show up on your screen as one line. This would prevent you from using the link to access the web page. If this happens, just use the 'cut' and 'paste' options to join the pieces of the URL.",
                                      this ) );

                //mail.Body = sb.ToString();
                //mail.BodyFormat = MailFormat.Text;

                //SmtpMail.SmtpServer = Config.SmtpServer;
                //SmtpMail.Send( mail );


                string subject = AppName + " - " + General.GetString("SIGNIN_SEND_PWD", "Send me password", this);
                MailMessage newmail = new MailMessage(Config.EmailFrom.ToString(),email.Text,subject,sb.ToString());
                SmtpClient client = new SmtpClient(Config.SmtpServer);
                client.Send(newmail);

                Message.Text =
                    General.GetString( "SIGNIN_PWD_WAS_SENT", "Your password was sent to the addess you provided",
                                      this );
                Message.TextKey = "SIGNIN_PWD_WAS_SENT";
            }
            else {
                Message.Text =
                    General.GetString( "SIGNIN_PWD_MISSING_IN_DB",
                                      "The email you specified does not exists on our database", this );
                Message.TextKey = "SIGNIN_PWD_MISSING_IN_DB";
            }
        }

        /// <summary>
        /// Overrides ModuleSetting to render this module type un-cacheable
        /// </summary>
        /// <value></value>
        public override bool Cacheable
        {
            get { return false; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SigninCool"/> class.
        /// </summary>
        public SigninCool()
        {
            var coolText = new SettingItem<string, TextBox> { Order = 10 };
            this.BaseSettings.Add("CoolText", coolText);

            var hideAutomatically = new SettingItem<bool, CheckBox>
                {
                    Value = true, EnglishName = "Hide automatically", Order = 20 
                };
            this.BaseSettings.Add("SIGNIN_AUTOMATICALLYHIDE", hideAutomatically);

            //1.2.8.1743b - 09/10/2003
            //New setting on Signin fo disable IE autocomplete by Mike Stone
            //If you uncheck this setting IE will not remember user name and passwords. 
            //Note that users who have memorized passwords will not be effected until their computer 
            //is reset, only new users and/or computers will honor this. 
            var autoComplete = new SettingItem<bool, CheckBox>
                {
                    Value = true,
                    EnglishName = "Allow IE Autocomplete",
                    Description = "If Checked IE Will try to remember logins",
                    Order = 30
                };
            this.BaseSettings.Add("SIGNIN_ALLOW_AUTOCOMPLETE", autoComplete);

            var rememberLogin = new SettingItem<bool, CheckBox>
                {
                    Value = true,
                    EnglishName = "Allow Remember Login",
                    Description = "If Checked allows to remember logins",
                    Order = 40
                };
            this.BaseSettings.Add("SIGNIN_ALLOW_REMEMBER_LOGIN", rememberLogin);

            var sendPassword = new SettingItem<bool, CheckBox>
                {
                    Value = true,
                    EnglishName = "Allow Send Password",
                    Description = "If Checked allows user to ask to get password by email if he forgotten",
                    Order = 50
                };
            this.BaseSettings.Add("SIGNIN_ALLOW_SEND_PASSWORD", sendPassword);
        }

        #region General Implementation

        /// <summary>
        /// GUID of module (mandatory)
        /// </summary>
        /// <value></value>
        public override Guid GuidID
        {
            get { return new Guid("{99F3511F-737C-4b57-87C0-9A010AF40A9C}"); }
        }

        #endregion

        #region Web Form Designer generated code

        /// <summary>
        /// On init
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            this.LoginBtn.Click += new EventHandler(this.LoginBtn_Click);
            this.SendPasswordBtn.Click += new EventHandler(this.SendPasswordBtnClickLink);
            this.RegisterBtn.Click += new EventHandler(this.RegisterBtn_Click);
            this.Load += new EventHandler(this.Signin_Load);
            base.OnInit(e);
        }

        #endregion

        /// <summary>
        /// Handles the Load event of the Signin control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Signin_Load(object sender, EventArgs e)
        {
            bool hide = true;
            bool autocomplete = false;

            if (this.PortalSettings.CustomSettings.ContainsKey("SITESETTINGS_ALLOW_NEW_REGISTRATION") &&
                !(string.IsNullOrEmpty(this.PortalSettings.CustomSettings.ContainsKey("SITESETTINGS_ALLOW_NEW_REGISTRATION").ToString())))
                if (!bool.Parse(this.PortalSettings.CustomSettings["SITESETTINGS_ALLOW_NEW_REGISTRATION"].ToString()))
                    RegisterBtn.Visible = false;

            if (this.PortalSettings.CustomSettings.ContainsKey("SIGNIN_AUTOMATICALLYHIDE") &&
                !(string.IsNullOrEmpty(this.PortalSettings.CustomSettings.ContainsKey("SIGNIN_AUTOMATICALLYHIDE").ToString())))
                hide = bool.Parse(Settings["SIGNIN_AUTOMATICALLYHIDE"].ToString());

            if (this.PortalSettings.CustomSettings.ContainsKey("SIGNIN_ALLOW_AUTOCOMPLETE") &&
                !(string.IsNullOrEmpty(this.PortalSettings.CustomSettings.ContainsKey("SIGNIN_ALLOW_AUTOCOMPLETE").ToString())))
                autocomplete = bool.Parse(Settings["SIGNIN_ALLOW_AUTOCOMPLETE"].ToString());

            if (this.PortalSettings.CustomSettings.ContainsKey("SIGNIN_ALLOW_REMEMBER_LOGIN") &&
                !(string.IsNullOrEmpty(this.PortalSettings.CustomSettings.ContainsKey("SIGNIN_ALLOW_REMEMBER_LOGIN").ToString())))
                RememberCheckBox.Visible = bool.Parse(Settings["SIGNIN_ALLOW_REMEMBER_LOGIN"].ToString());

            if (this.PortalSettings.CustomSettings.ContainsKey("SIGNIN_ALLOW_SEND_PASSWORD") &&
                !(string.IsNullOrEmpty(this.PortalSettings.CustomSettings.ContainsKey("SIGNIN_ALLOW_SEND_PASSWORD").ToString())))
                SendPasswordBtn.Visible = bool.Parse(Settings["SIGNIN_ALLOW_SEND_PASSWORD"].ToString());

            if (hide && Request.IsAuthenticated)
            {
                this.Visible = false;
            }
            else if (!autocomplete)
            {
                //New setting on Signin fo disable IE autocomplete by Mike Stone
                password.Attributes.Add("autocomplete", "off");
            }
            // Update cool text
            if(Settings.ContainsKey("CoolText"))
                CoolTextPlaceholder.Controls.Add(new LiteralControl(Settings["CoolText"].ToString()));
        }
    }
}