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
<div class="clearfix">&nbsp;</div>

<asp:Repeater ID="rptSliders" runat="server">
    <HeaderTemplate>
        <section class="overlay-dark80 dark-bg ptb ptb-sm-80" data-stellar-background-ratio="0.5" id="testimonial">
            <div class="container">
                <div class="owl-carousel testimonial-carousel nf-carousel-theme white">
    </HeaderTemplate>
    <ItemTemplate>
        <div class="item">
            <div class="testimonial text-center dark-color">
                <div class="container-icon"><i class="fa fa-quote-right"></i></div>

                <p class="lead">
                    "
                    <%# Eval("ClientQuote") %>
                    "
                </p>

                <div class="quote-author white"><%# Eval("ClientFirstName") %> <%# Eval("ClientLastName") %> <span style="font-weight: 400;">(<%# Eval("ClientWorkPosition") %>)</span></div>
                <br />
            </div>
        </div>
    </ItemTemplate>
    <FooterTemplate>
        </div>
</div>
</section>
    </FooterTemplate>
</asp:Repeater>

<asp:PlaceHolder ID="plcLoadScripts" runat="server" Visible="false">
    <script src="/aspnet_client/FlexSlider/owl.carousel.min.js"></script>
    <script type="text/javascript">
        $(function () {

            $(".testimonial-carousel").owlCarousel({
                autoPlay: !0,
                autoHeight: !0,
                stopOnHover: !0,
                singleItem: !0,
                slideSpeed: 350,
                pagination: !0,
                navigation: !1,
                navigationText: ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"]
            });
        });
    </script>

</asp:PlaceHolder>




