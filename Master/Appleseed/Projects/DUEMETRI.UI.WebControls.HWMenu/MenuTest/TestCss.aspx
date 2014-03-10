<%@ Register TagPrefix="cc1" Namespace="DUEMETRI.UI.WebControls.HWMenu" Assembly="DUEMETRI.UI.WebControls.HWMenu" %>
<%@ Page language="c#" Codebehind="TestCss.aspx.cs" AutoEventWireup="false" Inherits="MenuTest.TestCss" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>CssTest</title>
		<LINK rel="stylesheet" type="text/css" href="default.css">
	</HEAD>
	<body>

		<form id="Form1" method="post" runat="server">
			<CC1:MENU id="PortalMenu" height="20" runat="server" border="0" StartTop="3" Width="175px" Horizontal="False" ClientScriptPath="aspnet_client/">
				<ControlItemStyle CssClass="sm_HWMenuItem">
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

				<ControlSubStyle CssClass="sm_HWMenuSubItem">
				</ControlSubStyle>

				<ControlHiStyle CssClass="sm_HWMenuHiItem">
				</ControlHiStyle>

				<ControlHiSubStyle CssClass="sm_HWMenuHiSubItem">
				</ControlHiSubStyle>

				<ArrowImage Width="5px" Height="10px" ImageUrl="arrow.gif">
				</ArrowImage>

				<ArrowImageLeft Width="5px" Height="10px" ImageUrl="arrow_left.gif">
				</ArrowImageLeft>
			</CC1:MENU>
		</form>
	</body>
</HTML>
