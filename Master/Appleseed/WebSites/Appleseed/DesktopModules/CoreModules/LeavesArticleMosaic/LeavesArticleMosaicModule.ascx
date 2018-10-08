<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LeavesArticleMosaicModule.ascx.cs" Inherits="Appleseed.DesktopModules.CoreModules.LeavesArticleMosaic.LeavesArticleMosaicModule" %>
<style> 
    .artItmImage {
        width: 100%;
        height: 150px;
    }

    .artItm {
        min-height: 330px;
    }

    .artItmReadMore {
        margin-bottom: 10px;
        margin-top: 10px;
    }

    .tblArtMscMod {
        width: 100% !important;
    }

        .tblArtMscMod tr td:first-child {
            width: 210px;
        }

        .tblArtMscMod tr td img {
            max-width: 200px;
            max-height: 200px;
        }

        .tblArtMscMod tr td {
            vertical-align: top;
            padding: 5px;
        }
</style>
<div id="divLeavesArticles" class="LeavesArticles">
    <asp:Literal ID="ltrResults" runat="server"></asp:Literal>
    <%--<table class='tblArtMscMod'><tr><td><img src='' /></td><td></td></tr></table>--%>
</div>
