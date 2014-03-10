<%@ Control Inherits="Appleseed.Content.Web.Modules.Roles"
    Language="c#" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" Codebehind="Roles.ascx.cs" %>
<table border="0" cellpadding="2" cellspacing="0">
   
    <tr>
        <td>
            <asp:DataList ID="rolesList" runat="server" OnItemCommand="rolesList_ItemCommand" OnItemDataBound="RolesList_ItemDataBound" DataKeyField="Id" >
                <ItemTemplate>
                    <table cellspacing="3">
                        <tr>
                            <td>
                                <rbfwebui:ImageButton ID="ImageButton2" runat="server" AlternateText="Edit this item"
                                    CommandName="edit" ImageUrl='<%# this.CurrentTheme.GetImage("Buttons_Edit", "Edit.gif").ImageUrl %>'
                                    TextKey="EDIT_THIS_ITEM" CausesValidation="false" />
                            </td>
                            <td>
                                <rbfwebui:ImageButton ID="ImageButton1" runat="server" AlternateText="Delete this item"
                                    CausesValidation="false" CommandName="delete"  CommandArgument='<%# Eval( "Id" ) %>'
                                    ImageUrl='<%# this.CurrentTheme.GetImage("Buttons_Delete", "Delete.gif").ImageUrl %>'
                                    TextKey="DELETE_THIS_ITEM" />
                            </td>
                            <td>
                                <rbfwebui:HyperLink ID="Name" runat="server" CssClass="Normal" Text='<%# Eval("Name") %>' />
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <EditItemTemplate>
                    <table cellspacing="3">
                        <tr>
                            <td>
                                <asp:Label ID="roleId" runat="server" Text='<%# Eval("Id") %>' Visible="false" />
                                
                                <asp:TextBox ID="roleName" runat="server" CssClass="NormalTextBox" Text='<%# Eval("Name") %>' />
                            </td>
                            <td>
                                <rbfwebui:LinkButton ID="LinkButton2" runat="server" CommandName="apply" CssClass="CommandButton" TextKey="APPLY" Text="Apply" CausesValidation="false" />
                            </td>
                            <td>
                                <rbfwebui:LinkButton ID="LinkButton1" runat="server" CommandName="members" CssClass="CommandButton" TextKey="ROLE_CHANGE_MEMBERS" Text="Change Role Members" CausesValidation="false" />
                            </td>
                        </tr>
                    </table>
                </EditItemTemplate>
            </asp:DataList>
        </td>
    </tr>
    <tr>
        <td>
            <asp:TextBox ID='txtNewRole' runat='server'  ValidationGroup="roleValidationGroup" />&nbsp;
            <rbfwebui:LinkButton ID="AddRoleBtn" runat="server" CssClass="CommandButton" Text="Add New Role"
                TextKey="AM_ADDROLE" OnClick="AddRole_Click" ValidationGroup="roleValidationGroup" />
            <asp:RequiredFieldValidator ID="txtNewRoleValidator"  ValidationGroup="roleValidationGroup" runat="server" ControlToValidate="txtNewRole" ErrorMessage="Role Name can't be empty" />
        </td>
    </tr>
    <tr>
        <td class="Error">
            <rbfwebui:Localize ID="labelError" runat="server" Text="Failed to delete the role you selected. Please ensure no users are making use of this role."
                TextKey="ROLE_DELETE_FAILED" Visible="false">
            </rbfwebui:Localize>
        </td>
    </tr>
</table>
