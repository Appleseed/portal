// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Pictures.ascx.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   Appleseed Portal Pictures module
//   (c)2002 by Ender Malkoc
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Content.Web.Modules
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Web;
    using System.Web.UI.WebControls;
    using System.Xml;

    using Appleseed.Framework;
    using Appleseed.Framework.Content.Data;
    using Appleseed.Framework.Data;
    using Appleseed.Framework.DataTypes;
    using Appleseed.Framework.Design;
    using Appleseed.Framework.Helpers;
    using Appleseed.Framework.Web.UI.WebControls;

    using Label = Appleseed.Framework.Web.UI.WebControls.Label;
    using Path = Appleseed.Framework.Settings.Path;

    /// <summary>
    /// Appleseed Portal Pictures module
    ///     (c)2002 by Ender Malkoc
    /// </summary>
    [History("Mario Hartmann", "mario@hartmann.net", "2.3 beta", "2003/10/08", "moved to seperate folder")]
    public class Pictures : PortalModuleControl
    {
        #region Constants and Fields

        /// <summary>
        ///     Data list for pictures
        /// </summary>
        protected DataList dlPictures;

        /// <summary>
        ///     Error label
        /// </summary>
        protected Label lblError;

        /// <summary>
        ///     Paging for the pictures
        /// </summary>
        protected Paging pgPictures;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref = "Pictures" /> class.
        /// </summary>
        [History("Tim Capps", "tim@cappsnet.com", "2.4 beta", "2004/02/18", 
            "fixed order on settings and added ShowBulkLoad")]
        public Pictures()
        {
            // Add support for workflow
            this.SupportsWorkflow = true;

            // Album Path Setting
            var albumPath = new SettingItem<string, TextBox>(new PortalUrlDataType()) { Required = true, Value = "Album", Order = 3 };
            this.BaseSettings.Add("AlbumPath", albumPath);

            // Thumbnail Resize Options
            var thumbnailResizeOptions = new List<Option>
                {
                    new Option(
                        (int)ResizeOption.FixedWidthHeight,
                        General.GetString("PICTURES_FIXED_WIDTH_AND_HEIGHT", "Fixed width and height", this)),
                    new Option(
                        (int)ResizeOption.MaintainAspectWidth,
                        General.GetString("PICTURES_MAINTAIN_ASPECT_FIXED_WIDTH", "Maintain aspect fixed width", this)),
                    new Option(
                        (int)ResizeOption.MaintainAspectHeight,
                        General.GetString("PICTURES_MAINTAIN_ASPECT_FIXED_HEIGHT", "Maintain aspect fixed height", this))
                };

            // Thumbnail Resize Settings
            var thumbnailResize = new SettingItem<string, ListControl>(new CustomListDataType(thumbnailResizeOptions, "Name", "Val"))
                {
                    Required = true, Value = ((int)ResizeOption.FixedWidthHeight).ToString(), Order = 4 
                };
            this.BaseSettings.Add("ThumbnailResize", thumbnailResize);

            // Thumbnail Width Setting
            var thumbnailWidth = new SettingItem<int, TextBox>()
                {
                    Required = true, Value = 100, Order = 5, MinValue = 2, MaxValue = 9999 
                };
            this.BaseSettings.Add("ThumbnailWidth", thumbnailWidth);

            // Thumbnail Height Setting
            var thumbnailHeight = new SettingItem<int, TextBox>()
                {
                    Required = true, Value = 75, Order = 6, MinValue = 2, MaxValue = 9999 
                };
            this.BaseSettings.Add("ThumbnailHeight", thumbnailHeight);

            // Original Resize Options
            var originalResizeOptions = new List<Option>
                {
                    new Option(
                        (int)ResizeOption.NoResize, General.GetString("PICTURES_DONT_RESIZE", "Don't Resize", this)),
                    new Option(
                        (int)ResizeOption.FixedWidthHeight,
                        General.GetString("PICTURES_FIXED_WIDTH_AND_HEIGHT", "Fixed width and height", this)),
                    new Option(
                        (int)ResizeOption.MaintainAspectWidth,
                        General.GetString("PICTURES_MAINTAIN_ASPECT_FIXED_WIDTH", "Maintain aspect fixed width", this)),
                    new Option(
                        (int)ResizeOption.MaintainAspectHeight,
                        General.GetString("PICTURES_MAINTAIN_ASPECT_FIXED_HEIGHT", "Maintain aspect fixed height", this))
                };

            // Original Resize Settings
            var originalResize = new SettingItem<string, ListControl>(new CustomListDataType(originalResizeOptions, "Name", "Val"))
                {
                    Required = true, Value = ((int)ResizeOption.MaintainAspectWidth).ToString(), Order = 7 
                };
            this.BaseSettings.Add("OriginalResize", originalResize);

            // Original Width Settings
            var originalWidth = new SettingItem<int, TextBox>()
                {
                    Required = true, Value = 800, Order = 8, MinValue = 2, MaxValue = 9999 
                };
            this.BaseSettings.Add("OriginalWidth", originalWidth);

            // Original Width Settings
            var originalHeight = new SettingItem<int, TextBox>()
                {
                    Required = true, Value = 600, Order = 9, MinValue = 2, MaxValue = 9999 
                };
            this.BaseSettings.Add("OriginalHeight", originalHeight);

            // Repeat Direction Options
            var repeatDirectionOptions = new List<Option>
                {
                    new Option(
                        (int)RepeatDirection.Horizontal, General.GetString("PICTURES_HORIZONTAL", "Horizontal", this)),
                    new Option((int)RepeatDirection.Vertical, General.GetString("PICTURES_VERTICAL", "Vertical", this))
                };

            // Repeat Direction Setting
            var repeatDirectionSetting = new SettingItem<string, ListControl>(new CustomListDataType(repeatDirectionOptions, "Name", "Val"))
                {
                    Required = true, Value = ((int)RepeatDirection.Horizontal).ToString(), Order = 10 
                };
            this.BaseSettings.Add("RepeatDirection", repeatDirectionSetting);

            // Repeat Columns Setting
            var repeatColumns = new SettingItem<int, TextBox>()
                {
                    Required = true, Value = 6, Order = 11, MinValue = 1, MaxValue = 200 
                };
            this.BaseSettings.Add("RepeatColumns", repeatColumns);

            // Layouts
            var layouts = new Hashtable();
            foreach (var layoutControl in
                Directory.GetFiles(
                    HttpContext.Current.Server.MapPath(Path.ApplicationRoot + "/Design/PictureLayouts"), "*.ascx"))
            {
                var layoutControlDisplayName = layoutControl.Substring(
                    layoutControl.LastIndexOf("\\") + 1,
                    layoutControl.LastIndexOf(".") - layoutControl.LastIndexOf("\\") - 1);
                var layoutControlName = layoutControl.Substring(layoutControl.LastIndexOf("\\") + 1);
                layouts.Add(layoutControlDisplayName, layoutControlName);
            }

            // Thumbnail Layout Setting
            var thumbnailLayoutSetting = new SettingItem<string, ListControl>(new CustomListDataType(layouts, "Key", "Value"))
                {
                    Required = true, Value = "DefaultThumbnailView.ascx", Order = 12 
                };
            this.BaseSettings.Add("ThumbnailLayout", thumbnailLayoutSetting);

            // Thumbnail Layout Setting
            var imageLayoutSetting = new SettingItem<string, ListControl>(new CustomListDataType(layouts, "Key", "Value"))
                {
                    Required = true, Value = "DefaultImageView.ascx", Order = 13 
                };
            this.BaseSettings.Add("ImageLayout", imageLayoutSetting);

            // PicturesPerPage
            var picturesPerPage = new SettingItem<int, TextBox>()
                {
                    Required = true, Value = 9999, Order = 14, MinValue = 1, MaxValue = 9999 
                };
            this.BaseSettings.Add("PicturesPerPage", picturesPerPage);

            // If false the input box for bulk loads will be hidden
            var allowBulkLoad = new SettingItem<bool, CheckBox>() { Value = false, Order = 15 };
            this.BaseSettings.Add("AllowBulkLoad", allowBulkLoad);
        }

        #endregion

        #region Enums

        /// <summary>
        /// Resize Options
        ///     NoResize : Do not resize the picture
        ///     FixedWidthHeight : Use the width and height specified. 
        ///     MaintainAspectWidth : Use the specified height and calculate height using the original aspect ratio
        ///     MaintainAspectHeight : Use the specified width and calculate width using the original aspect ration
        /// </summary>
        public enum ResizeOption
        {
            /// <summary>
            ///     No resizing
            /// </summary>
            NoResize, 

            /// <summary>
            ///     FixedWidthHeight : Use the width and height specified.
            /// </summary>
            FixedWidthHeight, 

            /// <summary>
            ///     MaintainAspectWidth : Use the specified height and calculate height using the original aspect ratio
            /// </summary>
            MaintainAspectWidth, 

            /// <summary>
            ///     MaintainAspectHeight : Use the specified width and calculate width using the original aspect ration
            /// </summary>
            MaintainAspectHeight
        }

        #endregion

        #region Properties

        /// <summary>
        ///     GUID of module (mandatory)
        /// </summary>
        /// <value></value>
        public override Guid GuidID
        {
            get
            {
                return new Guid("{B29CB86B-AEA1-4E94-8B77-B4E4239258B0}");
            }
        }

        /// <summary>
        ///     Override on derivates classes.
        ///     Return true if the module is Searchable.
        /// </summary>
        /// <value></value>
        public override bool Searchable
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The install
        /// </summary>
        /// <param name="stateSaver">
        /// state saver
        /// </param>
        public override void Install(IDictionary stateSaver)
        {
            var currentScriptName = System.IO.Path.Combine(
                this.Server.MapPath(this.TemplateSourceDirectory), "install.sql");
            var errors = DBHelper.ExecuteScript(currentScriptName, true);
            if (errors.Count > 0)
            {
                // Call rollback
                throw new Exception("Error occurred:" + errors[0]);
            }
        }

        /// <summary>
        /// Searchable module implementation
        /// </summary>
        /// <param name="portalId">
        /// The portal ID
        /// </param>
        /// <param name="userId">
        /// ID of the user is searching
        /// </param>
        /// <param name="searchString">
        /// The text to search
        /// </param>
        /// <param name="searchField">
        /// The fields where perfoming the search
        /// </param>
        /// <returns>
        /// The SELECT sql to perform a search on the current module
        /// </returns>
        public override string SearchSqlSelect(int portalId, int userId, string searchString, string searchField)
        {
            // Parameters:
            // Table Name: the table that holds the data
            // Title field: the field that contains the title for result, must be a field in the table
            // Abstract field: the field that contains the text for result, must be a field in the table
            // Search field: pass the searchField parameter you recieve.
            var s = new SearchDefinition(
                "rb_Pictures", "ShortDescription", "Keywords", "CreatedByUser", "CreatedDate", "Keywords");

            // Add here extra search fields, this way
            s.ArrSearchFields.Add("itm.ShortDescription");

            // Builds and returns the SELECT query
            return s.SearchSqlSelect(portalId, userId, searchString);
        }

        /// <summary>
        /// The Uninstall
        /// </summary>
        /// <param name="stateSaver">
        /// state saver
        /// </param>
        public override void Uninstall(IDictionary stateSaver)
        {
            var currentScriptName = System.IO.Path.Combine(
                this.Server.MapPath(this.TemplateSourceDirectory), "uninstall.sql");
            var errors = DBHelper.ExecuteScript(currentScriptName, true);
            if (errors.Count > 0)
            {
                // Call rollback
                throw new Exception("Error occurred:" + errors[0]);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Given a key returns the value
        /// </summary>
        /// <param name="metadataXml">
        /// XmlDocument containing key value pairs in attributes
        /// </param>
        /// <param name="key">
        /// key of the pair
        /// </param>
        /// <returns>
        /// the string value
        /// </returns>
        protected string GetMetadata(object metadataXml, string key)
        {
            var metadata = new XmlDocument();
            metadata.LoadXml((string)metadataXml);

            var targetNode = metadata.SelectSingleNode(string.Format("/Metadata/@{0}", key));
            return targetNode == null ? null : targetNode.Value;
        }

        /// <summary>
        /// Raises OnInit event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnInit(EventArgs e)
        {
            this.dlPictures.EnableViewState = false;
            this.pgPictures.OnMove += this.Page_Changed;
            this.AddText = "ADD"; // "Add New Picture"
            this.AddUrl = "~/DesktopModules/CommunityModules/Pictures/PicturesEdit.aspx";
            base.OnInit(e);
        }

        /// <summary>
        /// The on load.
        /// </summary>
        /// <param name="e">
        /// Event arguments.
        /// </param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.dlPictures.RepeatDirection = RepeatDirection.Horizontal;
            if (this.Settings.ContainsKey("RepeatDirectionSetting")) 
            {
                this.dlPictures.RepeatDirection = (RepeatDirection)this.Settings["RepeatDirectionSetting"].Value;
            }
            this.dlPictures.RepeatColumns = Int32.Parse((SettingItem<int, TextBox>)this.Settings["RepeatColumns"]);
            this.dlPictures.ItemDataBound += this.Pictures_ItemDataBound;
            this.pgPictures.RecordsPerPage = Int32.Parse(this.Settings["PicturesPerPage"].ToString());
            this.BindData(this.pgPictures.PageNumber);
        }

        /// <summary>
        /// Overridden from PortalModuleControl, this override deletes unnecessary picture files from the system
        /// </summary>
        protected override void Publish()
        {
            var pathToDelete = string.Format(
                "{0}\\", this.Server.MapPath(((SettingItem<string, TextBox>)this.Settings["AlbumPath"]).FullPath));

            var albumDirectory = new DirectoryInfo(pathToDelete);

            foreach (var fi in albumDirectory.GetFiles(string.Format("{0}m*.Production.jpg", this.ModuleID)))
            {
                try
                {
                    File.Delete(fi.FullName);
                }
                catch
                {
                }
            }

            foreach (var fi in albumDirectory.GetFiles(string.Format("{0}m*", this.ModuleID)))
            {
                try
                {
                    File.Copy(fi.FullName, fi.FullName.Replace(".jpg", ".Production.jpg"), true);
                }
                catch
                {
                }
            }

            base.Publish();
        }

        /// <summary>
        /// The Binddata method on this User Control is used to
        ///     obtain a DataReader of picture information from the Pictures
        ///     table, and then databind the results to a templated DataList
        ///     server control. It uses the Appleseed.PictureDB()
        ///     data component to encapsulate all data functionality.
        /// </summary>
        /// <param name="page">
        /// The page.
        /// </param>
        private void BindData(int page)
        {
            var pictures = new PicturesDB();
            var dsPictures = pictures.GetPicturesPaged(
                this.ModuleID, page, Int32.Parse(this.Settings["PicturesPerPage"].ToString()), this.Version);

            if (dsPictures.Tables.Count > 0 && dsPictures.Tables[0].Rows.Count > 0)
            {
                this.pgPictures.RecordCount = (int)dsPictures.Tables[0].Rows[0]["RecordCount"];
            }

            this.dlPictures.DataSource = dsPictures;
            this.dlPictures.DataBind();
        }

        /// <summary>
        /// Handles the Changed event of the Page control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void Page_Changed(object sender, EventArgs e)
        {
            this.BindData(this.pgPictures.PageNumber);
        }

        /// <summary>
        /// Handles the ItemDataBound event of the Pictures control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Web.UI.WebControls.DataListItemEventArgs"/> instance containing the event data.
        /// </param>
        private void Pictures_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            PictureItem pictureItem;
            try
            {
                pictureItem =
                    (PictureItem)
                    this.Page.LoadControl(
                        string.Format("{0}/Design/PictureLayouts/{1}", Path.ApplicationRoot, this.Settings["ThumbnailLayout"]));
            }
            catch
            {
                this.lblError.Visible = true;
                this.dlPictures.Visible = false;
                this.pgPictures.Visible = false;
                return;
            }

            var di = (DataRowView)e.Item.DataItem;

            var metadata = new XmlDocument();
            metadata.LoadXml((string)di["MetadataXml"]);

            var albumPath = metadata.CreateAttribute("AlbumPath");
            albumPath.Value = ((SettingItem<string, TextBox>)this.Settings["AlbumPath"]).FullPath;

            var itemId = metadata.CreateAttribute("ItemID");
            itemId.Value = ((int)di["ItemID"]).ToString();

            var moduleId = metadata.CreateAttribute("ModuleID");
            moduleId.Value = this.ModuleID.ToString();

            var version = metadata.CreateAttribute("WVersion");
            version.Value = this.Version.ToString();

            var editable = metadata.CreateAttribute("IsEditable");
            editable.Value = this.IsEditable.ToString();

            if (metadata.DocumentElement != null)
            {
                metadata.DocumentElement.Attributes.Append(albumPath);
                metadata.DocumentElement.Attributes.Append(itemId);
                metadata.DocumentElement.Attributes.Append(moduleId);
                metadata.DocumentElement.Attributes.Append(editable);
                metadata.DocumentElement.Attributes.Append(version);

                if (this.Version == WorkFlowVersion.Production)
                {
                    var modifiedFilenameNode = metadata.DocumentElement.SelectSingleNode("@ModifiedFilename");
                    var thumbnailFilenameNode = metadata.DocumentElement.SelectSingleNode("@ThumbnailFilename");

                    if (modifiedFilenameNode != null)
                    {
                        modifiedFilenameNode.Value = modifiedFilenameNode.Value.Replace(".jpg", ".Production.jpg");
                    }

                    if (thumbnailFilenameNode != null)
                    {
                        thumbnailFilenameNode.Value = thumbnailFilenameNode.Value.Replace(".jpg", ".Production.jpg");
                    }
                }
            }

            pictureItem.Metadata = metadata;
            pictureItem.DataBind();
            e.Item.Controls.Add(pictureItem);
        }

        #endregion

        /// <summary>
        /// Structure used for list settings
        /// </summary>
        public struct Option
        {
            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="Option"/> struct.
            /// </summary>
            /// <param name="aVal">
            /// A val.
            /// </param>
            /// <param name="aName">
            /// A name.
            /// </param>
            public Option(int aVal, string aName)
                : this()
            {
                this.Val = aVal;
                this.Name = aName;
            }

            #endregion

            #region Properties

            /// <summary>
            ///     Gets or sets the name.
            /// </summary>
            /// <value>The name.</value>
            public string Name { get; set; }

            /// <summary>
            ///     Gets or sets the val.
            /// </summary>
            /// <value>The val.</value>
            public int Val { get; set; }

            #endregion
        }
    }
}