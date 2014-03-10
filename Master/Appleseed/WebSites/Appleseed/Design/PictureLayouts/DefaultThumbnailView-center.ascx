<%@ Control Language="c#" autoeventwireup="false" Inherits="Appleseed.Framework.Design.PictureItem" targetschema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register Assembly="Appleseed.Framework.Core" Namespace="Appleseed.Framework.Web.UI.WebControls" TagPrefix="rbfwebui" %>
<%@ Register Assembly="Appleseed.Framework.Web.UI.WebControls" Namespace="Appleseed.Framework.Web.UI.WebControls" TagPrefix="rbfwebui" %>
<table height="100%" width="100%">
    <tbody>
        <tr>
            <td valign="top" width="1%">
                <a href='<%# Appleseed.Framework.HttpUrlBuilder.BuildUrl("~/DesktopModules/CommunityModules/Pictures/PictureView.aspx","ItemID=" + GetMetadata("ItemID") + "&amp;mid=" + GetMetadata("ModuleID") + "&amp;wversion=" + GetMetadata("WVersion"))%>' runat="server"><img height='<%#GetMetadata("ThumbnailHeight")%>' alt='<%#GetMetadata("Caption")%>' src='<%# GetMetadata("AlbumPath") + "/" + GetMetadata("ThumbnailFilename")%>' width='<%#GetMetadata("ThumbnailWidth")%>' border="1" /> </a></td>
            <td class="Normal" valign="top" align="center">
                <%#GetMetadata("ShortDescription")%>
                <rbfwebui:HyperLink id="editLink" runat="server" Visible='<%# GetMetadata("IsEditable") == "True"%>' NavigateUrl='<%# Appleseed.Framework.HttpUrlBuilder.BuildUrl("~/DesktopModules/Pictures/PicturesEdit.aspx","ItemID=" + GetMetadata("ItemID") + "&amp;mid=" + GetMetadata("ModuleID"))%>' ImageUrl='<%# GetCurrentImageFromTheme("Buttons_Edit", "Edit.gif") %>' />
            </td>
        </tr>
        <tr>
            <td class="Normal" valign="top" align="center">
                [ <%#GetMetadata("ModifiedWidth") + " x " + GetMetadata("ModifiedHeight")%>]<br />
                <%#GetMetadata("ModifiedFileSize") %>bytes 
            </td>
        </tr>
    </tbody>
</table>
