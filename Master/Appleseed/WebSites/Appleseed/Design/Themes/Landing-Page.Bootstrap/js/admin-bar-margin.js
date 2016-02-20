// move the page down a little bit because of the admin bar. 
$(document).ready(function () {
    //When user is already logged in otherwise else part
    if ($('.navbar-admin').css("display") != "block") {
        $(".header-container").css("margin-top", "0px");
    }
    else {
        $(".header-container").css("margin-top", "90px");
    }

    //For adding white horizontal line between adminbar and portalbanner
    var $win = $(window);
    $win.resize(function () {
        //Top Right Non-Admin Menu

        //When user is already logged in otherwise else part    
        if ($('.navbar-admin').css("display") != "block") {
            if ($win.width() > 768) {
                $('#PortBan').css("margin-top", "61px");
            }
            else {
                $('#PortBan').css("margin-top", "68px");
            }
        }
        else {
            $('#PortBan').css("margin-top", "-39px");
        }
    }).resize();
});