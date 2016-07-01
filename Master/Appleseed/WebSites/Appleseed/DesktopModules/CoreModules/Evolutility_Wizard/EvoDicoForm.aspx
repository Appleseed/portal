<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EvoDicoForm.aspx.cs" Inherits="Appleseed.DesktopModules.CoreModules.Evolutility_Wizard.EvoDicoForm" %>

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
    <EVOL:UIServer ID="EvoForm" runat="server" DataIsMetadata="true"
            DBAllowInsert="False"
            DBAllowDelete="true"
            DBAllowInsertDetails="true"
            DBAllowUpdateDetails="true"
            DBAllowSelections="true"
            DBAllowExport="true"
            DBAllowHelp="true"
            SecurityModel="Single_User" SecurityKey="EvoDico"
            XMLfile="/aspnet_client/evolutility/XML/EvoDico_form.xml"
            VirtualPathToolbar="/aspnet_client/evolutility/pixevo/"
            VirtualPathPictures="/aspnet_client/evolutility/pixevo/"
            BackColorRowMouseOver="Beige" BackColor="#EDEDED"
            ToolbarPosition="Top" RowsPerPage="20" ShowTitle="true" Height="100%" Width="100%"
            DisplayModeStart="List" UserComments="None"></EVOL:UIServer>
    </div>
    </form>
</body>
</html>
