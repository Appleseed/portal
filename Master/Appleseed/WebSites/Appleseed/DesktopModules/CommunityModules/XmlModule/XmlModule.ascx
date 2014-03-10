<%@ Control AutoEventWireup="true" Inherits="Appleseed.Content.Web.Modules.XmlModule"
    Language="c#" CodeBehind="XmlModule.ascx.cs" %>
<%@ Register Assembly="Appleseed.Framework" Namespace="Appleseed.Framework.Web.UI.WebControls"
    TagPrefix="rbfwebui" %>
<rbfwebui:DesktopModuleTitle ID="XmlModuleTitle" runat="server" PropertiesText="PROPERTIES"
    PropertiesUrl="~/DesktopModules/CoreModules/Admin/PropertyPage.aspx">
</rbfwebui:DesktopModuleTitle>
<span class="Normal">
    <asp:Xml ID="xml1" runat="server"></asp:Xml></span>