<%@ Control Language="c#" %>
<%@ Register Assembly="Appleseed.Framework.Core" Namespace="Appleseed.Framework.Web.UI.WebControls" TagPrefix="rbfwebui" %>
<%@ Register Assembly="Appleseed.Framework.Web.UI.WebControls" Namespace="Appleseed.Framework.Web.UI.WebControls" TagPrefix="rbfwebui" %>
<script runat="server">
    private void Page_Load(object sender, System.EventArgs e)
    {
        PortalTitle.DataBind();
        PortalImage.DataBind();
        HtmlMeta keywords = new HtmlMeta();
        keywords.HttpEquiv = "viewport";
        keywords.Name = "viewport";
        keywords.Content = "width=device-width, initial-scale=1";
        this.Page.Header.Controls.Add(keywords);
    }
</script>

<header class="header">
    <div class="container">
        <div class="row">
            <div class="col-md-2 col-xs-6">
                <!--<a class="navbar-brand logo" href="/1">

                    <rbfwebui:HeaderImage ID="PortalImage" runat="server" EnableViewState="false" />
                    
                    <rbfwebui:HeaderTitle ID="PortalTitle" runat="server" CssClass="SiteTitle navbar-brand" EnableViewState="false"></rbfwebui:HeaderTitle>
                    
                </a>-->
                <a href="/1" class="logo">
                    <img src="/Design/Themes/Theme.Next/images/anant-logo-white-112.png" alt="Anant Logo" class="logo-white" />
                    <img src="/Design/Themes/Theme.Next/images/anant-logo-112.png" alt="Anant Logo" class="logo-dark" />
                </a>
            </div>
            <div class="col-md-7 hidden-sm hidden-xs header-nav">
                <!-- Begin Portal Menu -->
                <asp:Menu ID="biMenu" runat="server"
                    DataSourceID="biSMDS"
                    DynamicEnableDefaultPopOutImage="False"
                    StaticEnableDefaultPopOutImage="False">
                </asp:Menu>
                <!-- End Portal Menu -->
            </div>
            <!--<div class="col-md-3 text-right hidden-sm hidden-xs">
                <a href="https://appleseeds.wufoo.com/forms/anant-start-a-project/" class="button-small">Start a Project</a>
            </div>-->
            <div class="col-xs-6 hidden-md hidden-lg text-right">
                <a href="javascript:void(0);" class="toggle-nav">
                    <span class="fa fa-bars"></span>
                    <span class="fa fa-close"></span>
                </a>
            </div>
        </div>
    </div>
</header>
      
<div class="mobile-nav">
    <ul>
        <li><a href="/1">Home</a></li>
    </ul>
</div>

<asp:SiteMapDataSource ID="biSMDS" ShowStartingNode="false" runat="server" />

<script type="text/javascript">

    var siteurl = window.location.href.toString();
    var pagename = siteurl.substr(siteurl.lastIndexOf("/")+1);
    var sequence = siteurl.split('/');
    var toppage1 = sequence[3];
    var toppage2 = sequence[4];
    var toppage = sequence[5];
    $("body").addClass(toppage);
    $("body").addClass(toppage1);
    $("body").addClass(toppage2);
    $("body").addClass(pagename);

    // Hide Home and Administration Tabs in Top Menu.
    $("a.AspNet-Menu-Link[href*='Home']").parent().remove();
    $("a.AspNet-Menu-Link[href*='Administration']").parent().remove();

    $(document).ready(function() {
        if ($('body').is('[class*="Administration"]')) { 
            $(".CenterCol").addClass("admin-wrapper container");
            $("#contentpane").addClass("row");
        }

        if ($('body').is('[class*="Feature"]')) { 
            $(".CenterCol").removeClass("admin-wrapper container");
            $("#contentpane").removeClass("row");
        }
        
        if ($('body').is('[class*="Register"]') || $('body').is('[class*="Logon"]')) { 
            $(".ModuleContent").addClass("admin-wrapper container");
            $(".module_Body").addClass("row");
        }
        
        if ($('body').is('[class*="Logon"]')) { 
            $("#Main_Table table").attr("width","100%");
        }

        if ($('body').is('[class*="Home"]')) {
            $('.header').addClass('header-no-bg');
        };

        // Signin temp fix 2016-01-20 mlp
        
        $(".desktopmodules_coremodules_signin_signincool_ascx .password-label").text('Password');
    });

    $(window).scroll(function () {
        console.log('test0');
        var scroll = $(window).scrollTop().valueOf();
        console.log(scroll);
        if (scroll > 0) { 
            console.log('test1');
            $('.header').addClass('header-scroll-bg');
        }
        if (scroll == 0) {
            console.log('test1');
            $('.header').removeClass('header-scroll-bg');
        }
    });

</script>

<script type="text/javascript">

    /**
      * NAME: Bootstrap 3 Triple Nested Sub-Menus
      * This script will active Triple level multi drop-down menus in Bootstrap 3.*
      * Update 2015-12-07 by MLP to allow closing of submenus on second click.
    */
    
    $('ul.dropdown-menu [class="dropdown-toggle"]').on('click', function(event) {
        // Avoid following the href location when clicking.
        event.preventDefault(); 
        // Avoid having the menu to close when clicking.
        event.stopPropagation(); 
        // Add or remove .open to parent sub-menu and dropdowns depending on whether already open.
        if ($(this).parent().hasClass('open')) {
            $(this).parent().removeClass('open');
            $(this).siblings().children().removeClass('open');
        }
        else {
            $(this).parent().addClass('open');
            $(this).parent().siblings().each(function() {
                $(this).removeClass('open');
            });
        }
    });

</script>