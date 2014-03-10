<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="Appleseed.Framework" %>
<%@ Import Namespace="PageManagerTree" %>

<div id="jqtreePageManagement">
<script type="text/javascript" src="<%= Appleseed.Framework.HttpUrlBuilder.BuildUrl("~/PageManagerTree/Scripts/jquery.jstree.js") %>"></script>
<link type="text/css" rel="stylesheet" href="<%: Appleseed.Framework.HttpUrlBuilder.BuildUrl("~/aspnet_client/jQuery/jsTree/themes/default/style.css") %>" /> 


<form id="form1" runat="server">
    <div id="jsTree" class="demo">
        

    </div>
</form>
<div>
    <input type="button" value="<%: Appleseed.Framework.General.GetString("ADDTAB") %>" onclick="AddNewPage();"/>
    <input type="button" value="<%: Appleseed.Framework.General.GetString("RESET_SEE_CHANGES") %>" onclick="seeChanges();"/>
</div>


<script type="text/javascript">
    var nodeID;
    $(function () {
        $("#jsTree")
//        .bind("loaded.jstree", function (e, data) {
//            data.inst.open_all('#pjson_0'); // -1 opens all nodes in the container
//        })
        .jstree({
            "json_data": {
                "ajax": {
                    "type": "POST",
                    "url":
                        function (node) {
                            //var nodeId = node.attr('id'); 
                            if (node == -1) {
                                return '<%= Url.Action("GetTreeData", "PageManagerTree") %>';
                            }else
                                return '<%= Url.Action("AddModule", "PageManagerTree") %>' + '?id=' + node.attr('id');
                        },
                    
                    "data": function (n) {
                        return { ID: n.attr ? n.attr("ID") : 0 };
                    },
                    "success": function (new_data) {
                        return new_data;
                    }
                }
            },
            "types": {
                "max_depth" : -2,
                "max_children" : -2,
                "valid_children": "none",
                "types": {
                    "default": {
                        "valid_children": ["folder", "folder2", "default"],
                    },
                    "file": {
                        "valid_children": "none",
                        "icon": {
                            "image": '<%= HttpUrlBuilder.BuildUrl("~/images/file.png") %>',
                        }
                    },
                    "folder": {
                        "valid_children": ["file"],
                        "icon": {
                            "image": '<%= HttpUrlBuilder.BuildUrl("~/images/folder.png") %>',
                        },
                        "start_drag" : false,
					    "move_node" : false,
                        "delete_node": false,
					    "remove": false,
                        "rename": false,
                    },
                    "folder2": {
                        "valid_children": ["file"],
                        "icon": {
                            "image": '<%= HttpUrlBuilder.BuildUrl("~/images/folder2.png") %>',
                        },
                        "start_drag" : false,
                        "move_node" : false,
                        "delete_node": false,
                        "remove": false,
                        "rename": false,
                    },
                    "root": {
                        "valid_children": ["default"],
                        "icon": {
                            "image": '<%= HttpUrlBuilder.BuildUrl("~/images/root.png") %>',
                        },
                        "start_drag" : false,
                        "move_node" : false,
                        "delete_node" : false,
                        "remove" : false
                    }
                },
            },
            "contextmenu": {
                "items": contextMenu
            
            },
            "crrm": {
                "move": {
                    "check_move": function (m) {
                        if (m.np.attr("id") == "jsTree") return false;
                        return true;
                    }
                }
            },
            "dnd": {
                "drop_target": false,
                "drag_target": false
            },
            "core": { "initially_open" : ["pjson_0"]} ,

            "plugins": ["themes", "contextmenu", "json_data", "ui", "crrm", "dnd", "core", "types"]
        })
        .bind("move_node.jstree", function (e, data) {
            var selectedId = data.rslt.o.attr("id");
            
            if (selectedId.indexOf("module") != -1) {
                var selected = data.rslt.np.text().replace(/\s{2}/, "");
                var children = data.rslt.np.children().children().text().replace(/\s/, "");
                var folder = selected.replace(children, "");
                $.ajax({
                    url: '<%= Url.Action("MoveModuleNode","PageManagerTree")%>',
                    type: 'POST',
                    timeout: "100000",
                    data: {
                        "pageId": $.jstree._focused()._get_parent(data.rslt.np).attr("id").replace("pjson_", ""),
                        "paneName": folder,
                        "moduleId": data.rslt.o.attr("id").replace("pjson_module_", "")
                    },
                    success: function (data) {
                    }
                });
            } else {
                if ((typeof(data.rslt.or.attr("id"))) == 'undefined') {
                    result = -1;
                } else {
                    result = data.rslt.or.attr("id").replace("pjson_", "");
                }
                $.ajax({
                    url: '<%= Url.Action("moveNode","PageManagerTree")%>',
                    type: 'POST',
                    timeout: "100000",
                    data: {
                        "pageId": data.rslt.o.attr("id").replace("pjson_", ""),
                        "newParent": data.rslt.np.attr("id").replace("pjson_", ""),
                        "idOldNode": result
                    },
                    success: function(data) {
                    }
                });
            }
        })
        .bind("rename.jstree",function(event,data) {
            var selectedId = data.rslt.obj.attr("id");
            if (selectedId.indexOf("module") != -1) {
                $('#jsTree').jstree("deselect_all");
                $("#jsTree").jstree("select_node", data.rslt.obj.parents()[3]).trigger("select_node.jstree");
                $.ajax({
                    url: '<%= Url.Action("RenameModule","PageManagerTree")%>',
                    type: 'POST',
                    timeout: "100000",
                    data: {
                        "id": data.rslt.obj.attr("id").replace("pjson_module_", ""),
                        "name": data.rslt.new_name
                    },
                    success: function (data) {
                        if (data.error == true) {
                        } else {
                            //$("#jsTree").jstree("refresh", -1);
                        }
                    }
                });
            } else {

                $.ajax({
                    url: '<%= Url.Action("Rename","PageManagerTree")%>',
                    type: 'POST',
                    timeout: "100000",
                    data: {
                        "id": data.rslt.obj.attr("id").replace("pjson_", ""),
                        "name": data.rslt.new_name
                    },
                    success: function(data) {
                        if (data.error == true) {

                        } else {
                            $("#jsTree").jstree("refresh", -1);
                        }
                    }
                });
            }
        })
        .bind("open_node.jstree", function (event, data) {
            var pageid = data.rslt.obj.attr("id").replace("pjson_", "");
            if ((pageid != 0) && (pageid.indexOf('_') == -1)) {
                $.ajax({
                    url: '<%= Url.Action("AddModule","PageManagerTree")%>',
                    type: 'POST',
                    data: {
                        "id": pageid.replace("pjson_", ""),
                    },
                    success: function (data) {
                        return data;
                    }
                });
            }

        });

    });

    function contextMenu(node) {
        var items = {
            "create": {
                "label": '<%: General.GetString("CREATE") %>',
                            "action":
                                function(obj) {
                                    $.ajax({
                                        url: '<%= Url.Action("create","PageManagerTree")%>',
                            type: 'POST',
                            timeout: "100000",
                            data: {
                                "id": obj.attr("id").replace("pjson_", "")
                            },
                            success: function(data) {
                                $("#jsTree").jstree("refresh", -1);
                            }
                        });
                    }
            },
            
            ViewItem: {
                label: '<%: General.GetString("View","View") %>',
                action: function(obj) {
                    var currentId = this._get_node(obj).attr("id");
                    $.ajax({
                        url: '<%= Url.Action("ViewPage")%>',
                        type: 'POST',
                        timeout: "100000",
                        data: {
                            "pageId": currentId.replace("pjson_", "")
                        },
                        success: function(data) {
                            window.location.href = data;
                        }
                    });

                },
                "separator_before": true,
            },
            renameItem: {
                "label": '<%: General.GetString("RENAME") %>',
                "action": function(obj) {
                    this.rename(obj);

                },
            },
            "edit": {
                "label": '<%: General.GetString("EDIT") %>',
                "action":
                    function(obj) {
                        $.ajax({
                            url: '<%= Url.Action("edit","PageManagerTree")%>',
                            type: 'POST',
                            timeout: "100000",
                            data: {
                                "id": obj.attr("id").replace("pjson_", ""),
                            },
                            success: function(data) {
                                window.location.href = data.url;

                            }
                        });
                    }
            },
            "editModule": {
                "label": '<%: General.GetString("EDIT") %>',
                "action":
                    function (obj) {
                        editModule(obj.parents()[3].id.replace("pjson_", ""), obj.attr("id").replace("pjson_module_", ""));
                    },
                },
            "ccp": false,
            "remove": false,
            "rename": false,

            cloneItem: {
                label: '<%: General.GetString("Clone","Clone") %>',
                action: function(obj) {
                    var currentId = this._get_node(obj).attr("id");
                    var parentId = this._get_node(obj)[0].firstChild.parentElement.parentNode.parentElement.id;
                    $.ajax({
                        url: '<%= Url.Action("Clone")%>',
                        type: 'POST',
                        timeout: "100000",
                        data: {
                            "id": currentId.replace("pjson_", ""),
                            "parentId": parentId.replace("pjson_", "")
                        },
                        success: function(data) {
                            var name = $("#jsTree").jstree("get_text", '#' + currentId) + ' - Clone';
                            $("#jsTree").jstree("create", "#" + parentId, "last", { 'attr': { 'id': 'pjson_' + data.pageId }, 'title': name }, false, true);
                            $("#jsTree").jstree("set_text", '#pjson_' + data.pageId, name);
                            $("#jsTree").jstree("rename", "#pjson_" + data.pageId);
                        },
                        error: function(data) {
                            $.jstree.rollback(obj.rlbk);
                        }
                    });

                },
            },
            copyItem: {
                label: '<%: General.GetString("COPY", "Copy") %>',
                action: function(obj) {
                    var currentId = this._get_node(obj).attr("id");
                    var currentName = this._get_node(obj).text().replace(/\s{2}/, "");
                    var children = this._get_node(obj).children().children().text().replace(/\s/, "");
                    var folder = currentName.replace(children, "");
                    var parentId = this._get_node(obj)[0].firstChild.parentElement.parentNode.parentElement.id;
                    $.ajax({
                        url: '<%= Url.Action("CopyPage")%>',
                        type: 'POST',
                        timeout: "100000",
                        data: {
                            "pageId": currentId.replace("pjson_", ""),
                            "name": folder,
                        },
                        success: function(data) {
                            var name = $("#jsTree").jstree("get_text", '#' + currentId) + ' - Copy';
                            $("#jsTree").jstree("create", "#" + parentId, "last", { 'attr': { 'id': 'pjson_' + data.pageId }, 'title': name }, false, true);
                            $("#jsTree").jstree("set_text", '#pjson_' + data.pageId, name);
                            $("#jsTree").jstree("rename", "#pjson_" + data.pageId);
                        },
                        error: function(data) {
                            $.jstree.rollback(obj.rlbk);
                        }
                    });

                },
            },
            "delete": {
                "label": '<%: General.GetString("DELETE") %>',
                "action":
                    function(obj) {

                        var agree = confirm('<%: General.GetString("CONFIRM_DELETE") %>');
                        if (agree)
                            deletePage(obj.attr("id").replace("pjson_", ""));
                        else
                            return false;


                    },
                "separator_before": true
            },
            "deletemodule": {
                "label": '<%: General.GetString("DELETE") %>',
                "action":
                    function (obj) {
                        var agree = confirm('<%: General.GetString("CONFIRM_DELETE") %>');
                        if (agree) {
                            var object = obj;
                            var pageId = obj.parents()[3].id.replace("pjson_", "");
                            var moduleId = obj.attr("id").replace("pjson_module_", "");
                            var parent = obj.parents()[3];
                            deleteModule(pageId, moduleId, object, parent);
                            
                        } else
                            return false;
                    },
                "separator_before": true
            },
        };
        
        if (($(node).attr("rel") == "folder") || ($(node).attr("rel") == "root") || ($(node).attr("rel") == "folder2")) {
            delete items.delete;
            delete items.create;
            delete items.copyItem;
            delete items.cloneItem;
            delete items.renameItem;
            delete items.ViewItem;
            delete items.edit;
            delete items.deletemodule;
            delete items.editModule;
        }
        
        if ($(node).attr("rel") == "file") {
            delete items.create;
            delete items.copyItem;
            delete items.cloneItem;
            delete items.ViewItem;
            delete items.delete;
            delete items.edit;
        }
        
        if ($(node).attr("rel") == undefined) {
            delete items.deletemodule;
            delete items.editModule;
        }

        return items;
    }
    


    function seeChanges(){
        window.location.href = window.location.href;
    }

    function AddNewPage(){
        $.ajax({
                url: '<%= Url.Action("create","PageManagerTree")%>',
                type: 'POST',
                timeout: "100000",
                data: {
                    "id": 0
                },
                success: function (data) {
                    $("#jsTree").jstree("refresh", -1);
                }
            });
    }

    function deletePage(id){
        if(id != 0) {           
           
            $.ajax({
                url: '<%= Url.Action("remove","PageManagerTree")%>',
                type: 'POST',
                timeout: "100000",
                data: {
                    "id": id
                },
                success: function (data) {
                    if (data.error == true) {
                        alert(data.errorMess.toString());
                    } else {
                        $("#jsTree").jstree("refresh", -1);
                    }
                }
            });
                     
       }
    }
    
    function deleteModule(pageid, moduleId, obj, parent) {
        $.ajax({
            url: '<%= Url.Action("RemoveModule","PageManagerTree")%>',
            type: "POST",
            timeout: 180000,
            data: {
                id: moduleId,
            },
            success: function (data) {
                if (data.error == true) {
                    alert(data.errorMess);
                } else {
                    $("#jsTree").jstree("remove", obj);
                }
                $('#jsTree').jstree("deselect_all");
                $("#jsTree").jstree("select_node", parent).trigger("select_node.jstree");
            }
        });
    }
    
    function editModule(idPage, moduleId) {
       
         $.ajax({
             url: '<%= Url.Action("GetUrlToEdit","PageManagerTree")%>',
             type: "POST",
             timeout: 180000,
             data: {
                 pageId: idPage, 
                 moduleId: moduleId,
             },
             success: function (data) {
                 window.location.href = data;
             }
         });
    }

    $(function() {
         var c;
        $('#jqtreePageManagement').children().each(function (i) {
            var vs = this;
            if($(vs).has('input').length > 0){
                c = $(vs).children(0); 
                if($(c).attr('id') == '__VIEWSTATE')  {        
                    $(c).remove();
                    $(vs).remove();
                } 
            }      
        });
    });

    function logNop() {
        console.log('');
  
    }

</script>

</div>