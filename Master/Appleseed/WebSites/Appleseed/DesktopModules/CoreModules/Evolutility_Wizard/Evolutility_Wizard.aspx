<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Evolutility_Wizard.aspx.cs" Inherits="Appleseed.DesktopModules.CoreModules.Evolutility_Wizard.Evolutility_Wizard1" %>

<%--<%@ Register TagPrefix="uc" TagName="Spinner" Src="/DesktopModules/CoreModules/Evolutility_Wizard/Evolutility_Wizard.aspx" %>--%>

<%@ Register Assembly="Evolutility.Wizard" Namespace="Evolutility" TagPrefix="EVOL" %>  

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

<link rel="stylesheet" type="text/css" href="/aspnet_client/evolutility/css/evol.css" id="evolcss" runat="server" />
<link rel="stylesheet" type="text/css" href="/aspnet_client/evolutility/css/evolwiz.css" id="evolwizcss" runat="server"/>

</head>
<body>
    <form id="form1" runat="server">
         <div>
       <EVOL:Wizard ID="evoWizard" runat="server"  
        BuildDatabase="true" MustLogin="false" 
			PathWeb="/aspnet_client/evolutility/XML/" PathXML="/aspnet_client/evolutility/XML/" ShowASPX="false" ShowSkin="false"
			ShowSQL="true" ShowXML="true" VirtualPathPictures="/aspnet_client/evolutility/pixEvo/" WizardMode="Build"  />
    </div> 
    </form>
</body>
</html>
<script src="/aspnet_client/jQuery/jquery-1.8.3.min.js"></script>
<script type="text/javascript">
    $(document).ready(function () {
        var edf = $("a[href*='EvoDicoForm.aspx']");
        $("a[href*='EvoDicoForm.aspx']").attr("href", $(edf).attr("href") + "&mid=" + GetParameterValues("mid"));
        var edft = $("a[href*='EvoDicoTest.aspx']");
        $("a[href*='EvoDicoTest.aspx']").attr("href", $(edft).attr("href") + "&mid=" + GetParameterValues("mid"));
        var edfd = $("a[href*='EvoDoc.aspx']");
        $("a[href*='EvoDoc.aspx']").attr("href", $(edfd).attr("href") + "&mid=" + GetParameterValues("mid"));
    });
    function GetParameterValues(param) {
        var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < url.length; i++) {
            var urlparam = url[i].split('=');
            if (urlparam[0] == param) {
                return urlparam[1];
            }
        }
    }
</script>