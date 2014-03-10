(function(){
    CKEDITOR.plugins.add('accordion',
    {
        init: function (editor) {
            var pluginName = 'accordion';
            editor.ui.addButton('Accordion',
                {
                    label: 'Accordion',
                    command: 'Accordion',
                    icon: CKEDITOR.plugins.getPath('accordion') + 'accordion.png',
                });
            var cmd = editor.addCommand('Accordion', { exec: showMyDialog });
        }
    });
    function showMyDialog(e) {
        e.insertHtml(
            '<div class="accordion">' +
                '<div class="accordion-head">This is where the accordion header goes.</div>' +
                '<div class="accordion-body">' +
                    '<p>This is where the accordion body goes.</p>' +
                '</div>' +
            '</div><br><br>'
        ); 
    }
})();