<%@ control autoeventwireup="false" inherits="Appleseed.Content.Web.Modules.Recycler"
    language="c#" Codebehind="Recycler.ascx.cs" %>
<asp:datagrid id="DataGrid1" runat="server" allowsorting="true" alternatingitemstyle-backcolor="#fff5c9"
    autogeneratecolumns="False" cssclass="Normal" horizontalalign="Center" width="95%">
    <headerstyle cssclass="NormalBold" />
    <columns>
        <rbfwebui:templatecolumn sortexpression="ModuleTitle">
            <headertemplate>
                <rbfwebui:LinkButton ID="linkButton1" runat="server" CommandName="Sort" CommandArgument="ModuleName">Module Name</rbfwebui:LinkButton>
            </headertemplate>
            <itemtemplate>
                <rbfwebui:HyperLink ID="Hyperlink2" NavigateUrl='<%# Appleseed.Framework.HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/Recycler/view.aspx","mid=" + DataBinder.Eval(Container.DataItem,"ModuleID")) %>'
                    runat="server">
					<%#DataBinder.Eval(Container.DataItem,"ModuleTitle")%>
                </rbfwebui:HyperLink>
            </itemtemplate>
        </rbfwebui:templatecolumn>
        <rbfwebui:boundcolumn datafield="DateDeleted" dataformatstring="{0:MM/dd/yy}" headertext="Date Deleted"
            sortexpression="DateDeleted">
        </rbfwebui:boundcolumn>
        <rbfwebui:boundcolumn datafield="DeletedBy" headertext="Deleted By" sortexpression="DeletedBy">
        </rbfwebui:boundcolumn>
        <rbfwebui:boundcolumn datafield="OriginalPageName" headertext="Original Page" sortexpression="PageName">
        </rbfwebui:boundcolumn>
    </columns>
</asp:datagrid>
