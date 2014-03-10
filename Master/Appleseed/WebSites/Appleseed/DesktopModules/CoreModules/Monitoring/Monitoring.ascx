<%@ control autoeventwireup="false" inherits="Appleseed.Content.Web.Modules.Monitoring"
    language="c#" Codebehind="Monitoring.ascx.cs" %>
<asp:panel id="MonitoringPanel" runat="server" visible="true">
    <table align="center" border="0" cellpadding="2" cellspacing="2" width="100%">
        <tr valign="top">
            <td align="left" class="SubHead" height="19" valign="top" width="100">
                Report Type
            </td>
            <td align="left" class="SubHead" height="19" valign="top" width="170">
                <asp:dropdownlist id="cboReportType" runat="server" cssclass="NormalTextBox">
                    <asp:listitem selected="True" value="Detailed Site Log">Detailed Site Log</asp:listitem>
                    <asp:listitem value="Page Popularity">Page Popularity</asp:listitem>
                    <asp:listitem value="Most Active Users">Most Active Users</asp:listitem>
                    <asp:listitem value="Page Views By Day">Page Views By Day</asp:listitem>
                    <asp:listitem value="Page Views By Browser Type">Page Views By Browser Type</asp:listitem>
                </asp:dropdownlist></td>
            <td align="left" class="SubHead" height="19" rowspan="5" valign="top">
                <asp:checkbox id="CheckBoxPageRequests" runat="server" checked="True" /><rbfwebui:label
                    id="Label1" runat="server">Include page requests</rbfwebui:label><br>
                <asp:checkbox id="CheckBoxLogons" runat="server" checked="True" /><rbfwebui:label
                    id="Label3" runat="server">Include logins</rbfwebui:label><br>
                <asp:checkbox id="CheckBoxLogouts" runat="server" checked="True" /><rbfwebui:label
                    id="Label2" runat="server">Include logouts</rbfwebui:label><br>
                <asp:checkbox id="CheckBoxIncludeMonitorPage" runat="server" /><rbfwebui:label id="Label4"
                    runat="server">Include Monitor Page</rbfwebui:label><br>
                <asp:checkbox id="CheckBoxIncludeMyIPAddress" runat="server" /><rbfwebui:label id="Label6"
                    runat="server">Include my IP address</rbfwebui:label></td>
        </tr>
        <tr valign="top">
            <td class="SubHead">
                <rbfwebui:localize id="Literal2" runat="server" text="Start date" textkey="START_DATE">
                </rbfwebui:localize>:
            </td>
            <td>
                <asp:textbox id="txtStartDate" runat="server" cssclass="NormalTextBox"></asp:textbox></td>
            <td>
            </td>
        </tr>
        <tr valign="top">
            <td>
            </td>
            <td>
                <asp:requiredfieldvalidator id="RequiredStartDate" runat="server" controltovalidate="txtStartDate"
                    display="Dynamic" errormessage="You Must Enter a Valid Start Date" textkey="ERROR_VALID_STARTDATE"></asp:requiredfieldvalidator>
                <asp:comparevalidator id="VerifyStartDate" runat="server" controltovalidate="txtStartDate"
                    display="Dynamic" errormessage="You Must Enter a Valid Start Date" operator="DataTypeCheck"
                    textkey="ERROR_VALID_STARTDATE" type="Date"></asp:comparevalidator></td>
            <td>
            </td>
        </tr>
        <tr valign="top">
            <td class="SubHead">
                <rbfwebui:localize id="Literal3" runat="server" text="End date" textkey="END_DATE">
                </rbfwebui:localize>:
            </td>
            <td>
                <asp:textbox id="txtEndDate" runat="server" cssclass="NormalTextBox"></asp:textbox></td>
            <td>
            </td>
        </tr>
        <tr valign="top">
            <td>
            </td>
            <td>
                <asp:requiredfieldvalidator id="RequiredEndDate" runat="server" controltovalidate="txtEndDate"
                    display="Dynamic" errormessage="You Must Enter a Valid End Date" textkey="ERROR_VALID_ENDDATE"></asp:requiredfieldvalidator>
                <asp:comparevalidator id="VerifyExpireDate" runat="server" controltovalidate="txtEndDate"
                    display="Dynamic" errormessage="You Must Enter a Valid End Date" operator="DataTypeCheck"
                    textkey="ERROR_VALID_ENDDATE" type="Date"></asp:comparevalidator></td>
            <td>
            </td>
        </tr>
        <tr valign="top">
            <td>
            </td>
            <td align="left" class="NormalBold" valign="top">
            </td>
            <td align="left" class="NormalBold" valign="top">
                <rbfwebui:linkbutton id="cmdDisplay" runat="server" cssclass="CommandButton" text="Display"></rbfwebui:linkbutton>&nbsp;&nbsp;
            </td>
        </tr>
    </table>
    <br>
    <asp:image id="ChartImage" runat="server" visible="False" />
    <rbfwebui:label id="LabelNoData" runat="server" visible="False">No data available</rbfwebui:label>
    <asp:datagrid id="myDataGrid" runat="server" allowsorting="True" autogeneratecolumns="True"
        border="0" cellpadding="3" cssclass="normal" enableviewstate="False" onsortcommand="SortTasks"
        pagesize="1">
        <pagerstyle mode="NumericPages" />
    </asp:datagrid>
</asp:panel>
<rbfwebui:label id="ErrorLabel" runat="server" cssclass="Error" visible="false"></rbfwebui:label>
