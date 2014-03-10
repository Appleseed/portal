<%@ control autoeventwireup="false" inherits="Appleseed.Content.Web.Modules.ContentManager"
    language="c#" Codebehind="ContentManager.ascx.cs" %>
<table align="center" border="0" cellpadding="0" cellspacing="0" width="600">
    <tr>
        <td width="250">
            <span class="ItemTitle">ModuleTypes</span>
        </td>
        <td width="120">
            &nbsp;</td>
        <td>
        </td>
    </tr>
    <tr>
        <td width="250">
            <asp:dropdownlist id="ModuleTypes" runat="server" autopostback="true" style="width: 250px;
                overflow-y: auto; overflow-x: hidden;" width="250px">
            </asp:dropdownlist>
        </td>
        <td width="120">
            &nbsp;</td>
        <td>
        </td>
    </tr>
    <tr>
    </tr>
    <td colspan="2">
        <table id="MultiPortalTable" runat="server" align="center" border="0" width="250">
            <tr>
                <td width="250">
                    <rbfwebui:label id="SourcePortalLabel" runat="server" class="ItemTitle" text="Source Portal"></rbfwebui:label></td>
                <td width="120">
                    &nbsp;</td>
                <td width="250">
                    <rbfwebui:label id="DestinationPortalLabel" runat="server" class="ItemTitle" text="Destination Portal"></rbfwebui:label></td>
            </tr>
            <tr>
                <td width="250">
                    <asp:dropdownlist id="SourcePortal" runat="server" autopostback="true" style="width: 250px;
                        overflow-y: auto; overflow-x: hidden;" width="250px">
                    </asp:dropdownlist></td>
                <td width="120">
                    &nbsp;</td>
                <td width="250">
                    <asp:dropdownlist id="DestinationPortal" runat="server" autopostback="true" style="width: 250px;
                        overflow-y: auto; overflow-x: hidden;" width="250px">
                    </asp:dropdownlist></td>
            </tr>
        </table>
    </td>
    <tr>
        <td width="250">
            <span class="ItemTitle">Source Module</span>
        </td>
        <td width="120">
            &nbsp;</td>
        <td align="right" width="250">
            <span class="ItemTitle">Destination Module</span>
        </td>
    </tr>
    <tr>
        <td>
            <asp:dropdownlist id="SourceInstance" runat="server" autopostback="true" style="width: 250px;
                overflow-y: auto; overflow-x: hidden;" width="250px">
            </asp:dropdownlist>
        </td>
        <td width="120">
            &nbsp;</td>
        <td align="right">
            <asp:dropdownlist id="DestinationInstance" runat="server" autopostback="true" style="width: 250px;
                overflow-y: auto; overflow-x: hidden;" width="250px">
            </asp:dropdownlist>
        </td>
    </tr>
</table>
<table align="center" border="0" cellpadding="0" cellspacing="0" width="600">
    <tr>
        <td>
            <span class="ItemTitle">Source</span>
        </td>
        <td width="120">
            &nbsp;</td>
        <td>
            <span class="ItemTitle">Destination</span>
        </td>
    </tr>
    <tr>
        <td valign="top" width="250">
            <asp:listbox id="SourceListBox" runat="server" rows="15" style="width: 250px; overflow-y: auto;
                overflow-x: hidden;" width="250px"></asp:listbox>
        </td>
        <td valign="top" width="120">
            <p>
                <rbfwebui:linkbutton id="DeleteLeft_Btn" runat="server" cssclass="CommandButton"
                    text="<- Delete" textkey="DeleteLeft" width="120px"></rbfwebui:linkbutton></p>
            <p>
                <rbfwebui:linkbutton id="MoveLeft_Btn" runat="server" cssclass="CommandButton" text="<- Move"
                    textkey="MoveItemLeft" width="120px"></rbfwebui:linkbutton></p>
            <p>
                <rbfwebui:linkbutton id="MoveRight_Btn" runat="server" cssclass="CommandButton" text="Move ->"
                    textkey="MoveItemRight" width="120px"></rbfwebui:linkbutton></p>
            <p>
                <rbfwebui:linkbutton id="CopyRight_Btn" runat="server" cssclass="CommandButton" text="Copy ->"
                    textkey="CopyItem" width="120px"></rbfwebui:linkbutton></p>
            <p>
                <rbfwebui:linkbutton id="CopyAll_Btn" runat="server" cssclass="CommandButton" text=" Copy All ->"
                    textkey="CopyAll" width="120px"></rbfwebui:linkbutton></p>
            <p>
                <rbfwebui:linkbutton id="DeleteRight_Btn" runat="server" cssclass="CommandButton"
                    text="Delete->" textkey="DeleteRight" width="120px"></rbfwebui:linkbutton></p>
        </td>
        <td valign="top" width="250">
            <asp:listbox id="DestListBox" runat="server" rows="15" style="width: 250px; overflow-y: auto;
                overflow-x: hidden;" width="250px"></asp:listbox>
        </td>
    </tr>
</table>
