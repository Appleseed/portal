<%@ Page Language="C#" CodeBehind="HtmlVersonHistory.aspx.cs" Inherits="Appleseed.DesktopModules.CoreModules.HTMLDocument.HtmlVersonHistory" MasterPageFile="~/Shared/ModalMaster.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <div>
        <p>
            <asp:Button ID="btnCompareAndMarge" Text="Compare and Merge" CssClass="CommandButton" runat="server" OnClientClick="return CheckVersionSelection();" OnClick="btnCompareAndMarge_Click" />
            <%--<asp:Button ID="btnCompareAndMarge" Text="Compare and Merge" runat="server" OnClientClick="CheckVersionSelection()" />--%>
            <asp:Button ID="btnBack" Text="Back" CssClass="CommandButton" runat="server" OnClick="btnBack_Click" />
            <asp:Button ID="btnDelete" Text="Delete" runat="server" CssClass="CommandButton" OnClientClick="return DeleteVersion();" OnClick="btnDelete_Click" />
        </p>
        <asp:Repeater ID="RptVersionHistory" runat="server">
            <HeaderTemplate>
                <table id="TblVersionHistory" class="display tblclsVersion" border="1" cellspacing="1" style="border: 1px solid #000; border-collapse: collapse; border-right: 1px solid #000">
                    <colgroup>
                        <col style="width: 80px" />
                        <col style="width: 80px" />
                        <col style="width: 180px" />
                        <col style="width: 160px" />
                        <col style="width: 180px" />
                        <col style="width: 160px" />
                    </colgroup>
                    <thead style="border: 1px solid #000;">
                        <tr style="border: 1px solid #000;">
                            <th>Select</th>
                            <th>VersionNo</th>
                            <th>Created By UserName</th>
                            <th>Created Date</th>
                            <th>Modified By UserName</th>
                            <th>Modified Date</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody style="border: 1px solid #000;">
            </HeaderTemplate>
            <ItemTemplate>
                <tr style="border: 1px solid #000;">
                    <td style="text-align: center;">
                        <asp:CheckBox ID="chkVersion" verno='<%#Eval("VersionNo")%>' CssClass="chVersion" runat="server" onclick="CheckBoxCount()" />
                        <asp:HiddenField ID="hdnVersionNo" Value='<%#Eval("VersionNo")%>' runat="server" />
                    </td>
                    <td><%# Eval("VersionNo") %>                  </td>
                    <td><%#Eval("CreatedByUserName")%></td>
                    <td><%# Eval("CreatedDate")%></td>
                    <td><%#Eval("ModifiedByUserName")%></td>
                    <td><%#Eval("ModifiedDate")%></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody>
                <tfoot>
                    <tr>
                    </tr>
                </tfoot>
                </table>
            </FooterTemplate>
        </asp:Repeater>
        <p>
            Note: Published version will not be displayed in above version history.
        </p>
    </div>
    <script type="text/javascript">
        var verList = "";
        function CheckBoxCount() {
            var verCount = 0;
            $('.tblclsVersion').find('tr').each(function () {
                var row = $(this);
                var hv = $('.chVersion').find('verno').val();
                if (row.find('input[type="checkbox"]').is(':checked')) {
                    verCount += 1;
                    if (verCount <= 2) {
                        verList = verList + ',' + $(row).find('[id*=hdnVersionNo]').val();
                    }
                    else {
                        alert("Select any two versions to compare and merge.");
                    }
                }
            });
        }

        function CheckVersionSelection() {
            var verCount = 0;
            $('.tblclsVersion').find('tr').each(function () {
                var row = $(this);
                if (row.find('input[type="checkbox"]').is(':checked')) {
                    verCount += 1;
                }
            });

            if (verCount != 2) {
                alert("Select any two versions to compare and merge.");
                return false;
            }
        }

        function DeleteVersion() {
            if (confirm("Are you sure want to delete?")) {
                return true;
            }
            return false;
        }

    </script>
</asp:Content>

