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
        sbrMenu.AppendLine("<ul class=\"nav navbar-nav navbar-right\" data-role=\"dropdown\">");
        foreach (MenuItem item in this.biMenu.Items)
        {
            if (item.ChildItems.Count == 0)
            {
                sbrMenu.AppendLine("<li ><a href=" + item.NavigateUrl + ">" + item.Text + "</a></li>");
            }
            else
            {
                sbrMenu.AppendLine("<li class=\"dropdown\"><a href=" + item.NavigateUrl + ">" + item.Text + "</a>");
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
        sbrMenu.AppendLine("<ul class=\"dropdown-menu\" data-role=\"dropdown\">");
        foreach (MenuItem item in parent.ChildItems)
        {
            if (item.ChildItems.Count == 0)
            {
                sbrMenu.AppendLine("<li><a href=" + item.NavigateUrl + ">" + item.Text + "</a></li>");
            }
            else
            {
                sbrMenu.AppendLine("<li class=\"dropdown\"><a href=" + item.NavigateUrl + ">" + item.Text + "</a>");
                RenderSubLinks(item);
                sbrMenu.AppendLine("</li>");
            }
        }

        sbrMenu.AppendLine("</ul>");
    }
    
</script>

<header>
    <%--<div id="PortBan" class="navbar navbar-static-top" role="navigation">--%>
    <%--<div class="col-lg-12">--%>
    <nav class="navbar navbar-default navbar-custom navbar-fixed-top broke-endless-pages portalmenu-margin">
        <div class="container-fluid">
            <%--<nav class="navbar navbar-default navbar-custom  navbar-fixed-top navbar-shrink is-fixed is-visible" style="margin-top:105px!important">--%>
            <%--<nav class="navbar navbar-default navbar-custom  navbar-fixed-top ">--%>
            <%--<div class="col-lg-6">--%>
            <div class="container topcontainer">
                <div class="row">
                    <div class="navbar-header page-scroll">
                        <button class="navbar-toggle" data-target="#bs-example-navbar-collapse-1" data-toggle="collapse" type="button">
                            <span class="sr-only">Toggle navigation</span>
                            <span class="icon-bar"></span>
                            <span class="icon-bar"></span>
                            <span class="icon-bar"></span>
                        </button>
                        <a class="navbar-brand" id="navbar-brand-id" href="/1">
                            <!-- Portal Logo Image Uploaded-->
                            <rbfwebui:HeaderImage ID="PortalImage" runat="server" EnableViewState="false" />
                            <!-- End Portal Logo-->
                            <!-- Portal Title -->
                            <rbfwebui:HeaderTitle ID="PortalTitle" runat="server" CssClass="SiteTitle navbar-brand" EnableViewState="false"></rbfwebui:HeaderTitle>
                            <!-- End Portal Title -->
                        </a>
                    </div>
                    <%--</div>--%>
                    <%--<div class="col-lg-6">--%>
                    <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">

                        <!-- Begin Portal Menu -->
                        <asp:Menu ID="biMenu" runat="server"
                            DataSourceID="biSMDS"
                            DynamicEnableDefaultPopOutImage="False"
                            StaticEnableDefaultPopOutImage="False">
                        </asp:Menu>
                        <asp:Literal ID="ltrTopMenu" runat="server"></asp:Literal>
                        <!-- End Portal Menu -->
                    </div>
                    <%--</div>--%>
                    <%----%>
                </div>
            </div>
        </div>
    </nav>
    <%--</div>--%>
</header>
<asp:SiteMapDataSource ID="biSMDS" ShowStartingNode="false" runat="server" />


<%--<nav class="navbar navbar-default navbar-custom navbar-fixed-top is-fixed is-visible">
            <div class="container-fluid">
                <div class="navbar-header page-scroll">
                    <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1" aria-expanded="false">
                        <span class="sr-only">Toggle navigation</span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                    </button>
                    <a class="navbar-brand" href="Default.aspx">
                        <!-- Portal Logo Image Uploaded-->
                        <rbfwebui:HeaderImage ID="HeaderImage1" runat="server" EnableViewState="false" />
                        <!-- End Portal Logo-->
                        <!-- Portal Title -->
                        <rbfwebui:HeaderTitle ID="HeaderTitle1" runat="server" CssClass="SiteTitle navbar-brand" EnableViewState="false"></rbfwebui:HeaderTitle>
                        <!-- End Portal Title -->
                    </a>
                </div>
                <div class="navbar-collapse collapse" id="bs-example-navbar-collapse-1" aria-expanded="false" style="height: 1px;">
                    <!-- Begin Portal Menu -->
                    <asp:Menu ID="Menu1" runat="server"
                        DataSourceID="biSMDS"
                        DynamicEnableDefaultPopOutImage="False"
                        StaticEnableDefaultPopOutImage="False">
                    </asp:Menu>
                    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                    <!-- End Portal Menu -->
                </div>
            </div>
        </nav>--%>