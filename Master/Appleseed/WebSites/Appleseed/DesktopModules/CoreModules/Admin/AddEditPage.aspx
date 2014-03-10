<%@ Page AutoEventWireup="false" Inherits="Appleseed.Content.Web.Modules.AddEditPage"
    Language="c#" MasterPageFile="~/Shared/SiteMasterDefault.master" Codebehind="AddEditPage.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <table border="0" cellpadding="4" cellspacing="0" width="98%">
        <tr valign="top">
            <td width="150">
                &nbsp;
            </td>
            <td width="*">
                <table cellpadding="0" cellspacing="0" width="500">
                    <tr>
                        <td align="left" class="Head">
                            <rbfwebui:Localize ID="Literal1" runat="server" Text="Add/Edit Item" TextKey="ADDEDITITEMPAGE_TITLE">
                            </rbfwebui:Localize>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr noshade="noshade" size="1" />
                            <asp:PlaceHolder ID="AddEditControlPlaceHolder" runat="server"></asp:PlaceHolder>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
