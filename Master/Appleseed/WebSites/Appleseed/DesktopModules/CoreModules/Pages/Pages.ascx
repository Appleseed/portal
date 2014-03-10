<%@ Control AutoEventWireup="true" Inherits="Appleseed.DesktopModules.CoreModules.Pages.Pages"
    Language="c#" CodeBehind="Pages.ascx.cs" %>

    <link rel="Stylesheet" href="../../../aspnet_client/jQuery/jsTree/themes/default/style.css" />

<div id="PageTree">
    <img src="../../../images/img/throbber.gif" alt="Loading ... "/>
    <input runat="server" type="text" id="TreeRoute" class="TreeRoute" style="visibility: hidden"/>
</div>

<script type="text/javascript">



    $(document).ready(function () {
        getTree($('.TreeRoute').val());
    });

    function getTree(url) {

        $.ajax({
            url: url,
            type: 'POST',
            timeout: "100000",
            success: function (data) {
                $("#PageTree").html(data);
            }
        });
    }
</script>



<%--<table cellpadding="5" cellspacing="0">
    <tr>
        <td class="SubHead" colspan="3">
            <rbfwebui:Label ID="lblHead" runat="server" Text="Pages" TextKey="AM_TABS" />
        </td>
    </tr>
    <tr valign="top">
        <td>
            <asp:ListBox ID="tabList" runat="server" CssClass="NormalTextBox" DataTextField="Name"
                DataValueField="ID" Rows="20" Width="400" />
        </td>
        <td>
            &nbsp;
        </td>
        <td>
            <table>
                <tr>
                    <td>
                        <rbfwebui:ImageButton ID="upBtn" runat="server" CommandName="up" TextKey="MOVE_UP"
                            OnClick="UpDownClick" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <rbfwebui:ImageButton ID="downBtn" runat="server" CommandName="down" TextKey="MOVE_DOWN"
                            OnClick="UpDownClick" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <rbfwebui:ImageButton ID="EditBtn" runat="server" CommandName="Edit" TextKey="EDITBTN"
                            OnClick="EditBtnClick" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <rbfwebui:ImageButton ID="DeleteBtn" runat="server" CommandName="Delete" TextKey="DELETEBTN"
                            OnClick="DeleteBtnClick" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="3">
            <rbfwebui:LinkButton ID="addBtn" runat="server" CssClass="CommandButton" Text="Add New Page"
                TextKey="ADDPAGE" OnClick="AddPageClick" />
        </td>
    </tr>
</table>
--%>