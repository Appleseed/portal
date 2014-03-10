<%@ Control Language="c#" autoeventwireup="false" Inherits="Appleseed.Framework.Design.PictureItem" targetschema="http://schemas.microsoft.com/intellisense/ie5" %>
<table bgcolor="#ffffff" cellspacing="0" cellpadding="0" width="100%" border="0" runat="server">
	<tbody>
		<tr>
			<td align="middle"><br>
				<%if (GetMetadata("PreviousItemID") != null) {%>
				<a href='<%# Appleseed.Framework.HttpUrlBuilder.BuildUrl("~/DesktopModules/CommunityModules/Pictures/PictureView.aspx","ItemID=" + GetMetadata("PreviousItemID") + "&amp;mid=" + GetMetadata("ModuleID") + "&amp;wversion=" + GetMetadata("WVersion"))%>' runat="server">
					<img src='<%# Appleseed.Framework.HttpUrlBuilder.BuildUrl("~/Design/PictureLayouts/previous.gif")%>' border="0" runat="server" >
				</a>
				<%}%>
				<%if (GetMetadata("NextItemID") != null) {%>
				<a href='<%# Appleseed.Framework.HttpUrlBuilder.BuildUrl("~/DesktopModules/CommunityModules/Pictures/PictureView.aspx","ItemID=" + GetMetadata("NextItemID") + "&amp;mid=" + GetMetadata("ModuleID") + "&amp;wversion=" + GetMetadata("WVersion"))%>' runat="server">
					<img src='<%# Appleseed.Framework.HttpUrlBuilder.BuildUrl("~/Design/PictureLayouts/next.gif")%>' border="0" runat="server" id=IMG1>
				</a>
				<%}%>
			</td>
		</tr>
		<tr>
			<td align="middle">
				<img height='<%#GetMetadata("ModifiedHeight")%>' alt='<%#GetMetadata("LongDescription")%>' src='<%#GetMetadata("AlbumPath") + "/" + GetMetadata("ModifiedFilename")%>' width='<%#GetMetadata("ModifiedWidth")%>' border="0" >
			</td>
		</tr>
		<tr>
			<td align="middle"><table width="300" border="0">
					<tr>
						<td align="middle">
							<span class="Normal">
								<%#GetMetadata("LongDescription")%>
							</span>
							<br>
							<br>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</tbody>
</table>
