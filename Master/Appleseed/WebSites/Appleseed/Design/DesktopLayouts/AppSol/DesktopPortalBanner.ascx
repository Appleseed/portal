<%@ Control Language="c#" %>
<%@ Register Assembly="Appleseed.Framework.Core" Namespace="Appleseed.Framework.Web.UI.WebControls" TagPrefix="rbfwebui" %>
<%@ Register Assembly="Appleseed.Framework.Web.UI.WebControls" Namespace="Appleseed.Framework.Web.UI.WebControls" TagPrefix="rbfwebui" %>
<script runat="server">
    private void Page_Load( object sender, System.EventArgs e ) {
        PortalHeaderMenu.DataBind();
		PortalTitle.DataBind();
        PortalImage.DataBind();
    }
</script>


<!-- HEADER -->
<div id="outerheader">
    <div class="container header-wrapper">
        <header id="top" class="twelve columns">
    
	    <div class="userMenu">
		    <!-- begin User Menu at the Top of the Page -->
			    <rbfwebui:HeaderMenu 	ID="PortalHeaderMenu" runat="server" 
					CssClass="SiteLink" RepeatDirection="Horizontal" cellspacing="0"
					CellPadding="0" ShowHelp="False" ShowHome="False" 
					ShowLogon="true" ShowRegister="true" ShowDragNDrop="True"
					DialogLogon="true" >
				    <ItemStyle Wrap="False"></ItemStyle>
				    <ItemTemplate>
					    <!-- used to stylize the left border ex: border with images-->
						    <div class="SiteLink_Border_Left"></div>
					    <!-- End left border -->
						<div class="SiteLink_bg">
							<span class="SiteLink">
								<%# Container.DataItem %>
							</span>
						</div>
					    <!-- used to stylize the right border-->
						    <div class="SiteLink_Border_Right"></div>
					    <!-- End right border -->
				    </ItemTemplate>
				    
					    <SeparatorTemplate>
						    <span class="SiteLink">&nbsp;|&nbsp;</span>
					    </SeparatorTemplate>
				    
			    </rbfwebui:HeaderMenu>
		    <!-- End User Menu -->
	    </div>

	    <div id="logo"  class="six columns alpha">
		    <!-- Portal Logo Image Uploaded-->
			    <rbfwebui:headerimage id="PortalImage" runat="server" enableviewstate="false"/>
		    <!-- End Portal Logo-->
		    <!-- Portal Title -->
			    <!--<rbfwebui:headertitle id="PortalTitle" runat="server" cssclass="SiteTitle" enableviewstate="false"></rbfwebui:headertitle>-->
		    <!-- End Portal Title -->
		    <h4 class="tagline">Local Governments Building A Clean Economy</h4>
	    </div>
	    <div id="headerright" class="six columns omega">
		
		<div class="search">
		    <input name="s" id="s" size="35" class="userInput" type="text" onfocus =" if (this.value == ' Search this site') this.value = '';"
		      onblur =" if (this.value == '') this.value = ' Search this site';" value=" Search this site"  />
		    <input name="searchSubmitButton" class="submitButton" type="submit" value="Search" />
		</div>
	    
	    </div>
    
	    <section id="navigation" class="main_menu twelve columns">
		<nav id="nav-wrap">
		    <div id="topnav" class="sf-menu">
			<!-- Begin Portal Menu -->
			    <asp:Menu 	ID="biMenu"	runat="server" 
				    DataSourceID="biSMDS" 
				    Orientation="Horizontal"
				    CssClass="menu" 
				    DynamicEnableDefaultPopOutImage="False" 
				    StaticEnableDefaultPopOutImage="False">                                
			    </asp:Menu>
			<!-- End Portal Menu -->
		    </div>
		</nav><!-- nav -->	
		<div class="clear"></div>
	    </section>
	    <div class="clear"></div>
        </header>
    </div>
</div>
<!-- END HEADER -->


<asp:SiteMapDataSource ID="biSMDS" ShowStartingNode="false" runat="server" />
