namespace Appleseed.Framework.Web.UI
{
    using System.Linq;

    using Appleseed.Framework.Security;
    using Appleseed.Framework.Site.Data;

    /// <summary>
    /// SecurePage inherits from Appleseed.Framework.Web.UI.Page <br/>
    ///     Used for Security Access pages<br/>
    ///     Can be inherited
    /// </summary>
    [History("jviladiu@portalServices.net", "2004/07/22", "Created this to support pages that need Security Access.")]
    public class SecurePage : Page
    {
        #region Methods

        /// <summary>
        /// Load settings
        /// </summary>
        protected override void LoadSettings()
        {
            base.LoadSettings();
            this.PortalSettings.ActiveModule = this.ModuleID;
        }

        /// <summary>
        /// Get the AllowedModules array from page if exists and set the restrictions for use
        ///     For this method work, the user page need override AllowedModules with GUIDS
        /// </summary>
        protected override void ModuleGuidInCookie()
        {
            if (this.AllowedModules == null)
            {
                return;
            }
            
            var guidsInUse = string.Empty;
            var cookie = this.Request.Cookies["AppleseedSecurity"];
            if (cookie != null)
            {
                guidsInUse = cookie.Value;
            }

            if (this.AllowedModules.Any(mg => guidsInUse.IndexOf(mg.ToUpper()) > -1))
            {
                return;
            }

            if (this.ModuleID != 0)
            {
                guidsInUse = (new ModulesDB()).GetModuleGuid(this.ModuleID).ToString().ToUpper();

                if (this.AllowedModules.Any(mg => guidsInUse.IndexOf(mg.ToUpper()) > -1))
                {
                    return;
                }
            }

            PortalSecurity.AccessDenied();
        }

        #endregion
    }
}