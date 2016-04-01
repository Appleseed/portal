<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditFile.aspx.cs" Inherits="Appleseed.DesktopModules.CoreModules.FileBrowser.EditFile" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .btnSave {
            background-color: green;
            border-radius: 5px;
            color: white;
        }

        .CodeMirror {
            height: 429px !important;
        }

        .divFEFooter {
            border: 1px solid #CCC;
            background-color: #CCC;
            padding: 5px;
            text-align: right;
        }

        .divFEFooter span{
           display: none; color:green;float:left
        }

        .divFEEditor {
            border: 1px solid #CCC;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <link href="../../../aspnet_client/CodeMirrorV5.12/css/codemirror.css" rel="stylesheet" />
        <link href="../../../aspnet_client/CodeMirrorV5.12/css/docs.css" rel="stylesheet" />

        <asp:HiddenField runat="server" ID="hdnFileExtention" />
        <asp:HiddenField runat="server" ID="hdnSaved" Value="0" />

        <div class="divFEEditor">
            <asp:TextBox runat="server" ID="txtData" TextMode="MultiLine"></asp:TextBox>
        </div>
        <div class="divFEFooter">
            <span id="spMessage">Saved Successfully!</span>
            <asp:Button runat="server" Text="Save" CssClass="btnSave" ID="btnSave" OnClick="btnSave_Click" />
        </div>
        <script src="../../../aspnet_client/js/jquery.js"></script>
        <script src="../../../aspnet_client/CodeMirrorV5.12/js/codemirror.js"></script>
        <link href="../../../aspnet_client/CodeMirrorV5.12/css/codemirror.css" rel="stylesheet" />
        <script src="../../../aspnet_client/CodeMirrorV5.12/mode/htmlmixed/htmlmixed.js"></script>
        <script src="../../../aspnet_client/CodeMirrorV5.12/mode/javascript/javascript.js"></script>
        <script src="../../../aspnet_client/CodeMirrorV5.12/mode/xml/xml.js"></script>
        <script src="../../../aspnet_client/CodeMirrorV5.12/mode/css/css.js"></script>
        <script type="text/javascript">
            $(document).ready(function () {
                if ($('#hdnSaved').val() == "1") {
                    $('#spMessage').css('display', 'block');

                    setTimeout(function () { $('#spMessage').fadeOut('slow'); }, 4000);
                }

                var editorMode = {
                    htm: 'text/html',
                    html: 'text/html',
                    css: 'text/css',
                    js: 'text/javascript',
                    xml: 'application/xml',
                    json: 'application/json'
                };
                var selectedMode = 'text/html';
                var fileExtVal = editorMode[$('#hdnFileExtention').val()]
                if (fileExtVal != undefined && fileExtVal != '') {
                    selectedMode = fileExtVal;
                }
                editor = CodeMirror.fromTextArea(document.getElementById("txtData"), {
                    lineWrapping: true,
                    mode: selectedMode,
                    extraKeys: { "Ctrl-Space": "autocomplete" },
                    // value: document.documentElement.innerHTML
                });

                var charWidth = editor.defaultCharWidth(), basePadding = 4;
                editor.on("renderLine", function (cm, line, elt) {
                    var off = CodeMirror.countColumn(line.text, null, cm.getOption("tabSize")) * charWidth;
                    elt.style.textIndent = "-" + off + "px";
                    elt.style.paddingLeft = (basePadding + off) + "px";
                });
                editor.refresh();
            });
        </script>
    </form>
</body>
</html>
