/*
-------------------------------------------
  STICKY NAVIGATION
-------------------------------------------
*/

var nav_container = $("#sticky-header");
var nav = $("#sticky-header");
    
    var top_spacing = 0;
    var waypoint_offset = 0;
  
    nav_container.waypoint({
      handler: function(event, direction) {
        if (direction == 'down') {
          if (matchMedia('only screen and (min-width: 1000px)').matches) {
            nav_container.css({ 'height':nav.outerHeight() });    
            nav.stop().addClass("sticky").css("top",-nav.outerHeight()).animate({"top":top_spacing});
          }
        } else {
        
          nav_container.css({ 'height':'auto' });
          nav.stop().removeClass("sticky").css("top",nav.outerHeight()+waypoint_offset).animate({"top":""}, 0);
          
        }
        
      },
      offset: function() {
        return -nav.outerHeight()-waypoint_offset;
      }
    });
    
    var sections = $("section");
    var navigation_links = $("nav a");
    
    sections.waypoint({
      handler: function(event, direction) {
      
        var active_section;
        active_section = $(this);
        if (direction === "up") active_section = active_section.prev();
  
        var active_link = $('nav a[href="#' + active_section.attr("id") + '"]');
        navigation_links.removeClass("selected");
        active_link.addClass("selected");
  
      },
      offset: '95%'
    }, function(){ $.waypoints("refresh"); });
