<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminLeftMenu.ascx.cs" Inherits="Appleseed.DesktopModules.CoreModules.Admin.AdminLeftMenu" %>



<script runat="server">

    StringBuilder sbrMenu = new StringBuilder();
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        this.biMenu.DataBind();
        sbrMenu.AppendLine("<ul>");
        MenuItem menuAdmin = null;
        //item.DataPath == "100" - Administration page id
        foreach (MenuItem item in this.biMenu.Items)
        {
            if (item != null && item.DataPath == "100")
            {
                menuAdmin = item;
                break;
            }
        }
        if (menuAdmin != null)
        {
            foreach (MenuItem item in menuAdmin.ChildItems)
            {
                sbrMenu.AppendLine("<li ><a href=" + item.NavigateUrl + ">" + item.Text + "</a></li>");
            }
        }
        sbrMenu.AppendLine("</ul>");
        this.biMenu.Visible = false;
        ltrTopMenu.Text = sbrMenu.ToString();
    }

    
</script>
<div class="admin-left-menu-inner">
    <asp:Menu ID="biMenu" runat="server"
        DataSourceID="biSMDS"
        DynamicEnableDefaultPopOutImage="False"
        StaticEnableDefaultPopOutImage="False">
    </asp:Menu>

    <asp:Literal ID="ltrTopMenu" runat="server"></asp:Literal>
</div>
<asp:SiteMapDataSource ID="biSMDS" ShowStartingNode="false" runat="server" />