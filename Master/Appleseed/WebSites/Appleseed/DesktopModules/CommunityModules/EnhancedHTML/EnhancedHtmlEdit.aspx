<%@ register src="~/Design/DesktopLayouts/DesktopPortalBanner.ascx" tagname="Banner"
    tagprefix="portal" %>
<%@ register src="~/Design/DesktopLayouts/DesktopFooter.ascx" tagname="Footer" tagprefix="foot" %>

<%@ page autoeventwireup="false" inherits="Appleseed.Content.Web.Modules.EnhancedHtmlEdit"
    language="c#" Codebehind="EnhancedHtmlEdit.aspx.cs" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server"><title></title>
</head>
<body id="Body1" runat="server">
    <form id="Form1" runat="server" enctype="multipart/form-data">
        <div id="zenpanes" class="zen-main">
            <div class="rb_DefaultPortalHeader">
                <portal:banner id="SiteHeader" runat="server" />
            </div>
            <div class="div_ev_Table">
                <asp:panel id="pnlSelectPage" runat="server">
                    <table align="center" border='0' cellpadding='0' cellspacing='0'>
                        <tr>
                            <td>
                                <table border='0' cellpadding='0' cellspacing='0'>
                                    <tr>
                                        <td class="Head">
                                            <rbfwebui:localize id="Literal6" runat="server" text="Language" textkey="ENHANCEDHTML_LANGUAGE">
                                            </rbfwebui:localize></td>
                                        <td class="Head">
                                            <label for="<%= lstPages.ClientID %>">
                                                <rbfwebui:label id="Label1" runat="server" text="Page" textkey="ENHANCEDHTML_PAGE"></rbfwebui:label></label></td>
                                    </tr>
                                    <tr>
                                        <td align="center" valign="top">
                                            <asp:listbox id="lstLanguages" runat="server" autopostback="True" cssclass="Normal"
                                                rows="20" width="250px"></asp:listbox></td>
                                        <td align="center" valign="top">
                                            <asp:listbox id="lstPages" runat="server" cssclass="Normal" rows="20" width="300px">
                                            </asp:listbox></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" nowrap="nowrap">
                                <rbfwebui:button id="cmdNewPage" runat="server" causesvalidation="False" cssclass="CommandButton"
                                    text="New Page" textkey="ENHANCEDHTML_NEWPAGE" width="95" />&nbsp;
                                <rbfwebui:button id="cmdEditPage" runat="server" causesvalidation="False" cssclass="CommandButton"
                                    enabled="False" text="Edit Page" textkey="ENHANCEDHTML_EDITPAGE" width="95" />&nbsp;
                                <rbfwebui:button id="cmdDeletePage" runat="server" causesvalidation="False" cssclass="CommandButton"
                                    enabled="False" text="Delete Page" textkey="ENHANCEDHTML_DELETEPAGE" width="95" />&nbsp;
                                <rbfwebui:button id="cmdReturn" runat="server" causesvalidation="False" cssclass="CommandButton"
                                    text="Return" textkey="ENHANCEDHTML_RETURN" width="95" />&nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:panel>
                <asp:panel id="pnlEditPage" runat="server" visible="False">
                    <table align="center" border='0' cellpadding='0' cellspacing='0'>
                        <tr>
                            <td>
                                <table border='0' cellpadding='0' cellspacing='0'>
                                    <tr>
                                        <td align="left" class="SubHead" width="90">
                                            <label for="<%= txtPageName.ClientID %>">
                                                <rbfwebui:localize id="Literal3" runat="server" text="Page Name" textkey="ENHANCEDHTML_PAGENAME">
                                                </rbfwebui:localize></label></td>
                                        <td align="left">
                                            <asp:textbox id="txtPageName" runat="server" cssclass="NormalTextBox" textmode="SingleLine"
                                                width="500px"></asp:textbox></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table border='0' cellpadding='0' cellspacing='0'>
                                    <tr>
                                        <td class="SubHead" width="90">
                                            <label for="<%= txtViewOrder.ClientID %>">
                                                <rbfwebui:localize id="Literal4" runat="server" text="View Order" textkey="ENHANCEDHTML_VIEWORDER">
                                                </rbfwebui:localize></label></td>
                                        <td width="180">
                                            <asp:textbox id="txtViewOrder" runat="server" cssclass="NormalTextBox" textmode="SingleLine"
                                                width="100"></asp:textbox></td>
                                        <td align="right" class="SubHead">
                                            <rbfwebui:localize id="Literal5" runat="server" text="Language" textkey="ENHANCEDHTML_LANGUAGE">
                                            </rbfwebui:localize></td>
                                        <td class="Normal">
                                            &nbsp;
                                            <asp:dropdownlist id="listLanguages" runat="server">
                                            </asp:dropdownlist></td>
                                    </tr>
                                    <tr>
                                        <td class="SubHead">
                                            <rbfwebui:localize id="Literal1" runat="server" text="Content from" textkey="ENHANCEDHTML_CONTENT_FROM">
                                            </rbfwebui:localize></td>
                                        <td colspan="2">
                                            <asp:radiobuttonlist id="kindOfContent" runat="server" autopostback="True">
                                                <asp:listitem selected="True" value="Editor">Html Editor</asp:listitem>
                                                <asp:listitem value="Module">Module</asp:listitem>
                                            </asp:radiobuttonlist></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table border='0' cellpadding='0' cellspacing='0'>
                                    <tr>
                                        <td class="SubHead">
                                            <rbfwebui:localize id="Literal2" runat="server" text="Desktop HTML Content" textkey="HTML_DESKTOP_CONTENT">
                                            </rbfwebui:localize></td>
                                    </tr>
                                    <tr>
                                        <td class="SubHead">
                                            <asp:placeholder id="PlaceHolderHTMLEditor" runat="server"></asp:placeholder>
                                            <asp:dropdownlist id="listModules" runat="server" visible="false">
                                            </asp:dropdownlist>
                                            <asp:dropdownlist id="listAllModules" runat="server" visible="false">
                                            </asp:dropdownlist></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table align="center" border='0' cellpadding='0' cellspacing='0'>
                                    <tr>
                                        <td align="center" nowrap="nowrap">
                                            <rbfwebui:button id="cmdPageUpdate" runat="server" causesvalidation="False" cssclass="CommandButton"
                                                text="Update" textkey="ENHANCEDHTML_UPDATE" />&nbsp;
                                            <rbfwebui:button id="cmdPageCancel" runat="server" causesvalidation="False" cssclass="CommandButton"
                                                text="Cancel" textkey="ENHANCEDHTML_CANCEL" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <hr noshade="noshade" size="1" />
                    <span class="Normal">
                        <rbfwebui:localize id="CreatedLabel" runat="server" text="Created by" textkey="CREATED_BY">
                        </rbfwebui:localize>&nbsp;
                        <rbfwebui:label id="CreatedBy" runat="server"></rbfwebui:label>&nbsp;
                        <rbfwebui:localize id="OnLabel" runat="server" text="on" textkey="ON">
                        </rbfwebui:localize>&nbsp;
                        <rbfwebui:label id="CreatedDate" runat="server"></rbfwebui:label>
                    </span>
                </asp:panel>
            </div>
            <foot:footer id="Footer" runat="server" />
        </div>
    </form>
</body>
</html>
