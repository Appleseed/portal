<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SliderManager.aspx.cs" MasterPageFile="~/Shared/SiteMasterDefault.master" Inherits="Appleseed.DesktopModules.CoreModules.SliderManager.SliderManagerPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <style>
        textarea {
            color: #808080;
            font-size: 14px;
            padding: 3px;
            width: 50% !important;
            height: auto !important;
            line-height: normal;
        }

        #Content_gdvSliders {
            table-layout: fixed;
            margin-top: 10px;
        }

            #Content_gdvSliders tr, #Content_gdvSliders td, #Content_gdvSliders th {
                border: 1px solid #000;
                padding: 5px !important;
                vertical-align:top;
            }
    </style>
    <h3>Sliders</h3>
    <asp:PlaceHolder ID="plcSliderList" runat="server">
        <asp:LinkButton ID="lnkAddNew" runat="server" OnClick="lnkAddNew_Click">Add New</asp:LinkButton>
        <asp:GridView runat="server" ID="gdvSliders" AutoGenerateColumns="false" EmptyDataText="No Slider Found" OnRowEditing="gdvSliders_RowEditing"  OnRowCommand="gdvSliders_RowCommand">
            <Columns>
                <asp:BoundField DataField="ClientFirstName" HeaderText="Client First Name" />
                <asp:BoundField DataField="ClientLastName" HeaderText="Client List Name" />
                <asp:BoundField DataField="ClientQuote" HeaderText="Quote" />
                <asp:BoundField DataField="BackgroudColor" HeaderText="Background Color" />
                <asp:TemplateField HeaderText="Background Image">
                    <ItemTemplate>
                        <asp:HiddenField ID="hidId" runat="server" Value='<%# Eval("Id") %>' />
                        <asp:HiddenField ID="hidModuleId" runat="server" Value='<%# Eval("ModuleId") %>' />
                        <asp:Image runat="server" ImageUrl='<%# Eval("BackgroudImageUrl") %>' ID="imgBGImage" Width="100" Height="100" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="btnEdit" Text="Edit" CssClass="CommandButton" CommandName="EDIT" CommandArgument='<%# Eval("Id") %>' runat="server" />
                        <asp:Button ID="btnDelete" Text="Delete" CssClass="CommandButton" CommandName="DELETE" OnClientClick="return confirm('Are you sure want to delete?');" CommandArgument='<%# Eval("Id") %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="plcAddEdit" runat="server" Visible="false">
        <h4>Add/Edit Slider Details</h4>
        <asp:HiddenField ID="hidAddEditId" runat="server" Value="0" />
        <div>
            Client First Name:
        </div>
        <div>
            <asp:TextBox ID="txtClientFirstName" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="reqFN" runat="server" ControlToValidate="txtClientFirstName" ErrorMessage="Required!" Display="Dynamic"></asp:RequiredFieldValidator>
        </div>
        <div>
            Client Last Name:
        </div>
        <div>
            <asp:TextBox ID="txtClientLastName" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtClientLastName" ErrorMessage="Required!" Display="Dynamic"></asp:RequiredFieldValidator>
        </div>
        <div>
            Work Position:
        </div>
        <div>
            <asp:TextBox ID="txtClientWorkPosition" runat="server"></asp:TextBox>
        </div>
        <div>
            Client Quote:
        </div>
        <div>
            <asp:TextBox ID="txtClientQuote" TextMode="MultiLine" Rows="3" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtClientQuote" ErrorMessage="Required!" Display="Dynamic"></asp:RequiredFieldValidator>
        </div>
        <div>
            Backgrond Image Url:
        </div>
        <div>
            <asp:TextBox ID="txtBGImageUrl" runat="server"></asp:TextBox>
        </div>
        <div>
            Backgrond Color:
        </div>
        <div>
            <asp:TextBox ID="txtBGColor" runat="server"></asp:TextBox>
        </div>
        <asp:Button ID="btnSave" Text="Save" CssClass="CommandButton" runat="server" OnClick="btnSave_Click" />
    </asp:PlaceHolder>
</asp:Content>
