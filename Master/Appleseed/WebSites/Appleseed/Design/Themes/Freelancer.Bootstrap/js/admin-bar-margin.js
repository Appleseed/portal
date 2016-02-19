// move the page down a little bit because of the admin bar. 
$(document).ready(function () {
    /*if ($('.navbar-admin').css("display") != "block") {
        $(".header-container").css("margin-top", "0px");

    }
    else {
        $(".header-container").css("margin-top", "90px");
    }*/
    //$("#PortBan").css("margin-top", "61px");

    //For adding white horizontal line between adminbar and portalbanner
    var $win = $(window);
    $win.resize(function () {
        if ($win.width() > 768) {
            $('#PortBan').css("margin-top", "10px");
        }
        else {
            $('#PortBan').css("margin-top", "-39px");
        }

    }).resize();
});