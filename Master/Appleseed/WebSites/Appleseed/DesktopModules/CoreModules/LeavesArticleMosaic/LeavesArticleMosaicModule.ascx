<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LeavesArticleMosaicModule.ascx.cs" Inherits="Appleseed.DesktopModules.CoreModules.LeavesArticleMosaic.LeavesArticleMosaicModule" %>
<style>
     .artItmImage{
         width:100%;
         height:150px;
     }
     .artItm{
         min-height:330px;
     }
     .artItmReadMore{
         margin-bottom:10px;
         margin-top:10px;
     }
</style>
<div id="divLeavesArticles" class="LeavesArticles">
    <asp:Literal ID="ltrResults" runat="server"></asp:Literal>
</div>