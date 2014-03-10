using System;
using System.Collections;
using System.Data.SqlClient;
using System.Xml;
using Appleseed.Framework;
using Appleseed.Framework.Content.Data;
using Appleseed.Framework.Design;
using Appleseed.Framework.Settings;
using Appleseed.Framework.Web.UI;

namespace Appleseed.Content.Web.Modules
{
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    /// <summary>
    /// View picture page
    /// </summary>
    public partial class PictureView : ViewItemPage
    {
        /// <summary>
        /// The Page_Load event handler on this Page is used to
        /// obtain obtain the contents of a picture from the
        /// Pictures table, construct an HTTP Response of the
        /// correct type for the picture, and then stream the
        /// picture contents to the response.  It uses the
        /// Appleseed.PictureDB() data component to encapsulate
        /// the data access functionality.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack && ModuleID > 0 && ItemID > 0)
            {
                // Obtain a single row of picture information
                PicturesDB pictures = new PicturesDB();
                WorkFlowVersion version = Request.QueryString["wversion"] == "Staging"
                                              ? WorkFlowVersion.Staging
                                              : WorkFlowVersion.Production;
                SqlDataReader dr = pictures.GetSinglePicture(ItemID, version);

                PictureItem pictureItem;
                XmlDocument metadata = new XmlDocument();

                try
                {
                    // Read first row from database
                    if (dr.Read())
                    {
                        pictureItem =
                            (PictureItem)
                            Page.LoadControl(Path.ApplicationRoot + "/Design/PictureLayouts/" +
                                             this.ModuleSettings["ImageLayout"]);

                        metadata.LoadXml((string) dr["MetadataXml"]);

                        XmlAttribute albumPath = metadata.CreateAttribute("AlbumPath");
                        albumPath.Value = ((SettingItem<string, TextBox>) this.ModuleSettings["AlbumPath"]).FullPath;

                        XmlAttribute itemID = metadata.CreateAttribute("ItemID");
                        itemID.Value = ((int) dr["ItemID"]).ToString();

                        XmlAttribute moduleID = metadata.CreateAttribute("ModuleID");
                        moduleID.Value = ModuleID.ToString();

                        XmlAttribute wVersion = metadata.CreateAttribute("WVersion");
                        wVersion.Value = version.ToString();

                        if (dr["PreviousItemID"] != DBNull.Value)
                        {
                            XmlAttribute previousItemID = metadata.CreateAttribute("PreviousItemID");
                            previousItemID.Value = ((int) dr["PreviousItemID"]).ToString();
                            metadata.DocumentElement.Attributes.Append(previousItemID);
                        }

                        if (dr["NextItemID"] != DBNull.Value)
                        {
                            XmlAttribute nextItemID = metadata.CreateAttribute("NextItemID");
                            nextItemID.Value = ((int) dr["NextItemID"]).ToString();
                            metadata.DocumentElement.Attributes.Append(nextItemID);
                        }

                        metadata.DocumentElement.Attributes.Append(albumPath);
                        metadata.DocumentElement.Attributes.Append(itemID);
                        metadata.DocumentElement.Attributes.Append(moduleID);
                        metadata.DocumentElement.Attributes.Append(wVersion);

                        if (version == WorkFlowVersion.Production)
                        {
                            XmlNode modifiedFilenameNode =
                                metadata.DocumentElement.SelectSingleNode("@ModifiedFilename");
                            XmlNode thumbnailFilenameNode =
                                metadata.DocumentElement.SelectSingleNode("@ThumbnailFilename");

                            modifiedFilenameNode.Value = modifiedFilenameNode.Value.Replace(".jpg", ".Production.jpg");
                            thumbnailFilenameNode.Value = thumbnailFilenameNode.Value.Replace(".jpg", ".Production.jpg");
                        }


                        pictureItem.Metadata = metadata;
                        pictureItem.DataBind();

                        Picture.Controls.Add(pictureItem);
                    }
                }
                catch
                {
                    lblError.Visible = true;
                    Picture.Visible = false;
                    return;
                }
                finally
                {
                    // Close datareader
                    dr.Close();
                }

                DataBind();
            }
        }

        /// <summary>
        /// Set the module guids with free access to this page
        /// </summary>
        /// <value>The allowed modules.</value>
        protected override List<string> AllowedModules
        {
            get
            {
                List<string> al = new List<string>();
                al.Add("B29CB86B-AEA1-4E94-8B77-B4E4239258B0");
                return al;
            }
        }

        #region Web Form Designer generated code

        /// <summary>
        /// Raises OnInitEvent
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(this.Page_Load);

            base.OnInit(e);
        }

        #endregion
    }
}