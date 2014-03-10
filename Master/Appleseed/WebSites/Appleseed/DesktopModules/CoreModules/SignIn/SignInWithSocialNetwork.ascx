<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SignInWithSocialNetwork.ascx.cs" Inherits="Appleseed.DesktopModules.CoreModules.SignIn.SignInWithSocialNetwork" %>
<%@ Register src="Signin.ascx" tagname="Signin" tagprefix="uc1" %>
<%@ Register src="../SocialNetworksButtons/SocialNetworkButtons.ascx" tagname="SocialNetworkButtons" tagprefix="uc2" %>

<div>
    <uc1:Signin ID="Signin1" runat="server" />
</div>
<div id="SocialButtons" runat="server">
    <uc2:SocialNetworkButtons ID="SocialNetworkButtons1" runat="server" />
</div>
