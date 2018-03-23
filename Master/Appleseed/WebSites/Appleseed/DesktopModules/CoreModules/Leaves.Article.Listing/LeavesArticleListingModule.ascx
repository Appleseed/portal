<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LeavesArticleListingModule.ascx.cs" Inherits="Appleseed.DesktopModules.CoreModules.LeavesArticleListing.LeavesArticleListingModule" %>
<script>
    $(function () {
        $("#tabs").tabs();
    });
</script>
<div id="tabs">
    <ul>
        <asp:Literal ID="ltrTags" runat="server"></asp:Literal>
    </ul>
    <asp:Literal ID="ltrResults" runat="server"></asp:Literal>
</div>
