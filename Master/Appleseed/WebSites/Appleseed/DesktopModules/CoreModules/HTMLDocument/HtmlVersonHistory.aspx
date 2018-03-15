<%@ Page Language="C#" CodeBehind="HtmlVersonHistory.aspx.cs" Inherits="Appleseed.DesktopModules.CoreModules.HTMLDocument.HtmlVersonHistory" MasterPageFile="~/Shared/ModalMaster.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <link rel="stylesheet" type="text/css" href="//cdn.datatables.net/1.10.16/css/jquery.dataTables.min.css" />
    <script type="text/javascript" src="//cdn.datatables.net/v/bs/dt-1.10.16/datatables.min.js"></script>

    <div class="">
        <div class="">
            <div class="col-lg-12" style="background-color: #fff">
                <div style="min-height:625px;">
                    <asp:Repeater ID="RptVersionHistory" runat="server">
                        <HeaderTemplate>
                            <table id="TblVersionHistory" class="display tblclsVersion">

                                <thead>
                                    <tr>
                                        <th class="text-center">Select</th>
                                        <th>Version Number</th>
                                        <th>Created By User Name</th>
                                        <th>Created Date</th>
                                        <th>Modified By User Name</th>
                                        <th>Modified Date</th>
                                    </tr>
                                </thead>
                                <tbody style="border: 1px solid #000;">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td class="text-center" style="width: 80px;">
                                    <asp:CheckBox ID="chkVersion" verno='<%#Eval("VersionNo")%>' CssClass="chVersion" runat="server" onclick="CheckBoxCount(this)" />
                                    <asp:HiddenField ID="hdnVersionNo" Value='<%#Eval("VersionNo")%>' runat="server" />
                                </td>
                                <td><%# Eval("VersionNo") %>                  </td>
                                <td><%#Eval("CreatedByUserName")%></td>
                                <td><%# Convert.ToDateTime(Eval("CreatedDate")).ToString("yyyy/MM/dd HH:mm:ss")%></td>
                                <td><%#Eval("ModifiedByUserName")%></td>
                                <td><%# Eval("ModifiedDate") == DBNull.Value ? "" : Convert.ToDateTime(Eval("ModifiedDate")).ToString("yyyy/MM/dd HH:mm:ss")%></td>
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
                </div>
                <div class="text-right">
                    *Published version will not be displayed in above version history.<br />
                    **<%= TimeZone.CurrentTimeZone.StandardName %> Timezone
                </div>

                <p>
                    <asp:Button ID="btnCompareAndMarge" Text="Compare and Merge" CssClass="CommandButton" runat="server" OnClientClick="return CheckVersionSelection();" OnClick="btnCompareAndMarge_Click" />
                    <%--<asp:Button ID="btnCompareAndMarge" Text="Companre and Merge" runat="server" OnClientClick="CheckVersionSelection()" />--%>
                    <asp:Button ID="btnBack" Text="Back" CssClass="CommandButton" runat="server" OnClick="btnBack_Click" />
                    <asp:Button ID="btnDelete" Text="Delete" runat="server" CssClass="CommandButton" OnClientClick="return DeleteVersion();" OnClick="btnDelete_Click" />
                </p>
            </div>

        </div>
    </div>
    <script type="text/javascript">
        var verList = "";
        $(document).ready(function () {
            $('#TblVersionHistory').DataTable({ "paging": false, "order": [[1, "desc"]] });
        });

        function CheckBoxCount(chk) {

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
            var verCount = 0;
            $('.tblclsVersion').find('tr').each(function () {
                var row = $(this);
                if (row.find('input[type="checkbox"]').is(':checked')) {
                    verCount += 1;
                }
            });

            if (verCount == 0) {
                alert("You haven't selected any versions to delete, please check the checkbox next to the version you wish to delete first.");
                return false;
            }

            if (confirm("Are you sure want to delete?")) {
                return true;
            }
            return false;
        }

    </script>
</asp:Content>

