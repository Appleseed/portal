<%@ register src="~/Design/DesktopLayouts/DesktopFooter.ascx" tagname="Footer" tagprefix="foot" %>
<%@ register src="~/Design/DesktopLayouts/DesktopPortalBanner.ascx" tagname="Banner"
    tagprefix="portal" %>

<%@ page autoeventwireup="false" inherits="Appleseed.Content.Web.Modules.PicturesEdit"
    language="c#" Codebehind="PicturesEdit.aspx.cs" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server"><title></title>
</head>
<body id="Body1" runat="server">
    <form id="Form1" runat="server" enctype="multipart/form-data">
        <div id="zenpanes" class="zen-main">
            <div class="rb_DefaultPortalHeader">
                <portal:banner id="SiteHeader" runat="server" />
            </div>
            <div class="div_ev_Table">
                <table border="0" cellpadding="4" cellspacing="0" width="98%">
                    <tr>
                        <td align="left" class="Head">
                            <rbfwebui:label id="PageTitleLabel" runat="server" height="22"></rbfwebui:label></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr noshade="noshade" size="1" />
                        </td>
                    </tr>
                </table>
                <table border="0" cellpadding="4" cellspacing="0" width="98%">
                    <tr>
                        <td class="SubHead" width="200">
                            <rbfwebui:localize id="Literal1" runat="server" text="Display Order" textkey="PICTURES_DISPLAY_ORDER">
                            </rbfwebui:localize>
                        </td>
                        <td>
                            <asp:textbox id="DisplayOrder" runat="server" cssclass="NormalTextBox" maxlength="10"
                                width="100px"></asp:textbox>
                        </td>
                        <td class="Normal" width="266">
                        </td>
                    </tr>
                    <tr>
                        <td class="SubHead" width="200">
                            <rbfwebui:localize id="Literal2" runat="server" text="Flip" textkey="PICTURES_FLIP">
                            </rbfwebui:localize>
                        </td>
                        <td>
                            <asp:dropdownlist id="selFlip" runat="server" cssclass="NormalTextBox" width="100px">
                                <asp:listitem selected="True" value="None">None</asp:listitem>
                                <asp:listitem value="X">X</asp:listitem>
                                <asp:listitem value="Y">Y</asp:listitem>
                                <asp:listitem value="XY">XY</asp:listitem>
                            </asp:dropdownlist>
                        </td>
                        <td class="Normal" width="266">
                        </td>
                    </tr>
                    <tr>
                        <td class="SubHead" width="200">
                            <rbfwebui:localize id="Literal3" runat="server" text="Rotate" textkey="PICTURES_ROTATE">
                            </rbfwebui:localize>
                        </td>
                        <td>
                            <asp:dropdownlist id="selRotate" runat="server" cssclass="NormalTextBox" width="100px">
                                <asp:listitem selected="True" value="None">None</asp:listitem>
                                <asp:listitem value="90">90</asp:listitem>
                                <asp:listitem value="180">180</asp:listitem>
                                <asp:listitem value="270">270</asp:listitem>
                            </asp:dropdownlist>
                        </td>
                        <td class="Normal" width="266">
                        </td>
                    </tr>
                    <tr>
                        <td class="SubHead" width="200">
                            <rbfwebui:localize id="Literal4" runat="server" text="Caption" textkey="PICTURES_CAPTION">
                            </rbfwebui:localize>
                        </td>
                        <td>
                            <asp:textbox id="Caption" runat="server" cssclass="NormalTextBox" maxlength="255"
                                width="401px"></asp:textbox>
                        </td>
                        <td class="Normal" width="266">
                        </td>
                    </tr>
                    <tr>
                        <td class="SubHead" width="200">
                            <rbfwebui:localize id="Literal5" runat="server" text="Keywords" textkey="PICTURES_KEYWORDS">
                            </rbfwebui:localize>
                        </td>
                        <td>
                            <asp:textbox id="Keywords" runat="server" cssclass="NormalTextBox" maxlength="255"
                                width="401px"></asp:textbox>
                        </td>
                        <td class="Normal" width="266">
                        </td>
                    </tr>
                    <tr>
                        <td class="SubHead" width="200">
                            <rbfwebui:localize id="Literal6" runat="server" text="Short Description" textkey="PICTURES_SHORT_DESCRIPTION">
                            </rbfwebui:localize>
                        </td>
                        <td>
                            <asp:textbox id="ShortDescription" runat="server" cssclass="NormalTextBox" height="120px"
                                maxlength="255" textmode="MultiLine" width="401px"></asp:textbox>
                        </td>
                        <td class="Normal" width="266">
                        </td>
                    </tr>
                    <tr>
                        <td class="SubHead" width="200">
                            <rbfwebui:localize id="Literal7" runat="server" text="Long Description" textkey="PICTURES_LONG_DESCRIPTION">
                            </rbfwebui:localize>
                        </td>
                        <td>
                            <asp:textbox id="LongDescription" runat="server" cssclass="NormalTextBox" height="120px"
                                maxlength="255" textmode="MultiLine" width="401px"></asp:textbox>
                        </td>
                        <td class="Normal" width="266">
                        </td>
                    </tr>
                    <tr>
                        <td class="SubHead" width="200">
                            <rbfwebui:localize id="Literal8" runat="server" text="File" textkey="PICTURES_FILE">
                            </rbfwebui:localize>
                        </td>
                        <td>
                            <input id="flPicture" runat="server" cssclass="NormalTextBox" name="flPicture" type="file" />
                        </td>
                        <td class="Normal" width="266">
                        </td>
                    </tr>
                    <tr>
                        <td class="SubHead" width="200">
                            <rbfwebui:localize id="Literal9" runat="server" text="Include EXIF" textkey="PICTURES_INCLUDE_EXIF">
                            </rbfwebui:localize>
                        </td>
                        <td>
                            <asp:checkbox id="chkIncludeExif" runat="server" checked="True" />
                        </td>
                        <td class="Normal" width="266">
                        </td>
                    </tr>
                    <tr>
                        <td class="SubHead" width="200">
                            <rbfwebui:localize id="BulkDirLiteral" runat="server" text="Bulk load from server directory"
                                textkey="PICTURES_BULK_LOAD">
                            </rbfwebui:localize>
                        </td>
                        <td>
                            <asp:textbox id="BulkDir" runat="server" cssclass="NormalTextBox" maxlength="255"
                                width="401px">
                            </asp:textbox>
                        </td>
                        <td class="Normal" width="266">
                        </td>
                    </tr>
                </table>
                <p>
                </p>
                <rbfwebui:linkbutton id="UpdateButton" runat="server" cssclass="CommandButton">Update</rbfwebui:linkbutton>
                &nbsp;&nbsp;
                <rbfwebui:linkbutton id="CancelButton" runat="server" causesvalidation="False" cssclass="CommandButton">Cancel</rbfwebui:linkbutton>
                &nbsp;&nbsp;
                <rbfwebui:linkbutton id="DeleteButton" runat="server" causesvalidation="False" cssclass="CommandButton">Delete this item</rbfwebui:linkbutton>
                <br />
                <hr noshade="noshade" size="1" width="500" />
                <p>
                    <rbfwebui:label id="Message" runat="server" cssclass="NormalRed" forecolor="Red"></rbfwebui:label>
                </p>
            </div>
            <div class="rb_AlternatePortalFooter">
                <div class="rb_AlternatePortalFooter">
                    <foot:footer id="Footer" runat="server" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
