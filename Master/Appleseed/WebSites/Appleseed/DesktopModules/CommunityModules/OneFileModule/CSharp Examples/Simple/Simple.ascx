<%@ Control Language="c#" inherits="Appleseed.Content.Web.Modules.OneFileModule" %>
<%@ Register TagPrefix="rbfwebui" Namespace="Appleseed.Framework.Web.UI.WebControls" assembly="Appleseed.Framework" %>

<script language="C#" runat="server">

    void Page_Load(Object sender, EventArgs e)
    {
        InitSettings(SettingsType.Str);

        if (SettingsExists)
        {
            lblFirstName.Text = GetSetting("FirstName");
            lblLastName.Text = GetSetting("LastName");
            lblSettingsStr.Text = SettingsStr;
        }
    }

</script>

<rbfwebui:DesktopModuleTitle EditText="Edit" EditUrl="~/DesktopModules/CoreModules/Admin/PropertyPage.aspx"
    PropertiesText="PROPERTIES" PropertiesUrl="~/DesktopModules/CoreModules/Admin/PropertyPage.aspx"
    runat="server" ID="ModuleTitle" />
<b>Setting FirstName:</b>
<rbfwebui:label ID="lblFirstName" runat="server" /><br/>
<b>Setting LastName:</b>
<rbfwebui:label ID="lblLastName" runat="server" /><br/>
<b>SettingsStr:</b>
<rbfwebui:label ID="lblSettingsStr" runat="server" />
