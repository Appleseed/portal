<%@ Control AutoEventWireup="false" Inherits="Appleseed.Content.Web.Modules.SiteSettingsmod"
    Language="c#" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" CodeBehind="SiteSettings.ascx.cs" %>
<table border="0">
    <tr>
        <td class="Subhead" valign="top" width="140">
            <rbfwebui:Localize ID="site_title" runat="server" Text="Site Title" TextKey="SITESETTINGS_SITE_TITLE">
            </rbfwebui:Localize>
        </td>
        <td class="NormalTextBox" colspan="2">
            <asp:TextBox ID="siteName" runat="server" CssClass="NormalTextBox" Width="240"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="Subhead" valign="top" width="140">
            <rbfwebui:Localize ID="site_path" runat="server" Text="Site Path" TextKey="SITESETTINGS_SITE_PATH">
            </rbfwebui:Localize>
        </td>
        <td class="Normal" colspan="2">
            <%--<asp:label id="sitePath" runat="server" width="240"></asp:label>--%>
            <asp:TextBox ID="sitePath" runat="server" CssClass="NormalTextBox" Width="240" Enabled="false"></asp:TextBox>
        </td>
    </tr>
</table>
<rbfwebui:SettingsTable ID="EditTable" runat="server" />
<rbfwebui:LinkButton ID="UpdateButton" runat="server" CssClass="CommandButton" Text="Apply Changes"
    TextKey="APPLY">
</rbfwebui:LinkButton>

<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        $('#Content_ContentPane_ctl01_EditTable_SITESETTINGS_PORTALTIMEOUT').keydown(function (e) {
            var key = e.keyCode;
            if (e.shiftKey || e.ctrlKey || e.altKey) {
                e.preventDefault();
            } else if ($('#Content_ContentPane_ctl01_EditTable_SITESETTINGS_PORTALTIMEOUT').val().length >= 10 && !((key == 9) || (key == 8))) {
                e.preventDefault();
            } else {
                if (!((key == 9) || (key == 8) || (key == 46) || (key >= 35 && key <= 40) || (key >= 48 && key <= 57) || (key >= 96 && key <= 105))) {
                    e.preventDefault();
                }
            }
        });

        $('[id*=UpdateButton]').click(function () {
            if (!$.isNumeric($('#Content_ContentPane_ctl01_EditTable_SITESETTINGS_PORTALTIMEOUT').val())) {
                $('#Content_ContentPane_ctl01_EditTable_SITESETTINGS_PORTALTIMEOUT').val(20);
            }
        });

        $('#Content_ContentPane_ctl01_EditTable_SITESETTINGS_PORTALTIMEOUT').blur(function () {
            if (!$.isNumeric($('#Content_ContentPane_ctl01_EditTable_SITESETTINGS_PORTALTIMEOUT').val())) {
                $('#Content_ContentPane_ctl01_EditTable_SITESETTINGS_PORTALTIMEOUT').val(20);
            }
        });

    });
    var tpg1 = new xTabPanelGroup('tpg1', 700, 350, 50, 'tabPanel', 'tabGroup', 'tabDefault', 'tabSelected');
    var tabgroupd = xGetElementById('tpg1');
    var pareTbl = xGetElementById(tabgroupd.id);
    //pareTbl.style.posHeight= (xHeight(tabgroupd)+50);
    pareTbl.style["height"] = 380;
    pareTbl.style["overflow"] = 'hidden';
    //xResizeTo(pareTbl, xWidth(pareTbl), 380);
    //	alert(xHeight(tabgroupd));
</script>

