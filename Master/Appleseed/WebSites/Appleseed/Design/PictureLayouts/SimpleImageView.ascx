<%@ control autoeventwireup="false" inherits="Appleseed.Framework.Design.PictureItem" language="c#"
    targetschema="http://schemas.microsoft.com/intellisense/ie5" %>
<table id="Table1" runat="server" align="center" border="0" cellpadding="0" cellspacing="5"
    width="90%">
    <tr>
        <td align="center">
            <img alt='<%#GetMetadata("LongDescription")%>' border="1" height='<%#GetMetadata("ModifiedHeight")%>'
                src='<%#GetMetadata("AlbumPath") + "/" + GetMetadata("ModifiedFilename")%>' width='<%#GetMetadata("ModifiedWidth")%>'>
        </td>
    </tr>
    <tr>
        <td align="center">
            <span class="Normal">
                <%#GetMetadata("LongDescription")%>
            </span>
        </td>
    </tr>
</table>
