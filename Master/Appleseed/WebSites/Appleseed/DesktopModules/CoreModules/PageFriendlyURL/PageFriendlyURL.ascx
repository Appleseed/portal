<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageFriendlyURL.ascx.cs" Inherits="Appleseed.DesktopModules.CoreModules.PageFriendlyURL.PageFriendlyURL" %>

<style>
    .FixTop {
        margin-top: 8px;
        width: 100%;
    }

    .buttonMargin {
        margin-top: 16px;
        width: 10%;
    }

    .success {
        color: #3c763d;
        background-color: #dff0d8;
        border-color: #d6e9c6;
    }

    .danger {
        color: #a94442;
        background-color: #f2dede;
        border-color: #ebccd1;
    }

    .updatedelete {
        table-layout: fixed;
        margin-top: 10px;
    }

        .updatedelete .field-1 {
            width: 10%;
        }

        .updatedelete .field-2 {
            width: 31%;
        }

        .updatedelete .field-3 {
            width: 30%;
        }

        .updatedelete .field-4 {
            width: 15%;
        }

        .updatedelete .field-5 {
            width: 14%;
        }

        .updatedelete td.field-2 {
            word-wrap: break-word;
        }

        .updatedelete td.field-3 {
            overflow: hidden;
            text-overflow: ellipsis;
        }

        .updatedelete tr, .updatedelete td, .updatedelete th {
            border: 1px solid #000;
            padding: 5px !important;
        }

        .updatedelete tr, .updatedelete th {
            font-weight: bold;
            padding: 5px;
        }
</style>
<div style="width: 100%">


    <div class="module-row">
        <div id="url-instructions">
            <p>To create a "friendly" url, select the specific page to which the friendly url should be applied from the Page dropdown list. </p>
            <p>Then, in the Friendly URL text box, create the friendly url.</p>
        </div>
    </div>
    <div class="module-row">
        <br />
        <h5>Add Friendly Url</h5>
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
                <h6>Friendly URL</h6>
            </div>
            <div class="col-md-6">
                <asp:TextBox ID="txtFriendlyURL" CssClass="FixTop" runat="server" Width="265" PlaceHolder="Friendly Page Url" />
                <asp:Label ID="lblFriendlyExtension" runat="server" /><br />
                <asp:RequiredFieldValidator ID="rvftxtFriendlyUrl" runat="server" Display="Dynamic" ErrorMessage="Please enter valid Url " ControlToValidate="txtFriendlyURL" ValidationGroup="Save" />
            </div>
        </div>
    </div>
    <div class="module-row">
        <div class="col-md-9">
            <div class="col-md-9">
                <asp:Button ID="btnSave" Text="Save" CssClass="buttonMargin" runat="server" OnClick="btnSave_Click" CausesValidation="true" ValidationGroup="Save" />
                <asp:Button ID="btnSaveWithoutExtension" width="180px" Text="Save Without Extension" CssClass="buttonMargin" runat="server" OnClick="btnSaveWithoutExtension_Click" CausesValidation="true" ValidationGroup="Save" />
            </div>
        </div>
    </div>

    <div class="module-row" style="clear:both;">
        <br />
        <br />
        <h5>Update/Delete Friendly Url</h5>
    </div>
    <br />
    <label>Pages</label>
    <div class="module-row">
        <asp:GridView ID="gdPages" runat="server" Width="100%" class="updatedelete" AutoGenerateColumns="false" CellSpacing="1" BorderColor="Black" OnRowEditing="GdPages_RowEditing"
            OnRowDeleting="gdPages_RowDeleting" OnRowCancelingEdit="gdPages_RowCancelingEdit" OnRowDataBound="gdPages_RowDataBound" OnRowUpdating="gdPages_RowUpdating" AlternatingRowStyle-BorderStyle="Solid" Style="border: 1px solid #000; border-collapse: collapse; border-right: 1px solid #000" EmptyDataText="No Record Found">
            <Columns>
                <asp:TemplateField HeaderStyle-CssClass="field-1" ItemStyle-CssClass="field-1" HeaderText="Page#">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblPageID" Text='<%# Eval("PageID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderStyle-CssClass="field-2" ItemStyle-CssClass="field-2" HeaderText="Destination Page Url">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblPageFullName"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderStyle-CssClass="field-3" ItemStyle-CssClass="field-3" HeaderText="Friendly Url">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblPageFriendlyUrl"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtFriendlyUrl" runat="server" Width="90%" Text='<%# Eval("FriendlyUrl") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rqfvFriendlyUrl" runat="server" ControlToValidate="txtFriendlyUrl" ValidationGroup="EditFriendlyUrl" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:CommandField ShowEditButton="true" ButtonType="Button" ValidationGroup="EditFriendlyUrl" HeaderStyle-CssClass="field-4" ItemStyle-CssClass="field-4" ControlStyle-CssClass="CommandButton" runat="server" />

                <asp:TemplateField HeaderStyle-CssClass="field-5" ItemStyle-CssClass="field-5">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="CommandButton" OnClientClick="return confirm('Are you sure you want to delete friendly url?');" Text="Delete"></asp:LinkButton>
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
                <asp:TemplateField HeaderStyle-CssClass="field-1" ItemStyle-CssClass="field-1" HeaderText="SR#">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblSrNo" Text='<%# Eval("SRINDEX") %>'></asp:Label>
                       <asp:Label runat="server" ID="lblPageID"  Visible="false" Text='<%# Eval("DynamicPageId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderStyle-CssClass="field-2" ItemStyle-CssClass="field-2" HeaderText="Destination Page Url">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblPageFullName" Text='<%# Eval("RedirectToUrl") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtRedirectToUrl" runat="server" Width="90%" Text='<%# Eval("RedirectToUrl") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rqfvRedirectToUrl" runat="server" ControlToValidate="txtRedirectToUrl" ValidationGroup="EditFriendlyUrl" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderStyle-CssClass="field-3" ItemStyle-CssClass="field-3" HeaderText="Friendly Url">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblPageFriendlyUrl"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtFriendlyUrl" runat="server" Width="90%" Text='<%# Eval("FriendlyUrl") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rqfvFriendlyUrl" runat="server" ControlToValidate="txtFriendlyUrl" ValidationGroup="EditFriendlyUrl" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:CommandField ShowEditButton="true" ButtonType="Button" ValidationGroup="EditFriendlyUrl" HeaderStyle-CssClass="field-4" ItemStyle-CssClass="field-4" ControlStyle-CssClass="CommandButton" runat="server" />

                <asp:TemplateField HeaderStyle-CssClass="field-5" ItemStyle-CssClass="field-5">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="CommandButton" OnClientClick="return confirm('Are you sure you want to delete friendly url?');" Text="Delete"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
            <HeaderStyle Font-Bold="true" />
        </asp:GridView>

    </div>
</div>

