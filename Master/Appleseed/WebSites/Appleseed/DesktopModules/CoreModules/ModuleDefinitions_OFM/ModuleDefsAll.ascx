<%@ Control AutoEventWireup="false" Inherits="Appleseed.Content.Web.Modules.ModuleDefsAll_OFM"
    Language="c#" CodeBehind="ModuleDefsAll.ascx.cs" %>
<asp:DataList ID="defsList" runat="server" DataKeyField="GeneralModDefID">
    <ItemTemplate>
        <span nowrap="">&nbsp;<rbfwebui:ImageButton ID="ImageButton1" runat="server" AlternateText="Edit this item"
            ImageUrl='<%# this.CurrentTheme.GetImage("Buttons_Edit", "Edit.gif").ImageUrl %>'
            TextKey="EDIT_THIS_ITEM" />
            &nbsp;<rbfwebui:Label ID="Label1" runat="server" CssClass="Normal" Text='<%# DataBinder.Eval(Container.DataItem, "FriendlyName") %>'>
            </rbfwebui:Label>
            &nbsp;&nbsp;&nbsp;File:
            <%# DataBinder.Eval(Container.DataItem, "DesktopSrc") %>
        </span>
    </ItemTemplate>
</asp:DataList>