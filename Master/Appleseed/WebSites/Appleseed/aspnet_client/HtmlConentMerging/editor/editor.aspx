<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="editor.aspx.cs" Inherits="TestCompare.editor.editor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title></title>
    <meta http-equiv="content-type" content="text/html; charset=UTF-8" />
    <meta name="description" content="Merge and Diff your documents with diff online and share" />
    <meta name="keywords" content="diff,merge,compare,jsdiff,comparison,difference,file,text,unix,patch,algorithm,saas,longest common subsequence,diff online" />
    <meta name="author" content="Jamie Peabody" />
    <link href='http://fonts.googleapis.com/css?family=Noto+Sans:400,700' rel='stylesheet' type='text/css' />
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.9.0/jquery.min.js"></script>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.10.1/jquery-ui.min.js"></script>

    <link type='text/css' rel='stylesheet' href='/aspnet_client/HtmlConentMerging/editor/lib/wicked-ui.css' />
    <script type="text/javascript" src="/aspnet_client/HtmlConentMerging/editor/lib/wicked-ui.js"></script>


    <script type="text/javascript" src="/aspnet_client/HtmlConentMerging/lib/codemirror.min.js"></script>
    <link type="text/css" rel="stylesheet" href="/aspnet_client/HtmlConentMerging/lib/codemirror.css" />
    <script type="text/javascript" src="/aspnet_client/HtmlConentMerging/lib/mergely.js"></script>
    <link type="text/css" rel="stylesheet" href="/aspnet_client/HtmlConentMerging/lib/mergely.css" />

    <script type="text/javascript">
        //Get the value from SP and set it
        $(document).ready(function () {
            $('#mergely').mergely({
                cmsettings: { readOnly: false, lineNumbers: true },
                lhs: function (setValue) {
                    setValue(document.getElementById("hdnLvn").value);
                },
                rhs: function (setValue) {
                    setValue(document.getElementById("hdnRvn").value);
                }
            });
        });
    </script>

    <script type="text/javascript" src="/aspnet_client/HtmlConentMerging/editor/editor.js"></script>

    <link type='text/css' rel='stylesheet' href='/aspnet_client/HtmlConentMerging/editor/lib/tipsy/tipsy.css' />
    <script type="text/javascript" src="/aspnet_client/HtmlConentMerging/editor/lib/tipsy/jquery.tipsy.js"></script>
    <script type="text/javascript" src="/aspnet_client/HtmlConentMerging/editor/lib/farbtastic/farbtastic.js"></script>
    <link type="text/css" rel="stylesheet" href="/aspnet_client/HtmlConentMerging/editor/lib/farbtastic/farbtastic.css" />
    <script type="text/javascript" src="/aspnet_client/HtmlConentMerging/lib/codemirror.js"></script>

    <link type='text/css' rel='stylesheet' href='/aspnet_client/HtmlConentMerging/editor/editor.css' />
    <script type="text/javascript" src="/aspnet_client/HtmlConentMerging/lib/searchcursor.js"></script>

    <script type="text/javascript">
        var key = '<?php echo $key; ?>';
        var isSample = key == '4qsmsDyb';
    </script>

    <style>
        .float-right {
            float: right !important;
            padding: 5px !important;
            margin-right: 5px !important;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <div id="banner"></div>

        <ul id="toolbar">
            <li id="tb-view-change-prev" data-icon="icon-arrow-up" title="Previous change">Previous change</li>
            <li id="tb-view-change-next" data-icon="icon-arrow-down" title="Next change">Next change</li>
            <li class="separator"></li>

            <li class="separator"></li>
            <li id="tb-edit-right-merge-left" data-icon="icon-arrow-left-v" title="Merge change left">Merge change left</li>
            <li id="tb-edit-left-merge-right" data-icon="icon-arrow-right-v" title="Merge change right">Merge change right</li>
            <li class="separator"></li>
            <asp:Button ID="btnCancel" Text="Cancel" runat="server" CssClass="float-right" OnClick="btnCancel_Click" />
            <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="float-right" OnClientClick="SaveDiff()" OnClick="btnSave_Click" />

        </ul>

        <!-- dialog colors -->
        <div id="dialog-colors" title="Mergely Color Settings" style="display: none">
            <div id="picker" style="float: right;"></div>
            <fieldset>
                <legend>Changed</legend>
                <label for="c-border">Border:</label><input type="text" id="c-border" name="c-border" class="colorwell" />
                <br />
                <label for="c-bg">Background:</label><input type="text" id="c-bg" name="c-bg" class="colorwell" />
                <br />
            </fieldset>
            <fieldset>
                <legend>Added</legend>
                <label for="a-border">Border:</label><input type="text" id="a-border" name="a-border" class="colorwell" />
                <br />
                <label for="a-bg">Background:</label><input type="text" id="a-bg" name="a-bg" class="colorwell" />
                <br />
            </fieldset>
            <fieldset>
                <legend>Deleted</legend>
                <label for="d-border">Border:</label><input type="text" id="d-border" name="d-border" class="colorwell" />
                <br />
                <label for="d-bg">Background:</label><input type="text" id="d-bg" name="d-bg" class="colorwell" />
                <br />
            </fieldset>
        </div>

        <!-- dialog confirm -->

        <div id="dialog-confirm" title="Save a Permanent Copy?" style="display: none;">
            <p>
                Are you sure you want to save? A permanent copy will be
			created at the server and a link will be provided for you to share the URL 
            in an email, blog, twitter, etc.
	
            </p>
        </div>

        <!-- find -->
        <div class="find">
            <input type="text" />
            <button class="find-prev"><span class="icon icon-arrow-up"></span></button>
            <button class="find-next"><span class="icon icon-arrow-down"></span></button>
            <button class="find-close"><span class="icon icon-x-mark"></span></button>
        </div>

        <!-- editor -->
        <asp:HiddenField ID="hdnLvn" runat="server" />
        <asp:HiddenField ID="hdnRvn" runat="server" />
        <asp:HiddenField ID="hdnLvnPublished" runat="server" />
        <asp:HiddenField ID="hdnRvnPublished" runat="server" />

        <asp:HiddenField ID="hdnVersionData" runat="server" />
        <asp:HiddenField ID="hdnPublishedData" runat="server" />
        <asp:HiddenField ID="hdnLhsText" runat="server" />
        <asp:HiddenField ID="hdnRhsText" runat="server" />

        <div style="position: absolute; top: 73px; bottom: 10px; left: 3px; right: 3px; overflow-y: hidden; padding-bottom: 2px;">
            <div id="mergely">
            </div>
        </div>
    </form>

    <script type="text/javascript">
        function handle_operation() {
            var text = $('#mergely').mergely('diff');
            alert(text);
            if (navigator.userAgent.toLowerCase().indexOf('msie') === -1) {
                if (key == '') key = ''.random(8);
                var link = jQuery('<a />', {
                    href: 'data:application/stream;base64,' + window.btoa(unescape(encodeURIComponent(text))),
                    target: '_blank',
                    text: 'clickme',
                    id: key
                });
                link.attr('download', key + '.diff');
                jQuery('body').append(link);
                var a = $('a#' + key);
                a[0].click();
                a.remove();
            }
            alert('good');
        }

        function SaveDiff() {
            var lhsText = $('#mergely').mergely('get', 'lhs'); // For left side text
            var rhsText = $('#mergely').mergely('get', 'rhs'); // For Right side text
            var modid = '<%= Request.QueryString["mID"]%>';

            var leftversion = '<%= Request.QueryString["lvn"]%>';
            var rightversion = '<%= Request.QueryString["rvn"]%>';

            var leftPublishedVersion = document.getElementById("hdnLvnPublished").value;
            var rightPublishedVersion = document.getElementById("hdnRvnPublished").value;

            var versionData = leftversion + ',' + rightversion;
            var publishedData = leftPublishedVersion + ',' + rightPublishedVersion;

            $("#hdnVersionData").val(versionData);
            $("#hdnPublishedData").val(publishedData);
            $("#hdnLhsText").val(lhsText);
            $("#hdnRhsText").val(rhsText);

        }
    </script>
</body>
</html>

