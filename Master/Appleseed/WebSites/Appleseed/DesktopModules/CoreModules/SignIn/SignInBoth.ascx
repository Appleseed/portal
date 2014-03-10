<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SignInBoth.ascx.cs" Inherits="Appleseed.DesktopModules.CoreModules.SignIn.SignInBoth" %>
<%@ Register src="Signin.ascx" tagname="Signin" tagprefix="uc1" %>

<%@ Register src="SignInSocialNetwork.ascx" tagname="SignInSocialNetwork" tagprefix="uc2" %>

<asp:Table ID="Table1" runat="server">
    <asp:TableRow>
        <asp:TableCell>
            <uc1:Signin ID="Signin1" runat="server" />
        </asp:TableCell>
        <asp:TableCell>    
            <asp:label runat="server" Width="13px">  </asp:label>
        </asp:TableCell>
        <asp:TableCell ID="SocialNetwork">
            <asp:Table runat="server">
                <asp:TableRow>
                    <asp:TableCell>
                        
                        <asp:Label runat="server" ID="labelSocialNetwork"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <uc2:SignInSocialNetwork ID="SignInSocialNetwork1" runat="server" />    
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </asp:TableCell>
    </asp:TableRow>
</asp:Table>




