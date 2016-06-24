(function($){
      
  $(document).ready(function(){
  //////////////////////////////////////////////////////////////////////////////////////////////////////////////               
    
    // -------------------------------------------------------------------------------------------------------
    // Cufon - font replacement
    // -------------------------------------------------------------------------------------------------------
    
    if ( ! ( $.browser.msie && ($.browser.version == 6) ) ){ // only apply cufon on modern browsers
    
     // Cufon.replace('h1, h2, h3, h4, h5, h6, #dropdown-menu li a', {hover: true});
    }
    
    // -------------------------------------------------------------------------------------------------------
    // DD_belated - IE6 png transparency fix
    // -------------------------------------------------------------------------------------------------------
    
    if ( $.browser.msie ){
      if ( $.browser.version == 6 ){ // only apply the png transparency fix for Internet Explorer 6
        
        //DD_belatedPNG.fix('#logo img');
        //DD_belatedPNG.fix('.errormsg, .successmsg, .infomsg, .noticemsg. .pdf');
        //DD_belatedPNG.fix('#index-slideshow-pager a, #index-slideshow-pager .activeSlide');
        
      }
    }
    
    // -------------------------------------------------------------------------------------------------------
    // Tipsy - facebook like tooltips jQuery plugin
    // -------------------------------------------------------------------------------------------------------
    
  //  $('.tip').tipsy({gravity: 'w', fade: true});
    
    // -------------------------------------------------------------------------------------------------------
    // pretyPhoto - jQuery lightbox plugin
    // -------------------------------------------------------------------------------------------------------
    
    //$("a[rel^='prettyPhoto']").prettyPhoto({
    //  opacity: 0.80,             // Value between 0 and 1
    //  default_width: 500,
    //  default_height: 344,
    //  theme: 'light_square',         // light_rounded / dark_rounded / light_square / dark_square / facebook 
    //  hideflash: false,           // Hides all the flash object on a page, set to TRUE if flash appears over prettyPhoto 
    //  modal: false             // If set to true, only the close button will close the window 
    //});
    
    // -------------------------------------------------------------------------------------------------------
    // Cycle - slider jQuery plugin 
    // -------------------------------------------------------------------------------------------------------
    
    //$('#slideshow-index ul').cycle({
    //  timeout: 5000,// milliseconds between slide transitions (0 to disable auto advance)
    //  fx: 'fade',// choose your transition type, ex: fade, scrollUp, shuffle, etc...            
    //  pager: '#index-slideshow-pager',// selector for element to use as pager container
    //  delay: 0, // additional delay (in ms) for first transition (hint: can be negative)
    //  speed: 1000,  // speed of the transition (any valid fx speed value) 
    //  pause: true,// true to enable "pause on hover"
    //  cleartypeNoBg: true,// set to true to disable extra cleartype fixing (leave false to force background color setting on slides)
    //  pauseOnPagerHover: 0 // true to pause when hovering over pager link
    //});
    
    //$('#slideshow-portfolio ul').cycle({
    //  timeout: 5000,// milliseconds between slide transitions (0 to disable auto advance)
    //  fx: 'fade',// choose your transition type, ex: fade, scrollUp, shuffle, etc...            
    //  pager: '#portfolio-slideshow-pager',// selector for element to use as pager container
    //  delay: 0, // additional delay (in ms) for first transition (hint: can be negative)
    //  speed: 1000,  // speed of the transition (any valid fx speed value) 
    //  pause: true,// true to enable "pause on hover"
    //  cleartypeNoBg: true,// set to true to disable extra cleartype fixing (leave false to force background color setting on slides)
    //  pauseOnPagerHover: 0 // true to pause when hovering over pager link
    //});

    
    // -------------------------------------------------------------------------------------------------------
    //  Tabify - jQuery tabs plugin
    // -------------------------------------------------------------------------------------------------------
    
    //$('#tab-1').tabify();
    //$('#skills-tab').tabify();
    
    // -------------------------------------------------------------------------------------------------------
    //  Accordeon - jQuery accordeon plugin
    // -------------------------------------------------------------------------------------------------------
    
    $('#accordion-1').accordion();

  //////////////////////////////////////////////////////////////////////////////////////////////////////////////  
  });

})(window.jQuery);

// non jQuery plugins below

///////////////////////////////// CTA / CONVERSION FORMS /////////////////////////////////////////////////////////////////////



$(document).ready(function () {
  
  
  
  
  // CTA Form / Subscribe to Newsletter ( Bottom ) 
  
  $(".subscribe-submit-btn").click(function(){
    url = "http://anant.us1.list-manage.com/subscribe?u=d92549071121954997db2d1e1&id=d05aef7418";
    fname = $('#subscribe-firstname').val();
    lname = $('#subscribe-lastname').val();
    email = $('#subscribe-email').val();
    newurl = url+"&FNAME="+fname+"&LNAME="+lname+"&EMAIL="+email;
    //window.open(newurl,'mywindow','width=700,height=700');
    $(".subscribe-submit-btn").fancybox({
        autoDimensions: false,
        height: 700,
        width: 700,
        href: newurl,
    });

    mixpanel.track("CTA Button Click",
                   {
                     "page"  : $(document).attr('title'),
                     "module": "Subscribe to Newsletter",
                     "context": "Footer"
                   });    
    
  });


  
  // CTA Form / We'll Call you 
  $("#cta-button-call").click(function(){
    url = "https://appleseeds.wufoo.com/forms/z7x1z5/def/";
    phone = $('#cta-call-phone').val();
    email = $('#cta-call-email').val();
    name  = $('#cta-call-name').val();
    newurl = url+"&field12="+name+"&field3="+email+"&field4="+phone;
    //window.open(newurl,'mywindow','width=700,height=500');
    $("#cta-button-call").fancybox({
        autoDimensions: false,
        height: 500,
        width: 700,
        href: newurl,
    });

    mixpanel.track("CTA Button Click",
                   {
                     "page"  : $(document).attr('title'),
                     "module": "We'll Call You",
                     "context": "Content"
                   });
  });
  
  // CTA Form / Ask us a Question 
  $("#cta-button-answer").click(function(){
    url = "https://appleseeds.wufoo.com/forms/z7x1z5/def/";
    email = $('#cta-answer-email').val();
    note  = $('#cta-answer-question').val();
    newurl = url+"&field5="+note+"&field3="+email;
    //window.open(newurl,'mywindow','width=700,height=500');
    $("#cta-button-answer").fancybox({
        autoDimensions: false,
        height: 500,
        width: 700,
        href: newurl,
    });

    mixpanel.track("CTA Button Click", 
                   {
                     "page"  : $(document).attr('title'),
                     "module": "Ask a Question",
                     "context": "Content"
                   });      
  });

  // CTA Form / Subscribe to our Research
  
  $("#subscribe-research").click(function () {
      
      url = "http://anant.us1.list-manage.com/subscribe?u=d92549071121954997db2d1e1&id=d05aef7418";
      email = $('#cta-subscribe-email').val();
      newurl = url+"&EMAIL="+email;
      //window.open(newurl,'mywindow','width=700,height=700');
      $("#subscribe-research").fancybox({
          autoDimensions: false,
          height: 700,
          width: 700,
          href: newurl,
      });

      mixpanel.track("CTA Button Click", 
                     {
                       "page"  : $(document).attr('title'),
                       "module": "Subscribe to Research",
                       "context": "Content"
                     });        
  });
  

   // CTA Form / Become a Client
  

  
    $("#cta-button-become").click(function(){
      mixpanel.track("CTA Button Click", 
                     {
                       "page"  : $(document).attr('title'),
                       "module": "Become a Client",
                       "context": "Content"
                     });        
  });
  

});

///////////////////////////////// MIXPANEL /////////////////////////////////////////////////////////////////////

// start Mixpanel 
(function(e,b){if(!b.__SV){var a,f,i,g;window.mixpanel=b;a=e.createElement("script");a.type="text/javascript";a.async=!0;a.src=("https:"===e.location.protocol?"https:":"http:")+'//cdn.mxpnl.com/libs/mixpanel-2.2.min.js';f=e.getElementsByTagName("script")[0];f.parentNode.insertBefore(a,f);b._i=[];b.init=function(a,e,d){function f(b,h){var a=h.split(".");2==a.length&&(b=b[a[0]],h=a[1]);b[h]=function(){b.push([h].concat(Array.prototype.slice.call(arguments,0)))}}var c=b;"undefined"!==
typeof d?c=b[d]=[]:d="mixpanel";c.people=c.people||[];c.toString=function(b){var a="mixpanel";"mixpanel"!==d&&(a+="."+d);b||(a+=" (stub)");return a};c.people.toString=function(){return c.toString(1)+".people (stub)"};i="disable track track_pageview track_links track_forms register register_once alias unregister identify name_tag set_config people.set people.set_once people.increment people.append people.track_charge people.clear_charges people.delete_user".split(" ");for(g=0;g<i.length;g++)f(c,i[g]);
b._i.push([a,e,d])};b.__SV=1.2}})(document,window.mixpanel||[]);
mixpanel.init("ec5a0a18f08944ba5b96936516abd711");
// end Mixpanel 



$(document).ready(function () {
  

   /////////////////// GLOBAL ELEMENTS ///////////////////////////
 
  // Header Links
   mixpanel.track_links('header #Banner_biMenu ul li a', 'Header Link', function(link){
    return {
      'action': $(link).text(),
      'category': 'Navigation',
      'context': 'Header'     
      }
    });
  
  // Footer Links
   mixpanel.track_links('#footer a', 'Footer Link', function(link){
    return {
      'action': $(link).text(),
      'category': 'Navigation',
      'context': 'Footer'     
      }
    });
  
    // Calls to Action
  /* not working 
   mixpanel.track_links('a#cta-button-call, a#cta-button-answer, a#cta-button-subscribe, a#cta-button-become', 'CTA Link', function(link){
    return {
      'action': $(link).text(),
      'category': 'CTA',
      'context': 'Conversion Form'     
      }
    });*/
  
    /////////////////// HOME PAGE ///////////////////////////
  
  
  // Home Page Banners
  mixpanel.track_links("a[rel='prettyPhoto']", 'Iframe Link', function(link){
    return {
      'action': $(link).attr('title'),
      'category': 'Showcase',
      'context': 'Content'     
      }
    });
 
  
  //////////////  -- not yet implemented - RXS 9/22/3013
 
  
    /*
  // Company page Images
   mixpanel.track_links('.hot-items .item-img a', 'Hot Item Link', function(link){
    return {
      'action': $(link).attr('title'),
      'category': 'Feature',
      'context': 'Content'     
      }
    });
  */
  
  /*
   // Service page Links
   mixpanel.track_links('.hot-items portfolio-item-meta a', 'Hot Item Link', function(link){
    return {
      'action': $(link).text(),
      'category': 'Feature',
      'context': 'Content'     
      }
    });
  */
 
  /*
   // Product page Links
   mixpanel.track_links('.hot-items portfolio-item-meta a', 'Hot Item Link', function(link){
    return {
      'action': $(link).text(),
      'category': 'Feature',
      'context': 'Content'     
      }
    });
  */
  
  //////////////////////////// Global  //////////////////
  
  // Page View
   var page_location = window.location.href.toString().split(window.location.host)[1];
   mixpanel.track_pageview(page_location);
  
});
