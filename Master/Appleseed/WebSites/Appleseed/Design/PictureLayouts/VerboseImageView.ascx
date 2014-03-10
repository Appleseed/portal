<%@ Control Language="c#" AutoEventWireup="false" Inherits="Appleseed.Framework.Design.PictureItem" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table width="100%" cellspacing="0" cellpadding="0" border="0" runat="server" ID="Table1">
	<tr>
		<td align="center" colspan="2">
			<%if (GetMetadata("PreviousItemID") != null) {%>
			<a href='<%# Appleseed.Framework.HttpUrlBuilder.BuildUrl("~/DesktopModules/CommunityModules/Pictures/PictureView.aspx","ItemID=" + GetMetadata("PreviousItemID")+ "&mid=" + GetMetadata("ModuleID") + "&wversion=" + GetMetadata("WVersion"))%>' runat="server" ID="A1">
				<img runat='server' border='0' src='<%# Appleseed.Framework.HttpUrlBuilder.BuildUrl("~/Design/PictureLayouts/previous.gif")%>' ID="Img1">	
			</a>
			<%}%>
			<%if (GetMetadata("NextItemID") != null) {%>
			<a href='<%# Appleseed.Framework.HttpUrlBuilder.BuildUrl("~/DesktopModules/CommunityModules/Pictures/PictureView.aspx","ItemID=" + GetMetadata("NextItemID")+ "&mid=" + GetMetadata("ModuleID") + "&wversion=" + GetMetadata("WVersion"))%>' runat="server" ID="A2">
				<img runat='server' border='0' src='<%# Appleseed.Framework.HttpUrlBuilder.BuildUrl("~/Design/PictureLayouts/next.gif")%>' ID="Img2">
			</a>
			<%}%>		
		</td>
	</tr>
	<tr>
		<td align="center" valign="top">
			<img width="<%#GetMetadata("ModifiedWidth")%>" height="<%#GetMetadata("ModifiedHeight")%>" src="<%#GetMetadata("AlbumPath") + "/" + GetMetadata("ModifiedFilename")%>" alt="<%#GetMetadata("LongDescription")%>" border="1">
		</td>
		<td>
			<table cellpadding="0" cellspacing="0" border="0" runat="server">
				<tr>
					<td class="NormalBold">Original Filename:</td>
					<td class="Normal"><%#GetMetadata("OriginalFilename")%></td>
				</tr>
				<tr>
					<td class="NormalBold">Original Width:</td>
					<td class="Normal"><%#GetMetadata("OriginalWidth")%></td>
				</tr>
				<tr>
					<td class="NormalBold">Original Height:</td>
					<td class="Normal"><%#GetMetadata("OriginalHeight")%></td>
				</tr>
				<tr>
					<td class="NormalBold">Thumbnail Width:</td>
					<td class="Normal"><%#GetMetadata("ThumbnailWidth")%></td>
				</tr>
				<tr>
					<td class="NormalBold">Thumbnail Height:</td>
					<td class="Normal"><%#GetMetadata("ThumbnailHeight")%></td>
				</tr>
				<tr>
					<td class="NormalBold">Modified Width:</td>
					<td class="Normal"><%#GetMetadata("ModifiedWidth")%></td>
				</tr>
				<tr>
					<td class="NormalBold">Modified Height:</td>
					<td class="Normal"><%#GetMetadata("ModifiedHeight")%></td>
				</tr>
				<tr>
					<td class="NormalBold">Modified File Size:</td>
					<td class="Normal"><%#GetMetadata("ModifiedFileSize")%></td>
				</tr>
				<tr>
					<td class="NormalBold">Short Description:</td>
					<td class="Normal"><%#GetMetadata("ShortDescription")%></td>
				</tr>
				<tr>
					<td class="NormalBold">Long Description:</td>
					<td class="Normal"><%#GetMetadata("LongDescription")%></td>
				</tr>
				<tr>
					<td class="NormalBold">Caption:</td>
					<td class="Normal"><%#GetMetadata("Caption")%></td>
				</tr>
				<tr>
					<td class="NormalBold">Keywords:</td>
					<td class="Normal"><%#GetMetadata("Keywords")%></td>
				</tr>
				<tr>
					<td class="NormalBold">Upload Date:</td>
					<td class="Normal"><%#GetMetadata("UploadDate")%></td>
				</tr>
				<tr>
					<td class="NormalBold">Created By:</td>
					<td class="Normal"><%#GetMetadata("CreatedBy")%></td>
				</tr>
				<tr>
					<td class="NormalBold">Display Order:</td>
					<td class="Normal"><%#GetMetadata("DisplayOrder")%></td>
				</tr>
				<tr>
					<td class="NormalBold">Description:</td>
					<td class="Normal"><%#GetMetadata("ImageDescription")%></td>
				</tr>
				<tr>
					<td class="NormalBold">Equipment Make:</td>
					<td class="Normal"><%#GetMetadata("EquipMake")%></td>
				</tr>
				<tr>
					<td class="NormalBold">Equipment Model:</td>
					<td class="Normal"><%#GetMetadata("EquipModel")%></td>
				</tr>
				<tr>
					<td class="NormalBold">Datetime:</td>
					<td class="Normal"><%#GetMetadata("Datetime")%></td>
				</tr>
				<tr>
					<td class="NormalBold">Exposure:</td>
					<td class="Normal"><%#GetMetadata("ExifExposureTime")%></td>
				</tr>
				<tr>
					<td class="NormalBold">F Number:</td>
					<td class="Normal"><%#GetMetadata("ExifFNumber")%></td>
				</tr>
				<tr>
					<td class="NormalBold">Exposure Prog:</td>
					<td class="Normal"><%#GetMetadata("ExifExposureProg")%></td>
				</tr>
				<tr>
					<td class="NormalBold">ISO Speed:</td>
					<td class="Normal"><%#GetMetadata("ExifISOSpeed")%></td>
				</tr>
				<tr>
					<td class="NormalBold">Comp BPP:</td>
					<td class="Normal"><%#GetMetadata("ExifCompBPP")%></td>
				</tr>
				<tr>
					<td class="NormalBold">Exposure Bias:</td>
					<td class="Normal"><%#GetMetadata("ExifExposureBias")%></td>
				</tr>
				<tr>
					<td class="NormalBold">Max Aperture:</td>
					<td class="Normal"><%#GetMetadata("ExifMaxAperture")%></td>
				</tr>
				<tr>
					<td class="NormalBold">Light Source:</td>
					<td class="Normal"><%#GetMetadata("ExifLightSource")%></td>
				</tr>
				<tr>
					<td class="NormalBold">Flash:</td>
					<td class="Normal"><%#GetMetadata("ExifFlash")%></td>
				</tr>
				<tr>
					<td class="NormalBold">Focal Length:</td>
					<td class="Normal"><%#GetMetadata("ExifFocalLength")%></td>
				</tr>
			</table>
		</td>
	</tr>
	<tr>
		<td align="center" colspan="2">
			<span class="Normal">
				<%#GetMetadata("LongDescription")%>
			</span>
		</td>
	</tr>
</table>
