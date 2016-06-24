// JavaScript source code
//$(document).ready(function () {
//Top Left Admin Content Menu
$('a[href="#Admin_asSiteTree_SkipLink"]').remove();                                                         //Remove anchor to skiplink
$('#Admin_asSiteTree_SkipLink').remove();                                                                   //Remove skip link

//Top Right Admin Menu

//$('#Admin_PortalHeaderMenu>tbody>tr>td:first-child').contents().wrap('<li class="welcome-msg"></li>');      //Wrap the welcome msg
//$('#Admin_PortalHeaderMenu>tbody>tr>td:first-child').contents().wrap('<a href="#"></a>');       //Wrap the welcome msg if user is logged in
$('#Admin_PortalHeaderMenu>tbody>tr>td:first-child').contents().wrap('<li class="welcome-msg"></li>');       //Wrap the welcome msg if user is logged in

$('#Admin_PortalHeaderMenu>tbody>tr>td').contents().unwrap();                                               //Remove TDs
$('#Admin_PortalHeaderMenu>tbody>tr').contents().unwrap();                                                  //Remove TRs
$('#Admin_PortalHeaderMenu>tbody').contents().unwrap();                                                     //Remove TBODY
$('#Admin_PortalHeaderMenu a').wrap('<li></li>');                                                           //Wrap A tags
$('#Admin_PortalHeaderMenu').wrap('<ul class="admin-menu-list"></ul>');                                     //Wrap it all in a UL
$('#Admin_PortalHeaderMenu').contents().unwrap();                                                           //Remove the TABLE
$('.admin-menu-list').attr('id', 'Admin_PortalHeaderMenu');                                                 //Add the TABLEs ID to the UL
//$('.admin-menu-list').addClass('nav navbar-nav navbar-right');                                              //Add nav classes to right admin navbar

//Top Right Non-Admin Menu
var firstItemText = $.trim($('#Admin_HeaderMenu2>tbody>tr>td:first-child').text());                         //Checks the text of the first item in the menu
var firstChars = firstItemText.substr(0, 5);                                                                //Gets the first 5 characters of the first item
if (firstChars != "Login")                                                                                  //Checks to see if the firstChars aren't Login
{
    $('#Admin_HeaderMenu2>tbody>tr>td:first-child').contents().wrap('<a href="#"></a>');       //Wrap the welcome msg if user is logged in
    $('#Admin_HeaderMenu2>tbody>tr>td:first-child').contents().wrap('<li class="welcome-msg"></li>');       //Wrap the welcome msg if user is logged in
}
else {
    $("#divAdminBarMain").hide();                                                                           //Header - Hide "Admin Bar" - People shouldn't see Login / Language Bar
}

$('#Admin_HeaderMenu2>tbody>tr>td').contents().unwrap();                                                    //Remove TDs
$('#Admin_HeaderMenu2>tbody>tr').contents().unwrap();                                                       //Remove TRs
$('#Admin_HeaderMenu2>tbody').contents().unwrap();                                                          //Remove TBODY
$('#Admin_HeaderMenu2 a').wrap('<li></li>');                                                                //Wrap A tags
$('#Admin_HeaderMenu2').wrap('<ul class="admin-menu-list"></ul>');                                          //Wrap it all in a UL
$('#Admin_HeaderMenu2').contents().unwrap();                                                                //Remove the TABLE
$('.admin-menu-list').attr('id', 'Admin_HeaderMenu3');                                                      //Add the TABLEs ID to the UL
//$('.admin-menu-list').addClass('nav navbar-nav navbar-right');                                              //Add nav classes to right admin navbar
$("#Admin_HeaderMenu3").addClass('dl-menu dl-menu-toggle');  //.removeClass("admin-menu-list")

$('#admin-dl-menu-right').dlmenu({
    animationClasses: {
        classin: 'dl-animate-in-5',
        classout: 'dl-animate-out-5'
    }
});
$("#admin-left-menu .dl-back").remove();
$(".welcome-msg li").unwrap();
/*HatpiX*/

$(function () {
    $(window).resize(function () {

        if ($(window).width() < 620) {
            $('.top_line p').hide();
        }
        else {
            $('.top_line p').show();
        }
    });
});

$(function () {
    var $win = $(window);
    $win.resize(function () {
        //if ($win.width() > 768) {
        //    $(".admin-menu-list").attr("data-toggle", "");

        //    $('.admin-menu-list').addClass('nav navbar-nav navbar-right');
        //    $('.admin-menu-list').removeClass('dl-menu collapse');
        //    //$(".admin-menu-list").attr("data-toggle", " ");
        //}

        //else {
        //    $('.admin-menu-list').removeClass('nav navbar-nav navbar-right');
        //    $('.admin-menu-list').addClass('dl-menu collapse');
        //    //$(".admin-menu-list").attr("data-toggle", "dropdown");
        //}

        //if ($('#btnPortalBannerToggle').css("display") == "none") {
        //    $('#Banner_biMenu > ul').addClass('navbar-right');
        //    $('#Banner_biMenu > ul').removeClass('nav-left');
        //}
        //else {
        //    $('#Banner_biMenu > ul').removeClass('navbar-right');
        //    $('#Banner_biMenu > ul').addClass('nav-left');
        //}

    }).resize();
    $(".dropdown-menu").find("input, button").each(function () {
        $(this).click(function (e) {
            e.stopPropagation();
        });
    });
});


/* Top right Admin menu*/
/* Works only when use not logged in*/
//if (firstChars == "Login")                                                                //Checks to see if the firstChars is Login
//{
//    $('#as-not-admin-bar').attr('style', 'height:43px !important; margin:0px;z-index:99999'); // Resize the adminbar height  
//    $('.page_head').attr('style', 'margin-top: 55px;');                                         // Resize the portal banner top margin
//    $('.navbar-nav.navbar-right:last-child').attr('style', 'margin-top: -18px;');               // Resize the admin menu margin
//}

//});