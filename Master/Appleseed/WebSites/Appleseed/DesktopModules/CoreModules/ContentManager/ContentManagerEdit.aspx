<%@ Page AutoEventWireup="false" Inherits="Appleseed.Content.Web.Modules.ContentManagerEdit"
    Language="c#" MasterPageFile="~/Shared/SiteMasterDefault.master" Codebehind="ContentManagerEdit.aspx.cs" %>

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
                            <rbfwebui:Label ID="Label1" runat="server" Text="Content Manager 3rd Party Module-Support Installer"
                                TextKey="CONTENT_MGR_TITLE"></rbfwebui:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr noshade="noshade" size="1" />
                        </td>
                    </tr>
                </table>
                <table id="tableInstaller" runat="server" border="0" cellpadding="3" cellspacing="0"
                    width="750">
                    <tr>
                        <td class="SubHead" nowrap="nowrap" width="106">
                            <rbfwebui:Label ID="Label2" runat="server" Text="Friendly Name" TextKey="INSTALLER_FILE">Installer file</rbfwebui:Label>:
                        </td>
                        <td width="6">
                        </td>
                        <td>
                            <asp:TextBox ID="InstallerFileName" runat="server" Columns="30" CssClass="NormalTextBox"
                                MaxLength="150" Width="390"></asp:TextBox>
                        </td>
                        <td width="10">
                        </td>
                        <td class="Normal" width="250">
                            <asp:RequiredFieldValidator ID="Requiredfieldvalidator1" runat="server" ControlToValidate="InstallerFileName"
                                CssClass="Error" Display="Dynamic" ErrorMessage="Enter an Installer Name" textkey="ERROR_ENTER_A_FILE_NAME"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
                <p>
                    <rbfwebui:LinkButton ID="UpdateButton" runat="server" class="CommandButton" Text="Update"></rbfwebui:LinkButton>&nbsp;
                    <rbfwebui:LinkButton ID="CancelButton" runat="server" CausesValidation="False" class="CommandButton"
                        Text="Cancel"></rbfwebui:LinkButton>&nbsp;
                    <rbfwebui:LinkButton ID="DeleteButton" runat="server" CausesValidation="False" class="CommandButton"
                        Text="Delete this module type"></rbfwebui:LinkButton></p>
                <p>
                    <rbfwebui:Label ID="lblErrorDetail" runat="server" CssClass="error"></rbfwebui:Label></p>
            </td>
        </tr>
    </table>
</asp:Content>
