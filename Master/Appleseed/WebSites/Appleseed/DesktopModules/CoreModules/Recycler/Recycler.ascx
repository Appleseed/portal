<%@ Control AutoEventWireup="false" Inherits="Appleseed.Content.Web.Modules.Recycler"
    Language="c#" CodeBehind="Recycler.ascx.cs" %>

<style type="text/css">
    .NormalBold {
        font-weight: bold;
        color: white;
    }

        .NormalBold A {
            padding-right: 20px;
            padding-left: 3px;
            padding-bottom: 0px;
            color: #ffffff;
            padding-top: 0px;
            text-decoration: none;
        }

            .NormalBold A:hover {
                text-decoration: underline;
            }

    .gridHeaderSortASC A {
        background: url(/images/SortOrderAsc.png) no-repeat 100% 50%;
        background-size: 15px;
    }

    .gridHeaderSortDESC A {
        background: url(/images/SortOrderDesc.png) no-repeat 100% 50%;
        background-size: 15px;
    }
</style>

<h3>Modules</h3>
<asp:DataGrid ID="DataGrid1" runat="server" AllowSorting="true" AlternatingItemStyle-BackColor="#fff5c9"
    AutoGenerateColumns="False" CssClass="Normal" HorizontalAlign="Center" Width="95%">
    <HeaderStyle CssClass="NormalBold" />
    <Columns>
        <rbfwebui:TemplateColumn SortExpression="ModuleTitle">
            <HeaderStyle CssClass="gridHeaderSortASC" />
            <HeaderTemplate>
                <rbfwebui:LinkButton ID="linkButton1" runat="server" CommandName="Sort" CommandArgument="ModuleTitle">Module Name</rbfwebui:LinkButton>
            </HeaderTemplate>
            <ItemTemplate>
                <rbfwebui:HyperLink ID="Hyperlink2" NavigateUrl='<%# Appleseed.Framework.HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/Recycler/view.aspx","mid=" + DataBinder.Eval(Container.DataItem,"ModuleID")) %>'
                    runat="server">
					<%#DataBinder.Eval(Container.DataItem,"ModuleTitle")%>
                </rbfwebui:HyperLink>
            </ItemTemplate>
        </rbfwebui:TemplateColumn>
        <rbfwebui:BoundColumn DataField="DateDeleted" DataFormatString="{0:MM/dd/yy}" HeaderText="Date Deleted"
            SortExpression="DateDeleted">
        </rbfwebui:BoundColumn>
        <rbfwebui:BoundColumn DataField="DeletedBy" HeaderText="Deleted By" SortExpression="DeletedBy">
        </rbfwebui:BoundColumn>
        <rbfwebui:BoundColumn DataField="OriginalPageName" HeaderText="Original Page" SortExpression="OriginalPageName">
        </rbfwebui:BoundColumn>
    </Columns>
</asp:DataGrid>
<br />
<br />
<h3>Pages</h3>
<asp:DataGrid ID="dtgPages" runat="server" AllowSorting="true" AlternatingItemStyle-BackColor="#fff5c9" OnSortCommand="dtgPages_SortCommand"
    AutoGenerateColumns="False" CssClass="Normal" HorizontalAlign="Center" Width="95%">
    <HeaderStyle CssClass="NormalBold" />
    <Columns>
        <rbfwebui:TemplateColumn SortExpression="PageName">
            <HeaderStyle CssClass="gridHeaderSortASC" />
            <HeaderTemplate>
                <rbfwebui:LinkButton ID="linkButton2" runat="server" CommandName="Sort" CommandArgument="PageName">Page Name</rbfwebui:LinkButton>
            </HeaderTemplate>
            <ItemTemplate>
                <rbfwebui:HyperLink ID="Hyperlink1" NavigateUrl='<%# Appleseed.Framework.HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/Recycler/view.aspx","pID=" + DataBinder.Eval(Container.DataItem,"ModuleID")+"&prid="+ DataBinder.Eval(Container.DataItem,"OriginalTab") ) %>'
                    runat="server">
					<%#DataBinder.Eval(Container.DataItem,"PageName")%>
                </rbfwebui:HyperLink>
            </ItemTemplate>
        </rbfwebui:TemplateColumn>
        <rbfwebui:BoundColumn DataField="DateDeleted" DataFormatString="{0:MM/dd/yy}" HeaderText="Date Deleted"
            SortExpression="DateDeleted">
        </rbfwebui:BoundColumn>
        <rbfwebui:BoundColumn DataField="DeletedBy" HeaderText="Deleted By" SortExpression="DeletedBy">
        </rbfwebui:BoundColumn>
        <%--<rbfwebui:boundcolumn datafield="OriginalPageName" headertext="Original Page" sortexpression="PageName">
        </rbfwebui:boundcolumn>--%>
    </Columns>
</asp:DataGrid>
