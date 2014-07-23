<%@ Control Language="c#" %>
<%@ Register Assembly="Appleseed.Framework.Core" Namespace="Appleseed.Framework.Web.UI.WebControls" TagPrefix="rbfwebui" %>
<%@ Register Assembly="Appleseed.Framework.Web.UI.WebControls" Namespace="Appleseed.Framework.Web.UI.WebControls" TagPrefix="rbfwebui" %>
<script runat="server">
    private void Page_Load(object sender, System.EventArgs e)
    {
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
    <asp:PlaceHolder id="BarPaneUser" runat="server">
	
    </asp:PlaceHolder>
	<div class="contenedor_menu">
		<!-- Begin Portal Manu -->
			<div class="menu_border_left"></div>
			<asp:Menu 	ID="biMenu"	runat="server" 
						DataSourceID="biSMDS" 
						Orientation="Horizontal"
						CssClass="menu" 
						DynamicEnableDefaultPopOutImage="False" 
						StaticEnableDefaultPopOutImage="False">                                
			</asp:Menu>
			<div class="menu_border_right"></div>
		<!-- End Portla Menu -->
	</div>
</div>


<asp:SiteMapDataSource ID="biSMDS" ShowStartingNode="false" runat="server" />

<script type="text/javascript">
	$("a.AspNet-Menu-Link[href='/site/100/Administration']").parent().remove();
	
</script>
