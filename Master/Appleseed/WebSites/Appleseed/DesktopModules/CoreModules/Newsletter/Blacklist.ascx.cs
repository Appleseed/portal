using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;
using Appleseed.Framework;
using Appleseed.Framework.Content.Data;
using Appleseed.Framework.Data;
using Appleseed.Framework.DataTypes;
using Appleseed.Framework.Web.UI.WebControls;

namespace Appleseed.Admin
{
    using System.Collections.Generic;

    /// <summary>
    /// Blacklist Admin Module - Setup which users receive emails<br/>
    /// This module is typically used togeteher with the Newsletter module.
    /// Using the Blacklist module you can block some of the registred users 
    /// from receiving emails. Invalid emails are automatically blacklisted 
    /// by newsletter module to prevent further errors.
    /// </summary>
    /// <remarks>Written by: Jakob Hansen</remarks>
    public partial class Blacklist : PortalModuleControl
    {
        protected DataView myDataView;
        protected string sortField;
        protected string sortDirection;

        protected bool showColumnName, showColumnEmail, showColumnSendNewsletter;
        protected bool showColumnReason, showColumnDate;
        protected bool showAllUsers, showSubscribersOnly;

        /// <summary>
        /// Admin Module
        /// </summary>
        public override bool AdminModule
        {
            get { return true; }
        }

        /// <summary>
        /// The Page_Load event handler on this User Control is used to
        /// obtain a DataReader of blacklist information from the Users
        /// table, and then databind the results to a templated DataList
        /// server control. It uses the Appleseed.BlacklistDB()
        /// data component to encapsulate all data functionality.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Load(object sender, EventArgs e)
        {
            showColumnName = "True" == Settings["BLACKLIST_SHOWCOLUMNNAME"].ToString();
            showColumnEmail = "True" == Settings["BLACKLIST_SHOWCOLUMNEMAIL"].ToString();
            showColumnSendNewsletter = "True" == Settings["BLACKLIST_SHOWCOLUMNSENDNEWSLETTER"].ToString();
            showColumnReason = "True" == Settings["BLACKLIST_SHOWCOLUMNREASON"].ToString();
            showColumnDate = "True" == Settings["BLACKLIST_SHOWCOLUMNDATE"].ToString();
            showAllUsers = "True" == Settings["BLACKLIST_SHOWALLUSERS"].ToString();
            showSubscribersOnly = "True" == Settings["BLACKLIST_SHOWSUBSCRIBERSONLY"].ToString();

            if (Page.IsPostBack == false)
            {
                sortField = Settings["BLACKLIST_SORTFIELD"].ToString();
                sortDirection = "ASC";
                if (sortField == "LastSend" || sortField == "Date")
                    sortDirection = "DESC";
                ViewState["SortField"] = sortField;
                ViewState["sortDirection"] = sortDirection;
            }
            else
            {
                sortField = (string)ViewState["SortField"];
                sortDirection = (string)ViewState["sortDirection"];
            }


            BlacklistDB blacklist = new BlacklistDB();
            DataSet blist = blacklist.GetBlacklist(PortalID, showAllUsers, showSubscribersOnly);

            myDataView = new DataView();
            myDataView = blist.Tables[0].DefaultView;

            if (!Page.IsPostBack)
                myDataView.Sort = sortField + " " + sortDirection;

            BindGrid();
        }


        /// <summary>
        /// The SortList event handler sorts the list (a DataGrid control)
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void SortList(Object source, DataGridSortCommandEventArgs e)
        {
            if (sortField == e.SortExpression)
            {
                if (sortDirection == "ASC")
                    sortDirection = "DESC";
                else
                    sortDirection = "ASC";
            }
            else
            {
                if (e.SortExpression == "LastSend" || e.SortExpression == "Date")
                    sortDirection = "DESC";
                else
                    sortDirection = "ASC";
            }

            ViewState["SortField"] = e.SortExpression;
            ViewState["sortDirection"] = sortDirection;

            myDataView.Sort = e.SortExpression + " " + sortDirection;
            BindGrid();
        }

        private void BindGrid()
        {
            myDataGrid.DataSource = myDataView;
            myDataGrid.DataBind();

            if (!showColumnName)
                myDataGrid.Columns[0].Visible = false;
            if (!showColumnEmail)
                myDataGrid.Columns[1].Visible = false;
            if (!showColumnSendNewsletter)
                myDataGrid.Columns[2].Visible = false;
            if (!showColumnReason)
                myDataGrid.Columns[3].Visible = false;
            if (!showColumnDate)
                myDataGrid.Columns[4].Visible = false;
        }

        /// <summary>
        /// Guid
        /// </summary>
        public override Guid GuidID
        {
            get { return new Guid("{2502DB18-B580-4F90-8CB4-C15E6E531017}"); }
        }

        /// <summary>
        /// Public constructor. Sets base settings for module.
        /// </summary>
        public Blacklist()
        {
            var setSortField =
                new SettingItem<string, ListControl>(new ListDataType<string, ListControl>("Name;Email;SendNewsletter;Reason;Date"));
            setSortField.Required = true;
            setSortField.Value = "Name";
            setSortField.Order = 1;
            this.BaseSettings.Add("BLACKLIST_SORTFIELD", setSortField);

            var showColumnName = new SettingItem<bool, CheckBox>();
            showColumnName.Order = 2;
            showColumnName.Value = true;
            this.BaseSettings.Add("BLACKLIST_SHOWCOLUMNNAME", showColumnName);

            var showColumnEmail = new SettingItem<bool, CheckBox>();
            showColumnEmail.Order = 3;
            showColumnEmail.Value = true;
            this.BaseSettings.Add("BLACKLIST_SHOWCOLUMNEMAIL", showColumnEmail);

            var showColumnSendNewsletter = new SettingItem<bool, CheckBox>();
            showColumnSendNewsletter.Order = 4;
            showColumnSendNewsletter.Value = false;
            this.BaseSettings.Add("BLACKLIST_SHOWCOLUMNSENDNEWSLETTER", showColumnSendNewsletter);

            var showColumnReason = new SettingItem<bool, CheckBox>();
            showColumnReason.Order = 5;
            showColumnReason.Value = true;
            this.BaseSettings.Add("BLACKLIST_SHOWCOLUMNREASON", showColumnReason);

            var showColumnDate = new SettingItem<bool, CheckBox>();
            showColumnDate.Order = 6;
            showColumnDate.Value = true;
            this.BaseSettings.Add("BLACKLIST_SHOWCOLUMNDATE", showColumnDate);

            var showAllUsers = new SettingItem<bool, CheckBox>();
            showAllUsers.Order = 8;
            showAllUsers.Value = false;
            this.BaseSettings.Add("BLACKLIST_SHOWALLUSERS", showAllUsers);

            var showSubscribersOnly = new SettingItem<bool, CheckBox>();
            showSubscribersOnly.Order = 9;
            showSubscribersOnly.Value = false;
            this.BaseSettings.Add("BLACKLIST_SHOWSUBSCRIBERSONLY", showSubscribersOnly);
        }

        #region Web Form Designer generated code

        /// <summary>
        /// Raises OnInit event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            this.AddText = "BLACKLIST_ADD";
            this.AddUrl = "~/DesktopModules/CoreModules/Newsletter/BlacklistEdit.aspx";
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.Load += new EventHandler(this.Page_Load);
        }

        #endregion

        # region Install / Uninstall Implementation

        public override void Install(IDictionary stateSaver)
        {
            string currentScriptName = Server.MapPath(this.TemplateSourceDirectory + "/Blacklist_Install.sql");
            List<string> errors = DBHelper.ExecuteScript(currentScriptName, true);
            if (errors.Count > 0)
            {
                // Call rollback
                throw new Exception("Error occurred:" + errors[0].ToString());
            }
        }

        public override void Uninstall(IDictionary stateSaver)
        {
            string currentScriptName = Server.MapPath(this.TemplateSourceDirectory + "/Blacklist_Uninstall.sql");
            List<string> errors = DBHelper.ExecuteScript(currentScriptName, true);
            if (errors.Count > 0)
            {
                // Call rollback
                throw new Exception("Error occurred:" + errors[0].ToString());
            }
        }

        #endregion
    }
}