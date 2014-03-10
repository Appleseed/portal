
<%@ Page CodeBehind="print.aspx.cs" Language="c#" AutoEventWireup="True" Inherits="Appleseed.PrintPage" %>
<%@ Register TagPrefix="head" TagName="PrintHeader" Src="~/Design/DesktopLayouts/PrintHeader.ascx" %>
<%@ Register TagPrefix="foot" TagName="PrintFooter" Src="~/Design/DesktopLayouts/PrintFooter.ascx" %>
<html>
	<head runat="server"></head>
	<body runat="server">
		<form runat="server">
			<table class="PrintPage" width="100%" cellpadding="0" cellspacing="0">
				<tr>
					<td>
						<head:printheader id="PrintHeader" runat="server"></head:printheader>
					</td>
				</tr>
				<tr>
					<td>
						<asp:placeholder id="PrintPlaceHolder" runat="server"></asp:placeholder>
					</td>
				</tr>
				<tr>
					<td>
						<foot:printfooter id="PrintFooter" runat="server"></foot:printfooter>
					</td>
				</tr>
			</table>
		</form>
	</body>
</html>
