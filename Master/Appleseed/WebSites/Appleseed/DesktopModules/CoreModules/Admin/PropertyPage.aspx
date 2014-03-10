<%@ Page AutoEventWireup="false" Inherits="Appleseed.Content.Web.Modules.PagePropertyPage"
    Language="c#" MasterPageFile="~/Shared/SiteMasterDefault.master" Codebehind="PropertyPage.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <div class="div_ev_Table">
        <table align="center" border="0" cellpadding="4" cellspacing="0">
            <tr valign="top">
                <td>
                    <table cellpadding="0" cellspacing="0" width="1000">
                        <tr>
                            <% if (Request.QueryString.GetValues("ModalChangeMaster") == null) {%>
                                <td align="left" class="Head" nowrap="nowrap">
                                    <rbfwebui:Localize ID="Literal1" runat="server" Text="Module settings" TextKey="MODULESETTINGS_SETTINGS" />
                                </td>
                            <% } %>
                            <td align="right">
                                <asp:PlaceHolder ID="PlaceholderButtons2" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <hr noshade="noshade" size="1" />
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
