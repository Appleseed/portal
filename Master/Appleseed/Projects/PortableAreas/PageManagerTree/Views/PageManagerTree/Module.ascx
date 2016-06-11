<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<PageManagerTree.Models.Permissions>" %>
<%@ Import Namespace="Appleseed.Framework" %>
<%@ Import Namespace="PageManagerTree" %>
<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
     {
      
     }
</script>

<div id="jqtreePageManagement">
    <link type="text/css" rel="stylesheet" href="<%: Appleseed.Framework.HttpUrlBuilder.BuildUrl("~/aspnet_client/jQuery/jsTree/themes/proton/style.css") %>" />
    <form id="form1" runat="server">
            <div class="toolbar">
                <table>
                    <tr>
                        <td>
                            <table class="innertable">
                                <asp:PlaceHolder runat="server" ID="plcAdd">
                                <tr>
                                    <td>
                                        <img class="actionicon" src="/aspnet_client/jQuery/jsTree/add.png" title="<%: Appleseed.Framework.General.GetString("ADDTAB") %>" /></td>
                                    <td><a href="#" onclick="AddNewPage();">Add New </a></td>
                                </tr>
                                </asp:PlaceHolder>
                                <tr>
                                    <td>
                                        <img class="editthispage" src="/aspnet_client/jQuery/jsTree/edit.png" title="Rename Page" onclick="openeditthispage();" /></td>
                                    <td><a href="#" onclick="openeditthispage();">Edit This Page</a></td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <table class="innertable">
                                <tr>
                                    <td>
                                        <img class="actionicon" src="/aspnet_client/jQuery/jsTree/copy.png" title="Copy Page" onclick="copy_page();" /></td>
                                    <td><a href="#" onclick="copy_page();">Copy</a></td>
                                    <td>
                                        <img class="actionicon" src="/aspnet_client/jQuery/jsTree/cut.png" title="Cut Page" onclick="cut_page();" /></td>
                                    <td><a href="#" onclick="cut_page();">Cut</a></td>
                                </tr>
                                <tr>
                                    <td>
                                        <img class="actionicon" src="/aspnet_client/jQuery/jsTree/paste.png" title="Paste" onclick="paste();" /></td>
                                    <td><a href="#" onclick="paste();">Paste</a></td>
                                    <td>
                                        <img class="actionicon" src="/aspnet_client/jQuery/jsTree/delete.png" title="Delete" onclick="deletePage();" /></td>
                                    <td><a href="#" onclick="deletePage();">Delete</a></td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <table class="innertable">
                                <tr>
                                    <td>
                                        <img class="actionicon" src="/aspnet_client/jQuery/jsTree/navigate_up.png" title="Move Up" onclick="move_up()" /></td>
                                    <td><a href="#" onclick="move_up();">Up</a></td>
                                    <td>
                                        <img class="actionicon" src="/aspnet_client/jQuery/jsTree/navigate_down.png" title="Move Down" onclick="move_down()" /></td>
                                    <td><a href="#" onclick="move_down();">Down</a></td>
                                </tr>
                                <tr>
                                    <td>
                                        <img class="actionicon" src="/aspnet_client/jQuery/jsTree/navigate_up2.png" title="Move First" onclick="move_first()" /></td>
                                    <td><a href="#" onclick="move_first()">First</a></td>
                                    <td>
                                        <img class="actionicon" src="/aspnet_client/jQuery/jsTree/navigate_down2.png" title="Move Last" onclick="move_last()" /></td>
                                    <td><a href="#" onclick="move_last()">Last</a></td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <a href="#" onclick="seeChanges();">&nbsp;<img class="" src="/aspnet_client/jQuery/jsTree/refresh.png" title="Refresh" onclick="seeChanges();" /><br />
                                <span>Refresh</span>
                            </a>
                        </td>
                    </tr>

                </table>
                <%--
            <div class="add-rename-delete">
                <div class="add-rename">
                    <a href="#" onclick="AddNewPage();">
                        <img class="actionicon" src="/aspnet_client/jQuery/jsTree/add.png" title="<%: Appleseed.Framework.General.GetString("ADDTAB") %>" /><span>Add New</span> </a>
                </div>
                <div>
                    <a href="#" onclick="openeditthispage();" >
                        <img class="editthispage" src="/aspnet_client/jQuery/jsTree/edit.png" title="Rename Page" onclick="openeditthispage();"  />
                        <span>Edit this Page</span>
                    </a>
                </div>
            </div>
            <div class="copy-cut-paste-delete">
                <div>
                    <a href="#" onclick="copy_page();">
                        <img class="actionicon" src="/aspnet_client/jQuery/jsTree/copy.png" title="Copy Page" onclick="copy_page();" />
                        <span>Copy</span>
                    </a>
                    <a href="#" onclick="cut_page();">
                        <img class="actionicon" src="/aspnet_client/jQuery/jsTree/cut.png" title="Cut Page" onclick="cut_page();" />
                        <span>Cut</span>
                    </a>
                </div>
                <div>
                    <a href="#" onclick="paste();">
                        <img class="actionicon" src="/aspnet_client/jQuery/jsTree/paste.png" title="Paste" onclick="paste();" />
                        <span>Paste</span>
                    </a>
                    <a href="#" onclick="deletePage();">
                        <img class="actionicon" src="/aspnet_client/jQuery/jsTree/delete.png" title="Delete" onclick="deletePage();" />
                        <span>Delete</span>
                    </a>
                </div>
            </div>
            <div class="sorting">
                <div>
                    <a href="#" onclick="move_up();">
                        <img class="actionicon" src="/aspnet_client/jQuery/jsTree/navigate_up.png" title="Move Up" onclick="move_up()" />
                        <span>Up</span> </a>
                    <a href="#" onclick="move_down();">
                        <img class="actionicon" src="/aspnet_client/jQuery/jsTree/navigate_down.png" title="Move Down" onclick="move_down()" />
                        <span>Down</span>
                    </a>
                </div>
                <div>
                    <a href="#" onclick="move_first()">
                        <img class="actionicon" src="/aspnet_client/jQuery/jsTree/navigate_up2.png" title="Move First" onclick="move_first()" /><span>First</span></a>
                    <a href="#" onclick="move_last()">
                        <img class="actionicon" src="/aspnet_client/jQuery/jsTree/navigate_down2.png" title="Move Last" onclick="move_last()" />
                        <span>Last</span></a>
                </div>
            </div>
             <a href="#" onclick="seeChanges();">
                        <img class="" src="/aspnet_client/jQuery/jsTree/refresh.png" title="Refresh" onclick="seeChanges();" /><br />
                        <span>Refresh</span>
                    </a> --%>

                <%--<img class="actionicon" src="/aspnet_client/jQuery/jsTree/add_page.png" alt="<%: Appleseed.Framework.General.GetString("ADDTAB") %>" title="<%: Appleseed.Framework.General.GetString("ADDTAB") %>" onclick="AddNewPage();" />
            <img class="actionicon" src="/aspnet_client/jQuery/jsTree/edit_page.png" alt="Edit this page" title="Edit this page" onclick="openeditthispage();" />
            <img class="actionicon" src="/aspnet_client/jQuery/jsTree/refresh_page.png" alt="<%: Appleseed.Framework.General.GetString("RESET_SEE_CHANGES") %>" title="<%: Appleseed.Framework.General.GetString("RESET_SEE_CHANGES") %>" onclick="seeChanges();" />
            <img class="actionicon" src="/aspnet_client/jQuery/jsTree/delete_page.png" alt="Delete this page" title="Delete this page" onclick="deletePage()" />--%>
            </div>
        <div class="clearfix"></div>
        <div id="newjsTree"></div>
    </form>
    <div style="display: none">
        <input type="button" value="<%: Appleseed.Framework.General.GetString("ADDTAB") %>" onclick="AddNewPage();" />
        <input type="button" value="<%: Appleseed.Framework.General.GetString("RESET_SEE_CHANGES") %>" onclick="seeChanges();" />
        <input type="button" value="Edit This Page" onclick="openeditthispage();" />
    </div>
    <script type="text/javascript" src="<%= Appleseed.Framework.HttpUrlBuilder.BuildUrl("~/PageManagerTree/Scripts/jquery.jstree.js") %>"></script>
    <script type="text/javascript">
        var selectedpageid = 0; var parentid = 0; var nodeposition = 0; var lastpos = 0;
        var cutcpyid = -1;
        var copiednodename = '';
        var iscopy = false; var copyctclicked = false;
        $("#newjsTree").jstree({
            "core": {
                "animation": 0,
                "check_callback": true,
                "themes": {
                    'name': 'proton',
                    'responsive': true
                },
                "sort": function (a, b) {
                    return this.get_node(a).original.position > this.get_node(b).original.position ? 1 : -1;
                },
                "data": {
                    "url":
                        function (node) {
                            if (node.id === "#") {
                                return '<%= Url.Action("GetRootPage", "PageManagerTree") %>';
                            }
                            else {
                                return '<%= Url.Action("GetSubPages", "PageManagerTree") %>';
                            };
                        },
                    'type': 'POST',
                    'dataType': 'json',
                    'contentType': 'application/json',
                    'cache': false,
                    'data':
                        function (node) {
                            if (node.id === "#") {
                                return JSON.stringify({
                                    pageid: node.id.toString(),
                                    isPane: false,
                                    parentid: node.parent
                                });
                            }
                            else {
                                return JSON.stringify({
                                    pageid: node.id.toString(),
                                    isPane: Boolean(node.original.isPane),
                                    parentid: node.parent
                                });
                            }
                        }
                    ,
                    'success': function (data) {
                    }
                }
            },
            "contextmenu": {
                "items": contextMenu
            },
            "plugins": ["contextmenu", "dnd", "search", "state", "types", "unique", "crrm", "themes"]
        }).on('move_node.jstree', function (event, data) {
            movenode(data.node.id, data.node.parent, data.old_parent, data.position);
        }).on("select_node.jstree", function (e, data) {
            if (!copyctclicked) {
                copiednodename = data.node.text;
            }
            selectedpageid = data.node.id; parentid = data.node.parent; nodeposition = data.node.original.position; lastpos = data.node.original.lastpos
            $('#pagedetail').removeAttr("src");
            $('#pagedetail').attr("src", "/DesktopModules/CoreModules/Pages/PageLayout.aspx?PageID=" + data.node.id + "&mID=110&ModalChangeMaster=true");
        }).bind("rename_node.jstree", function (event, data) {
            var urlctrl;
            if (data.node.original.nodeType == "module") {
                urlctrl = '<%= Url.Action("RenameModule","PageManagerTree")%>' + '?id=' + parseInt(data.node.id.toString()) + '&name=' + data.text;
            }
            else {
                urlctrl = '<%= Url.Action("Rename","PageManagerTree")%>' + '?id=' + parseInt(data.node.id.toString()) + '&name=' + data.text;
            }
            $.ajax({
                url: urlctrl,
                type: 'POST',
                timeout: "100000",
                data: JSON.stringify(data.node),
                success: function (data) {
                    if (data.error == true) {
                        alert(data.errorMess.toString());
                        seeChanges();
                    } else {
                        $('#newjsTree').jstree('refresh');
                    }
                }
            });
        });
        function contextMenu(node) {
            if (node.original.nodeType == "pane") {
                return;
            }
            var items = {
                "create": {
                    "label": '<%: General.GetString("CREATE") %>',
                    "action":
                        function (obj) {
                            var pagenameval = prompt("Please enter page name", "New Page Name");
                            if (pagenameval != null) {
                                $.ajax({
                                    url: '/PageManagerTree/PageManagerTree/Create',//?pageid="' + + '"',
                                    type: 'POST',
                                    timeout: "100000",
                                    dataType: 'json',
                                    contentType: "application/json; charset=utf-8",
                                    data: JSON.stringify({
                                        pageid: node.id.toString(),
                                        pagename: pagenameval
                                    }),
                                    success: function (data) {
                                        if (data.error == true) {
                                            alert(data.errorMess.toString());
                                        }
                                        else {
                                            $('#newjsTree').jstree('refresh');
                                        }
                                    }
                                });
                            }
                        }
                    },
                "rename": {
                    "label": '<%: General.GetString("RENAME") %>',
                    "action": function (data) {
                        var inst = $.jstree.reference(data.reference),
                            obj = inst.get_node(data.reference);
                        inst.edit(obj);
                    }
                },
                "delete": {
                    "label": '<%: General.GetString("DELETE") %>',
                    "action":
                        function (obj) {
                            selectedpageid = node.id;
                            deletePage();
                        },
                    "separator_before": true
                },
                "copy": {
                    "label": '<%: General.GetString("COPY", "Copy") %>',
                    "action": function (obj) {
                        cutcpyid = parseInt(node.id);
                        copiednodename = node.text;
                        iscopy = true;
                        copyctclicked = true;
                    }
                },
                "cut": {
                    "label": '<%: General.GetString("CUT", "Cut") %>',
                    "action": function (obj) {
                        cutcpyid = parseInt(node.id);
                        iscopy = false;
                        copyctclicked = true;
                    }
                },
                "paste": {
                    "label": 'Paste',
                    "action": function (obj) {
                        paste();
                    }
                },
                "clone": {
                    "label": '<%: General.GetString("Clone","Clone") %>',
                    "action": function (obj) {
                        $.ajax({
                            url: '<%= Url.Action("Clone","PageManagerTree")%>' + '?id=' + parseInt(node.id) + '&parentId=' + parseInt(node.parent),
                            type: 'POST',
                            timeout: "100000",
                            data: JSON.stringify(node),
                            success: function (data) {
                                if (data.error == true) {
                                    alert(data.errorMess.toString());
                                }
                                else {
                                    $('#newjsTree').jstree('refresh');
                                }
                            }
                        });
                    }
                },
                "sort": {
                    "label": '<%: General.GetString("Sort","Sort") %>',
                    "submenu": {
                        "move_first": {
                            "label": '<%: General.GetString("Move_First","Move First") %>',
                            "action": function (obj) {
                                movenode(node.id, node.parent, node.parent, 0);
                            }
                        },
                        "move_up": {
                            "label": '<%: General.GetString("Move_Up","Move up") %>',
                            "action": function (obj) {
                                movenode(node.id, node.parent, node.parent, (parseInt(node.original.position) - 1));
                            }
                        },
                        "move_down": {
                            "label": '<%: General.GetString("Move_Down","Move Down") %>',
                            "action": function (obj) {
                                movenode(node.id, node.parent, node.parent, (parseInt(node.original.position) + 1));
                            }
                        },
                        "move_last": {
                            "label": '<%: General.GetString("Move_Last","Move Last") %>',
                            "action": function (obj) {
                                movenode(node.id, node.parent, node.parent, parseInt(node.original.lastpos));
                            }
                        },
                    }
                },
                "view": {
                    "label": '<%: General.GetString("View","View") %>',
                    "action": function (obj) {
                        $.ajax({
                            url: '<%= Url.Action("ViewPage","PageManagerTree")%>' + '?page_Id="' + node.id + '"',
                            type: 'POST',
                            timeout: "100000",
                            data: JSON.stringify(node),
                            success: function (data) {
                                window.location.href = data;
                                $('#newjsTree').jstree('refresh');
                            }
                        });
                    }
                }
            }
            // root node
            if (node.id == 0) {
                items.rename = items.delete = items.copy = items.cut = items.clone = items.view = items.sort = false;
            }
            if (cutcpyid == -1) {
                items.paste = false;
            }
            return items;
        }
        function move_last() {
            if (selectedpageid != 0)
                movenode(selectedpageid, parentid, parentid, parseInt(lastpos));
            else
                alert('Please select node first');
        }
        function move_first() {
            if (selectedpageid != 0)
                movenode(selectedpageid, parentid, parentid, 0);
            else
                alert('Please select node first');
        }
        function move_up() {
            if (selectedpageid != 0)
                movenode(selectedpageid, parentid, parentid, (parseInt(nodeposition) - 1));
            else
                alert('Please select node first');
        }
        function move_down() {
            if (selectedpageid != 0)
                movenode(selectedpageid, parentid, parentid, (parseInt(nodeposition) + 1));
            else
                alert('Please select node first');
        }
        function copy_page() {
            if (selectedpageid != '0') {
                cutcpyid = selectedpageid;
                iscopy = true;
                copyctclicked = true;
            }
            else {
                copyctclicked = false;
                alert('Please select node first');
            }
        }
        function cut_page() {
            if (selectedpageid != '0') {
                cutcpyid = parseInt(selectedpageid);
                iscopy = false;
                copyctclicked = true;
            }
            else {
                copyctclicked = false;
                alert('Please select node first');
            }
        }
        function paste() {
            if (cutcpyid != -1) {
                if (selectedpageid != cutcpyid) {
                    //paste code
                    $.ajax({
                        url: '<%= Url.Action("CutCopyPage","PageManagerTree")%>',
                        type: 'POST',
                        dataType: 'json',
                        contentType: "application/json; charset=utf-8",
                        timeout: "100000",
                        data: JSON.stringify({
                            pageId: parseInt(cutcpyid),
                            name: copiednodename,
                            parentId: parseInt(selectedpageid),
                            isCopy: Boolean(iscopy)
                        }),
                        success: function (data) {
                            if (data.error == true) {
                                alert(data.errorMess.toString());
                            } else {
                                $('#newjsTree').jstree('refresh');
                                cutcpyid = -1;
                                copyctclicked = false;
                            }
                        }
                    });
                }
                else {
                    alert('Please paste it into another node');
                }
            }
            else {
                copyctclicked = false;
                alert('Please perform copy or cut operation before paste');
            }
        }
        function deletePage() {
            if (selectedpageid != 0) {
                var agree = confirm('<%: General.GetString("CONFIRM_DELETE") %>');
                    if (agree) {
                        $.ajax({
                            url: '<%= Url.Action("remove","PageManagerTree")%>',
                        type: 'POST',
                        timeout: "100000",
                        dataType: 'json',
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify({
                            id: parseInt(selectedpageid),
                        }),
                        success: function (data) {
                            if (data.error == true) {
                                alert(data.errorMess.toString());
                            } else {
                                $('#newjsTree').jstree('refresh');
                                selectedpageid = 0;
                            }
                        }
                    });
                }
                else
                    return false;
            }
            else {
                alert('Please select page to delete');
            }
        }
        function seeChanges() {
            window.location.href = window.location.href;
        }
        function AddNewPage() {
            var pageid = 0;
            if (selectedpageid != undefined) {
                pageid = selectedpageid;
            }
            var pagenameval = prompt("Please enter page name", "New Page Name");
            if (pagenameval != null) {
                $.ajax({
                    url: '/PageManagerTree/PageManagerTree/Create',
                    type: 'POST',
                    timeout: "100000",
                    dataType: 'json',
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({
                        pageid: pageid,
                        pagename: pagenameval
                    }),
                    success: function (data) {
                        if (data.error == true) {
                            alert(data.errorMess.toString());
                        }
                        else {
                            $('#newjsTree').jstree('refresh');
                        }
                    }
                });
            }
        }
        function movenode(nodeid, parentid, oldparent, position) {
            $.ajax({
                url: '<%= Url.Action("moveNode","PageManagerTree")%>',
                type: 'POST',
                dataType: 'json',
                contentType: "application/json; charset=utf-8",
                timeout: "100000",
                data: JSON.stringify({
                    pageID: parseInt(nodeid),
                    newParent: parseInt(parentid),
                    idOldNode: parseInt(oldparent),
                    selectedposition: parseInt(position)
                }),
                success: function (data) {
                    if (data.error == true) {
                        seeChanges();
                        alert(data.errorMess.toString());
                    }
                    else {
                        $('#newjsTree').jstree('refresh');
                    }
                }
            });
        }
        function openeditthispage() {
            openInModal('/DesktopModules/CoreModules/Pages/PageLayout.aspx?PageID=' + selectedpageid + '&mID=110', 'Edit This Page');
        }
    </script>
</div>
