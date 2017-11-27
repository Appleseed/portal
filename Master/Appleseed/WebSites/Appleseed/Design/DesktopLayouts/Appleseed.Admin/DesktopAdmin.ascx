<%@ Control Language="c#" %>
<%@ Register Assembly="Appleseed.Framework.Core" Namespace="Appleseed.Framework.Web.UI.WebControls" TagPrefix="rbfwebui" %>
<%@ Register Assembly="Appleseed.Framework.Web.UI.WebControls" Namespace="Appleseed.Framework.Web.UI.WebControls" TagPrefix="rbfwebui" %>
<script runat="server">

    public string ContentContainerSelector;
    private void Page_Load(object sender, System.EventArgs e)
    {
        PortalHeaderMenu.DataBind();

        if (Appleseed.Framework.Security.PortalSecurity.IsInRoles("Admins") || Appleseed.Framework.Security.PortalSecurity.IsInRoles("Builder"))
        {
            BarPanel.Visible = true;
        }
		else if (Appleseed.Framework.Security.PortalSecurity.IsInRoles("Staff"))
        {
            BarPanelStaff.Visible = true;
			HeaderMenu1.ID = "PortalHeaderMenu";
			PortalHeaderMenu.ID = "PortalHeaderMenu1";
			asSiteTree1.ID = "asSiteTree";
        }
        else
        {
            UserPanel.Visible = true;
        }
        //plcAdminMenu.Visible = Appleseed.Framework.Security.PortalSecurity.IsInRoles("Admins");
        //plcBuilderMenu.Visible = Appleseed.Framework.Security.PortalSecurity.IsInRoles("Builder");
    }
</script>

<asp:Panel ID="BarPanel" runat="server" Visible="false">
    <div id="as-admin-bar" class="navbar navbar-admin navbar-fixed-top" role="navigation">
        <div class="container">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".userMenu">
                    <span class="sr-only">Toggle navigation</span>
                    <span>Admin</span>
                </button>
                <a href="/1" class="navbar-brand" id="hypHome">
                   <i class="fa fa-home fa-2x" aria-hidden="true"></i>
                </a>
                <ul id="admin-left-menu" class="nav navbar-nav">
                    <li>
                        <a href="/100"><span>Administration</span></a>
                        <ul>
                            <ul>
                            <li><a href="/240">Site Settings</a></li>
                            <li><a href="/110">Page Manager</a></li>
                            <li><a href="/155">File Manager</a></li>
                            <li><a href="/355">Member Manager</a></li>
                            <li><a href="/354">Event Manager</a></li>
                            <!--<li><a href="/378">View Donations</a></li>--><!-- for Stage -->
                            <li><a href="/377">View Donations</a></li><!-- for Prod -->
                            <li><a href="/280">Role Manager</a></li>
                            <li><a href="/368">Reports</a></li>
                            <!--<li><a href="/380">Page Sponsors</a></li>--><!-- for Stage -->
                            <li><a href="/381">Page Sponsors</a></li><!-- for Prod -->
                            <li><a href="/120">OTW Specific Modules</a></li>
                            <li><a href="/215">Recycle Bin</a></li>
                        </ul>
                        </ul>
                    </li>
					<li>
                        <a href="#">Site Content</a>
                        <asp:TreeView ID="asSiteTree" runat="server" DataSourceID="biSMDS" CssClass="sitecontent" ExpandDepth="0" />
                    </li>
                </ul>
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
                    <!-- End User Menu -->
                </div>
                <!-- End Portal Menu -->
            </div>
        </div>
    </div>
    <div class="clearfix"></div>
</asp:Panel>
<asp:Panel ID="BarPanelStaff" runat="server" Visible="false">
    <div id="as-admin-bar" class="navbar navbar-admin navbar-fixed-top" role="navigation">
        <div class="container">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle edit-button" data-toggle="collapse" data-target=".userMenu">
                    <span class="sr-only">Toggle navigation</span>
                    <span>Edit</span>
                </button>
                <button type="button" class="navbar-toggle admin-button" data-toggle="collapse" data-target=".admin-mobile-menu">
                    <span class="sr-only">Toggle navigation</span>
                    <span>Admin</span>
                </button>
                <a href="/1" class="navbar-brand">
                    <img alt='Appleseed' src='/Design/Themes/OTW/images/brick.png' class='admin-logo' height='16' width='16' />AS
                </a>
                <ul id="admin-left-menu" class="nav navbar-nav">
                    <li>
                        <a href="/100"><span>Administration</span></a>
                        <ul>
                            <li><a href="/240">Site Settings</a></li>
                            <li><a href="/110">Page Manager</a></li>
                            <li><a href="/155">File Manager</a></li>
                            <li><a href="/355">Member Manager</a></li>
                            <li><a href="/354">Event Manager</a></li>
                            <!--<li><a href="/378">View Donations</a></li>--><!-- for Stage -->
                            <li><a href="/377">View Donations</a></li><!-- for Prod -->
                            
                            <li><a href="/368">Reports</a></li>
                            <!--<li><a href="/380">Page Sponsors</a></li>--><!-- for Stage -->
                            <li><a href="/381">Page Sponsors</a></li><!-- for Prod -->
                            <li><a href="/120">OTW Specific Modules</a></li>
                            <li><a href="/215">Recycle Bin</a></li>
                        </ul>
                    </li>
                    <li>
                        <a href="#">Site Content</a>
                        <asp:TreeView ID="asSiteTree1" runat="server" DataSourceID="biSMDS" CssClass="sitecontent" ExpandDepth="0" />
                    </li>
                </ul>
                <div class="admin-mobile-menu admin-menu-list nav">
                    <ul class="navbar-nav">
                        <li><a href="/354">Event Manager</a></li>
                        <li><a href="/355">Member Manager</a></li>
                        <!--<li><a href="/368">Reports</a></li>-->
                    </ul>
                </div>
                <div class="navbar-collapse collapse userMenu">
                    <!-- begin User Menu at the Top of the Page -->
                    <rbfwebui:HeaderMenu ID="HeaderMenu1" runat="server"
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
    </div>
    <div class="clearfix"></div>
</asp:Panel>
<!-- Panel for Users who aren't logged in. Doesn't include the top left menu administrator menu. -->
<asp:Panel ID="UserPanel" runat="server" Visible="false">
    <div id="as-not-admin-bar" class="navbar navbar-admin navbar-fixed-top" role="navigation">
        <div class="container">
            <div class="navbar-header">
                <div class="navbar-collapse collapse userMenu">
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
        </div>
    </div>
</asp:Panel>
<asp:SiteMapDataSource ID="biSMDS" ShowStartingNode="false" runat="server" />