<%@ Control Language="c#" %>
<%@ Register Assembly="Appleseed.Framework.Core" Namespace="Appleseed.Framework.Web.UI.WebControls" TagPrefix="rbfwebui" %>
<%@ Register Assembly="Appleseed.Framework.Web.UI.WebControls" Namespace="Appleseed.Framework.Web.UI.WebControls" TagPrefix="rbfwebui" %>
<script runat="server">
    private void Page_Load( object sender, System.EventArgs e ) {
        PortalHeaderMenu.DataBind();
		PortalTitle.DataBind();
        PortalImage.DataBind();
    }
</script>


<div class="header">

	<div class="logo_div">
		<!-- Portal Logo Image Uploaded-->
			<rbfwebui:headerimage id="PortalImage" runat="server" enableviewstate="false"/>
		<!-- End Portal Logo-->
						<!-- Portal Title -->
			<rbfwebui:headertitle id="PortalTitle" runat="server" cssclass="SiteTitle" enableviewstate="false"></rbfwebui:headertitle>
		<!-- End Portal Title -->
	</div>

	<div class="userMenu">
		<!-- begin User Menu at the Top of the Page -->
			<rbfwebui:HeaderMenu 	ID="PortalHeaderMenu" runat="server" 
									CssClass="SiteLink" RepeatDirection="Horizontal" cellspacing="0"
									CellPadding="0" ShowHelp="False" ShowHome="False" 
									ShowLogon="true" ShowRegister="true" ShowDragNDrop="True"
                                    DialogLogon="true" >
				<ItemStyle Wrap="False"></ItemStyle>
				<ItemTemplate>
					<!-- used to stylize the left border ex: border with images-->
						<div class="SiteLink_Border_Left"></div>
					<!-- End left border -->
							<div class="SiteLink_bg">
								<span class="SiteLink">
									<%# Container.DataItem %>
								</span>
							</div>
					<!-- used to stylize the right border-->
						<div class="SiteLink_Border_Right"></div>
					<!-- End right border -->
				</ItemTemplate>
				
					<SeparatorTemplate>
						<span class="SiteLink">&nbsp;&nbsp;|&nbsp;&nbsp;</span>
					</SeparatorTemplate>
				
			</rbfwebui:HeaderMenu>
		<!-- End User Menu -->
	</div>

	<div class="contenedor_menu">
		<!-- Begin Portal Manu -->
			<asp:Menu 	ID="biMenu"	runat="server" 
						DataSourceID="biSMDS" 
						Orientation="Horizontal"
						CssClass="menu" 
						DynamicEnableDefaultPopOutImage="False" 
						StaticEnableDefaultPopOutImage="False">                                
			</asp:Menu>
		<!-- End Portla Menu -->
	</div>
</div>


<asp:SiteMapDataSource ID="biSMDS" ShowStartingNode="false" runat="server" />
