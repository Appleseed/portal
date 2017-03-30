<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SocialNetworkButtons.ascx.cs" Inherits="Appleseed.DesktopModules.CoreModules.SocialNetworksButtons.SocialNetworkButtons" %>

<input type="hidden" runat="server" id="appidfacebook"/>
<div id="fb-root"></div>
<script type="text/javascript" src="//connect.facebook.net/en_US/all.js"></script>
 
<table cellpadding="0" cellspacing="0" id="signInContainer" class="signInContainer" runat="server">
    <tbody>
        <tr>
            <td>
                <div runat="server" id="loginfb_div">
                    <fb:login-button autologoutlink="true" scope="email">Login with Facebook</fb:login-button>
                    
                </div>
            </td>
        </tr>
         <tr>
            <td>
                <div runat="server" id="logintwit_div">
                    <a id="TwitterLogin" href="#" onclick="PopUp('<%= getTwitterLink() %>');return false;"><img src="<%= Appleseed.Framework.HttpUrlBuilder.BuildUrl("~/images/sign-in-with-twitter-l.png") %>" alt="Login with twitter"/> </a>
                </div>
            </td>
        </tr>
        <tr>
            <td>

                <div runat="server" id="google_div">
                <a id="GoogleLink" href="#" onclick="PopUp('<%= getGoogleLink() %>');return false;"><img src="<%= Appleseed.Framework.HttpUrlBuilder.BuildUrl("~/images/sign-in-google.png") %>" alt="Login with google"/> </a>
                <%--    <asp:HyperLink runat="server" Text="LoginGoogle" ID="googleLogin" ImageUrl="~/images/sign-in-google.png" o></asp:HyperLink>--%>
                   
                </div>
                <%--<asp:Label runat="server" ForeColor="Red" ID="errtwit" Visible="false">Twitter settings are not correct</asp:Label>--%>
            </td>
        </tr>
         <tr>
            <td>
                <div runat="server" id="linkedin_div">
                    <a id="LinkedInLink" href="#" onclick="PopUp('<%= getLinkedInLink() %>');return false;"><img src="<%= Appleseed.Framework.HttpUrlBuilder.BuildUrl("~/images/sign-in-with-linkedin.jpg") %>" alt="Login with linkedin"/> </a>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblError" runat="server" Visible="false" EnableViewState="false" ForeColor="Red"
                    Text="Ha ocurrido un error. Intente nuevamente más tarde." />
                <asp:Label ID="lblPassSent" runat="server" Visible="false" EnableViewState="false" ForeColor="Red"
                    Text="La contrase&ntilde;a ha sido enviada a la direcci&oacute;n ingresada. Revise su casilla de correos para poder acceder al sitio web." />
                <asp:Label ID="lblPassSentErr" runat="server" Visible="false" EnableViewState="false" ForeColor="Red"
                    Text="La contrase&ntilde;a ha sido enviada a la direcci&oacute;n ingresada. Revise su casilla de correos para poder acceder al sitio web." />
                
            </td>
        </tr>
    </tbody>
</table>

<asp:HiddenField ID="appId" runat="server" />

<script type="text/javascript">
    var appId = $(".signInContainer").next().val();
    FB.init({
        appId: appId,
        status: false, // check login status
        cookie: true, // enable cookies to allow the server to access the session
        xfbml: true,  // parse XFBML
        oauth: true
    });

    FB.Event.subscribe('auth.login', function (response) {
        location.href = '/';
    });

    function gotoTwitter(link) {
        window.parent.location = link
    }

    function PopUp(url) {
        window.open(url, 'Google login', "height= 500,width= 600, location = 0, status = 1, resizable = 0, scrollbars=1, toolbar = 0");
        return true;
    }

    
 </script>
