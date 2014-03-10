<%@ Control Language="c#" autoeventwireup="false" Inherits="Appleseed.Framework.Design.PictureItem" targetschema="http://schemas.microsoft.com/intellisense/ie5" %>
<table cellspacing="0" cellpadding="0" width="100%" bgcolor="#ffffff" border="0" runat="server">
    <tbody>
        <tr>
            <td align="center">
                <br />
                <%if (GetMetadata("PreviousItemID") != null) {%><a href='<%# Appleseed.Framework.HttpUrlBuilder.BuildUrl("~/DesktopModules/CommunityModules/Pictures/PictureView.aspx","ItemID=" + GetMetadata("PreviousItemID") + "&amp;mid=" + GetMetadata("ModuleID") + "&amp;wversion=" + GetMetadata("WVersion"))%>' runat="server"><img src='<%# Appleseed.Framework.HttpUrlBuilder.BuildUrl("~/Design/PictureLayouts/previous.gif")%>' border="0" runat="server" /> </a><%}%><%if (GetMetadata("NextItemID") != null) {%><a href='<%# Appleseed.Framework.HttpUrlBuilder.BuildUrl("~/DesktopModules/CommunityModules/Pictures/PictureView.aspx","ItemID=" + GetMetadata("NextItemID") + "&amp;mid=" + GetMetadata("ModuleID") + "&amp;wversion=" + GetMetadata("WVersion"))%>' runat="server"><img src='<%# Appleseed.Framework.HttpUrlBuilder.BuildUrl("~/Design/PictureLayouts/next.gif")%>' border="0" runat="server" /> </a><%}%></td>
        </tr>
        <tr>
            <td align="center">
                <img height='<%#GetMetadata("ModifiedHeight")%>' alt='<%#GetMetadata("LongDescription")%>' src='<%#GetMetadata("AlbumPath") + "/" + GetMetadata("ModifiedFilename")%>' width='<%#GetMetadata("ModifiedWidth")%>' border="0" /> 
            </td>
        </tr>
        <tr>
            <td align="center">
                <table width="300" border="0">
                    <tbody>
                        <tr>
                            <td align="center">
                                <span class="Normal"><%#GetMetadata("LongDescription")%></span>
                                <br />
                                <br />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
    </tbody>
</table>
