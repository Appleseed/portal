<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="SignInLoginView" Codebehind="SignInLoginView.ascx.cs" %>
<asp:Login ID="loginControl" runat="server" OnAuthenticate="loginControl_Authenticate" o>
    <LabelStyle CssClass="Normal" />
    <LoginButtonStyle CssClass="CommandButton" />
    <CheckBoxStyle CssClass="Normal" />
    <TextBoxStyle CssClass="NormalTextBox" />
    <FailureTextStyle CssClass="Error" />
</asp:Login>
