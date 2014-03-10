<%@ control autoeventwireup="True" inherits="Appleseed.Content.Web.Modules.AddModule.AddPage" language="c#" CodeBehind="AddTab.ascx.cs" %>
<table align="center" border="0">
    <tr>
        <td height="20" valign="top">
            <rbfwebui:localize id="tabParentLabel" runat="server" text="Page parent:" textkey="AT_PAGEPARENT">
            </rbfwebui:localize></td>
        <td height="20" valign="top">
            <asp:DropDownList ID="parentTabDropDown" runat="server" CssClass="NormalTextBox" DataTextField="PageName"
                                        DataValueField="PageID" useuniqueid="true" width="200px" />
            <rbfwebui:Label ID="lblErrorNotAllowed" runat="server" CssClass="Error"
                EnableViewState="False" TextKey="ERROR_NOT_ALLOWED_PARENT" Visible="False">Not allowed to choose that parent</rbfwebui:Label></td>
    </tr>
    <tr>
        <td valign="top">
            <rbfwebui:localize id="tabVisibleLabel" runat="server" text="Page Visible To:" textkey="AM_PAGEVISIBLETO">
            </rbfwebui:localize><br/>
        </td>
        <td valign="top">
            <asp:dropdownlist id="PermissionDropDown" runat="server" cookiememory="False" cssclass="NormalTextBox"
                enableviewstate="False" useuniqueid="true" width="200px">
            </asp:dropdownlist></td>
    </tr>
    <tr>
        <td valign="top">
            <rbfwebui:localize id="lbl_ShowMobile" runat="server" text="Show Mobile:" textkey="AT_SHOWMOBILE">
            </rbfwebui:localize></td>
        <td valign="top">
            <asp:checkbox id="cb_ShowMobile" runat="server" /></td>
    </tr>
    <tr>
        <td valign="top">
            <rbfwebui:localize id="lbl_MobileTabName" runat="server" text="Mobile Page Name:" textkey="AT_MOBILETABNAME">
            </rbfwebui:localize></td>
        <td valign="top">
            <asp:textbox id="tb_MobileTabName" runat="server" maxlength="50" width="200px"></asp:textbox></td>
    </tr>
    <tr>
        <td valign="top">
            <rbfwebui:localize id="tabTitleLabel" runat="server" text="Page Title:" textkey="AT_TABTITLE">
            </rbfwebui:localize></td>
        <td nowrap="nowrap" valign="top">
            <asp:textbox id="TabTitleTextBox" runat="server" cssclass="NormalTextBox" enableviewstate="false"
                maxlength="50" text="New Page" width="200px" />&nbsp;&nbsp;<rbfwebui:LinkButton
                    id="AddTabButton" runat="server" cssclass="CommandButton" TextKey="AT_ADDPAGE" Text="Add this page" /></td>
    </tr>
    <tr>
        <td colspan="2" valign="top">
            <rbfwebui:localize id="Literal1" runat="server" text="Would you like to jump to the new page?" TextKey="AT_JUMPTOTAB">
            </rbfwebui:localize>&nbsp;&nbsp;<asp:radiobuttonlist id="rbl_JumpToTab" runat="server"
                height="24px" repeatdirection="Horizontal" width="136px">
                <asp:listitem selected="True" value="No">No</asp:listitem>
                <asp:listitem value="Yes">Yes</asp:listitem>
            </asp:radiobuttonlist></td>
    </tr>
</table>
<div align="center" class="Error">
    <!-- 
Key Should be : AM_MODULEADDERROR
-->
    <rbfwebui:localize id="moduleError" runat="server" enableviewstate="False" text="There was an error adding this module. It has been logged."
        textkey="AM_MODULENAME" visible="False">
    </rbfwebui:localize></div>
