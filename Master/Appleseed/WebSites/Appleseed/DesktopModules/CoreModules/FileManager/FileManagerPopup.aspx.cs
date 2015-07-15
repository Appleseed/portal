// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileManagerPopup.cs">
//   Copyright © -- 2014. All Rights Reserved.
// </copyright>
// <summary>
//  File Manager in Popup window
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Appleseed.DesktopModules.CoreModules.FileManager
{
    using Appleseed.Framework;
    using Appleseed.Framework.Web.UI;
    using System;

    /// <summary>
    /// Load MVC module in Popup window
    /// </summary>
    [History("Ashish.patel@haptix.biz", "2015/05/27", "Open File Manager from HtmlModule control")]
    public partial class FileManagerPopup : EditItemPage
    {
        /// <summary>
        /// Set permater for File Manager control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.MVCModule.ModuleID = 155;
            this.MVCModule.AreaName = "FileManager";
            this.MVCModule.ControllerName = "Home";
            this.MVCModule.ActionName = "Module";
        }
    }
}