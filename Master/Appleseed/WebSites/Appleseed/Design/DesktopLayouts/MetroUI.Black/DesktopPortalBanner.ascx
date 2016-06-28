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

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
    }

</script>

<header class="bg-dark">
    <div class="navigation-bar dark">
        <div class="navigation-bar-content container">
            <a href="/" class="element">
				<span class="icon-grid-view"></span>
                <!-- Portal Logo Image Uploaded-->
                <rbfwebui:HeaderImage ID="PortalImage" runat="server" EnableViewState="false" />
                <!-- End Portal Logo-->
                <!-- Portal Title -->
                <rbfwebui:HeaderTitle ID="PortalTitle" runat="server" CssClass="" EnableViewState="false"></rbfwebui:HeaderTitle>
                <!-- End Portal Title -->
            </a>
            <span class="element-divider"></span>
            <a class="element1 pull-menu" href="#"></a>
            <!-- Begin Portal Menu -->
           
            <!-- End Portal Menu -->
        </div>
    </div>
</header>

