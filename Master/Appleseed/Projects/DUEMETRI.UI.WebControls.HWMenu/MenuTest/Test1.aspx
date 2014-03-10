<%@ Register TagPrefix="cc1" Namespace="DUEMETRI.UI.WebControls.HWMenu" Assembly="DUEMETRI.UI.WebControls.HWMenu" %>
<%@ Page language="c#" Codebehind="Test1.aspx.cs" AutoEventWireup="false" Inherits="MenuTest.Test1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>WebForm1</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<cc1:Menu id="PortalMenu" runat="server" forecolor="#ffffff" Font-Bold="True" StartTop="0" border="0" height="18" CssClass="Menu" Bind="BindOptionTop" AutoBind="True" Font-Size="10pt" Font-Names="Verdana" Width="120px" Horizontal="True" backcolor="#009900" bordercolor="#009900" borderstyle="none" style="Z-INDEX: 101; LEFT: 13px; POSITION: absolute; TOP: 12px" ClientScriptPath="aspnet_client/">
				<ControlItemStyle ForeColor="Black" BorderColor="Black" BackColor="White">
				</ControlItemStyle>

				<ArrowImageDown Width="10px" Height="5px" ImageUrl="arrow_down.gif">
				</ArrowImageDown>

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

				<ControlSubStyle BorderStyle="None" ForeColor="White" BorderColor="#009900" BackColor="#009900">
				</ControlSubStyle>

				<ControlHiStyle BorderStyle="None" ForeColor="White" BackColor="#DA0B0B">
				</ControlHiStyle>

				<ControlHiSubStyle BorderStyle="None" ForeColor="White" BackColor="#DA0B0B">
				</ControlHiSubStyle>

				<ArrowImage Width="5px" Height="10px" ImageUrl="arrow.gif">
				</ArrowImage>

				<ArrowImageLeft Width="5px" Height="10px" ImageUrl="arrow_left.gif">
				</ArrowImageLeft>
			</cc1:Menu>
		</form>
	</body>
</HTML>
