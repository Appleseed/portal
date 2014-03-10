/// <reference path="/Scripts/jquery-1.5-vsdoc.js" />




function scheduleUpdatePackage(packageId, schedule, source, version) {

    var actionurl = '/SelfUpdater/Updates/DelayedUpgrade';
    if (schedule == false) {
        actionurl = '/SelfUpdater/Updates/RemoveDelayedUpgrade';

        $('#schedule' + packageId).show();
        $('#schedule' + packageId).parents('tr').first().removeClass('ui-state-highlight ui-corner-all');

        $('#unschedule' + packageId).hide();

    } else {
        $('#schedule' + packageId).hide();
        $('#schedule' + packageId).parents('tr').first().addClass('ui-state-highlight ui-corner-all');

        $('#unschedule' + packageId).show();
    }

    $.ajax({
        url: actionurl,
        data: {
            packageId: packageId,
            source: source,
            version: version
        },
        dataType: 'json',
        timeout: 1200000,
        success: function (data) {

        },
        error: function (data) {
            trace(data);
            $('#upgradingDiv').dialog("close");
            alert("Communication error");
        }
    });

    return false;
}

function applyUpdates() {

    $('#upgradingDiv').dialog({
        modal: true,
        closeOnEscape: false,
        closeText: '',
        resizable: false,
        title: 'Upgrading site',
        open: function (event, ui) {
            $(this).closest('.ui-dialog').find('.ui-dialog-titlebar-close').hide();
        }

    });

    $.post('/SelfUpdater/Updates/ApplyUpdates')
    .success(function () {
        $('<li>Applying updates...</li>').appendTo('#upgradingUl');

        var xhr;
        var reloading = false;
        var fn = function () {
            if (!reloading) {
                if (xhr && xhr.readystate != 4) {
                    xhr.abort();
                }
                xhr = $.post('/SelfUpdater/Updates/Status').success(function (data) {
                    if (data.online) {
                        $('<li>Reloading site...</li>').appendTo('#upgradingUl');
                        reloading = true;
                        window.location.reload();
                    }
                });
            }
        };

        var interval = setInterval(fn, 10000);
    })
    .error(function () {
        trace(data);
        $('#upgradingDiv').dialog("close");
        alert("Communication error");
    });


    return false;
}



function InstallPackages() {

    var packages = getPackagesToInstall();
    if (packages.length == 0) {

        alert('No packages were selected');


    } else {
        $('#installingDiv').dialog("open");
        $.ajax({
            type: "POST",
            url: instalationPackages,
            data: { packages: JSON.stringify(packages) },
            success: function (data) {
                for (var k = 0; k < 3; k++) {
                    getCurrentPage();
                }
                getCurrentPage();
                console.log('Success');
            },
            error: function (data) {
                console.log(data);
                $('#installingDiv').dialog("close");
            }
        });

    }
}

function getPackagesToInstall() {

    var packages = [];

    $('.InstallChecker').each(function () {

        if (this.checked) {

            var name = $(this).siblings('.PackageName').val();
            var source = $(this).siblings('.PackageSource').val();
            var version = $(this).siblings('.PackageVersion').val();

            var pack = { Name: name, Source: source, Version: version, Install: true };
            packages.push(pack);

        }


    });
    
    $('.UpdateChecker').each(function () {

        if (this.checked) {

            var name = $(this).siblings('.PackageName').val();
            var source = $(this).siblings('.PackageSource').val();
            var version = $(this).siblings('.PackageVersion').val();

            var pack = { Name: name, Source: source, Version: version, Install: false };
            packages.push(pack);

        }
    });
    
    //$('.AppleseedUpdateisCheked').each(function () {

    //    if ($(this).val() == 'true') {
    //        var name = $(this).siblings('.PackageName').val();
    //        var source = $(this).siblings('.PackageSource').val();
    //        var version = $(this).siblings('.PackageVersion').val();

    //        var pack = { Name: name, Source: source, Version: version, Install: false };
    //        packages.push(pack);
    //    }
    //});

    return packages;


}