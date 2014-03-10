<%@ control autoeventwireup="false" inherits="Appleseed.Content.Web.Modules.MagicUrls"
    language="c#" targetschema="http://schemas.microsoft.com/intellisense/ie5" Codebehind="MagicUrls.ascx.cs" %>
<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder>
<rbfwebui:xmleditgrid id="XmlEditGrid1" runat="server" cssclass="Grid">
    <selecteditemstyle cssclass="SelItem" />
    <edititemstyle cssclass="EditItem" />
    <alternatingitemstyle cssclass="AltItem" />
    <itemstyle cssclass="Item" verticalalign="Top" />
    <headerstyle cssclass="Header" />
    <footerstyle cssclass="Footer" />
    <pagerstyle cssclass="Pager" />
</rbfwebui:xmleditgrid><br />
<asp:placeholder id="PlaceHolder2" runat="server"></asp:placeholder>
