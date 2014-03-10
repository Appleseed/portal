using System;
using Appleseed.Framework;
using Appleseed.Framework.DataTypes;
using Appleseed.Framework.Site.Data;
using History=Appleseed.Framework.History;

namespace Appleseed.Content.Web.Modules
{
    using System.Web.UI.WebControls;

    /// <summary>
    /// ShortcutAll module provide a quick way to duplicate
    /// a module content in different page from different portals 
    /// </summary>
    [History("Mario Hartmann", "mario@hartmann.net", "1.3", "2003/10/08", "moved to separate folder")]
    public partial class ShortcutAll : Shortcut
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShortcutAll"/> class.
        /// </summary>
        /// <remarks></remarks>
        public ShortcutAll()
        {
            // Get a list of modules of all portals
            var linkedModule =
                new SettingItem<string, ListControl>(
                    new CustomListDataType(new ModulesDB().GetModulesAllPortals(), "ModuleTitle", "ModuleID"))
                    {
                        Required = true, Order = 0, Value = "0" 
                    };
            
            // Overrides the base setting
            this.BaseSettings["LinkedModule"] = linkedModule;
        }

        #region General Implementation

        /// <summary>
        /// GUID of module (mandatory)
        /// </summary>
        /// <value></value>
        public override Guid GuidID
        {
            get { return new Guid("{F9F9C3A4-6E16-43b4-B540-984DDB5F1CD0}"); }
        }

        #endregion

        #region Web Form Designer generated code

        /// <summary>
        /// On init
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            int p = this.PortalSettings.PortalID;
        }

        #endregion
    }
}