<%@ control autoeventwireup="false" inherits="Appleseed.Content.Web.Modules.ModuleDefs"
    language="c#" Codebehind="ModuleDefs.ascx.cs" %>
<%@ register assembly="Appleseed.Framework" namespace="Appleseed.Framework.Web.UI.WebControls"
    tagprefix="rbfwebui" %>
<div class="settings-table">
    <fieldset class="SettingsTableGroup">
        <legend class="SubSubHead">
            <rbfwebui:localize id="userTitle" runat="server" text="User Modules" textkey="MODULE_DEFS_USER">
            </rbfwebui:localize>
        </legend>
        <table border="0" class="SettingsTableGroup" width="100%">
            <tr>
                <td>
                    <asp:datalist id="userModules" runat="server" class="SettingsTableGroup" datakeyfield="ModuleDefID">
                        <itemtemplate>
                            <rbfwebui:label id="Label1" runat="server" cssclass="Normal" text='<%# DataBinder.Eval(Container.DataItem, "FriendlyName") %>'>
                            </rbfwebui:label>
                        </itemtemplate>
                    </asp:datalist>
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset class="SettingsTableGroup">
        <legend class="SubSubHead">
            <rbfwebui:localize id="adminTitle" runat="server" text="Admin Modules" textkey="MODULE_DEFS_ADMIN">
            </rbfwebui:localize>
        </legend>
        <table border="0" class="SettingsTableGroup" width="100%">
            <tr>
                <td>
                    <asp:datalist id="adminModules" runat="server" datakeyfield="ModuleDefID">
                        <itemtemplate>
                            <rbfwebui:label id="Label2" runat="server" cssclass="Normal" text='<%# DataBinder.Eval(Container.DataItem, "FriendlyName") %>'>
                            </rbfwebui:label>
                        </itemtemplate>
                    </asp:datalist>
                </td>
            </tr>
        </table>
    </fieldset>
</div>
