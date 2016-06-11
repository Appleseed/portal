<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EvoDoc.aspx.cs" Inherits="Appleseed.DesktopModules.CoreModules.Evolutility_Wizard.EvoDoc" %>

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
    <h1 id="docTitle" runat="server">Design document: entities/forms</h1>
			<p><EVOL:UIServer ID="evoDocument" runat="server"  DataIsMetadata=true 
					DBAllowDelete="False" DBAllowExport="True" DBAllowSelections="false" DBReadOnly="true"
					DisplayModeStart="List" RowsPerPage="100" 
					SecurityModel="Single_User" SecurityKey="EvoDico"
					 ShowTitle="true" 
					SqlConnection="" ToolbarPosition="Top" VirtualPathPictures="/aspnet_client/evolutility/pixevo/" VirtualPathToolbar="/aspnet_client/evolutility/PixEvo"
					Width="100%" XMLfile="/aspnet_client/evolutility/XML/evoDoc_Form.xml" /></p> 
    </div>
    </form>
</body>
</html>
