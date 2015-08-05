<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="XmlImportExport.ascx.cs" Inherits="Appleseed.DesktopModules.CoreModules.XMLImportExport.XmlImportExport" %>

<script type="text/javascript">
    var pageid = '';
    function client_OnTreeNodeChecked(event) {
        var treeNode = event.srcElement || event.target;
        if (treeNode.tagName == "INPUT" && treeNode.type == "checkbox") {
            if (treeNode.checked) {
                pageid = treeNode.id;
                uncheckOthers(treeNode.id);
            }
            else {
                pageid = '';
            }
        }
    }

    function uncheckOthers(id) {
        var elements = document.getElementsByTagName('input');
        // loop through all input elements in form
        for (var i = 0; i < elements.length; i++) {
            if (elements.item(i).type == "checkbox") {
                if (elements.item(i).id != id) {
                    elements.item(i).checked = false;
                }
            }
        }
    }
</script>

<style>
    .LeftFloat {
        width: 50%;
        float: left;
        padding: 5px;
    }
</style>

<div style="width: 100%;">
    <div class="LeftFloat">
        <h3>Export XML</h3>
        <div id="divTree" class="trvClass" style="height: 300px; overflow: auto;">
            <asp:TreeView ID="pagesTree" runat="server" CssClass="sitecontent" ShowCheckBoxes="All" ExpandDepth="0" />
            <input type="button" value="Export" class="ExportClass" />
        </div>
    </div>

    <div class="LeftFloat">
        <div>
            <h3>Import XML</h3>
            <asp:FileUpload ID="fileUploadXML" runat="server" />
            <br />
            <asp:Button ID="btnImportXml" runat="server" Text="Import" OnClick="btnImportXml_Click" />
        </div>
        <div>
            <asp:Repeater ID="rptLogTable" runat="server">
                <HeaderTemplate>
                    <table id="tblLog" border="0">
                        <tr>
                            <th>
                                <h4>Import Log table</h4>
                            </th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td><%#Eval("LogMessage")%> </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
</div>

<asp:SiteMapDataSource ID="biSMDS" ShowStartingNode="false" runat="server" />

<script type="text/javascript">

    $("#divTree td").css("padding", "0px");

    $('.ExportClass').click(function (e) {
        if (pageid != '') {
            e.preventDefault();
            window.location.href = '/DesktopModules/CoreModules/XMLImportExport/ExportXml.aspx?pid=' + $("#" + pageid).next().attr("href").split('/')[1];
            return false;
        }
        else {
            alert("Please select page to export");
            return false;
        }
    });
</script>
