(function(){
    CKEDITOR.plugins.add('additem',
    {
        init: function (editor) {
            var pluginName = 'additem';
            var iconPath = CKEDITOR.plugins.getPath('additem') + 'accordion.png';
            editor.ui.addButton('AddItem',
                {
                    label: 'AddItem',
                    command: 'AddItem',
                    icon: iconPath,
                });
            var cmd = editor.addCommand('AddItem', { exec: showMyDialog });
            
            if (editor.addMenuItem) {
                // A group menu is required
                // order, as second parameter, is not required
                editor.addMenuGroup('testgroup');

                // Create a manu item
                editor.addMenuItem('testitem', {
                    label: 'Add new item',
                    command: 'AddItem',
                    group: 'testgroup',
                    icon: iconPath,
                });
            }

            //if (editor.contextMenu) {
            //    editor.contextMenu.addListener(function (element, selection) {
            //        return { testitem: CKEDITOR.TRISTATE_ON };
            //    });
            //}
            editor.contextMenu.addListener(function (element, selection) {
                // Get elements parent, strong parent first
                var parents = element.getAscendant('div', true);
                // Check if it's strong
                if ((parents == undefined) || !(parents.hasClass('accordion-body')))
                    return null; // No item
                // Show item
                return { testitem: CKEDITOR.TRISTATE_ON };
            });
            
        }
    });
    function showMyDialog(e) {
        var sel = e.getSelection().getStartElement();
        if (!(sel.hasClass('accordion-body'))) {
            sel = e.getSelection().getStartElement().getParent();
        }
        var element = e.document.createElement('div');
        element.addClass('accordion-body');
        element.setText('This is where the accordion body goes.');
        element.insertAfter(sel);
        
        var elementhead = e.document.createElement('div');
        elementhead.addClass('accordion-head');
        elementhead.setText('This is where the accordion header goes.');
        this.insertMode = true;
        elementhead.insertAfter(sel);

    }
})();