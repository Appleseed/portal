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

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
    }
</script>

<asp:Panel ID="BarPanel" runat="server" Visible="false">
    <div class="container topcontainer">
        <div class="row">
            <div class="navbar-header page-scroll">
                <button type="button" id="adminMenuToggle" class="navbar-toggle collapsed admin-toggle" data-target=".userMenu" data-toggle="collapse" aria-expanded="false">
                    <span class="sr-only">Toggle navigation</span>
                    <span>Admin</span>
                </button>
                <a href="/1" class="navbar-brand">
                    <img alt='Appleseed' src='/Design/Themes/Clean-Blog.Bootstrap/images/brick.png' class='admin-logo' height='16' width='16' />AS
                </a>
            </div>
            <div id="admin-contentid">
                <ul class="nav navbar-nav ulpadding" data-role="dropdown">
                    <li class="dropdown">
                        <a href="/100" class="dropdown-toggle"><span>Administration</span></a>
                        <ul class="dropdown-menu">
                            <li><a href="/240">Site Settings</a></li>
                            <li><a href="/110">Page Manager</a></li>
                            <li><a href="/281">User Manager</a></li>
                            <li><a href="/155">File Manager</a></li>
                            <li><a href="/120">Global Modules</a></li>
                            <li><a href="/215">Recycle Bin</a></li>
                        </ul>
                    </li>
                </ul>
            </div>
            <div class="navbar-collapse collapse userMenu">

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
    <!-- End User Menu -->
    <!-- End Portal Menu -->
    <div class="clearfix"></div>
</asp:Panel>
<!-- Panel for Users who aren't logged in. Doesn't include the top left menu administrator menu. -->
<asp:Panel ID="UserPanel" runat="server" Visible="false">
    <%--<div id="as-not-admin-bar" class="navbar navbar-default navbar-fixed-top" role="navigation">--%>
    <div id="as-not-admin-bar" class="" role="">
        <div class="container topcontainer">
            <div class="row">
                <div class="navbar-header page-scroll">
                    <button type="button" class="navbar-toggle collapsed admin-toggle" data-target=".loginMenu" data-toggle="collapse" aria-expanded="false">
                        <span class="sr-only">Toggle navigation</span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                    </button>
                </div>
                <!-- begin User Menu at the Top of the Page -->
                <div class="navbar-collapse collapse loginMenu" aria-expanded="false">
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
                </div>
                <!-- End User Menu -->
            </div>
        </div>
    </div>

</asp:Panel>

