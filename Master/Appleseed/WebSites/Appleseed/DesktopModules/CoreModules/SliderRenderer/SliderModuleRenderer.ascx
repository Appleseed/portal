<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SliderModuleRenderer.ascx.cs" Inherits="Appleseed.DesktopModules.CoreModules.SliderRenderer.SliderModuleRenderer" %>

<div class="clearfix"></div>
<div class="col-lg-12 sliderrenderer">
        <section class="slider">
            <div class="flexslider">
                <ul class="slides">
                    <asp:Literal ID="ltrSliderLi" runat="server"></asp:Literal>
                </ul>
            </div>
        </section>
</div>
<div class="loadflexscript"></div>
<div class="clearfix"></div>

<asp:PlaceHolder ID="plcLoadScripts" runat="server" Visible="false" >
    <script src="/aspnet_client/FlexSlider/js/jquery.flexslider-min.js"></script>
    <script defer type="text/javascript">
        $(window).load(function () {
                $('.flexslider').flexslider({
                    animation: "slide",
                    
                });
        });
</script>
</asp:PlaceHolder>




