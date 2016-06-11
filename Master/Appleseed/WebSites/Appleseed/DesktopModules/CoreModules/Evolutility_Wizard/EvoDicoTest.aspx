<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EvoDicoTest.aspx.cs" Inherits="Appleseed.DesktopModules.CoreModules.Evolutility_Wizard.EvoDicoTest" %>

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
    <h1 runat="server" id="txtp" visible="false">No application specified</h1>
 	
    <p id="evop" runat="server" >	
		<EVOL:UIServer ID="EvoTest" runat="server"  DataIsMetadata=false
		BackColor="#EDEDED" BackColorRowMouseOver="Beige"
			CssClass="main1" DBAllowDelete="true" DBAllowDesign="true" DBAllowExport="True"
			DBAllowSelections="false" DisplayModeStart="List" RowsPerPage="20" 
			SecurityModel="Single_User" SecurityKey="EvoDico"
			ShowTitle="true" SqlConnection="" ToolbarPosition="Top" UserComments="None" 
			VirtualPathPictures="/aspnet_client/evolutility/pixevo/"
			 VirtualPathToolbar="/aspnet_client/evolutility/pixevo/"  
			 Width="100%" XMLfile="" /> 
			</p> 
    </div>
    </form>
</body>
</html>
