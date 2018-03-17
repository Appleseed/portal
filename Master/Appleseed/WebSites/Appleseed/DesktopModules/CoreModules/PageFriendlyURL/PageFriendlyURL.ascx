<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageFriendlyURL.ascx.cs" Inherits="Appleseed.DesktopModules.CoreModules.PageFriendlyURL.PageFriendlyURL" %>
<link rel="stylesheet" type="text/css" href="//cdn.datatables.net/1.10.16/css/jquery.dataTables.min.css" />
<script type="text/javascript" src="//cdn.datatables.net/v/bs/dt-1.10.16/datatables.min.js"></script>

<style>
    #Content_ContentPane_ctl01_gdPages>tbody>tb>span{
        color:#000 !important;
    }
    .tHead, .tHead span{
        background-color:#fff;
        color:#000 !important;
    }
    .tHead span{
        margin: 10px 18px;
    }
    .dataTables_filter label{
        color:#fff;
    }
    #Content_ContentPane_ctl01_gdPages_info span{
        color:#000000 !important;
    }
    .dataTables_wrapper .dataTables_info{
        color:#fff;
    }
    tr.odd{
        background-color: #f9f9f9 !important;
    }

    tr.odd td, tr.even td
    {
        padding: 8px 10px !important;
    }
     tr.odd td span, tr.even td span
    {
        color:#000000 !important;
    }
     tr.odd:hover, tr.even:hover
     {
             background-color: #f6f6f6;
     }
</style>

<div style="width: 100%">


    <div class="module-row">
        <div id="url-instructions">
            <p>To create a "Short" url, select the specific page to which the Short url should be applied from the Page dropdown list. </p>
            <p>Then, in the Short URL text box, create the Short url.</p>
        </div>
    </div>
    <div class="module-row">
        <br />
        <h5>Add Short Url</h5>
    </div>
    <div class="module-row">
        <div class="col-md-9">
            <div id="divSuccessMessage" runat="server" role="alert" visible="false">
                <h5 style="color: green">Saved successfully</h5>
            </div>
            <div id="divErrorMessage" runat="server" role="alert" visible="false">
                <h5 style="color: red">Specified Friedly Url is already exists. Please specify another/different.</h5>
            </div>
        </div>
    </div>
    <div class="module-row">
        <div class="col-md-9">
            <div class="col-md-3">
                <h6>Page</h6>
            </div>
            <div class="col-md-6">
                <asp:DropDownList ID="drpPageList" CssClass="FixTop" runat="server" ViewStateMode="Enabled" Width="265" AutoPostBack="true" OnSelectedIndexChanged="drpPageList_SelectedIndexChanged" />
                <div runat="server" id="divDyanamicPage">
                    <asp:TextBox ID="txtDyanmicPage" CssClass="FixTop" runat="server" Width="265" PlaceHolder="Custom Destination Page Url" /><br />
                    <asp:RequiredFieldValidator ID="rqvtxtDyanmicPage" runat="server" Display="Dynamic" ErrorMessage="Please enter valid Url " ControlToValidate="txtDyanmicPage" ValidationGroup="Save" />
                </div>
            </div>
        </div>
    </div>
    <div class="clear"></div>
    <div class="module-row">
        <div class="col-md-9">
            <div class="col-md-3">
                <h6>Short URL</h6>
            </div>
            <div class="col-md-6">
                <asp:TextBox ID="txtFriendlyURL" CssClass="FixTop" runat="server" Width="265" PlaceHolder="Short Page Url" />
                <asp:Label ID="lblFriendlyExtension" runat="server" /><br />
                <asp:RequiredFieldValidator ID="rvftxtFriendlyUrl" runat="server" Display="Dynamic" ErrorMessage="Please enter valid Url " ControlToValidate="txtFriendlyURL" ValidationGroup="Save" />
            </div>
        </div>
    </div>
    <div class="module-row">
        <div class="col-md-9">
            <div class="col-md-9">
                <asp:Button ID="btnSave" Text="Save" CssClass="buttonMargin CommandButton" runat="server" OnClick="btnSave_Click" CausesValidation="true" ValidationGroup="Save" />
                <asp:Button ID="btnSaveWithoutExtension" Text="Save Without Extension" CssClass="buttonMargin CommandButton" runat="server" OnClick="btnSaveWithoutExtension_Click" CausesValidation="true" ValidationGroup="Save" />
            </div>
        </div>
    </div>
    <br />
    <div class="module-row">
        <asp:GridView ID="gdPages" runat="server" Width="100%" class="updatedelete" AutoGenerateColumns="false" CellSpacing="1"  OnRowEditing="GdPages_RowEditing"
            OnRowDeleting="gdPages_RowDeleting" OnRowCancelingEdit="gdPages_RowCancelingEdit" OnRowDataBound="gdPages_RowDataBound" OnRowUpdating="gdPages_RowUpdating" AlternatingRowStyle-BorderStyle="Solid" Style="border: 1px solid #000; border-collapse: collapse; border-right: 1px solid #000" EmptyDataText="No Record Found">
            <Columns>
                <asp:TemplateField HeaderStyle-CssClass="tHead" ItemStyle-CssClass="" HeaderText="Page ID">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblPageID" Text='<%# Eval("PageID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderStyle-CssClass="tHead" ItemStyle-CssClass="" HeaderText="Destination Page URL">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblPageFullName"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderStyle-CssClass="tHead" ItemStyle-CssClass="" HeaderText="Short URL">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblPageFriendlyUrl"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtFriendlyUrl" runat="server" Width="90%" Text='<%# Eval("FriendlyUrl") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rqfvFriendlyUrl" runat="server" ControlToValidate="txtFriendlyUrl" ValidationGroup="EditFriendlyUrl" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:CommandField ShowEditButton="true" ButtonType="Button" ValidationGroup="EditFriendlyUrl" HeaderStyle-CssClass="tHead  no-sort" ItemStyle-CssClass="" ControlStyle-CssClass="CommandButton" runat="server" />

                <asp:TemplateField HeaderStyle-CssClass="no-sort tHead" ItemStyle-CssClass="">
                    <ItemTemplate>
                        <%--<asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="CommandButton" OnClientClick="return confirm('Are you sure you want to delete short url?');" Text="Delete"></asp:LinkButton>--%>
                        <asp:Button ID="lnkDelete" runat="server" CommandName="Delete" CssClass="CommandButton" OnClientClick="return confirm('Are you sure you want to delete short url?');" Text="Delete" />
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
            <HeaderStyle Font-Bold="true" />
        </asp:GridView>

    </div>
    <br />
    <label>Dynamic Pages</label>
    <div class="module-row">
        <asp:GridView ID="gdDynamicPages" runat="server" Width="100%" class="updatedelete" AutoGenerateColumns="false" CellSpacing="1" BorderColor="Black" OnRowUpdating="gdDynamicPages_RowUpdating" OnRowCancelingEdit="gdDynamicPages_RowCancelingEdit" OnRowEditing="GdDynamicPages_RowEditing" OnRowDeleting="gdDynamicPages_RowDeleting" OnRowDataBound="gdDynamicPages_RowDataBound" AlternatingRowStyle-BorderStyle="Solid" Style="border: 1px solid #000; border-collapse: collapse; border-right: 1px solid #000" EmptyDataText="No Record Found">
            <Columns>
                <asp:TemplateField HeaderStyle-CssClass="tHead" ItemStyle-CssClass="" HeaderText="SR#">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblSrNo" Text='<%# Eval("SRINDEX") %>'></asp:Label>
                        <asp:Label runat="server" ID="lblPageID" Visible="false" Text='<%# Eval("DynamicPageId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderStyle-CssClass="tHead" ItemStyle-CssClass="" HeaderText="Destination Page URL">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblPageFullName" Text='<%# Eval("RedirectToUrl") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtRedirectToUrl" runat="server" Width="90%" Text='<%# Eval("RedirectToUrl") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rqfvRedirectToUrl" runat="server" ControlToValidate="txtRedirectToUrl" ValidationGroup="EditFriendlyUrl" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderStyle-CssClass="tHead" ItemStyle-CssClass="" HeaderText="Short URL">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblPageFriendlyUrl"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtFriendlyUrl" runat="server" Width="90%" Text='<%# Eval("FriendlyUrl") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rqfvFriendlyUrl" runat="server" ControlToValidate="txtFriendlyUrl" ValidationGroup="EditFriendlyUrl" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:CommandField ShowEditButton="true" ButtonType="Button" ValidationGroup="EditFriendlyUrl" HeaderStyle-CssClass="tHead  no-sort" ItemStyle-CssClass="" ControlStyle-CssClass="CommandButton" runat="server" />

                <asp:TemplateField HeaderStyle-CssClass="tHead no-sort" ItemStyle-CssClass="">
                    <ItemTemplate>
                        <asp:Button ID="lnkDelete" runat="server" CommandName="Delete" CssClass="CommandButton" OnClientClick="return confirm('Are you sure you want to delete short url?');" Text="Delete"></asp:Button>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
            <HeaderStyle Font-Bold="true" />
        </asp:GridView>

    </div>
</div>

<script type="text/javascript">
    var verList = "";
    $(document).ready(function () {
        var th = $('#Content_ContentPane_ctl01_gdPages tbody tr:first');
        $('#Content_ContentPane_ctl01_gdPages').prepend($('<thead>').append(th));

        var th = $('#Content_ContentPane_ctl01_gdDynamicPages tbody tr:first');
        $('#Content_ContentPane_ctl01_gdDynamicPages').prepend($('<thead>').append(th));

        $('#Content_ContentPane_ctl01_gdPages').DataTable({
            "paging": false, "order": [[1, "desc"]],
            columnDefs: [
                { targets: 'no-sort', orderable: false }
            ]
        });

       

        $('#Content_ContentPane_ctl01_gdDynamicPages').DataTable({
            "paging": false, "order": [[1, "desc"]],
            columnDefs: [
                { targets: 'no-sort', orderable: false }
            ] });

    });

</script>
