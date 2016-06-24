$(document).ready(function () {
    //.AspNet-Menu-Horizontal #Banner_biMenu  CONTAINER
    //.AspNet-Menu                            UL
    //.AspNet-Menu-Leaf                       LI
    //.AspNet-Menu-WithChildren               LI
    //.AspNet-Menu-Link                       A
    $('#Banner_biMenu').addClass('navbar-collapse collapse').removeClass('AspNet-Menu-Horizontal AspNet-Menu-Vertical');
    $('.AspNet-Menu').addClass('nav navbar-nav').removeClass('AspNet-Menu');
    $('.AspNet-Menu-Leaf .AspNet-Menu-Link').removeClass('AspNet-Menu-Link');
    $('.AspNet-Menu-Leaf').removeClass('AspNet-Menu-Leaf');
    $('.AspNet-Menu-WithChildren').addClass('dropdown-menu');
    //$('.dropdown>a').addClass('dropdown-toggle').attr('data-toggle', 'dropdown');
    $('.dropdown ul').addClass('dropdown-menu');
    $('.AspNet-Menu-WithChildren').removeClass('AspNet-Menu-WithChildren');
    $('.nav .dropdown-toggle').attr("data-hover", "dropdown");
    //dropdown-toggle

    $('.dropdown-toggle').append("<span class=\"caret\"></span>");

    $(window).bind('resize', function (e) {

        // To check User Logged in or not
        if ($('#Admin_UserPanel').length == 0) {
            // div doesn't exist
        }
        else {

        }

        if ($(window).width() > 768) {

            $('.portalmenu-margin').css('margin-top', '53px');
            if ($('#Admin_UserPanel').length == 0) {
                // user logged in
                $('.portalmenu-margin').css('margin-top', '80px');
            }

            $('#admin-contentid').css("display", "block");
            $('#bs-example-navbar-collapse-1 > ul').removeClass('navbar-left');
            $('#bs-example-navbar-collapse-1 > ul').addClass('navbar-right');
            //$('#navbar-brand-id').show();

            $('.loginMenu > ul').removeClass('navbar-left');
            $('.loginMenu > ul').addClass('navbar-right');

        }
        else {
            if ($('#Admin_UserPanel').length == 0) {
            $('.portalmenu-margin').css('margin-top', '53px');
                // div doesn't exist
            }
            $('.portalmenu-margin').css('margin-top', '50px');
            $('#admin-contentid').css("display", "none");
            $('#bs-example-navbar-collapse-1 > ul').removeClass('navbar-right');
            $('#bs-example-navbar-collapse-1 > ul').addClass('navbar-left');
            //$('#navbar-brand-id').hide();

            $('.loginMenu > ul').removeClass('navbar-right');
            $('.loginMenu > ul').addClass('navbar-left');

        }

    }).resize();
    $(".dropdown-menu").find("input, button").each(function () {
        $(this).click(function (e) {
            e.stopPropagation();
        });
    });
});
