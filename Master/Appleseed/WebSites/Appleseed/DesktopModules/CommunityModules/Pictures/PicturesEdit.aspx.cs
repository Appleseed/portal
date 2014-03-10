using System;
using System.Collections;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml;
using Appleseed.Framework;
using Appleseed.Framework.Content.Data;
using Appleseed.Framework.Site.Configuration;
using Appleseed.Framework.Web.UI;
using History=Appleseed.Framework.History;

namespace Appleseed.Content.Web.Modules
{
    using System.Collections.Generic;

    /// <summary>
    /// Appleseed Portal Pictures module - Edit page part
    /// (c)2002 by Ender Malkoc
    /// </summary>
    [History("Jes1111", "2003/03/04", "Cache flushing now handled by inherited page")]
    public partial class PicturesEdit : AddEditItemPage
    {
        #region Declarations

        /// <summary>
        /// 
        /// </summary>
        protected RequiredFieldValidator RequiredFieldValidatorPicture;

        /// <summary>
        /// 
        /// </summary>
        protected RequiredFieldValidator RequiredFieldValidatorShortDescription;

        /// <summary>
        /// 
        /// </summary>
        protected RequiredFieldValidator RequiredFieldValidatorLongDescription;

        protected XmlDocument Metadata;

        #endregion

        #region Puiblic Properties

        /// <summary>
        /// Thumbnail file name
        /// </summary>
        protected string ThumbnailFilename
        {
            get { return (string) ViewState["ThumbnailFilename"]; }
            set { ViewState["ThumbnailFilename"] = value; }
        }

        /// <summary>
        /// Modified file name
        /// </summary>
        protected string ModifiedFilename
        {
            get { return (string) ViewState["ModifiedFilename"]; }
            set { ViewState["ModifiedFilename"] = value; }
        }

        /// <summary>
        /// Metadata for Esperantus.Esperantus.Localize. image in XML format
        /// </summary>
        protected string MetadataXml
        {
            get { return (string) ViewState["MetadataXml"]; }
            set { ViewState["MetadataXml"] = value; }
        }

        #endregion

        /// <summary>
        /// Esperantus.Esperantus.Localize. Page_Load event on this Page is used to obtain Esperantus.Esperantus.Localize. ModuleID
        /// and ItemID of Esperantus.Esperantus.Localize. picture to edit.
        /// It Esperantus.Esperantus.Localize.n uses Esperantus.Esperantus.Localize. Appleseed.PicturesDB() data component
        /// to populate Esperantus.Esperantus.Localize. page's edit controls with Esperantus.Esperantus.Localize. picture details.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        [History("Tim Capps", "tim@cappsnet.com", "2.4 beta", "2004/02/18", "added hide/show bulk load logic")]
        private void Page_Load(object sender, EventArgs e)
        {
            // If Esperantus.Esperantus.Localize. page is being requested Esperantus.Esperantus.Localize. first time, determine if a
            // picture itemID value is specified, and if so populate page
            // contents with Esperantus.Esperantus.Localize. picture details

            Metadata = new XmlDocument();

            if (Page.IsPostBack == false)
            {
                BulkDir.Visible = false; // make bulk load controls not visible for now
                BulkDirLiteral.Visible = false;

                if (ItemID != 0)
                {
                    // Obtain a single row of picture information
                    PicturesDB pictures = new PicturesDB();
                    SqlDataReader dr = pictures.GetSinglePicture(ItemID, WorkFlowVersion.Staging);

                    try
                    {
                        // Read first row from database
                        if (dr.Read())
                        {
                            ShortDescription.Text = (string) dr["ShortDescription"];
                            MetadataXml = (string) dr["MetadataXml"];
                            Metadata.LoadXml(MetadataXml);
                            Keywords.Text = GetMetadata("Keywords");
                            LongDescription.Text = GetMetadata("LongDescription");
                            Caption.Text = GetMetadata("Caption");
                            DisplayOrder.Text = ((int) dr["DisplayOrder"]).ToString();
                            ThumbnailFilename = GetMetadata("ThumbnailFilename");
                            ModifiedFilename = GetMetadata("ModifiedFilename");
                        }
                    }
                    finally
                    {
                        // Close data reader
                        dr.Close();
                    }
                }
                else
                {
                    Metadata.AppendChild(Metadata.CreateElement("Metadata"));
                    MetadataXml = Metadata.OuterXml;
                    if (((SettingItem<bool, CheckBox>)this.ModuleSettings["AllowBulkLoad"]).Value)
                    {
                        // Esperantus.Esperantus.Localize.y are adding, and we are allowed to bulk load so
                        // make Esperantus.Esperantus.Localize. controls visible
                        BulkDir.Visible = true;
                        BulkDirLiteral.Visible = true;
                    }
                }
            }
            else
            {
                Metadata.LoadXml(MetadataXml);
            }
        }

        /// <summary>
        /// Set Esperantus.Esperantus.Localize. module guids with free access to this page
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

        /// <summary>
        /// Esperantus.Esperantus.Localize. UpdateBtn_Click event handler on this Page is used to eiEsperantus.Esperantus.Localize.r
        /// create or update a picture.  It  uses Esperantus.Esperantus.Localize. Appleseed.PicturesDB()
        /// data component to encapsulate all data functionality.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        [History("Tim Capps", "tim@cappsnet.com", "2.4 beta", "2004/02/18", "added bulk load logic")]
        protected override void OnUpdate(EventArgs e)
        {
            base.OnUpdate(e);

            // Only Update if Entered data is Valid
            if (Page.IsValid == true)
            {
                // Create an instance of Esperantus.Esperantus.Localize. PicturesDB component
                PicturesDB pictures = new PicturesDB();
                Bitmap fullPicture = null;

                //Get Esperantus.Esperantus.Localize. resize option for Esperantus.Esperantus.Localize. thumbnail
                Pictures.ResizeOption thumbnailResize =
                    this.ModuleSettings["ThumbnailResize"].Value.ToString() == string.Empty
                        ?
                    Pictures.ResizeOption.FixedWidthHeight
                        :
                    (Pictures.ResizeOption)Enum.Parse(typeof(Pictures.ResizeOption), this.ModuleSettings["ThumbnailResize"].Value.ToString());

                //Get Esperantus.Esperantus.Localize. resize option for Esperantus.Esperantus.Localize. original picture
                Pictures.ResizeOption originalResize =
                    this.ModuleSettings["OriginalResize"].Value.ToString() == string.Empty
                        ?
                    Pictures.ResizeOption.NoResize
                        :
                    (Pictures.ResizeOption)Enum.Parse(typeof(Pictures.ResizeOption), this.ModuleSettings["OriginalResize"].Value.ToString());

                //Where are we going to save Esperantus.Esperantus.Localize. picture?
                string PathToSave = string.Format("{0}\\", this.Server.MapPath(this.ModuleSettings["AlbumPath"].FullPath));

                //Dimensions of Esperantus.Esperantus.Localize. thumbnail as specified in settings
                int thumbnailWidth = (int)this.ModuleSettings["ThumbnailWidth"].Value;
                int thumbnailHeight = (int)this.ModuleSettings["ThumbnailHeight"].Value;

                //Dimensions of Esperantus.Esperantus.Localize. original picture as specified in settings
                int originalWidth = (int)this.ModuleSettings["OriginalWidth"].Value;
                int originalHeight = (int)this.ModuleSettings["OriginalHeight"].Value;

                // See if Esperantus.Esperantus.Localize.y are doing a bulk load.  Esperantus.Esperantus.Localize.y must have specified
                // a bulk load directory (which is on Esperantus.Esperantus.Localize. server) and Esperantus.Esperantus.Localize.y must not
                // have specified a specific upload file name, and Esperantus.Esperantus.Localize.y must not be
                // editing an item.
                if (ItemID == 0 && flPicture.PostedFile.FileName == string.Empty &&
                    BulkDir.Text != string.Empty)
                {
                    // Esperantus.Esperantus.Localize.y are bulk loading - run through Esperantus.Esperantus.Localize. specified directory and
                    // insert all .JPG files into Esperantus.Esperantus.Localize. database.  User is expected to
                    // provide a FULL file path on Esperantus.Esperantus.Localize. SERVER.
                    string[] files = Directory.GetFiles(BulkDir.Text, "*.jpg");

                    foreach (string file in files)
                    {
                        MetadataXml = null;

                        // Create a new FileInfo object for this filename
                        FileInfo fileInfo = new FileInfo(file);

                        //Create new filenames for Esperantus.Esperantus.Localize. thumbnail and Esperantus.Esperantus.Localize. original picture
                        ModifiedFilename = ModuleID.ToString() + "m" + Guid.NewGuid().ToString() + ".jpg";
                        SetMetadata("ModifiedFilename", ModifiedFilename);

                        ThumbnailFilename = ModuleID.ToString() + "m" + Guid.NewGuid().ToString() + ".jpg";
                        SetMetadata("ThumbnailFilename", ThumbnailFilename);

                        //Full path of Esperantus.Esperantus.Localize. original picture
                        string physicalPath = PathToSave + ModifiedFilename;

                        //Make sure Esperantus.Esperantus.Localize. picture target directory exists
                        Directory.CreateDirectory(PathToSave);

                        SetMetadata("OriginalFilename", fileInfo.FullName);

                        try
                        {
                            //Create a bitmap from Esperantus.Esperantus.Localize. saved picture
                            fullPicture = new Bitmap(fileInfo.FullName);
                        }
                        catch (Exception ex)
                        {
                            Message.Text =
                                General.GetString("PICTURES_INVALID_IMAGE_FILE", "Invalid Image File", this) + "!<br>" +
                                ex.Message;
                            return;
                        }

                        SetMetadata("OriginalWidth", fullPicture.Width.ToString());
                        SetMetadata("OriginalHeight", fullPicture.Height.ToString());

                        if (chkIncludeExif.Checked) SetExifInformation(fullPicture);

                        RotateFlip(fullPicture, selFlip.SelectedItem.Value, selRotate.SelectedItem.Value);

                        try
                        {
                            //Resize Esperantus.Esperantus.Localize. original picture with Esperantus.Esperantus.Localize. given settings to create Esperantus.Esperantus.Localize. thumbnail
                            Bitmap thumbnail =
                                ResizeImage(fullPicture, thumbnailWidth, thumbnailHeight, thumbnailResize);
                            thumbnail.Save(PathToSave + ThumbnailFilename, ImageFormat.Jpeg);
                            SetMetadata("ThumbnailWidth", thumbnail.Width.ToString());
                            SetMetadata("ThumbnailHeight", thumbnail.Height.ToString());
                            thumbnail.Dispose();
                        }
                        catch (Exception ex)
                        {
                            Message.Text =
                                General.GetString("PICTURES_THUMBNAIL_ERROR",
                                                  "Error occured while creating Esperantus.Esperantus.Localize. thumbnail",
                                                  this) + "!<br>" + ex.Message;
                            return;
                        }

                        Bitmap modified = null;

                        try
                        {
                            //Resize original image
                            modified = ResizeImage(fullPicture, originalWidth, originalHeight, originalResize);
                        }
                        catch (Exception ex)
                        {
                            Message.Text =
                                General.GetString("PICTURES_RESIZE_ERROR",
                                                  "Error occured while resizing Esperantus.Esperantus.Localize. image",
                                                  this) + "!<br>" + ex.Message;
                            return;
                        }

                        SetMetadata("ModifiedWidth", modified.Width.ToString());
                        SetMetadata("ModifiedHeight", modified.Height.ToString());

                        fullPicture.Dispose();

                        //Save Esperantus.Esperantus.Localize. resized one
                        modified.Save(physicalPath, ImageFormat.Jpeg);
                        modified.Dispose();

                        FileInfo fi = new FileInfo(physicalPath);
                        SetMetadata("ModifiedFileSize", fi.Length.ToString());

                        int displayOrder = 0;

                        try
                        {
                            displayOrder = Int32.Parse(DisplayOrder.Text);
                        }
                        catch
                        {
                        }

                        SetMetadata("ShortDescription", ShortDescription.Text);
                        SetMetadata("LongDescription", LongDescription.Text);
                        SetMetadata("Caption", Caption.Text);
                        SetMetadata("Keywords", Keywords.Text);
                        SetMetadata("UploadDate", DateTime.Now.ToString());
                        SetMetadata("CreatedBy", PortalSettings.CurrentUser.Identity.UserName);
                        SetMetadata("DisplayOrder", displayOrder.ToString());

                        //Add new picture to Esperantus.Esperantus.Localize. database
                        ItemID =
                            pictures.AddPicture(ModuleID, ItemID, displayOrder, MetadataXml, ShortDescription.Text,
                                                Keywords.Text, PortalSettings.CurrentUser.Identity.UserName, DateTime.Now);
                    }
                }
                else
                {
                    // Determine wheEsperantus.Esperantus.Localize.r a file was uploaded
                    if (flPicture.PostedFile.FileName.Length != 0)
                    {
                        //Create new filenames for Esperantus.Esperantus.Localize. thumbnail and Esperantus.Esperantus.Localize. original picture
                        ModifiedFilename = ModuleID.ToString() + "m" + Guid.NewGuid().ToString() + ".jpg";
                        SetMetadata("ModifiedFilename", ModifiedFilename);

                        ThumbnailFilename = ModuleID.ToString() + "m" + Guid.NewGuid().ToString() + ".jpg";
                        SetMetadata("ThumbnailFilename", ThumbnailFilename);

                        //Full path of Esperantus.Esperantus.Localize. original picture
                        string physicalPath = PathToSave + ModifiedFilename;

                        try
                        {
                            // Save Esperantus.Esperantus.Localize. picture
                            flPicture.PostedFile.SaveAs(physicalPath);
                        }
                        catch (DirectoryNotFoundException ex)
                        {
                            // If Esperantus.Esperantus.Localize. directory is not found, create and Esperantus.Esperantus.Localize.n save
                            Directory.CreateDirectory(PathToSave);
                            flPicture.PostedFile.SaveAs(physicalPath);

                            //This line is here to supress Esperantus.Esperantus.Localize. warning
                            ex.ToString();
                        }
                        catch (Exception ex)
                        {
                            // If oEsperantus.Esperantus.Localize.r error occured report to Esperantus.Esperantus.Localize. user
                            Message.Text = General.GetString("PICTURES_INVALID_FILENAME", "Invalid Filename", this) +
                                           "!<br>" + ex.Message;
                            return;
                        }

                        SetMetadata("OriginalFilename", flPicture.PostedFile.FileName);

                        try
                        {
                            //Create a bitmap from Esperantus.Esperantus.Localize. saved picture
                            fullPicture = new Bitmap(physicalPath);
                        }
                        catch (Exception ex)
                        {
                            Message.Text =
                                General.GetString("PICTURES_INVALID_IMAGE_FILE", "Invalid Image File", this) + "!<br>" +
                                ex.Message;
                            return;
                        }

                        SetMetadata("OriginalWidth", fullPicture.Width.ToString());
                        SetMetadata("OriginalHeight", fullPicture.Height.ToString());

                        if (chkIncludeExif.Checked) SetExifInformation(fullPicture);

                        RotateFlip(fullPicture, selFlip.SelectedItem.Value, selRotate.SelectedItem.Value);

                        try
                        {
                            //Resize Esperantus.Esperantus.Localize. original picture with Esperantus.Esperantus.Localize. given settings to create Esperantus.Esperantus.Localize. thumbnail
                            Bitmap thumbnail =
                                ResizeImage(fullPicture, thumbnailWidth, thumbnailHeight, thumbnailResize);
                            thumbnail.Save(PathToSave + ThumbnailFilename, ImageFormat.Jpeg);
                            SetMetadata("ThumbnailWidth", thumbnail.Width.ToString());
                            SetMetadata("ThumbnailHeight", thumbnail.Height.ToString());
                            thumbnail.Dispose();
                        }
                        catch (Exception ex)
                        {
                            Message.Text =
                                General.GetString("PICTURES_THUMBNAIL_ERROR",
                                                  "Error occured while creating Esperantus.Esperantus.Localize. thumbnail",
                                                  this) + "!<br>" + ex.Message;
                            return;
                        }

                        Bitmap modified = null;

                        try
                        {
                            //Resize original image
                            modified = ResizeImage(fullPicture, originalWidth, originalHeight, originalResize);
                        }
                        catch (Exception ex)
                        {
                            Message.Text =
                                General.GetString("PICTURES_RESIZE_ERROR",
                                                  "Error occured while resizing Esperantus.Esperantus.Localize. image",
                                                  this) + "!<br>" + ex.Message;
                            return;
                        }

                        SetMetadata("ModifiedWidth", modified.Width.ToString());
                        SetMetadata("ModifiedHeight", modified.Height.ToString());

                        fullPicture.Dispose();

                        //Delete Esperantus.Esperantus.Localize. original
                        File.Delete(physicalPath);

                        //Save Esperantus.Esperantus.Localize. resized one
                        modified.Save(physicalPath, ImageFormat.Jpeg);
                        modified.Dispose();

                        FileInfo fi = new FileInfo(physicalPath);
                        SetMetadata("ModifiedFileSize", fi.Length.ToString());
                    }
                    else if (ItemID == 0)
                    {
                        Message.Text =
                            General.GetString("PICTURES_SPECIFY_FILENAME", "Please specify a filename", this) + "!<br>";
                        return;
                    }

                    int displayOrder = 0;

                    try
                    {
                        displayOrder = Int32.Parse(DisplayOrder.Text);
                    }
                    catch
                    {
                    }

                    SetMetadata("ShortDescription", ShortDescription.Text);
                    SetMetadata("LongDescription", LongDescription.Text);
                    SetMetadata("Caption", Caption.Text);
                    SetMetadata("Keywords", Keywords.Text);
                    SetMetadata("UploadDate", DateTime.Now.ToString());
                    SetMetadata("CreatedBy", PortalSettings.CurrentUser.Identity.UserName);
                    SetMetadata("DisplayOrder", displayOrder.ToString());

                    if (ItemID == 0)
                    {
                        //If this is a new picture add it to Esperantus.Esperantus.Localize. database
                        ItemID =
                            pictures.AddPicture(ModuleID, ItemID, displayOrder, MetadataXml, ShortDescription.Text,
                                                Keywords.Text, PortalSettings.CurrentUser.Identity.UserName, DateTime.Now);
                    }
                    else
                    {
                        //Update Esperantus.Esperantus.Localize. existing one
                        pictures.UpdatePicture(ModuleID, ItemID, displayOrder, MetadataXml, ShortDescription.Text,
                                               Keywords.Text, PortalSettings.CurrentUser.Identity.Email, DateTime.Now);
                    }
                }
                // Redirect back to Esperantus.Esperantus.Localize. portal home page
                RedirectBackToReferringPage();
            }
        }

        /// <summary>
        /// Esperantus.Esperantus.Localize. DeleteBtn_Click event handler on this Page is used to delete
        /// a picture.  It  uses Esperantus.Esperantus.Localize. Appleseed.PicturesDB()
        /// data component to encapsulate all data functionality.
        /// </summary>
        protected override void OnDelete(EventArgs e)
        {
            base.OnDelete(e);

            // Only attempt to delete Esperantus.Esperantus.Localize. item if it is an existing item
            // (new items will have "ItemID" of 0)

            if (ItemID != 0)
            {
                PicturesDB pictures = new PicturesDB();
                string PathToDelete = Server.MapPath(((SettingItem<string, TextBox>)this.ModuleSettings["AlbumPath"]).FullPath) + "\\";

                SqlDataReader dr = pictures.GetSinglePicture(ItemID, WorkFlowVersion.Staging);

                string filename = string.Empty;
                string thumbnailFilename = string.Empty;
                try
                {
                    // Read first row from database
                    if (dr.Read())
                    {
                        Metadata.LoadXml((string) dr["MetadataXml"]);

                        filename = GetMetadata("ModifiedFilename");
                        thumbnailFilename = GetMetadata("ThumbnailFilename");
                    }
                }
                finally
                {
                    // Close datareader
                    dr.Close();
                }

                try
                {
                    //Delete Esperantus.Esperantus.Localize. files
                    File.Delete(PathToDelete + filename);
                    File.Delete(PathToDelete + thumbnailFilename);
                }
                catch
                {
                    // We don't really have much to do at this point
                }

                //Delete Esperantus.Esperantus.Localize. record from database.
                pictures.DeletePicture(ItemID);
            }

            // Redirect back to Esperantus.Esperantus.Localize. portal home page
            RedirectBackToReferringPage();
        }

        /// <summary>
        /// 
        /// </summary>
        public PicturesEdit()
        {
            Page.Init += new EventHandler(Page_Init);
        }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string GetMetadata(string key)
        {
            XmlNode targetNode = Metadata.SelectSingleNode("/Metadata/@" + key);
            if (targetNode == null)
            {
                return null;
            }
            else
            {
                return targetNode.Value;
            }
        }

        /// <summary>
        /// Sets the metadata.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        public void SetMetadata(string key, string data)
        {
            XmlNode targetNode = Metadata.SelectSingleNode("/Metadata/@" + key);
            if (targetNode == null)
            {
                XmlAttribute newAttribute = Metadata.CreateAttribute(key);
                newAttribute.Value = data;
                Metadata.DocumentElement.Attributes.Append(newAttribute);
            }
            else
            {
                targetNode.Value = data;
            }

            MetadataXml = Metadata.OuterXml;
        }

        /// <summary>
        /// Converts the byte array to string.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns></returns>
        private string ConvertByteArrayToString(byte[] array)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < array.Length - 1; i++) sb.Append((char) array[i]);
            return sb.ToString();
        }

        /// <summary>
        /// Converts the byte array to byte.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns></returns>
        private byte ConvertByteArrayToByte(byte[] array)
        {
            return array[0];
        }

        /// <summary>
        /// Converts the byte array to short.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns></returns>
        private short ConvertByteArrayToShort(byte[] array)
        {
            short val = 0;
            for (int i = 0; i < array.Length; i++) val += (short) (array[i]*Math.Pow(2, (i*8)));
            return val;
        }

        /// <summary>
        /// Converts the byte array to long.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns></returns>
        private long ConvertByteArrayToLong(byte[] array)
        {
            long val = 0;
            for (int i = 0; i < array.Length; i++) val += (array[i]*(long) Math.Pow(2, (i*8)));
            return val;
        }

        /// <summary>
        /// Converts the byte array to rational.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns></returns>
        private string ConvertByteArrayToRational(byte[] array)
        {
            int val1 = 0;
            int val2 = 0;

            for (int i = 0; i < 4; i++) val1 += (array[i]*(int) Math.Pow(2, (i*8)));
            for (int i = 4; i < 8; i++) val2 += (array[i]*(int) Math.Pow(2, ((i - 4)*8)));
            if (val2 == 1)
            {
                return val1.ToString();
            }
            else
            {
                return ((double) val1/(double) val2).ToString();
            }
        }

        /// <summary>
        /// Converts the byte array to S rational.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns></returns>
        private string ConvertByteArrayToSRational(byte[] array)
        {
            int val1 = 0;
            int val2 = 0;

            for (int i = 0; i < 4; i++) val1 += (array[i]*(int) Math.Pow(2, (i*8)));
            for (int i = 4; i < 8; i++) val2 += (array[i]*(int) Math.Pow(2, ((i - 4)*8)));
            if (val2 == 1)
            {
                return val1.ToString();
            }
            else
            {
                return ((double) val1/(double) val2).ToString();
            }
        }

        /// <summary>
        /// Rotates the flip.
        /// </summary>
        /// <param name="original">The original.</param>
        /// <param name="flip">The flip.</param>
        /// <param name="rotate">The rotate.</param>
        /// <returns></returns>
        private Bitmap RotateFlip(Bitmap original, string flip, string rotate)
        {
            RotateFlipType rotateFlipType = RotateFlipType.RotateNoneFlipNone;

            if (flip == "None" && rotate == "180")
            {
                rotateFlipType = RotateFlipType.Rotate180FlipNone;
            }
            else if (flip == "X" && rotate == "180")
            {
                rotateFlipType = RotateFlipType.Rotate180FlipX;
            }
            else if (flip == "XY" && rotate == "180")
            {
                rotateFlipType = RotateFlipType.Rotate180FlipXY;
            }
            else if (flip == "Y" && rotate == "180")
            {
                rotateFlipType = RotateFlipType.Rotate180FlipY;
            }
            else if (flip == "None" && rotate == "270")
            {
                rotateFlipType = RotateFlipType.Rotate270FlipNone;
            }
            else if (flip == "X" && rotate == "270")
            {
                rotateFlipType = RotateFlipType.Rotate270FlipX;
            }
            else if (flip == "XY" && rotate == "270")
            {
                rotateFlipType = RotateFlipType.Rotate270FlipXY;
            }
            else if (flip == "Y" && rotate == "270")
            {
                rotateFlipType = RotateFlipType.Rotate270FlipY;
            }
            else if (flip == "None" && rotate == "90")
            {
                rotateFlipType = RotateFlipType.Rotate90FlipNone;
            }
            else if (flip == "X" && rotate == "90")
            {
                rotateFlipType = RotateFlipType.Rotate90FlipX;
            }
            else if (flip == "XY" && rotate == "90")
            {
                rotateFlipType = RotateFlipType.Rotate90FlipXY;
            }
            else if (flip == "Y" && rotate == "90")
            {
                rotateFlipType = RotateFlipType.Rotate90FlipY;
            }
            else if (flip == "None" && rotate == "None")
            {
                rotateFlipType = RotateFlipType.RotateNoneFlipNone;
            }
            else if (flip == "X" && rotate == "None")
            {
                rotateFlipType = RotateFlipType.RotateNoneFlipX;
            }
            else if (flip == "XY" && rotate == "None")
            {
                rotateFlipType = RotateFlipType.RotateNoneFlipXY;
            }
            else if (flip == "Y" && rotate == "None")
            {
                rotateFlipType = RotateFlipType.RotateNoneFlipY;
            }

            original.RotateFlip(rotateFlipType);
            return original;
        }

        /// <summary>
        /// Resize a given image
        /// </summary>
        /// <param name="original">Original Bitmap that needs resizing</param>
        /// <param name="newWidth">New width of Esperantus.Esperantus.Localize. bitmap</param>
        /// <param name="newHeight">New height of Esperantus.Esperantus.Localize. bitmap</param>
        /// <param name="option">Option for resizing</param>
        /// <returns></returns>
        private Bitmap ResizeImage(Bitmap original, int newWidth, int newHeight, Pictures.ResizeOption option)
        {
            if (original.Width == 0 || original.Height == 0 || newWidth == 0 || newHeight == 0) return null;
            if (original.Width < newWidth) newWidth = original.Width;
            if (original.Height < newHeight) newHeight = original.Height;

            switch (option)
            {
                case Pictures.ResizeOption.NoResize:
                    newWidth = original.Width;
                    newHeight = original.Height;
                    break;
                case Pictures.ResizeOption.FixedWidthHeight:
                    break;
                case Pictures.ResizeOption.MaintainAspectWidth:
                    newHeight = (newWidth*original.Height/original.Width);
                    break;
                case Pictures.ResizeOption.MaintainAspectHeight:
                    newWidth = (newHeight*original.Width/original.Height);
                    break;
            }
            Bitmap newBitmap = new Bitmap(newWidth, newHeight);
            Graphics g = Graphics.FromImage(newBitmap);
            g.DrawImage(original, 0, 0, newWidth, newHeight);
            return newBitmap;
        }

        /// <summary>
        /// Handles the Init event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void Page_Init(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Thumbnails the callback.
        /// </summary>
        /// <returns></returns>
        public bool ThumbnailCallback()
        {
            return false;
        }

        /// <summary>
        /// Sets the exif information.
        /// </summary>
        /// <param name="bitmap">The bitmap.</param>
        private void SetExifInformation(Bitmap bitmap)
        {
            foreach (PropertyItem pi in bitmap.PropertyItems)
            {
                switch (pi.Id)
                {
                    case 0x010F:
                        SetMetadata("EquipMake", ConvertByteArrayToString(pi.Value));
                        break;
                    case 0x0110:
                        SetMetadata("EquipModel", ConvertByteArrayToString(pi.Value));
                        break;
                    case 0x0112:
                        switch (ConvertByteArrayToShort(pi.Value))
                        {
                            case 1:
                                SetMetadata("Orientation", "upper left");
                                break;
                            case 2:
                                SetMetadata("Orientation", "upper right");
                                break;
                            case 3:
                                SetMetadata("Orientation", "lower right");
                                break;
                            case 4:
                                SetMetadata("Orientation", "lower left");
                                break;
                            case 5:
                                SetMetadata("Orientation", "upper left flipped");
                                break;
                            case 6:
                                SetMetadata("Orientation", "upper right flipped");
                                break;
                            case 7:
                                SetMetadata("Orientation", "lower right flipped");
                                break;
                            case 8:
                                SetMetadata("Orientation", "lower left flipped");
                                break;
                        }
                        break;
                    case 0x011a:
                        SetMetadata("XResolution", ConvertByteArrayToRational(pi.Value));
                        break;
                    case 0x011b:
                        SetMetadata("YResolution", ConvertByteArrayToRational(pi.Value));
                        break;
                    case 0x0128:
                        SetMetadata("ResolutionUnit", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x0132:
                        SetMetadata("Datetime", ConvertByteArrayToString(pi.Value));
                        break;
                    case 0x0213:
                        SetMetadata("YCbCrPositioning", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x00FE:
                        SetMetadata("NewSubfileType", ConvertByteArrayToLong(pi.Value).ToString());
                        break;
                    case 0x00FF:
                        SetMetadata("SubfileType", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x0100:
                        SetMetadata("ImageWidth", ConvertByteArrayToLong(pi.Value).ToString());
                        break;
                    case 0x0101:
                        SetMetadata("ImageHeight", ConvertByteArrayToLong(pi.Value).ToString());
                        break;
                    case 0x0102:
                        SetMetadata("BitsPerSample", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x0103:
                        SetMetadata("Compression", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x0106:
                        SetMetadata("PhotometricInterp", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x0107:
                        SetMetadata("ThreshHolding", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x0108:
                        SetMetadata("CellWidth", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x0109:
                        SetMetadata("CellHeight", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x010A:
                        SetMetadata("FillOrder", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x010D:
                        SetMetadata("DocumentName", ConvertByteArrayToString(pi.Value));
                        break;
                    case 0x010E:
                        SetMetadata("ImageDescription", ConvertByteArrayToString(pi.Value));
                        break;
                    case 0x0111:
                        SetMetadata("StripOffsets", ConvertByteArrayToLong(pi.Value).ToString());
                        break;
                    case 0x0115:
                        SetMetadata("SamplesPerPixel", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x0116:
                        SetMetadata("RowsPerStrip", ConvertByteArrayToLong(pi.Value).ToString());
                        break;
                    case 0x0117:
                        SetMetadata("StripBytesCount", ConvertByteArrayToLong(pi.Value).ToString());
                        break;
                    case 0x0118:
                        SetMetadata("MinSampleValue", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x0119:
                        SetMetadata("MaxSampleValue", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x011C:
                        SetMetadata("PlanarConfig", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x011D:
                        SetMetadata("PageName", ConvertByteArrayToString(pi.Value));
                        break;
                    case 0x011E:
                        SetMetadata("XPosition", ConvertByteArrayToRational(pi.Value));
                        break;
                    case 0x011F:
                        SetMetadata("YPosition", ConvertByteArrayToRational(pi.Value));
                        break;
                    case 0x0120:
                        SetMetadata("FreeOffset", ConvertByteArrayToLong(pi.Value).ToString());
                        break;
                    case 0x0121:
                        SetMetadata("FreeByteCounts", ConvertByteArrayToLong(pi.Value).ToString());
                        break;
                    case 0x0122:
                        SetMetadata("GrayResponseUnit", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x0123:
                        SetMetadata("GrayResponseCurve", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x0124:
                        SetMetadata("T4Option", ConvertByteArrayToLong(pi.Value).ToString());
                        break;
                    case 0x0125:
                        SetMetadata("T6Option", ConvertByteArrayToLong(pi.Value).ToString());
                        break;
                    case 0x0129:
                        SetMetadata("PageNumber", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x012D:
                        SetMetadata("TransferFunction", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x0131:
                        SetMetadata("SoftwareUsed", ConvertByteArrayToString(pi.Value));
                        break;
                    case 0x013B:
                        SetMetadata("Artist", ConvertByteArrayToString(pi.Value));
                        break;
                    case 0x013C:
                        SetMetadata("HostComputer", ConvertByteArrayToString(pi.Value));
                        break;
                    case 0x013D:
                        SetMetadata("Predictor", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x013E:
                        SetMetadata("WhitePoint", ConvertByteArrayToRational(pi.Value));
                        break;
                    case 0x013F:
                        SetMetadata("PrimaryChromaticities", ConvertByteArrayToRational(pi.Value));
                        break;
                    case 0x0140:
                        SetMetadata("ColorMap", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x0141:
                        SetMetadata("HalftoneHints", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x0142:
                        SetMetadata("TileWidth", ConvertByteArrayToLong(pi.Value).ToString());
                        break;
                    case 0x0143:
                        SetMetadata("TileLength", ConvertByteArrayToLong(pi.Value).ToString());
                        break;
                    case 0x0144:
                        SetMetadata("TileOffset", ConvertByteArrayToLong(pi.Value).ToString());
                        break;
                    case 0x0145:
                        SetMetadata("TileByteCounts", ConvertByteArrayToLong(pi.Value).ToString());
                        break;
                    case 0x014C:
                        SetMetadata("InkSet", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x014D:
                        SetMetadata("InkNames", ConvertByteArrayToString(pi.Value));
                        break;
                    case 0x014E:
                        SetMetadata("NumberOfInks", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x0150:
                        SetMetadata("DotRange", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x0151:
                        SetMetadata("TargetPrinter", ConvertByteArrayToString(pi.Value));
                        break;
                    case 0x0152:
                        SetMetadata("ExtraSamples", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x0153:
                        SetMetadata("SampleFormat", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x0156:
                        SetMetadata("TransferRange", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x0200:
                        SetMetadata("JPEGProc", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x0201:
                        SetMetadata("JPEGInterFormat", ConvertByteArrayToLong(pi.Value).ToString());
                        break;
                    case 0x0202:
                        SetMetadata("JPEGInterLength", ConvertByteArrayToLong(pi.Value).ToString());
                        break;
                    case 0x0203:
                        SetMetadata("JPEGRestartInterval", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x0205:
                        SetMetadata("JPEGLosslessPredictors", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x0206:
                        SetMetadata("JPEGPointTransforms", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x0207:
                        SetMetadata("JPEGQTables", ConvertByteArrayToLong(pi.Value).ToString());
                        break;
                    case 0x0208:
                        SetMetadata("JPEGDCTables", ConvertByteArrayToLong(pi.Value).ToString());
                        break;
                    case 0x0209:
                        SetMetadata("JPEGACTables", ConvertByteArrayToLong(pi.Value).ToString());
                        break;
                    case 0x0211:
                        SetMetadata("YCbCrCoefficients", ConvertByteArrayToRational(pi.Value));
                        break;
                    case 0x0212:
                        SetMetadata("YCbCrSubsampling", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x0214:
                        SetMetadata("REFBlackWhite", ConvertByteArrayToRational(pi.Value));
                        break;
                    case 0x0301:
                        SetMetadata("Gamma", ConvertByteArrayToRational(pi.Value));
                        break;
                    case 0x0302:
                        SetMetadata("ICCProfileDescriptor", ConvertByteArrayToString(pi.Value));
                        break;
                    case 0x0303:
                        switch (ConvertByteArrayToShort(pi.Value))
                        {
                            case 0:
                                SetMetadata("SRGBRenderingIntent", "perceptual");
                                break;
                            case 1:
                                SetMetadata("SRGBRenderingIntent", "relative colorimetric");
                                break;
                            case 2:
                                SetMetadata("SRGBRenderingIntent", "saturation");
                                break;
                            case 3:
                                SetMetadata("SRGBRenderingIntent", "absolute colorimetric");
                                break;
                        }
                        break;
                    case 0x0320:
                        SetMetadata("ImageTitle", ConvertByteArrayToString(pi.Value));
                        break;
                    case 0x5001:
                        SetMetadata("ResolutionXUnit", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x5002:
                        SetMetadata("ResolutionYUnit", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x5003:
                        SetMetadata("ResolutionXLengthUnit", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x5004:
                        SetMetadata("ResolutionYLengthUnit", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x5005:
                        SetMetadata("PrintFlags", ConvertByteArrayToString(pi.Value));
                        break;
                    case 0x5006:
                        SetMetadata("PrintFlagsVersion", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x5007:
                        SetMetadata("PrintFlagsCrop", ConvertByteArrayToByte(pi.Value).ToString());
                        break;
                    case 0x5008:
                        SetMetadata("PrintFlagsBleedWidth", ConvertByteArrayToLong(pi.Value).ToString());
                        break;
                    case 0x5009:
                        SetMetadata("PrintFlagsBleedWidthScale", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x500A:
                        SetMetadata("HalftoneLPI", ConvertByteArrayToRational(pi.Value));
                        break;
                    case 0x500B:
                        SetMetadata("HalftoneLPIUnit", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x500C:
                        SetMetadata("HalftoneDegree", ConvertByteArrayToRational(pi.Value));
                        break;
                    case 0x500D:
                        SetMetadata("HalftoneShape", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x500E:
                        SetMetadata("HalftoneMisc", ConvertByteArrayToLong(pi.Value).ToString());
                        break;
                    case 0x500F:
                        SetMetadata("HalftoneScreen", ConvertByteArrayToByte(pi.Value).ToString());
                        break;
                    case 0x5010:
                        SetMetadata("JPEGQuality", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x5012:
                        SetMetadata("ThumbnailFormat", ConvertByteArrayToLong(pi.Value).ToString());
                        break;
                    case 0x5013:
                        SetMetadata("ThumbnailWidth", ConvertByteArrayToLong(pi.Value).ToString());
                        break;
                    case 0x5014:
                        SetMetadata("ThumbnailHeight", ConvertByteArrayToLong(pi.Value).ToString());
                        break;
                    case 0x5015:
                        SetMetadata("ThumbnailColorDepth", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x5016:
                        SetMetadata("ThumbnailPlanes", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x5017:
                        SetMetadata("ThumbnailRawBytes", ConvertByteArrayToLong(pi.Value).ToString());
                        break;
                    case 0x5018:
                        SetMetadata("ThumbnailSize", ConvertByteArrayToLong(pi.Value).ToString());
                        break;
                    case 0x5019:
                        SetMetadata("ThumbnailCompressedSize", ConvertByteArrayToLong(pi.Value).ToString());
                        break;
                    case 0x501B:
                        SetMetadata("ThumbnailData", ConvertByteArrayToByte(pi.Value).ToString());
                        break;
                    case 0x5020:
                        SetMetadata("ThumbnailImageWidth", ConvertByteArrayToLong(pi.Value).ToString());
                        break;
                    case 0x5021:
                        SetMetadata("ThumbnailImageHeight", ConvertByteArrayToLong(pi.Value).ToString());
                        break;
                    case 0x5022:
                        SetMetadata("ThumbnailBitsPerSample", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x5023:
                        SetMetadata("ThumbnailCompression", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x5024:
                        SetMetadata("ThumbnailPhotometricInterp", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x5025:
                        SetMetadata("ThumbnailImageDescription", ConvertByteArrayToString(pi.Value));
                        break;
                    case 0x5026:
                        SetMetadata("ThumbnailEquipMake", ConvertByteArrayToString(pi.Value));
                        break;
                    case 0x5027:
                        SetMetadata("ThumbnailEquipModel", ConvertByteArrayToString(pi.Value));
                        break;
                    case 0x5028:
                        SetMetadata("ThumbnailStripOffsets", ConvertByteArrayToLong(pi.Value).ToString());
                        break;
                    case 0x5029:
                        SetMetadata("ThumbnailOrientation", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x502A:
                        SetMetadata("ThumbnailSamplesPerPixel", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x502B:
                        SetMetadata("ThumbnailRowsPerStrip", ConvertByteArrayToLong(pi.Value).ToString());
                        break;
                    case 0x502C:
                        SetMetadata("ThumbnailStripBytesCount", ConvertByteArrayToLong(pi.Value).ToString());
                        break;
                    case 0x502F:
                        SetMetadata("ThumbnailPlanarConfig", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x5030:
                        SetMetadata("ThumbnailResolutionUnit", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x5031:
                        SetMetadata("ThumbnailTransferFunction", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x5032:
                        SetMetadata("ThumbnailSoftwareUsed", ConvertByteArrayToString(pi.Value));
                        break;
                    case 0x5033:
                        SetMetadata("ThumbnailDateTime", ConvertByteArrayToString(pi.Value));
                        break;
                    case 0x5034:
                        SetMetadata("ThumbnailArtist", ConvertByteArrayToString(pi.Value));
                        break;
                    case 0x5035:
                        SetMetadata("ThumbnailWhitePoint", ConvertByteArrayToRational(pi.Value));
                        break;
                    case 0x5036:
                        SetMetadata("ThumbnailPrimaryChromaticities", ConvertByteArrayToRational(pi.Value));
                        break;
                    case 0x5037:
                        SetMetadata("ThumbnailYCbCrCoefficients", ConvertByteArrayToRational(pi.Value));
                        break;
                    case 0x5038:
                        SetMetadata("ThumbnailYCbCrSubsampling", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x5039:
                        SetMetadata("ThumbnailYCbCrPositioning", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x503A:
                        SetMetadata("ThumbnailRefBlackWhite", ConvertByteArrayToRational(pi.Value));
                        break;
                    case 0x503B:
                        SetMetadata("ThumbnailCopyRight", ConvertByteArrayToString(pi.Value));
                        break;
                    case 0x5090:
                        SetMetadata("LuminanceTable", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x5091:
                        SetMetadata("ChrominanceTable", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x5100:
                        SetMetadata("FrameDelay", ConvertByteArrayToLong(pi.Value).ToString());
                        break;
                    case 0x5101:
                        SetMetadata("LoopCount", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x5110:
                        SetMetadata("PixelUnit", ConvertByteArrayToByte(pi.Value).ToString());
                        break;
                    case 0x5111:
                        SetMetadata("PixelPerUnitX", ConvertByteArrayToLong(pi.Value).ToString());
                        break;
                    case 0x5112:
                        SetMetadata("PixelPerUnitY", ConvertByteArrayToLong(pi.Value).ToString());
                        break;
                    case 0x5113:
                        SetMetadata("PaletteHistogram", ConvertByteArrayToByte(pi.Value).ToString());
                        break;
                    case 0x8298:
                        SetMetadata("Copyright", ConvertByteArrayToString(pi.Value));
                        break;
                    case 0x829A:
                        SetMetadata("ExifExposureTime", ConvertByteArrayToRational(pi.Value));
                        break;
                    case 0x829D:
                        SetMetadata("ExifFNumber", ConvertByteArrayToRational(pi.Value));
                        break;
                    case 0x8769:
                        SetMetadata("ExifIFD", ConvertByteArrayToLong(pi.Value).ToString());
                        break;
                    case 0x8773:
                        SetMetadata("ICCProfile", ConvertByteArrayToByte(pi.Value).ToString());
                        break;
                    case 0x8822:
                        switch (ConvertByteArrayToShort(pi.Value))
                        {
                            case 0:
                                SetMetadata("ExifExposureProg", "Not defined");
                                break;
                            case 1:
                                SetMetadata("ExifExposureProg", "Manual");
                                break;
                            case 2:
                                SetMetadata("ExifExposureProg", "Normal Program");
                                break;
                            case 3:
                                SetMetadata("ExifExposureProg", "Aperture Priority");
                                break;
                            case 4:
                                SetMetadata("ExifExposureProg", "Shutter Priority");
                                break;
                            case 5:
                                SetMetadata("ExifExposureProg", "Creative program (biased toward depth of field)");
                                break;
                            case 6:
                                SetMetadata("ExifExposureProg", "Action program (biased toward fast shutter speed)");
                                break;
                            case 7:
                                SetMetadata("ExifExposureProg",
                                            "Portrait mode (for close-up photos with Esperantus.Esperantus.Localize. background out of focus)");
                                break;
                            case 8:
                                SetMetadata("ExifExposureProg",
                                            "Landscape mode (for landscape photos with Esperantus.Esperantus.Localize. background in focus)");
                                break;
                            default:
                                SetMetadata("ExifExposureProg", "Unknown");
                                break;
                        }
                        break;
                    case 0x8824:
                        SetMetadata("ExifSpectralSense", ConvertByteArrayToString(pi.Value));
                        break;
                    case 0x8825:
                        SetMetadata("GpsIFD", ConvertByteArrayToLong(pi.Value).ToString());
                        break;
                    case 0x8827:
                        SetMetadata("ExifISOSpeed", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0x9003:
                        SetMetadata("ExifDTOrig", ConvertByteArrayToString(pi.Value));
                        break;
                    case 0x9004:
                        SetMetadata("ExifDTDigitized", ConvertByteArrayToString(pi.Value));
                        break;
                    case 0x9102:
                        SetMetadata("ExifCompBPP", ConvertByteArrayToRational(pi.Value));
                        break;
                    case 0x9201:
                        SetMetadata("ExifShutterSpeed", ConvertByteArrayToSRational(pi.Value));
                        break;
                    case 0x9202:
                        SetMetadata("ExifAperture", ConvertByteArrayToRational(pi.Value));
                        break;
                    case 0x9203:
                        SetMetadata("ExifBrightness", ConvertByteArrayToSRational(pi.Value));
                        break;
                    case 0x9204:
                        SetMetadata("ExifExposureBias", ConvertByteArrayToSRational(pi.Value));
                        break;
                    case 0x9205:
                        SetMetadata("ExifMaxAperture", ConvertByteArrayToRational(pi.Value));
                        break;
                    case 0x9206:
                        SetMetadata("ExifSubjectDist", ConvertByteArrayToRational(pi.Value));
                        break;
                    case 0x9207:
                        switch (ConvertByteArrayToShort(pi.Value))
                        {
                            case 0:
                                SetMetadata("ExifMeteringMode", "Unknown");
                                break;
                            case 1:
                                SetMetadata("ExifMeteringMode", "Average");
                                break;
                            case 2:
                                SetMetadata("ExifMeteringMode", "CenterWeightedAverage");
                                break;
                            case 3:
                                SetMetadata("ExifMeteringMode", "Spot");
                                break;
                            case 4:
                                SetMetadata("ExifMeteringMode", "MultiSpot");
                                break;
                            case 5:
                                SetMetadata("ExifMeteringMode", "Pattern");
                                break;
                            case 6:
                                SetMetadata("ExifMeteringMode", "Partial");
                                break;
                            case 255:
                                SetMetadata("ExifMeteringMode", "OEsperantus.Esperantus.Localize.r");
                                break;
                            default:
                                SetMetadata("ExifMeteringMode", "Unknown");
                                break;
                        }
                        break;
                    case 0x9208:
                        switch (ConvertByteArrayToShort(pi.Value))
                        {
                            case 0:
                                SetMetadata("ExifLightSource", "Unknown");
                                break;
                            case 1:
                                SetMetadata("ExifLightSource", "Daylight");
                                break;
                            case 2:
                                SetMetadata("ExifLightSource", "Flourescent");
                                break;
                            case 3:
                                SetMetadata("ExifLightSource", "Tungsten");
                                break;
                            case 17:
                                SetMetadata("ExifLightSource", "Standard Light A");
                                break;
                            case 18:
                                SetMetadata("ExifLightSource", "Standard Light B");
                                break;
                            case 19:
                                SetMetadata("ExifLightSource", "Standard Light C");
                                break;
                            case 20:
                                SetMetadata("ExifLightSource", "D55");
                                break;
                            case 21:
                                SetMetadata("ExifLightSource", "D65");
                                break;
                            case 22:
                                SetMetadata("ExifLightSource", "D75");
                                break;
                            case 255:
                                SetMetadata("ExifLightSource", "OEsperantus.Esperantus.Localize.r");
                                break;
                            default:
                                SetMetadata("ExifLightSource", "Unknown");
                                break;
                        }
                        break;
                    case 0x9209:
                        switch (ConvertByteArrayToShort(pi.Value))
                        {
                            case 0:
                                SetMetadata("ExifFlash", "Flash did not fire");
                                break;
                            case 1:
                                SetMetadata("ExifFlash", "Flash fired");
                                break;
                            case 5:
                                SetMetadata("ExifFlash", "Strobe return light not detected");
                                break;
                            default:
                                SetMetadata("ExifFlash", "Uknown");
                                break;
                        }
                        break;
                    case 0x920A:
                        SetMetadata("ExifFocalLength", ConvertByteArrayToRational(pi.Value));
                        break;
                    case 0x9290:
                        SetMetadata("ExifDTSubsec", ConvertByteArrayToString(pi.Value));
                        break;
                    case 0x9291:
                        SetMetadata("ExifDTOrigSS", ConvertByteArrayToString(pi.Value));
                        break;
                    case 0x9292:
                        SetMetadata("ExifDTDigSS", ConvertByteArrayToString(pi.Value));
                        break;
                    case 0xA001:
                        switch (ConvertByteArrayToLong(pi.Value))
                        {
                            case 0x1:
                                SetMetadata("ExifColorSpace", "sRGB");
                                break;
                            case 0xFFFF:
                                SetMetadata("ExifColorSpace", "Uncalibrated");
                                break;
                            default:
                                SetMetadata("ExifColorSpace", "Reserved");
                                break;
                        }
                        break;
                    case 0xA002:
                        SetMetadata("ExifPixXDim", ConvertByteArrayToLong(pi.Value).ToString());
                        break;
                    case 0xA003:
                        SetMetadata("ExifPixYDim", ConvertByteArrayToLong(pi.Value).ToString());
                        break;
                    case 0xA004:
                        SetMetadata("ExifRelatedWav", ConvertByteArrayToString(pi.Value));
                        break;
                    case 0xA005:
                        SetMetadata("ExifInterop", ConvertByteArrayToLong(pi.Value).ToString());
                        break;
                    case 0xA20B:
                        SetMetadata("ExifFlashEnergy", ConvertByteArrayToRational(pi.Value));
                        break;
                    case 0xA20E:
                        SetMetadata("ExifFocalXRes", ConvertByteArrayToRational(pi.Value));
                        break;
                    case 0xA20F:
                        SetMetadata("ExifFocalYRes", ConvertByteArrayToRational(pi.Value));
                        break;
                    case 0xA210:
                        switch (ConvertByteArrayToShort(pi.Value))
                        {
                            case 2:
                                SetMetadata("ExifFocalResUnit", "Inch");
                                break;
                            case 3:
                                SetMetadata("ExifFocalResUnit", "Centimeter");
                                break;
                        }
                        break;
                    case 0xA214:
                        SetMetadata("ExifSubjectLoc", ConvertByteArrayToShort(pi.Value).ToString());
                        break;
                    case 0xA215:
                        SetMetadata("ExifExposureIndex", ConvertByteArrayToRational(pi.Value));
                        break;
                    case 0xA217:
                        switch (ConvertByteArrayToShort(pi.Value))
                        {
                            case 1:
                                SetMetadata("ExifSensingMethod", "Not defined");
                                break;
                            case 2:
                                SetMetadata("ExifSensingMethod", "One-chip color area sensor");
                                break;
                            case 3:
                                SetMetadata("ExifSensingMethod", "Two-chip color area sensor");
                                break;
                            case 4:
                                SetMetadata("ExifSensingMethod", "Three-chip color area sensor");
                                break;
                            case 5:
                                SetMetadata("ExifSensingMethod", "Color sequential area sensor");
                                break;
                            case 7:
                                SetMetadata("ExifSensingMethod", "Trilinear sensor");
                                break;
                            case 8:
                                SetMetadata("ExifSensingMethod", "Color sequential linear sensor");
                                break;
                            default:
                                SetMetadata("ExifSensingMethod", "Reserved");
                                break;
                        }
                        break;
                }
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