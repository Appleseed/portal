<%@ Register Assembly="Appleseed.Framework" Namespace="Appleseed.Framework.Web.UI.WebControls"
    TagPrefix="rbfwebui" %>
<%@ Control AutoEventWireup="false" Inherits="Appleseed.Content.Web.Modules.Portals"
    Language="c#" CodeBehind="Portals.ascx.cs" %>
<table align="center" border="0" cellpadding="2" cellspacing="0">
    <tr valign="top">
        <td>
            <table border="0" cellpadding="0" cellspacing="0">
                <tr valign="top">
                    <td>
                        <rbfwebui:Label ID="lblPortals" TextKey="PORTALS" runat="server"></rbfwebui:Label>
                        <br />
                        <asp:ListBox ID="portalList" runat="server" CssClass="NormalTextBox SelectList" DataSource="<%# portals %>"
                            DataTextField="Name" DataValueField="ID" Rows="8" Width="200"></asp:ListBox>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <rbfwebui:ImageButton ID="EditBtn" runat="server" AlternateText="Edit selected portal"
                                        TextKey="EDIT_PORTAL" OnClick="EditBtn_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <rbfwebui:ImageButton ID="DeleteBtn" runat="server" AlternateText="Delete selected portal"
                                        TextKey="DELETE_PORTAL" OnClick="DeleteBtn_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <rbfwebui:Button ID="ExportBtn" TextKey="EXPORT" Text="Export" runat="server" OnClick="SerializeBtn_Click" />
                                    <%--<rbfwebui:ImageButton ID="ExportBtn" runat="server" AlternateText="Export selected portal"
                                        TextKey="EXPORT_PORTAL" OnClick="SerializeBtn_Click" />--%>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <p class="Normal">
                <rbfwebui:Label ID="ErrorMessage" runat="server" CssClass="Error" Visible="false"></rbfwebui:Label>
                <rbfwebui:Label ID="SuccessMessage" runat="server" Visible="false"></rbfwebui:Label>
            </p>
        </td>
        <td>
            <table border="0" cellpadding="0" cellspacing="0" style="margin-left: 50px">
                <tr valign="top">
                    <td>
                       <rbfwebui:Label ID="lblTemplates" TextKey="TEMPLATES" runat="server"></rbfwebui:Label>
                        <br />
                        <asp:ListBox ID="templatesList" runat="server" CssClass="NormalTextBox SelectList"
                            DataSource="<%# templates %>" DataTextField="Name" DataValueField="ID" Rows="8"
                            Width="200"></asp:ListBox>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <table>                           
                            <tr>
                                <td>
                                    <rbfwebui:ImageButton ID="btnDeleteTemplate" runat="server" AlternateText="Delete selected template"
                                        TextKey="DELETE_TEMPLATE" onclick="btnDeleteTemplate_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <rbfwebui:ImageButton ID="btnSaveTemplate" runat="server" AlternateText="Export selected template"
                                        TextKey="EXPORT_TEMPLATE" onclick="btnSaveTemplate_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <rbfwebui:Button ID="Button1" TextKey="IMPORT" Text="Import" runat="server" OnClick="btnImport_click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <p class="Normal">
                <rbfwebui:Label ID="TemplateErrorMessage" runat="server" CssClass="Error" Visible="false"></rbfwebui:Label>
                <rbfwebui:Label ID="TemplateSuccessMessage" runat="server" Visible="false"></rbfwebui:Label>
            </p>
        </td>
    </tr>
</table>
