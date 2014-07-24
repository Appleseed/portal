<%@ Control Language="c#" %>
<%@ Register Assembly="Appleseed.Framework.Core" Namespace="Appleseed.Framework.Web.UI.WebControls" TagPrefix="rbfwebui" %>
<%@ Register Assembly="Appleseed.Framework.Web.UI.WebControls" Namespace="Appleseed.Framework.Web.UI.WebControls" TagPrefix="rbfwebui" %>
<script runat="server">
    private void Page_Load( object sender, System.EventArgs e ) {
		PortalTitle.DataBind();
        PortalImage.DataBind();
    }
</script>

<header>
    <div class="navbar navbar-default" role="navigation">
        <div class="container">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#Banner_biMenu">
                    <span class="sr-only">Toggle navigation</span>
                    <span>Main Navigation</span>
                </button>
                <a class="navbar-brand" href="Default.aspx">
                <!-- Portal Logo Image Uploaded-->
			        <rbfwebui:headerimage id="PortalImage" runat="server" enableviewstate="false"/>
		        <!-- End Portal Logo-->
                <!-- Portal Title -->
			        <rbfwebui:headertitle id="PortalTitle" runat="server" cssclass="SiteTitle" enableviewstate="false"></rbfwebui:headertitle>
		        <!-- End Portal Title -->
                </a>
                <!-- Begin Portal Menu -->
	            <asp:Menu 	ID="biMenu"	runat="server" 
				            DataSourceID="biSMDS" 
				            DynamicEnableDefaultPopOutImage="False" 
				            StaticEnableDefaultPopOutImage="False">                                
	            </asp:Menu>
            <!-- End Portal Menu -->
            </div>
        </div>
    </div>
    <div class="clearfix"></div>
</header>
<asp:SiteMapDataSource ID="biSMDS" ShowStartingNode="false" runat="server" />
