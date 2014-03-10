using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mail;
using System.Web.UI.WebControls;
using Appleseed.Framework;
using Appleseed.Framework.Content.Data;
using Appleseed.Framework.Data;
using Appleseed.Framework.DataTypes;
using Appleseed.Framework.Settings;
using Appleseed.Framework.Site.Configuration;
using Appleseed.Framework.Web.UI.WebControls;

namespace Appleseed.Content.Web.Modules
{
    using System.Collections.Generic;

    /// <summary>
    ///	Summary description for SendNewsletter.
    /// </summary>
    public partial class SendNewsletter : PortalModuleControl
    {
        private string InvalidRecipients = string.Empty;

        protected string ServerVariables;
        protected string Titulo;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            // Added EsperantusKeys for Localization 
            // Mario Endara mario@softworks.com.uy 11/05/2004 
            Titulo = General.GetString("NEWSLETTER_TITLE");

            if (!Page.IsPostBack)
            {
                CreatedDate.Text = DateTime.Now.ToLongDateString();

                //Set default
                txtName.Text = Settings["NEWSLETTER_DEFAULTNAME"].ToString();
                txtEMail.Text = Settings["NEWSLETTER_DEFAULTEMAIL"].ToString();
                if (txtEMail.Text == string.Empty)
                    txtEMail.Text = PortalSettings.CurrentUser.Identity.Email;

                //create a DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("StringValue"));

                NewsletterDB newsletter = new NewsletterDB();

                if (!bool.Parse(Settings["TestMode"].ToString()))
                {
                    var users = newsletter.GetUsersNewsletter(this.PortalSettings.PortalID);
                    foreach (dynamic user in users)
                    {
                        DataRow dr = dt.NewRow();
                        dr[0] = "<b>" + user.Name + ":</b> " + user.Email;
                    }
                    
                    DataList1.DataSource = new DataView(dt);
                    DataList1.RepeatDirection = RepeatDirection.Vertical;
                    DataList1.RepeatLayout = RepeatLayout.Table;
                    DataList1.BorderWidth = Unit.Pixel(1);
                    DataList1.GridLines = GridLines.Both;
                    DataList1.RepeatColumns = 3;
                    DataList1.DataBind();
                    DataList1.Visible = true;

                    int cnt = users.Count;

                    // Added EsperantusKeys for Localization 
                    // Mario Endara mario@softworks.com.uy 11/05/2004 
                    lblMessage.Text = General.GetString("NEWSLETTER_MSG").Replace("{1}", cnt.ToString());
                }
                else
                {
                    // Added EsperantusKeys for Localization 
                    // Mario Endara mario@softworks.com.uy 11/05/2004 
                    lblMessage.Text =
                        General.GetString("NEWSLETTER_MSG_TEST").Replace("{1}", txtName.Text).Replace("{2}",
                                                                                                      txtEMail.Text);
                }

                //Try to get template
                int HTMLModID = int.Parse(Settings["NEWSLETTER_HTMLTEMPLATE"].ToString());
                if (HTMLModID > 0)
                {
                    // Obtain the selected item from the HtmlText table
                    NewsletterHtmlTextDB text = new NewsletterHtmlTextDB();
                    SqlDataReader dr = text.GetHtmlText(HTMLModID, WorkFlowVersion.Staging);
                    try
                    {
                        if (dr.Read())
                        {
                            string buffer = (string) dr["DesktopHtml"];
                            // Replace relative path to absolute path. jviladiu@portalServices.net 19/07/2004
                            buffer = buffer.Replace(Path.ApplicationFullPath, Path.ApplicationRoot);
                            if (Path.ApplicationRoot.Length > 0)
                                //by Manu... on root PortalSettings.ApplicationPath is empty
                                buffer = buffer.Replace(Path.ApplicationRoot, Path.ApplicationFullPath);

                            txtBody.Text = Server.HtmlDecode(buffer);
                            HtmlMode.Checked = true;
                        }
                        else
                            HtmlMode.Checked = false;
                    }
                    finally
                    {
                        dr.Close();
                    }
                }
            }
            EditPanel.Visible = true;
            PrewiewPanel.Visible = false;
            UsersPanel.Visible = true;
        }

        /// <summary>
        /// The CancelBtn_Click server event handler on this page is used
        /// to handle the scenario where a user clicks the "cancel"
        /// button to discard a message post and toggle out of edit mode.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            txtSubject.Text = string.Empty;
            txtBody.Text = string.Empty;
        }

        /// <summary>
        /// The SubmitBtn_Click server event handler on this page is used
        /// to handle the scenario where a user clicks the "send"
        /// button after entering a response to a message post.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void submitButton_Click(object sender, EventArgs e)
        {
            string message;
            int cnt = 0;
            EditPanel.Visible = false;
            PrewiewPanel.Visible = true;
            UsersPanel.Visible = false;

            message = General.GetString("NEWSLETTER_SENDTO", "<b>Message:</b> sent to:<br>");

            try
            {
                NewsletterDB newsletter = new NewsletterDB();
                if (!bool.Parse(Settings["TestMode"].ToString()))
                {
                    // Get Newsletter Users from DB
                    var users = newsletter.GetUsersNewsletter(this.PortalSettings.PortalID);
                    foreach (dynamic user in users) 
                    {
                        cnt++;
                        message += user.Email + ", ";
                        try
                        {
                            //Send the email
                            newsletter.SendMessage(txtEMail.Text, user.Email, user.Name, txtSubject.Text,
                                                    txtBody.Text, true, HtmlMode.Checked, InsertBreakLines.Checked);
                            // Here the systems used to notify the DB that an email was sent to the user. 
                            // Since we implemented the MembershipProvider for Appleseed, we doesn't have a place to store this info,
                            // thus, now the fact that an email was sent won't be persisted in the DB. However this should change in future releases.
                        }
                        catch (Exception ex)
                        {
                            InvalidRecipients += user.Email + "<br/>";
                            BlacklistDB.AddToBlackList(this.PortalSettings.PortalID, user.Email, ex.Message);
                        }
                    }
                    lblMessage.Text =
                        General.GetString("NEWSLETTER_SENDINGTO", "Message has been sent to {1} registered users.").
                            Replace("{1}", cnt.ToString());
                }
                else
                {
                    newsletter.SendMessage(txtEMail.Text, txtEMail.Text, txtName.Text, txtSubject.Text,
                                           txtBody.Text, true, HtmlMode.Checked, InsertBreakLines.Checked);
                    lblMessage.Text = General.GetString("NEWSLETTER_TESTSENDTO", "Test message sent to: ") +
                                      txtName.Text + " [" + txtEMail.Text + "]";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = General.GetString("NEWSLETTER_ERROR", "An error occurred: ") + ex.Message;
            }

            CreatedDate.Text = General.GetString("NEWSLETTER_SENDDATE", "Message sent: ") +
                               DateTime.Now.ToLongDateString() + "<br>";

            if (InvalidRecipients.Length > 0)
                CreatedDate.Text += General.GetString("NEWSLETTER_INVALID_RECIPIENTS", "Invalid recipients:<br>") +
                                    InvalidRecipients;

            //Hides commands
            submitButton.Visible = false;
            cancelButton2.Visible = false;
        }

        /// <summary>
        /// Admin Module
        /// </summary>
        /// <value></value>
        public override bool AdminModule
        {
            get { return true; }
        }

        /// <summary>
        /// GUID of module (mandatory)
        /// </summary>
        /// <value></value>
        public override Guid GuidID
        {
            get { return new Guid("{B484D450-5D30-4C4B-817C-14A25D06577E}"); }
        }

        #region Web Form Designer generated code

        /// <summary>
        /// Raises Init event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            this.previewButton.Click += new EventHandler(this.previewButton_Click);
            this.cancelButton.Click += new EventHandler(this.cancelButton_Click);
            this.submitButton.Click += new EventHandler(this.submitButton_Click);
            this.Load += new EventHandler(this.Page_Load);
            base.OnInit(e);
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="SendNewsletter"/> class.
        /// </summary>
        public SendNewsletter()
        {
            // modified by Hongwei Shen
            const SettingItemGroup Group = SettingItemGroup.MODULE_SPECIAL_SETTINGS;
            const int GroupBase = (int)Group;

            var defaultEmail = new SettingItem<string, TextBox>
                { EnglishName = "Sender Email", Group = Group, Order = GroupBase + 20 };
            this.BaseSettings.Add("NEWSLETTER_DEFAULTEMAIL", defaultEmail);

            var defaultName = new SettingItem<string, TextBox>
                { Group = Group, Order = GroupBase + 25, EnglishName = "Sender Name" };
            this.BaseSettings.Add("NEWSLETTER_DEFAULTNAME", defaultName);

            var testMode = new SettingItem<bool, CheckBox>
                {
                    Value = false,
                    Group = Group,
                    Order = GroupBase + 30,
                    EnglishName = "Test Mode",
                    Description =
                        "Use Test Mode for testing your settings. It will send only one email to the address specified as sender. Useful for testing"
                };
            this.BaseSettings.Add("TestMode", testMode);

            //SettingItem HTMLTemplate = new SettingItem(new CustomListDataType(new Appleseed.Framework.Site.Configuration.ModulesDB().GetModuleDefinitionByGuid(p, new Guid("{0B113F51-FEA3-499A-98E7-7B83C192FDBB}")), "ModuleTitle", "ModuleID"));
            var htmlTemplate = new SettingItem<string, ListControl>(new ModuleListDataType("Html Document"))
                {
                    Value = "0",
                    Group = Group,
                    Order = GroupBase + 35,
                    EnglishName = "HTML Template",
                    Description =
                        "Select an HTML module that the will be used as base for this newsletter sent with this module."
                };
            this.BaseSettings.Add("NEWSLETTER_HTMLTEMPLATE", htmlTemplate);

            var loginHomePage = new SettingItem<string, TextBox>
                {
                    EnglishName = "Site URL",
                    Group = Group,
                    Order = GroupBase + 40,
                    Description =
                        "The Url or the Home page of the site used for build the instant login url. Leave blank if using the current site."
                };
            this.BaseSettings.Add("NEWSLETTER_LOGINHOMEPAGE", loginHomePage);

            var doNotResendWithin = new SettingItem<string, ListControl>(new ListDataType<string, ListControl>("1;2;3;4;5;6;7;10;15;30;60;90"))
                {
                    Value = "7",
                    Group = Group,
                    Order = GroupBase + 45,
                    EnglishName = "Do not resend within (days)",
                    Description =
                        "To avoid spam and duplicate sent you cannot email an user more than one time in specifed number of days."
                };
            this.BaseSettings.Add("NEWSLETTER_DONOTRESENDWITHIN", doNotResendWithin);

            var userBlock = new SettingItem<string, ListControl>(new ListDataType<string, ListControl>("50;100;200;250;300;1000;1500;5000"))
                {
                    EnglishName = "Group users",
                    Group = Group,
                    Order = GroupBase + 50,
                    Description =
                        "Select the maximum number of users. For sending emails to all you have to repeat the process. Use small values to avoid server timeouts.",
                    Value = "250"
                };
            this.BaseSettings.Add("NEWSLETTER_USERBLOCK", userBlock);
        }

        /// <summary>
        /// The previewButton_Click server event handler on this page is used
        /// to handle the scenario where a user clicks the "preview"
        /// button to see a preview of the message.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void previewButton_Click(object sender, EventArgs e)
        {
            CreatedDate.Text = General.GetString("NEWSLETTER_LOCALTIME", "Local time: ") +
                               DateTime.Now.ToLongDateString();

            NewsletterDB newsletter = new NewsletterDB();

            string email;
            string name;

            if (!bool.Parse(Settings["TestMode"].ToString()))
            {
                var users = newsletter.GetUsersNewsletter(this.PortalSettings.PortalID);
                if (users.Count > 0)
                {
                    email = users[0].Email;
                    name = users[0].Name;
                }
                else
                {
                    lblMessage.Text = General.GetString("NEWSLETTER_NORECIPIENTS", "No recipients");
                    return; //nothing more to do here
                }
            }
            else
            {
                email = txtEMail.Text;
                name = txtName.Text;
            }

            EditPanel.Visible = false;
            PrewiewPanel.Visible = true;
            UsersPanel.Visible = false;
            lblFrom.Text = txtName.Text + " (" + txtEMail.Text + ")";
            lblTo.Text = name + " (" + email + ")";
            lblSubject.Text = txtSubject.Text;
            string body =
                newsletter.SendMessage(txtEMail.Text, email, name, txtSubject.Text, txtBody.Text,
                                       false, HtmlMode.Checked, InsertBreakLines.Checked);
            if (HtmlMode.Checked)
            {
                lblBody.Text = body;
            }
            else
            {
                lblBody.Text = "<PRE>" + body + "</PRE>";
            }
        }

        # region Install / Uninstall Implementation

        /// <summary>
        /// Unknown
        /// </summary>
        /// <param name="stateSaver"></param>
        public override void Install(IDictionary stateSaver)
        {
            string currentScriptName = Server.MapPath(this.TemplateSourceDirectory + "/Newsletter_Install.sql");
            List<string> errors = DBHelper.ExecuteScript(currentScriptName, true);
            if (errors.Count > 0)
            {
                // Call rollback
                throw new Exception("Error occurred:" + errors[0].ToString());
            }
        }

        /// <summary>
        /// Unknown
        /// </summary>
        /// <param name="stateSaver"></param>
        public override void Uninstall(IDictionary stateSaver)
        {
            string currentScriptName = Server.MapPath(this.TemplateSourceDirectory + "/Newsletter_Uninstall.sql");
            List<string> errors = DBHelper.ExecuteScript(currentScriptName, true);
            if (errors.Count > 0)
            {
                // Call rollback
                throw new Exception("Error occurred:" + errors[0].ToString());
            }
        }

        #endregion
    }
}