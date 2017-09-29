<%@ Control AutoEventWireup="false" Inherits="Appleseed.Content.Web.Modules.SigninCool"
    Language="c#" CodeBehind="SigninCool.ascx.cs" %>

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
            border: 1px solid #cccccc !important;
            box-shadow: none;
        }

        .signin-page input[type="checkbox"] {
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

    .desktopmodules_coremodules_signin_signincool_ascx .module_Body {
        width: 1000px;
    }

    @media screen and (max-width: 768px) {
        .desktopmodules_coremodules_signin_signincool_ascx .module_Body {
            width: auto;
        }

        table {
            border-collapse: separate;
            border-spacing: 0;
            margin-left: auto;
            margin-right: auto;
        }
    }
</style>


<div class="signin-page">

    <asp:Panel DefaultButton="LoginBtn" runat="server">

        <div id="Table1" class="login-module landing-content">
            <div class="input-row">
                <span class="username-label login-label">
                    <rbfwebui:Localize ID="EmailLabel" runat="server" Text="Username:" TextKey="EMAIL">
                    </rbfwebui:Localize>
                </span>
                <span class="username-textbox login-textbox">
                    <asp:TextBox ID="email" runat="server" Columns="24" CssClass="NormalTextBox"></asp:TextBox>
                </span>
            </div>
            <div class="input-row">
                <span class="password-label login-label">
                    <rbfwebui:Localize ID="PasswordLabel" runat="server" Text="Password" TextKey="PASSWORD">
                    </rbfwebui:Localize>
                </span>
                <span class="password-textbox login-textbox">
                    <asp:TextBox ID="password" runat="server" Columns="24" CssClass="NormalTextBox" TextMode="password"></asp:TextBox>
                </span>
            </div>
            <div class="remember-box">
                <span class="show">
                    <asp:CheckBox ID="RememberCheckBox" runat="server" Text="Remember Login" textkey="REMEMBER_LOGIN" />
                </span>
                <span>
                    <p>
                        <rbfwebui:Button ID="LoginBtn" runat="server" CssClass="CommandButton login-button" EnableViewState="False" Text="Sign in" TextKey="SIGNIN" />
                    </p>
                </span>
            </div>
            <div class="send-register-box">
                <span>
                    <rbfwebui:LinkButton ID="SendPasswordBtn" runat="server" CssClass="forgot-link"
                        EnableViewState="False" Text="Forgot your password? Click here."></rbfwebui:LinkButton>
                    <%-- <a href="/302" class="forgot-link">Forgot your password? Click here.</a> --%>
                </span>
                <span class="hide">
                    <p>
                        <rbfwebui:LinkButton ID="RegisterBtn" runat="server" CssClass="CommandButton" EnableViewState="False"
                            TextKey="REGISTER">Register</rbfwebui:LinkButton>
                    </p>
                </span>
            </div>
            <div class="error-message">
                <span>
                    <rbfwebui:Label ID="Message" runat="server" CssClass="Error"></rbfwebui:Label>
                </span>
            </div>
            <div>
                <asp:PlaceHolder ID="CoolTextPlaceholder" runat="server"></asp:PlaceHolder>
            </div>
        </div>
    </asp:Panel>

</div>
