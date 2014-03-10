<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        <%: ViewData["Message"] %></h2>
    <ul>
        <li>Crear el área portable mediante "Add new project..." en la carpeta "PortableAreas".</li>
        <li>Agregar una vista con el nombre del área mediante "Add View..." en la carpeta "HostWebSite/Views/Home".</li>
        <li>Agregar una acción en en HomeController con el nombre del área y que retorne "View()".</li>
        <li>Agregar una pestaña en "Site.master" que llame a la acción anterior. </li>
    </ul>
    <h2>
        Advertencia:</h2>
    <ul>
        <li>No utilizar forms en las áreas (da conflictos al integrar con Appleseed).</li>
    </ul>
</asp:Content>
