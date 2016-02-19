// JavaScript source code
$(document).ready(function () {
    //Top Left Admin Content Menu
    $('a[href="#Admin_asSiteTree_SkipLink"]').remove();                                                         //Remove anchor to skiplink
    $('#Admin_asSiteTree_SkipLink').remove();                                                                   //Remove skip link

    //Top Right Admin Menu
    $('#Admin_PortalHeaderMenu>tbody>tr>td:first-child').contents().wrap('<li class="welcome-msg"></li>');      //Wrap the welcome msg
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
        $('#Admin_HeaderMenu2>tbody>tr>td:first-child').contents().wrap('<li class="welcome-msg"></li>');       //Wrap the welcome msg if user is logged in
        $('#PortBanContainer').css("margin-top", "19px");
    }
    else {
        $('#PortBanContainer').css("margin-top", "1px");
    }

    $('#Admin_HeaderMenu2>tbody>tr>td').contents().unwrap();                                                    //Remove TDs
    $('#Admin_HeaderMenu2>tbody>tr').contents().unwrap();                                                       //Remove TRs
    $('#Admin_HeaderMenu2>tbody').contents().unwrap();                                                          //Remove TBODY
    $('#Admin_HeaderMenu2 a').wrap('<li></li>');                                                                //Wrap A tags
    $('#Admin_HeaderMenu2').wrap('<ul class="admin-menu-list"></ul>');                                          //Wrap it all in a UL
    $('#Admin_HeaderMenu2').contents().unwrap();                                                                //Remove the TABLE
    $('.admin-menu-list').attr('id', 'Admin_HeaderMenu2');                                                      //Add the TABLEs ID to the UL
    $('.admin-menu-list').addClass('nav navbar-nav navbar-right');                                              //Add nav classes to right admin navbar
});