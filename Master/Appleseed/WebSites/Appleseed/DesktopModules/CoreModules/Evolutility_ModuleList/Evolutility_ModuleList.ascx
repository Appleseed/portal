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
           <a runat="server" id="WizardBasic" href="/DesktopModules/CoreModules/Evolutility_Wizard/Evolutility_Wizard.aspx?mid="
onclick="openInModal('/DesktopModules/CoreModules/Evolutility_Wizard/Evolutility_Wizard.aspx?mid=','Evolutility Module - Add New');return false;">Wizard</a>
            <a runat="server" id="DBMapper" href="/DesktopModules/CoreModules/Evolutility_Wizard/Evolutility_Wizard.aspx?WIZ=dbscan&mid="
onclick="openInModal('/DesktopModules/CoreModules/Evolutility_Wizard/Evolutility_Wizard.aspx?WIZ=dbscan&mid=','Evolutility Module - Add New');return false;">DB Mapper</a>
              <a runat="server" id="ImportXML" href="/DesktopModules/CoreModules/Evolutility_Wizard/Evolutility_Wizard.aspx?WIZ=xml2db&mid="
onclick="openInModal('/DesktopModules/CoreModules/Evolutility_Wizard/Evolutility_Wizard.aspx?WIZ=xml2db&mid=','Evolutility Module - Add New');return false;">Import XML</a>
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
<asp:HiddenField runat="server" ID="hdnModuleID" />
<script type="text/javascript">
    $(document).ready(function () {
        $('[id*=WizardBasic]').attr("onclick", "openInModal('" + $('[id*=WizardBasic]').attr("href") + "');");
        $('[id*=WizardBasic]').attr("href", "javascript:void(0)");
        $('[id*=DBMapper]').attr("onclick", "openInModal('" + $('[id*=DBMapper]').attr("href") + "');");
        $('[id*=DBMapper]').attr("href", "javascript:void(0)");
        $('[id*=ImportXML]').attr("onclick", "openInModal('" + $('[id*=ImportXML]').attr("href") + "');");
        $('[id*=ImportXML]').attr("href", "javascript:void(0)");
        $("a[href*='EvolutilityFormTest.aspx']").each(function (index, item) {
            $(this).attr("href", $(this).attr("href") + "&mid=" + $('[id*=hdnModuleID]').val());
        });
        $("a[href*='EvolutilityFormDocument.aspx']").each(function (index, item) {
            $(this).attr("href", $(this).attr("href") + "&mid=" + $('[id*=hdnModuleID]').val());
        });
        $("a[href*='Evolutility_Wizard.aspx']").each(function (index, item) {
            if ($(this).attr("href").indexOf('mid=')== -1) {
                $(this).attr("href", $(this).attr("href") + "&mid=" + $('[id*=hdnModuleID]').val());
            }
        });
    });
</script>

