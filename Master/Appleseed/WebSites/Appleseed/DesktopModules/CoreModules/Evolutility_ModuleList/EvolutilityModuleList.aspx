<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EvolutilityModuleList.aspx.cs" Inherits="Appleseed.DesktopModules.CoreModules.Evolutility_ModuleList.EvolutilityModuleList" %>

<%@ Register TagPrefix="uc" TagName="Spinner" Src="/DesktopModules/CoreModules/Evolutility_ModuleList/EvolutilityModuleList.aspx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <uc:Spinner id="Spinner1"
                runat="server"
                MinValue="1"
                MaxValue="10" />
    </div>
    </form>
</body>
</html>
