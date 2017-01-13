<%@ control autoeventwireup="false" inherits="Appleseed.Content.Web.Modules.SigninCool"
    language="c#" Codebehind="SigninCool.ascx.cs" %>

<style>
    /* SignIn Cool Styling */
    .signin-page {
        width: 300px;
        margin: 0 auto;
        font-family: "Lato", "Helvetica Neue", Helvetica, Arial, sans-serif;
        font-size: 15px;
    }
    .signin-page .input-row {
        margin-bottom: 10px;
    }
    .signin-page input[type="text"],
    .signin-page input[type="password"] {
        width: 300px;
        height: 40px;
        font-size: 15px;
        border: 1px solid #cccccc!important;
        box-shadow: none;
    }
    .signin-page input[type="checkbox"]{
        margin: 0px 5px 0px 10px;
        vertical-align: -2px;
    }
    .signin-page input[type="checkbox"] + label {
        font-size: 15px;
    }
    .signin-page .forgot-link {
        font-size: 15px;
        text-decoration: none;
    }
    .signin-page .forgot-link:hover {
        color: #6696ff;
    }
    .signin-page .login-button {
        width: 300px;
        height: 44px;
        color: #fff;
        cursor: pointer;
        background: #1a242f;
        text-decoration: none;
    }
</style>


<div class="signin-page">
    
    <asp:Panel DefaultButton="LoginBtn" runat="server">

    <div id="Table1" class="login-module landing-content">
        <div class="input-row">
            <span class="username-label login-label">
                <rbfwebui:localize id="EmailLabel" runat="server" text="Username:" textkey="EMAIL">
                </rbfwebui:localize>
            </span>
            <span class="username-textbox login-textbox">
                <asp:textbox id="email" runat="server" columns="24" cssclass="NormalTextBox"></asp:textbox>
            </span>
        </div>
        <div class="input-row">
            <span class="password-label login-label">
                <rbfwebui:localize id="PasswordLabel" runat="server" text="Password:" textkey="PASSWORD">
                </rbfwebui:localize>:
            </span>
            <span class="password-textbox login-textbox">
                <asp:textbox id="password" runat="server" columns="24" cssclass="NormalTextBox" textmode="password"></asp:textbox>
            </span>
        </div>
        <div class="remember-box">
            <span class= "show">
                <asp:checkbox id="RememberCheckBox" runat="server" text="Remember Login" textkey="REMEMBER_LOGIN" />
            </span>
            <span>
                <p>
                    <rbfwebui:button id="LoginBtn" runat="server" cssclass="CommandButton login-button" enableviewstate="False" text="Login" textkey="SIGNIN" />
                </p>
            </span>
        </div>
        <div class="send-register-box">
            <span>
                <rbfwebui:linkbutton id="SendPasswordBtn" runat="server" cssclass="forgot-link"
                    enableviewstate="False" text="Forgot your password? Click here." textkey="SIGNIN_SEND_PWD"></rbfwebui:linkbutton>
               <%-- <a href="/302" class="forgot-link">Forgot your password? Click here.</a> --%>
            </span>
            <span class="hide">
                <p>
                    <rbfwebui:linkbutton id="RegisterBtn" runat="server" cssclass="CommandButton" enableviewstate="False"
                        textkey="REGISTER">Register</rbfwebui:linkbutton>
                </p>
            </span>
        </div>
        <div class="error-message">
            <span>
                <rbfwebui:label id="Message" runat="server" cssclass="Error"></rbfwebui:label>
            </span>
        </div>
        <div>
            <asp:placeholder id="CoolTextPlaceholder" runat="server"></asp:placeholder>
        </div>
    </div>
    </asp:Panel>

</div>