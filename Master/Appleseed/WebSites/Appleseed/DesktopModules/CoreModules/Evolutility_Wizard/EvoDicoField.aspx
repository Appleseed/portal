<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EvoDicoField.aspx.cs" Inherits="Appleseed.DesktopModules.CoreModules.Evolutility_Wizard.EvoDicoField" %>

<%@ Register Assembly="Evolutility.UIServer" Namespace="Evolutility" TagPrefix="EVOL" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="/aspnet_client/evolutility/css/evol.css" id="evolcss" runat="server" />
    <link rel="stylesheet" type="text/css" href="/aspnet_client/evolutility/css/evolwiz.css" id="evolwizcss" runat="server"/>
    <script src="/aspnet_client/evolutility/PixEvo/JS/EvoDicoRules.js" type="text/javascript"></script> 
</head>
<body>
    <form id="form1" runat="server">
    <div>
    

			<h1><img src="../pixevo/edi_fld.png" class="icon" alt=""/> Fields</h1>
			<p><EVOL:UIServer ID="evoModuleField" runat="server"  DataIsMetadata=true 
					CssClass="main1" 
					DBAllowDelete="true" 
					DBAllowExport="True" 
					DBAllowSelections="false"
					DisplayModeStart="List" 
					DBAllowHelp="true"
					RowsPerPage="20" 
					
					SecurityModel="Single_User" SecurityKey="EvoDico"
					ShowTitle="true"
					SqlConnection="" ToolbarPosition="Top" UserComments="None" 
					VirtualPathPictures="/aspnet_client/evolutility//pixevo/"
					 VirtualPathToolbar="/aspnet_client/evolutility//pixevo/" 
					 Width="100%" 
					 XMLfile="/aspnet_client/evolutility/XML/EvoDico_Field.xml" /></p>
    </div>
    </form>
</body>
</html>
