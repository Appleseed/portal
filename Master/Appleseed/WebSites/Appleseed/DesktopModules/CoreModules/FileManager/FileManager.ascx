<%@ control autoeventwireup="false" inherits="Appleseed.Content.Web.Modules.FileManager"
    language="c#" targetschema="http://schemas.microsoft.com/intellisense/ie5" Codebehind="FileManager.ascx.cs" %>
<!-- *** File Directory Tree *** -->
<asp:placeholder id="myPlaceHolder" runat="server"></asp:placeholder>

<script language="javascript" type="text/javascript">
function btnDelete_Click() {
	a = window.confirm("Are you sure want to delete this Files ?");
	if(a) {
		return true;
	}
	else {
		return false;
	}
}
function imgACL_Click(filename) {
	alert(filename);
}
</script>

<table border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td>
            <rbfwebui:imagebutton id="btnGoUp" runat="server" alternatetext="Up One Level" imagealign="AbsMiddle"
                imageurl="images/btnUp.jpg" />&nbsp;&nbsp;&nbsp;<rbfwebui:label id="lblError" runat="server"
                    forecolor="red"></rbfwebui:label></td>
        <td>
            <rbfwebui:imagebutton id="btnDelete" runat="server" alternatetext="Delete selected files"
                imagealign="AbsMiddle" imageurl="images/btnDelete.jpg" /></td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:datagrid id="dgFile" runat="server" allowsorting="True" alternatingitemstyle-cssclass="Grid_AlternatingItem Normal"
                alternatingitemstyle-wrap="false" autogeneratecolumns="False" borderwidth="0"
                cellpadding="0" cellspacing="0" headerstyle-cssclass="Grid_Header NormalBold"
                itemstyle-cssclass="Grid_Item Normal" itemstyle-wrap="false" width="100%">
                <columns>
                    <rbfwebui:templatecolumn>
                        <itemtemplate>
                            <asp:CheckBox ID="chkChecked" runat="server"></asp:CheckBox>
                        </itemtemplate>
                    </rbfwebui:templatecolumn>
                    <rbfwebui:templatecolumn headertext="FileName" sortexpression="fileName" textkey="FILEMAN_FILENAME">
                        <headerstyle horizontalalign="left" />
                        <itemstyle />
                        <itemtemplate><span style="white-space:nowrap;">
							&nbsp;
							<ASP:PLACEHOLDER ID="plhImgEdit" RUNAT="server"></ASP:PLACEHOLDER>
							<ASP:IMAGE ID="imgType" RUNAT="server" BORDERWIDTH="0" BORDERSTYLE="None" ImageAlign="absmiddle"></ASP:IMAGE>
							<rbfwebui:LinkButton ID="lnkName" CSSCLASS ="FileManager" RUNAT="server" TEXT='<%# DataBinder.Eval(Container.DataItem,"filename") %>' COMMANDNAME="ItemClicked" CAUSESVALIDATION="false">
							</rbfwebui:LinkButton>
							</span>
						</itemtemplate>
                        <edititemtemplate><span style="white-space:nowrap;">
							&nbsp;
							<ASP:PLACEHOLDER ID="plhImgEdit" RUNAT="server"></ASP:PLACEHOLDER>
							<ASP:IMAGE ID="imgType" RUNAT="server" BORDERWIDTH="0" BORDERSTYLE="None" ImageAlign="absmiddle"></ASP:IMAGE>
							<ASP:TEXTBOX ID="txtEditName" RUNAT="server" TEXT='<%# DataBinder.Eval(Container.DataItem,"filename") %>'>
							</ASP:TEXTBOX></span>
						</edititemtemplate>
                    </rbfwebui:templatecolumn>
                    <rbfwebui:boundcolumn datafield="size" headertext="Size (KB)" readonly="True" sortexpression="size"
                        textkey="FILEMAN_SIZE">
                        <headerstyle horizontalalign="Right" />
                        <itemstyle horizontalalign="Right" />
                    </rbfwebui:boundcolumn>
                    <rbfwebui:boundcolumn datafield="modified" headertext="Modified" readonly="True"
                        sortexpression="modified" textkey="FILEMAN_MODIFIED">
                        <headerstyle horizontalalign="Right" />
                        <itemstyle horizontalalign="Right" />
                    </rbfwebui:boundcolumn>
                    <rbfwebui:templatecolumn itemstyle-horizontalalign="Right">
                        <itemtemplate>
                            <span style="white-space: nowrap;">&nbsp;
                                <rbfwebui:LinkButton ID="LinkButton1" runat="server" CommandName="Edit" CausesValidation="false"
                                    Text=" [Rename]"></rbfwebui:LinkButton>
                                &nbsp;</span>
                        </itemtemplate>
                        <edititemtemplate>
                            <span style="white-space: nowrap;">&nbsp;
                                <rbfwebui:LinkButton ID="LinkButton3" runat="server" CommandName="Update" Text=" [Update]"></rbfwebui:LinkButton>&nbsp;
                                <rbfwebui:LinkButton ID="LinkButton2" runat="server" CommandName="Cancel" CausesValidation="false"
                                    Text=" [Cancel]"></rbfwebui:LinkButton>
                                &nbsp;</span>
                        </edititemtemplate>
                    </rbfwebui:templatecolumn>
                </columns>
            </asp:datagrid></td>
    </tr>
    <tr class="Grid_Header NormalBold">
        <td align="left" colspan="2">
            <rbfwebui:label id="lblCounter" runat="server"></rbfwebui:label></td>
    </tr>
    <tr>
        <td colspan="2">
            &nbsp;</td>
    </tr>
</table>
<br>
<table border="0" cellpadding="2" cellspacing="0">
    <tr class="Normal">
        <td nowrap="nowrap">
            <b>Create New Directory&nbsp;:</b>&nbsp;<asp:textbox id="txtNewDirectory" runat="server"
                enableviewstate="False" width="224px"></asp:textbox>&nbsp;<rbfwebui:imagebutton id="btnNewFolder"
                    runat="server" alternatetext="New Folder" imageurl="images/btnNewFolder.jpg" /></td>
    </tr>
    <tr class="Normal">
        <td>
            &nbsp;</td>
    </tr>
    <tr class="Normal">
        <td>
            <b>Upload File (You can upload up to 3 files at the same time)</b></td>
    </tr>
    <tr class="Normal">
        <td>
            1.&nbsp;<input id="file1" runat="server" name="file1" size="28" type="file" /><br />
            2.&nbsp;<input id="file2" runat="server" name="file2" size="28" type="file" /><br />
            3.&nbsp;<input id="file3" runat="server" name="file3" size="28" type="file" /><br />
            &nbsp;&nbsp;&nbsp;&nbsp;<rbfwebui:button id="btnUpload" runat="server" text="Upload" /></td>
    </tr>
</table>
<!-- *** End of File Directory Tree *** -->
