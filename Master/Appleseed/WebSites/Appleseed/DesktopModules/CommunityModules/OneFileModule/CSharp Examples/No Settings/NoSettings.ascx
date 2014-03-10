<%@ Control Language="c#" inherits="Appleseed.Content.Web.Modules.OneFileModule" %>
<%@ Register TagPrefix="rbfwebui" Namespace="Appleseed.Framework.Web.UI.WebControls" assembly="Appleseed.Framework" %>

<script language="C#" runat="server">

    void Page_Load(Object sender, EventArgs e)
    {
        if (IsPostBack == false)
        {
        }
    }

</script>

<rbfwebui:DesktopModuleTitle EditText="Edit" EditUrl="~/DesktopModules/CoreModules/Admin/PropertyPage.aspx"
    PropertiesText="PROPERTIES" PropertiesUrl="~/DesktopModules/CoreModules/Admin/PropertyPage.aspx"
    runat="server" ID="ModuleTitle" />
This module does not use the settings system provided by the OneFileModuleKit!<p>
    Note: You can delete the tag &lt;rbfwebui:DesktopModuleTitle ... /&gt; and the &lt;%@
Register ... %&gt; if you do not need the title (which includes the edit settings
and properties link). </p>