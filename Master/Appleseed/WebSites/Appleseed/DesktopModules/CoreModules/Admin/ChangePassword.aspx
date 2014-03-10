<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs"
     MasterPageFile="~/Shared/SiteMasterDefault.master" Inherits="Appleseed.DesktopModules.CoreModules.Admin.ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <div class="div_ev_Table">
        <table border="0" cellpadding="4" cellspacing="0" width="98%">
            <tr>
                <td align="left" class="Head">
                    <rbfwebui:Localize ID="Literal1" runat="server" Text="Change your password" TextKey="CHANGE_PWD_HEADER">
                    </rbfwebui:Localize>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr noshade="noshade" size="1" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="4" cellspacing="0" width="98%">
            <tr>
                <td class="Normal">
                   <asp:Label runat="server" ID="lblMessage" CssClass="Normal"></asp:Label>
                </td>
            </tr>
            <tr runat="server" id="trFields">
                <td>
                    <table border="0" cellpadding="4" cellspacing="0">
                        <tr>
                            <td><rbfwebui:Localize ID="Localize1" runat="server" Text="New password" TextKey="CHANGE_PWD_ENTER_NEW_PWD" CssClass="Normal" /></td>
                            <td><asp:TextBox runat="server" ID="txtPass" TextMode="Password"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td><rbfwebui:Localize ID="Localize2" runat="server" Text="New password again" TextKey="CHANGE_PWD_ENTER_NEW_PWD_AGAIN" CssClass="Normal" /></td>
                            <td><asp:TextBox runat="server" ID="txtPass2" TextMode="Password"></asp:TextBox></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <p>
            <rbfwebui:button id="SaveBtn" runat="server" cssclass="CommandButton" enableviewstate="False"
                text="Save" textkey="SAVE" />

            <rbfwebui:button id="CancelBtn" runat="server" cssclass="CommandButton" enableviewstate="False"
                text="Cancel" textkey="CANCEL" />

            <rbfwebui:button id="GoHomeBtn" runat="server" cssclass="CommandButton" enableviewstate="False" Visible="false"
                text="Go to home page" textkey="CHANGE_PWD_GO_TO_HOME_PAGE" />

        </p>
    </div>
</asp:Content>
