// --------------------------------------------------------------------------------------------------------------------
// <copyright file="upload.aspx.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   upload files to server.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Content.Web.Modules.FCK.filemanager.upload.aspx
{
    using System;

    using Appleseed.Framework.Security;
    using Appleseed.Framework.Site.Configuration;
    using Appleseed.Framework.Web.UI;

    /// <summary>
    /// upload files to server.
    /// </summary>
    public partial class upload : EditItemPage
    {
        #region Methods

        /// <summary>
        /// Load settings
        /// </summary>
        protected override void LoadSettings()
        {
            if (PortalSecurity.HasEditPermissions(this.PortalSettings.ActiveModule) == false)
            {
                PortalSecurity.AccessDeniedEdit();
            }
        }

        /// <summary>
        /// The on init.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnInit(EventArgs e)
        {
            // CODEGEN: llamada requerida por el Diseñador de Web Forms ASP.NET.
            this.InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        ///   el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.Load += this.Page_Load;
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void Page_Load(object sender, EventArgs e)
        {
            if (this.Request.Files.Count > 0)
            {
                var oFile = this.Request.Files.Get("FCKeditor_File");

                var fileName = oFile.FileName.Substring(oFile.FileName.LastIndexOf("\\") + 1);
                var ms = Framework.Site.Configuration.ModuleSettings.GetModuleSettings(this.PortalSettings.ActiveModule);
                var DefaultImageFolder = "default";
                if (ms["MODULE_IMAGE_FOLDER"] != null)
                {
                    DefaultImageFolder = ms["MODULE_IMAGE_FOLDER"].ToString();
                }
                else if (this.PortalSettings.CustomSettings["SITESETTINGS_DEFAULT_IMAGE_FOLDER"] != null)
                {
                    DefaultImageFolder =
                        this.PortalSettings.CustomSettings["SITESETTINGS_DEFAULT_IMAGE_FOLDER"].ToString();
                }

                var sFileURL = string.Format(
                    "{0}/images/{1}/{2}", this.PortalSettings.PortalFullPath, DefaultImageFolder, fileName);
                var sFilePath = this.Server.MapPath(sFileURL);

                oFile.SaveAs(sFilePath);

                this.Response.Write(
                    string.Format(
                        "<SCRIPT language=javascript>window.opener.setImage('{0}') ; window.close();</SCRIPT>", sFileURL));
            }
        }

        #endregion
    }
}