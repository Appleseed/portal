<%@ Page AutoEventWireup="false" Inherits="Appleseed.Admin.BlacklistEdit"
    Language="c#" MasterPageFile="~/Shared/SiteMasterDefault.master" Codebehind="BlacklistEdit.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <table border="0" cellpadding="4" cellspacing="0" width="98%">
        <tr valign="top">
            <td width="50">
                &nbsp;
            </td>
            <td width="*">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td align="left" class="Head">
                            <rbfwebui:Localize ID="Literal1" runat="server" Text="Blacklist" TextKey="BLACKLIST">
                            </rbfwebui:Localize>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr noshade="noshade" size="1" />
                        </td>
                    </tr>
                </table>
                <asp:Repeater ID="repListItem" runat="server">
                    <HeaderTemplate>
                        <table width="100%">
                        </table>
                        <tr>
                            <td colspan="5">
                            </td>
                            <td>
                                <input onclick="AllCheckboxCheck(0,true);" type="button" value='<%=General.GetString("BLACKLIST_ALL") %>'>&#160;
                                <input onclick="AllCheckboxCheck(0,false);" type="button" value='<%=General.GetString("BLACKLIST_NONE") %>'>
                            </td>
                        </tr>
                        <tr>
                            <th align="left">
                                <rbfwebui:Localize ID="Localize1" runat="server" Text="Name" TextKey="BLACKLIST_NAME">
                                </rbfwebui:Localize>
                            </th>
                            <th align="left">
                                <rbfwebui:Localize ID="Localize2" runat="server" TextKey="BLACKLIST_EMAIL">
                                </rbfwebui:Localize>
                            </th>
                            <th align="left">
                                <rbfwebui:Localize ID="Localize3" runat="server" Text="Send Newsletter" TextKey="BLACKLIST_SENDNEWSLETTER">
                                </rbfwebui:Localize>
                            </th>
                            <th align="left">
                                <rbfwebui:Localize ID="Localize4" runat="server" TextKey="BLACKLIST_DATE">
                                </rbfwebui:Localize>
                            </th>
                            <th align="left">
                                <rbfwebui:Localize ID="Localize5" runat="server" TextKey="BLACKLIST_REASON">
                                </rbfwebui:Localize>
                            </th>
                            <th align="left">
                                <rbfwebui:Localize ID="Localize6" runat="server" TextKey="BLACKLISTED">
                                </rbfwebui:Localize>
                            </th>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "Name") %>
                                <rbfwebui:Label ID="lblEMail" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "EMail") %>'
                                    Visible="False">
                                </rbfwebui:Label>
                            </td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "Email") %>
                            </td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "SendNewsletter") %>
                            </td>
                            <td>
                                <%# GetDate(Container.DataItem) %>
                            </td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "Reason") %>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# GetBlacklisted(Container.DataItem) %>'
                                    EnableViewState="False" />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
                <p>
                    <rbfwebui:LinkButton ID="UpdateButton" runat="server" CssClass="CommandButton" Text="Update">
                    </rbfwebui:LinkButton>
                    &nbsp;
                    <rbfwebui:LinkButton ID="CancelButton" runat="server" CausesValidation="False" CssClass="CommandButton"
                        Text="Cancel">
                    </rbfwebui:LinkButton>
                </p>
            </td>
        </tr>
    </table>
</asp:Content>
