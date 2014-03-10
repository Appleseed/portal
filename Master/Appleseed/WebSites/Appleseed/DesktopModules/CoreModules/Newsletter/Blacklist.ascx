<%@ control autoeventwireup="false" inherits="Appleseed.Admin.Blacklist"
    language="c#" Codebehind="Blacklist.ascx.cs" %>
<asp:datagrid id="myDataGrid" runat="server" allowsorting="True" autogeneratecolumns="False"
    border="0" enableviewstate="False" onsortcommand="SortList" width="100%">
    <columns>
        <rbfwebui:boundcolumn datafield="Name" headertext="Name" sortexpression="Name" textkey="BLACKLIST_NAME">
            <headerstyle cssclass="NormalBold" />
            <itemstyle cssclass="Normal" />
        </rbfwebui:boundcolumn>
        <rbfwebui:boundcolumn datafield="Email" headertext="Email" sortexpression="Email"
            textkey="BLACKLIST_EMAIL">
            <headerstyle cssclass="NormalBold" />
            <itemstyle cssclass="Normal" />
        </rbfwebui:boundcolumn>
        <rbfwebui:boundcolumn datafield="SendNewsletter" headertext="Send Newsletter" sortexpression="SendNewsletter"
            textkey="BLACKLIST_SENDNEWSLETTER">
            <headerstyle cssclass="NormalBold" />
            <itemstyle cssclass="Normal" />
        </rbfwebui:boundcolumn>
        <rbfwebui:boundcolumn datafield="Reason" headertext="Reason" sortexpression="Reason"
            textkey="BLACKLIST_REASON">
            <headerstyle cssclass="NormalBold" />
            <itemstyle cssclass="Normal" />
        </rbfwebui:boundcolumn>
        <rbfwebui:boundcolumn datafield="Date" dataformatstring="{0:d}" headertext="Date"
            sortexpression="Date" textkey="BLACKLIST_DATE">
            <headerstyle cssclass="NormalBold" />
            <itemstyle cssclass="Normal" />
        </rbfwebui:boundcolumn>
    </columns>
</asp:datagrid>
