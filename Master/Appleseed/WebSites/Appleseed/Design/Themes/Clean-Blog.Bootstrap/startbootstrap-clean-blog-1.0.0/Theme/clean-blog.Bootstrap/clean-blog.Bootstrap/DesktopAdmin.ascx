<%@ Control Language="c#" %>
<%@ Register Assembly="Appleseed.Framework.Core" Namespace="Appleseed.Framework.Web.UI.WebControls" TagPrefix="rbfwebui" %>
<%@ Register Assembly="Appleseed.Framework.Web.UI.WebControls" Namespace="Appleseed.Framework.Web.UI.WebControls" TagPrefix="rbfwebui" %>
<script runat="server">

    public string ContentContainerSelector;
    private void Page_Load(object sender, System.EventArgs e)
    {
        PortalHeaderMenu.DataBind();

        if (Appleseed.Framework.Security.PortalSecurity.IsInRoles("Admins"))
        {
            BarPanel.Visible = true;
        }
        else
        {
            UserPanel.Visible = true;
        }
    }

    StringBuilder sbrMenu = new StringBuilder();
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        this.asSiteTree.DataBind();
        sbrMenu.AppendLine("<ul class=\"dropdown-menu\" data-role=\"dropdown\">");
        foreach (TreeNode item in this.asSiteTree.Nodes)
        {
            if (item.ChildNodes.Count == 0)
                sbrMenu.AppendLine("<li><a href=" + item.NavigateUrl + ">" + item.Text + "</a></li>");
            else
            {
                sbrMenu.AppendLine("<li class=\"dropdown\"><a href=" + item.NavigateUrl + " class=\"dropdown-toggle\">" + item.Text + "</a>");
                RenderSubLinks(item);
                sbrMenu.AppendLine("</li>");
            }
        }

        sbrMenu.AppendLine("</ul>");
        this.asSiteTree.Visible = false;
        ltrTopMenu.Text = sbrMenu.ToString();
    }

    private void RenderSubLinks(TreeNode parent)
    {
        sbrMenu.AppendLine("<ul class=\"dropdown-menu \" data-role=\"dropdown\">");
        foreach (TreeNode item in parent.ChildNodes)
        {
            if (item.ChildNodes.Count == 0)
                sbrMenu.AppendLine("<li><a href=" + item.NavigateUrl + ">" + item.Text + "</a></li>");
            else
            {
                sbrMenu.AppendLine("<li class=\"dropdown\"><a href=" + item.NavigateUrl + " class=\"dropdown-toggle\">" + item.Text + "</a>");
                RenderSubLinks(item);
                sbrMenu.AppendLine("</li>");
            }
        }

        sbrMenu.AppendLine("</ul>");
    }
</script>

<asp:Panel ID="BarPanel" runat="server" Visible="false">
    <nav class="navbar navbar-default navbar-custom navbar-fixed-top">
        <div class="container-fluid">
            <div class="navbar-header page-scroll">
                <button type="button" class="navbar-toggle collapsed admin-toggle"  data-target="#userMenu" data-toggle="collapse" aria-expanded="false">
                    <span class="sr-only">Toggle navigation</span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                <a href="<%= Appleseed.Framework.Settings.Path.ApplicationFullPath %>" class="navbar-brand">
                    <img alt='Appleseed' src='<%=Appleseed.Framework.Settings.Path.ApplicationFullPath  %>/Design/Themes/clean-blog.Bootstrap/images/brick.png' class='admin-logo' height='16' width='16' />AS
                </a>
                </div>
                <div class="navbar-collapse collapse in" id="userMenu" aria-expanded="false" style="">
                <ul class="nav navbar-nav " data-role="dropdown">
                    <li class="dropdown">
                        <a href="<%= Appleseed.Framework.Settings.Path.ApplicationFullPath %>/100" class="dropdown-toggle12"><span>Administration</span></a>
                        <ul class="dropdown-menu" data-role="dropdown">
                            <li><a href="<%=Appleseed.Framework.Settings.Path.ApplicationFullPath  %>/240">Site Settings</a></li>
                            <li><a href="<%=Appleseed.Framework.Settings.Path.ApplicationFullPath  %>/110">Page Manager</a></li>
                            <li><a href="<%=Appleseed.Framework.Settings.Path.ApplicationFullPath  %>/281">User Manager</a></li>
                            <li><a href="<%=Appleseed.Framework.Settings.Path.ApplicationFullPath  %>/120">Global Modules</a></li>
                            <li><a href="<%=Appleseed.Framework.Settings.Path.ApplicationFullPath  %>/215">Recycle Bin</a></li>
                            <li><a href="http://file.app.clients.anant.us">File.App</a></li>
                        </ul>
                    </li>
                    <li class="dropdown" style="z-index: 9999999999!important">
                        <a href="#">Site Content</a>
                        <asp:TreeView ID="asSiteTree" runat="server" DataSourceID="biSMDS" CssClass="sitecontent" ExpandDepth="5" />
                        <asp:Literal ID="ltrTopMenu" runat="server"></asp:Literal>
                    </li>
                </ul>
                <!-- begin User Menu at the Top of the Page -->
                
                    <rbfwebui:HeaderMenu ID="PortalHeaderMenu" runat="server"
                        CssClass="SiteLink" RepeatDirection="Horizontal" CellSpacing="0"
                        CellPadding="0" ShowHelp="False" ShowHome="False"
                        ShowLogon="true" ShowRegister="true" ShowDragNDrop="True"
                        DialogLogon="true" ShowLanguages="true" ShowFlags="true" ShowLangString="true">
                        <ItemStyle Wrap="False"></ItemStyle>
                        <ItemTemplate>
                            <%# Container.DataItem %>
                        </ItemTemplate>
                    </rbfwebui:HeaderMenu>
                </div>
                <!-- End User Menu -->
                <!-- End Portal Menu -->
            </div>
    </nav>
    <div class="clearfix"></div>
</asp:Panel>
<!-- Panel for Users who aren't logged in. Doesn't include the top left menu administrator menu. -->
<asp:Panel ID="UserPanel" runat="server" Visible="false">
    <div id="as-not-admin-bar" class="navbar navbar-default navbar-fixed-top" role="navigation">
        <nav class="navbar  navbar-custom">
            <div class="container-fluid">
            <div class="navbar-header page-scroll">
                <!-- begin User Menu at the Top of the Page -->
                <rbfwebui:HeaderMenu ID="HeaderMenu2" runat="server"
                    CssClass="SiteLink" RepeatDirection="Horizontal" CellSpacing="0"
                    CellPadding="0" ShowHelp="False" ShowHome="False"
                    ShowLogon="true" ShowRegister="true" ShowDragNDrop="True"
                    DialogLogon="true" ShowLanguages="true" ShowFlags="true" ShowLangString="true">
                    <ItemStyle Wrap="False"></ItemStyle>
                    <ItemTemplate>
                        <%# Container.DataItem %>
                    </ItemTemplate>
                </rbfwebui:HeaderMenu>
                <!-- End User Menu -->
            </div>
                </div>
        </nav>
    </div>

</asp:Panel>
<asp:SiteMapDataSource ID="biSMDS" ShowStartingNode="false" runat="server" />

