<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogInGoogle.aspx.cs" Inherits="Appleseed.DesktopModules.CoreModules.SignIn.LogInGoogle" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:HyperLink ID="GotoAuthSubLink" runat="server" />
    </div>
    </form>
    <script type="text/javascript">
        function close() {
            if (window.opener) {
                //window.opener.location.href = window.opener.location.href;
                window.close();
            }
        }

        
    </script>
</body>
</html>
