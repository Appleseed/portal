<%@ register src="~/Design/DesktopLayouts/DesktopFooter.ascx" tagname="Footer" tagprefix="foot" %>
<%@ register src="~/Design/DesktopLayouts/DesktopPortalBanner.ascx" tagname="Banner"
    tagprefix="portal" %>
<%@ page autoeventwireup="false" inherits="Appleseed.Content.Web.Modules.PictureView"
    language="c#" Codebehind="PictureView.aspx.cs" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server"><title></title>
</head>
<body id="Body1" runat="server">
    <form id="PictureViewForm" runat="server" method="post">
        <div id="zenpanes" class="zen-main">
        </div>
            <div class="rb_DefaultPortalHeader">
                <portal:banner id="SiteHeader" runat="server" />
            </div>
            <div align="center">
                <div class="rb_DefaultLayoutDiv">
                    <rbfwebui:label id="lblError" runat="server" font-bold="True" forecolor="Red" text="Failed to load templates. Revise your settings"
                        textkey="PICTURES_FAILED_TO_LOAD" visible="false"></rbfwebui:label>
                    <asp:placeholder id="Picture" runat="server"></asp:placeholder>
                </div>
                <div class="rb_AlternatePortalFooter">
                    <div class="rb_AlternatePortalFooter">
                        <foot:footer id="Footer" runat="server" />
                    </div>
                </div>
            </div>
    </form>
</body>
</html>
