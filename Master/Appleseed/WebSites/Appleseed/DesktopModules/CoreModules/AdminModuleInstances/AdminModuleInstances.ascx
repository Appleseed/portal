<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminModuleInstances.ascx.cs" Inherits="Appleseed.DesktopModules.CoreModules.AdminModuleInstances.AdminModuleInstances" %>
<h4>Modules: <asp:DropDownList ID="ddlModules" runat="server" OnSelectedIndexChanged="ddlModules_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList></h4>
<br />
<h4>Instances</h4>
<hr />
<asp:Repeater ID="rptPages" runat="server">
    <HeaderTemplate>
        <table style="border-spacing: 10px!important;">
            <thead style="border: 1px solid #000;">
                <tr style="border: 1px solid #000;">
                    <td>Page#</td>
                    <td>Page Name</td>
                    <td>Module Title</td>
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td><%# Eval("ID") %></td>
            <td><%# Eval("PageName") %></td>
            <td><%# Eval("ModuleTitle") %></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </tbody>
        </table>
    </FooterTemplate>
</asp:Repeater>
