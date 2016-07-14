<%@ Page Inherits="Appleseed.DesktopModules.CoreModules.HTMLDocument.HtmlEdit"
    Language="c#" MasterPageFile="~/Shared/SiteMasterDefault.master" CodeBehind="HtmlEdit.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <style>
        textarea {
            color: #808080;
        }
    </style>
    <div class="div_ev_Table">
        <% if (Request.QueryString.GetValues("ModalChangeMaster") == null)
           {%>
        <table border="0" cellpadding="4" cellspacing="0" width="98%">
            <tr>
                <td align="left" class="Head">
                    <rbfwebui:Localize ID="Literal1" runat="server" Text="HTML Editor" TextKey="HTML_EDITOR">
                    </rbfwebui:Localize>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr noshade="noshade" size="1" />
                </td>
            </tr>
        </table>
        <% } %>
        <div id="Content" class="Content">
        <table border="0" cellpadding="4" cellspacing="0" width="98%">
            <tr>
                <td class="SubHead">
                    <%--<p>--%>
                    <% if (Request.QueryString.GetValues("ModalChangeMaster") == null)
                       {%>
                    <rbfwebui:Localize ID="Literal2" runat="server" Text="Desktop HTML Content" TextKey="HTML_DESKTOP_CONTENT">
                    </rbfwebui:Localize><font face="ËÎÌו">:</font>
                    <br />
                    <%} %>
                    <div class="normal">
                        <asp:PlaceHolder ID="PlaceHolderHTMLEditor" runat="server"></asp:PlaceHolder>
                    </div>
                    <%--</p>--%>
                </td>
            </tr>
            <% if (Request.QueryString.GetValues("ModalChangeMaster") == null)
               {%>
            <tr id="MobileRow" runat="server">
                <td class="SubHead">&nbsp;
                    <p>
                        <br />
                        <rbfwebui:Localize ID="Literal3" runat="server" Text="Mobile Summary" TextKey="HTML_MOBILE_SUMMARY">
                        </rbfwebui:Localize><font face="ËÎÌו">:</font>
                        <br />
                        <asp:TextBox ID="MobileSummary" runat="server" Columns="75" CssClass="NormalTextBox"
                            Rows="3" TextMode="multiline" Width="650"></asp:TextBox><br />
                        <rbfwebui:Localize ID="Literal4" runat="server" Text="Mobile Details" TextKey="HTML_MOBILE_DETAILS">
                        </rbfwebui:Localize>:
                        <br />
                        <asp:TextBox ID="MobileDetails" runat="server" Columns="75" CssClass="NormalTextBox"
                            Rows="5" TextMode="multiline" Width="650"></asp:TextBox>
                    </p>
                </td>
            </tr>
            <% } %>
        </table>
        </div>
        <div id="footerpopup" class="control ui-widget-header">
            
                <asp:PlaceHolder ID="PlaceHolderButtons" runat="server"></asp:PlaceHolder>
                <asp:Button ID="btnCnVersion" Text="Create New Version" runat="server" OnClick="btnCreateNewVersion_Click" />
                <asp:DropDownList ID="drpVirsionList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpVirsionList_SelectedIndexChanged" />
                <asp:Button ID="btnPsVersion" Text="Publish this Version" runat="server" OnClick="btnPsVersion_Click" />
                <asp:Button ID="btnHsVersion" Text="Version History" runat="server" OnClick="btnHsVersion_Click" />
                <a href="/DesktopModules/CoreModules/FileManager/FileManagerPopup.aspx?mID=155&ModalChangeMaster=true" onclick="openModelInModal('/DesktopModules/CoreModules/FileManager/FileManagerPopup.aspx?mID=155&ModalChangeMaster=true','File Manager');return false;" title="Open File Manager" style="display: block" class="rb_mod_btn btn-img-only CommandButton">
                    <%--<img src="/DesktopModules/CoreModules/FileManager/images/FileManager.png"
                        alt="Edit" style="border-style: None; height: 23px; width: 23px; border-width: 0px;">--%>
                    File Manager
                </a>
        </div>
    </div>
</asp:Content>


