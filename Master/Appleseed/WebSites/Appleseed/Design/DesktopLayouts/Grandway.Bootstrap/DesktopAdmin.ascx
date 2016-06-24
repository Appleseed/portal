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
        sbrMenu.AppendLine("<ul class=\"dl-menu\">");
        foreach (TreeNode item in this.asSiteTree.Nodes)
        {
            if (item.ChildNodes.Count == 0)
                sbrMenu.AppendLine("<li><a href=" + item.NavigateUrl + ">" + item.Text + "</a></li>");
            else
            {
                sbrMenu.AppendLine("<li><a href=" + item.NavigateUrl + ">" + item.Text + "</a>");
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
        sbrMenu.AppendLine("<ul class=\"dl-submenu\">");
        foreach (TreeNode item in parent.ChildNodes)
        {
            if (item.ChildNodes.Count == 0)
                sbrMenu.AppendLine("<li><a href=" + item.NavigateUrl + ">" + item.Text + "</a></li>");
            else
            {
                sbrMenu.AppendLine("<li><a href=" + item.NavigateUrl + ">" + item.Text + "</a>");
                RenderSubLinks(item);
                sbrMenu.AppendLine("</li>");
            }
        }

        sbrMenu.AppendLine("</ul>");
    }
</script>

<asp:Panel ID="BarPanel" runat="server" Visible="false">
    <div class="top_line">
        <div class="page_head" id="divAdminBarLoggedIn">
            <div class="nav-container">
                <nav>
                    <div class="container">
                        <div class="row">
                            <div class="col-lg-6 pull-left">
                                <!-- Left menu -->
                                <div class="menu float_left">
                                    <div id="admin-dl-menu-left" class="dl-menuwrapper">
                                        <a href="/1" class="navbar-brand">
                                            <img alt='Appleseed' src='/Design/Themes/Grandway.Bootstrap/images/brick.png' class='admin-logo' height='16' width='16' />AS
                                        </a>
                                        <ul id="admin-left-menu" class="dl-menu dl-menu-toggle">
                                            <li>
                                                <a href="/100"><span>Administration</span></a>
                                                <ul class="dl-menu">
                                                    <li><a href="/240">Site Settings</a></li>
                                                    <li><a href="/110">Page Manager</a></li>
                                                    <li><a href="/281">User Manager</a></li>
                                                    <li><a href="/155">File Manager</a></li>
                                                    <li><a href="/120">Global Modules</a></li>
                                                    <li><a href="/215">Recycle Bin</a></li>
                                                </ul>
                                            </li>
                                            <li>
                                                <a href="#">Site Content</a>
                                                <asp:TreeView ID="asSiteTree" runat="server" DataSourceID="biSMDS" CssClass="sitecontent" ExpandDepth="5" />
                                                <asp:Literal ID="ltrTopMenu" runat="server"></asp:Literal>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-6 pull-right">
                                <!-- right menu -->
                                <div class="menu">
                                    <div id="admin-dl-menu-right" class="dl-menuwrapper">
                                        <button class="dl-trigger float_right">Open Menu</button>
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
                                </div>

                            </div>
                        </div>
                    </div>
                </nav>
            </div>
        </div>
    </div>

    <div class="clearfix"></div>
</asp:Panel>


<!-- Panel for Users who aren't logged in. Doesn't include the top left menu administrator menu. -->
<asp:Panel ID="UserPanel" runat="server" Visible="false">

    <div style="width: 100%;" class="top_line">
        <div class="page_head" id="divAdminBarLoggedOut">
            <div class="nav-container" style="height: auto;">
                <nav style="top: 00px; z-index: 99990">
                    <div class="container">
                        <div class="row">
                            <div class="menu">
                                <div id="as-not-admin-bar" class="dl-menuwrapper">
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

                        </div>
                    </div>
                </nav>
            </div>
        </div>
    </div>
    <%--<header>
        <div id="as-not-admin-bar1" class="navbar navbar-admin navbar-fixed-top" role="navigation">
            <div class="container">
                <div class="navbar-header">
                    <div class="navbar-collapse collapse userMenu">
                        <!-- begin User Menu at the Top of the Page -->

                    </div>
                </div>
            </div>
        </div>
    </header>--%>
</asp:Panel>

<asp:SiteMapDataSource ID="biSMDS" ShowStartingNode="false" runat="server" />
