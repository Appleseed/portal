<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SliderManager.ascx.cs" Inherits="Appleseed.DesktopModules.CoreModules.SliderManager.SliderManager" %>

<asp:Panel ID="panel1" runat="server" CssClass="panelslider">
    <div id="1" class="divslider">
        <div class="row">
            <div class="row">
                <div class="col-md-9">
                    <div id="divSuccessMessage" runat="server" role="alert" visible="false">
                        <h5 style="color: green">Saved successfully</h5>
                    </div>
                    <div id="divErrorMessage" runat="server" role="alert" visible="false">
                        <h5 style="color: red">Slider name is already exist. Please enter different name.</h5>
                    </div>
                </div>
            </div>
        </div>
        <div class="clearfix"></div>
        <div id="TitleSlider" class="row" runat="server" visible="false">
            <div>
                <h3>Slider Listing</h3>
            </div>
        </div>
        <div class="row">
            <asp:GridView ID="GdSliderManager"
                runat="server"
                Width="100%" CssClass="table table-striped table-bordered table-condensed "
                AutoGenerateColumns="false"
                CellSpacing="1"
                BorderColor="Black"
                OnRowEditing="GdSliderManager_RowEditing"
                OnRowDeleting="GdSliderManager_RowDeleting"
                OnRowCancelingEdit="GdSliderManager_RowCancelingEdit"
                OnRowUpdating="GdSliderManager_RowUpdating"
                OnRowCommand="GdSliderManager_RowCommand"
                AlternatingRowStyle-BorderStyle="Solid"
                Style="border: 1px solid #000; border-collapse: collapse; border-right: 1px solid #000" EmptyDataText="No Record Found">
                <Columns>
                    <asp:TemplateField HeaderText="Slider #" Visible="false">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblSliderID" Text='<%# Eval("SliderID") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="00" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Sliders">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblSliderName" Text='<%# Eval("SliderName") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtSliders" runat="server" Width="90%" Text='<%# Eval("SliderName") %>'></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rqfvSlider" runat="server" ControlToValidate="txtSliders" ValidationGroup="EditSlider" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <ItemStyle Width="250" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Add Images">
                        <ItemTemplate>
                            <asp:Button ID="lnkImage" runat="server" CommandName="Image" CssClass="CommandButton" Text="Images" CommandArgument='<%# Eval("SliderID") %>'></asp:Button>
                        </ItemTemplate>
                        <ItemStyle Width="25" />
                    </asp:TemplateField>


                    <asp:CommandField ShowEditButton="true" ButtonType="Button" ValidationGroup="EditSlider" ControlStyle-CssClass="CommandButton" runat="server" HeaderText="Edit Slider" ItemStyle-Width="25" />

                    <asp:TemplateField HeaderText="Delete Slider">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn CommandButton btn-danger" OnClientClick="return confirm('Are you sure what to delete slider?');" Text="Delete"></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="25" />
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle Font-Bold="true" />
            </asp:GridView>

        </div>

        <div class="row buttonMargin" id="DivAddNewSLider" runat="server" visible="false">
            <div>
                <h3>Add New Slider</h3>
            </div>
            <asp:Label ID="lblSliderName" Text="Slider Name" runat="server"></asp:Label>
            <asp:TextBox ID="txtSliderName" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rqfvSliderName" runat="server" ControlToValidate="txtSliderName" ValidationGroup="SliderNameGrp" Display="Dynamic" ErrorMessage="Display name is Emplty." />
            <asp:Button ID="BtnAddNewSlider" Text="New Slider" runat="server" OnClick="BtnAddNewSlider_Click" ValidationGroup="SliderNameGrp" />
        </div>
    </div>
</asp:Panel>

<asp:Panel ID="panel2" runat="server" CssClass="panelimage">
    <div id="div2" class="divimage">
        <div id="TitleImage" class="row" runat="server" visible="false">
            <div class="col-lg-12">
                <div class="col-lg-8">
                    <h3>Slider - <asp:Label ID="lblTitleSliderName" runat="server" /> > Images </h3>
                </div>
                <div class="pull-right">
                    <asp:Button ID="BtnReturn" Text="Return To Slider Listing" CssClass="" OnClick="BtnReturn_Click" runat="server" Visible="false" />
                </div>
            </div>
        </div>

        <div class="row">
            <asp:GridView ID="GdSliderImageManager"
                runat="server"
                Width="100%" CssClass="table table-striped table-bordered table-condensed"
                AutoGenerateColumns="false"
                CellSpacing="1"
                BorderColor="Black"
                OnRowEditing="GdSliderImageManager_RowEditing"
                OnRowDeleting="GdSliderImageManager_RowDeleting"
                OnRowCancelingEdit="GdSliderImageManager_RowCancelingEdit"
                OnRowDataBound="GdSliderImageManager_RowDataBound"
                OnRowUpdating="GdSliderImageManager_RowUpdating"
                OnRowCommand="GdSliderImageManager_RowCommand"
                OnRowCreated="GdSliderImageManager_RowCreated"
                AlternatingRowStyle-BorderStyle="Solid"
                ShowHeaderWhenEmpty="true"
                Style="border: 1px solid #000; border-collapse: collapse; border-right: 1px solid #000" EmptyDataText="No Record Found">
                <Columns>
                    <asp:TemplateField HeaderText="Slider #" Visible="false">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblImageID" Text='<%# Eval("SliderImageID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Image">
                        <ItemTemplate>
                            <asp:Image ID="imgSliderImage" runat="server" Width="130" Height="100" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Image ID="imgSliderImageEdit" runat="server" Width="130" Height="100" />
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Upload/Change" ItemStyle-Width="10">
                        <ItemTemplate>
                            <asp:FileUpload ID="UploadImage" runat="server" />
                            <asp:RequiredFieldValidator ID="rqfvUploadNewImage" ControlToValidate="UploadImage" ErrorMessage="Please, Select Proper Image" runat="server" ValidationGroup="UploadImage" Display="Dynamic" />
                            <asp:Button ID="btnUplad" runat="server" CommandName="Upload" CssClass="CommandButton" Text="Change Image" CommandArgument='<%# Eval("SliderImageID") %>' ValidationGroup="UploadImage"></asp:Button>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Slider Caption">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblSliderCaption" Text='<%# Eval("SliderCaption") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtSliderCaption" runat="server" Width="90%" Text='<%# Eval("SliderCaption") %>'></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rqfvSliderCaption" runat="server" ControlToValidate="txtSliderCaption" ValidationGroup="EditSlider" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Show On Page">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkActive" runat="server" Enabled="false" Checked='<%#Convert.ToBoolean( Eval("Active")) %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:CheckBox ID="chkActiveEdit" runat="server" Checked='<%#Convert.ToBoolean( Eval("Active")) %>' />
                        </EditItemTemplate>

                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Display Order">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblDisplayOrder" Text='<%# Eval("DisplayOrder") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtDisplayOrder" runat="server" Width="90%" Text='<%# Eval("DisplayOrder") %>'  Onkeypress="return isNumber(event)"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rqfvSliderDisplayOrder" runat="server" ControlToValidate="txtDisplayOrder" ValidationGroup="EditSlider" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </EditItemTemplate>

                    </asp:TemplateField>

                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkEdit" runat="server" CommandName="Edit" CssClass="btn CommandButton btn-danger" Text="Edit"></asp:LinkButton>
                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn CommandButton btn-danger" OnClientClick="return confirm('Are you sure want to delete this image?');" Text="Delete"></asp:LinkButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton ID="LinkUpdate" runat="server" CommandName="Update" CssClass="btn CommandButton btn-danger" Text="Update"></asp:LinkButton>
                            <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" CssClass="btn CommandButton btn-danger" Text="Cancel"></asp:LinkButton>
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle Font-Bold="true" />
            </asp:GridView>
        </div>

        <div class="row buttonMargin updatedelete" id="DivAddNewImage" runat="server" visible="false">
            <div id="TitleAddNewImage" class="row">
                <div>
                    <h3>Add New Image</h3>
                </div>
                <div>
                </div>
            </div>
            <div class="row buttonMargin">
                <div class="col-lg-6">
                    <div class="col-lg-4">New Image:</div>
                    <div class="col-lg-6">
                        <asp:FileUpload ID="UploadNewImage" runat="server" CssClass="" />
                        <asp:RequiredFieldValidator ID="rqfvUploadNewImage" ControlToValidate="UploadNewImage" ErrorMessage="Please, Select Proper Image" runat="server" Display="Dynamic" ValidationGroup="DisplayOrder" />
                    </div>
                </div>
            </div>

            <div class="row buttonMargin">
                <div class="col-lg-6  ">
                    <div class="col-lg-4">Caption:</div>
                    <div class="col-lg-6">
                        <asp:TextBox ID="txtSliderCaptionNew" runat="server" CssClass=""></asp:TextBox>
                    </div>
                </div>
            </div>

            <div class="row buttonMargin">
                <div class="col-lg-6  ">
                    <div class="col-lg-4">Show On Page: </div>
                    <div class="col-lg-6">
                        <asp:CheckBox ID="chkActiveNew" runat="server" CssClass="" Checked="true" />
                    </div>
                </div>
            </div>

            <div class="row buttonMargin">
                <div class="col-lg-6  ">
                    <div class="col-lg-4">Display Order:  </div>
                    <div class="col-lg-6">
                        <asp:TextBox ID="txtDisplayOrderNew" runat="server" Onkeypress="return isNumber(event)" CssClass=""></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rafvDispalyOrder" ControlToValidate="txtDisplayOrderNew" ValidationGroup="DisplayOrder" runat="server" ErrorMessage="Please Enter Valid Display Order." Display="Dynamic" />
                    </div>
                </div>
            </div>

            <div class="row buttonMargin">
                <div class="col-lg-3">
                    <asp:Button ID="BtnAddNew" runat="server" Text="Add New" OnClick="BtnAddNew_Click" ValidationGroup="DisplayOrder" />
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        // For allowing only number in Display order
        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
    </script>
</asp:Panel>

