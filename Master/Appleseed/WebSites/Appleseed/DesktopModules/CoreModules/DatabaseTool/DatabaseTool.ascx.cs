using System;
using System.Data;
using System.Data.SqlClient;
using Appleseed.Framework;
using Appleseed.Framework.DataTypes;
using Appleseed.Framework.Web.UI.WebControls;
using Label = System.Web.UI.WebControls.Label;

namespace Appleseed.Content.Web.Modules
{
    using System.Web.UI.WebControls;

    /// <summary>
    /// DatabaseTool Module
    /// Based on VB code Written by Sreedhar Koganti (w3coder)
    /// Modifications (lots!) and conversion for Appleseed by Jakob hansen
    /// </summary>
    public partial class DatabaseTool : PortalModuleControl
    {
        protected bool Trusted_Connection;
        protected string ServerName;
        protected string DatabaseName;
        protected string UserID;
        protected string Password;

        protected string InfoFields;
        protected string InfoExtendedFields;
        protected bool ShowQueryBox;
        protected int QueryBoxHeight;

        protected bool Connected = false;
        protected string ConnectionString;


        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            Trusted_Connection = "True" == Settings["Trusted Connection"].ToString();
            ServerName = Settings["ServerName"].ToString();
            DatabaseName = Settings["DatabaseName"].ToString();
            UserID = Settings["UserID"].ToString();
            Password = Settings["Password"].ToString();

            if (Trusted_Connection)
                ConnectionString = "Server=" + ServerName + ";Trusted_Connection=true;database=" + DatabaseName;
            else
                ConnectionString = "Server=" + ServerName + ";database=" + DatabaseName + ";uid=" + UserID + ";pwd=" +
                                   Password + ";";

            Connected = Connect(lblRes);

            panConnected.Visible = Connected;
            if (Connected)
            {
                lblConnectedError.Visible = false;

                InfoFields = Settings["InfoFields"].ToString();
                InfoExtendedFields = Settings["InfoExtendedFields"].ToString();

                ShowQueryBox = "True" == Settings["Show Query Box"].ToString();
                QueryBoxHeight = int.Parse(Settings["Query Box Height"].ToString());
                panQueryBox.Visible = ShowQueryBox;
                txtQueryBox.Height = QueryBoxHeight;

                if (!Page.IsPostBack)
                    FillObjects("U", "dbo"); // The User table is the initial selected object
            }
            else
            {
                lblConnectedError.Visible = true;
                lblConnectedError.Text = "Please connect to a database... (check settings)";
            }
        }


        /// <summary>
        /// Connects the specified LBL.
        /// </summary>
        /// <param name="lbl">The LBL.</param>
        /// <returns></returns>
        protected bool Connect(Label lbl)
        {
            lbl.Text = string.Empty;
            bool retValue;
            try
            {
                SqlConnection SqlCon = new SqlConnection(ConnectionString);
                SqlDataAdapter DA = new SqlDataAdapter("SELECT NULL", SqlCon);
                SqlCon.Open();

                SqlCon.Close();
                //SqlCon.Dispose();
                retValue = true;
            }
            catch (Exception ex)
            {
                lbl.Text = "Error: " + ex.Message;
                retValue = false;
            }
            return retValue;
        }


        /// <summary>
        /// Fills the objects.
        /// </summary>
        /// <param name="xtype">The xtype.</param>
        /// <param name="user">The user.</param>
        protected void FillObjects(string xtype, string user)
        {
            lblRes.Text = string.Empty;
            try
            {
                SqlConnection SqlCon = new SqlConnection(ConnectionString);
                SqlDataAdapter DA =
                   new SqlDataAdapter(
                       "Select name,id from sysobjects where uid=USER_ID('" + user + "') AND xtype='" + xtype +
                       "' order by name", SqlCon);
                SqlCon.Open();

                DataSet DS = new DataSet();
                try
                {
                    DA.Fill(DS, "Table");
                    lbObjects.DataSource = DS;
                    lbObjects.DataTextField = "name";
                    lbObjects.DataValueField = "id";
                    lbObjects.DataBind();
                    lbObjects.SelectedIndex = 0;
                }
                finally
                {
                    SqlCon.Close();
                }
            }
            catch (Exception ex)
            {
                lblRes.Text = "Error: " + ex.Message;
            }
        }


        /// <summary>
        /// Fills the data grid.
        /// </summary>
        /// <param name="SQL">The SQL.</param>
        protected void FillDataGrid(string SQL)
        {
            lblRes.Text = string.Empty;
            try
            {
                SqlConnection SqlCon = new SqlConnection(ConnectionString);
                SqlDataAdapter DA = new SqlDataAdapter(SQL, SqlCon);
                SqlCon.Open();

                DataSet DS = new DataSet();
                try
                {
                    DA.Fill(DS, "Table");
                    DataGrid1.DataSource = DS;
                    DataGrid1.DataBind();
                }
                finally
                {
                    SqlCon.Close();
                }
            }
            catch (Exception ex)
            {
                lblRes.Text = "Error: " + ex.Message;
            }
        }


        /// <summary>
        /// Gets the table field.
        /// </summary>
        /// <param name="selectCmd">The select CMD.</param>
        /// <param name="idxField">The idx field.</param>
        protected void GetTableField(string selectCmd, int idxField)
        {
            lblRes.Text = string.Empty;
            try
            {
                SqlConnection SqlCon = new SqlConnection(ConnectionString);
                SqlCommand SqlComm = new SqlCommand(selectCmd, SqlCon);

                SqlComm.Connection.Open();
                try
                {
                    SqlDataReader dr = SqlComm.ExecuteReader(CommandBehavior.CloseConnection);
                    try
                    {
                        if (dr.Read())
                            txtQueryBox.Text = dr[idxField].ToString();
                        else
                            txtQueryBox.Text = "No data for SQL: \n" + selectCmd;
                    }
                    finally
                    {
                        dr.Close(); //by Manu, fixed bug 807858
                    }
                }
                finally
                {
                    SqlCon.Close();
                }
            }
            catch (Exception ex)
            {
                lblRes.Text = "Error: " + ex.Message;
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ObjectSelectList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected void ObjectSelectList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // There are only data in tables or views:
            if (ddObjectSelectList.SelectedItem.Value == "U" ||
                ddObjectSelectList.SelectedItem.Value == "V")
                btnGetObjectData.Visible = true;
            else
                btnGetObjectData.Visible = false;

            FillObjects(ddObjectSelectList.SelectedItem.Value, tbUserName.Text);
        }


        /// <summary>
        /// Handles the Click event of the GetObjectInfo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected void GetObjectInfo_Click(object sender, EventArgs e)
        {
            FillDataGrid("SELECT " + InfoFields + " FROM sysobjects WHERE uid=USER_ID('" + tbUserName.Text +
                         "') AND id=" + lbObjects.SelectedItem.Value.ToString());
        }


        /// <summary>
        /// Handles the Click event of the GetObjectInfoExtended control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected void GetObjectInfoExtended_Click(object sender, EventArgs e)
        {
            FillDataGrid("SELECT " + InfoExtendedFields + " FROM sysobjects WHERE uid=USER_ID('" + tbUserName.Text +
                         "') AND id=" + lbObjects.SelectedItem.Value.ToString());
        }


        /// <summary>
        /// Handles the Click event of the GetObjectProps control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected void GetObjectProps_Click(object sender, EventArgs e)
        {
            string SQL = string.Empty;
            if (ddObjectSelectList.SelectedItem.Value == "U" ||
                ddObjectSelectList.SelectedItem.Value == "V")
            {
                SQL += "EXEC sp_columns";
                SQL += " @table_name = '" + lbObjects.SelectedItem.Text.ToString() + "'";
                SQL += ",@table_owner = '" + tbUserName.Text + "'";
                FillDataGrid(SQL);
            }
            else
            {
                SQL += " SELECT c.[text] FROM sysobjects o, syscomments c";
                SQL += " WHERE o.uid=USER_ID('" + tbUserName.Text + "')";
                SQL += " AND o.id=c.id";
                SQL += " AND o.id=" + lbObjects.SelectedItem.Value.ToString();

                GetTableField(SQL, 0);
            }
        }


        /// <summary>
        /// Handles the Click event of the GetObjectData control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected void GetObjectData_Click(object sender, EventArgs e)
        {
            FillDataGrid("SELECT * FROM " + lbObjects.SelectedItem.Text.Trim());
        }


        /// <summary>
        /// Handles the Click event of the QueryExecute control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected void QueryExecute_Click(object sender, EventArgs e)
        {
            lblRes.Text = string.Empty;
            try
            {
                SqlConnection SqlCon = new SqlConnection(ConnectionString);

                string SQL = txtQueryBox.Text.Trim();
                if (SQL.Length > 6 && SQL.Substring(0, 6).ToUpper() == "SELECT")
                {
                    SqlDataAdapter DA = new SqlDataAdapter(SQL, SqlCon);
                    SqlCon.Open();

                    DataSet DS = new DataSet();
                    try
                    {
                        DA.Fill(DS, "Table");
                        DataGrid1.DataSource = DS;
                        DataGrid1.DataBind();
                        lblRes.Text = "Successful Query...";
                    }
                    finally
                    {
                        SqlCon.Close();
                    }
                }
                else
                {
                    SqlCommand SqlComm = new SqlCommand(SQL, SqlCon);
                    SqlCon.Open();

                    try
                    {
                        int Rowseff = SqlComm.ExecuteNonQuery();
                        lblRes.Text = "Effected Rows: " + Rowseff.ToString();
                    }
                    finally
                    {
                        SqlCon.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                lblRes.Text = "Error: " + ex.Message;
            }
        }


        /// <summary>
        /// Gets the GUID ID.
        /// </summary>
        /// <value>The GUID ID.</value>
        public override Guid GuidID
        {
            get { return new Guid("{2502DB18-B580-4F90-8CB4-C15E6E531032}"); }
        }

        /// <summary>
        /// Admin Module
        /// </summary>
        /// <value><c>true</c> if [admin module]; otherwise, <c>false</c>.</value>
        public override bool AdminModule
        {
            get { return true; }
        }

        /// <summary>
        /// Public constructor. Sets base settings for module.
        /// </summary>
        public DatabaseTool()
        {
            var trustedConnection = new SettingItem<bool, CheckBox> { Order = 1, Value = true };
            //Trusted_Connection.Required = true;   // hmmm... problem here! Dont set to true!" 
            this.BaseSettings.Add("Trusted Connection", trustedConnection);

            var serverName = new SettingItem<string, TextBox> { Order = 2, Required = true, Value = "localhost" };
            this.BaseSettings.Add("ServerName", serverName);

            var databaseName = new SettingItem<string, TextBox> { Order = 3, Required = true, Value = "Appleseed" };
            this.BaseSettings.Add("DatabaseName", databaseName);

            var userId = new SettingItem<string, TextBox> { Order = 4, Required = false, Value = string.Empty };
            this.BaseSettings.Add("UserID", userId);

            var password = new SettingItem<string, TextBox> { Order = 5, Required = false, Value = string.Empty };
            this.BaseSettings.Add("Password", password);

            var infoFields = new SettingItem<string, TextBox> { Order = 6, Required = true, Value = "name,id,xtype,uid" };
            this.BaseSettings.Add("InfoFields", infoFields);

            var infoExtendedFields = new SettingItem<string, TextBox> { Order = 7, Required = true, Value = "*" };
            this.BaseSettings.Add("InfoExtendedFields", infoExtendedFields);

            var showQueryBox = new SettingItem<bool, CheckBox> { Order = 8, Value = true };
            //ShowQueryBox.Required = true;   // hmmm... problem here! Dont set to true!" 
            this.BaseSettings.Add("Show Query Box", showQueryBox);

            var queryBoxHeight = new SettingItem<int, TextBox> { Order = 9, Required = true, Value = 150, MinValue = 10, MaxValue = 2000 };
            this.BaseSettings.Add("Query Box Height", queryBoxHeight);
        }

        #region Web Form Designer generated code

        /// <summary>
        /// Raises the init event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            this.ddObjectSelectList.SelectedIndexChanged += new EventHandler(this.ObjectSelectList_SelectedIndexChanged);
            this.btnGetObjectInfo.Click += new EventHandler(this.GetObjectInfo_Click);
            this.btnGetObjectInfoExtended.Click += new EventHandler(this.GetObjectInfoExtended_Click);
            this.btnGetObjectProps.Click += new EventHandler(this.GetObjectProps_Click);
            this.btnGetObjectData.Click += new EventHandler(this.GetObjectData_Click);
            this.btnQueryExecute.Click += new EventHandler(this.QueryExecute_Click);
            this.Load += new EventHandler(this.Page_Load);
            //			ModuleTitle = new DesktopModuleTitle();
            //			Controls.AddAt(0, ModuleTitle);
            base.OnInit(e);
        }

        #endregion
    }
}