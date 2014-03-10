<%@ Page AutoEventWireup="false" Inherits="Appleseed.Content.Web.Modules.PageCustomPropertyPage"
    Language="c#" MasterPageFile="~/Shared/SiteMasterDefault.master" Codebehind="CustomPropertyPage.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <table align="center" border="0" cellpadding="4" cellspacing="0">
        <tr valign="top">
            <td>
                <table cellpadding="0" cellspacing="0" width="600">
                    <tr>
                        <td align="left" class="Head">
                            <rbfwebui:Localize ID="Literal1" runat="server" Text="Module Custom Settings" TextKey="MODULE_CUSTOM_SETTINGS">
                            </rbfwebui:Localize>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr noshade="noshade" size="1" />
                            <rbfwebui:SettingsTable ID="EditTable" runat="server"></rbfwebui:SettingsTable>
                        </td>
                    </tr>
                </table>
                <p>
                    <asp:PlaceHolder ID="PlaceHolderButtons" runat="server"></asp:PlaceHolder>
                    &nbsp;
                </p>
            </td>
        </tr>
    </table>
</asp:Content>
