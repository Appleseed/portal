$(document).ready(function () {
    //.AspNet-Menu-Horizontal #Banner_biMenu  CONTAINER
    //.AspNet-Menu                            UL
    //.AspNet-Menu-Leaf                       LI
    //.AspNet-Menu-WithChildren               LI
    //.AspNet-Menu-Link                       A

    $('#Banner_biMenu').addClass('navbar-collapse collapse').removeClass('AspNet-Menu-Horizontal AspNet-Menu-Vertical');
    $('.AspNet-Menu').addClass('nav navbar-nav navbar-right').removeClass('AspNet-Menu');
    $('.AspNet-Menu-Leaf .AspNet-Menu-Link').removeClass('AspNet-Menu-Link');
    $('.AspNet-Menu-Leaf').removeClass('AspNet-Menu-Leaf');
    $('.AspNet-Menu-WithChildren').addClass('dropdown');
    $('.dropdown>a').addClass('dropdown-toggle').attr('data-toggle', 'dropdown');
    $('.dropdown ul').addClass('dropdown-menu');
    $('.AspNet-Menu-WithChildren').removeClass('AspNet-Menu-WithChildren');
    $('.nav .dropdown-toggle').attr("data-hover", "dropdown");

    $(function () {
        var $win = $(window);
        $win.resize(function () {
            if ($win.width() > 768)
                $(".navbar-nav > .dropdown > a").attr("data-toggle", "");
            else
                $(".navbar-nav > .dropdown > a").attr("data-toggle", "dropdown");
        }).resize();
        $(".dropdown-menu").find("input, button").each(function () {
            $(this).click(function (e) {
                e.stopPropagation();
            });
        });
    });
});