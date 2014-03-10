<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title></title>
</head>
<body>
    <%List<String[]> datos = (List<String[]>) ViewData["datos"]; %>
    <div id="divLeng13_1" style="position:absolute;top:20px;width:150px">
        <table id="tableLanguages">
            <% int length = datos.Count / 2;
             for (int i = 0; i<length;i++)
            {
                String[] dato = datos.First<String[]>();
                datos.Remove(dato);
             %>
                <tr>
                    <td class="rb_LangSw_tbl"> <img alt="<%=dato[2] %>" src="<%=dato[1] %>"/> </td>
                    <td class="rb_LangSw_tbl"> 
                        <a href=" <%=dato[0] %>" > <%=dato[2] %></a>
                    </td>
                </tr>
            <%} %>    
        </table>
    </div>
    <div id="divLeng13_2" style="position:absolute;top:20px;left:150px;width:150px">
        <table id="table1">
            <%
            length = datos.Count;
            for (int i = 0; i<length;i++)
            {
                String[] dato = datos.First<String[]>();
                datos.Remove(dato);
              %>
            <tr>
                <td class="rb_LangSw_tbl"> <img alt="<%=dato[2] %>" src="<%=dato[1] %>"/> </td>
                <td class="rb_LangSw_tbl">
                    <a href=" <%=dato[0] %>" > <%=dato[2] %></a>
                       
                </td>
            </tr>
            <%} %>    
        </table>
    </div>
</body>
</html>
