<%@ Page AutoEventWireup="false" Inherits="Appleseed.Content.Web.Modules.PagePropertyPage"
    Language="c#" MasterPageFile="~/Shared/SiteMasterDefault.master" CodeBehind="PropertyPage.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <div class="div_ev_Table">
        <% if (Request.QueryString.GetValues("ModalChangeMaster") == null)
            {%>
        <rbfwebui:Localize ID="Literal1" runat="server" Text="Module settings" TextKey="MODULESETTINGS_SETTINGS" />
        <% } %>
        <asp:PlaceHolder ID="PlaceholderButtons2" Visible="false" runat="server" />

        <table align="center" border="0" cellpadding="4" cellspacing="0">
            <tr valign="top">
                <td>
                    <table cellpadding="0" cellspacing="0" width="1000">
                         <tr>
                            <td colspan="2">
                                <rbfwebui:SettingsTable ID="EditTable" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:PlaceHolder ID="PlaceHolderButtons" runat="server" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
