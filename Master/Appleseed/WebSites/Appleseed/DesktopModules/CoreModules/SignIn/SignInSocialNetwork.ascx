<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SignInSocialNetwork.ascx.cs" Inherits="Appleseed.DesktopModules.CoreModules.SignIn.SignInSocialNetwork" %>


<%@ Register src="../SocialNetworksButtons/SocialNetworkButtons.ascx" tagname="SocialNetworkButtons" tagprefix="uc2" %>

<%@ Register src="Signin.ascx" tagname="Signin" tagprefix="uc1" %>

<div id="SignInSocialNetworkButtons" runat="server">
    
    <uc2:SocialNetworkButtons ID="SocialNetworkButtons1" runat="server" />
    
</div>

<div id="SignInCommon" runat="server">
    <uc1:Signin ID="Signin1" runat="server" />
</div>














