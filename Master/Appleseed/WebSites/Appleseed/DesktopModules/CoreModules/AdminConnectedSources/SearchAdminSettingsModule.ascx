<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchAdminSettingsModule.ascx.cs" Inherits="Appleseed.DesktopModules.CoreModules.AdminConnectedSources.SearchAdminSettingsModule" %>
<h4>Engine Types:<br/>
    <asp:DropDownList ID="ddlTypes" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTypes_SelectedIndexChanged"></asp:DropDownList></h4>
<asp:PlaceHolder ID="plcMain" runat="server" Visible="false">
    <div>
        Engines:<br/>
        <asp:DropDownList ID="ddlEngines" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlEngines_SelectedIndexChanged"></asp:DropDownList>
        <asp:LinkButton ID="btnEngineDelete" runat="server" Text="Remove" OnClientClick="return confirm('Are you sure want to delete this engine?');" CssClass="CommandButton" OnClick="btnEngineDelete_Click" />
        <asp:PlaceHolder ID="plcEngineAdd" runat="server" Visible="false">
            <p>
                Engine Name*:<br />
                <asp:TextBox ID="txtEngineName" runat="server" ValidationGroup="AddNewEngine"></asp:TextBox>
                <asp:RequiredFieldValidator ValidationGroup="AddNewEngine" ID="reqEngineName" ErrorMessage="Please enter engine name" ControlToValidate="txtEngineName" Display="Dynamic" runat="server"></asp:RequiredFieldValidator><asp:CustomValidator ID="cvUniuqeName" runat="server" ValidationGroup="AddNewEngine" ControlToValidate="txtEngineName" ErrorMessage="Engine name is already exists" OnServerValidate="cvUniuqeName_ServerValidate">
                </asp:CustomValidator>
            </p>
            <p>
                <asp:LinkButton ID="btnAddEngine" runat="server" Text="Save" ValidationGroup="AddNewEngine" OnClick="btnAddEngine_Click"  CssClass="CommandButton" />
            </p>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="plcEngineItems" runat="server" Visible="false">
            <h4>Engine Items</h4>
            <p><asp:LinkButton ID="btnAddnewEngineItem" CssClass="CommandButton" runat="server" Text="Add New Engine Item" OnClick="btnAddnewEngineItem_Click" /></p>
            <asp:GridView ID="gvEngineItems" runat="server" AutoGenerateColumns="false" GridLines="Both" Width="100%">
                <Columns>
                    <asp:BoundField HeaderText="Id" DataField="id" Visible="false" />
                    <asp:BoundField HeaderText="Name" DataField="name" />
                    <asp:BoundField HeaderText="Location Url" DataField="locationurl" />
                    <asp:BoundField HeaderText="Type" DataField="type" />
                    <asp:BoundField HeaderText="Collection Name" DataField="collectionname" />
                    <asp:BoundField HeaderText="Index Path" DataField="indexpath" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEdit" runat="server" OnClick="lnkEdit_Click" CommandArgument='<%# Bind("id") %>'>Edit</asp:LinkButton> &nbsp;|&nbsp;
                            <asp:LinkButton ID="lnkDelete" runat="server" OnClick="lnkDelete_Click" OnClientClick="return confirm('Are you sure want to remove this item?');" CommandArgument='<%# Bind("id") %>'>Remove</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

        </asp:PlaceHolder>
        <asp:PlaceHolder ID="plcAddEditEngineItem" runat="server" Visible="false">
            <h4>Engine Items > Add/Edit Item</h4><asp:HiddenField ID="hdnEngineItemId" runat="server" />
            <p>
                Engine Item Name*:<br />
                <asp:TextBox ID="txtEngineItemName" runat="server" ValidationGroup="AddNewItemEngine"></asp:TextBox>
                <asp:RequiredFieldValidator ValidationGroup="AddNewEngineItem" ID="RequiredFieldValidator1" ErrorMessage="Please enter engine item name" ControlToValidate="txtEngineItemName" Display="Dynamic" runat="server"></asp:RequiredFieldValidator><asp:CustomValidator ID="CustomValidator1" runat="server" ValidationGroup="AddNewEngineItem" ControlToValidate="txtEngineItemName" ErrorMessage="Engine item name is already exists" OnServerValidate="CustomValidator1_ServerValidate">
                </asp:CustomValidator>
            </p>
            <p>
                Location Url:<br />
                <asp:TextBox ID="txtLocationUrl" runat="server" ValidationGroup="AddNewItemEngine"></asp:TextBox>
            </p>
            <p>
                Type:<br />
                <asp:TextBox ID="txtType" runat="server" ValidationGroup="AddNewItemEngine"></asp:TextBox>
            </p>
            <p>
                Collection Name:<br />
                <asp:TextBox ID="txtCollectioName" runat="server" ValidationGroup="AddNewItemEngine"></asp:TextBox>
            </p>
            <p>
                Index Path:<br />
                <asp:TextBox ID="txtIndexPath" runat="server" ValidationGroup="AddNewItemEngine"></asp:TextBox>
            </p>
            <p>
                <asp:LinkButton ID="btnSaveEngineItem"  CssClass="CommandButton" runat="server" Text="Save" ValidationGroup="AddNewEngineItem" OnClick="btnSaveEngineItem_Click" />&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="btnCancel"  CssClass="CommandButton" runat="server" Text="Cancel" OnClick="btnCancel_Click" OnClientClick="return confirm('Are you sure want to cancel this Add/Edit?');" />
            </p>
        </asp:PlaceHolder>
    </div>
</asp:PlaceHolder>
