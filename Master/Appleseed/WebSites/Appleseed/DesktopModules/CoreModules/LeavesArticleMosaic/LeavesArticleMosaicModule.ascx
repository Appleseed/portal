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

    .currentPage {
        font-weight: bold;
        text-decoration:underline;
    }

</style>
<div id="divLeavesArticles" class="LeavesArticles">
    <asp:Repeater ID="apiResults" runat="server">
        <HeaderTemplate>
            <table>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <table class='<%# Eval("ColumnCSS") %> tblArtMscMod'>
                        <tr>
                            <td>
                                <img src='<%# Eval("ImageUrl") %>' /></td>
                            <td>
                                <div class='artItmTitle'><a target="_blank" href='<%# Eval("PageUrl") %>'><%# Eval("Title") %></a></div>
                                <div class='artItmDesc'><%# Eval("Content") %></div>
                                <div class='artItmReadMore'><a target="_blank" href='<%# Eval("PageUrl") %>'>Read More</a></div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    <asp:Repeater ID="apiResultsPages" runat="server">
        <HeaderTemplate>
            <%--<a  href="<%= this.APIPagination.FirstPageUrl %>">First</a>--%>
        </HeaderTemplate>
        <ItemTemplate>
            <a href='<%# Eval("Url") %>' class='<%# Eval("CssClass") %>' ><%# Eval("PageIndex") %></a>
        </ItemTemplate>
        <FooterTemplate>
            <%--<a href="<%= this.APIPagination.LastPageUrl %>">Last</a><span> |--%> Total Pages: <%= this.APIPagination.TotalPages %></span><span> | Total Results: <%= this.APIPagination.TotalItems %></span>
        </FooterTemplate>
    </asp:Repeater>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        if (!$('.CenterCol').hasClass('container')) {
            $(".CenterCol").addClass("admin-wrapper container");
            $("#contentpane").addClass("row");
        }
    });
</script>
