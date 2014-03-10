<%@ Page language="c#" Codebehind="ChrisTest.aspx.cs" AutoEventWireup="false" Inherits="MenuTest.ChrisTest" %>
<%@ Register TagPrefix="cc1" Namespace="DUEMETRI.UI.WebControls.HWMenu" Assembly="DUEMETRI.UI.WebControls.HWMenu" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ChrisTest</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body bottommargin="0" topmargin="0" leftmargin="0" rightmargin="0">
		<form id="Form1" method="post" runat="server">
			<table border="0" cellpadding="0" cellspacing="0" bgcolor="#ff00ff" style="HEIGHT:100%">
				<tr>
					<td valign="top">
						<cc1:Menu id="PortalMenu" runat="server" forecolor="#ffffff" Font-Bold="True" StartTop="0"
							border="0" height="18" CssClass="Menu" Bind="BindOptionTop" AutoBind="True" Font-Size="10pt"
							Font-Names="Verdana" Width="120px" Horizontal="false" backcolor="#009900" bordercolor="#009900"
							borderstyle="none" ClientScriptPath="aspnet_client/">
							<ControlItemStyle ForeColor="Black" BorderColor="Black" BackColor="White"></ControlItemStyle>
							<ArrowImageDown Width="10px" Height="5px" ImageUrl="arrow_down.gif"></ArrowImageDown>
							<Childs>
								<cc1:MenuTreeNode Height="20px" Text="Untitled menu" Width="100px" ID="menuTreeNode1">
									<Childs>
										<cc1:MenuTreeNode Height="20px" Text="Untitled menu" Width="100px" ID="menuTreeNode11"></cc1:MenuTreeNode>
										<cc1:MenuTreeNode Height="20px" Text="Untitled menu" Width="100px" ID="menuTreeNode12"></cc1:MenuTreeNode>
										<cc1:MenuTreeNode Height="20px" Text="Untitled menu" Width="100px" ID="menuTreeNode13"></cc1:MenuTreeNode>
									</Childs>
								</cc1:MenuTreeNode>
								<cc1:MenuTreeNode Height="20px" Text="Untitled menu" Width="100px" ID="menuTreeNode2"></cc1:MenuTreeNode>
								<cc1:MenuTreeNode Height="20px" Text="Untitled menu" Width="100px" ID="menuTreeNode3"></cc1:MenuTreeNode>
								<cc1:MenuTreeNode Height="20px" Text="Untitled menu" Width="100px" ID="menuTreeNode4"></cc1:MenuTreeNode>
								<cc1:MenuTreeNode Height="20px" Text="Untitled menu" Width="100px" ID="menuTreeNode5"></cc1:MenuTreeNode>
							</Childs>
							<ControlSubStyle BorderStyle="None" ForeColor="White" BorderColor="#009900" BackColor="#009900"></ControlSubStyle>
							<ControlHiStyle BorderStyle="None" ForeColor="White" BackColor="#DA0B0B"></ControlHiStyle>
							<ControlHiSubStyle BorderStyle="None" ForeColor="White" BackColor="#DA0B0B"></ControlHiSubStyle>
							<ArrowImage Width="5px" Height="10px" ImageUrl="arrow.gif"></ArrowImage>
							<ArrowImageLeft Width="5px" Height="10px" ImageUrl="arrow_left.gif"></ArrowImageLeft>
						</cc1:Menu>
					</td>
					<td width="200">&nbsp;</td>
					<td width="200">&nbsp;</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
