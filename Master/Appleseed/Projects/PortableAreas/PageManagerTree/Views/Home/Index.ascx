<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="PageManagerTree" %>
<link rel="stylesheet" type="text/css" href='<%: Url.PageManagerTreeResource("PageManagerTree.Content.style.css") %>' />
<script language="javascript" type="text/javascript">

    if (typeof jQuery != 'function') {
        var s = '<%: Url.PageManagerTreeResource("PageManagerTree.Scripts.jquery-1.4.1.min.js") %> ';
        document.write("<script language='javascript' type='text/javascript' src='" + s + "' />");
    }

    $(document).ready(function () {
        $('#spn').text('.js files works !');
    });

</script>
<ul>
    <li>RenderAction works !</li>
    <li>
        <img id="example" src='<%: Url.PageManagerTreeResource("PageManagerTree.Content.img.koala.jpg") %>'
            alt="" width="40px" />Images works !</li>
    <li><span id="spn"></span></li>
    <li class="cssli" style="color: White; background-color: white">css files works !</li>
    <li class="not-cssli">css don´t :( </li>
</ul>
