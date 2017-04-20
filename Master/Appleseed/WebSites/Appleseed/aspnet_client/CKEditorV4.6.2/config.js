/**
 * @license Copyright (c) 2003-2016, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function (config) {
    // Define changes to default configuration here. For example:
    // config.language = 'fr';
    // config.uiColor = '#AADC6E';
    config.allowedContent = true;
    // Referencing the new plugin
    config.extraPlugins = 'accordion,savebtn,versionbtn';
    // Define the toolbar buttons you want to have available
    config.toolbar = 'accordion';
    config.toolbar_MyToolbar =
       [
          ['accordion']
       ];
    config.filebrowserBrowseUrl = '/DesktopModules/CoreModules/FileBrowser/FileBrowser.aspx';
    config.filebrowserImageBrowseUrl = '/DesktopModules/CoreModules/FileBrowser/FileBrowser.aspx';
    config.filebrowserFlashBrowseUrl = '/DesktopModules/CoreModules/FileBrowser/FileBrowser.aspx';

    config.skin = 'moono-lisa';
};