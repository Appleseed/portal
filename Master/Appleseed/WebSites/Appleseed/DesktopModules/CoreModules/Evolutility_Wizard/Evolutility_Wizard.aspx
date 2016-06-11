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
