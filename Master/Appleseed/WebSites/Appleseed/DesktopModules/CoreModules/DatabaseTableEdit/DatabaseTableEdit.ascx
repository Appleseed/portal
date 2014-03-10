<%@ register assembly="TripleASP.TableEditor" namespace="TripleASP.SiteAdmin.TableEditorControl"
    tagprefix="TAN" %>
<%@ control autoeventwireup="false" inherits="Appleseed.Content.Web.Modules.DatabaseTableEdit"
    language="c#" Codebehind="DatabaseTableEdit.ascx.cs" %>
<rbfwebui:label id="lblConnectedError" runat="server" forecolor="Red"></rbfwebui:label>
<asp:panel id="panConnected" runat="server">
    <table border="0" cellspacing="0">
        <tr>
            <td>
                <rbfwebui:localize id="Message" runat="server">
                </rbfwebui:localize></td>
        </tr>
        <tr>
            <td>
                <p>
                    <asp:dropdownlist id="tablelist" runat="server" autopostback="true" cssclass="NormalTextBox">
                    </asp:dropdownlist></p>
                <p>
                    <tan:tableeditor id="tableeditor" runat="server" class="Normal" width="100%">
                    </tan:tableeditor></p>
            </td>
        </tr>
    </table>
</asp:panel>
