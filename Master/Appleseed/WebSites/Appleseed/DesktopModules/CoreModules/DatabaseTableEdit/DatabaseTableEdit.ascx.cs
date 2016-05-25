using System;
using System.Data;
using System.Data.SqlClient;
using Appleseed.Framework;
using Appleseed.Framework.DataTypes;
using Appleseed.Framework.Web.UI.WebControls;

namespace Appleseed.Content.Web.Modules
{
    using System.Web.UI.WebControls;

    /// <summary>
    /// Database Table Edit Module
    /// Based on control from TripelASP (code is free)
    /// Original programmer: ? (Manu knows)
    /// Modifications by Jakob hansen
    /// </summary>
    public partial class DatabaseTableEdit : PortalModuleControl
    {
        protected bool Connected = false;
        protected string ConnectionString;
        protected bool Trusted_Connection;
        protected string ServerName;
        protected string DatabaseName;
        protected string UserID;
        protected string Password;

        protected int MaxStringLength;
        protected bool AllowPaging;
        protected int PageSize;


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
            MaxStringLength = int.Parse(Settings["MaxStringLength"].ToString());
            AllowPaging = "True" == Settings["AllowPaging"].ToString();
            PageSize = int.Parse(Settings["PageSize"].ToString());

            if (Trusted_Connection)
                ConnectionString = "Server=" + ServerName + ";Trusted_Connection=true;database=" + DatabaseName;
            else
                ConnectionString = "Server=" + ServerName + ";database=" + DatabaseName + ";uid=" + UserID + ";pwd=" +
                                   Password + ";";

            Connected = Connect(ConnectionString);

            panConnected.Visible = Connected;
            if (Connected)
            {
                lblConnectedError.Visible = false;

                tableeditor.ConnectionString = ConnectionString;
                tableeditor.ConfigConnectionString = ConnectionString;
                tableeditor.MaxStringLength = MaxStringLength;
                tableeditor.AllowPaging = AllowPaging;
                tableeditor.PageSize = PageSize;
            }
            else
            {
                lblConnectedError.Visible = true;
                // Added EsperantusKeys for Localization --%>
                // Mario Endara mario@softworks.com.uy 11/05/2004 --%>
                lblConnectedError.Text = General.GetString("TABLEEDIT_MSG_CONNECT");
            }

            if (!Page.IsPostBack)
            {
                BindTables();
            }
        }


        /// <summary>
        /// Connects the specified con STR.
        /// </summary>
        /// <param name="ConStr">The con STR.</param>
        /// <returns></returns>
        protected bool Connect(string ConStr)
        {
            bool retValue;
            try
            {
                SqlConnection SqlCon = new SqlConnection(ConStr);
                SqlDataAdapter DA = new SqlDataAdapter("SELECT NULL", SqlCon);
                SqlCon.Open();

                SqlCon.Close();
                retValue = true;
            }
            catch (Exception)
            {
                retValue = false;
            }
            return retValue;
        }


        /// <summary>
        /// Binds the tables.
        /// </summary>
        private void BindTables()
        {
            string sql =
                @"	Select so.name, so.id From sysobjects so 
                            where xtype = 'U' AND so.name <> 'dtproperties'
                            order by so.name";

            SqlCommand cmd = new SqlCommand(sql, new SqlConnection(ConnectionString));
            try
            {
                cmd.Connection.Open();

                tablelist.DataSource = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                tablelist.DataValueField = "name";
                tablelist.DataTextField = "name";
                tablelist.DataBind();
                // Added EsperantusKeys for Localization --%>
                // Mario Endara mario@softworks.com.uy 11/05/2004 --%>
                tablelist.Items.Insert(0, General.GetString("TABLEEDIT_SELECT_TABLE"));
            }
            catch (Exception ex)
            {
                lblConnectedError.Visible = true;
                lblConnectedError.Text = ex.Message;
            }
            cmd.Connection.Close(); // Added by Ashish - Connection Pool Issue
        }


        /// <summary>
        /// Handles the SelectedIndexChanged event of the tablelist control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected void tablelist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tablelist.SelectedIndex > 0)
            {
                tableeditor.Table = tablelist.SelectedItem.Value.ToString();
            }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseTableEdit"/> class.
        /// </summary>
        public DatabaseTableEdit()
        {
            var Trusted_Connection = new SettingItem<bool, CheckBox>();
            Trusted_Connection.Order = 1;
            //Trusted_Connection.Required = true;   // hmmm... problem here! Dont set to true!" 
            Trusted_Connection.Value = true;
            this.BaseSettings.Add("Trusted Connection", Trusted_Connection);

            var ServerName = new SettingItem<string, TextBox>();
            ServerName.Order = 2;
            ServerName.Required = true;
            ServerName.Value = "localhost";
            this.BaseSettings.Add("ServerName", ServerName);

            var DatabaseName = new SettingItem<string, TextBox>();
            DatabaseName.Order = 3;
            DatabaseName.Required = true;
            DatabaseName.Value = "Appleseed";
            this.BaseSettings.Add("DatabaseName", DatabaseName);

            var UserID = new SettingItem<string, TextBox>();
            UserID.Order = 4;
            UserID.Required = false;
            UserID.Value = "sa";
            this.BaseSettings.Add("UserID", UserID);

            var Password = new SettingItem<string, TextBox>();
            Password.Order = 5;
            Password.Required = false;
            Password.Value = string.Empty;
            this.BaseSettings.Add("Password", Password);

            var MaxStringLength = new SettingItem<int, TextBox>();
            MaxStringLength.Order = 6;
            MaxStringLength.Required = true;
            MaxStringLength.Value = 100;
            this.BaseSettings.Add("MaxStringLength", MaxStringLength);

            var AllowPaging = new SettingItem<bool, CheckBox>();
            AllowPaging.Order = 7;
            //AllowPaging.Required = true;   // hmmm... problem here! Dont set to true!" 
            AllowPaging.Value = true;
            this.BaseSettings.Add("AllowPaging", AllowPaging);

            var PageSize = new SettingItem<int, TextBox>();
            PageSize.Order = 8;
            PageSize.Required = true;
            PageSize.Value = 10;
            this.BaseSettings.Add("PageSize", PageSize);
        }


        /// <summary>
        /// GUID of module (mandatory)
        /// </summary>
        /// <value></value>
        public override Guid GuidID
        {
            get { return new Guid("{AB02A3F4-A0A4-45e0-96ED-8450C19166C5}"); }
        }

        /// <summary>
        /// Admin Module
        /// </summary>
        /// <value></value>
        public override bool AdminModule
        {
            get { return true; }
        }

        #region Web Form Designer generated code

        /// <summary>
        /// Raises OnInit Event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            this.tablelist.SelectedIndexChanged += new EventHandler(this.tablelist_SelectedIndexChanged);
            this.Load += new EventHandler(this.Page_Load);

            // Create a new Title the control
//			ModuleTitle = new DesktopModuleTitle();
            // Add title ad the very beginning of 
            // the control's controls collection
//			Controls.AddAt(0, ModuleTitle);

            base.OnInit(e);
        }

        #endregion
    }
}