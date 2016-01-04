using System;
using System.Collections;
using System.IO;
using Appleseed.Framework;
using Appleseed.Framework.Content.Data;
using Appleseed.Framework.Data;
using Appleseed.Framework.DataTypes;
using Appleseed.Framework.Web.UI.WebControls;

namespace Appleseed.Content.Web.Modules
{
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    public partial class ContentManager : PortalModuleControl
    {
        /// <summary>
        /// The Page_Load event handler on this User Control populates the comboboxes
        /// for portals and module types for the ContentManager.
        /// It uses the Appleseed.ContentManagerDB()
        /// data component to encapsulate all data functionality.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //populate module types dropdownlist.
                ContentManagerDB contentDB = new ContentManagerDB();

                //populate moduleTypes list
                ModuleTypes.DataSource = contentDB.GetModuleTypes();
                ModuleTypes.DataValueField = "ItemID";
                ModuleTypes.DataTextField = "FriendlyName";
                ModuleTypes.DataBind();

                //populate source portal list
                SourcePortal.DataValueField = "PortalID";
                SourcePortal.DataTextField = "PortalAlias";
                SourcePortal.DataSource = contentDB.GetPortals();
                SourcePortal.DataBind();

                //destination portal list.
                DestinationPortal.DataValueField = "PortalID";
                DestinationPortal.DataTextField = "PortalAlias";
                DestinationPortal.DataSource = contentDB.GetPortals();
                DestinationPortal.DataBind();

                //Function to set visibility for Portal dropdowns and select current portal
                //as default
                MultiPortalSupport();

                //functions to load the modules in the currently selected portal.
                LoadSourceModules();
                LoadDestinationModules();
            }
        }

        #region System Event Handlers for dropdownlists

        /// <summary>
        /// The ModuleTypesChanged event handler on this User Control fires when the
        /// Moduletypes dropdownlist has been changed(ex. Announcements to FAQ).
        /// The event then refreshes the source modules for that type and the destination
        /// modules for htat type.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void ModuleTypesChanged(object sender, EventArgs e)
        {
            //does not change portals!!
            LoadSourceModules();
            LoadDestinationModules();
        }

        /// <summary>
        /// The SourcePortalChanged event handler on this User Control fires when the
        /// SourcePortal dropdownlist has been changed(ex. Portal Instance 1 changed to 2).
        /// The event then refreshes the source modules for that type.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void SourcePortalChanged(object sender, EventArgs e)
        {
            //refresh source instances, destination instances should stay same?
            LoadSourceModules();
        }

        /// <summary>
        /// The DestinationPortalChanged event handler on this User Control fires when the
        /// DestinationPortal dropdownlist has been changed
        /// The event then refreshes the modules for that portal instance
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void DestinationPortalChanged(object sender, EventArgs e)
        {
            LoadDestinationModules();
        }

        /// <summary>
        /// The source instance is changed.  Load the data for that instance, and also
        /// This means reload the destination instances also so that the old source
        /// instance is now a potential destination.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void SourceInstanceChanged(object sender, EventArgs e)
        {
            LoadSourceModuleData();
            LoadDestinationModules();
        }

        /// <summary>
        /// same SourceInstanceChanged only in reverse.  Load data for the destination
        /// instance instead of source instance, and reload source instances.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void DestinationInstanceChanged(object sender, EventArgs e)
        {
            LoadDestinationModuleData();
        }

        #endregion

        #region MultiPortalSupport

        /// <summary>
        /// This function does 2 things.
        /// First:  sets visibility of source portal/destination portal table to true/false
        /// depending on if the Module Configuration Settings have multiportalsupport
        /// enabled.
        /// Second:  Selects the current portal instance as the default in the listboxes.
        /// even if the listboxes are invisible to the user they need to have valid data!!
        /// </summary>
        private void MultiPortalSupport()
        {
            bool enabled = bool.Parse(Settings["MultiPortalSupport"].ToString());
            MultiPortalTable.Visible = enabled;

            //whether visible or not, we need to make sure that the proper sourceportal
            //and destination portal are selected.  Default == running portal instance.
            //This function iterates through the portals listed in SourcePortal listbox
            //by changing the SelectedItem.Value until the current portal instance running
            //is selected in the SourcePortal dropdownlist
            for (int i = 0; i < SourcePortal.Items.Count; i++)
            {
                SourcePortal.SelectedIndex = i;
                DestinationPortal.SelectedIndex = i;

                if (SourcePortal.SelectedItem.Value == (this.PortalSettings.PortalID).ToString())
                    return;
            } //end for
        }

        #endregion

        #region Data binding Functions

        /// <summary>
        /// Loads the source modules.
        /// </summary>
        private void LoadSourceModules()
        {
            if (ModuleTypes.SelectedIndex > -1)
            {
                int ModuleTypeID = Int32.Parse(ModuleTypes.SelectedItem.Value);

                ContentManagerDB contentDB = new ContentManagerDB();

                SourceInstance.DataValueField = "ModuleID";
                SourceInstance.DataTextField = "TabModule";

                SourceInstance.DataSource =
                    contentDB.GetModuleInstances(ModuleTypeID, Int32.Parse(SourcePortal.SelectedItem.Value));
                SourceInstance.DataBind();

                //if items exist in the sourceinstance, select the first item
                if (SourceInstance.Items.Count > 0)
                {
                    SourceInstance.SelectedIndex = 0;
                    LoadSourceModuleData();
                }
                else
                {
                    //if there are no instances, there can be no data!!
                    SourceListBox.Items.Clear();
                }
            }
        }

        /// <summary>
        /// Loads the source module data.
        /// </summary>
        private void LoadSourceModuleData()
        {
            //check to be sure that a source instance has been selected before proceeding.
            //this can cause errors if not checked for!
            if (SourceInstance.SelectedIndex > -1)
            {
                int SourceModID = Int32.Parse(SourceInstance.SelectedItem.Value);
                int ModuleTypeID = Int32.Parse(ModuleTypes.SelectedItem.Value);

                ContentManagerDB contentDB = new ContentManagerDB();
                SourceListBox.DataValueField = "ItemID";
                SourceListBox.DataTextField = "ItemDesc";
                SourceListBox.DataSource = contentDB.GetSourceModuleData(ModuleTypeID, SourceModID);
                SourceListBox.DataBind();
                //Added by Ashish - Connection Pool Issues
                contentDB.GetSourceModuleData(ModuleTypeID, SourceModID).Close();
            }
        }

        /// <summary>
        /// Loads the destination modules.
        /// </summary>
        private void LoadDestinationModules()
        {
            if (ModuleTypes.SelectedIndex > -1 && SourceInstance.Items.Count > 0)
            {
                //Get the Module Type(ex announcements) and the Source ModuleID
                int ModuleTypeID = Int32.Parse(ModuleTypes.SelectedItem.Value);
                int SourceModID = Int32.Parse(SourceInstance.SelectedItem.Value);

                ContentManagerDB contentDB = new ContentManagerDB();

                DestinationInstance.DataValueField = "ModuleID";
                DestinationInstance.DataTextField = "TabModule";
                DestinationInstance.DataSource = contentDB.GetModuleInstancesExc(ModuleTypeID,
                                                                                 SourceModID,
                                                                                 Int32.Parse(
                                                                                     DestinationPortal.SelectedItem.
                                                                                         Value));
                DestinationInstance.DataBind();

                //if any items exist in destination instance dropdown, select first and
                //load data for that instance.
                if (DestinationInstance.Items.Count > 0)
                {
                    DestinationInstance.SelectedIndex = 0;
                    LoadDestinationModuleData();
                }
                else
                {
                    DestListBox.Items.Clear();
                }
            }
        }

        /// <summary>
        /// Loads the destination module data.
        /// </summary>
        private void LoadDestinationModuleData()
        {
            if (DestinationInstance.SelectedIndex > -1)
            {
                int DestModID = Int32.Parse(DestinationInstance.SelectedItem.Value);
                int ModuleTypeID = Int32.Parse(ModuleTypes.SelectedItem.Value);

                ContentManagerDB contentDB = new ContentManagerDB();
                DestListBox.DataValueField = "ItemID";
                DestListBox.DataTextField = "ItemDesc";
                DestListBox.DataSource = contentDB.GetDestModuleData(ModuleTypeID, DestModID);
                DestListBox.DataBind();
            }
        }

        #endregion

        #region BUTTON EVENTS

        /// <summary>
        /// Handles the Click event of the MoveLeft control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void MoveLeft_Click(object sender, EventArgs e)
        {
            if (DestListBox.SelectedIndex > -1 && SourceInstance.SelectedIndex > -1 &&
                DestinationInstance.SelectedIndex > -1)
            {
                int ModuleTypeID = Int32.Parse(ModuleTypes.SelectedItem.Value);
                //source module ID not needed.

                //these two lines opposite in MoveRight_Click
                int DestModID = Int32.Parse(SourceInstance.SelectedItem.Value);
                int ItemToMove = Int32.Parse(DestListBox.SelectedItem.Value);

                ContentManagerDB contentDB = new ContentManagerDB();
                contentDB.MoveItemLeft(ModuleTypeID, ItemToMove, DestModID);
                LoadSourceModuleData();
                LoadDestinationModuleData();
            }
        }

        /// <summary>
        /// Handles the Click event of the MoveRight control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void MoveRight_Click(object sender, EventArgs e)
        {
            if (SourceListBox.SelectedIndex > -1 && SourceInstance.SelectedIndex > -1 &&
                DestinationInstance.SelectedIndex > -1)
            {
                int ModuleTypeID = Int32.Parse(ModuleTypes.SelectedItem.Value);

                //these two lines opposite in MoveLeft_Click
                int DestModID = Int32.Parse(DestinationInstance.SelectedItem.Value);
                int ItemToMove = Int32.Parse(SourceListBox.SelectedItem.Value);

                ContentManagerDB contentDB = new ContentManagerDB();
                contentDB.MoveItemRight(ModuleTypeID, ItemToMove, DestModID);
                LoadSourceModuleData();
                LoadDestinationModuleData();
            }
        }

        /// <summary>
        /// Handles the Click event of the CopyRight control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void CopyRight_Click(object sender, EventArgs e)
        {
            if (SourceListBox.SelectedIndex > -1 && SourceInstance.SelectedIndex > -1 &&
                DestinationInstance.SelectedIndex > -1)
            {
                int ModuleTypeID = Int32.Parse(ModuleTypes.SelectedItem.Value);
                int DestModID = Int32.Parse(DestinationInstance.SelectedItem.Value);
                int ItemToMove = Int32.Parse(SourceListBox.SelectedItem.Value);

                ContentManagerDB contentDB = new ContentManagerDB();
                contentDB.CopyItem(ModuleTypeID, ItemToMove, DestModID);
                LoadDestinationModuleData();
            }
        }

        /// <summary>
        /// Handles the Click event of the CopyAll control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void CopyAll_Click(object sender, EventArgs e)
        {
            if (SourceListBox.Items.Count > 0 && SourceInstance.SelectedIndex > -1 &&
                DestinationInstance.SelectedIndex > -1)
            {
                int ModuleTypeID = Int32.Parse(ModuleTypes.SelectedItem.Value);
                int DestModID = Int32.Parse(DestinationInstance.SelectedItem.Value);
                int SourceModID = Int32.Parse(SourceInstance.SelectedItem.Value);

                ContentManagerDB contentDB = new ContentManagerDB();
                contentDB.CopyAll(ModuleTypeID, SourceModID, DestModID);
                LoadDestinationModuleData();
            }
        }

        /// <summary>
        /// Handles the Click event of the DeleteLeft control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void DeleteLeft_Click(object sender, EventArgs e)
        {
            if (SourceListBox.SelectedIndex > -1)
            {
                int ModuleTypeID = Int32.Parse(ModuleTypes.SelectedItem.Value);
                int ItemToDelete = Int32.Parse(SourceListBox.SelectedItem.Value);

                ContentManagerDB contentDB = new ContentManagerDB();
                contentDB.DeleteItemLeft(ModuleTypeID, ItemToDelete);
                LoadSourceModuleData();
            }
        }

        /// <summary>
        /// Handles the Click event of the DeleteRight control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void DeleteRight_Click(object sender, EventArgs e)
        {
            if (DestListBox.SelectedIndex > -1)
            {
                int ModuleTypeID = Int32.Parse(ModuleTypes.SelectedItem.Value);
                int ItemToDelete = Int32.Parse(DestListBox.SelectedItem.Value);

                ContentManagerDB contentDB = new ContentManagerDB();
                contentDB.DeleteItemRight(ModuleTypeID, ItemToDelete);
                LoadDestinationModuleData();
            }
        }

        #endregion

        #region AppleseedCode

        /// <summary>
        /// Admin Module
        /// </summary>
        /// <value></value>
        public override bool AdminModule
        {
            get { return true; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentManager"/> class.
        /// </summary>
        public ContentManager()
        {
            // setting item for show portals
            var showPortals = new SettingItem<bool, CheckBox>()
                {
                    Value = false,
                    Group = SettingItemGroup.MODULE_SPECIAL_SETTINGS,
                    Description = "Enable or Disable Multi-Portal Support"
                };
            this.BaseSettings.Add("MultiPortalSupport", showPortals);
        }

        /// <summary>
        /// GUID of module (mandatory)
        /// </summary>
        /// <value></value>
        public override Guid GuidID
        {
            get { return new Guid("{EDDD32E0-2135-4276-9157-3478995CCCD2}"); }
        }

        //#region Search Implementation
        /// <summary>
        /// Searchable module
        /// </summary>
        /// <value></value>
        public override bool Searchable
        {
            get { return false; }
        }


        /// <summary>
        /// Unknown
        /// </summary>
        /// <param name="stateSaver"></param>
        public override void Install(IDictionary stateSaver)
        {
            string currentScriptName = Path.Combine(Server.MapPath(TemplateSourceDirectory), "install.sql");

            List<string> errors = DBHelper.ExecuteScript(currentScriptName, true);
            if (errors.Count > 0)
            {
                // Call rollback
                throw new Exception("Error occurred:" + errors[0].ToString());
            }

            DirectoryInfo installDir =
                new DirectoryInfo(Path.Combine(Server.MapPath(TemplateSourceDirectory), "InstallScripts"));
            FileInfo[] installFiles = installDir.GetFiles("*_Install.sql");
            //foreach (FileInfo scriptToInstall in installFiles)
            //{
            //    currentScriptName = scriptToInstall.FullName;
            //    errors = DBHelper.ExecuteScript(currentScriptName, true);
            //    if (errors.Count > 0)
            //    {
            //        //call rollback
            //        throw new Exception("Error occured:" + errors[0].ToString());
            //    }
            //}
        }

        /// <summary>
        /// Unknown
        /// </summary>
        /// <param name="stateSaver"></param>
        public override void Uninstall(IDictionary stateSaver)
        {
            string currentScriptName = Path.Combine(Server.MapPath(TemplateSourceDirectory), "uninstall.sql");

            List<string> errors = DBHelper.ExecuteScript(currentScriptName, true);

            if (errors.Count > 0)
            {
                // Call rollback
                throw new Exception("Error occurred:" + errors[0].ToString());
            }

            DirectoryInfo installDir =
                new DirectoryInfo(Path.Combine(Server.MapPath(TemplateSourceDirectory), "InstallScripts"));
            FileInfo[] installFiles = installDir.GetFiles("*_uninstall.sql");
            foreach (FileInfo scriptToInstall in installFiles)
            {
                currentScriptName = scriptToInstall.FullName;
                errors = DBHelper.ExecuteScript(currentScriptName, true);
                if (errors.Count > 0)
                {
                    //call rollback
                    throw new Exception("Error occured:" + errors[0].ToString());
                }
            }
        }

        #endregion

        #region Web Form Designer generated code

        /// <summary>
        /// Raises Init event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            this.ModuleTypes.SelectedIndexChanged += new System.EventHandler(this.ModuleTypesChanged);
            this.DestinationPortal.SelectedIndexChanged += new System.EventHandler(this.DestinationPortalChanged);
            this.SourceInstance.SelectedIndexChanged += new System.EventHandler(this.SourceInstanceChanged);
            this.DestinationInstance.SelectedIndexChanged += new System.EventHandler(this.DestinationInstanceChanged);
            this.DeleteLeft_Btn.Click += new System.EventHandler(this.DeleteLeft_Click);
            this.MoveLeft_Btn.Click += new System.EventHandler(this.MoveLeft_Click);
            this.MoveRight_Btn.Click += new System.EventHandler(this.MoveRight_Click);
            this.CopyRight_Btn.Click += new System.EventHandler(this.CopyRight_Click);
            this.CopyAll_Btn.Click += new System.EventHandler(this.CopyAll_Click);
            this.DeleteRight_Btn.Click += new System.EventHandler(this.DeleteRight_Click);
            this.Load += new System.EventHandler(this.Page_Load);
            // Create a new Title the control
            AddUrl = "~/DesktopModules/CoreModules/ContentManager/ContentManagerEdit.aspx";

            base.OnInit(e);
        }

        #endregion
    }
}