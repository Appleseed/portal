<%@ Page Inherits="Appleseed.DesktopModules.CommunityModules.HTMLDocument.HtmlEdit"
    Language="c#" MasterPageFile="~/Shared/SiteMasterDefault.master" CodeBehind="HtmlEdit.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <div class="div_ev_Table">
        <% if (Request.QueryString.GetValues("ModalChangeMaster") == null) {%>
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
        <table border="0" cellpadding="4" cellspacing="0" width="98%">
            <tr>
                <td class="SubHead">
                    <%--<p>--%>
                     <% if (Request.QueryString.GetValues("ModalChangeMaster") == null) {%>
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
            <% if (Request.QueryString.GetValues("ModalChangeMaster") == null) {%>
            <tr id="MobileRow" runat="server">
                <td class="SubHead">
                    &nbsp;
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
                            Rows="5" TextMode="multiline" Width="650"></asp:TextBox></p>
                </td>
            </tr>
            <% } %>
        </table>
        <p>
            <asp:PlaceHolder ID="PlaceHolderButtons" runat="server"></asp:PlaceHolder>
        </p>
    </div>
</asp:Content>
