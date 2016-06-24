<!-- Fat Footer -->
<div id="footer">
  
    <!-- 960 Container -->
    <div class="container">
    
      <!--  Text Widget - 1/4 Columns -->
      <div class="four columns">
        <div id="footer-logo"><img src="/onebillion/Portals/_Appleseed/images/Sensation/logo.png" alt="" /></div>
        <ul class="social">
          <li class="facebook"><a href="#"></a></li>
          <li class="twitter"><a href="#"></a></li>
          <li class="vimeo"><a href="#"></a></li>
          <li class="linked"><a href="#"></a></li>
          <li class="flickr"><a href="#"></a></li>
          </ul>
      </div>
      
      <!--  Contact - 1/4 Columns -->
      <div class="four columns">
        <h4>Contact Details</h4>
        <div class="address"><img style="padding-top: 1px" src="/onebillion/Portals/_Appleseed/images/Sensation/address.png" alt="" /><strong>Address:</strong> Aleje Jerozolimskie 30, Warsaw, Poland</div>
        <div class="address"><img style="padding-top: 1px" src="/onebillion/Portals/_Appleseed/images/Sensation/phone.png" alt=""/><strong>Phone:</strong> +44 (0) 560-655-995</div>
        <div class="address"><img style="padding-top: 3px" src="/onebillion/Portals/_Appleseed/images/Sensation/mail.png" alt=""/><strong>Email:</strong><a href="#"> contact@sensation.com</a></div>
      </div>
      
      <!--  Latest Tweets - 1/4 Columns -->
      <div class="four columns">
        <h4>Latest Tweets</h4>
        <ul id="twitter"></ul>
          <script type="text/javascript">
            jQuery(document).ready(function($){
            $.getJSON('http://api.twitter.com/1/statuses/user_timeline/envato.json?count=2&callback=?', function(tweets){
            $("#twitter").html(tz_format_twitter(tweets));
            }); });
          </script>
        <div class="clearfix"></div>
      </div>
      
      <!--  photo Stream - 1/4 Columns -->
      <div class="four columns">
        <h4>Photo Stream</h4>
        <div class="flickr-widget">
          <script type="text/javascript" src="http://www.flickr.com/badge_code_v2.gne?count=8&amp;display=latest&amp;size=s&amp;layout=x&amp;source=user&amp;user=36587311@N08"></script>
          <div class="clearfix"></div>
        </div>
      </div>
      
    </div><!-- End 960 Container -->
</div><!-- End Fat Footer -->
  
<!--  Footer - Copyright-->
    <div id="footer_bottom">
      <!-- 960 Container -->
      <div class="container">
        
        <div class="sixteen columns">
          <div class="copyright">© Copyright 2012 by <span>Sensation</span>. All Rights Reserved.</div>
        </div>
        
      </div><!-- End 960 Container -->
    </div><!--  End Footer - Copyright-->
    
    <!-- Back To Top Button -->
    <div id="backtotop"><a href="#"></a></div>
    
    <!-- Imagebox Build -->
    <script src="js/imagebox.build.js"></script>