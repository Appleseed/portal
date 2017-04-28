using Appleseed.Framework;
using Appleseed.Framework.Providers.ConnectedSourcesProvider;
using Appleseed.Framework.Web.UI.WebControls;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Appleseed.DesktopModules.CoreModules.AdminConnectedSources
{
    public partial class SearchAdminSettingsModule : PortalModuleControl
    //public partial class SearchAdminSettingsModule : System.Web.UI.UserControl
    {
        private const string CASSANDRAHOST_KEY = "MODULE_SPECIAL_SETTING_CASSANDRA_HOST";
        private CassandraProvider csProvider = new CassandraProvider("");

        public SearchAdminSettingsModule()
        {
            var group = SettingItemGroup.MODULE_SPECIAL_SETTINGS;
            var groupOrderBase = (int)SettingItemGroup.MODULE_SPECIAL_SETTINGS;

            // If false the input box for mobile content will be hidden
            var cassandraHost = new SettingItem<string, TextBox>
            {
                Value = "",
                Order = groupOrderBase + 5,
                Group = group,
                EnglishName = "Cassandra Host",
                Description = "Cassandra Connect Host e.g. localhost"
            };
            this.BaseSettings.Add(CASSANDRAHOST_KEY, cassandraHost);
        }

        /// <summary>
        ///   General Module Def GUID
        /// </summary>
        /// <value></value>
        public override Guid GuidID
        {
            get
            {
                return new Guid("{28E871BE-1A0C-4F05-99C4-14681D5FD02A}");
            }
        }

        public void LoadConnectHostSetting()
        {
            try
            {
                if (this.Settings[CASSANDRAHOST_KEY] != null && this.Settings[CASSANDRAHOST_KEY].Value != null && !string.IsNullOrEmpty(this.Settings[CASSANDRAHOST_KEY].Value.ToString()))
                {
                    csProvider = new CassandraProvider(this.Settings[CASSANDRAHOST_KEY].Value.ToString());
                }
            }
            catch (Exception ex)
            {

                ErrorHandler.Publish(LogLevel.Error, "Error while loading ConnectHost from setting. " + ex.Message, ex);
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadConnectHostSetting();

            if (!IsPostBack)
            {
                LoadEngineTypes();
            }
        }

        private void LoadEngineTypes()
        {
            ddlTypes.DataSource = csProvider.GetEngineTypes();
            ddlTypes.DataTextField = "name";
            ddlTypes.DataValueField = "id";
            ddlTypes.DataBind();
            ddlTypes.Items.Insert(0, new ListItem() { Value = "", Text = "Select Type" });
        }

        private void LoadEngines()
        {
            ddlEngines.DataSource = csProvider.GetEngines(Guid.Parse(ddlTypes.SelectedValue));
            ddlEngines.DataTextField = "name";
            ddlEngines.DataValueField = "id";
            ddlEngines.DataBind();
            ddlEngines.Items.Insert(0, new ListItem() { Value = "", Text = "Select Engine" });
            ddlEngines.Items.Add(new ListItem() { Value = Guid.Empty.ToString(), Text = "Add New Engine" });
            this.btnEngineDelete.Visible = false;
        }

        protected void ddlTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.plcMain.Visible = false;
            if (ddlTypes.SelectedIndex > 0)
            {
                this.plcMain.Visible = true;
                LoadEngines();
            }
        }

        protected void ddlEngines_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.plcEngineAdd.Visible = false;
            this.plcEngineItems.Visible = false;
            this.btnEngineDelete.Visible = false;
            if (ddlEngines.SelectedValue == Guid.Empty.ToString())
            {
                this.plcEngineAdd.Visible = true;
                this.txtEngineName.Text = string.Empty;
            }
            else
            {
                this.btnEngineDelete.Visible = true;
                this.plcEngineItems.Visible = true;
                LoadEnginItems();
            }
        }

        protected void btnAddEngine_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                csProvider.AddNewEngine(new Engine() { name = txtEngineName.Text.Trim(), typeid = Guid.Parse(ddlTypes.SelectedValue) });
                LoadEngines();
                this.plcEngineAdd.Visible = false;
            }
        }

        protected void cvUniuqeName_ServerValidate(object source, ServerValidateEventArgs args)
        {
            var engExs = csProvider.GetEngines().FirstOrDefault(eng => eng.name.ToLower() == txtEngineName.Text.Trim().ToLower());
            if (engExs != null)
            {
                args.IsValid = false;
            }
        }

        protected void btnEngineDelete_Click(object sender, EventArgs e)
        {
            csProvider.DeleteEngine(new Engine() { id = Guid.Parse(ddlEngines.SelectedValue) });
            LoadEngines();
            this.plcEngineAdd.Visible = false;
            this.plcEngineItems.Visible = false;
        }

        protected void btnAddnewEngineItem_Click(object sender, EventArgs e)
        {
            this.plcAddEditEngineItem.Visible = true;
            this.txtCollectioName.Text = string.Empty;
            this.txtEngineItemName.Text = string.Empty;
            this.txtIndexPath.Text = string.Empty;
            this.txtLocationUrl.Text = string.Empty;
            this.txtType.Text = string.Empty;
            this.hdnEngineItemId.Value = Guid.Empty.ToString();
            this.plcAddEditEngineItem.Visible = true;
            this.plcEngineItems.Visible = false;
        }

        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            var lnk = (System.Web.UI.WebControls.LinkButton)(sender);
            var engItm = csProvider.GetEngineItem(Guid.Parse(lnk.CommandArgument));
            this.txtCollectioName.Text = engItm.collectionname;
            this.txtEngineItemName.Text = engItm.name;
            this.txtIndexPath.Text = engItm.indexpath;
            this.txtLocationUrl.Text = engItm.locationurl;
            this.txtType.Text = engItm.type;
            this.hdnEngineItemId.Value = engItm.id.ToString();
            this.plcAddEditEngineItem.Visible = true;
            this.plcEngineItems.Visible = false;
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            var lnk = (System.Web.UI.WebControls.LinkButton)(sender);
            csProvider.DeleteEngineItem(new EngineItem() { id = Guid.Parse(lnk.CommandArgument) });
            LoadEnginItems();
            hdnEngineItemId.Value = string.Empty;
            this.plcAddEditEngineItem.Visible = false;
            this.plcEngineItems.Visible = true;
        }

        private void LoadEnginItems()
        {
            gvEngineItems.DataSource = csProvider.GetEngineItems(new Engine() { id = Guid.Parse(ddlEngines.SelectedValue) });
            gvEngineItems.DataBind();
        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            var engExs = csProvider.GetEngineItems(new Engine() { id = Guid.Parse(ddlEngines.SelectedValue) }).FirstOrDefault(eng => eng.name.ToLower() == txtEngineItemName.Text.Trim().ToLower() && eng.id != Guid.Parse(hdnEngineItemId.Value));
            if (engExs != null)
            {
                args.IsValid = false;
            }
        }

        protected void btnSaveEngineItem_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (hdnEngineItemId.Value == Guid.Empty.ToString())
                {
                    csProvider.AddNewEngineItem(new EngineItem()
                    {
                        collectionname = txtCollectioName.Text,
                        engineid = Guid.Parse(ddlEngines.SelectedValue),
                        indexpath = txtIndexPath.Text,
                        locationurl = txtLocationUrl.Text,
                        type = txtType.Text,
                        name = txtEngineItemName.Text.Trim()
                    });
                }
                else
                {
                    csProvider.UpdateEngineItem(new EngineItem()
                    {
                        id = Guid.Parse(hdnEngineItemId.Value),
                        collectionname = txtCollectioName.Text,
                        engineid = Guid.Parse(ddlEngines.SelectedValue),
                        indexpath = txtIndexPath.Text,
                        locationurl = txtLocationUrl.Text,
                        type = txtType.Text,
                        name = txtEngineItemName.Text.Trim()
                    });
                }
                hdnEngineItemId.Value = string.Empty;
                LoadEnginItems();
                this.plcAddEditEngineItem.Visible = false;
                this.plcEngineItems.Visible = true;
            }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            hdnEngineItemId.Value = string.Empty;
            LoadEnginItems();
            this.plcAddEditEngineItem.Visible = false;
            this.plcEngineItems.Visible = true;
        }
    }
}