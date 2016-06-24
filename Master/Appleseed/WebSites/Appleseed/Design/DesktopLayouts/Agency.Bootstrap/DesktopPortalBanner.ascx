<%@ Control Language="c#" %>
<%@ Register Assembly="Appleseed.Framework.Core" Namespace="Appleseed.Framework.Web.UI.WebControls" TagPrefix="rbfwebui" %>
<%@ Register Assembly="Appleseed.Framework.Web.UI.WebControls" Namespace="Appleseed.Framework.Web.UI.WebControls" TagPrefix="rbfwebui" %>
<script runat="server">
    private void Page_Load(object sender, System.EventArgs e)
    {
        PortalTitle.DataBind();
        PortalImage.DataBind();
        HtmlMeta keywords = new HtmlMeta();
        keywords.HttpEquiv = "viewport";
        keywords.Name = "viewport";
        keywords.Content = "width=device-width, initial-scale=1";
        this.Page.Header.Controls.Add(keywords);
    }
</script>

<div id="PortBan" class="navbar navbar-default navbar-fixed-top" role="navigation">
    <nav class="navbar navbar-default navbar-shrink">
        <div id="PortBanContainer" class="container" style="text-align: left">
            <div class="navbar-header page-scroll">
                <button id="btnPortalBannerToggle" type="button" class="navbar-toggle" data-toggle="collapse" data-target="#Banner_biMenu">
                    <span class="sr-only">Toggle navigation</span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                <a class="navbar-brand" href="/1">
                    <!-- Portal Logo Image Uploaded-->
                    <rbfwebui:HeaderImage ID="PortalImage" runat="server" EnableViewState="false" />
                    <!-- End Portal Logo-->
                    <!-- Portal Title -->
                    <rbfwebui:HeaderTitle ID="PortalTitle" runat="server" CssClass="SiteTitle navbar-brand" EnableViewState="false"></rbfwebui:HeaderTitle>
                    <!-- End Portal Title -->
                </a>
                <!-- Begin Portal Menu -->
                <asp:Menu ID="biMenu" runat="server"
                    DataSourceID="biSMDS"
                    DynamicEnableDefaultPopOutImage="False"
                    StaticEnableDefaultPopOutImage="False">
                </asp:Menu>
                <!-- End Portal Menu -->
            </div>
        </div>
    </nav>
</div>

<asp:SiteMapDataSource ID="biSMDS" ShowStartingNode="false" runat="server" />

<script type="text/javascript">
    
    $(window).load(function() {
        $("#AppleseedLogin input#iframeAppleseedLogin").attr("style","width: 440px!important; height: 230px!important;");
    });
    $(document).ready(function() {
        $( "ui.dialog" ).dialog( "moveToTop" );
    });

</script>