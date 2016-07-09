<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SelfUpdater.Models.NugetPackagesModel>" %>
<%@ Import Namespace="MvcContrib" %>
<script src="<%: Url.Resource("Scripts.SelfUpdater.js") %>" type="text/javascript"></script>
<script src="<%: Url.Resource("Scripts.jquery.signalR-1.0.1.min.js") %>" type="text/javascript"></script>
<link type="text/css" rel="stylesheet" href="<%: Url.Resource("Content.SelfUpdater.css") %>" />
<script src="/signalr/hubs" type="text/javascript"></script>
<div id="InstalationDiv" style="margin-bottom: 25px;">
    <table id="available_packages">
    <% Html.RenderPartial("InstallModule", Model.Install); %>
        <tr>
            <th>
            &nbsp;
            </th>
        </tr>
    <% Html.RenderPartial("UpdateModule", Model.Updates); %>
    </table>
</div>

<%--<div style="display:inline">
    <span class="ModuleTitle">
        <span class="editTitle">Installed Packages </span>
    </span>
    <hr>
</div>--%>

<%--<div id="UpdateDiv">
    <% Html.RenderPartial("UpdateModule", Model.Updates); %>
    <%--<% Html.RenderAction("UpdateModule","Updates"); %>
</div>--%>

<div id="installingDiv" style="display: none">
    <div class="ui-state-highlight ui-corner-all"><span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span> This may take a few minutes, please wait until this dialog closes.</div>
    <br />
    <ul id="installingUl">
        <li>Starting installation...</li>
    </ul>
    <div id="TestingSignalR"></div><br/>
    <div id="PackagesProgressbar"><div class="progress-label"></div></div>
</div>

<script type="text/javascript">



    function getCurrentPage() {
        $.ajax({
            type: "GET",
            url: window.location.href,
            success: function () { }
        });
    }


    $(function () {
        var progressbar = $("#PackagesProgressbar"), progressLabel = $(".progress-label");

        $("#PackagesProgressbar").progressbar({
            value: 0,
            change: function () {
                progressLabel.text(progressbar.progressbar("value") + "%");
            },
        });

        var updaterHub = $.connection.selfUpdaterHub;

        var packagesModels = [];

        updaterHub.client.newMessage = function (message) {
            $('#TestingSignalR').append('<span>' + message + '</span><br/><br/>');
            $("#installingDiv").scrollTop($("#installingDiv")[0].scrollHeight);
        };

        updaterHub.client.newPercentaje = function (message) {
            var menos = '-';
            var space = ' ';
            // removing space
            if (message.Pct.substring(0, space.length) === space) {
                message.Pct = message.Pct.substring(1, message.Pct.length);
            }

            if (packagesModels.length == 0) {
                $('#TestingSignalR').append('<span>' + message.Msg + '</span><br/><br/>');
                var model = { msg: message.Msg, pct: message.Pct, firstNegative: "" };
                packagesModels.push(model);
            }
            else {
                var progressModel = getProgressBarModel(packagesModels, message.Msg);
                if (progressModel != null) {
                    if (message.Pct.substring(0, menos.length) === menos) {
                        var absoluteNumber = Math.abs(message.Pct);
                        if (progressModel.firstNegative == "") {
                            progressModel.firstNegative = absoluteNumber;
                        }
                        var realPercentaje = (progressModel.firstNegative - absoluteNumber) * 2 + 1 + absoluteNumber;
                        message.Pct = realPercentaje;
                    }
                    if (progressModel.msg == message.Msg && progressModel.pct != message.Pct) {
                        $("#installingDiv").scrollTop($("#installingDiv")[0].scrollHeight);
                        //console.log("NuevoPorcentaje: " + message.Msg + ' ' + message.Pct);
                        progressModel.msg = message.Msg;
                        progressModel.pct = message.Pct;
                        var number = Math.abs(message.Pct);
                        $("#PackagesProgressbar").progressbar("option", "value", number);
                        //console.log("El porcentaje :'" + progressModel.pct + "'");
                    }

                }
                else {
                    $('#TestingSignalR').append('<span>' + message.Msg + '</span><br/><br/>');
                    model = { msg: message.Msg, pct: message.Pct, firstNegative: "" };
                    packagesModels.push(model);
                }
            }
        };

        updaterHub.client.reloadPage = function (data) {
            getCurrentPage();
            setTimeout(window.location = window.location.href, 5000);
        };

        updaterHub.client.openPopUp = function (data) {
            $('#installingDiv').dialog("open");
            console.log('SingalR opening pop up');
        };

        updaterHub.client.console = function (data) {
            console.log(data);
        };

        $.connection.hub.start().done(function () {
            console.log('SignalR loaded');
        });


        $('#installingDiv').dialog({
            modal: true,
            closeOnEscape: false,
            closeText: '',
            resizable: false,
            title: 'Install package',
            width: 550,
            height: 600,
            open: function (event, ui) {
                $(this).closest('.ui-dialog').find('.ui-dialog-titlebar-close').hide();
            },
            autoOpen: false

        });

        function getProgressBarModel(array, msg) {
            for (var i = 0; i < array.length; i++) {
                if (array[i].msg == msg) {
                    return array[i];
                }
            }
            return null;
        }

    });

    function openPopUpSelfUpdater() {
        $('#installingDiv').dialog("open");
    }

</script>




<input class="buttonAppliedChanges CommandButton" type="button" value="Apply Changes" onclick="InstallPackages()" />


<script type="text/javascript">

    var instalationPackages = '<%= Url.Action("InstallPackages")%>';
    
</script>

<script>
    function checkUpdate(object) {

        var selectedTd = $(object).parent().siblings(".package_update_checks");
        var val = $(selectedTd).children(".AppleseedUpdateisCheked").val();
        if (val == "false") {
            $(selectedTd).children(".AppleseedUpdateisCheked").val("true");
            $(selectedTd).children(".unchekedSpan").hide();
            $(selectedTd).children(".chekedSpan").show();
            $(object).text('remove');

        } else {
            $(selectedTd).children(".AppleseedUpdateisCheked").val("false");
            $(selectedTd).children(".unchekedSpan").show();
            $(selectedTd).children(".chekedSpan").hide();
            $(object).text('update');
        }
    }
    
    function checkAllUpdates() {
        $('.UpdateChecker').each(function () {
            $(this).attr('checked', true);
        });
    }
</script>