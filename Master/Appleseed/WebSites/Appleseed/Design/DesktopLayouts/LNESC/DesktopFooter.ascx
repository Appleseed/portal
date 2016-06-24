<%@ Control Language="c#" %>

<div id="footer">
    <div id="footer-content">
        <div id="quick-links">
          <h3>Quick links</h3>
          <ul>
              <li><a href="/292">About us</a></li>
              <li><a href="/336">Partners</a></li>
              <li><a href="/356">News</a></li>
              <li><a href="/295">Resources</a></li>
            </ul>
        </div>
        <div id="get-moving">
          <h3>Get moving</h3>
          <ul>
              <li><a href="/296">Scholarships</a></li>
              <li><a href="/294">Programs</a></li>
              <li><a href="/293">Centers</a></li>
            </ul>
        </div>
        <div id="follow">
          <h3>Follow us</h3>
          <ul>
              <li id="twitter"><a href="https://twitter.com/#!/LNESC"><img runat="server" src="~/Portals/_Appleseed/images/global/footer/twitter.png" width="35" height="24" border="0" 
                  alt="Follow LNESC on twitter" title="Follow LNESC on twitter"></a></li>
              <li><a href="https://www.facebook.com/LNESC"><img  runat="server" src="~/Portals/_Appleseed/images/global/footer/facebook.png" width="24" height="24" border="0" 
                  alt="Like LNESC on facebook" title="Like LNESC on facebook"></a></li>
            </ul>
        </div>
        <div id="twitter-feed">
          <h3>From Twitter</h3>
          <div id="feed">
            <script src="http://widgets.twimg.com/j/2/widget.js"></script>
            <script>

            new TWTR.Widget({
              version: 2,
              type: 'profile',
              rpp: 1,
              interval: 6000,
              title: '@lnesc',
              subject: 'LNESC on Twitter',
              width: 275,
              height: 160,
              footer: "Follow LNESC",
              theme: {
                shell: {
                  background: 'transparent',
                  color: '#ffffff'
                },
                tweets: {
                  background: 'transparent',
                  color: '#ffffff',
                  links: '##3FBFEA'
                }
              },
              features: {
                scrollbar: false,
                loop: false,
                live: true,
                hashtags: true,
                timestamp: true,
                avatars: false,
                behavior: 'all'
              }
            }).render().setUser('lnesc').start();
            </script>


            </div>
        </div>
        <div id="action-calls">
          <h3>Calls to action</h3>
          <ul>
              <li id="footer-donate"><a class="cta-lightbox" data-fancybox-type="iframe" href="http://fundly.com/nnrd7w6o/widgets/card">Donate now</a></li>
              <li><a class="cta-lightbox" data-fancybox-type="iframe" href="http://fundly.com/nnrd7w6o/widgets/card">Support us</a></li>
              <li><a class="cta-lightbox fancybox.iframe" href="http://eepurl.com/mr0AD">Get involved</a></li>
              <li><a class="cta-lightbox fancybox.iframe" href="http://eepurl.com/mr0AD">Contact us</a></li>
            </ul>
        </div>
    </div>
    <div id="address"><p>LNESC, National Headquarters  |  1133 19th Street, NW  |  Suite 1000  |  Washington, DC 20036 | Copyright &copy;2012 LNESC, LULAC National Educational Services Centers, Inc.<br /><a href="http://www.lulac.org"><img  runat="server" src="~/Portals/_Appleseed/images/global/footer/lulac-shield.png" width="30" height="40" border="0" alt="LNESC is the education arm of the League of United Latin American Citizens (LULAC)" title="LNESC is the education arm of the League of United Latin American Citizens (LULAC)"></a><br/>
      © Copyright LNESC - Powered by <a style="color:#3FBFEA;" href="http://www.appleseedportal.net">Appleseed Portal</a> - Implemented by <a style="color:#3FBFEA;" href="http://www.anant.us">Anant</a>
    </p>
    </div>
</div>


      
