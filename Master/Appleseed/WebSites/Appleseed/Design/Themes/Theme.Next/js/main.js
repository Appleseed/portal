'use strict';


jQuery('.toggle-nav').click(function() {
    jQuery(this).toggleClass('open');
    jQuery('.mobile-nav').toggleClass('open');
    if (jQuery('.mobile-nav').hasClass('open')) {
        jQuery('.header').addClass('transparent');
    }
    else {
        jQuery('.header').removeClass('transparent');
    }
});

jQuery('.mobile-nav a').click(function() {
    jQuery('.toggle-nav, .mobile-nav').toggleClass('open');
    jQuery('.header').removeClass('transparent');
});