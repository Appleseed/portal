<%@ control autoeventwireup="false" inherits="Appleseed.Content.Web.Modules.AddModule.Viewer"
    language="c#" Codebehind="Viewer.ascx.cs" %>
<table align="center" border="0">
    <tr>
        <td valign="top">
            <rbfwebui:localize id="moduleNameLabel" runat="server" text="<%$ Resources:Appleseed, AM_MODULETYPE %>">
            </rbfwebui:localize></td>
        <td valign="top">
            <asp:dropdownlist id="moduleType" runat="server" autopostback="False" cssclass="NormalTextBox"
                datatextfield="FriendlyName" datavaluefield="ModuleDefID" Width="430px">
            </asp:dropdownlist>&nbsp;
            <rbfwebui:HyperLink id="AddModuleHelp" runat="server"></rbfwebui:HyperLink></td>
    </tr>
    <tr>
        <td valign="top">
            <rbfwebui:localize id="moduleLocationLabel" runat="server" text="<%$ Resources:Appleseed, AM_MODULELOCATION %>">
            </rbfwebui:localize></td>
        <td valign="top">
            <asp:dropdownlist id="paneLocation" runat="server" Width="430px">
                <asp:listitem value="TopPane">Header</asp:listitem>
                <asp:listitem value="LeftPane">Left Column</asp:listitem>
                <asp:listitem selected="True" value="ContentPane">Center Column</asp:listitem>
                <asp:listitem value="RightPane">Right Column</asp:listitem>
                <asp:listitem value="BottomPane">Footer</asp:listitem>
            </asp:dropdownlist>
        </td>
    </tr>
    <tr>
        <td valign="top" style="width: 130px">
            <rbfwebui:localize id="moduleVisibleLabel" runat="server" text="<%$ Resources:Appleseed, AM_MODULEVISIBLETO %>">
            </rbfwebui:localize><br />
        </td>
        <td valign="top">
            <asp:dropdownlist id="viewPermissions" runat="server" Width="430px">
                <asp:listitem selected="True" value="All Users;">All Users</asp:listitem>
                <asp:listitem value="Authenticated Users;">Authenticated Users</asp:listitem>
                <asp:listitem value="Unauthenticated Users;">Unauthenticated Users</asp:listitem>
                <asp:listitem value="Authorised Roles">Authorised Roles</asp:listitem>
            </asp:dropdownlist>
        </td>
    </tr>
    <tr>
        <td valign="top">
            <rbfwebui:localize id="moduleTitleLabel" runat="server" text="<%$ Resources:Appleseed, AM_MODULENAME %>">
            </rbfwebui:localize></td>
        <td valign="top">
            <%--<asp:textbox id="moduleTitle" runat="server" cssclass="NormalTextBox"
                text="New Module Name" width="423px">New Module Title</asp:textbox>--%>
            <input runat="server" type="text" style="width: 423px" id="TitleTextBox" class="NormalTextBox" value="(Set Module Title)"/>
        </td>
    </tr>
    <tr>
        <td></td>
        <td>
            <rbfwebui:LinkButton id="AddModuleBtn" runat="server" cssclass="CommandButton" text="Add to 'Organize Modules' Below"
                textkey="AM_ADDMODULETOTAB" >Add this Module to the page</rbfwebui:LinkButton>
        </td>
    </tr>
</table>
<div align="center" class="Error">
    <!--
Key Should be : AM_MODULEADDERROR
-->
    <rbfwebui:localize id="moduleError" runat="server" enableviewstate="False" text="<%$ Resources:Appleseed, AM_MODULENAME %>"
        visible="False">
    </rbfwebui:localize></div>
