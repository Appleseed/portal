// JavaScript source code
$(document).ready(function () {
    //Top Left Admin Content Menu
    $('a[href="#Admin_asSiteTree_SkipLink"]').remove();                                                         //Remove anchor to skiplink
    $('#Admin_asSiteTree_SkipLink').remove();                                                                   //Remove skip link

    //Top Right Admin Menu
    $('#Admin_PortalHeaderMenu>tbody>tr>td:first-child').contents().wrap('<li class=""></li>');      //Wrap the welcome msg
    $('#Admin_PortalHeaderMenu>tbody>tr>td').contents().unwrap();                                               //Remove TDs
    $('#Admin_PortalHeaderMenu>tbody>tr').contents().unwrap();                                                  //Remove TRs
    $('#Admin_PortalHeaderMenu>tbody').contents().unwrap();                                                     //Remove TBODY
    $('#Admin_PortalHeaderMenu a').wrap('<li></li>');                                                           //Wrap A tags
    $('#Admin_PortalHeaderMenu').wrap('<ul class="admin-menu-list"></ul>');                                     //Wrap it all in a UL
    $('#Admin_PortalHeaderMenu').contents().unwrap();                                                           //Remove the TABLE
    $('.admin-menu-list').attr('id', 'Admin_PortalHeaderMenu');                                                 //Add the TABLEs ID to the UL
    $('.admin-menu-list').addClass('nav navbar-nav navbar-right');                                              //Add nav classes to right admin navbar

    //Top Right Non-Admin Menu
    var firstItemText = $.trim($('#Admin_HeaderMenu2>tbody>tr>td:first-child').text());                         //Checks the text of the first item in the menu
    var firstChars = firstItemText.substr(0, 5);                                                                //Gets the first 5 characters of the first item

    if (firstChars != "Login")                                                                                  //Checks to see if the firstChars aren't Login
    {
        $('#Admin_HeaderMenu2>tbody>tr>td:first-child').contents().wrap('<li class=""></li>');       //Wrap the welcome msg if user is logged in
    }    
    
    $('#Admin_HeaderMenu2>tbody>tr>td').contents().unwrap();                                                    //Remove TDs
    $('#Admin_HeaderMenu2>tbody>tr').contents().unwrap();                                                       //Remove TRs
    $('#Admin_HeaderMenu2>tbody').contents().unwrap();                                                          //Remove TBODY
    $('#Admin_HeaderMenu2 a').wrap('<li></li>');                                                                //Wrap A tags
    $('#Admin_HeaderMenu2').wrap('<ul class="admin-menu-list"></ul>');                                          //Wrap it all in a UL
    $('#Admin_HeaderMenu2').contents().unwrap();                                                                //Remove the TABLE
    $('.admin-menu-list').attr('id', 'Admin_HeaderMenu2');                                                      //Add the TABLEs ID to the UL
    $('.admin-menu-list').addClass('nav navbar-nav navbar-right');

    //Add nav classes to right admin navbar



    if ($("#Admin_HeaderMenu2").html().toLowerCase().indexOf("/logon.aspx") == -1) {

        //logged in user menu
        $("#Admin_HeaderMenu2").append("<li id='liAdminProfile' class=\"dropdown\"><a class='SiteLink dropdown-toggle'  data-hover='dropdown' data-toggle=''  href='javascript:;' id='hypProfile'><span>Profile</span><span class='caret'></span></a> <ul id='ulAdminProfileSubMenu' class='dropdown-menu'></ul></li>");

        $("#ulAdminProfileSubMenu").html('');
        $("#ulAdminProfileSubMenu").append($("#Admin_HeaderMenu2 li")[0]);
        $("#ulAdminProfileSubMenu").append("<li><hr/></li>");
        var added = 0;
        for (var i = 0; i < $("#Admin_HeaderMenu2").children().length; i++) {
            if ($($("#Admin_HeaderMenu2 li")[i]).html().toLowerCase().indexOf("/register.aspx") > -1 && added == 0) {
                $("#ulAdminProfileSubMenu").append($("#Admin_HeaderMenu2 li")[i]);
                added = 1;
            }
        }
        $("#ulAdminProfileSubMenu").append($("#Admin_HeaderMenu2 li")[0]);

        added = 0;
        for (var i = 0; i < $("#Admin_HeaderMenu2").children().length; i++) {
            if ($($("#Admin_HeaderMenu2 li")[i]).html().toLowerCase().indexOf("/memberinvite/renderview") > -1 && added == 0) {
                $("#ulAdminProfileSubMenu").append($("#Admin_HeaderMenu2 li")[i]);
                added = 1;
            }
        }
        added = 0;
        for (var i = 0; i < $("#Admin_HeaderMenu2").children().length; i++) {
            if ($($("#Admin_HeaderMenu2 li")[i]).html().toLowerCase().indexOf("popuplang") > -1 && added == 0) {
                $("#ulAdminProfileSubMenu").append($("#Admin_HeaderMenu2 li")[i]);
                added = 1;
            }
        }

        $("#ulAdminProfileSubMenu").append("<li><hr/></li>");

        added = 0;
        for (var i = 0; i < $("#Admin_HeaderMenu2").children().length; i++) {
            if ($($("#Admin_HeaderMenu2 li")[i]).html().toLowerCase().indexOf("/logoff.aspx") > -1 && added == 0) {
                $("#ulAdminProfileSubMenu").append($("#Admin_HeaderMenu2 li")[i]);
                added = 1;
            }
        }

    }	
});