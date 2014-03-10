<%@ page autoeventwireup="false" inherits="Appleseed.Content.Web.Modules.ComponentModuleEdit"
    language="c#" MasterPageFile="~/Shared/SiteMasterDefault.master" Codebehind="ComponentModuleEdit.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">



            <div class="div_ev_Table">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td align="left" class="Head">
                            Details
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr noshade="noshade" size="1" />
                        </td>
                    </tr>
                </table>
                
                <table cellpadding="0" cellspacing="0" width="750">
                    <tr valign="top">
                        <td class="SubHead" width="100">
                            Title
                        </td>
                        <td rowspan="4">
                            &nbsp;
                        </td>
                        <td>
                            <asp:textbox id="TitleField" runat="server" columns="30" cssclass="NormalTextBox"
                                maxlength="150" width="490">
                            </asp:textbox>
                        </td>
                        <td rowspan="4" width="25">
                            &nbsp;
                        </td>
                        <td class="Normal" width="250">
                            <asp:requiredfieldvalidator id="RequiredTitle" runat="server" controltovalidate="TitleField"
                                display="Dynamic">
                            </asp:requiredfieldvalidator>
                        </td>
                    </tr>
                </table>
                    <%--<tr valign="top">
                        <td></td>
                        <td >--%>
                            <br/>
                           <div class="normal">
                                <asp:PlaceHolder ID="PlaceHolderComponentEditor" runat="server"></asp:PlaceHolder>
                         </div>
                        <%--</td>
                        <td>
                            <asp:textbox id="ComponentField" runat="server" columns="44" rows="10" textmode="Multiline"
                                width="490">
                            </asp:textbox>
                        </td>
                        <td class="Normal">
                            <asp:requiredfieldvalidator id="RequiredComponent" runat="server" controltovalidate="ComponentField"
                                display="Dynamic">
                            </asp:requiredfieldvalidator>
                        </td>--%>
                    <%--</tr>
                </table>--%>
                <p>
                    <rbfwebui:linkbutton id="UpdateButton" runat="server" class="CommandButton" text="UPDATE">
                    </rbfwebui:linkbutton>
                    &nbsp;
                    <rbfwebui:linkbutton id="CancelButton" runat="server" causesvalidation="False" class="CommandButton"
                        text="CANCEL">
                    </rbfwebui:linkbutton>
                </p>
                <hr noshade="noshade" size="1" width="600" />
                <span class="Normal">
                    <rbfwebui:localize id="CreatedLabel" runat="server" text="Created by" textkey="CREATED_BY">
                    </rbfwebui:localize>&nbsp;
                    <rbfwebui:label id="CreatedBy" runat="server"></rbfwebui:label>&nbsp;
                    <rbfwebui:localize id="OnLabel" runat="server" text="on" textkey="ON">
                    </rbfwebui:localize>&nbsp;
                    <rbfwebui:label id="CreatedDate" runat="server"></rbfwebui:label>
                </span>
                
                </div>

</asp:Content>