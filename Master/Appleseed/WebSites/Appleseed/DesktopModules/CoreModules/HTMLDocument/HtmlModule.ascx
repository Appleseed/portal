<%@ Control Language="c#" Inherits="Appleseed.DesktopModules.CoreModules.HTMLDocument.HtmlModule"
    CodeBehind="HtmlModule.ascx.cs" %>

<asp:PlaceHolder ID="plcAloha" runat="server" Visible="false">
    <script src="/aspnet_client/AlohaHtmlEditor/js/aloha-config.js"></script>
    <script src="/aspnet_client/AlohaHtmlEditor/lib/require.js"></script>
    <script src="/aspnet_client/AlohaHtmlEditor/js/htmlEncode.js"></script>
    <script src="/aspnet_client/AlohaHtmlEditor/lib/aloha.js" data-aloha-plugins="common/ui,common/format,common/table,common/list,common/link,common/highlighteditables,common/undo,common/contenthandler,common/paste,common/characterpicker,common/save,common/version,common/commands,common/block,common/image,common/abbr,common/horizontalruler,common/align,common/dom-to-xhtml,extra/textcolor,extra/formatlesspaste,extra/hints,extra/toc,extra/headerids,extra/listenforcer,extra/metaview,extra/numerated-headers,extra/textcolor,extra/wai-lang,extra/linkbrowser,extra/imagebrowser,extra/cite"></script>
</asp:PlaceHolder>

<div id="HTMLContainer" runat="server">

    <% if (HasEditPermission())
        {%>
    <div id="HTMLEditContainer" runat="server" style="position: relative;">
        <div id="HtmlModuleText" runat="server"></div>
        <asp:PlaceHolder ID="HtmlHolder" runat="server"></asp:PlaceHolder>
        <div id="HtmlModuleDialog" runat="server" style="display: none">
            <iframe id="HtmlMoudleIframe"></iframe>
        </div>
    </div>
    <% }
        else
        { %>
    <asp:PlaceHolder ID="HtmlHolder2" runat="server"></asp:PlaceHolder>
    <% } %>
</div>

<asp:PlaceHolder ID="plcAlohaStartupJs" runat="server" Visible="false">
    <script type="text/javascript">
        $(document).ready(function () {
            Aloha.ready(function () {
                Aloha.settings.jQuery = jQuery.noConflict(true);
                Aloha.jQuery('.area-content').aloha();
            });
            $('body').css('padding-top', '0px');
        });
    </script>
</asp:PlaceHolder>
