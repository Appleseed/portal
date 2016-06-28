<%@ Control Language="c#" %>
<%@ Register Assembly="Appleseed.Framework.Core" Namespace="Appleseed.Framework.Web.UI.WebControls" TagPrefix="rbfwebui" %>
<%@ Register Assembly="Appleseed.Framework.Web.UI.WebControls" Namespace="Appleseed.Framework.Web.UI.WebControls" TagPrefix="rbfwebui" %>
<script runat="server">

    public string ContentContainerSelector;
    private void Page_Load( object sender, System.EventArgs e ) {
        PortalHeaderMenu.DataBind();

        if(Appleseed.Framework.Security.PortalSecurity.IsInRoles("Admins")){
          BarPanel.Visible = true;
        }
    }
</script>

<asp:Panel id="BarPanel" runat="server" visible="false">

<script type="text/javascript">
  // move the page down a little bit because of the admin bar. 
  $(document).ready(function(){
    $("<%=ContentContainerSelector%>").css("margin-top","30px");    
  });
</script>

<div id="as-admin-bar" >
      <div class="quicklinks">
        <ul>                          
        <li id="admin-avatar" class="menupop">
          <a href="<%= Appleseed.Framework.Settings.Path.ApplicationFullPath %>"><span><img alt='Appleseed' src='<%=Appleseed.Framework.Settings.Path.ApplicationFullPath  %>/Design/Themes/AHRI/images/brick.png' class='avatar avatar-32 photo' height='32' width='32' />Appleseed Portal</span></a>
        
        </li>
                <li class="menupop">
                   <a href="<%= Appleseed.Framework.Settings.Path.ApplicationFullPath %>/100"><span>Administration</span></a>
                    <ul>        
                <li class=""></li>                      
                        <li class=""><a href="<%=Appleseed.Framework.Settings.Path.ApplicationFullPath  %>/240">Site Settings</a></li>                                
                        <li class=""><a href="<%=Appleseed.Framework.Settings.Path.ApplicationFullPath  %>/110">Page Manager</a></li>                                
                        <li class=""><a href="<%=Appleseed.Framework.Settings.Path.ApplicationFullPath  %>/281">User Manager</a></li>
                        <li class=""><a href="<%=Appleseed.Framework.Settings.Path.ApplicationFullPath  %>/120">Global Modules</a></li>
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
                      

    
  </asp:Panel>

 