<%@ Control Language="c#" %>
<%@ Register Assembly="Appleseed.Framework.Core" Namespace="Appleseed.Framework.Web.UI.WebControls" TagPrefix="rbfwebui" %>
<%@ Register Assembly="Appleseed.Framework.Web.UI.WebControls" Namespace="Appleseed.Framework.Web.UI.WebControls" TagPrefix="rbfwebui" %>
<script runat="server">
    private void Page_Load( object sender, System.EventArgs e ) {
        PortalHeaderMenu.DataBind();
        PortalTitle.DataBind();
        PortalImage.DataBind();

        if(Appleseed.Framework.Security.PortalSecurity.IsInRoles("Admins")){
          asAdminBarPanel.Visible = true;
        }
    }
</script>
<asp:Panel id="asAdminBarPanel" runat="server" visible="false">

<script type="text/javascript">
  // move the page down a little bit because of the admin bar. 
  $(document).ready(function(){
    $("#header").css("margin-top","30px");
  });
</script>

<div id="as-admin-bar" >
      <div class="quicklinks">
        <ul>                          
        <li id="admin-avatar" class="menupop">
          <a href="<%= Appleseed.Framework.Settings.Path.ApplicationFullPath %>"><span><img alt='Appleseed' src='<%=Appleseed.Framework.Settings.Path.ApplicationFullPath  %>/Design/Themes/LNESC/images/brick.png' class='avatar avatar-32 photo' height='32' width='32' />Appleseed Portal</span></a>
        
        </li>
                <li class="menupop">
                   <a href="<%= Appleseed.Framework.Settings.Path.ApplicationFullPath %>/100"><span>Administration</span></a>
                    <ul>        
                <li class=""></li>                      
                        <li class=""><a href="<%=Appleseed.Framework.Settings.Path.ApplicationFullPath  %>/240">Site Settings</a></li>                                
                        <li class=""><a href="<%=Appleseed.Framework.Settings.Path.ApplicationFullPath  %>/110">Page Manager</a></li>                                
                        <li class=""><a href="<%=Appleseed.Framework.Settings.Path.ApplicationFullPath  %>/281">User Manager</a></li>
                        <li class=""><a href="<%=Appleseed.Framework.Settings.Path.ApplicationFullPath  %>/307">Global Modules</a></li>
                        <li class=""><a href="<%=Appleseed.Framework.Settings.Path.ApplicationFullPath  %>/215">Recycle Bin</a></li>
                        <li class=""><a href="http://file.app.clients.anant.us">File.App</a></li>                       
                </ul>       
                </li>

                <div id="adminbarsearch-wrap" class="userMenu">
                   <rbfwebui:HeaderMenu   ID="PortalHeaderMenu" runat="server" 
                              CssClass="SiteLink" RepeatDirection="Horizontal" cellspacing="0"
                              CellPadding="0" ShowHelp="False" ShowHome="False" 
                              ShowLogon="true" ShowRegister="false" ShowDragNDrop="True"
                                                DialogLogon="true" ShowLanguages="false" ShowFlags="false" ShowLangString="false" >
                    <ItemStyle Wrap="False"></ItemStyle>
                    <ItemTemplate>
                              <li><%# Container.DataItem %></li>                
                    </ItemTemplate>        
                      <SeparatorTemplate>
                      </SeparatorTemplate>        
                  </rbfwebui:HeaderMenu>                          
                </div>
            </div>
  </div>
                      

     <%-- <!--Begin menu 
            <div class="contenedor_menu">
            
                Framew
                <div class="menu_border_left"></div>
                <asp:Menu   ID="biMenu"  runat="server" 
                    DataSourceID="biSMDS" 
                    Orientation="Vertical"
                    CssClass="menu" 
                    DynamicEnableDefaultPopOutImage="False" 
                    StaticEnableDefaultPopOutImage="False">                                
                </asp:Menu>
                <div class="menu_border_right"></div>
            </div>
            End Portal Menu -->
         
         --%>
  </asp:Panel>

<div id="header" >
  <div id="utility-bar">
         <div id="search">
            <div id="search-label">Search</div>
            <!-- <form method="get" action="http://www.google.com/search" id="searchForm">   -->
                <input name="q" class="userInput" type="text" onfocus =" if (this.value == ' Search this site') this.value = '';"
                  onblur =" if (this.value == '') this.value = ' Search this site';" value=" Search this site" size="20" />
                <input name="searchSubmitButton" class="submitButton" type="submit"  value="" />
            <!-- </form>  -->
            <script type="text/javascript">
                searchurl = "<%=Appleseed.Framework.Settings.Path.ApplicationFullPath %>/308";  //<-- point to the page that has the search elements
                $(document).ready(function () {
                    $("#searchSubmitButton").click(function () {
                        var search = $('#searchQuery').val();                        
                        window.location.replace(searchurl + "#q=" + search);
                    });
                });
            </script>
        </div>
        <div id="topnav">
            <ul id="utility">
                <li id="about"><a href="/292">About Us</a></li>
              <li id="partners"><a href="/336">Partners</a></li>
              <li id="news"><a href="/330">News</a></li>
              <li id="resources"><a href="/295">Resources</a></li>
            </ul>
            <ul id="action">
              <li id="share"><a href="#">Share</a>
                    <div class="subnav">
                       <div id="share-sub">
                            <a href="https://twitter.com/#!/LNESC"><img id="Img1" src="~/Portals/_Appleseed/images/global/footer/twitter.png"
                              width="35" height="24" border="0" alt="Follow LNESC on twitter" title="Follow LNESC on twitter" runat="server"></a>
                            <a href="https://twitter.com/#!/LNESC"><img id="Img2" src="~/Portals/_Appleseed/images/global/footer/facebook.png"
                              width="24" height="24" border="0" alt="Follow LNESC on twitter" title="Follow LNESC on facebook" runat="server"></a>
                        </div>
                    </div>
                </li>
              <li id="support"><a href="/343">Support us</a>
                    <div class="subnav">
                        <div id="support-sub">
                            <!--<a class="cta-lightbox" data-fancybox-type="iframe" href="http://fundly.com/nnrd7w6o/widgets/card" >-->
                            <p><a  href="/343" >Support LNESC</a>. <br />LNESC serves over 11,000 high-need students a year. Learn how you can be a part of this impact.</p>
                        </div>
                    </div>
</li>
              <li id="donate"><a href="/344">Donate now</a>
                    <div class="subnav">
                        <div id="donate-sub">
                        <!--<a class="cta-lightbox" data-fancybox-type="iframe" href="http://fundly.com/nnrd7w6o/widgets/card" >-->
                            <p><a  href="/343" >Give now and change lives</a>. <br />From $10 to $100, every dollar provides essential services for high-need youth.</p>
                        </div>
                    </div>
        </li>
            </ul>
        </div>
  </div>
   <div id="logo-bar">
        <div id="logo">
          <a href="<%= Appleseed.Framework.Settings.Path.ApplicationFullPath %>"><rbfwebui:headerimage id="PortalImage" runat="server" enableviewstate="false" /></a>
          <rbfwebui:headertitle id="PortalTitle" runat="server" cssclass="SiteTitle" enableviewstate="false" visible="false"></rbfwebui:headertitle>
        </div>
        <div id="logobar-nav">
          <ul>
              <li><a href="/296">Scholarships</a></li>
              <li><a href="/294">Programs</a></li>
              <li><a href="/293">Education Centers</a></li>
              <li><a class="cta-lightbox fancybox.iframe" href="http://eepurl.com/mr0AD" target="_blank">Get involved</a></li>
            </ul>
        </div>
    </div>
</div>
</div>


<script type="text/javascript">
  $(document).ready(function() {
    $(".cta-lightbox").fancybox({      
      autoSize  : true,
      closeClick  : true,
      openEffect  : 'none',
      closeEffect : 'none'
    });
  });
        
</script>
  


