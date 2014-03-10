<%@ control autoeventwireup="false" inherits="Appleseed.Content.Web.Modules.Pictures"
    language="c#" %>
<rbfwebui:label id="lblError" runat="server" font-bold="True" forecolor="Red" text="Failed to load templates. Revise your settings"
    textkey="PICTURES_FAILED_TO_LOAD" visible="false"></rbfwebui:label>
<asp:datalist id="dlPictures" runat="server" itemstyle-width="1%">
</asp:datalist>
<div align="center">
    <rbfwebui:paging id="pgPictures" runat="server" />
</div>
