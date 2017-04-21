CKEDITOR.plugins.add('savebtn', {
    icons: 'savebtn',
    init: function( editor ) {
        editor.addCommand( 'savecontent', {

        	exec : function(editor){

                //get the text from ckeditor you want to save
        		var data = editor.getData();
                
                //get the current url
	            var page = document.URL;

                //path to the ajaxloader gif
                loading_icon=CKEDITOR.basePath+'plugins/savebtn/icons/loader.gif';

                //css style for setting the standard save icon. We need this when the request is completed.
                normal_icon=$('.cke_button__savebtn_icon').css('background-image');

                //replace the standard save icon with the ajaxloader icon. We do this with css.
                $('.cke_button__savebtn_icon').css("background-image", "url("+loading_icon+")");

                var moduleID = editor.name.split('_')[1];
                if (moduleID.indexOf('ct') > -1) {
                    moduleID = GetParameterValues("mID");
                }
        	    var result = editor.getData();
        	    //var dataParams = '{"moduleid":"' + moduleID + '","data":"' + result + '"}';
        	    var dataParams = {moduleid : moduleID , data : result};
        	    
        	    $.ajax({
        	        type: "POST",
        	        url: "/DesktopModules/CoreModules/HTMLDocument/AlohaHtmlEditorService.asmx/UpdateHtmlData",
        	        data: JSON.stringify(dataParams),
        	        contentType: "application/json; charset=utf-8",
        	        dataType: "json",
        	        success: function (response) {
        	            console.log("response", response);
        	        },
        	        error: function (XMLHttpRequest, textStatus, errorThrown) {
        	            console.log("error", errorThrown);
        	        }
        	    })

                .always(function() {
                    console.log("complete");
                    $('.cke_button__savebtn_icon').css("background-image", normal_icon);
                });
                

        	} 
    });

        function GetParameterValues(param) {
            var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            for (var i = 0; i < url.length; i++) {
                var urlparam = url[i].split('=');
                if (urlparam[0] == param) {
                    return urlparam[1];
                }
            }
        }
//add the save button to the toolbar

        editor.ui.addButton( 'savebtn', {
            label: 'Save',
            command: 'savecontent',
           // toolbar: 'savebtn'
        });


    }
});
