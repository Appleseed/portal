



var dnd = false;

function DnD() {
    if (!dnd) {
        $('.draggable-container').show();
        $('.draggable-container').css("border", "3px solid ForestGreen");
        $('.draggable-container').sortable({
            dropOnEmpty: true,
            items: '.ModuleWrap',
            cursor: 'crosshair',
            disabled: false,
            /*handle: '#dragHandle'*/
            tolerance: 'pointer',
            connectWith: '.draggable-container',
            forceHelperSize: true,
            over: function(event, ui) {
                ui.item.css("border", "1px solid red");
            },
            out: function(event, ui) {
                ui.item.css("border", "");
            }
        });

        $('[hidewhenempty=true]:hidden').each(function (i, item) {
            var childCount = $(this).children('.draggable-container').size();
            if (childCount > 0) {
                $(this).show();
            }
        });

    } else {
        var result = '';
        $('.draggable-container').css("border", "");
        $('.draggable-container .ModuleWrap').css("border", "");
        
        $('.draggable-container').each(function(i, elem) {
            
            result += elem.id + '@' + $(elem).sortable('toArray') + ';';
        });

        $('[hidewhenempty=true]').each(function (i, item) {
            var childCount = $(this).children('.draggable-container').children().size();
            if (childCount > 0) {
                $(this).show();
            } else {
                $(this).hide();
            }

        });
        $('.draggable-container').sortable({ disabled: true });

         Appleseed.DesktopModules.CoreModules.Admin.PortalService.Reorder(result, CallbackMethod);
    }


    dnd = !dnd;
}

function CallbackMethod(m) {
    /*alert(m);*/
}
        
    