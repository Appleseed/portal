<%@ Page AutoEventWireup="false" Inherits="Appleseed.Admin.AddPage"
    Language="c#" MasterPageFile="~/Shared/SiteMasterDefault.master" Codebehind="AddPage.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <table cellpadding="4" cellspacing="0" width="98%">
        <tr valign="top">
            <td width="150">
                &nbsp;
            </td>
            <td width="*">
                <table border="0" cellpadding="2" cellspacing="1">
                    <tr>
                        <td colspan="4">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td align="left" class="Head">
                                        <rbfwebui:Localize ID="tab_name" runat="server" Text="Add New Page" TextKey="AM_TABNAME">
                                        </rbfwebui:Localize>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <hr noshade="noshade" size="1" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="Normal" width="100">
                            <rbfwebui:Localize ID="tab_name1" runat="server" Text="Page Name" TextKey="AM_TABNAME1">
                            </rbfwebui:Localize>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="tabName" runat="server" CssClass="NormalTextBox" MaxLength="200"
                                Width="300"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="Normal" nowrap="nowrap">
                            <rbfwebui:Localize ID="roles_auth" runat="server" Text="Authorized Roles" TextKey="AM_ROLESAUTH">
                            </rbfwebui:Localize>
                        </td>
                        <td colspan="3">
                            <asp:CheckBoxList ID="authRoles" runat="server" CssClass="Normal" RepeatColumns="2"
                                Width="300">
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td class="Normal" nowrap="nowrap">
                            <rbfwebui:Localize ID="tab_parent" runat="server" Text="Parent Page" TextKey="TAB_PARENT">
                            </rbfwebui:Localize>
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="parentPage" runat="server" CssClass="NormalTextBox" DataTextField="Name"
                                DataValueField="ID" Width="300px">
                            </asp:DropDownList>
                            <rbfwebui:Label ID="lblErrorNotAllowed" runat="server" CssClass="Error" EnableViewState="False"
                                TextKey="ERROR_NOT_ALLOWED_PARENT" Visible="False">Not allowed to choose that parent</rbfwebui:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="3">
                            <hr noshade="noshade" size="1" />
                        </td>
                    </tr>
                    <tr>
                        <td class="Normal" nowrap="nowrap">
                            <rbfwebui:Localize ID="show_mobile" runat="server" Text="Show to mobile users" TextKey="AM_SHOWMOBILE">
                            </rbfwebui:Localize>
                        </td>
                        <td colspan="3">
                            <asp:CheckBox ID="showMobile" runat="server" CssClass="Normal" />
                        </td>
                    </tr>
                    <tr>
                        <td class="Normal" nowrap="nowrap">
                            <rbfwebui:Localize ID="mobiletab" runat="server" Text="Mobile Page Name" TextKey="AM_MOBILETAB">
                            </rbfwebui:Localize>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="mobilePageName" runat="server" CssClass="NormalTextBox" MaxLength="50"
                                Width="300"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr noshade="noshade" size="1" />
                        </td>
                    </tr>
                    <tr>
                        <td class="Normal" nowrap="nowrap">
                            <rbfwebui:Localize ID="lbl_jump_to_tab" runat="server" Text="Jump to this tab?" TextKey="AM_JUMPTOTAB">
                            </rbfwebui:Localize>
                        </td>
                        <td colspan="3">
                            <asp:CheckBox ID="cb_JumpToPage" runat="server" CssClass="Normal" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr noshade="noshade" size="1" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="Error" colspan="4">
                            <rbfwebui:Localize ID="msgError" runat="server" Text="You do not have the appropriate permissions to delete or move this module"
                                TextKey="AM_NO_RIGHTS">
                            </rbfwebui:Localize>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr noshade="noshade" size="1" />
                            <rbfwebui:SettingsTable ID="EditTable" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <rbfwebui:LinkButton ID="saveButton" runat="server" class="CommandButton" Text="Save Changes"
                                TextKey="SAVE_CHANGES">Save Page</rbfwebui:LinkButton>&nbsp;
                            <rbfwebui:LinkButton ID="CancelButton" runat="server" class="CommandButton" Text="Cancel"
                                TextKey="CANCEL"></rbfwebui:LinkButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
