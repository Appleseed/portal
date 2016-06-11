<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Evolutility_ModuleRenderer.ascx.cs" Inherits="Appleseed.DesktopModules.CoreModules.Evolutility_ModuleRenderer.Evolutility_ModuleRenderer" %>

<%@ Register Assembly="Evolutility.UIServer" Namespace="Evolutility" TagPrefix="EVOL" %>

<link rel="stylesheet" type="text/css" href="/aspnet_client/evolutility/css/evol.css" id="evolcss" runat="server" />
<link rel="stylesheet" type="text/css" href="/aspnet_client/evolutility/css/evolwiz.css" id="evolwizcss" runat="server" />

<table style="width: 100%">
    <tr>
        <td>
            <h1>

                <asp:Label ID="Label1" runat="server" Text=""></asp:Label></h1>

        </td>
    </tr>
</table>

<EVOL:UIServer ID="evoModuleRenderer" runat="server" CssClass="main1" BackColor="#EDEDED" Language="EN"
    BackColorRowMouseOver="Beige" VirtualPathToolbar="/aspnet_client/evolutility/PixEvo/"
    RowsPerPage="20" Width="100%" ToolbarPosition="top" NavigationLinks="false" DBAllowDesign="true"
    DisplayModeStart="List" SecurityModel="Single_User" DBAllowExport="True" />
