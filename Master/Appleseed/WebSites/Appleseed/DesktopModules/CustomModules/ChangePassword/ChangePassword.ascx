<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.ascx.cs" Inherits="Appleseed.ChangePassword.DesktopModules.CoreModules.ChangePassword.ChangePassword" %>

<style>       
    .labelwrappers , textarea  
    {
        padding:10px 5px; 
        width:300px;
    }
    .buttonWrappers
    {
        padding: 15px 20px;
        width: 210px;
    }
    input 
    {
       float:right;
    }
</style>

<asp:PlaceHolder ID="plcFirst" runat="server">
    <div>
        <div class="labelwrappers" >
            <asp:Label Text="Current Password" runat="server"></asp:Label>
            <asp:TextBox runat="server" ID="tbCurrentPW" TextMode="Password" />
        </div>
        <div class="labelwrappers" >
            <asp:Label Text="New Password" runat="server" ID="lblNewPassword" ></asp:Label>
            <asp:TextBox runat="server" ID="tbNewPassword" TextMode="Password" ></asp:TextBox>
        </div>
        <div class="labelwrappers" >
            <asp:Label Text="Repeat New Password" runat="server" ID="lblNewPassword1" ></asp:Label>
            <asp:TextBox runat="server" ID="tbNewPassword1" TextMode="Password" ></asp:TextBox>
        </div>
        <div class="buttonWrappers">
            <asp:Button ID="btnSubmit" runat="server" Text="Change Password" OnClick="validateTextBoxes" />
        </div>
    </div>
</asp:PlaceHolder>

<asp:Label ID="lblMessage" runat="server" ></asp:Label>