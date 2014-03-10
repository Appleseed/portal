<%@ page autoeventwireup="false" inherits="Appleseed.Content.Web.Modules.EventsEdit"
    language="c#" Codebehind="EventsEdit.aspx.cs" %>

<%@ register src="~/Design/DesktopLayouts/DesktopPortalBanner.ascx" tagname="Banner"
    tagprefix="portal" %>
<%@ register src="~/Design/DesktopLayouts/DesktopFooter.ascx" tagname="Footer" tagprefix="foot" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server"><title></title>
</head>
<body id="Body1" runat="server">
    <form id="Form1" runat="server">
        <div id="zenpanes" class="zen-main">
            <div class="rb_DefaultPortalHeader">
                <portal:banner id="SiteHeader" runat="server" />
            </div>
            <div class="div_ev_Table">
                <table border="0" cellpadding="4" cellspacing="0" width="98%">
                    <tr>
                        <td align="left" class="Head">
                            <rbfwebui:label id="Label5" runat="server" name="Label1" text="Event detail" textkey="EVENTS_DETAILS"></rbfwebui:label>:
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <hr noshade="noshade" size="1" />
                        </td>
                    </tr>
                </table>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr valign="top">
                        <td class="SubHead" width="100">
                            <rbfwebui:label id="Label1" runat="server" name="Label1" text="Title" textkey="EVENTS_TITLE"></rbfwebui:label>:
                        </td>
                        <td>
                            <asp:textbox id="TitleField" runat="server" columns="30" cssclass="NormalTextBox"
                                maxlength="150" width="390">
                            </asp:textbox>
                        </td>
                        <td class="Normal" width="250">
                            <asp:requiredfieldvalidator id="RequiredTitle" runat="server" controltovalidate="TitleField"
                                display="Dynamic" errormessage="Please insert a valid Title" textkey="ERROR_VALID_TITLE">
                            </asp:requiredfieldvalidator>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td class="SubHead">
                            <rbfwebui:label id="Label2" runat="server" name="Label2" text="Description" textkey="EVENTS_DESCRIPTION"></rbfwebui:label>:
                        </td>
                        <td>
                            <asp:placeholder id="PlaceHolderHTMLEditor" runat="server"></asp:placeholder>
                        </td>
                        <td class="Normal">
                            &nbsp;
                        </td>
                    </tr>
                    <tr valign="top">
                        <td class="SubHead">
                            <rbfwebui:label id="Label3" runat="server" name="Label2" text="Where" textkey="EVENTS_WHEREWHEN"></rbfwebui:label>:
                        </td>
                        <td>
                            <asp:textbox id="WhereWhenField" runat="server" columns="30" cssclass="NormalTextBox"
                                maxlength="150" width="390">
                            </asp:textbox>
                        </td>
                        <td class="Normal">
                            <asp:requiredfieldvalidator id="RequiredWhereWhen" runat="server" controltovalidate="WhereWhenField"
                                display="Dynamic" errormessage="Please insert a valid Where When field" textkey="ERROR_VALID_WHEREWHEN">
                            </asp:requiredfieldvalidator>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td class="SubHead">
                            <rbfwebui:label id="Label7" runat="server" name="Label7" text="When" textkey="EVENTS_STARTDATE"></rbfwebui:label>:
                        </td>
                        <td>
                            <asp:textbox id="StartDate" runat="server" columns="8" cssclass="NormalTextBox" width="100">
                            </asp:textbox>
                        </td>
                        <td class="Normal">
                            <asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" controltovalidate="StartDate"
                                display="Dynamic" errormessage="Please insert a valid date" textkey="ERROR_VALID_STARTDATE">
                            </asp:requiredfieldvalidator>
                            <asp:requiredfieldvalidator id="RequiredFieldValidator2" runat="server" controltovalidate="StartDate"
                                display="Dynamic" errormessage="Please insert a valid date" textkey="ERROR_VALID_STARTDATE"></asp:requiredfieldvalidator>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td class="SubHead">
                            <rbfwebui:label id="Label8" runat="server" name="Label8" text="Time" textkey="EVENTS_STARTTIME"></rbfwebui:label>:
                        </td>
                        <td colspan="2" nowrap="nowrap">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td nowrap="nowrap">
                                        <asp:radiobuttonlist id="AllDay" runat="server" autopostback="True" cssclass="Normal">
                                            <asp:listitem selected="True" value="1">All Day Event</asp:listitem>
                                            <asp:listitem value="0">Start at</asp:listitem>
                                        </asp:radiobuttonlist>&nbsp;<asp:dropdownlist id="StartHour" runat="server" cssclass="NormalTextBox"
                                            enabled="False" width="45">
                                            <asp:listitem selected="True" value="1">1</asp:listitem>
                                            <asp:listitem value="2">2</asp:listitem>
                                            <asp:listitem value="3">3</asp:listitem>
                                            <asp:listitem value="4">4</asp:listitem>
                                            <asp:listitem value="5">5</asp:listitem>
                                            <asp:listitem value="6">6</asp:listitem>
                                            <asp:listitem value="7">7</asp:listitem>
                                            <asp:listitem value="8">8</asp:listitem>
                                            <asp:listitem value="9">9</asp:listitem>
                                            <asp:listitem value="10">10</asp:listitem>
                                            <asp:listitem value="11">11</asp:listitem>
                                            <asp:listitem value="12">12</asp:listitem>
                                        </asp:dropdownlist>&nbsp;<b>:</b>
                                        <asp:dropdownlist id="StartMinute" runat="server" cssclass="NormalTextBox" enabled="False"
                                            width="45">
                                            <asp:listitem selected="True" value="00">00</asp:listitem>
                                            <asp:listitem value="05">05</asp:listitem>
                                            <asp:listitem value="10">10</asp:listitem>
                                            <asp:listitem value="15">15</asp:listitem>
                                            <asp:listitem value="20">20</asp:listitem>
                                            <asp:listitem value="25">25</asp:listitem>
                                            <asp:listitem value="30">30</asp:listitem>
                                            <asp:listitem value="35">35</asp:listitem>
                                            <asp:listitem value="40">40</asp:listitem>
                                            <asp:listitem value="45">45</asp:listitem>
                                            <asp:listitem value="50">50</asp:listitem>
                                            <asp:listitem value="55">55</asp:listitem>
                                        </asp:dropdownlist>&nbsp;
                                        <asp:dropdownlist id="StartAMPM" runat="server" cssclass="NormalTextBox" enabled="False"
                                            width="50">
                                            <asp:listitem selected="True" value="AM">AM</asp:listitem>
                                            <asp:listitem value="PM">PM</asp:listitem>
                                        </asp:dropdownlist>&nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td class="SubHead">
                            <rbfwebui:label id="Label4" runat="server" name="Label2" text="Expires" textkey="EVENTS_EXPIRES"></rbfwebui:label>:
                        </td>
                        <td>
                            <asp:textbox id="ExpireField" runat="server" columns="8" cssclass="NormalTextBox"
                                width="100">
                            </asp:textbox><br/>
                        </td>
                        <td class="Normal">
                            <asp:requiredfieldvalidator id="RequiredExpireDate" runat="server" controltovalidate="ExpireField"
                                designtimedragdrop="73" display="Dynamic" errormessage="Please insert a valid date"
                                textkey="ERROR_VALID_EXPIRE_DATE"></asp:requiredfieldvalidator>
                            <asp:comparevalidator id="VerifyExpireDate" runat="server" controltovalidate="ExpireField"
                                display="Dynamic" errormessage="Please insert a valid date" operator="DataTypeCheck"
                                textkey="ERROR_VALID_EXPIRE_DATE" type="Date">
                            </asp:comparevalidator>
                        </td>
                    </tr>
                </table>
                <p>
                    <rbfwebui:linkbutton id="UpdateButton" runat="server" class="CommandButton" text="Update">
                    </rbfwebui:linkbutton>
                    &nbsp;
                    <rbfwebui:linkbutton id="CancelButton" runat="server" causesvalidation="False" class="CommandButton"
                        text="Cancel">
                    </rbfwebui:linkbutton>
                    &nbsp;
                    <rbfwebui:linkbutton id="DeleteButton" runat="server" causesvalidation="False" class="CommandButton"
                        text="Delete this item">
                    </rbfwebui:linkbutton>
                </p>
                <hr noshade="noshade" size="1" />
                <span class="Normal">
                    <rbfwebui:localize id="CreatedLabel" runat="server" text="Created by" textkey="CREATED_BY">
                    </rbfwebui:localize>&nbsp;
                    <rbfwebui:label id="CreatedBy" runat="server"></rbfwebui:label>&nbsp;
                    <rbfwebui:localize id="OnLabel" runat="server" text="on" textkey="ON">
                    </rbfwebui:localize>&nbsp;
                    <rbfwebui:label id="CreatedDate" runat="server"></rbfwebui:label>
                </span>
            </div>
            <div class="rb_AlternatePortalFooter">
                <foot:footer id="Footer" runat="server" />
            </div>

        </div>
    </form>
</body>
</html>
