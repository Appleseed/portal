<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EvoDicoPanel.aspx.cs" Inherits="Appleseed.DesktopModules.CoreModules.Evolutility_Wizard.EvoDicoPanel" %>

<%@ Register Assembly="Evolutility.UIServer" Namespace="Evolutility" TagPrefix="EVOL" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
<link rel="stylesheet" type="text/css" href="/aspnet_client/evolutility/css/evol.css" id="evolcss" runat="server" />
<link rel="stylesheet" type="text/css" href="/aspnet_client/evolutility/css/evolwiz.css" id="evolwizcss" runat="server"/>

</head>
<body>
    <form id="form1" runat="server">
    <div>
    <h1>Panels</h1>
			<p><EVOL:UIServer id="evoPanel" runat="server"  DataIsMetadata=true
					 ToolbarPosition="Top" Width="100%" CssClass="main1" 
					BackColor="#EDEDED" BackColorRowMouseOver="Beige"
					 UserComments="None" SqlConnection=""
					XMLfile="/aspnet_client/evolutility/XML/evoDico_Panel.xml" 
					 VirtualPathToolbar="/aspnet_client/evolutility/PixEvo/"
					 VirtualPathPictures="/aspnet_client/evolutility/PixEvo/"
					 DBAllowSelections="false" 
					 SecurityModel="Single_User" SecurityKey="EvoDico"
					DBAllowDelete="true" DisplayModeStart="List" ShowTitle="true" DBAllowExport="True" RowsPerPage="20" /></p>
    </div>
    </form>
</body>
</html>
