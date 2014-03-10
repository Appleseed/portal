<%@ Control AutoEventWireup="false" Inherits="Appleseed.Content.Web.Modules.ModuleDefsAll"
    Language="c#" CodeBehind="ModuleDefsAll.ascx.cs" %>
<asp:DataList ID="defsList" runat="server" DataKeyField="GeneralModDefID">
    <ItemTemplate>
        &#160;
        <rbfwebui:ImageButton ID="ImageButton1" runat="server" AlternateText="Edit this item"
            ImageUrl='<%# this.CurrentTheme.GetImage("Buttons_Edit", "Edit.gif").ImageUrl %>'
            TextKey="EDIT_THIS_ITEM" />&#160;
        <rbfwebui:Label ID="Label1" runat="server" CssClass="Normal" Text='<%# DataBinder.Eval(Container.DataItem, "FriendlyName") %>'>
        </rbfwebui:Label>
    </ItemTemplate>
</asp:DataList>