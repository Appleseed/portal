// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imagegallery.aspx.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   Imagegallery.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Content.Web.Modules.FCK.filemanager.browse
{
    using System;
    using System.IO;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    using Appleseed.Framework.Security;
    using Appleseed.Framework.Site.Configuration;
    using Appleseed.Framework.Web.UI;

    using Image = System.Drawing.Image;
    using Path = Appleseed.Framework.Settings.Path;

    /// <summary>
    /// Imagegallery.
    /// </summary>
    public partial class imagegallery : EditItemPage
    {
        #region Constants and Fields

        /// <summary>
        /// The accepted file types.
        /// </summary>
        private readonly string[] AcceptedFileTypes = new[] { "jpg", "jpeg", "jpe", "gif", "bmp", "png" };

        /// <summary>
        /// The delete is enabled.
        /// </summary>
        private bool DeleteIsEnabled = true;

        /// <summary>
        /// The invalid file type message.
        /// </summary>
        private string InvalidFileTypeMessage = "Invalid file type";

        /// <summary>
        /// The no file message.
        /// </summary>
        private string NoFileMessage = "No file selected";

        /// <summary>
        /// The no file to delete message.
        /// </summary>
        private string NoFileToDeleteMessage = "No file to delete";

        /// <summary>
        /// The no folder specified message.
        /// </summary>
        private string NoFolderSpecifiedMessage = "No folder";

        /// <summary>
        /// The no images message.
        /// </summary>
        private string NoImagesMessage = "No Images";

        // Configuration
        /// <summary>
        /// The upload is enabled.
        /// </summary>
        private bool UploadIsEnabled = true;

        /// <summary>
        /// The upload success message.
        /// </summary>
        private string UploadSuccessMessage = "Uploaded Sucess";

        #endregion

        #region Public Methods

        /// <summary>
        /// Handles the OnClick event of the DeleteImage control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        public void DeleteImage_OnClick(object sender, EventArgs e)
        {
            if (this.FileToDelete.Value.Length != 0 && this.FileToDelete.Value != "undefined")
            {
                try
                {
                    var AppPath = HttpContext.Current.Request.PhysicalApplicationPath;
                    File.Delete(AppPath + this.CurrentImagesFolder.Value + "\\" + this.FileToDelete.Value);
                    this.ResultsMessage.Text = "Deleted: " + this.FileToDelete.Value;
                }
                catch
                {
                    this.ResultsMessage.Text = "There was an error.";
                }
            }
            else
            {
                this.ResultsMessage.Text = this.NoFileToDeleteMessage;
            }

            this.DisplayImages();
        }

        /// <summary>
        /// Displays the images.
        /// </summary>
        public void DisplayImages()
        {
            var FilesArray = this.ReturnFilesArray();
            var DirectoriesArray = this.ReturnDirectoriesArray();
            var AppPath = HttpContext.Current.Request.PhysicalApplicationPath;
            string AppUrl;

            // Get the application's URL
            AppUrl = this.Request.ApplicationPath;
            if (!AppUrl.EndsWith("/"))
            {
                AppUrl += "/";
            }

            AppUrl = AppUrl.Replace("//", "/");

            this.GalleryPanel.Controls.Clear();
            if ((FilesArray == null || FilesArray.Length == 0) &&
                (DirectoriesArray == null || DirectoriesArray.Length == 0))
            {
                this.gallerymessage.Text = this.NoImagesMessage + ": " + this.RootImagesFolder.Value;
            }
            else
            {
                var ImageFileName = string.Empty;
                var ImageFileLocation = string.Empty;

                var thumbWidth = 94;
                var thumbHeight = 94;

                if (this.CurrentImagesFolder.Value != this.RootImagesFolder.Value)
                {
                    var myHtmlImage = new HtmlImage();
                    myHtmlImage.Src = Path.ApplicationRoot + "/DesktopModules/FCK/filemanager/folder.gif";
                    myHtmlImage.Attributes["unselectable"] = "on";
                    myHtmlImage.Attributes["align"] = "absmiddle";
                    myHtmlImage.Attributes["vspace"] = "36";

                    var ParentFolder = this.CurrentImagesFolder.Value.Substring(
                        0, this.CurrentImagesFolder.Value.LastIndexOf("\\"));

                    var myImageHolder = new Panel();
                    myImageHolder.CssClass = "imageholder";
                    myImageHolder.Attributes["unselectable"] = "on";
                    myImageHolder.Attributes["onclick"] = "divClick(this,'');";
                    myImageHolder.Attributes["ondblclick"] = "gotoFolder('" + this.RootImagesFolder.Value + "','" +
                                                             ParentFolder.Replace("\\", "\\\\") + "');";
                    myImageHolder.Controls.Add(myHtmlImage);

                    var myMainHolder = new Panel();
                    myMainHolder.CssClass = "imagespacer";
                    myMainHolder.Controls.Add(myImageHolder);

                    var myTitleHolder = new Panel();
                    myTitleHolder.CssClass = "titleHolder";
                    myTitleHolder.Controls.Add(new LiteralControl("Up"));
                    myMainHolder.Controls.Add(myTitleHolder);

                    this.GalleryPanel.Controls.Add(myMainHolder);
                }

                foreach (var _Directory in DirectoriesArray)
                {
                    try
                    {
                        var DirectoryName = _Directory;

                        var myHtmlImage = new HtmlImage();
                        myHtmlImage.Src = Path.ApplicationRoot + "/DesktopModules/FCK/filemanager/folder.gif";
                        myHtmlImage.Attributes["unselectable"] = "on";
                        myHtmlImage.Attributes["align"] = "absmiddle";
                        myHtmlImage.Attributes["vspace"] = "29";

                        var myImageHolder = new Panel();
                        myImageHolder.CssClass = "imageholder";
                        myImageHolder.Attributes["unselectable"] = "on";
                        myImageHolder.Attributes["onclick"] = "divClick(this);";
                        myImageHolder.Attributes["ondblclick"] = "gotoFolder('" + this.RootImagesFolder.Value + "','" +
                                                                 DirectoryName.Replace(AppPath, string.Empty).Replace(
                                                                     "\\", "\\\\") + "');";
                        myImageHolder.Controls.Add(myHtmlImage);

                        var myMainHolder = new Panel();
                        myMainHolder.CssClass = "imagespacer";
                        myMainHolder.Controls.Add(myImageHolder);

                        var myTitleHolder = new Panel();
                        myTitleHolder.CssClass = "titleHolder";
                        myTitleHolder.Controls.Add(
                            new LiteralControl(
                                DirectoryName.Replace(AppPath + this.CurrentImagesFolder.Value + "\\", string.Empty)));
                        myMainHolder.Controls.Add(myTitleHolder);

                        this.GalleryPanel.Controls.Add(myMainHolder);
                    }
                    catch
                    {
                        // nothing for error
                    }
                }

                foreach (var ImageFile in FilesArray)
                {
                    try
                    {
                        ImageFileName = ImageFile;
                        ImageFileName = ImageFileName.Substring(ImageFileName.LastIndexOf("\\") + 1);
                        ImageFileLocation = AppUrl;

                        // 						ImageFileLocation = ImageFileLocation.Substring(ImageFileLocation.LastIndexOf("\\")+1);
                        // galleryfilelocation += "/";
                        ImageFileLocation += this.CurrentImagesFolder.Value;
                        ImageFileLocation += "/";
                        ImageFileLocation += ImageFileName;
                        ImageFileLocation = ImageFileLocation.Replace("//", "/");
                        var myHtmlImage = new HtmlImage();
                        myHtmlImage.Src = ImageFileLocation;
                        var myImage = Image.FromFile(ImageFile);
                        myHtmlImage.Attributes["unselectable"] = "on";

                        // myHtmlImage.border=0;

                        // landscape image
                        if (myImage.Width > myImage.Height)
                        {
                            if (myImage.Width > thumbWidth)
                            {
                                myHtmlImage.Width = thumbWidth;
                                myHtmlImage.Height = Convert.ToInt32(myImage.Height * thumbWidth / myImage.Width);
                            }
                            else
                            {
                                myHtmlImage.Width = myImage.Width;
                                myHtmlImage.Height = myImage.Height;
                            }

                            // portrait image
                        }
                        else
                        {
                            if (myImage.Height > thumbHeight)
                            {
                                myHtmlImage.Height = thumbHeight;
                                myHtmlImage.Width = Convert.ToInt32(myImage.Width * thumbHeight / myImage.Height);
                            }
                            else
                            {
                                myHtmlImage.Width = myImage.Width;
                                myHtmlImage.Height = myImage.Height;
                            }
                        }

                        if (myHtmlImage.Height < thumbHeight)
                        {
                            myHtmlImage.Attributes["vspace"] =
                                Convert.ToInt32((thumbHeight / 2) - (myHtmlImage.Height / 2)).ToString();
                        }

                        var myImageHolder = new Panel();
                        myImageHolder.CssClass = "imageholder";
                        myImageHolder.Attributes["onclick"] = "divClick(this,'" + ImageFileName + "');";
                        myImageHolder.Attributes["ondblclick"] = "returnImage('" + ImageFileLocation.Replace("\\", "/") +
                                                                 "','" + myImage.Width + "','" + myImage.Height + "');";
                        myImageHolder.Controls.Add(myHtmlImage);

                        var myMainHolder = new Panel();
                        myMainHolder.CssClass = "imagespacer";
                        myMainHolder.Controls.Add(myImageHolder);

                        var myTitleHolder = new Panel();
                        myTitleHolder.CssClass = "titleHolder";
                        myTitleHolder.Controls.Add(
                            new LiteralControl(ImageFileName + "<BR>" + myImage.Width + "x" + myImage.Height));
                        myMainHolder.Controls.Add(myTitleHolder);

                        // GalleryPanel.Controls.Add(myImage);
                        this.GalleryPanel.Controls.Add(myMainHolder);

                        myImage.Dispose();
                    }
                    catch
                    {
                    }
                }

                this.gallerymessage.Text = string.Empty;
            }
        }

        /// <summary>
        /// Upload Image OnClick
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        public void UploadImage_OnClick(object sender, EventArgs e)
        {
            if (this.Page.IsValid)
            {
                if (this.CurrentImagesFolder.Value.Length != 0)
                {
                    if (this.UploadFile.PostedFile.FileName.Trim().Length != 0)
                    {
                        if (this.IsValidFileType(this.UploadFile.PostedFile.FileName))
                        {
                            try
                            {
                                var UploadFileName = string.Empty;
                                var UploadFileDestination = string.Empty;
                                UploadFileName = this.UploadFile.PostedFile.FileName;
                                UploadFileName = UploadFileName.Substring(UploadFileName.LastIndexOf("\\") + 1);
                                UploadFileDestination = HttpContext.Current.Request.PhysicalApplicationPath;
                                UploadFileDestination += this.CurrentImagesFolder.Value;
                                UploadFileDestination += "\\";
                                this.UploadFile.PostedFile.SaveAs(UploadFileDestination + UploadFileName);
                                this.ResultsMessage.Text = this.UploadSuccessMessage;
                            }
                            catch
                            {
                                // ResultsMessage.Text = "Your file could not be uploaded: " + ex.Message;
                                this.ResultsMessage.Text = "There was an error.";
                            }
                        }
                        else
                        {
                            this.ResultsMessage.Text = this.InvalidFileTypeMessage;
                        }
                    }
                    else
                    {
                        this.ResultsMessage.Text = this.NoFileMessage;
                    }
                }
                else
                {
                    this.ResultsMessage.Text = this.NoFolderSpecifiedMessage;
                }
            }
            else
            {
                this.ResultsMessage.Text = this.InvalidFileTypeMessage;
            }

            this.DisplayImages();
        }

        #endregion

        #region Methods

        /// <summary>
        /// LoadSettings
        ///   Check if user has edit permissions
        /// </summary>
        protected override void LoadSettings()
        {
            if (PortalSecurity.HasEditPermissions(this.PortalSettings.ActiveModule) == false)
            {
                PortalSecurity.AccessDeniedEdit();
            }
        }

        /// <summary>
        /// OnInit
        /// </summary>
        /// <param name="e">
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
        /// Determines whether [is valid file type] [the specified file name].
        /// </summary>
        /// <param name="FileName">
        /// Name of the file.
        /// </param>
        /// <returns>
        /// <c>true</c> if [is valid file type] [the specified file name]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsValidFileType(string FileName)
        {
            var ext = FileName.Substring(FileName.LastIndexOf(".") + 1, FileName.Length - FileName.LastIndexOf(".") - 1);
            ext = ext.ToLower();
            for (var i = 0; i < this.AcceptedFileTypes.Length; i++)
            {
                if (ext == this.AcceptedFileTypes[i])
                {
                    return true;
                }
            }

            return false;
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
            var isframe = string.Empty + this.Request["frame"];

            if (isframe.Length != 0)
            {
                this.MainPage.Visible = true;
                this.iframePanel.Visible = false;

                var rif = string.Empty + this.Request["rif"];
                var cif = string.Empty + this.Request["cif"];

                if (cif.Length != 0 && rif.Length != 0)
                {
                    this.RootImagesFolder.Value = rif;
                    this.CurrentImagesFolder.Value = cif;
                }
                else
                {
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

                    this.RootImagesFolder.Value = this.PortalSettings.PortalPath + "/images/" + DefaultImageFolder;
                    this.RootImagesFolder.Value = this.RootImagesFolder.Value.Replace("//", "/");
                    this.CurrentImagesFolder.Value = this.RootImagesFolder.Value;
                }

                this.UploadPanel.Visible = this.UploadIsEnabled;
                this.DeleteImage.Visible = this.DeleteIsEnabled;

                var FileErrorMessage = string.Empty;
                var ValidationString = ".*(";

                // [\.jpg]|[\.jpeg]|[\.jpe]|[\.gif]|[\.bmp]|[\.png])$"
                for (var i = 0; i < this.AcceptedFileTypes.Length; i++)
                {
                    ValidationString += "[\\." + this.AcceptedFileTypes[i] + "]";
                    if (i < (this.AcceptedFileTypes.Length - 1))
                    {
                        ValidationString += "|";
                    }

                    FileErrorMessage += this.AcceptedFileTypes[i];
                    if (i < (this.AcceptedFileTypes.Length - 1))
                    {
                        FileErrorMessage += ", ";
                    }
                }

                this.FileValidator.ValidationExpression = ValidationString + ")$";
                this.FileValidator.ErrorMessage = FileErrorMessage;

                if (!this.IsPostBack)
                {
                    this.DisplayImages();
                }
            }
            else
            {
            }
        }

        /// <summary>
        /// Returns the directories array.
        /// </summary>
        /// <returns>
        /// </returns>
        private string[] ReturnDirectoriesArray()
        {
            if (this.CurrentImagesFolder.Value.Length != 0)
            {
                try
                {
                    var AppPath = HttpContext.Current.Request.PhysicalApplicationPath;
                    var CurrentFolderPath = AppPath + this.CurrentImagesFolder.Value;
                    var DirectoriesArray = Directory.GetDirectories(CurrentFolderPath, "*");
                    return DirectoriesArray;
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the files array.
        /// </summary>
        /// <returns>
        /// </returns>
        private string[] ReturnFilesArray()
        {
            if (this.CurrentImagesFolder.Value.Length != 0)
            {
                try
                {
                    var AppPath = HttpContext.Current.Request.PhysicalApplicationPath;
                    var ImageFolderPath = AppPath + this.CurrentImagesFolder.Value;
                    var FilesArray = Directory.GetFiles(ImageFolderPath, "*");
                    return FilesArray;
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}