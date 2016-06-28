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
    <header class="bg-dark">
        <div id="as-admin-bar" class="navigation-bar navbar-fixed-top">
            <div class="navigation-bar-content container">
                <a href="<%= Appleseed.Framework.Settings.Path.ApplicationFullPath %>" class="element">
                    <img alt='Appleseed' src='<%=Appleseed.Framework.Settings.Path.ApplicationFullPath  %>/Design/Themes/MetroUI.Blue/images/brick.png' class='admin-logo' height='16' width='16' />AS
                </a>
                <span class="element-divider"></span>
                <ul class="element-menu" id="admin-left-menu1">
                    <li>
                        <a href="<%= Appleseed.Framework.Settings.Path.ApplicationFullPath %>/100" class="dropdown-toggle">Administration</a>
                        <ul class="dropdown-menu" data-role="dropdown" style="display: none;">
                            <li><a href="<%=Appleseed.Framework.Settings.Path.ApplicationFullPath  %>/240">Site Settings</a></li>
                            <li><a href="<%=Appleseed.Framework.Settings.Path.ApplicationFullPath  %>/110">Page Manager</a></li>
                            <li><a href="<%=Appleseed.Framework.Settings.Path.ApplicationFullPath  %>/281">User Manager</a></li>
                            <li><a href="<%=Appleseed.Framework.Settings.Path.ApplicationFullPath  %>/120">Global Modules</a></li>
                            <li><a href="<%=Appleseed.Framework.Settings.Path.ApplicationFullPath  %>/215">Recycle Bin</a></li>
                            <li><a href="http://file.app.clients.anant.us">File.App</a></li>
                        </ul>
                    </li>
                </ul>

                <div class="navbar-collapse collapse userMenu">
                    <a class="element1 pull-menu" href="#" data-target="#Admin_HeaderMenu2"></a>
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
                    <!-- End User Menu -->
                </div>
                <!-- End Portal Menu -->
            </div>
        </div>
    </header>
    <div class="clearfix"></div>
</asp:Panel>
<!-- Panel for Users who aren't logged in. Doesn't include the top left menu administrator menu. -->
<asp:Panel ID="UserPanel" runat="server" Visible="false">
    <header class="bg-dark">

        <div id="as-not-admin-bar" class="navigation-bar navbar-fixed-top navbar-right">
            <div class="navigation-bar-content container">
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
    </header>
</asp:Panel>
