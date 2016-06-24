/* Responisve Menu */
$('#dl-menu').dlmenu({
    animationClasses: {
        classin: 'dl-animate-in-5',
        classout: 'dl-animate-out-5'
    }
});


//$('#admin-dl-menu-right').dlmenu({
//    animationClasses: {
//        classin: 'dl-animate-in-5',
//        classout: 'dl-animate-out-5'
//    }
//});
/* Responisve Menu  - Admin Menu Left - LogggedIn User*/
$('#admin-dl-menu-left').dlmenu({
    animationClasses: {
        classin: 'dl-animate-in-5',
        classout: 'dl-animate-out-5'
    }
});



/* Responisve Menu */
$('#dl-menu-new').dlmenu({
    animationClasses: {
        classin: 'dl-animate-in-5',
        classout: 'dl-animate-out-5'
    }
});

$('#dl-admin-menu').dlmenu({
    animationClasses: {
        classin: 'dl-animate-in-5',
        classout: 'dl-animate-out-5'
    }
});

$('#dl-admin-menu2').dlmenu({
    animationClasses: {
        classin: 'dl-animate-in-5',
        classout: 'dl-animate-out-5'
    }
});

$('#dl-menu-admin-right').dlmenu({
    animationClasses: {
        classin: 'dl-animate-in-5',
        classout: 'dl-animate-out-5'
    }
});

/* Responisve Menu */

/* Accordion */
$('.accordion-grandway span').click(function () {
    if ($(this).next().is(':hidden')) {
        $(this).parents().siblings().children('span').removeClass('active').next().slideUp();
        $(this).addClass('active').next().slideDown();
    }
    return false;
});
/* Accordion */

/* Tabs */
jQuery('.shortcode_tabs').each(function (index) {
    var i = 1;
    jQuery('.shortcode_tab_item_title').each(function (index) {
        jQuery(this).addClass('it' + i);
        jQuery(this).attr('whatopen', 'body' + i);
        jQuery(this).addClass('head' + i);
        jQuery(this).parents('.shortcode_tabs').find('.all_heads_cont').append(this);
        i++;
    });
    var i = 1;
    jQuery('.shortcode_tab_item_body').each(function (index) {
        jQuery(this).addClass('body' + i);
        jQuery(this).addClass('it' + i);
        jQuery(this).parents('.shortcode_tabs').find('.all_body_cont').append(this);
        i++;
    });
});
jQuery('.shortcode_tabs .all_body_cont div:first-child').addClass('active');
jQuery('.shortcode_tabs .all_heads_cont div:first-child').addClass('active');

jQuery('.shortcode_tab_item_title').click(function () {
    jQuery(this).parents('.shortcode_tabs').find('.shortcode_tab_item_body').removeClass('active');
    jQuery(this).parents('.shortcode_tabs').find('.shortcode_tab_item_title').removeClass('active');
    var whatopen = jQuery(this).attr('data-open');
    jQuery(this).parents('.shortcode_tabs').find('.' + whatopen).addClass('active');
    jQuery(this).addClass('active');
});
/* Tabs */

jQuery(function ($) {
    $("#switcher-head").toggle(function () {
        $("#style-switcher").animate({
            left: 0
        }, 500);
    }, function () {
        $("#style-switcher").animate({
            left: -225
        }, 500);
    });
});

/* Twitter */
//$(".tweet").tweet({
//    modpath: 'assets/php/',
//    username: "DankovTheme",
//    avatar_size: 46,
//    count: 5,
//    loading_text: "loading twitter...",
//    template: "{avatar} {text}"
//});
//var ul = $(this).find(".tweet_list");
//ul.cycle({
//    fx: 'fade',
//    timeout: 5000,
//    sync: 0,
//    speed: 1000
//});
//$('#prev').click(function () {
//    $('.tweet_list').cycle('prev');
//});
//$('#next').click(function () {
//    $('.tweet_list').cycle('next');
//});
/* Twitter */

/* ShowBiz */
jQuery('#portfolio').showbizpro({
    visibleElementsArray: [3, 3, 2, 1],
    mediaMaxHeight: [250, 250, 250, 250],
    carousel: "on",
    heightOffsetBottom: 10
});
jQuery(".fancybox").fancybox();


jQuery('#blog').showbizpro({
    visibleElementsArray: [2, 2, 2, 1],
    mediaMaxHeight: [210, 210, 210, 210],
    carousel: "on",
    heightOffsetBottom: 10
});

jQuery('#becomeclients').showbizpro({
    //visibleElementsArray: [5, 0, 0, 0],
    visibleElementsArray: [3, 3, 2, 1],
    carousel: "on",
    heightOffsetBottom: 10,
    mediaMaxHeight: [200, 200, 200, 200],
});

jQuery('#clients').showbizpro({
    visibleElementsArray: [6, 5, 3, 2],
    carousel: "on",
    heightOffsetBottom: 10,
    mediaMaxHeight: [76, 76, 76, 76],
});
/* ShowBiz */

/* Animation */
$(window).scroll(function () {
    $(".animated-area").each(function () {
        if ($(window).height() + $(window).scrollTop() - $(this).offset().top > 0) {
            $(this).trigger("animate-it");
        }
    });
});
$(".animated-area").on("animate-it", function () {
    var cf = $(this);
    cf.find(".animated").each(function () {
        $(this).css("-webkit-animation-duration", "0.9s");
        $(this).css("-moz-animation-duration", "0.9s");
        $(this).css("-ms-animation-duration", "0.9s");
        $(this).css("animation-duration", "0.9s");
        $(this).css("-webkit-animation-delay", $(this).attr("data-animation-delay"));
        $(this).css("-moz-animation-delay", $(this).attr("data-animation-delay"));
        $(this).css("-ms-animation-delay", $(this).attr("data-animation-delay"));
        $(this).css("animation-delay", $(this).attr("data-animation-delay"));
        $(this).addClass($(this).attr("data-animation"));
    });
});
/* Animation */

/* Isotope Portfolio */
$(window).load(function () {
    var $container = $('.portfolio');
    $container.isotope({
        filter: '.first',
        layoutMode: 'sloppyMasonry',
    });
    $('.portfolioFilter a').click(function () {
        $('.portfolioFilter .current').removeClass('current');
        $(this).addClass('current');
        var selector = $(this).attr('data-filter');
        $container.isotope({
            filter: selector,
            animationOptions: {
                duration: 750,
                easing: 'linear',
                queue: false
            }
        });
        return false;
    });
});
/* Isotope Portfolio */

/* Testimonials */
$(window).load(function () {
	//try{
    //$("#tr1").testimonialrotator({
      //  settings_slideshowTime: 3
    //});
	//}
	//catch{}
});
/* Testimonials */

/* Tooltip  */
jQuery(function ($) {
    $('.tooltip_s').tooltip()
});
/* Tooltip  */

//var _gaq = _gaq || [];
//_gaq.push(['_setAccount', 'UA-41160498-1']);
//_gaq.push(['_setDomainName', 'dankov-theme.com']);
//_gaq.push(['_setAllowLinker', true]);
//_gaq.push(['_trackPageview']);
//(function () {
//    var ga = document.createElement('script');
//    ga.type = 'text/javascript';
//    ga.async = true;
//    ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
//    var s = document.getElementsByTagName('script')[0];
//    s.parentNode.insertBefore(ga, s);
//})();