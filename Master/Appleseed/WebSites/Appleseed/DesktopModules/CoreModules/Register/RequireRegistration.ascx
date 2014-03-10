<%@ control autoeventwireup="false" inherits="Appleseed.Admin.RequireRegistration"
    language="c#" Codebehind="RequireRegistration.ascx.cs" %>
<rbfwebui:label id="LabelRegister" runat="server" cssclass="Normal" text="To use this module you have to register."
    textkey="REGISTER_REQUIRED"></rbfwebui:label><br />
<rbfwebui:label id="LabelAlreadyAccount" runat="server" cssclass="Normal" text="If you already have an account."
    textkey="ALREADY_REGISTERED1"></rbfwebui:label>&#160;
<rbfwebui:hyperlink id="SignInHyperLink" runat="server" text="Log in" textkey="SIGNIN"></rbfwebui:hyperlink>.<br />
<rbfwebui:label id="LabelRegisterNow" runat="server" cssclass="Normal" text="Register and Sign In Now."
    textkey="REGISTER_NOW"></rbfwebui:label>
<rbfwebui:hyperlink id="RegisterHyperlink" runat="server" text="Register" textkey="REGISTER"></rbfwebui:hyperlink>.
