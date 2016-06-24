<%@ Control Language="c#" %>
<%@ Register Assembly="Appleseed.Framework.Core" Namespace="Appleseed.Framework.Web.UI.WebControls" TagPrefix="rbfwebui" %>
<%@ Register Assembly="Appleseed.Framework.Web.UI.WebControls" Namespace="Appleseed.Framework.Web.UI.WebControls" TagPrefix="rbfwebui" %>
<script runat="server">
    private void Page_Load( object sender, System.EventArgs e ) {
		PortalTitle.DataBind();
        PortalImage.DataBind();
    }
</script>

<header class="header">
    <nav id="main_menu" class="navbar navbar-static-top" role="navigation">
        <div class="container">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#Banner_biMenu">
                    <span class="sr-only">Toggle navigation</span>
                    <!--<span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>-->
                    <i class="fa fa-bars"></i>
                </button>
                <a class="navbar-brand" href="/1">
                <!-- Portal Logo Image Uploaded-->
			        <rbfwebui:headerimage id="PortalImage" runat="server" enableviewstate="false"/>
		        <!-- End Portal Logo-->
                <!-- Portal Title -->
			        <rbfwebui:headertitle id="PortalTitle" runat="server" cssclass="SiteTitle navbar-brand" enableviewstate="false"></rbfwebui:headertitle>
		        <!-- End Portal Title -->
                </a>
                <!-- Begin Portal Menu -->
	            <asp:Menu 	ID="biMenu"	runat="server" 
                            CssClass="collapse navbar-collapse menu"
				            DataSourceID="biSMDS" 
				            DynamicEnableDefaultPopOutImage="False" 
				            StaticEnableDefaultPopOutImage="False">                                
	            </asp:Menu>
            <!-- End Portal Menu -->
            </div>
        </div>
    </nav>
</header>
<asp:SiteMapDataSource ID="biSMDS" ShowStartingNode="false" runat="server" />

<!-- slider settings -->
<script type="text/javascript">
    //<![CDATA[
        var revapi;
        jQuery(document).ready(function() {
            revapi = jQuery('.fullwidthbanner').revolution(
            {
                delay:9000,
                startwidth:1170,
                startheight:600,
                fullWidth:"on",
                onHoverStop:"on" ,
                touchenabled:"on"
            });
        }); 
    //]]>
</script>

<script type="text/javascript">
    //<![CDATA[
    jQuery(document).ready(function($) {
        $("#slider_home").carouFredSel({ 
            width : "100%", 
            height : "auto",
            responsive : true,
            auto : false,
            items : { width : 280, visible: { min: 1, max: 3 }
            },
            swipe : { onTouch : true, onMouse : true },
            scroll: { items: 1, },
            prev : { button : "#sl-prev", key : "left"},
            next : { button : "#sl-next", key : "right" }
            });
        });
    //]]>

    $("a.AspNet-Menu-Link[href='/site/100/Administration']").parent().remove();
    
    var siteurl = window.location.href.toString();
    var pagename = siteurl.substr(siteurl.lastIndexOf("/")+1);
    var toppage = siteurl.split('/');
    var toppagename = $("#divCurrentCategory li").text()
    if (toppage[5] == pagename) { $("#divCurrentCategory li").addClass("current"); };
    $("body").addClass(toppage[5]);
    $("body").addClass(pagename);
</script>
