<%@ Control Language="c#" %>
<%@ Register Assembly="Appleseed.Framework.Core" Namespace="Appleseed.Framework.Web.UI.WebControls" TagPrefix="rbfwebui" %>
<%@ Register Assembly="Appleseed.Framework.Web.UI.WebControls" Namespace="Appleseed.Framework.Web.UI.WebControls" TagPrefix="rbfwebui" %>

<%@ Register TagPrefix="alm" TagName="AdminLeftMenu"
    Src="~/DesktopModules/CoreModules/Admin/AdminLeftMenu.ascx" %>

<script runat="server">
    private void Page_Load(object sender, System.EventArgs e)
    {
        PortalTitle.DataBind();
        PortalImage.DataBind();
        ltrLoggedInUserName.Text = PortalSettings.CurrentUser.Identity.Name;
        var localLastLoginTime = TimeZoneInfo.ConvertTime(DateTime.SpecifyKind(PortalSettings.CurrentUser.Identity.LastLoginDate, DateTimeKind.Utc), TimeZoneInfo.Utc, TimeZoneInfo.Local);
        ltrLoggedInUserLastActivityDate.Text = localLastLoginTime.ToString("dd MMM h:mmtt");
    }
</script>

<div class="navbar" role="navigation">
    <div class="container">
        <div class="navbar-header">
            <a class="navbar-brand" href="/1">
                <!-- Portal Logo Image Uploaded-->
                <rbfwebui:HeaderImage ID="PortalImage" runat="server" EnableViewState="false" />
                <!-- End Portal Logo-->
                <!-- Portal Title -->
                <!--<rbfwebui:headertitle id="PortalTitle" runat="server" cssclass="SiteTitle navbar-brand" enableviewstate="false"></rbfwebui:headertitle>-->
                <!-- End Portal Title -->
            </a>
            <!-- Begin Portal Menu -->
            <!--<asp:Menu 	ID="biMenu"	runat="server" 
			            DataSourceID="biSMDS"
                        CssClass="menu main-menu" 
			            DynamicEnableDefaultPopOutImage="False" 
			            StaticEnableDefaultPopOutImage="False">                                
            </asp:Menu>-->
            <!-- End Portal Menu -->
            <a data-placement="bottom" data-original-title="Show / Hide Left" data-toggle="tooltip" class="btn btn-primary toggle-left" id="menu-toggle">
                <i class="fa fa-bars"></i>
                <!--<img class="fa-img" src="/Design/Themes/Appleseed.Admin/images/icons/fa-bars.png" />-->
            </a>
        </div>
    </div>
</div>

<div class="menus">
    <!-- wide sidebar -->
    <div id="left-wide" style="float: right;">
        <!-- /.search-bar -->
        <div class="media user-media bg-dark dk">
           
            <div class="user-wrapper" style="padding:10px;">
              
                <div class="media-body">
                    <h5 class="media-heading">
                        <asp:Literal ID="ltrLoggedInUserName" runat="server"></asp:Literal></h5>
                    <ul class="list-unstyled user-info">
                        <li>Last Access :
              <br>
                            <small>
                                <i class="fa fa-calendar"></i>
                                <span class="access-date">&nbsp;<asp:Literal ID="ltrLoggedInUserLastActivityDate" runat="server"></asp:Literal></span></small>
                        </li>
                    </ul>
                </div>
            </div>
        </div>

        <!-- .wide-menu #home-menu-->
        <div id="home-menu" class="wide-menu bg-dark dk">

            <!-- Admin Left Menu from separate control 2015-03-12 mlp -->
            <alm:AdminLeftMenu ID="adminMenuLeft" runat="server" />

        </div>

    </div>
    <!-- /#left-wide -->
</div>

<asp:SiteMapDataSource ID="biSMDS" ShowStartingNode="false" runat="server" />



<script>
    $(function () {
        $(".wide-menu.accordion-lw, .wide-menu .accordion-sub").accordion({ collapsible: true });
    });

    $(document).ready(function () {
        var $menuDivs = $('#home-menu, #users-menu, #users-menu, #pages-menu, #files-menu, #modules-menu, #design-menu, #settings-menu, #recycler-menu');
        $('#home-menu, #users-menu, #users-menu, #pages-menu, #files-menu, #modules-menu, #design-menu, #settings-menu, #recycler-menu').hide();
        $('#home-menu').show();
        $('.menu-button').click(function () {
            var href = $(this).attr('href');
            console.log(href);
            $menuDivs.hide();
            console.log(href);
            $(href).show();
            //$('body').scrollTop(-155); not working ---
            removeHash(); // not working either.
            console.log('test2');
        });
    });

    function removeHash() {
        var scrollV, scrollH, loc = window.location;
        if ("pushState" in history)
            history.pushState("", document.title, loc.pathname + loc.search);
        else {
            // Prevent scrolling by storing the page's current scroll offset
            scrollV = document.body.scrollTop;
            scrollH = document.body.scrollLeft;

            loc.hash = "";

            // Restore the scroll offset, should be flicker free
            document.body.scrollTop = scrollV;
            document.body.scrollLeft = scrollH;
        }
    }
</script>

<script type="text/javascript">

    $("a.AspNet-Menu-Link[href='/site/1/Home']").parent().hide();
    $("a.AspNet-Menu-Link[href='/site/100/Administration']").parent().hide();

    var siteurl = window.location.href.toString();
    var pagename = siteurl.substr(siteurl.lastIndexOf("/") + 1);
    var sequence = siteurl.split('/');
    var toppage1 = sequence[3];
    var toppage2 = sequence[4];
    var toppage = sequence[5];
    var toppagename = $("#divCurrentCategory li").text()
    if (toppage == pagename) { $("#divCurrentCategory li").addClass("current"); };
    $("body").addClass(toppage);
    $("body").addClass(toppage1);
    $("body").addClass(toppage2);
    if (pagename == "Administration.aspx") {
        $("body").addClass("Administration");
    }


    $(document).ready(function () {
        $('p').each(function () {
            var $this = $(this);
            if ($this.html().replace(/\s|&nbsp;/g, '').length == 0)
                $this.addClass("hide");
        });

        var url = window.location.href.toString().split("/");
        var pagename2 = url.pop();
        var pagename1 = url.pop();
        $("#Banner_biMenu a[href$='" + pagename1 + "/" + pagename2 + "']")
            .closest("li")
            .addClass("selected-item");

        $('#menu-toggle').click(function () {
            //$('#left,#left-wide').toggleClass( "show" );
            // $('#left-wide').css('left', '0px');
            $('#left-wide').toggleClass("show");
        });

        $("#menu.wide-menu li a").click(function () {

            $("#menu.wide-menu li a + ul").slideToggle("fast");
        });

        $('[class*="Recycler"] tr[style*="#FFF5C9"]').attr('style', 'background-color:#303030');

    });

</script>
<style type="text/css">
    #left {
        width: 33% !important;
    }

    footer p {
        color: #fff !important;
    }

    footer a {
        color: #f4f4f4 !important;
    }

        footer a:hover {
            color: #CCC !important;
        }
</style>
