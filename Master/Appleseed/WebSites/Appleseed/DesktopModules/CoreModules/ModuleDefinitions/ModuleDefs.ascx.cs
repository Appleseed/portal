using System;
using System.Data;
using System.Data.SqlClient;
using Appleseed.Framework.Site.Data;
using Appleseed.Framework.Web.UI.WebControls;

namespace Appleseed.Content.Web.Modules
{
    /// <summary>
    /// Module that shows available modules for current portal (readonly)
    /// </summary>
    public partial class ModuleDefs : PortalModuleControl
    {
        /// <summary>
        /// Admin Module
        /// </summary>
        /// <value></value>
        public override bool AdminModule
        {
            get { return true; }
        }

        /// <summary>
        /// The Page_Load server event handler on this user control is used
        /// to populate the current defs settings from the configuration system
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            // If this is the first visit to the page, bind the definition data to the datalist
            if (Page.IsPostBack == false)
            {
                BindData();
            }
        }

        /// <summary>
        /// The BindData helper method is used to bind the list of
        /// module definitions for this portal to an asp:datalist server control
        /// </summary>
        protected void BindData()
        {
            // Get the portal's defs from the database
            var modulelist = new ModulesDB().GetCurrentModuleDefinitions(this.PortalSettings.PortalID);

            DataTable userTable = new DataTable();
            userTable.Columns.Add(new DataColumn("FriendlyName", typeof(string)));
            userTable.Columns.Add(new DataColumn("ModuleDefID", typeof(string)));

            DataTable adminTable = new DataTable();
            adminTable.Columns.Add(new DataColumn("FriendlyName", typeof(string)));
            adminTable.Columns.Add(new DataColumn("ModuleDefID", typeof(string)));

            DataRow drow;
            //while (dr.Read())
            //{
            //    if (bool.Parse(dr["Admin"].ToString()))
            //    {
            //        drow = adminTable.NewRow();
            //        drow["ModuleDefID"] = dr["ModuleDefID"];
            //        string aux = dr["FriendlyName"].ToString();
            //        if (aux.StartsWith("Admin"))
            //        {
            //            aux = aux.Substring(5);
            //            while (aux[0] == ' ' || aux[0] == '-') aux = aux.Substring(1);
            //        }
            //        drow["FriendlyName"] = aux;
            //        adminTable.Rows.Add(drow);
            //    }
            //    else
            //    {
            //        drow = userTable.NewRow();
            //        drow["ModuleDefID"] = dr["ModuleDefID"];
            //        drow["FriendlyName"] = dr["FriendlyName"];
            //        userTable.Rows.Add(drow);
            //    }
            //}


            foreach (var item in modulelist)
            {

                if (item.Admin)
                {
                    drow = adminTable.NewRow();
                    drow["ModuleDefID"] = item.ModuleDefId;
                    string aux = item.FriendlyName;
                    if (aux.StartsWith("Admin"))
                    {
                        aux = aux.Substring(5);
                        while (aux[0] == ' ' || aux[0] == '-') aux = aux.Substring(1);
                    }
                    drow["FriendlyName"] = aux;
                    adminTable.Rows.Add(drow);
                }
                else
                {
                    drow = userTable.NewRow();
                    drow["ModuleDefID"] = item.ModuleDefId;
                    drow["FriendlyName"] =item.FriendlyName;
                    userTable.Rows.Add(drow);
                }
            }

            userModules.DataSource = userTable;
            userModules.DataBind();
            adminModules.DataSource = adminTable;
            adminModules.DataBind();
           // dr.Close();
        }

        /// <summary>
        /// GUID of module (mandatory)
        /// </summary>
        /// <value></value>
        public override Guid GuidID
        {
            get { return new Guid("{5E0DB0C7-FD54-4F55-ACF5-6ECF0EFA59C0}"); }
        }

        #region Web Form Designer generated code

        /// <summary>
        /// Raises OnInit Event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(this.Page_Load);
            base.OnInit(e);
        }

        #endregion
    }
}