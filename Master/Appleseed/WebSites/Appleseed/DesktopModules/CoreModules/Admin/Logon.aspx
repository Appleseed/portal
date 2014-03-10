<%@ Page Language="c#" AutoEventWireup="false" Inherits="Appleseed.Admin.LogonPage"
    MasterPageFile="~/Shared/SiteMasterDefault.master" Codebehind="Logon.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td align="center">
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td align="left">
                                <table class="rb_DefaultLayoutTable" cellspacing="0" cellpadding="0" id="Table1"
                                    border="0">
                                    <tbody>
                                        <tr>
                                            <td>
                                                <asp:PlaceHolder ID="signIn" runat="server"></asp:PlaceHolder>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
