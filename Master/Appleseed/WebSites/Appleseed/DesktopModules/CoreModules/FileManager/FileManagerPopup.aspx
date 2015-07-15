<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/ModalMaster.Master" AutoEventWireup="true" CodeBehind="FileManagerPopup.aspx.cs" Inherits="Appleseed.DesktopModules.CoreModules.FileManager.FileManagerPopup" %>

<%--<%@ Register Src="~/DesktopModules/CoreModules/FileManager/FileManager.ascx" TagPrefix="uc1" TagName="FileManager" %>--%>
<%@ Register Src="~/DesktopModules/CoreModules/MVC/MVCModule.ascx" TagPrefix="uc1" TagName="MVCModule" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <uc1:MVCModule runat="server" ID="MVCModule" />
</asp:Content>

