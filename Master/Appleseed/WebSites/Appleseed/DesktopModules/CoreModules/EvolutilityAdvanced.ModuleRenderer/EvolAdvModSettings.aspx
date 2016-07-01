<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EvolAdvModSettings.aspx.cs" Inherits="Appleseed.DesktopModules.CoreModules.EvolutilityAdvanced.ModuleRenderer.EvolAdvModSettings"
    MasterPageFile="~/Shared/SiteMasterDefault.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <table class="SettingsTableGroup" width="100%" border="0">
        <tr>
            <td class="SubHead" width="20%">Model ID</td>
            <td class="st-control" width="80%">
                <asp:TextBox ID="txtModelID" runat="server" /></td>
        </tr>
        <tr>
            <td class="SubHead" width="20%">Model Label</td>
            <td class="st-control" width="80%">
                <asp:TextBox ID="txtModelLabel" runat="server" /></td>
        </tr>
        <tr>
            <td class="SubHead" width="20%">Model Entity</td>
            <td class="st-control" width="80%">
                <asp:TextBox ID="txtEntity" runat="server" /></td>
        </tr>
        <tr>
            <td class="SubHead" width="20%">Model Entities</td>
            <td class="st-control" width="80%">
                <asp:TextBox ID="txtEntities" runat="server" /></td>
        </tr>
        <tr>
            <td class="SubHead" width="20%">Model Lead Fields</td>
            <td class="st-control" width="80%">
                <asp:TextBox ID="txtLeadFields" runat="server" /></td>
        </tr>
        <tr>
            <td class="SubHead" width="20%">Model Elements</td>
            <td class="st-control" width="80%">
                <asp:TextBox ID="txtElements" TextMode="MultiLine" Rows="15" runat="server" Width="700"/></td>
        </tr>
       
    </table>
    <p>
            <asp:PlaceHolder ID="PlaceHolderButtons" runat="server"></asp:PlaceHolder>
        </p>
</asp:Content>
