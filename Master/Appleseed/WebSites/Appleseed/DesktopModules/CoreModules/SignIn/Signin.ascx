<%@ Control Inherits="Appleseed.DesktopModules.CoreModules.SignIn.Signin"
    Language="c#" CodeBehind="Signin.ascx.cs" %>
<asp:Panel DefaultButton="LoginBtn" runat="server">
<table align="center" border="0" cellpadding="1" cellspacing="1" width="100%">
    <tr>
        <td class="Normal" nowrap="nowrap">
            <rbfwebui:Localize ID="EmailLabel" runat="server" Text="Email" TextKey="EMAIL">
            </rbfwebui:Localize>
        </td>
    </tr>
    <tr>
        <td nowrap="nowrap">
            <asp:TextBox ID="email" runat="server" Columns="24" CssClass="NormalTextBox"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="Normal" nowrap="nowrap">
            <rbfwebui:Localize ID="PasswordLabel" runat="server" Text="Password" TextKey="PASSWORD">
            </rbfwebui:Localize>
        </td>
    </tr>
    <tr>
        <td nowrap="nowrap">
            <asp:TextBox ID="password" runat="server" Columns="24" CssClass="NormalTextBox" TextMode="password"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td nowrap="nowrap">
            <asp:CheckBox ID="RememberCheckBox" runat="server" CssClass="Normal" Text="<%$ Resources:Appleseed, REMEMBER_LOGIN %> " />
        </td>
    </tr>
    <tr>
        <td nowrap="nowrap">
			
            <rbfwebui:Button ID="LoginBtn" runat="server" CssClass="CommandButton" EnableViewState="False"
                Text="Sign in" TextKey="SIGNIN" />
			
        </td>
    </tr>
    <tr>
        <td nowrap="nowrap">
            <rbfwebui:Button ID="RegisterBtn" runat="server" CssClass="CommandButton" EnableViewState="False"
                Text="Register" TextKey="REGISTER" />
        </td>
    </tr>
    <tr>
        <td nowrap="nowrap">
            <rbfwebui:Button ID="SendPasswordBtn" runat="server" CssClass="CommandButton" EnableViewState="False"
                Text="Forgotten Password?" TextKey="SIGNIN_PWD_LOST" />
        </td>
    </tr>
    <tr>
        <td>
            <rbfwebui:Label ID="Message" runat="server" CssClass="Error"></rbfwebui:Label>
        </td>
    </tr>
</table>
</asp:Panel>