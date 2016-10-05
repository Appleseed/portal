CKEDITOR.plugins.add('versionbtn', {
    icons: 'versionbtn',
    init: function( editor ) {
        editor.addCommand( 'versioncontent', {

        	exec : function(editor){

                //get the text from ckeditor you want to save
        		var data = editor.getData();
                
                //get the current url
	            var page = document.URL;

                //css style for setting the standard save icon. We need this when the request is completed.
                normal_icon=$('.cke_button__versionbtn_icon').css('background-image');

                var moduleID = editor.name.split('_')[1];
                var pageID = $('#' + editor.name).attr("pageid");

                if (moduleID.indexOf('ct') > -1) {
                    moduleID = GetParameterValues("mID");
                    pageID = GetParameterValues("pageId");
                    window.location.href = window.location.href
                    //window.location = '/DesktopModules/CoreModules/HTMLDocument/HtmlEdit.aspx?pageId=' + pageID + '&mID=' + moduleID;

                } else {
                    openInModal('/DesktopModules/CoreModules/HTMLDocument/HtmlEdit.aspx?pageId=' + pageID + '&mID=' + moduleID, 'HTML Editor')
                }
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

        editor.ui.addButton( 'versionbtn', {
            label: 'Version',
            command: 'versioncontent',
            //toolbar: 'insert'
        });


    }
});
