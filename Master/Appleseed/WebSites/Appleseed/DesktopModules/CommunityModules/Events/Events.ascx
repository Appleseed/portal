<%@ control autoeventwireup="false" inherits="Appleseed.Content.Web.Modules.Events"
    language="c#" Codebehind="Events.ascx.cs" %>
<asp:panel id="CalendarPanel" runat="server" visible="false">
    <asp:textbox id="txtDisplayMonth" runat="server" enabled="False" readonly="True"
        visible="False"></asp:textbox>
    <asp:textbox id="txtDisplayYear" runat="server" enabled="False" readonly="True" visible="False"></asp:textbox>
    <table border="0" cellpadding="3" cellspacing="0" width="100%">
        <tr>
            <td align="left" nowrap="nowrap">
                <rbfwebui:linkbutton id="PreviousMonth" runat="server" cssclass="EventCalendar">&lt&lt</rbfwebui:linkbutton></td>
            <!-- EventCalendarTitle -->
            <td align="middle" nowrap="nowrap">
                <rbfwebui:label id="lblDisplayDate" runat="server" cssclass="ItemTitle" enableviewstate="False"></rbfwebui:label></td>
            <td align="right" nowrap="nowrap">
                <rbfwebui:linkbutton id="NextMonth" runat="server" align="right" cssclass="EventCalendar"> &gt&gt</rbfwebui:linkbutton></td>
        </tr>
        <tr>
            <td colspan="3">
                <rbfwebui:label id="lblCalendar" runat="server" enableviewstate="False"></rbfwebui:label>
            </td>
        </tr>
    </table>
</asp:panel>
<asp:datalist id="myDataList" runat="server" cellpadding="4" enableviewstate="false"
    width="98%">
    <itemtemplate>
        <span class="ItemTitle">
            <rbfwebui:hyperlink id="editLink" runat="server" imageurl='<%# this.CurrentTheme.GetImage("Buttons_Edit", "Edit.gif").ImageUrl %>'
                navigateurl='<%# Appleseed.Framework.HttpUrlBuilder.BuildUrl("~/DesktopModules/CommunityModules/Events/EventsEdit.aspx","ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID") + "&mid=" + ModuleID) %>'
                visible="<%# IsEditable %>">
            </rbfwebui:hyperlink>
            <rbfwebui:label id="Label1" runat="server" text='<%# DataBinder.Eval(Container.DataItem,"Title") %>'>
            </rbfwebui:label>
        </span>
        <br>
        <span class="Normal"><i>
            <%# DataBinder.Eval(Container.DataItem,"WhereWhen") %>
        </i></span>
        <br>
        <span class="Normal">
            <%# DataBinder.Eval(Container.DataItem,"Description") %>
        </span>
        <br>
    </itemtemplate>
</asp:datalist>
