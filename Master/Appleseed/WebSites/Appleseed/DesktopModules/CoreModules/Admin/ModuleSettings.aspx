<%@ Page AutoEventWireup="false" Inherits="Appleseed.Admin.ModuleSettingsPage"
    Language="c#" MasterPageFile="~/Shared/SiteMasterDefault.master" Codebehind="ModuleSettings.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <div class="div_ev_Table">
         <% if (Request.QueryString.GetValues("ModalChangeMaster") == null) {%>
                                                <rbfwebui:Localize ID="Literal1" runat="server" Text="Module base settings" TextKey="MODULESETTINGS_BASE_SETTINGS">
                                                </rbfwebui:Localize>
                                        <% } %>
                                            <asp:PlaceHolder ID="PlaceholderButtons2" Visible="false" runat="server"></asp:PlaceHolder>

        <table border="0" cellpadding="4" cellspacing="0" width="98%">
            <tr valign="top">
                <td width="*">
                    <table border="0" cellpadding="2" cellspacing="1">
                        <tr>
                            <td class="SubHead" height="50" width="200">
                                <rbfwebui:Localize ID="Literal2" runat="server" Text="Module type" TextKey="MODULESETTINGS_MODULE_TYPE">
                                </rbfwebui:Localize>:
                            </td>
                            <td colspan="3" height="38">
                                &nbsp;<rbfwebui:Label ID="moduleType" runat="server" CssClass="NormalBold" Width="300"></rbfwebui:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="SubHead" width="200">
                                <rbfwebui:Localize ID="Literal18" runat="server" Text="Module name" TextKey="MODULESETTINGS_MODULE_NAME">
                                </rbfwebui:Localize>:
                            </td>
                            <td colspan="3">
                                &nbsp;<asp:TextBox ID="moduleTitle" runat="server" CssClass="NormalTextBox" Width="300"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="SubHead" width="200">
                                <rbfwebui:Localize ID="Literal3" runat="server" Text="Cache Timeout" TextKey="MODULESETTINGS_CACHE_TIMEOUT">
                                </rbfwebui:Localize>:
                            </td>
                            <td colspan="3">
                                &nbsp;<asp:TextBox ID="cacheTime" runat="server" CssClass="NormalTextBox" Width="100"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="SubHead" width="200">
                                <rbfwebui:Localize ID="Literal13" runat="server" Text="Move to tab" TextKey="MODULESETTINGS_MOVE_TO_TAB">
                                </rbfwebui:Localize>:
                            </td>
                            <td colspan="3">
                                &nbsp;<asp:DropDownList ID="tabDropDownList" runat="server" CssClass="NormalTextBox"
                                    DataSource="<%# portalTabs %>" DataTextField="Name" DataValueField="ID" Width="300px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td colspan="3">
                                <hr noshade="noshade" size="1" />
                            </td>
                        </tr>
                        <tr>
                            <td class="SubHead" valign="top" width="200">
                                <rbfwebui:Localize ID="Literal5" runat="server" Text="Roles that can view" TextKey="MODULESETTINGS_ROLE_VIEW">
                                </rbfwebui:Localize>:
                            </td>
                            <td colspan="3" nowrap="nowrap">
                                <asp:CheckBoxList ID="authViewRoles" runat="server" CellPadding="0" CellSpacing="0"
                                    CssClass="Normal" RepeatColumns="2" Width="300">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td class="SubHead" valign="top" width="200">
                                <rbfwebui:Localize ID="Literal4" runat="server" Text="Roles that can edit" TextKey="MODULESETTINGS_ROLES_EDIT">
                                </rbfwebui:Localize>:
                            </td>
                            <td colspan="3" nowrap="nowrap">
                                <asp:CheckBoxList ID="authEditRoles" runat="server" CellPadding="0" CellSpacing="0"
                                    CssClass="Normal" RepeatColumns="2" Width="300">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td class="SubHead" valign="top" width="200">
                                <rbfwebui:Localize ID="Literal6" runat="server" Text="Roles that can add" TextKey="MODULESETTINGS_ROLES_ADD">
                                </rbfwebui:Localize>:
                            </td>
                            <td colspan="3" nowrap="nowrap">
                                <asp:CheckBoxList ID="authAddRoles" runat="server" CellPadding="0" CellSpacing="0"
                                    CssClass="Normal" RepeatColumns="2" Width="300">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td class="SubHead" valign="top" width="200">
                                <rbfwebui:Localize ID="Literal7" runat="server" Text="Roles that can delete" TextKey="MODULESETTINGS_ROLES_DELETE">
                                </rbfwebui:Localize>:
                            </td>
                            <td colspan="3" nowrap="nowrap">
                                <asp:CheckBoxList ID="authDeleteRoles" runat="server" CellPadding="0" CellSpacing="0"
                                    CssClass="Normal" RepeatColumns="2" Width="300">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td class="SubHead" valign="top" width="200">
                                <rbfwebui:Localize ID="Literal8" runat="server" Text="Roles that can edit properties"
                                    TextKey="MODULESETTINGS_ROLES_EDIT_COLLECTION">
                                </rbfwebui:Localize>:
                            </td>
                            <td colspan="3" nowrap="nowrap">
                                <asp:CheckBoxList ID="authPropertiesRoles" runat="server" CellPadding="0" CellSpacing="0"
                                    CssClass="Normal" RepeatColumns="2" Width="300">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td class="SubHead" valign="top" width="200">
                                <rbfwebui:Localize ID="Literal16" runat="server" Text="Roles that can move modules"
                                    TextKey="MODULESETTINGS_ROLES_MOVE_MODULES">
                                </rbfwebui:Localize>:
                            </td>
                            <td colspan="3" nowrap="nowrap">
                                <asp:CheckBoxList ID="authMoveModuleRoles" runat="server" CellPadding="0" CellSpacing="0"
                                    CssClass="Normal" RepeatColumns="2" Width="300">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td class="SubHead" valign="top" width="200">
                                <rbfwebui:Localize ID="Literal17" runat="server" Text="Roles that can delete modules"
                                    TextKey="MODULESETTINGS_ROLES_DELETE_MODULES">
                                </rbfwebui:Localize>:
                            </td>
                            <td colspan="3" nowrap="nowrap">
                                <asp:CheckBoxList ID="authDeleteModuleRoles" runat="server" CellPadding="0" CellSpacing="0"
                                    CssClass="Normal" RepeatColumns="2" Width="300">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td colspan="3">
                                <hr noshade="noshade" size="1" />
                            </td>
                        </tr>
                        <tr>
                            <td class="SubHead" valign="top" width="200">
                                <rbfwebui:Localize ID="Literal9" runat="server" Text="Enable workflow" TextKey="MODULESETTINGS_SUPPORT_WORKFLOW">
                                </rbfwebui:Localize>:
                            </td>
                            <td colspan="3">
                                <asp:CheckBox ID="enableWorkflowSupport" runat="server" AutoPostBack="True" />
                            </td>
                        </tr>
                        <tr>
                            <td class="SubHead" valign="top" width="200">
                                <rbfwebui:Localize ID="Literal10" runat="server" Text="Approve roles" TextKey="MODULESETTINGS_ROLES_APPROVING">
                                </rbfwebui:Localize>:
                            </td>
                            <td colspan="3" nowrap="nowrap">
                                <asp:CheckBoxList ID="authApproveRoles" runat="server" CellPadding="0" CellSpacing="0"
                                    CssClass="Normal" RepeatColumns="2" Width="300">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td class="SubHead" valign="top" width="200">
                                <rbfwebui:Localize ID="Literal11" runat="server" Text="Publishing roles" TextKey="MODULESETTINGS_ROLES_PUBLISHING">
                                </rbfwebui:Localize>:
                            </td>
                            <td colspan="3" nowrap="nowrap">
                                <asp:CheckBoxList ID="authPublishingRoles" runat="server" CellPadding="0" CellSpacing="0"
                                    CssClass="Normal" RepeatColumns="2" Width="300">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td colspan="3">
                                <hr noshade="noshade" size="1" />
                            </td>
                        </tr>
                        <tr>
                            <td class="SubHead" nowrap="nowrap" width="200">
                                <rbfwebui:Localize ID="Literal12" runat="server" Text="Show to mobile users" TextKey="SHOWMOBILE">
                                </rbfwebui:Localize>:
                            </td>
                            <td colspan="3">
                                <asp:CheckBox ID="ShowMobile" runat="server" CssClass="Normal" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td colspan="3">
                                <hr noshade="noshade" size="1" />
                            </td>
                        </tr>
                        <tr>
                            <td class="SubHead" nowrap="nowrap" width="200">
                                <rbfwebui:Localize ID="Literal14" runat="server" Text="Show on every page?" TextKey="MODULESETTINGS_SHOW_EVERYWHERE">
                                </rbfwebui:Localize>:
                            </td>
                            <td colspan="3">
                                <asp:CheckBox ID="showEveryWhere" runat="server" CssClass="Normal" />
                            </td>
                        </tr>
                        <tr>
                            <td class="SubHead" nowrap="nowrap" width="200">
                                <rbfwebui:Localize ID="Literal15" runat="server" Text="Can collapse window?" TextKey="MODULESETTINGS_SHOW_COLLAPSABLE">
                                </rbfwebui:Localize>:
                            </td>
                            <td colspan="3">
                                <asp:CheckBox ID="allowCollapsable" runat="server" CssClass="Normal" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <hr noshade="noshade" size="1" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" nowrap="true">
                                <asp:PlaceHolder ID="PlaceHolderButtons" runat="server"></asp:PlaceHolder>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
   
</asp:Content>

