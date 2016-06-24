<%@ Control Language="c#" %>

<%@ Register Assembly="Appleseed.Framework.Core" Namespace="Appleseed.Framework.Web.UI.WebControls" TagPrefix="rbfwebui" %>
<%@ Register Assembly="Appleseed.Framework.Web.UI.WebControls" Namespace="Appleseed.Framework.Web.UI.WebControls" TagPrefix="rbfwebui" %>

<script runat="server">
    private void Page_Load( object sender, System.EventArgs e ) {
        PortalImage.DataBind();
    }
</script>

<!-- Fat Footer -->
<div id="footer">
  
    <!-- 960 Container -->
    <div class="container">
    
      <!--  Text Widget - 1/3 Columns -->
      <div class="one-third columns social">
        <h4>Join the Community!</h4>
        <div class="address"><img style="padding-top: 3px" src="/Portals/_Appleseed/images/Onebillion/mail.png" alt=""/><strong>Contact Us: </strong><a href="mailto:contact@one-billion-strong.com">contact@one-billion-strong.org</a></div>
        <p class="social">Follow us: </p>
        <a href="http://www.facebook.com/OneBillionStrong"><img src="/Portals/_Appleseed/images/Onebillion/social/facebook-32.png" alt=""/></a>
        <a href="http://twitter.com/@OneBStrong"><img src="/Portals/_Appleseed/images/Onebillion/social/twitter-32.png" alt=""/></a>
        <a href="http://uk.linkedin.com/pub/one-billion-strong/56/48b/b7b"><img src="/Portals/_Appleseed/images/Onebillion/social/linkedin-32.png" alt=""/></a>
        <!--<ul class="social">
          <li class="facebook"><a href="#"></a></li>
          <li class="twitter"><a href="#"></a></li>
          <li class="linked"><a href="#"></a></li>
          <li class="vimeo"><a href="#"></a></li>
          <li class="flickr"><a href="#"></a></li>
        </ul>-->
      </div>
      
      <!--  Contact - 1/3 Columns -->
      <div class="one-third columns">
        <h4>Publications and Multimedia</h4>
        <ul class="pubs">
          <li class="pub"><a href="/onebillion/site/297">HIV/AIDS, Disability and Discrimination</a></li>
          <li class="pub"><a href="/onebillion/site/298">Shadow Report Guide</a></li>
          <li class="pub"><a href="/onebillion/site/299">Human Rights Yes!</a></li>
        </ul>
        <ul class="multimedia">
          <li class="mm"><a href="/onebillion/site/300">Stimson Center HIV Guide Video</a></li>
        </ul>
      </div>
      
      <!--  Latest Tweets - 1/3 Columns -->
      <div class="one-third columns">
        <h4>Events</h4>
        <ul class="events">
          <li class="event"><a href="#">Event1</a></li>
          <li class="event"><a href="#">Event2</a></li>
        </ul>
        <div id="footer-logo">
          <!-- Portal Logo Image Uploaded-->
              <rbfwebui:headerimage id="PortalImage" runat="server" enableviewstate="false"/>
          <!-- End Portal Logo-->
        </div>
      </div>
      
    </div><!-- End 960 Container -->
</div><!-- End Fat Footer -->
  
<!--  Footer - Copyright-->
    <div id="footer_bottom">
      <!-- 960 Container -->
      <div class="container">
        
        <div class="sixteen columns">
          <div class="copyright">© Copyright 2012 by <span>One Billion Strong</span>. All Rights Reserved.</div>
        </div>
        
      </div><!-- End 960 Container -->
    </div><!--  End Footer - Copyright-->
    
    <!-- Back To Top Button -->
    <div id="backtotop"><a href="#"></a></div>
    
    <!-- Imagebox Build -->
    <script src="js/imagebox.build.js"></script>