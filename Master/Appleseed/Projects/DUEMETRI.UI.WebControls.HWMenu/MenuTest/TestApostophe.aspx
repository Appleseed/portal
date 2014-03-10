<%@ Register TagPrefix="cc1" Namespace="DUEMETRI.UI.WebControls.HWMenu" Assembly="DUEMETRI.UI.WebControls.HWMenu" %>
<%@ Page language="c#" Codebehind="TestApostophe.aspx.cs" AutoEventWireup="false" Inherits="MenuTest.WebForm1" %>
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
			<cc1:Menu id="Menu1" runat="server" ClientScriptPath="aspnet_client/">
				<ControlItemStyle ForeColor="Black" BorderColor="Black" BackColor="White">
				</ControlItemStyle>

				<ArrowImageDown Width="10px" Height="5px" ImageUrl="tridown.gif">
				</ArrowImageDown>

				<Childs>
					<cc1:MenuTreeNode Height="20px" Text="Untitled' menu" Width="100px" ID="menuTreeNode1"></cc1:MenuTreeNode>
					<cc1:MenuTreeNode Height="20px" Text="Untitled 'menu" Width="100px" ID="menuTreeNode2"></cc1:MenuTreeNode>
					<cc1:MenuTreeNode Height="20px" Text='Untitled" menu' Width="100px" ID="menuTreeNode3"></cc1:MenuTreeNode>
					<cc1:MenuTreeNode Height="20px" Text='Untitled "menu' Width="100px" ID="menuTreeNode4"></cc1:MenuTreeNode>
					<cc1:MenuTreeNode Height="20px" Text="Untitled 'menu" Width="100px" ID="menuTreeNode5"></cc1:MenuTreeNode>
				</Childs>

				<ArrowImage Width="5px" Height="10px" ImageUrl="tri.gif">
				</ArrowImage>

				<ArrowImageLeft Width="5px" Height="10px" ImageUrl="trileft.gif">
				</ArrowImageLeft>
			</cc1:Menu>&nbsp;
		</form>
	</body>
</HTML>
