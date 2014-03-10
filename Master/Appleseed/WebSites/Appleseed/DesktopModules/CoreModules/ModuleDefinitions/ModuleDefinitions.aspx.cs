// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModuleDefinitions.aspx.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   Add/Remove modules, assign modules to portals
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.AdminAll
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Web.UI.WebControls;

    using Appleseed.Framework;
    using Appleseed.Framework.Core.Model;
    using Appleseed.Framework.Helpers;
    using Appleseed.Framework.Site.Data;
    using Appleseed.Framework.Web.UI;

    using Path = Appleseed.Framework.Settings.Path;

    /// <summary>
    /// Add/Remove modules, assign modules to portals
    /// </summary>
    public partial class ModuleDefinitions : EditItemPage
    {
        #region Constants and Fields

        /// <summary>
        ///   The def id.
        /// </summary>
        private Guid defId;

        #endregion

        #region Properties

        /// <summary>
        ///   Set the module guids with free access to this page
        /// </summary>
        /// <value>The allowed modules.</value>
        protected override List<string> AllowedModules
        {
            get
            {
                var al = new List<string> { "D04BB5EA-A792-4E87-BFC7-7D0ED3ADD582" };
                return al;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Changes install mode.
        /// </summary>
        /// <param name="mode">
        /// The edit mode.
        /// </param>
        public void ChangeInstallMode(EditMode mode)
        {
            this.portalsDiv.Visible = true;

            switch (mode)
            {
                case EditMode.Installer:
                    this.tableInstaller.Visible = true;
                    this.tableManual.Visible = false;
                    this.tableMVC.Visible = false;
                    this.tablePortableAreas.Visible = false;

                    this.btnUseInstaller.Visible = false;
                    this.btnDescription.Visible = true;
                    this.chbMVCAction.Visible = true;
                    this.DeleteButton.Visible = true;
                    this.chbPortableAreas.Visible = true;

                    break;
                case EditMode.Manually:
                    this.tableInstaller.Visible = false;
                    this.tableManual.Visible = true;
                    this.tableMVC.Visible = false;
                    this.tablePortableAreas.Visible = false;

                    this.btnUseInstaller.Visible = true;
                    this.btnDescription.Visible = false;
                    this.chbMVCAction.Visible = true;
                    this.DeleteButton.Visible = true;
                    this.chbPortableAreas.Visible = true;

                    break;
                case EditMode.MVC:
                    this.tableInstaller.Visible = false;
                    this.tableManual.Visible = false;
                    this.tableMVC.Visible = true;
                    this.tablePortableAreas.Visible = false;

                    this.btnUseInstaller.Visible = true;
                    this.btnDescription.Visible = true;
                    this.chbMVCAction.Visible = false;
                    this.DeleteButton.Visible = false;
                    this.chbPortableAreas.Visible = true;

                    break;
                case EditMode.PortableAreas:
                    this.tableInstaller.Visible = false;
                    this.tableManual.Visible = false;
                    this.tableMVC.Visible = false;
                    this.tablePortableAreas.Visible = true;

                    this.btnUseInstaller.Visible = true;
                    this.btnDescription.Visible = true;
                    this.chbMVCAction.Visible = true;
                    this.chbPortableAreas.Visible = false;

                    this.DeleteButton.Visible = false;
                    this.portalsDiv.Visible = false;
                    break;
                default:
                    break;
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
                if (!this.btnUseInstaller.Visible)
                {
                    ModuleInstall.UninstallGroup(
                        this.Server.MapPath(string.Format("{0}/{1}", Path.ApplicationRoot, this.InstallerFileName.Text)));
                }
                else
                {
                    ModuleInstall.Uninstall(this.DesktopSrc.Text, this.MobileSrc.Text);
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
                this.lblErrorDetail.Text = General.GetString(
                    "MODULE_DEFINITIONS_DELETE_ERROR", "An error occurred deleting module.", this);
                this.lblErrorDetail.Visible = true;
                ErrorHandler.Publish(LogLevel.Error, this.lblErrorDetail.Text, ex);
            }
        }

        /// <summary>
        /// Handles OnInit event
        /// </summary>
        /// <param name="e">
        /// An <see cref="T:System.EventArgs"></see> that contains the event data.
        /// </param>
        protected override void OnInit(EventArgs e)
        {
            var modules = new ModulesDB();

            // Calculate security defID
            if (this.Request.Params["defID"] != null)
            {
                this.defId = new Guid(this.Request.Params["defID"]);
            }

            if (this.defId.Equals(Guid.Empty))
            {
                this.ChangeInstallMode(EditMode.Installer);

                // new module definition
                this.InstallerFileName.Text = @"DesktopModules/[ModuleFolder]/install.xml";
                this.FriendlyName.Text = string.Empty;
                this.DesktopSrc.Text = string.Empty;
                this.MobileSrc.Text = string.Empty;
            }
            else
            {
                // Obtain the module definition to edit from the database
                var dr = modules.GetSingleModuleDefinition(this.defId);

                // Read in first row from database
                var friendlyName = dr.FriendlyName;
                this.FriendlyName.Text = friendlyName;
                var desktopSrc = dr.DesktopSource;
                this.DesktopSrc.Text = desktopSrc;
                this.MobileSrc.Text = dr.MobileSource;
                this.lblGUID.Text = dr.GeneralModDefID.ToString();

                if (this.DesktopSrc.Text.Contains(".aspx") || this.DesktopSrc.Text.Contains(".ascx"))
                {
                    this.ChangeInstallMode(EditMode.Manually);
                }
                else
                {
                    this.FriendlyNameMVC.Text = friendlyName;

                    this.ChangeInstallMode(EditMode.MVC);
                    var items = ModelServices.GetMVCActionModules();
                    foreach (var item in this.GetPortableAreaModules())
                    {
                        items.Add(item.Text, item.Value);
                    }

                    this.ddlAction.DataSource = items;
                    this.ddlAction.DataBind();

                    var val = this.ddlAction.Items[0].Value;
                    foreach (var item in
                        this.ddlAction.Items.Cast<ListItem>().Where(
                            item => item.Text.Contains(desktopSrc.Replace("/", "\\"))))
                    {
                        val = item.Value;
                        break;
                    }

                    this.ddlAction.SelectedValue = val;
                }
            }

            // Populate checkbox list with all portals
            // and "check" the ones already configured for this tab
            var portals = modules.GetModuleInUse(this.defId).ToArray();

            // Clear existing items in checkbox list
            this.PortalsName.Items.Clear();
            this.PortalsName.Items.AddRange(portals);

            this.btnUseInstaller.Click += this.btnUseInstaller_Click;
            this.btnDescription.Click += this.btnDescription_Click;
            this.chbMVCAction.Click += this.chbMVCAction_Click;
            this.chbPortableAreas.Click += this.chbPortableAreas_Click;
            this.selectAllButton.Click += this.selectAllButton_Click;
            this.selectNoneButton.Click += this.selectNoneButton_Click;

            base.OnInit(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">
        /// The <see cref="T:System.EventArgs"/> object that contains the event data.
        /// </param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Verify that the current user has access to access this page
            // Removed by Mario Endara <mario@softworks.com.uy> (2004/11/04)
            // if (PortalSecurity.IsInRoles("Admins") == false) 
            // PortalSecurity.AccessDeniedEdit();
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
                    if (this.chbMVCAction.Visible || this.chbPortableAreas.Visible)
                    {
                        // Es un módulo clásico
                        if (!this.btnUseInstaller.Visible)
                        {
                            ModuleInstall.InstallGroup(
                                this.Server.MapPath(Path.ApplicationRoot + "/" + this.InstallerFileName.Text),
                                this.lblGUID.Text == string.Empty);
                        }
                        else
                        {
                            ModuleInstall.Install(
                                this.FriendlyName.Text,
                                this.DesktopSrc.Text,
                                this.MobileSrc.Text,
                                this.lblGUID.Text == string.Empty);
                        }

                        // Update the module definition
                    }
                    else
                    {
                        // Es una acción MVC
                        var path = this.ddlAction.SelectedValue;

                        path = path.Substring(path.IndexOf("Areas"));
                        path = path.Replace("\\", "/");
                        path = path.Replace(".aspx", string.Empty);
                        path = path.Replace(".ascx", string.Empty);

                        var name = this.FriendlyNameMVC.Text;

                        this.defId = ModelServices.AddMVCActionModule(name, path);
                    }

                    var modules = new ModulesDB();

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
                    this.lblErrorDetail.Text += ex.Message + "<br />";
                    if (!this.btnUseInstaller.Visible)
                    {
                        this.lblErrorDetail.Text += string.Format(
                            " Installer: {0}",
                            this.Server.MapPath(
                                string.Format("{0}/{1}", Path.ApplicationRoot, this.InstallerFileName.Text)));
                    }
                    else
                    {
                        this.lblErrorDetail.Text += string.Format(
                            " Module: '{0}' - Source: '{1}' - Mobile: '{2}'",
                            this.FriendlyName.Text,
                            this.DesktopSrc.Text,
                            this.MobileSrc.Text);
                    }

                    this.lblErrorDetail.Visible = true;

                    ErrorHandler.Publish(LogLevel.Error, this.lblErrorDetail.Text, ex);
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the btnRegisterPortableAreas control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        protected void btnRegisterPortableAreas_Click(object sender, EventArgs e)
        {
            var modulesRegistered = new List<string>();
            foreach (var file in new DirectoryInfo(this.Server.MapPath("~/bin/")).GetFiles("*.dll"))
            {
                try
                {
                    var assembly = Assembly.ReflectionOnlyLoadFrom(file.FullName);
                    AssemblyName[] refNames = assembly.GetReferencedAssemblies();
                    foreach (AssemblyName refName in refNames)
                    {
                        var loadedAssem = Assembly.ReflectionOnlyLoad(refName.FullName);
                    }

                    var areaName = assembly.GetName().Name;

                    var modules = from t in assembly.GetTypes()
                                  where
                                      t.IsClass && t.Namespace == areaName + ".Controllers" &&
                                      t.GetMethods().FirstOrDefault(d => d.Name == "Module") != default(MethodInfo)
                                  select
                                      new
                                      {
                                          AssemblyFullName = assembly.FullName,
                                          AreaName = areaName,
                                          Controller = t.Name
                                      };

                    foreach (var module in modules)
                    {
                        var controllerName = module.Controller.Replace("Controller", string.Empty);
                        ModelServices.RegisterPortableAreaModule(module.AreaName, module.AssemblyFullName, controllerName);
                        modulesRegistered.Add(string.Format("{0} - {1}", module.AreaName, controllerName));
                    }
                }
                catch (Exception exc)
                {
                    ErrorHandler.Publish(LogLevel.Debug, exc);
                }
            }

            this.registeredAreas.DataSource = modulesRegistered;
            this.registeredAreas.DataBind();
        }

        /// <summary>
        /// The get portable area modules.
        /// </summary>
        /// <returns>
        /// An enumerable of list items.
        /// </returns>
        private IEnumerable<ListItem> GetPortableAreaModules()
        {
            var result = new List<ListItem>();
            foreach (var file in new DirectoryInfo(this.Server.MapPath("~/bin/")).GetFiles("*.dll"))
            {
                try
                {
                    var assembly = Assembly.ReflectionOnlyLoadFrom(file.FullName);
                    AssemblyName[] refNames = assembly.GetReferencedAssemblies();
                    foreach (AssemblyName refName in refNames)
                    {
                        var loadedAssem = Assembly.ReflectionOnlyLoad(refName.FullName);
                    }
                    var areaName = assembly.GetName().Name;

                    var modules = from t in assembly.GetTypes()
                                  where
                                      t.IsClass && t.Namespace == areaName + ".Controllers" &&
                                      t.GetMethods().FirstOrDefault(d => d.Name == "Module") != default(MethodInfo)
                                  select
                                      new
                                      {
                                          AssemblyFullName = assembly.FullName,
                                          AreaName = areaName,
                                          Controller = t.Name
                                      };

                    result.AddRange(
                        from module in modules
                        let controllerName = module.Controller.Replace("Controller", string.Empty)
                        let itemValue =
                            String.Format("Areas\\{0}\\Views\\{1}\\Module", module.AreaName, controllerName)
                        let itemText =
                            String.Format(
                                "[PortableArea] Areas\\{0}\\Views\\{1}\\Module", module.AreaName, controllerName)
                        select new ListItem(itemText, itemValue));
                }
                catch (Exception exc)
                {
                    ErrorHandler.Publish(LogLevel.Debug, exc);
                }
            }

            return result;
        }

        /// <summary>
        /// Handles the Click event of the btnDescription control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="T:System.EventArgs"/> instance containing the event data.
        /// </param>
        private void btnDescription_Click(object sender, EventArgs e)
        {
            this.ChangeInstallMode(EditMode.Manually);
        }

        /// <summary>
        /// Handles the Click event of the btnUseInstaller control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="T:System.EventArgs"/> instance containing the event data.
        /// </param>
        private void btnUseInstaller_Click(object sender, EventArgs e)
        {
            this.ChangeInstallMode(EditMode.Installer);
        }

        /// <summary>
        /// The chb mvc action_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void chbMVCAction_Click(object sender, EventArgs e)
        {
            this.ChangeInstallMode(EditMode.MVC);
        }

        /// <summary>
        /// The chb portable areas_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void chbPortableAreas_Click(object sender, EventArgs e)
        {
            this.ChangeInstallMode(EditMode.PortableAreas);
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
        private void selectAllButton_Click(object sender, EventArgs e)
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
        private void selectNoneButton_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < this.PortalsName.Items.Count; i++)
            {
                this.PortalsName.Items[i].Selected = false;
            }
        }

        #endregion
    }

    /// <summary>
    /// The edit mode.
    /// </summary>
    public enum EditMode
    {
        /// <summary>
        ///   The installer.
        /// </summary>
        Installer = 0,

        /// <summary>
        ///   The manually.
        /// </summary>
        Manually,

        /// <summary>
        ///   The MVC edit mode.
        /// </summary>
        MVC,

        /// <summary>
        ///   The portable areas.
        /// </summary>
        PortableAreas
    }
}