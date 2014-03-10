<%@ control autoeventwireup="false" inherits="Appleseed.Content.Web.Modules.SendNewsletter"
    language="c#" targetschema="http://schemas.microsoft.com/intellisense/ie5" Codebehind="Newsletter.ascx.cs" %>
<table width="100%">
    <tr>
        <td colspan="5">
            <rbfwebui:label id="lblMessage" runat="server" cssclass="Normal"></rbfwebui:label></td>
    </tr>
    <tr>
        <td>
            <asp:panel id="UsersPanel" runat="server" horizontalalign="Center" visible="true">
                <asp:datalist id="DataList1" runat="server" alternatingitemstyle-backcolor="Gainsboro"
                    bordercolor="black" cellpadding="7" cssclass="Normal" font-name="Verdana" font-size="8pt"
                    headerstyle-backcolor="silver" horizontalalign="center">
                    <headertemplate>
                        <%= Titulo %>
                    </headertemplate>
                    <itemtemplate>
                        <%# DataBinder.Eval(Container.DataItem,"StringValue") %>
                    </itemtemplate>
                </asp:datalist>
            </asp:panel>
        </td>
    </tr>
    <tr>
        <td>
            <asp:panel id="EditPanel" runat="server" horizontalalign="Center" visible="true">
                <table border="0" cellpadding="4" cellspacing="0" width="600">
                    <tr valign="top">
                        <td class="SubHead" nowrap="nowrap" width="200">
                            <rbfwebui:localize id="Literal1" runat="server" text="Sender email address" textkey="NEWSLETTER_SENDER_EMAIL_ADDRESS">
                            </rbfwebui:localize>:
                        </td>
                        <td width="*">
                            <asp:textbox id="txtEMail" runat="server" columns="40" cssclass="NormalTextBox" maxlength="100"
                                width="450"></asp:textbox>
                            <asp:regularexpressionvalidator id="validEmailRegExp" runat="server" controltovalidate="txtEMail"
                                cssclass="SubHead" display="Dynamic" errormessage="Please enter a valid email address."
                                textkey="ERROR_VALID_EMAIL" validationexpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"></asp:regularexpressionvalidator></td>
                    </tr>
                    <tr valign="top">
                        <td class="SubHead" nowrap="nowrap" width="200">
                            <rbfwebui:localize id="Literal2" runat="server" text="Your Name (optional)" textkey="NEWSLETTER_SENDER_OPTIONAL_NAME">
                            </rbfwebui:localize>:
                        </td>
                        <td width="*">
                            <asp:textbox id="txtName" runat="server" columns="40" cssclass="NormalTextBox" maxlength="100"
                                width="450"></asp:textbox></td>
                    </tr>
                    <tr valign="top">
                        <td class="SubHead" nowrap="nowrap">
                            <rbfwebui:localize id="Literal3" runat="server" text="Subject" textkey="NEWSLETTER_SUBJECT">
                            </rbfwebui:localize>:
                        </td>
                        <td width="*">
                            <asp:textbox id="txtSubject" runat="server" columns="40" cssclass="NormalTextBox"
                                maxlength="100" width="450"></asp:textbox></td>
                    </tr>
                    <tr valign="top">
                        <td class="SubHead" nowrap="nowrap">
                            <rbfwebui:localize id="Literal4" runat="server" text="Message Body" textkey="NEWSLETTER_MESSAGE_BODY">
                            </rbfwebui:localize>:
                        </td>
                        <td width="*">
                            <asp:textbox id="txtBody" runat="server" columns="59" cssclass="NormalTextBox" rows="15"
                                textmode="Multiline" width="450"></asp:textbox>
                            <asp:requiredfieldvalidator id="validEmailRequired" runat="server" controltovalidate="txtBody"
                                cssclass="SubHead" display="Dynamic" errormessage="Please enter message text."
                                textkey="ERROR_VALID_MESSAGE_TEXT"></asp:requiredfieldvalidator></td>
                    </tr>
                    <tr>
                        <td nowrap="nowrap">
                        </td>
                        <td>
                            <asp:checkbox id="HtmlMode" runat="server" cssclass="Normal" text="HtmlMode" textkey="NEWSLETTER_HTML_MODE" />&nbsp;
                            <asp:checkbox id="InsertBreakLines" runat="server" cssclass="Normal" text="Insert break lines"
                                textkey="NEWSLETTER_INSERT_BREAK_LINES" />&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td nowrap="nowrap">
                        </td>
                        <td class="Normal">
                            {NAME} =
                            <rbfwebui:localize id="Literal5" runat="server" text="UserName" textkey="NEWSLETTER_USERNAME">
                            </rbfwebui:localize><br/>
                            {EMAIL} =
                            <rbfwebui:localize id="Literal7" runat="server" text="UserEmail" textkey="NEWSLETTER_USEREMAIL">
                            </rbfwebui:localize><br/>
                    </tr>
                    <tr valign="top">
                        <td nowrap="nowrap">
                            &nbsp;</td>
                        <td>
                            <rbfwebui:LinkButton id="previewButton" runat="server" cssclass="CommandButton" enableviewstate="False"
                                text="Preview" textkey="PREVIEW"></rbfwebui:LinkButton>&nbsp;
                            <rbfwebui:LinkButton id="cancelButton" runat="server" causesvalidation="False" cssclass="CommandButton"
                                enableviewstate="False" text="Cancel" textkey="CANCEL"></rbfwebui:LinkButton></td>
                    </tr>
                </table>
            </asp:panel>
        </td>
    </tr>
    <tr>
        <td>
            <asp:panel id="PrewiewPanel" runat="server" horizontalalign="Center" visible="true">
                <table border="0" cellpadding="4" cellspacing="0" width="600">
                    <tr valign="top">
                        <td class="Normal">
                            <rbfwebui:label id="Label1" runat="server" cssclass="SubHead" text="From" textkey="FROM"></rbfwebui:label>:
                            <rbfwebui:label id="lblFrom" runat="server"></rbfwebui:label></td>
                    </tr>
                    <tr valign="top">
                        <td class="Normal">
                            <rbfwebui:label id="Label2" runat="server" cssclass="SubHead" text="to" textkey="TO"></rbfwebui:label>:
                            <rbfwebui:label id="lblTo" runat="server"></rbfwebui:label></td>
                    </tr>
                    <tr valign="top">
                        <td class="Normal">
                            <rbfwebui:label id="Label3" runat="server" cssclass="SubHead" text="Subject" textkey="SUBJECT"></rbfwebui:label>
                            <rbfwebui:label id="lblSubject" runat="server"></rbfwebui:label></td>
                    </tr>
                    <tr valign="top">
                        <td class="Normal">
                            <rbfwebui:label id="lblBody" runat="server"></rbfwebui:label></td>
                    </tr>
                    <tr valign="top">
                        <td class="Normal">
                            <rbfwebui:LinkButton id="submitButton" runat="server" cssclass="CommandButton" enableviewstate="False"
                                text="Submit" textkey="SUBMIT"></rbfwebui:LinkButton>&nbsp;
                            <rbfwebui:LinkButton id="cancelButton2" runat="server" causesvalidation="False" cssclass="CommandButton"
                                enableviewstate="False" text="Cancel" textkey="CANCEL"></rbfwebui:LinkButton></td>
                    </tr>
                </table>
            </asp:panel>
        </td>
    </tr>
    <tr>
        <td>
            <table border="0" cellpadding="4" cellspacing="0" width="98%">
                <tr>
                    <td align="left" valign="top">
                        <rbfwebui:label id="CreatedDate" runat="server" cssclass="Normal"></rbfwebui:label>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
