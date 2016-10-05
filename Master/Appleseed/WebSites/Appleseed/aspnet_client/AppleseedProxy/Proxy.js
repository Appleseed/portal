$(document).ready(function () {

    $("#AddNewProxy").on("click", function (e) {
        var url = $(this).attr('href');
        $("#dialog-edit").dialog({
            title: 'New Proxy',
            autoOpen: false,
            resizable: false,
            height: 555,
            width: 550,
            show: { effect: 'drop', direction: "up" },
            modal: true,
            draggable: true,
            open: function (event, ui) {
                $(this).load("/ASProxy" + url + "/0/");

            },
            close: function (event, ui) {
                $(this).dialog('close');
            }
        });

        $("#dialog-edit").dialog('open');
        return false;
    });

    $(".editProxy").on("click", function (e) {
        var url = $(this).attr('href');
        $("#dialog-edit").dialog({
            title: 'Proxy Settings - Edit',
            autoOpen: false,
            resizable: false,
            height: 555,
            width: 550,
            show: { effect: 'drop', direction: "up" },
            modal: true,
            draggable: true,
            open: function (event, ui) {
                $(this).load(url + "/");

            },
            close: function (event, ui) {
                $(this).dialog('close');
            }
        });

        $("#dialog-edit").dialog('open');
        return false;
    });

    $(".deleteProxy").on("click", function (e) {
        if (!confirm("Do you want to delete")) {
            return false;
        } else {
            $.ajax({
                url: $(this).attr("href") + "/",
                type: "POST",
                dataType: 'json',
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    window.location = window.location;
                },
                error: function () {
                    console.log("Error");
                }
            });
        }
    });

    $(".chkCARole").click(function () {
        var selVals = "";
        $(".chkCARole").each(function () {
            if($(this).is(":checked"))
            {
                selVals = selVals + $(this).attr("value") + ";"
            }
        });

        $("#ContentAccessRoles").val(selVals);
    });

    $("#trContentAccessRoles").hide();
    if($("#EnabledContentAccess").is(":checked"))
    {
        $("#trContentAccessRoles").show();
    }

    $("#EnabledContentAccess").click(function () {
        $("#trContentAccessRoles").hide();
        if ($(this).is(":checked")) {
            $("#trContentAccessRoles").show();
        }
    });
});

function validateProxySettings() {
    if ($("#ServiceTitle").val().trim() == "") {
        alert("Please enter valid Service Title");
        return false;
    } else if ($("#ServiceUrl").val().trim() == "") {
        alert("Please enter valid Service URL");
        return false;
    }
    return true;
}

function closeProxySettings() {
    $("#dialog-edit").dialog('close');
}