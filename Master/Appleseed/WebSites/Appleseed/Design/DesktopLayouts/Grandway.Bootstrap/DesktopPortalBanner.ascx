<%@ Control Language="c#" %>
<%@ Register Assembly="Appleseed.Framework.Core" Namespace="Appleseed.Framework.Web.UI.WebControls" TagPrefix="rbfwebui" %>
<%@ Register Assembly="Appleseed.Framework.Web.UI.WebControls" Namespace="Appleseed.Framework.Web.UI.WebControls" TagPrefix="rbfwebui" %>
<script runat="server">
    private void Page_Load(object sender, System.EventArgs e)
    {
        PortalTitle.DataBind();
        PortalImage.DataBind();
        HtmlMeta keywords = new HtmlMeta();
        keywords.HttpEquiv = "viewport";
        keywords.Name = "viewport";
        keywords.Content = "width=device-width, initial-scale=1";
        this.Page.Header.Controls.Add(keywords);
    }

    StringBuilder sbrMenu = new StringBuilder();
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        this.biMenu.DataBind();
        sbrMenu.AppendLine("<ul class=\"dl-menu\">");
        foreach (MenuItem item in this.biMenu.Items)
        {
            string currentcss = "";
            if (Request.Url.AbsolutePath.ToLower().Contains(item.NavigateUrl.ToLower().Split('.').GetValue(0).ToString()))
                currentcss = "current";
            
            if (item.ChildItems.Count == 0)
            {
                sbrMenu.AppendLine("<li class=" + currentcss + " ><a href=" + item.NavigateUrl + ">" + item.Text + "</a></li>");
            }
            else
            {
                sbrMenu.AppendLine("<li class=" + currentcss + " ><a href=" + item.NavigateUrl + ">" + item.Text + "</a>");
                RenderSubLinks(item);
                sbrMenu.AppendLine("</li>");
            }
        }

        sbrMenu.AppendLine("</ul>");
        this.biMenu.Visible = false;
        ltrTopMenu.Text = sbrMenu.ToString();
    }

    private void RenderSubLinks(MenuItem parent)
    {
        sbrMenu.AppendLine("<ul class=\"dl-submenu\">");
        foreach (MenuItem item in parent.ChildItems)
        {
            if (item.ChildItems.Count == 0)
            {
                sbrMenu.AppendLine("<li><a href=" + item.NavigateUrl + ">" + item.Text + "</a></li>");
            }
            else
            {
                sbrMenu.AppendLine("<li><a href=" + item.NavigateUrl + ">" + item.Text + "</a>");
                RenderSubLinks(item);
                sbrMenu.AppendLine("</li>");
            }
        }

        sbrMenu.AppendLine("</ul>");
    }
</script>

<div class="container" style="margin-top:10px;">
    <div class="row">
        <div class="col-lg-3 pull-left">
            <a class="navbar-brand1" href="/1">
                <div class="col-lg-12">
                    <!-- Portal Logo Image Uploaded-->
                    <rbfwebui:HeaderImage ID="PortalImage" runat="server" EnableViewState="false" />
                    <!-- End Portal Logo-->
                </div>
                <div >
                    <!-- Portal Title -->
                    <rbfwebui:HeaderTitle ID="PortalTitle" runat="server" EnableViewState="false"></rbfwebui:HeaderTitle>
                    <!-- End Portal Title -->
                    <!-- Portal Title - hidden by default - 
                    change visible="false" to visible ="true" to allow it to show-->
                        <rbfwebui:headertitle id="PortalTitle" runat="server" cssclass="SiteTitle navbar-brand" visible="false" enableviewstate="false"></rbfwebui:headertitle>
                    <!-- End Portal Title -->
                </div>
            </a>
        </div>

        <div class="col-lg-9 pull-right">
            <div class="menu">
                <div id="dl-menu" class="dl-menuwrapper">
                    <button class="dl-trigger">Open Menu</button>
                    <!-- Begin Portal Menu -->
                    <asp:Menu ID="biMenu" runat="server"
                        DataSourceID="biSMDS"
                        DynamicEnableDefaultPopOutImage="False"
                        StaticEnableDefaultPopOutImage="False">
                    </asp:Menu>
                    <asp:Literal ID="ltrTopMenu" runat="server"></asp:Literal>
                    <!-- End Portal Menu -->
                </div>
            </div>
        </div>
    </div>
</div>

<asp:SiteMapDataSource ID="biSMDS" ShowStartingNode="false" runat="server" />
<script type="text/javascript">
    $(document).ready(function () {
        var topListItem = $(".item-5").parents("li").last();
    });

    //Attempt at Updating button classes for Bootstrap - not working for iframes
    //var doc = window.frames.document;
    //$(window).bind("load", function() { 
    /*$(doc).ready(function () {
        $(".CommandButton").addClass("btn btn-primary");
        $("input[type='submit']").addClass("btn btn-primary");
        console.log("test1");
    });*/

    $('.dl-submenu li a').click(function(){
        var topListItem = this.parents("li").last();
        alert(topListItem);
        window.location.href = anchorhref;
    });

</script>