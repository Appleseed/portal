<%@ Control Language="c#" Inherits="Appleseed.DesktopModules.CommunityModules.HTMLDocument.HtmlModule"
    CodeBehind="HtmlModule.ascx.cs" %>

<div id="HTMLContainer" runat="server">

<% if (HasEditPermission()) {%>
    <div id="HTMLEditContainer" runat="server" style="position: relative;">
         <div id="HtmlModuleText" runat="server"></div>
         <asp:PlaceHolder ID="HtmlHolder" runat="server"></asp:PlaceHolder>
         <div id="HtmlModuleDialog" runat="server" style="display: none" >
                <iframe id="HtmlMoudleIframe" runat="server" ></iframe>
         </div>
    </div>
<% } else{ %>
        <asp:PlaceHolder ID="HtmlHolder2" runat="server"></asp:PlaceHolder>
<% } %>

</div>