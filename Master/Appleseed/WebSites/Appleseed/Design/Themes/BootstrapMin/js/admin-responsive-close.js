// move the page down a little bit because of the admin bar. 
$(document).ready(function () {
    if ($("#admin-toggle").css('display') == 'block') {
        $(".admin-menu-list li .SiteLink").click(function () {
            $("#admin-navbar-collapse").collapse('hide');
        });
    }
});

