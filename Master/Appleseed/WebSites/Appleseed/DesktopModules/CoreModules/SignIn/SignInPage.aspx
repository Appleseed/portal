<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignInPage.aspx.cs" Inherits="Appleseed.DesktopModules.CoreModules.SignIn.SignInPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server" style="visibility: hidden">
    <title></title>
    <script type="text/javascript" src="http://connect.facebook.net/en_US/all.js"></script>
    <script type="text/javascript" src="../../../aspnet_client/jQuery/jquery-1.7.2.min.js"></script>
    

    
   
</head>
<body style="background-image: none !important">
    <form id="formSignIn" runat="server">
    <div>
        <asp:PlaceHolder runat="server" ID="signin"></asp:PlaceHolder>
    </div>
    </form>
</body>
</html>
