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

    StringBuilder sbrMenu = new StringBuilder();
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        this.biMenu.DataBind();
        sbrMenu.AppendLine("<ul class=\"element-menu navbar-right\" data-role=\"dropdown\"> ");
        foreach (MenuItem item in this.biMenu.Items)
        {
            if (item.ChildItems.Count == 0)
                sbrMenu.AppendLine("<li><a href=" + item.NavigateUrl + ">" + item.Text + "</a></li>");
            else
            {
                sbrMenu.AppendLine("<li><a href=" + item.NavigateUrl + " class=\"dropdown-toggle\">" + item.Text + "</a>");
                RenderSubLinks(item);
                sbrMenu.AppendLine("</li>");
            }
        }

        sbrMenu.AppendLine("</ul>");
        this.biMenu.Visible = false;
        ltrTopMenu.Text = sbrMenu.ToString();
    }

    private void RenderSubLinks(MenuItem parent)
    {
        sbrMenu.AppendLine("<ul class=\"dropdown-menu \" data-role=\"dropdown\">");
        foreach (MenuItem item in parent.ChildItems)
        {
            if (item.ChildItems.Count == 0)
                sbrMenu.AppendLine("<li><a href=" + item.NavigateUrl + ">" + item.Text + "</a></li>");
            else
            {
                sbrMenu.AppendLine("<li><a href=" + item.NavigateUrl + " class=\"dropdown-toggle\">" + item.Text + "</a>");
                RenderSubLinks(item);
                sbrMenu.AppendLine("</li>");
            }
        }

        sbrMenu.AppendLine("</ul>");
    }
</script>

<header class="bg-dark">
    <div class="navigation-bar">
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
            <asp:Menu ID="biMenu" runat="server"
                DataSourceID="biSMDS"
                DynamicEnableDefaultPopOutImage="False"
                StaticEnableDefaultPopOutImage="False">
            </asp:Menu>
            <asp:Literal ID="ltrTopMenu" runat="server"></asp:Literal>
            <!-- End Portal Menu -->
        </div>
    </div>
</header>

<asp:SiteMapDataSource ID="biSMDS" ShowStartingNode="false" runat="server" />
