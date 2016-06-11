<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EvolutilityAdvancedModuleRendererPage.aspx.cs" Inherits="Appleseed.DesktopModules.CoreModules.EvolutilityAdvanced.ModuleRenderer.EvolutilityAdvancedModuleRendererPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="/aspnet_client/EvolutilityAdvanced/dist/css/vendors.min.css" id="vendormin" runat="server" />
    <link rel="stylesheet" type="text/css" href="/aspnet_client/EvolutilityAdvanced/dist/css/evolutility.css" id="evolutility" runat="server" />

    <script type="text/javascript" src="/aspnet_client/EvolutilityAdvanced/js/jquery.min.js"></script>
    <script type="text/javascript" src="/aspnet_client/EvolutilityAdvanced/js/underscore-min.js"></script>
    <script type="text/javascript" src="/aspnet_client/EvolutilityAdvanced/js/backbone-min.js"></script>

    <script type="text/javascript" src="/aspnet_client/EvolutilityAdvanced/js/vendors.min.js"></script>
    <script type="text/javascript" src="/aspnet_client/EvolutilityAdvanced/js/evolutility.js"></script>
    <script type="text/javascript" src="/aspnet_client/EvolutilityAdvanced/js/jquery.ba-resize.js"></script>

</head>
<body>
    <form id="form1" runat="server">
        <div class="evo-content2" style="margin-right: 50px">
            <div id="evol" class="">
            </div>
        </div>
        <div style="display: none;" id="uimodel"></div>
        <script type="text/javascript">
           
            var jsmodels_data = {}

            $(document).ready(function () {
                if (window.location.href.indexOf('#<%= this.ModelID %>') == -1)
                    window.location.href = window.location.href + "#<%= this.ModelID %>";

                jsModel_ui =
          {
              id: '<%= this.ModelID %>',
              label: '<%= this.ModelLabel %>',
              entity: '<%= this.ModelEntity %>',
              entities: '<%= this.ModelEntities %>',
              leadfield: '<%= this.ModelLeadField %>',
              elements: [<%= this.ModelElements %>]
          };
                
                $.ajax({
                    type: "POST",
                    url: "/DesktopModules/CoreModules/EvolutilityAdvanced.ModuleRenderer/EvolutilityAdvancedModuleRendererService.asmx/SelectAllData",
                    data:'{"moduleid":"'+getQSValueByName('moduleid')+'"}', 
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        jsmodels_data = $.parseJSON(response.d);
                        new Evol.Shell({
                            el: $('#evol'),
                            uiModelsObj: {
                                <%= this.ModelID %> : jsModel_ui
                            }
                        }).render();
                    },
                    failure: function (response) {
                        alert('Unkown error occured');
                    }
                });
            });
			
            function getQSValueByName(name) {
                name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
                var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                    results = regex.exec(location.search);
                return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
            }

            $(document).ready(function () {
                $(window.parent.document.getElementsByClassName("frmEvolModuleRenderer")).attr("height", $(document).height());
                $(".evo-content2").resize(function(){
                    $(window.parent.document.getElementsByClassName("frmEvolModuleRenderer")).attr("height",$(document).height());
                });
            });


        </script>
    </form>
    <div id="dRes"></div>
    <div id="dRes1"></div>
</body>
</html>
