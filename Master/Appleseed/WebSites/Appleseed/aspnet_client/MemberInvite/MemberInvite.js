
$(document).ready(function () {
    var index = 2;

    $(".addNew").click(function () {
        if ($('.inviteRow').length >= 10) {
            alert("You can invite max 10 members at a time.");
        } else {
            $("#customFields").append('<tr class="inviteRow" valign="top"><th scope="row"><label for="customFieldName">Name &nbsp;</label></th><td><input type="text" class="FullName code" id="txtFullName_' + index + '" name="customFieldName[]" value="" placeholder="Name" /> &nbsp; Email <input type="text" class="code" id="txtEmail_' + index + '" name="customFieldValue[]" value="" placeholder="Email" /> &nbsp; <a href="javascript:void(0);" class="remCF CommandButton">Remove</a></td></tr>');
            index++;
        }
    });
    $("#customFields").on('click', '.remCF', function () {
        $(this).parent().parent().remove();
    });

    $("#addMember").on("click", function (e) {
        $("#customFields").removeClass('hide');
        $("#sendInvites").css('display', 'block');
        $("#cancel").css('display', 'block');

        $("#divmemberInvites").dialog({
            title: 'Invite Members',
            autoOpen: false,
            resizable: false,
            height: 555,
            width: 750,
            show: { effect: 'drop', direction: "up" },
            modal: true,
            draggable: true,
            close: function (event, ui) {
                $(this).dialog('close');
            }
        });

        $("#divmemberInvites").dialog('open');
        return false;
    });

    $('#sendInvites').on('click', function () {
        $.blockUI();
        var error = false;
        var arr = []
        var textboxes = $('input.code[type="text"]');

        $.each(textboxes, function (index, item) {
            if (item.value == "") {
                error = true;
                alert("Please input valid values");
                $.unblockUI();
                return false;

            }
        });

        $("input[id*='txtEmail']").each(function (i, el) {
            if (!validateEmail(el.value)) {
                error = true;
                alert("Please input valid email address");
                $.unblockUI();
                return false;

            }
        });

        if (!error) {
            var data = $('input.code[type="text"]');

            $('.inviteRow').each(function (ind, itm) {
                var inputs = $(itm).find('td input');
                var data = inputs[0].value + '#' + inputs[1].value
                arr.push(data)
            });

            console.log("array", arr);

            $.ajax({
                url: "/MemberInvite/SendEmail",
                type: "POST",
                data: JSON.stringify(arr),
                dataType: 'json',
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $.unblockUI();
                    console.log(data.Message);
                    if (data.ok == true) {
                        if (confirm(data.Message)) {
                            window.location = window.location;
                        } else {
                            return false;
                        }
                    }

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log("error", errorThrown);
                }
            });
        }

    })
    $('#cancel').on('click', function () {
        $("#divmemberInvites").dialog('close');
    })
});


function validateEmail($email) {
    var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
    return emailReg.test($email);
}

