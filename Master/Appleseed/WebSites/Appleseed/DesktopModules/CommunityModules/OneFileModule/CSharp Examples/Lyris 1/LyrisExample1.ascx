<%@ Control Language="c#" inherits="Appleseed.Content.Web.Modules.OneFileModule" %>
<%@ Register TagPrefix="rbfwebui" Namespace="Appleseed.Framework.Web.UI.WebControls" assembly="Appleseed.Framework" %>
<%@ Import Namespace="System.Web.Mail" %>
<%@ Import Namespace="Appleseed.Framework.Web.UI" %>
<%@ Import Namespace="Appleseed.Framework.Web.UI.WebControls" %>
<%@ import namespace="Appleseed.Framework.Site.Configuration" %>

<script language="C#" runat="server">


    string lyrisListName;
    string lyrisServerName;


    void Page_Load(Object sender, EventArgs e)
    {
        InitSettings(SettingsType.Xml);
        // Note you have these variables available everywhere in your code:
        // string SettingsStr  -- The content of setting "Settings string"
        // bool DebugMode      -- true if setting "Debug Mode" is clicked
        // bool SettingsExists -- false if settings are missing
        // string GetStrSetting(settingName) -- Returns the setting from SettingsStr
        // string GetXmlSetting(settingName) -- Returns the setting from XML file
        // string GetSetting(settingName)    -- Returns the setting in search order: 
        //                                      1)SettingsStr, 2)XML file

        if (SettingsExists)
        {
            lyrisListName = GetSetting("LyrisListName");
            lyrisServerName = GetSetting("LyrisServerName");
            if (DebugMode)
                Message.Text = "Debug info: " + lyrisListName + " - " + lyrisServerName;
        }

        if (IsPostBack == false)
        {
            // prepopulate box
            email.Text = PortalSettings.CurrentUser.Identity.Email;
        }
    }

    void SubscribeBtn_Click(Object sender, EventArgs e)
    {
        JoinList(email.Text, lyrisListName);
        Message.Text = email.Text + " subscribed!";
    }

    void LeaveBtn_Click(Object sender, EventArgs e)
    {
        LeaveList(email.Text, lyrisListName);
        Message.Text = email.Text + " unsubscribed!";
    }

    void JoinList(string email, string listname)
    {
        System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage(email, "join-" + listname + lyrisServerName);
        mail.Subject = "Join";
        mail.Body = "Join";
        //MailMessage Mailer = new MailMessage();
        //Mailer.From = email;
        //Mailer.To = "join-" + listname + lyrisServerName;
        //Mailer.Subject = "Join";
        //Mailer.Body = "Join";
        System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(Appleseed.Framework.Settings.Config.SmtpServer);
        smtp.Send(mail);
        //SmtpMail.SmtpServer = Appleseed.Framework.Settings.Config.SmtpServer;
        //SmtpMail.Send(Mailer);
    }

    void LeaveList(string email, string listname)
    {
        System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage(email, "leave-" + listname + lyrisServerName);
        mail.Subject = "Leave";
        mail.Body = "Leave";
        //MailMessage Mailer = new MailMessage();
        //Mailer.From = email;
        //Mailer.To = "leave-" + listname + lyrisServerName;
        //Mailer.Subject = "Leave";
        //Mailer.Body = "Leave";
        System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(Appleseed.Framework.Settings.Config.SmtpServer);
        smtp.Send(mail);
        //SmtpMail.SmtpServer = Appleseed.Framework.Settings.Config.SmtpServer;
        //SmtpMail.Send(Mailer);
    }
</script>

<rbfwebui:DesktopModuleTitle EditText="Edit" EditUrl="~/DesktopModules/CoreModules/Admin/PropertyPage.aspx"
    PropertiesText="PROPERTIES" PropertiesUrl="~/DesktopModules/CoreModules/Admin/PropertyPage.aspx"
    runat="server" ID="ModuleTitle" />
<hr noshade="noshade" size="1pt" width="98%" />
<table cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td>
            <span class="SubSubHead" style="height: 20">Newsletter Signup:</span></td>
    </tr>
    <tr>
        <td>
            <span class="Normal">Email:</span></td>
        
    </tr>
    <tr>
        <td>
            <asp:TextBox ID="email" Columns="9" Width="130" CssClass="NormalTextBox" runat="server" />
            <div class="SubHead">
                <asp:RegularExpressionValidator runat="server" ID="validEMailRegExp" ControlToValidate="email"
                    Display="Dynamic" ErrorMessage="Please enter a valid email address." ValidationExpression="[\w\.-]+(\+[\w-]*)?@([\w-]+\.)+[\w-]+" />
                <asp:RequiredFieldValidator runat="server" ID="rfvEMail" ControlToValidate="email"
                    Display="Dynamic" ErrorMessage="'Email' must not be left blank." />
            </div>
        </td>
    </tr>
    <tr>
        <td>
            <rbfwebui:Button ID="SubscribeBtn" runat="server" OnClick="SubscribeBtn_Click" Text="Join" />
            <rbfwebui:Button ID="LeaveBtn" runat="server" OnClick="LeaveBtn_Click" Text="Leave" />
        </td>
    </tr>
    <tr>
        <td>
            <rbfwebui:label ID="Message" class="NormalRed" runat="server" />
        </td>
    </tr>
</table>
