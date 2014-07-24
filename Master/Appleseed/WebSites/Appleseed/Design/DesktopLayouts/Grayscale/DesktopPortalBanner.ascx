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
    <div class="navbar navbar-custom" role="navigation">
        <div class="container">
            <div class="navbar-header page-scroll">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#Banner_biMenu">
                    <i class="fa fa-bars"></i>
                </button>
                <a class="navbar-brand" href="#page-top">
                <!-- Portal Logo Image Uploaded-->
			        <rbfwebui:headerimage id="PortalImage" runat="server" enableviewstate="false"/>
		        <!-- End Portal Logo-->
                <!-- Portal Title -->
			        <rbfwebui:headertitle id="PortalTitle" runat="server" cssclass="SiteTitle navbar-brand" enableviewstate="false"></rbfwebui:headertitle>
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
