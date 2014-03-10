<%@ Page AutoEventWireup="false" Inherits="Appleseed.AdminAll.EditPortal"
    Language="c#"  MasterPageFile="~/Shared/SiteMasterDefault.master" Codebehind="EditPortal.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <table cellpadding="4" cellspacing="0" width="98%">
        <tr valign="top">
            <td width="150">
                &nbsp;
            </td>
            <td width="*">
                <table border="0" cellpadding="2" cellspacing="1">
                    <tr>
                        <td align="left" class="Head" colspan="3">
                            <rbfwebui:Localize ID="Literal1" runat="server" Text="Edit Portal" TextKey="EDIT_PORTAL">
                            </rbfwebui:Localize>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr noshade="noshade" size="1" />
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="SubHead" width="140">
                            <rbfwebui:Localize ID="Literal2" runat="server" Text="Portal ID" TextKey="AM_PORTALID">
                            </rbfwebui:Localize>
                        </td>
                        <td class="Normal">
                            <rbfwebui:Label ID="PortalIDField" runat="server"></rbfwebui:Label>
                        </td>
                        <td class="Normal">
                        </td>
                    </tr>
                    <tr>
                        <td class="SubHead" width="140">
                            <rbfwebui:Localize ID="Literal3" runat="server" Text="Portal Title" TextKey="AM_SITETITLE">
                            </rbfwebui:Localize>
                        </td>
                        <td class="Normal">
                            <asp:TextBox ID="TitleField" runat="server" CssClass="NormalTextBox" Width="350"></asp:TextBox>
                        </td>
                        <td class="Normal">
                            <asp:RequiredFieldValidator ID="RequiredTitle" runat="server" ControlToValidate="TitleField"
                                ErrorMessage="Required Field"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="SubHead" width="140">
                            <rbfwebui:Localize ID="Literal4" runat="server" Text="Portal Alias" TextKey="AM_PORTALALIAS">
                            </rbfwebui:Localize>
                        </td>
                        <td class="Normal">
                            <rbfwebui:Label ID="AliasField" runat="server"></rbfwebui:Label>
                        </td>
                        <td class="Normal">
                        </td>
                    </tr>
                    <tr>
                        <td class="SubHead" width="140">
                            <rbfwebui:Localize ID="Literal5" runat="server" Text="Site Path" TextKey="AM_SITEPATH">
                            </rbfwebui:Localize>
                        </td>
                        <td class="Normal">
                            <rbfwebui:Label ID="PathField" runat="server"></rbfwebui:Label>
                        </td>
                        <td class="Normal">
                        </td>
                    </tr>
                </table>
                <rbfwebui:SettingsTable ID="EditTable" runat="server" />
                <p>
                    <rbfwebui:LinkButton ID="UpdateButton" runat="server" class="CommandButton"></rbfwebui:LinkButton></p>
                <p class="Normal">
                    <rbfwebui:Label ID="ErrorMessage" runat="server" CssClass="Error" Visible="false"></rbfwebui:Label></p>
            </td>
        </tr>
    </table>
</asp:Content>
