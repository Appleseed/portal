<%@ Page Inherits="Appleseed.DesktopModules.CoreModules.HTMLDocument.HtmlEdit"
    Language="c#" MasterPageFile="~/Shared/SiteMasterDefault.master" CodeBehind="HtmlEdit.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <style type="text/css">
        textarea {
            color: #808080;
        }

        .normal div {
            width: 100% !important;
            /*height:100vh;*/
        }
    </style>
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

        </asp:PlaceHolder>
        <asp:PlaceHolder runat="server" ID="plcCodewriter" Visible="false">
            <style type="text/css">
                textarea {
                    height: auto !important;
                }

                .CodeMirror {
                    height: 300px !important;
                }

                .divPreview {
                    height: 300px;
                    border: 1px solid gray;
                }

                .myframe {
                    width: 100%;
                    height: 300px;
                }
            </style>
            <div class="col-lg-12">
                <h2>Code Writer</h2>
            </div>
            <div class="col-lg-3">
                <h3>CSS</h3>
                <div style="border: 1px solid gray">
                    <textarea id="cwCSS" cols="100" rows="5" style="width: 100%;" runat="server"></textarea>
                </div>
            </div>
            <div class="col-lg-3">
                <h3>HTML</h3>
                <div style="border: 1px solid gray">
                    <textarea id="cwHTML" cols="100" rows="5" style="width: 100%;" runat="server"></textarea>
                </div>
            </div>
            <div class="col-lg-3">
                <h3>JavaScript</h3>
                <div style="border: 1px solid gray">
                    <textarea id="cwJS" cols="100" rows="5" style="width: 100%;" runat="server"></textarea>
                </div>
            </div>


            <div class="col-lg-3">
                <h3>JS/CSS References</h3>
                <div style="border: 1px solid gray">
                    <textarea id="cwJSCSSRef" runat="server" cols="100" rows="5" style="width: 100%;"></textarea>
                </div>
            </div>

            <div class="col-lg-12">
                <h3>Preview</h3>
                <div class="divPreview">
                    <iframe id="ifrmPreview" class="myframe" name="myframe" src="/DesktopModules/CoreModules/HTMLDocument/preview.html"></iframe>
                </div>
            </div>


            <asp:HiddenField runat="server" ID="hdnPageId" Value="0" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ID="hdnModuleId" Value="0" ClientIDMode="Static" />
            <link href="/aspnet_client/CodeMirrorV5.12/css/docs.css" type="text/css" rel="stylesheet" />
            <link href="/aspnet_client/CodeMirrorV5.12/css/codemirror.css" type="text/css" rel="stylesheet" />
            <script src="/aspnet_client/CodeMirrorV5.12/js/codemirror.js" type="text/javascript"></script>
            <script src="/aspnet_client/CodeMirrorV5.12/mode/xml/xml.js" type="text/javascript"></script>
            <script src="/aspnet_client/CodeMirrorV5.12/mode/javascript/javascript.js" type="text/javascript"></script>
            <script src="/aspnet_client/CodeMirrorV5.12/mode/css/css.js" type="text/javascript"></script>
            <script src="/aspnet_client/CodeMirrorV5.12/mode/htmlmixed/htmlmixed.js" type="text/javascript"></script>
            <script type="text/javascript">
                $(document).ready(function () {
                    $('#actionButtonWrapper').addClass("col-lg-12");
                    var csseditor = CodeMirror.fromTextArea(document.getElementById('Content_cwCSS'),
                        {
                            mode: "text/css", extraKeys:
                                {
                                    "Ctrl-Space": "autocomplete"
                                },
                            value: document.getElementById('Content_cwCSS').innerHTML,
                            lineNumbers: true,
                            indentWithTabs: true
                        });

                    var htmleditor = CodeMirror.fromTextArea(document.getElementById('Content_cwHTML'),
                       {
                           mode: "text/html", extraKeys:
                               {
                                   "Ctrl-Space": "autocomplete"
                               },
                           value: document.getElementById('Content_cwHTML').innerHTML,
                           lineNumbers: true,
                           indentWithTabs: true
                       });

                    var jseditor = CodeMirror.fromTextArea(document.getElementById('Content_cwJS'),
                       {
                           mode: "text/javascript", extraKeys:
                               {
                                   "Ctrl-Space": "autocomplete"
                               },
                           value: document.getElementById('Content_cwJS').innerHTML,
                           lineNumbers: true,
                           indentWithTabs: true
                       });

                    var jscssRefeditor = CodeMirror.fromTextArea(document.getElementById('Content_cwJSCSSRef'),
                     {
                         mode: "text/javascript", extraKeys:
                             {
                                 "Ctrl-Space": "autocomplete"
                             },
                         value: document.getElementById('Content_cwJSCSSRef').innerHTML,
                         lineNumbers: true,
                         indentWithTabs: true
                     });

                    function SaveData() {
                        var dt = {};
                        dt.css = csseditor.getValue();
                        dt.html = htmleditor.getValue();
                        dt.js = jseditor.getValue();
                        dt.pageId = $('#hdnPageId').val();
                        dt.moduleId = $('#hdnModuleId').val();
                        dt.jscssref = jscssRefeditor.getValue();

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
                    var refreshTime;
                    var seconds = 3000;

                    csseditor.on("change", function () {
                        clearTimeout(refreshTime);
                        refreshTime = setTimeout(function () { SaveData(); }, 3000);
                    });

                    htmleditor.on("change", function () {
                        clearTimeout(refreshTime);
                        refreshTime = setTimeout(function () { SaveData(); }, 3000);
                    });

                    jseditor.on("change", function () {
                        clearTimeout(refreshTime);
                        refreshTime = setTimeout(function () { SaveData(); }, 3000);
                    });

                    jscssRefeditor.on("change", function () {
                        clearTimeout(refreshTime);
                        refreshTime = setTimeout(function () { SaveData(); }, 3000);
                    });

                    //csseditor.on("blur", function () {

                    //    SaveData();
                    //});

                    //htmleditor.on("blur", function () {
                    //    SaveData();
                    //});

                    //jseditor.on("blur", function () {
                    //    SaveData();
                    //});

                    $('#btnRefresh').click(function () {
                        SaveData();
                    });
                    SaveData();
                });
            </script>
        </asp:PlaceHolder>
        <div id="actionButtonWrapper">
            <div id="footerpopup" class="control ui-widget-header">
                <asp:PlaceHolder ID="PlaceHolderButtons" runat="server"></asp:PlaceHolder>
                <asp:Button ID="btnCnVersion" Text="Create New Version" runat="server" OnClick="btnCreateNewVersion_Click" />
                <asp:DropDownList ID="drpVirsionList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpVirsionList_SelectedIndexChanged" />
                <asp:Button ID="btnPsVersion" Text="Publish this Version" runat="server" OnClick="btnPsVersion_Click" />
                <asp:Button ID="btnHsVersion" Text="Version History" runat="server" OnClick="btnHsVersion_Click" />
            </div>
        </div>
    </div>
</asp:Content>


