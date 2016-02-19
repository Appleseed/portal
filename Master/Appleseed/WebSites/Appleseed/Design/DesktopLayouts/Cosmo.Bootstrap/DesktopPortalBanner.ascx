<%@ Control Language="c#" %>
<%@ Register Assembly="Appleseed.Framework.Core" Namespace="Appleseed.Framework.Web.UI.WebControls" TagPrefix="rbfwebui" %>
<%@ Register Assembly="Appleseed.Framework.Web.UI.WebControls" Namespace="Appleseed.Framework.Web.UI.WebControls" TagPrefix="rbfwebui" %>
<script runat="server">
    private void Page_Load( object sender, System.EventArgs e ) {
		PortalTitle.DataBind();
        PortalImage.DataBind();
        HtmlMeta keywords = new HtmlMeta();
        keywords.HttpEquiv = "viewport";
        keywords.Name = "viewport";
        keywords.Content = "width=device-width, initial-scale=1";
        this.Page.Header.Controls.Add(keywords);
    }
</script>

<header>	
    <div id="PortBan" class="navbar navbar-default navbar-static-top" role="navigation">
        <div class="container">
            <div class="navbar-header">
                <button id="btnPortalBannerToggle" type="button" class="navbar-toggle" data-toggle="collapse" data-target="#Banner_biMenu">
                    <span class="sr-only">Toggle navigation</span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                <a class="navbar-brand" href="/1">
                <!-- Portal Logo Image Uploaded-->
			        <rbfwebui:headerimage id="PortalImage" runat="server" enableviewstate="false"/>
		        <!-- End Portal Logo-->
                <!-- Portal Title - hidden by default - 
                change visible="false" to visible ="true" to allow it to show-->
                    <rbfwebui:headertitle id="PortalTitle" runat="server" cssclass="SiteTitle navbar-brand" visible="false" enableviewstate="false"></rbfwebui:headertitle>
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
</header>
<asp:SiteMapDataSource ID="biSMDS" ShowStartingNode="false" runat="server" />