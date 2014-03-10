<%@ Register Src="~/Design/DesktopLayouts/DesktopPortalBanner.ascx" TagName="Banner"
    TagPrefix="portal" %>
<%@ Register Src="~/Design/DesktopLayouts/DesktopFooter.ascx" TagName="Footer" TagPrefix="foot" %>

<%@ Page AutoEventWireup="false" Inherits="Appleseed.Content.Web.Modules.UsersManage"
    Language="c#" MasterPageFile="~/Shared/SiteMasterDefault.master" Codebehind="UsersManage.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
   <div class="div_ev_Table" style="float: left;">
        <table align="left" border="0" cellpadding="4" cellspacing="0">
            <tr valign="top">
                <td colspan="2">
                    <%--    <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td align="left">
                                        <span id="title" runat="server" class="Head">
                                            <rbfwebui:Label ID="Label2" runat="server" TextKey="USER_MANAGE">Manage User</rbfwebui:Label>
                                        </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <hr noshade="noshade" size="1" />
                                    </td>
                                </tr>
                            </table>--%>
                </td>
            </tr>
            <tr>
                <td class="Normal" colspan="2">
                    <!-- Start Register control -->
                    <asp:PlaceHolder ID="register" runat="server"></asp:PlaceHolder>
                    <!-- End Register control -->
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <p>
                        &nbsp;</p>
                </td>
            </tr>
            <tr>
                <td style="width: 30px">
                </td>
                <td>
                    <asp:DropDownList ID="allRoles" runat="server" DataTextField="Name" DataValueField="Id" />
                    <rbfwebui:LinkButton ID="addExisting" runat="server" CssClass="CommandButton" Text="Add user to this role"
                        TextKey="ADDUSER" />
                </td>
            </tr>
            <tr valign="top">
                <td style="width: 30px">
                </td>
                <td>
                    <asp:DataList ID="userRoles" runat="server" RepeatColumns="2">
                        <ItemStyle Width="205" />
                        <ItemTemplate>
                            &#160;&#160;
                            <rbfwebui:ImageButton ID="deleteBtn" runat="server" AlternateText='DELUSER' CommandName="delete"
                                ImageUrl='<%# CurrentTheme.GetImage("Buttons_Delete", "Delete.gif").ImageUrl %>' />
                            <rbfwebui:Label ID="Label1" runat="server" CssClass="Normal" Text='<%# Eval("Name") %>'>
                            </rbfwebui:Label>
                        </ItemTemplate>
                    </asp:DataList>
                </td>
            </tr>
            <tr>
                <td style="width: 30px">
                </td>
                <td>
                    <hr noshade="noshade" size="1" />
                    <rbfwebui:Label ID="ErrorLabel" runat="server" CssClass="Error" Visible="False"></rbfwebui:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 30px">
                </td>
                <td>
                    <rbfwebui:LinkButton ID="saveBtn" runat="server" CssClass="CommandButton" Text="Save User Changes"
                      ValidationGroup="USER"  TextKey="SAVEUSER"></rbfwebui:LinkButton>
                </td>
            </tr>
        </table>
    </div>
    <%--<div class="rb_AlternatePortalFooter" style="float: left; width: 100%">
        <foot:Footer ID="Footer" runat="server" />
    </div>--%>
</asp:Content>
