<%@ Control AutoEventWireup="false" Inherits="Appleseed.Content.Web.Modules.AppleseedVersion" Language="c#" Codebehind="AppleseedVersion.ascx.cs" %>
<p>
    <rbfwebui:Label ID="AppleseedVersionLabel" runat="server" CssClass="Normal" EnableViewState="false"
        Text="The running Appleseed version is" TextKey="Appleseed_RUNNING_VERSION">
    </rbfwebui:Label>
    <rbfwebui:Label ID="VersionLabel" runat="server" CssClass="Normal" EnableViewState="False"></rbfwebui:Label>
</p>
<p>
    <rbfwebui:Label ID="currentUILanguage" runat="server" CssClass="Normal" EnableViewState="False"></rbfwebui:Label>/
    <rbfwebui:Label ID="currentLanguage" runat="server" CssClass="Normal" EnableViewState="False"></rbfwebui:Label>
</p>
