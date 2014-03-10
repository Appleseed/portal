<%@ control autoeventwireup="false" inherits="Appleseed.Content.Web.Modules.DatabaseTool"
    language="c#" Codebehind="DatabaseTool.ascx.cs" %>
<rbfwebui:label id="lblConnectedError" runat="server" forecolor="Red">
</rbfwebui:label>
<asp:panel id="panConnected" runat="server">
    <rbfwebui:label id="Label1" runat="server" textkey="DBTOOL_OWNER">Owner</rbfwebui:label>&nbsp;
    <asp:textbox id="tbUserName" runat="server" cssclass="NormalTextBox" width="60px">dbo</asp:textbox>&nbsp;
    <asp:dropdownlist id="ddObjectSelectList" runat="server" autopostback="true" cssclass="NormalTextBox">
        <asp:listitem selected="True" value="U">User table</asp:listitem>
        <asp:listitem value="P">Stored procedure</asp:listitem>
        <asp:listitem value="C">CHECK constraint</asp:listitem>
        <asp:listitem value="D">Default or DEFAULT constraint</asp:listitem>
        <asp:listitem value="F">FOREIGN KEY constraint</asp:listitem>
        <asp:listitem value="L">Log</asp:listitem>
        <asp:listitem value="FN">Scalar function</asp:listitem>
        <asp:listitem value="IF">Inlined table-function</asp:listitem>
        <asp:listitem value="PK">PRIMARY KEY constraint (type is K)</asp:listitem>
        <asp:listitem value="RF">Replication filter stored procedure </asp:listitem>
        <asp:listitem value="S">System table</asp:listitem>
        <asp:listitem value="TF">Table function</asp:listitem>
        <asp:listitem value="TR">Trigger</asp:listitem>
        <asp:listitem value="UQ">UNIQUE constraint (type is K)</asp:listitem>
        <asp:listitem value="V">View</asp:listitem>
        <asp:listitem value="X">Extended stored procedure</asp:listitem>
    </asp:dropdownlist><br/>
    <asp:listbox id="lbObjects" runat="server" cssclass="NormalTextBox" height="100px"></asp:listbox><br/>
    <rbfwebui:button id="btnGetObjectProps" runat="server" text="Properties" textkey="DBTOOL_GETPROPERTIES" />
    <rbfwebui:button id="btnGetObjectInfo" runat="server" text="Info" textkey="DBTOOL_GETINFO" />
    <rbfwebui:button id="btnGetObjectInfoExtended" runat="server" text="InfoExtended"
        textkey="DBTOOL_GETINFOEXTENDED" />
    <rbfwebui:button id="btnGetObjectData" runat="server" text="Data" textkey="DBTOOL_GETDATA" /><br/>
    <asp:panel id="panQueryBox" runat="server">
        <br/>
        <asp:textbox id="txtQueryBox" runat="server" cssclass="NormalTextBox" height="150px"
            textmode="MultiLine" width="100%"></asp:textbox>
        <br/>
        <rbfwebui:button id="btnQueryExecute" runat="server" text="Execute Query" textkey="DBTOOL_QUERYEXECUTE" />
        <rbfwebui:label id="lblRes" runat="server" forecolor="Red"></rbfwebui:label>
        <br/>
    </asp:panel>
    <br/>
    <asp:datagrid id="DataGrid1" runat="server" backcolor="White" bordercolor="#697898"
        borderwidth="1px" cellpadding="3" font-size="Smaller" height="90px" horizontalalign="Center"
        pagesize="20" width="100%">
        <headerstyle backcolor="#697898" font-bold="True" forecolor="White" />
        <alternatingitemstyle backcolor="#F0F0F0" />
        <itemstyle bordercolor="White" borderwidth="2px" forecolor="#000066" horizontalalign="Left"
            width="80%" />
    </asp:datagrid>
</asp:panel>
