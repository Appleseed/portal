<%@ Page Language="C#" MasterPageFile="~/Shared/SiteMasterDefault.master" AutoEventWireup="true" CodeBehind="AccessPermissionViewer.aspx.cs" Inherits="Appleseed.DesktopModules.CoreModules.Roles.AccessPermissionViewer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <h3>Access Permission Viewer</h3>

    <asp:Literal runat="server" ID="ltrAccessViewer">
    </asp:Literal>
    <br />
    <br />
    <rbfwebui:LinkButton ID="btnSaveAccess" runat="server" CssClass="CommandButton" TextKey="ROLE_PREMISSION_ACCESS_SAVE" Text="Save" OnClientClick="return SetValue(); " OnClick="btnSaveAccess_Click" />
    <rbfwebui:LinkButton ID="btnCancelAccess" runat="server" CssClass="CommandButton" TextKey="ROLE_PREMISSION_ACCESS_CANCEL" Text="Cancel" OnClick="btnCancelAccess_Click" />
    <br />
    <br />
    <br />
    <asp:HiddenField runat="server" ID="ReturnUrl" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="selectedPermissions" ClientIDMode="Static" />

    <%--    <style type="text/css">
        .headcol {
            position: absolute;
            width: 250px;
            left: 0;
            top: auto;
            /*border-right: 0px none black;*/
            /*border-top-width: 3px;*/ /*only relevant for first row*/
            /*margin-top: -3px;*/ /*compensate for top border*/
        }

        #accessviewer table tr:first-child td {
            font-weight: 900;

        }

        #accessviewer table
        {
            padding:0px;

        }

        #accessviewer table tr td
        {
            border:1px solid gray;
        }


        .long {
            text-align: center;
            padding-left: 15px;
        }

        #accessviewer {
            overflow-x: scroll;
            margin-left: 235px;
            overflow-y: visible;
            padding-bottom: 1px;
        }
    </style>--%>
    <style>
        #accessviewer table tr:first-child td {
            font-weight: bold;
            background-color: Gray;
            color: #fff;
        }

        #accessviewer table {
            padding: 0px;
        }

            #accessviewer table tr td {
                border: 1px solid gray;
                padding: 4px;
            }

            #accessviewer table tr:hover {
                background-color: #f1f1f1;
            }

        .long {
            text-align: center;
        }
    </style>


    <script type="text/javascript">
        function SetValue() {
            var checkboxData = [];
            $(".long input[type=checkbox]:checked").each(function () {
                var curentItem = $(this);
                checkboxData.push(curentItem.attr("id"));
            });
            $("#selectedPermissions").val(checkboxData.join(','));
            return true;
        }
    </script>
</asp:Content>
