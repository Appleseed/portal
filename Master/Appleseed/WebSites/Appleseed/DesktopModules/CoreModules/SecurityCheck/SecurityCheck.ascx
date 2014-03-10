<%@ control autoeventwireup="false" inherits="Appleseed.Content.Web.Modules.SecurityCheck"
    language="c#" Codebehind="SecurityCheck.ascx.cs" %>
<table border="0" cellpadding="2" cellspacing="0">
    <tr>
        <td>
            <rbfwebui:localize id="label_description" runat="server" text="Roles" textkey="ROLES_DESCRIPTION">
            </rbfwebui:localize>
            <asp:dropdownlist id="ddlRoles" runat="server" datatextfield="RoleName" datavaluefield="RoleID">
            </asp:dropdownlist>
            <rbfwebui:button id="btnSearch" runat="server" text="Search" textkey="SECURITYCHECK_SEARCH" />
            <asp:checkbox id="chkAdmin" runat="server" text="Only Admin Modules" textkey="SECURITYCHECK_ONLYADMIN" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:datagrid id="dgModules" runat="server" allowpaging="True" allowsorting="True"
                autogeneratecolumns="False" datakeyfield="ModuleID">
                <footerstyle cssclass="Grid_Footer" />
                <alternatingitemstyle cssclass="Grid_AlternatingItem" />
                <itemstyle cssclass="Grid_Item" />
                <headerstyle cssclass="Grid_Header" />
                <columns>
                    <rbfwebui:templatecolumn headertext="Page" sortexpression="PageName" textkey="SECURITYCHECK_HEADERTAB">
                        <itemtemplate>
                            <rbfwebui:label id="Label2" runat="server" text='<%# DataBinder.Eval(Container, "DataItem.PageName") %>'>

                            </rbfwebui:label>
                        </itemtemplate>
                    </rbfwebui:templatecolumn>
                    <rbfwebui:templatecolumn headertext="Name" sortexpression="ModuleTitle" textkey="SECURITYCHECK_HEADERNAME">
                        <itemtemplate>
                            <rbfwebui:label id="Label1" runat="server" text='<%# DataBinder.Eval(Container, "DataItem.ModuleTitle") %>'>

                            </rbfwebui:label>
                        </itemtemplate>
                    </rbfwebui:templatecolumn>
                    <rbfwebui:templatecolumn headertext="Module Type" sortexpression="FriendlyName" textkey="SECURITYCHECK_HEADERTYPE">
                        <itemtemplate>
                            <rbfwebui:label id="Label3" runat="server" text='<%# DataBinder.Eval(Container, "DataItem.FriendlyName") %>'>

                            </rbfwebui:label>
                        </itemtemplate>
                    </rbfwebui:templatecolumn>
                    <rbfwebui:templatecolumn headertext="Admin" sortexpression="IsAdmin" textkey="SECURITYCHECK_HEADERADMIN">
                        <itemtemplate>
                            <rbfwebui:label id="Label4" runat="server" text='<%# DataBinder.Eval(Container, "DataItem.IsAdmin") %>'>

                            </rbfwebui:label>
                        </itemtemplate>
                    </rbfwebui:templatecolumn>
                    <rbfwebui:templatecolumn headertext="View" sortexpression="CanView" textkey="SECURITYCHECK_HEADERVIEW">
                        <itemtemplate>
                            <rbfwebui:label id="Label5" runat="server" text='<%# DataBinder.Eval(Container, "DataItem.CanView") %>'>

                            </rbfwebui:label>
                        </itemtemplate>
                    </rbfwebui:templatecolumn>
                    <rbfwebui:templatecolumn headertext="Add" sortexpression="CanAdd" textkey="SECURITYCHECK_HEADERADD">
                        <itemtemplate>
                            <rbfwebui:label id="Label6" runat="server" text='<%# DataBinder.Eval(Container, "DataItem.CanAdd") %>'>

                            </rbfwebui:label>
                        </itemtemplate>
                    </rbfwebui:templatecolumn>
                    <rbfwebui:templatecolumn headertext="Edit" sortexpression="CanEdit" textkey="SECURITYCHECK_HEADEREDIT">
                        <itemtemplate>
                            <rbfwebui:label id="Label7" runat="server" text='<%# DataBinder.Eval(Container, "DataItem.CanEdit") %>'>

                            </rbfwebui:label>
                        </itemtemplate>
                    </rbfwebui:templatecolumn>
                    <rbfwebui:templatecolumn headertext="Delete" sortexpression="CanDelete" textkey="SECURITYCHECK_HEADERDELETE">
                        <itemtemplate>
                            <rbfwebui:label id="Label8" runat="server" text='<%# DataBinder.Eval(Container, "DataItem.CanDelete") %>'>

                            </rbfwebui:label>
                        </itemtemplate>
                    </rbfwebui:templatecolumn>
                    <rbfwebui:templatecolumn headertext="Properties" sortexpression="CanProperties" textkey="SECURITYCHECK_HEADERPROPERTIES">
                        <itemtemplate>
                            <rbfwebui:label id="Label9" runat="server" text='<%# DataBinder.Eval(Container, "DataItem.CanProperties") %>'>

                            </rbfwebui:label>
                        </itemtemplate>
                    </rbfwebui:templatecolumn>
                    <rbfwebui:templatecolumn headertext="Move" sortexpression="CanMoveModule" textkey="SECURITYCHECK_HEADERMOVE">
                        <itemtemplate>
                            <rbfwebui:label id="Label10" runat="server" text='<%# DataBinder.Eval(Container, "DataItem.CanMove") %>'>

                            </rbfwebui:label>
                        </itemtemplate>
                    </rbfwebui:templatecolumn>
                    <rbfwebui:templatecolumn headertext="Delete Mod." sortexpression="CanDeleteModule"
                        textkey="SECURITYCHECK_HEADERDELMOD">
                        <itemtemplate>
                            <rbfwebui:label id="Label11" runat="server" text='<%# DataBinder.Eval(Container, "DataItem.CanDeleteModule") %>'>

                            </rbfwebui:label>
                        </itemtemplate>
                    </rbfwebui:templatecolumn>
                </columns>
                <pagerstyle mode="NumericPages" />
            </asp:datagrid>
        </td>
    </tr>
</table>
