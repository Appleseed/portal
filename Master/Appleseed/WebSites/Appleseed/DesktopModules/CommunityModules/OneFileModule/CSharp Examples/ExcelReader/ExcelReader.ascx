<%@ control inherits="Appleseed.Content.Web.Modules.OneFileModule" language="c#" %>
<%@ register assembly="Appleseed.Framework" namespace="Appleseed.Framework.Web.UI.WebControls"
    tagprefix="rbfwebui" %>

<script runat="server" language="C#">

    void Page_Load(Object sender, EventArgs e)
    {
        try
        {
            // get the settings for this module
            InitSettings(SettingsType.Str);

            if (SettingsExists)
            {
                // Settings are : ExcelFile + RangeName

                // pick the Excel file from settings adding the path from the portal Data directory
                Appleseed.Framework.DataTypes.PortalUrlDataType pt;
                pt = new Appleseed.Framework.DataTypes.PortalUrlDataType();
                pt.Value = GetSetting("ExcelFile");
                String sExcelFile = Server.MapPath("~/DeskTopModules/" + pt.Value);

                // Create connection
                String sConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                    "Data Source=" + sExcelFile + ";" + "Extended Properties=Excel 8.0;";
                System.Data.OleDb.OleDbConnection objConn = new System.Data.OleDb.OleDbConnection(sConnectionString);
                objConn.Open();

                // The code to follow uses a SQL SELECT command to display the data from the worksheet.
                System.Data.OleDb.OleDbCommand objCmdSelect = new System.Data.OleDb.OleDbCommand("SELECT * FROM " + GetSetting("RangeName"), objConn);
                System.Data.OleDb.OleDbDataAdapter objAdapter1 = new System.Data.OleDb.OleDbDataAdapter();
                objAdapter1.SelectCommand = objCmdSelect;

                // Create new DataSet to hold information from the worksheet.
                System.Data.DataSet objDataset1 = new System.Data.DataSet();
                try
                {
                    objAdapter1.Fill(objDataset1, "XLData");
                }
                finally
                {
                    // Clean up objects.
                    objConn.Close();
                }

                // Bind data to DataGrid control.
                DataGrid1.DataSource = objDataset1.Tables[0].DefaultView;
                DataGrid1.DataBind();
            }

        }

        catch (Exception ex)
        {
            Response.Write("Problem in ExcelReader. Message:" + ex.ToString());
        }
    }

</script>

<rbfwebui:desktopmoduletitle id="ModuleTitle" runat="server" edittext="Edit" editurl="~/DesktopModules/CoreModules/Admin/PropertyPage.aspx"
    propertiestext="PROPERTIES" propertiesurl="~/DesktopModules/CoreModules/Admin/PropertyPage.aspx">
</rbfwebui:desktopmoduletitle>
<asp:datagrid id="DataGrid1" runat="server">
</asp:datagrid>
