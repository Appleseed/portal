<%@ Control Language="c#" AutoEventWireup="false" Inherits="Appleseed.Framework.Design.PictureItem" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register Assembly="Appleseed.Framework.Core" Namespace="Appleseed.Framework.Web.UI.WebControls" TagPrefix="rbfwebui" %>
<%@ Register Assembly="Appleseed.Framework.Web.UI.WebControls" Namespace="Appleseed.Framework.Web.UI.WebControls" TagPrefix="rbfwebui" %>
<table width="100%" height="100%">
	<tr>
		<td valign="top" width="1%">
			<a href='<%# "~/DesktopModules/CommunityModules/Pictures/PictureView.aspx?ItemID=" + GetMetadata("ItemID") + "&mid=" + GetMetadata("ModuleID") + "&wversion=" + GetMetadata("WVersion")%>' runat="server">
				<img border='0' width='<%#GetMetadata("ThumbnailWidth")%>' height='<%#GetMetadata("ThumbnailHeight")%>' alt='<%#GetMetadata("Caption")%>' src='<%# GetMetadata("AlbumPath") + "/" + GetMetadata("ThumbnailFilename")%>' />
			</a>
		</td>
	</tr>
<tr>
		<td align="left" valign="middle" class="Normal">
			<%#GetMetadata("ShortDescription")%>
			<rbfwebui:HyperLink id="editLink" ImageUrl='<%# GetCurrentImageFromTheme("Buttons_Edit", "Edit.gif") %>' NavigateUrl='<%# "~/DesktopModules/CommunityModules/Pictures/PicturesEdit.aspx?ItemID=" + GetMetadata("ItemID") + "&mid=" + GetMetadata("ModuleID")%>' Visible='<%# GetMetadata("IsEditable") == "True"%>' runat="server" />
		</td>

	</tr>
</table>
