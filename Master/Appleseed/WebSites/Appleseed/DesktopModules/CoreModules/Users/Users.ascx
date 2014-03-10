<%@ Control AutoEventWireup="false" Inherits="Appleseed.Content.Web.Modules.Users"
    Language="c#" Codebehind="Users.ascx.cs" %>
<table border="0" cellpadding="2" cellspacing="0">
    <tr valign="top">
        <td class="Normal">
            <asp:PlaceHolder ID="UserDomain" runat="server" Visible="False">
                <rbfwebui:Localize ID="DomainMessage1" runat="server" Text="Domain users do not need to be registered to access portal content that is available to 'All Users'"
                    TextKey="USER_MESSAGE_DOMAIN1">
                </rbfwebui:Localize>
                <br />
                <rbfwebui:Localize ID="DomainMessage2" runat="server" Text="Administrators may add domain users to specific roles using the Security Roles function above."
                    TextKey="USER_MESSAGE_DOMAIN2">
                </rbfwebui:Localize>
                <br />
                <rbfwebui:Localize ID="DomainMessage3" runat="server" Text="This section permits Administrators to manage users and their security roles directly."
                    TextKey="USER_MESSAGE_DOMAIN3">
                </rbfwebui:Localize>
                <br />
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="UserForm" runat="server" Visible="False">
                <rbfwebui:Localize ID="FormsMessage1" runat="server" Text="Users must be registered to view secure content."
                    TextKey="USER_MESSAGE1">
                </rbfwebui:Localize>
                <br />
                <rbfwebui:Localize ID="FormsMessage2" runat="server" Text="Users may add themselves using the Register form, and Administrators may add users to specific roles using the Security Roles function above."
                    TextKey="USER_MESSAGE2">
                </rbfwebui:Localize>
                <br />
                <rbfwebui:Localize ID="FormsMessage3" runat="server" Text="This section permits Administrators to manage users and their security roles directly."
                    TextKey="USER_MESSAGE3">
                </rbfwebui:Localize>
                <br />
            </asp:PlaceHolder>
        </td>
    </tr>
    <tr valign="top">
        <td class="Normal">
            <table>
                <tr>
                    <td>
                        <rbfwebui:Label ID="RegisteredLabel" runat="server" Text="Registered users" TextKey="REGISTERED_USERS" /></td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:UpdatePanel ID="AllUsersUpdatePanel" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="allUsers" runat="server" AllowPaging="True" AllowSorting="True"
                                    AutoGenerateColumns="false" CellPadding="4" DataKeyNames="Email" EnableSortingAndPagingCallbacks="false"
                                    ForeColor="#333333" GridLines="None">
                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="User Name">
                                            <ItemTemplate>
                                                <%# Eval("Name") %>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <%# Eval( "Name" )%>
                                            </AlternatingItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Email">
                                            <ItemTemplate>
                                                <a id="lnkUser" runat="server" href='<%# "mailto:" + Eval("Email") %>'>
                                                    <%# Eval("Email") %>
                                                </a>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Commands">
                                            <ItemStyle Wrap="False" />
                                            <ItemTemplate>
                                                <rbfwebui:LinkButton ID="EditBtn" runat="server" CommandArgument='<%# Eval("Email") %>' OnClientClick=<%# "openInModal('"+ Builddir((String)Eval("Email"))+"','"+Resources.Appleseed.EDIT_USER+"');return false;" %>
                                                    CommandName="Edit" Text="Edit" TextKey="EDIT_USER" CausesValidation="false" />&nbsp;
                                                <rbfwebui:LinkButton ID="DeleteBtn" runat="server" CommandArgument='<%# Eval("ProviderUserKey") %>'
                                                    CommandName="Delete" Text="Delete" TextKey="DELETE_USER" CausesValidation="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                    <EditRowStyle BackColor="#999999" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
