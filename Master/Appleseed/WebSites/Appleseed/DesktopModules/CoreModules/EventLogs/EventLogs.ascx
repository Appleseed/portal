<%@ control autoeventwireup="false" inherits="Appleseed.Content.Web.Modules.EventLogs"
    language="c#" Codebehind="EventLogs.ascx.cs" %>
<%@ import namespace="System.Diagnostics" %>
Machine :
<asp:textbox id="MachineName" runat="server" autopostback="True" columns="10" cssclass="NormalTextBox"
    ontextchanged="MachineName_Change">.</asp:textbox>
&nbsp;&nbsp;&nbsp; Log :
<asp:dropdownlist id="LogName" runat="server" autopostback="True" cssclass="NormalTextBox"
    onselectedindexchanged="LogName_Change">
</asp:dropdownlist>
&nbsp;&nbsp;&nbsp; Source :
<asp:dropdownlist id="LogSource" runat="server" autopostback="True" cssclass="NormalTextBox"
    onselectedindexchanged="LogSource_Change">
</asp:dropdownlist>
<br />
<rbfwebui:label id="Message" runat="server" cssclass="NormalRed"></rbfwebui:label><br />
<asp:datagrid id="LogGrid" runat="server" allowpaging="True" allowsorting="True"
    autogeneratecolumns="False" onpageindexchanged="LogGrid_Change" onsortcommand="LogGrid_Sort"
    pagerstyle-horizontalalign="Right" pagerstyle-mode="NumericPages" pagerstyle-nextpagetext="Next"
    pagerstyle-prevpagetext="Prev" pagesize="100">
    <columns>
        <rbfwebui:templatecolumn headertext="Type" sortexpression="EntryType">
            <itemtemplate>
                <asp:Image ID="Image1" runat="server" ImageUrl='<%#GetEntryTypeImage((EventLogEntryType) DataBinder.Eval(Container.DataItem, "EntryType"))%>'>
                </asp:Image>
            </itemtemplate>
        </rbfwebui:templatecolumn>
        <rbfwebui:boundcolumn datafield="TimeGenerated" headertext="Date" sortexpression="TimeGenerated">
        </rbfwebui:boundcolumn>
        <rbfwebui:boundcolumn datafield="Source" headertext="Source" sortexpression="Source">
        </rbfwebui:boundcolumn>
        <rbfwebui:boundcolumn datafield="EventID" headertext="Event ID" sortexpression="EventID">
        </rbfwebui:boundcolumn>
        <rbfwebui:boundcolumn datafield="Message" headertext="Message" sortexpression="Message">
        </rbfwebui:boundcolumn>
    </columns>
    <pagerstyle horizontalalign="Right" mode="NumericPages" nextpagetext="Next" position="TopAndBottom"
        prevpagetext="Prev" />
</asp:datagrid>
