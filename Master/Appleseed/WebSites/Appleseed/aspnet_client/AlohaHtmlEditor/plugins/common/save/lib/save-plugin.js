/* characterpicker-plugin.js is part of Aloha Editor project http://aloha-editor.org
 *
 * Aloha Editor is a WYSIWYG HTML5 inline editing library and editor.
 * Copyright (c) 2010-2013 Gentics Software GmbH, Vienna, Austria.
 * Contributors http://aloha-editor.org/contribution.php
 *
 * Aloha Editor is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or any later version.
 *
 * Aloha Editor is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301, USA.
 *
 * As an additional permission to the GNU GPL version 2, you may distribute
 * non-source (e.g., minimized or compacted) forms of the Aloha-Editor
 * source code without the copy of the GNU GPL normally required,
 * provided you include this license notice and a URL through which
 * recipients can access the Corresponding Source.
 */

define([
	'aloha',
	'jquery',
	'aloha/plugin',
	'ui/ui',
	'ui/button',
	'ui/floating',
	'PubSub',
	'i18n!save/nls/i18n'
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
    return Plugin.create('save', {
        init: function () {
            var that = this;

            this._insertEquation = Ui.adopt("save", Button, {
                tooltip: i18n.t('button.save.tooltip'),
                icon: 'aloha-multisplit aloha-icon-save',
                click: function () {
                            var moduleID = $(".aloha-editable-active").attr("moduleid");
                            var result = htmlEncode($(".aloha-editable-active").html(), "1", 4);
                            var dataParams = '{"moduleid":"' + moduleID + '","data":"' + result + '"}';
                            $.ajax({
                                type: "POST",
                                url: "/DesktopModules/CoreModules/HTMLDocument/AlohaHtmlEditorService.asmx/UpdateHtmlData",
                                data: dataParams,
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (response) {
                                },
                                failure: function (response) {
                                    alert('Unkown error occured');
                                }
                            });
                }
            });
        },
        insertEQ: function () {
            var self = this;

            if (Aloha.activeEditable) {
                window.location = OpenLatexEditor('target', 'html', '');
            }
        }
    });

});
