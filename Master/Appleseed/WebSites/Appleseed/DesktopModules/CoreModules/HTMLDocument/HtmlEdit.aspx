<%@ Page Inherits="Appleseed.DesktopModules.CoreModules.HTMLDocument.HtmlEdit"
    Language="c#" MasterPageFile="~/Shared/SiteMasterDefault.master" CodeBehind="HtmlEdit.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <style type="text/css">
        textarea {
            color: #808080;
        }
    </style>
    <asp:PlaceHolder runat="server" ID="plcCSSCKEditor" Visible="false">
        <style type="text/css">
            .normal div {
                width: 100% !important;
                /*height:100vh;*/
            }

            .Content {
                height: 485px !important;
            }

            #cke_1_contents {
                height: 347px !important;
            }

            textarea {
                line-height: 20px;
                color: #000000 !important;
            }
        </style>
    </asp:PlaceHolder>
    <asp:PlaceHolder runat="server" ID="plcCSSCMEditor" Visible="false">
        <style type="text/css">
            .normal {
                width: 1000px !important;
                /*height:100vh;*/
            }
        </style>
    </asp:PlaceHolder>
    <div class="div_ev_Table">
        <asp:PlaceHolder runat="server" ID="plcNoCodeWriter" Visible="false">

            <% if (Request.QueryString.GetValues("ModalChangeMaster") == null)
                {%>
            <table border="0" cellpadding="4" cellspacing="0" width="98%">
                <tr>
                    <td align="left" class="Head">
                        <rbfwebui:Localize ID="Literal1" runat="server" Text="HTML Editor" TextKey="HTML_EDITOR">
                        </rbfwebui:Localize>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr noshade="noshade" size="1" />
                    </td>
                </tr>
            </table>
            <% } %>
            <div id="Content" class="Content">
                <table border="0" cellpadding="4" cellspacing="0" width="98%">
                    <tr>
                        <td class="SubHead">
                            <%--<p>--%>
                            <% if (Request.QueryString.GetValues("ModalChangeMaster") == null)
                                {%>
                            <rbfwebui:Localize ID="Literal2" runat="server" Text="Desktop HTML Content" TextKey="HTML_DESKTOP_CONTENT">
                            </rbfwebui:Localize><font face="ËÎÌו">:</font>
                            <br />
                            <%} %>
                            <div class="normal">
                                <asp:PlaceHolder ID="PlaceHolderHTMLEditor" runat="server"></asp:PlaceHolder>
                            </div>
                            <%--</p>--%>
                        </td>
                    </tr>
                    <% if (Request.QueryString.GetValues("ModalChangeMaster") == null)
                        {%>
                    <tr id="MobileRow" runat="server">
                        <td class="SubHead">&nbsp;
                    <p>
                        <br />
                        <rbfwebui:Localize ID="Literal3" runat="server" Text="Mobile Summary" TextKey="HTML_MOBILE_SUMMARY">
                        </rbfwebui:Localize><font face="ËÎÌו">:</font>
                        <br />
                        <asp:TextBox ID="MobileSummary" runat="server" Columns="75" CssClass="NormalTextBox"
                            Rows="3" TextMode="multiline" Width="650"></asp:TextBox><br />
                        <rbfwebui:Localize ID="Literal4" runat="server" Text="Mobile Details" TextKey="HTML_MOBILE_DETAILS">
                        </rbfwebui:Localize>:
                        <br />
                        <asp:TextBox ID="MobileDetails" runat="server" Columns="75" CssClass="NormalTextBox"
                            Rows="5" TextMode="multiline" Width="650"></asp:TextBox>
                    </p>
                        </td>
                    </tr>
                    <% } %>
                </table>
            </div>
            <script type="text/javascript">
                $(document).ready(function () {
                    var editor1 = CKEDITOR.replace('Content_ctl01', {
                        extraAllowedContent: 'div'
                    });
                    editor1.on('instanceReady', function () {
                        // Output self-closing tags the HTML4 way, like <br>.
                        this.dataProcessor.writer.selfClosingEnd = '>';

                        // Use line breaks for block elements, tables, and lists.
                        var dtd = CKEDITOR.dtd;
                        for (var e in CKEDITOR.tools.extend({}, dtd.$nonBodyContent, dtd.$block, dtd.$listItem, dtd.$tableContent)) {
                            this.dataProcessor.writer.setRules(e, {
                                indent: true,
                                breakBeforeOpen: true,
                                breakAfterOpen: true,
                                breakBeforeClose: true,
                                breakAfterClose: true
                            });
                        }
                    });                });
            </script>
        </asp:PlaceHolder>
        <asp:PlaceHolder runat="server" ID="plcCodewriter" Visible="false">
            <style type="text/css">
                textarea {
                    /*height: auto !important;*/
                    line-height: 25px;
                }

                .CodeMirror {
                    height: 400px !important;
                }

                .divPreview {
                    height: 400px;
                    border: 1px solid gray;
                }

                .myframe {
                    width: 100%;
                    height: 390px;
                }

                .tabs .CommandButton {
                    margin: 0px;
                    border-radius: 0px;
                }

                .tabs .tbActive {
                    font-size: 18px !important;
                }
            </style>
            <script type="text/javascript">
                $(document).ready(function () {
                    $('.tabs a').click(function () {
                        $('.tabs a').each(function (itm) {
                            $(this).removeClass('tbActive');
                        });

                        $(this).addClass('tbActive');

                        $('.divTab').each(function (itm) {
                            $(this).addClass('hide');
                        });

                        $('#' + $(this).attr('tabid')).removeClass('hide');
                    });
                    $("#tbPreview").click(function () {
                        SaveData();
                    });
                    $("#tbHtml").click(function () {
                        if (!isCWBoxChanges) {
                            htmleditor.setValue(CKEDITOR.instances['Content_ctl02'].getData());
                        }
                    });
                });
            </script>
            <div class="col-lg-12">
                <h2>Codewriter</h2>
            </div>
            <div class="col-lg-12 tabs">
                <a id="tbEditor" tabid="divEditor" class="CommandButton tbActive">CKEditor</a>
                <a id="tbHtml" tabid="divHTML" class="CommandButton ">HTML</a>
                <a id="tbJavaScript" tabid="divJS" class="CommandButton">JavaScript</a>
                <a id="tbCSS" tabid="divCSS" class="CommandButton">CSS</a>
                <a id="tbCSSJSRefs" tabid="divJSCSSRef" class="CommandButton">JS/CSS References</a>
                <a id="tbPreview" tabid="divPreview" class="CommandButton">Preview</a>
            </div>
            <div class="col-lg-12 divTab " id="divEditor">
                <div style="border: 1px solid gray">
                    <asp:PlaceHolder ID="plcCWCKEditor" runat="server"></asp:PlaceHolder>
                </div>
            </div>
            <div class="col-lg-12 divTab" id="divJSCSSRef">
                <div style="border: 1px solid gray">
                    <textarea id="cwJSCSSRef" runat="server" cols="100" rows="12" style="width: 100%;"></textarea>
                </div>
            </div>
            <div class="col-lg-12 divTab" id="divCSS">
                <div style="border: 1px solid gray">
                    <textarea id="cwCSS" cols="100" rows="5" style="width: 100%;" runat="server"></textarea>
                </div>
            </div>
            <div class="col-lg-12 divTab" id="divJS">
                <div style="border: 1px solid gray">
                    <textarea id="cwJS" cols="100" rows="5" style="width: 100%;" runat="server"></textarea>
                </div>
            </div>
            <div class="col-lg-12 divTab" id="divHTML">
                <div style="border: 1px solid gray">
                    <textarea id="cwHTML" cols="100" rows="5" style="width: 100%;" runat="server"></textarea>
                </div>
            </div>


            <div class="col-lg-12 divTab" id="divPreview">
                <div class="divPreview">
                    <iframe id="ifrmPreview" class="myframe" name="myframe" src="/DesktopModules/CoreModules/HTMLDocument/preview.html"></iframe>
                </div>
            </div>


            <asp:HiddenField runat="server" ID="hdnPageId" Value="0" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ID="hdnModuleId" Value="0" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ID="hdnDefaultJSCSS" Value="" ClientIDMode="Static" />
            <link href="/aspnet_client/CodeMirrorV5.12/css/docs.css" type="text/css" rel="stylesheet" />
            <link href="/aspnet_client/CodeMirrorV5.12/css/codemirror.css" type="text/css" rel="stylesheet" />
            <script src="/aspnet_client/CodeMirrorV5.12/js/codemirror.js" type="text/javascript"></script>
            <script src="/aspnet_client/CodeMirrorV5.12/mode/xml/xml.js" type="text/javascript"></script>
            <script src="/aspnet_client/CodeMirrorV5.12/mode/javascript/javascript.js" type="text/javascript"></script>
            <script src="/aspnet_client/CodeMirrorV5.12/mode/css/css.js" type="text/javascript"></script>
            <script src="/aspnet_client/CodeMirrorV5.12/mode/htmlmixed/htmlmixed.js" type="text/javascript"></script>
            <script type="text/javascript">
                var csseditor, htmleditor, jseditor, jscssRefeditor;
                var isCWBoxChanges = false;

                function SaveData() {
                    if (!isCWBoxChanges) {
                        htmleditor.setValue(CKEDITOR.instances['Content_ctl02'].getData());
                    }
                    var dt = {};
                    dt.css = csseditor.getValue();
                    dt.html = htmleditor.getValue();
                    dt.js = jseditor.getValue();
                    dt.pageId = $('#hdnPageId').val();
                    dt.moduleId = $('#hdnModuleId').val();
                    dt.jscssref = $('#hdnDefaultJSCSS').val() + jscssRefeditor.getValue();
                    dt.cwckeditor = ''; //$('#Content_ctl02').val();
                    dt.iscwboxchanges = isCWBoxChanges;

                    $.ajax({
                        url: "htmledit.aspx/SaveData",
                        type: "POST",
                        dataType: 'json',
                        data: JSON.stringify(dt),
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            // var previewUrl = $('#ifrmPreview').attr('src');
                            $('#ifrmPreview').attr('src', '');
                            $('#ifrmPreview').attr('src', data.d);
                        },
                        error: function () {
                            console.log("Error");
                        }
                    });
                }



                $(document).ready(function () {

                    $('#actionButtonWrapper').addClass("col-lg-12");
                    csseditor = CodeMirror.fromTextArea(document.getElementById('Content_cwCSS'),
                        {
                            mode: "text/css", extraKeys:
                            {
                                "Ctrl-Space": "autocomplete"
                            },
                            value: document.getElementById('Content_cwCSS').innerHTML,
                            lineNumbers: true,
                            indentWithTabs: true
                        });

                    htmleditor = CodeMirror.fromTextArea(document.getElementById('Content_cwHTML'),
                        {
                            mode: "text/html", extraKeys:
                            {
                                "Ctrl-Space": "autocomplete"
                            },
                            value: document.getElementById('Content_cwHTML').innerHTML,
                            lineNumbers: true,
                            indentWithTabs: true
                        });

                    jseditor = CodeMirror.fromTextArea(document.getElementById('Content_cwJS'),
                        {
                            mode: "text/javascript", extraKeys:
                            {
                                "Ctrl-Space": "autocomplete"
                            },
                            value: document.getElementById('Content_cwJS').innerHTML,
                            lineNumbers: true,
                            indentWithTabs: true
                        });

                    jscssRefeditor = CodeMirror.fromTextArea(document.getElementById('Content_cwJSCSSRef'),
                        {
                            mode: "text/javascript", extraKeys:
                            {
                                "Ctrl-Space": "autocomplete"
                            },
                            value: document.getElementById('Content_cwJSCSSRef').innerHTML,
                            lineNumbers: true,
                            indentWithTabs: true
                        });

                    var refreshTime;
                    var seconds = 3000;

                    csseditor.on("change", function () {
                        isCWBoxChanges = true;
                        //clearTimeout(refreshTime);
                        //refreshTime = setTimeout(function () { SaveData(); }, 3000);
                    });

                    htmleditor.on("keyup", function () {
                        isCWBoxChanges = true;
                        htmleditorchange();
                    });

                    function htmleditorchange() {
                        isCWBoxChanges = true;
                        CKEDITOR.instances['Content_ctl02'].setData(htmleditor.getValue());
                        setTimeout(function () {
                            CKEDITOR.instances['Content_ctl02'].document.on('keyup', CKEditorChange);
                        }, 2000);
                    }

                    jseditor.on("change", function () {
                        isCWBoxChanges = true;
                        //clearTimeout(refreshTime);
                        //refreshTime = setTimeout(function () { SaveData(); }, 3000);
                    });

                    jscssRefeditor.on("change", function () {
                        isCWBoxChanges = true;
                        //clearTimeout(refreshTime);
                        //refreshTime = setTimeout(function () { SaveData(); }, 3000);
                    });
                    setTimeout(function () {
                        CKEDITOR.instances['Content_ctl02'].document.on('keyup', CKEditorChange);
                    }, 2000);

                    function CKEditorChange() {
                        isCWBoxChanges = false;
                        htmleditor.setValue(CKEDITOR.instances['Content_ctl02'].getData());
                        //clearTimeout(refreshTime);
                        //refreshTime = setTimeout(function () { SaveData(); }, 3000);
                    }

                    $('.divTab').each(function (itm) {
                        $(this).addClass('hide');
                    });

                    $('#divEditor').removeClass('hide');

                    var editor1 = CKEDITOR.replace('Content_ctl02', {
                        extraAllowedContent: 'div'
                    });
                    editor1.on('instanceReady', function () {
                        // Output self-closing tags the HTML4 way, like <br>.
                        this.dataProcessor.writer.selfClosingEnd = '>';

                        // Use line breaks for block elements, tables, and lists.
                        var dtd = CKEDITOR.dtd;
                        for (var e in CKEDITOR.tools.extend({}, dtd.$nonBodyContent, dtd.$block, dtd.$listItem, dtd.$tableContent)) {
                            this.dataProcessor.writer.setRules(e, {
                                indent: true,
                                breakBeforeOpen: true,
                                breakAfterOpen: true,
                                breakBeforeClose: true,
                                breakAfterClose: true
                            });
                        }
                    });
                });
            </script>
        </asp:PlaceHolder>
        <div id="actionButtonWrapper">
            <div id="footerpopup" class="control ui-widget-header">
                <asp:PlaceHolder ID="PlaceHolderButtons" runat="server"></asp:PlaceHolder>
                <asp:Button ID="btnCnVersion" Text="Create New Version" runat="server" OnClick="btnCreateNewVersion_Click" Visible="false" />
                <asp:DropDownList ID="drpVirsionList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpVirsionList_SelectedIndexChanged" />
                <asp:Button ID="btnPsVersion" Text="Publish Selected Version" runat="server" OnClick="btnPsVersion_Click" />
                <asp:Button ID="btnHsVersion" Text="Version History" runat="server" OnClick="btnHsVersion_Click" />
            </div>
        </div>
    </div>
</asp:Content>


