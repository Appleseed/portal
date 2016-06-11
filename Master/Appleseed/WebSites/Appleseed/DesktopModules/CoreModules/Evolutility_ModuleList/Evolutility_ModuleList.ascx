<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Evolutility_ModuleList.ascx.cs" Inherits="Appleseed.DesktopModules.CoreModules.Evolutility_ModuleList.Evolutility_ModuleList" %>
<%@ Register Assembly="Evolutility.UIServer" Namespace="Evolutility" TagPrefix="EVOL" %>

<link rel="stylesheet" type="text/css" href="/aspnet_client/evolutility/pixevo/css/evol.css" id="evolcss" runat="server" />
<link rel="stylesheet" type="text/css" href="/aspnet_client/evolutility/pixevo/css/evolwiz.css" id="evolwizcss" runat="server" />

<table width="100%">
    <tr>
        <td>
            <h1>
                <img src="/aspnet_client/evolutility/pixevo/edi_frm.png" class="icon" alt="" />
                Evolutility Modules List</h1>
        </td>
        <td align="Right" style="vertical-align: middle">Add New: 
           <a href="/DesktopModules/CoreModules/Evolutility_Wizard/Evolutility_Wizard.aspx?"
onclick="openInModal('/DesktopModules/CoreModules/Evolutility_Wizard/Evolutility_Wizard.aspx?','Evolutility Module - Add New');return false;">Wizard</a>
            <a href="/DesktopModules/CoreModules/Evolutility_Wizard/Evolutility_Wizard.aspx?WIZ=dbscan"
onclick="openInModal('/DesktopModules/CoreModules/Evolutility_Wizard/Evolutility_Wizard.aspx?WIZ=dbscan','Evolutility Module - Add New');return false;">DB Mapper</a>
              <a href="/DesktopModules/CoreModules/Evolutility_Wizard/Evolutility_Wizard.aspx?WIZ=xml2db"
onclick="openInModal('/DesktopModules/CoreModules/Evolutility_Wizard/Evolutility_Wizard.aspx?WIZ=xml2db','Evolutility Module - Add New');return false;">Import XML</a>
    </tr>
</table>

<p>
    <EVOL:UIServer ID="evoModuleList" runat="server" DataIsMetadata="true"
        DBAllowInsert="False"
        DBAllowDelete="true"
        DBAllowInsertDetails="true"
        DBAllowUpdateDetails="true"
        DBAllowSelections="true"
        DBAllowExport="true"
        DBAllowHelp="true"
        SecurityModel="Single_User" SecurityKey="EvoDico"
        XMLfile="/aspnet_client/Evolutility/XML/EvoDico_form.xml"
        VirtualPathToolbar="/aspnet_client/evolutility/PixEvo/"
        VirtualPathPictures="/aspnet_client/evolutility/PixEvo/"
        BackColorRowMouseOver="Beige" BackColor="#EDEDED"
        ToolbarPosition="Top" RowsPerPage="20" ShowTitle="true" Height="100%" Width="100%"
        DisplayModeStart="List" UserComments="None"></EVOL:UIServer>
</p>

