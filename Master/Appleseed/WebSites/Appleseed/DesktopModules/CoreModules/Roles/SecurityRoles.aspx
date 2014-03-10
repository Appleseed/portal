<%@ Page AutoEventWireup="false" Inherits="Appleseed.Content.Web.Modules.SecurityRoles"
    Language="c#" MasterPageFile="~/Shared/SiteMasterDefault.master" Codebehind="SecurityRoles.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <div class="div_ev_Table">
        <table border="0" cellpadding="2" cellspacing="2" width="98%">
            <tr>
                <td colspan="2">
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td align="left">
                                <span id="title" runat="server" class="Head">Role Membership</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <hr noshade="noshade" size="1" />
                            </td>
                        </tr>
                    </table>
                    <rbfwebui:Label ID="Message" runat="server" CssClass="NormalRed">
                    </rbfwebui:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:DropDownList ID="allUsers" runat="server" CssClass="NormalTextBox" DataTextField="Email"
                        DataValueField="ProviderUserKey">
                    </asp:DropDownList>
                </td>
                <td nowrap="nowrap">
                    <rbfwebui:LinkButton ID="addExisting" runat="server" CssClass="CommandButton" TextKey="ROLE_ADD_USER"
                        Text="Add existing user to role" OnClick="AddUser_Click" />
                </td>
            </tr>
            <tr valign="top">
                <td width="11">
                    &nbsp;
                </td>
                <td nowrap="nowrap">
                    <asp:DataList ID="usersInRole" runat="server" RepeatColumns="2" OnItemCommand="usersInRole_ItemCommand">
                        <ItemStyle Width="225" />
                        <ItemTemplate>
                            &#160;&#160;
                            <rbfwebui:ImageButton ID="Imagebutton1" runat="server" AlternateText="Remove this user from role"
                                CommandName="delete" ImageUrl='<%# CurrentTheme.GetImage("Buttons_Delete", "Delete.gif").ImageUrl %>' />
                            <rbfwebui:Label ID="lblUserEmail" runat="server" CssClass="Normal" Text='<%# Container.DataItem %>'>
                            </rbfwebui:Label>
                        </ItemTemplate>
                    </asp:DataList>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr noshade="noshade" size="1" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <rbfwebui:LinkButton ID="saveBtn" runat="server" CssClass="CommandButton" TextKey="ROLE_SAVE_CHANGES"
                        Text="Save Role Changes" OnClick="Save_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
