<%@ Control Language="c#" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.Sql" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Xml"%>
<script runat="server">

    protected Literal errors = new Literal();

    // Copied from /App_Controls/top_nav_dropdown.ascx.cs
    private void Page_Load( object sender, System.EventArgs e ) {

      DataView dv = getDV("PARENT_ID=1", "");
      Literal top_nav_item;
      
      for (int i = 0; i < dv.Count; i++)
      {
          string id = dv[i]["ID"].ToString();
          string menuTitle = dv[i]["MENU_TITLE"].ToString();
          string pageName = dv[i]["PAGE_NAME"].ToString();
          

          top_nav_item = new Literal();
          string liClass = "AspNet-Menu-Leaf";
          string aClass  = "AspNet-Menu-Link";

          top_nav_item.Text += "<li class='" + liClass + "'><a href=\"" + pageName + "\" class=\"" + aClass + "\">" + menuTitle + "</a>";
          top_nav_item.Text += InsertFirstLevelChidren(id);
          top_nav_item.Text += "</li>";
          custom_nav_placeholder.Controls.Add(top_nav_item);
          
      }

      custom_nav_placeholder.Controls.Add(errors);
    }

    // Copied from /App_Controls/top_nav_dropdown.ascx.cs
    protected String InsertFirstLevelChidren(String id)
    {
        String childHtml = "";

        DataView dv = getDV("PARENT_ID=" + id, "SORT_ORDER ASC");

        if (dv.Count > 0)
        {
            childHtml += "<ul>";

            foreach (DataRowView row in dv)
            {
                string pageName = row["PAGE_NAME"].ToString();
                string menuTitle = row["MENU_TITLE"].ToString();
                childHtml += "<li class=\"AspNet-Menu-Leaf\"><a href=\"" + pageName + "\" class=\"AspNet-Menu-Link\">" + menuTitle + "</a>";
            }

            childHtml += "</ul>";
        }

        return childHtml;
    }

    // Copied from /App_Code/baseOAWebControl.cs
    //return a dataview from a query from the main table
    public DataView getDV(String strQuery, String strOrder)
    {
        DataView dv = new DataView();
        try
        {
            DataSet dsall = dsall = doQuery("SELECT * FROM CONTENT");                
            dv = new DataView(dsall.Tables["table"], strQuery, strOrder, DataViewRowState.CurrentRows);
            //errors.Text += "TableColumns:"+dsall.Tables["table"].Columns.Count;
        }
        catch (Exception e)
        {
          //errors.Text += "caught:"+e.ToString(); 
        }
        finally
        {
          //errors.Text += "finally"; 
        }
        return dv;
    }

    // Copied from /App_Code/baseOAWebControl.cs
    // return a dataset from a query
    public DataSet doQuery(String strSQL)
    {
        SqlDataAdapter da = new SqlDataAdapter(strSQL, ConfigurationManager.ConnectionStrings["ConnectionString_AHRICMS"].ConnectionString);
        DataSet ds = new DataSet();
        da.Fill(ds, "table");
        return ds;
    }
</script>
    <div class="AspNet-Menu-Horizontal">
        <ul class="AspNet-Menu">
            <asp:PlaceHolder ID="custom_nav_placeholder" runat="server" />
        </ul>
    </div>
