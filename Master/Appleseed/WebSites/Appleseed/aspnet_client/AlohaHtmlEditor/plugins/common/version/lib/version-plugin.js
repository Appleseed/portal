
define([
	'aloha',
	'jquery',
	'aloha/plugin',
	'ui/ui',
	'ui/button',
	'ui/floating',
	'PubSub',
	'i18n!version/nls/i18n'
], function (
	Aloha,
	$,
	Plugin,
	Ui,
	Button,
	Floating,
	PubSub,
	i18n
) {
    'use strict';
    return Plugin.create('version', {

        _constructor: function () {
            this._super('version');
        },

        config: ['version'],

        init: function () {
            var that = this;
            
            this._custombutton = Ui.adopt("version", Button, {
                tooltip: i18n.t('button.version.tooltip'),
                icon: 'aloha-multisplit aloha-icon-version',
                scope: 'Aloha.continuoustext',
                click: function () {
                    $('.aloha-toolbar').hide();
                    var pageID = $(".aloha-editable-active").attr("pageid");
                    var moduleID = $(".aloha-editable-active").attr("moduleid");
                    openInModal('/DesktopModules/CoreModules/HTMLDocument/HtmlEdit.aspx?pageId=' + pageID + '&mID=' + moduleID, 'HTML Editor'); 
                }
            });
        }
    });
});
