<%@ control autoeventwireup="false" inherits="Appleseed.Content.Web.Modules.SigninCool"
    language="c#" Codebehind="SigninCool.ascx.cs" %>
<asp:Panel DefaultButton="LoginBtn" runat="server">
<table id="Table1" align="center" border="0" cellpadding="1" cellspacing="1" width="100%">
    <tr>
        <td>
            <table align="center" cellpadding="3" cellspacing="1" class="Normal" width="150">
                <tr>
                    <td nowrap="nowrap">
                        <rbfwebui:localize id="EmailLabel" runat="server" text="Email" textkey="EMAIL">
                        </rbfwebui:localize>:
                    </td>
                    <td nowrap="nowrap">
                        <rbfwebui:localize id="PasswordLabel" runat="server" text="Password" textkey="PASSWORD">
                        </rbfwebui:localize>:</td>
                </tr>
                <tr>
                    <td nowrap="nowrap">
                        <asp:textbox id="email" runat="server" columns="24" cssclass="NormalTextBox" width="130"></asp:textbox></td>
                    <td nowrap="nowrap">
                        <asp:textbox id="password" runat="server" columns="24" cssclass="NormalTextBox" textmode="password"
                            width="130"></asp:textbox></td>
                </tr>
                <tr>
                    <td nowrap="nowrap">
                        <asp:checkbox id="RememberCheckBox" runat="server" text="Remember Login" textkey="REMEMBER_LOGIN" /></td>
                    <td nowrap="nowrap">
                        <p align="right">
                            <rbfwebui:button id="LoginBtn" runat="server" cssclass="CommandButton" enableviewstate="False"
                                text="Sign in" textkey="SIGNIN" /></p>
                    </td>
                </tr>
                <tr>
                    <td nowrap="nowrap">
                        <rbfwebui:linkbutton id="SendPasswordBtn" runat="server" cssclass="CommandButton"
                            enableviewstate="False" text="Send me password" textkey="SIGNIN_SEND_PWD"></rbfwebui:linkbutton></td>
                    <td nowrap="nowrap">
                        <p align="right">
                            <rbfwebui:linkbutton id="RegisterBtn" runat="server" cssclass="CommandButton" enableviewstate="False"
                                textkey="REGISTER">
		Register</rbfwebui:linkbutton></p>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <rbfwebui:label id="Message" runat="server" cssclass="Error"></rbfwebui:label></td>
                </tr>
            </table>
        </td>
        <td>
            &nbsp;</td>
        <td>
            <asp:placeholder id="CoolTextPlaceholder" runat="server"></asp:placeholder>
        </td>
    </tr>
</table>
</asp:Panel>

