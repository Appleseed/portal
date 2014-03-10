// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModuleDefinitions.aspx.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   Add/Remove modules, assign OneFileModules to portals
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.AdminAll
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Web.UI.WebControls;

    using Appleseed.Framework;
    using Appleseed.Framework.Site.Data;
    using Appleseed.Framework.Web.UI;

    using Label = Appleseed.Framework.Web.UI.WebControls.Label;

    /// <summary>
    /// Add/Remove modules, assign OneFileModules to portals
    /// </summary>
    public partial class ModuleDefinitions_OFM : EditItemPage
    {
        #region Constants and Fields

        /// <summary>
        /// The installer file name.
        /// </summary>
        protected TextBox InstallerFileName;

        /// <summary>
        /// The label 7.
        /// </summary>
        protected Label Label7;

        /// <summary>
        /// The required field validator 1.
        /// </summary>
        protected RequiredFieldValidator Requiredfieldvalidator1;

        /// <summary>
        /// The add module.
        /// </summary>
        private bool addModule;

        /// <summary>
        /// The def id.
        /// </summary>
        private Guid defId;

        #endregion

        #region Properties

        /// <summary>
        ///   Set the module Guids with free access to this page
        /// </summary>
        /// <value>The allowed modules.</value>
        protected override List<string> AllowedModules
        {
            get
            {
                var al = new List<string> { "D04BB5EA-A792-4E87-BFC7-7D0ED3AD1234" };
                return al;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete a Module definition
        /// </summary>
        /// <param name="e">
        /// The <see cref="T:System.EventArgs"/> instance containing the event data.
        /// </param>
        protected override void OnDelete(EventArgs e)
        {
            try
            {
                var modules = new ModulesDB();
                modules.DeleteModuleDefinition(new Guid(this.ModuleGuid.Text));

                // Redirect back to the portal admin page
                this.RedirectBackToReferringPage();
            }
            catch (ThreadAbortException)
            {
                // normal with redirect 
            }
            catch (Exception ex)
            {
                this.lblErrorDetail.Text = General.GetString(
                    "MODULE_DEFINITIONS_DELETE_ERROR", "An error occurred deleting module.", this);
                this.lblErrorDetail.Visible = true;
                ErrorHandler.Publish(LogLevel.Error, this.lblErrorDetail.Text, ex);
            }
        }

        /// <summary>
        /// Handles OnInit event
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            // if (PortalSecurity.IsInRoles("Admins") == false) 
            //     PortalSecurity.AccessDeniedEdit();

            // Calculate security defID
            if (this.Request.Params["DefID"] != null)
            {
                this.defId = new Guid(this.Request.Params["DefID"]);
            }

            if (this.Request.Params["defID"] == null)
            {
                this.addModule = true;
            }

            this.defId = Guid.NewGuid();

            // new module definition
            this.FriendlyName.Text = "My New Module";
            this.DesktopSrc.Text = @"DesktopModules/MyNewModule.ascx";
            this.MobileSrc.Text = string.Empty;
            this.ModuleGuid.Text = this.defId.ToString();
            
            this.UpdateButton.Click += this.UpdateButtonClick;
            this.DeleteButton.Click += this.DeleteButtonClick;
            this.selectAllButton.Click += this.SelectAllButtonClick;
            this.selectNoneButton.Click += this.SelectNoneButtonClick;
            
            base.OnInit(e);
        }

        /// <summary>
        /// OnUpdate installs or refresh module definition on db
        /// </summary>
        /// <param name="e">
        /// The <see cref="T:System.EventArgs"/> instance containing the event data.
        /// </param>
        protected override void OnUpdate(EventArgs e)
        {
            if (this.Page.IsValid)
            {
                try
                {
                    var modules = new ModulesDB();
                    modules.AddGeneralModuleDefinitions(
                        new Guid(this.ModuleGuid.Text), 
                        this.FriendlyName.Text, 
                        this.DesktopSrc.Text, 
                        this.MobileSrc.Text, 
                        "Appleseed.Modules.OneFileModule.dll", 
                        "Appleseed.Content.Web.ModulesOneFileModule", 
                        false, 
                        false);

                    // Update the module definition
                    for (var i = 0; i < this.PortalsName.Items.Count; i++)
                    {
                        modules.UpdateModuleDefinitions(
                            this.defId, 
                            Convert.ToInt32(this.PortalsName.Items[i].Value), 
                            this.PortalsName.Items[i].Selected);
                    }

                    // Redirect back to the portal admin page
                    this.RedirectBackToReferringPage();
                }
                catch (ThreadAbortException)
                {
                    // normal with redirect 
                }
                catch (Exception ex)
                {
                    this.lblErrorDetail.Text =
                        string.Format("{0}<br />", General.GetString("MODULE_DEFINITIONS_INSTALLING", "An error occurred installing.", this));
                    this.lblErrorDetail.Text += string.Format("{0}<br />", ex.Message);
                    this.lblErrorDetail.Text += string.Format(" Module: '{0}' - Source: '{1}' - Mobile: '{2}'", this.FriendlyName.Text, this.DesktopSrc.Text, this.MobileSrc.Text);
                    this.lblErrorDetail.Visible = true;

                    ErrorHandler.Publish(LogLevel.Error, this.lblErrorDetail.Text, ex);
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            var modules = new ModulesDB();

            // If this is the first visit to the page, bind the definition data 
            if (this.Page.IsPostBack)
            {
                this.addModule = bool.Parse(this.ViewState["addModule"].ToString());
            }
            else
            {
                this.ViewState["addModule"] = this.addModule;
                if (this.addModule)
                {
                    this.DeleteButton.Visible = false;
                }
                else
                {
                    // Obtain the module definition to edit from the database
                    var def = modules.GetSingleModuleDefinition(this.defId);

                    // WLF: Set UI values
                    this.FriendlyName.Text = def.FriendlyName;
                    this.DesktopSrc.Text = def.DesktopSource;
                    this.MobileSrc.Text = def.MobileSource;
                    this.ModuleGuid.Text = def.GeneralModDefID.ToString();
                    this.ModuleGuid.Enabled = false;
                }

                // Clear existing items in checkbox list
                this.PortalsName.Items.Clear();

                // Populate checkbox list with all portals
                // and "check" the ones already configured for this tab
                var portals = modules.GetModuleInUse(this.defId).ToArray();
                this.PortalsName.Items.AddRange(portals);
            }
        }

        /// <summary>
        /// Handles the Click event of the deleteButton control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="T:System.EventArgs"/> instance containing the event data.
        /// </param>
        private void DeleteButtonClick(object sender, EventArgs e)
        {
            this.OnDelete(e);
        }

        /// <summary>
        /// Handles the Click event of the selectAllButton control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="T:System.EventArgs"/> instance containing the event data.
        /// </param>
        private void SelectAllButtonClick(object sender, EventArgs e)
        {
            for (var i = 0; i < this.PortalsName.Items.Count; i++)
            {
                this.PortalsName.Items[i].Selected = true;
            }
        }

        /// <summary>
        /// Handles the Click event of the selectNoneButton control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="T:System.EventArgs"/> instance containing the event data.
        /// </param>
        private void SelectNoneButtonClick(object sender, EventArgs e)
        {
            for (var i = 0; i < this.PortalsName.Items.Count; i++)
            {
                this.PortalsName.Items[i].Selected = false;
            }
        }

        /// <summary>
        /// Handles the Click event of the updateButton control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="T:System.EventArgs"/> instance containing the event data.
        /// </param>
        private void UpdateButtonClick(object sender, EventArgs e)
        {
            this.OnUpdate(e);
        }

        #endregion
    }
}