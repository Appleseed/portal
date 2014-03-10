<%@ Control Language="C#" AutoEventWireup="true" Inherits="DesktopModules_CoreModules_MVC_MVCModule"
    CodeBehind="MVCModule.ascx.cs" %>
<span runat="server">       
    <% 
        try
        {
            System.Web.Routing.RouteValueDictionary dict = new System.Web.Routing.RouteValueDictionary();

            foreach (var de in this.Settings)
            {

                dict.Add(de.Key.ToString(), de.Value);
            }
            dict.Add("area", this.AreaName.ToString());
            dict.Add("moduleId", this.ModuleID);
            Html.RenderAction(this.ActionName, this.ControllerName, dict);
        }
        catch (Exception exc)
        {
            
            ErrorHandler.Publish(LogLevel.Error, exc);

    %>
    Couldn´t load module
    <%} %>
</span>