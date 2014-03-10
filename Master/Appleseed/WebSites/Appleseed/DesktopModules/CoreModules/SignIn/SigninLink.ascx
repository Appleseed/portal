<%@ control autoeventwireup="false" inherits="Appleseed.Content.Web.Modules.SigninLink"
    language="c#" Codebehind="SigninLink.ascx.cs" %>
	<asp:Panel DefaultButton="SignInBtn" runat="server">
<table align="center" cellpadding="1" cellspacing="1" class="Normal" width="100%">
    <tr>
        <td>
            <rbfwebui:linkbutton id="SignInBtn" runat="server" causesvalidation="False" cssclass="CommandButton"
                enableviewstate="False" text="Sign In" textkey="SIGNIN">
				Sign In</rbfwebui:linkbutton>
        </td>
    </tr>
    <tr>
        <td>
            <rbfwebui:linkbutton id="RegisterBtn" runat="server" causesvalidation="False" cssclass="CommandButton"
                enableviewstate="False" text="Register" textkey="REGISTER">
				Register</rbfwebui:linkbutton>
        </td>
    </tr>
</table>
</asp:Panel>
