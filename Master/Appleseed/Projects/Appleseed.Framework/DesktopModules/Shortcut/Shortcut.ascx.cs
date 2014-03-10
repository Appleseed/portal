// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Shortcut.ascx.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   Shortcut control provide a quick way to duplicate
//   a module content in different page of the portal
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Content.Web.Modules
{
    using System;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Appleseed.Framework;
    using Appleseed.Framework.DataTypes;
    using Appleseed.Framework.Settings;
    using Appleseed.Framework.Settings.Cache;
    using Appleseed.Framework.Site.Configuration;
    using Appleseed.Framework.Site.Data;
    using Appleseed.Framework.Web.UI.WebControls;

    /// <summary>
    /// Shortcut control provide a quick way to duplicate
    ///   a module content in different page of the portal
    /// </summary>
    /// <remarks>
    /// </remarks>
    [History("jminond", "march 2005", "Changes for moving Tab to Page")]
    [History("Mario Hartmann", "mario@hartmann.net", "1.1", "2003/10/08", "moved to separate folder")]
    public class Shortcut : PortalModuleControl
    {
        #region Constants and Fields

        /// <summary>
        /// The place holder module.
        /// </summary>
        protected PlaceHolder PlaceHolderModule;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Shortcut"/> class. 
        /// </summary>
        /// <remarks>
        /// </remarks>
        public Shortcut()
        {
            // Obtain PortalSettings from Current Context
            if (HttpContext.Current == null || HttpContext.Current.Items["PortalSettings"] == null)
            {
                return;
            }

            // Do not remove these checks!! It fails installing modules on startup
            var PortalSettings1 = (PortalSettings)HttpContext.Current.Items["PortalSettings"];

            var p = PortalSettings1.PortalID;

            // Get a list of modules of the current running portal
            var linkedModule =
                new SettingItem<string, ListControl>(
                    new CustomListDataType(new ModulesDB().GetModulesSinglePortal(p), "ModuleTitle", "ModuleID"))
                    {
                        Required = true,
                        Order = 0,
                        Value = "0"
                    };
            this.BaseSettings.Add("LinkedModule", linkedModule);
        }

        #endregion

        #region Properties

        /// <summary>
        ///   GUID of module (mandatory)
        /// </summary>
        /// <remarks>
        /// </remarks>
        public override Guid GuidID
        {
            get
            {
                return new Guid("{F9F9C3A4-6E16-43b4-B540-984DDB5F1CD2}");
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        /// <remarks></remarks>
        protected override void OnInit(EventArgs e)
        {
            var controlPath = string.Empty;

            // Try to get info on linked control
            var linkedModuleId = Int32.Parse(this.Settings["LinkedModule"].ToString());
            var dr = ModuleSettings.GetModuleDefinitionByID(linkedModuleId);
            try
            {
                if (dr.Read())
                {
                    controlPath = string.Format("{0}/{1}", Path.ApplicationRoot, dr["DesktopSrc"]);
                }
            }
            finally
            {
                dr.Close();
            }

            // Load control
            PortalModuleControl portalModule = null;
            try
            {
                if (controlPath.Length == 0) {
                    this.PlaceHolderModule.Controls.Add(
                        new LiteralControl(
                            string.Format("Module '{0}' not found!  Use Admin panel to add a linked control.", linkedModuleId)));
                    portalModule = new PortalModuleControl();
                    
                } else {
                    if (controlPath.ToLowerInvariant().Trim().EndsWith(".ascx"))
                    {
                        portalModule = (PortalModuleControl)this.Page.LoadControl(controlPath);
                    }
                    else // MVC
                    {
                        var strArray = controlPath.Split(
                            new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
                        int index = 1;
                        if (!Path.ApplicationRoot.Equals(string.Empty))
                        {
                            index++;
                        }
                        var areaName = (strArray[index].ToLower() == "views") ? string.Empty : strArray[index];
                        var controllerName = strArray[strArray.Length - 2];
                        var actionName = strArray[strArray.Length - 1];

                        // var ns = strArray[2];
                        portalModule =
                            
                            (PortalModuleControl)this.Page.LoadControl("~/DesktopModules/CoreModules/MVC/MVCModule.ascx");

                        ((MVCModuleControl)portalModule).ControllerName = controllerName;
                        ((MVCModuleControl)portalModule).ActionName = actionName;
                        ((MVCModuleControl)portalModule).AreaName = areaName;
                        ((MVCModuleControl)portalModule).ModID = linkedModuleId;

                        ((MVCModuleControl)portalModule).Initialize();
                    }
                   

                    // Sets portal ID
                    portalModule.PortalID = this.PortalID;

                    // Update settings
                    var m = new ModuleSettings {
                        ModuleID = linkedModuleId,
                        PageID = this.ModuleConfiguration.PageID,
                        PaneName = this.ModuleConfiguration.PaneName,
                        ModuleTitle = this.ModuleConfiguration.ModuleTitle,
                        AuthorizedEditRoles = string.Empty,   // read only
                        AuthorizedViewRoles = string.Empty,   // read only
                        AuthorizedAddRoles = string.Empty,    // read only
                        AuthorizedDeleteRoles = string.Empty, // read only
                        AuthorizedPropertiesRoles = this.ModuleConfiguration.AuthorizedPropertiesRoles,
                        CacheTime = this.ModuleConfiguration.CacheTime,
                        ModuleOrder = this.ModuleConfiguration.ModuleOrder,
                        ShowMobile = this.ModuleConfiguration.ShowMobile,
                        DesktopSrc = controlPath,
                        MobileSrc = string.Empty, // not supported yet
                        SupportCollapsable = this.ModuleConfiguration.SupportCollapsable
                    };

                    // added bja@reedtek.com
                    portalModule.ModuleConfiguration = m;

                    portalModule.Settings["MODULESETTINGS_APPLY_THEME"] = this.Settings["MODULESETTINGS_APPLY_THEME"];
                    portalModule.Settings["MODULESETTINGS_THEME"] = this.Settings["MODULESETTINGS_THEME"];

                    // added so ShowTitle is independent of the Linked Module
                    portalModule.Settings["MODULESETTINGS_SHOW_TITLE"] = this.Settings["MODULESETTINGS_SHOW_TITLE"];

                    // added so that shortcut works for module "print this..." feature
                    this.PlaceHolderModule.ID = "Shortcut";

                    // added so AllowCollapsable -- bja@reedtek.com
                    if (portalModule.Settings.ContainsKey("AllowCollapsable"))
                        portalModule.Settings["AllowCollapsable"] = this.Settings["AllowCollapsable"];

                    // Add control to the page
                    this.PlaceHolderModule.Controls.Add(portalModule);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Publish(LogLevel.Error, string.Format("Shortcut: Unable to load control '{0}'!", controlPath), ex);
                this.PlaceHolderModule.Controls.Add(
                    new LiteralControl(
                        string.Format("<br /><span class=\"NormalRed\">Unable to load control '{0}'!</span><br />", controlPath)));
                this.PlaceHolderModule.Controls.Add(new LiteralControl(ex.Message));
                return; // The controls has failed!
            }

            // Set title
            portalModule.PropertiesUrl = HttpUrlBuilder.BuildUrl(
                "~/DesktopModules/CoreModules/Admin/PropertyPage.aspx", this.PageID, "mID=" + this.ModuleID);
            portalModule.PropertiesText = "PROPERTIES";
            portalModule.AddUrl = string.Empty; // Readonly
            portalModule.AddText = string.Empty; // Readonly
            portalModule.EditUrl = string.Empty; // Readonly
            portalModule.EditText = string.Empty; // Readonly

            // jes1111
            portalModule.OriginalModuleID = this.ModuleID;
            CurrentCache.Remove(Key.ModuleSettings(linkedModuleId));

            base.OnInit(e);
        }

        #endregion
    }
}