<%@ Control Language="vb" inherits="Appleseed.Content.Web.Modules.OneFileModule" %>
<%@ Register TagPrefix="rbfwebui" Namespace="Appleseed.Framework.Web.UI.WebControls" assembly="Appleseed.Framework" %>

<script language="vb" runat="server">

    Sub Page_Load()
        InitSettings(SettingsType.Str)

        If SettingsExists Then
            lblFirstName.Text = GetSetting("FirstName")
            lblLastName.Text = GetSetting("LastName")
            lblSettingsStr.Text = SettingsStr
        End If
    End Sub

</script>

<rbfwebui:DesktopModuleTitle EditText="Edit" EditUrl="~/DesktopModules/CoreModules/Admin/PropertyPage.aspx"
    PropertiesText="PROPERTIES" PropertiesUrl="~/DesktopModules/CoreModules/Admin/PropertyPage.aspx"
    runat="server" ID="ModuleTitle" />
<b>Setting FirstName:</b>
<rbfwebui:label ID="lblFirstName" runat="server" /><br>
<b>Setting LastName:</b>
<rbfwebui:label ID="lblLastName" runat="server" /><br>
<b>SettingsStr:</b>
<rbfwebui:label ID="lblSettingsStr" runat="server" />
