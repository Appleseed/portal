// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SliderManager.aspx.cs">
//   Copyright © -- 2015. All Rights Reserved.
// </copyright>
// <summary>
//   Dynemic Sliders
// </summary>
//// <company>
//// HaptiX
//// </company>
// --------------------------------------------------------------------------------------------------------------------
namespace Appleseed.DesktopModules.CoreModules.SliderManager
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.UI.WebControls;
    using Appleseed.Framework;
    using Appleseed.Framework.Configuration.Items;
    using Appleseed.Framework.Site.Data;
    using Appleseed.Framework.Web;
    using Appleseed.Framework.Web.UI.WebControls;

    /// <summary>
    /// Slider Manager 
    /// </summary>
    [History("Ashish.patel@haptix.biz", "2015/02/10", "Slider Manager")]
    public partial class SliderManager : PortalModuleControl
    {
        /// <summary>
        /// GUID of module (mandatory)
        /// </summary>
        /// <value></value>
        public override Guid GuidID
        {
            get { return new Guid("{1010D514-73B2-4E46-B6A9-2C0182D22865}"); }
        }

        /// <summary>
        /// General object of Slider Class 
        /// </summary>
        private SliderDB slider = new SliderDB();

        /// <summary>
        /// Gets Slider detail by SliderID
        /// </summary>
        public SliderItem SliderDetailByID
        {
            get
            {
                SliderItem item = new SliderItem();
                if (!string.IsNullOrEmpty(Request.QueryString["slid"]))
                {
                    try
                    {
                        item = this.slider.GetSliderByID(Convert.ToInt32(Request.QueryString["slid"]));
                    }
                    catch { }
                }

                return item;
            }
        }

        /// <summary>
        /// Gets Return the Maximum Display order for given slider id
        /// </summary>
        public int MaxDisplayOrderBySliderID
        {
            get
            {
                int item = 0;
                if (!string.IsNullOrEmpty(Request.QueryString["slid"]))
                {
                    try
                    {
                        return item = this.slider.GetMaxDisplayOrder(Convert.ToInt32(Request.QueryString["slid"]));
                    }
                    catch { }
                }

                return item;
            }
        }

        /// <summary>
        /// Gets Slider ID
        /// </summary>
        protected int SliderID
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["slid"]))
                {
                    try
                    {
                        return Convert.ToInt32(Request.QueryString["slid"]);
                    }
                    catch {
                    }
                }

                return 0;
            }
        }

        /// <summary>
        /// Gets or sets SliderImage Item list
        /// </summary>
        protected List<SliderImageItem> SliderImageList { get; set; }

        /// <summary>
        /// Page Load
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.divErrorMessage.Visible = false;
                this.divSuccessMessage.Visible = false;
                this.LoadGrid();
            }
        }

        /// <summary>
        /// Slider Listing - Row Update
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.GridViewUpdateEventArgs"/> instance containing the event data.</param>
        protected void GdSliderManager_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = (GridViewRow)this.GdSliderManager.Rows[e.RowIndex];
            TextBox txtSliders = (TextBox)row.FindControl("txtSliders");
            System.Web.UI.WebControls.Label lblSliderID = (System.Web.UI.WebControls.Label)row.FindControl("lblSliderID");
            int sliderID = Convert.ToInt32(lblSliderID.Text);
            this.AddUpdateSlider(sliderID, txtSliders.Text.ToLower());
            this.GdSliderManager.EditIndex = -1;
            this.LoadGrid();
        }

        /// <summary>
        /// Update existing slider
        /// </summary>
        /// <param name="sliderID">Slider ID</param>
        /// <param name="sliderName">Slider Name</param>
        protected void AddUpdateSlider(int sliderID, string sliderName)
        {
            SliderItem item = new SliderItem();

            item.SliderID = sliderID;
            item.SliderName = sliderName;
            item.UpdateDate = System.DateTime.Now;
            item.UpdatedUserName = Appleseed.Framework.Site.Configuration.PortalSettings.CurrentUser.Identity.UserName;
            this.slider.UpdateSlider(item);
        }

        /// <summary>
        /// Slider Listing - Row Update
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.GridViewDeleteEventArgs"/> instance containing the event data.</param>
        protected void GdSliderManager_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow currentRow = (GridViewRow)this.GdSliderManager.Rows[e.RowIndex];
            System.Web.UI.WebControls.Label lblSliderID = (System.Web.UI.WebControls.Label)currentRow.FindControl("lblSliderID");
            this.slider.DeleteSlider(Convert.ToInt32(lblSliderID.Text));

            var path = Server.MapPath("/Images/Sliders/" + lblSliderID.Text);

            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            SqlUrlBuilderProvider.ClearCachePageUrl(this.PageID);
            UrlBuilderHelper.ClearUrlElements(this.PageID);

            this.LoadGrid();
        }

        /// <summary>
        /// Slider Listing - Row canceling Edit
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.GridViewCancelEditEventArgs"/> instance containing the event data.</param>
        protected void GdSliderManager_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            this.GdSliderManager.EditIndex = -1;
            this.LoadGrid();
        }

        /// <summary>
        /// Slider Listing - Row Editing
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.GridViewEditEventArgs"/> instance containing the event data.</param>
        protected void GdSliderManager_RowEditing(object sender, GridViewEditEventArgs e)
        {
            this.GdSliderManager.EditIndex = e.NewEditIndex;
            this.LoadGrid();
        }

        /// <summary>
        /// Slider Listing - Add New Slider
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected void BtnAddNewSlider_Click(object sender, EventArgs e)
        {
            // Get Check for Slider exist or not
            int result = this.slider.GetSliderByName(txtSliderName.Text);

            if (result > 0)
            {
                divErrorMessage.Visible = true;
            }
            else
            {
                SliderItem newSlider = new SliderItem();
                newSlider.SliderName = txtSliderName.Text;
                newSlider.CreatedDate = System.DateTime.Now;
                newSlider.CreatedUserName = Appleseed.Framework.Site.Configuration.PortalSettings.CurrentUser.Identity.UserName;
                var newSLiderID = this.slider.AddNewSlider(newSlider);

                var path = Server.MapPath("/Images/Sliders/" + newSLiderID);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                this.divErrorMessage.Visible = false;
                this.divSuccessMessage.Visible = true;

                this.LoadGrid();
            }
        }

        /// <summary>
        /// Slider Listing - Row Command
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.GridViewCommandEventArgs"/> instance containing the event data.</param>
        protected void GdSliderManager_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // If command equal to "Image" then it will display the List of Image
            if (e.CommandName == "Image")
            {
                int sliderid = Convert.ToInt32(e.CommandArgument);

                var path = Request.Url.AbsolutePath;
                Response.Redirect(path + "?slid=" + sliderid);
            }
        }

        /// <summary>
        /// Slider Image Listing - Row Editing
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.GridViewEditEventArgs"/> instance containing the event data.</param>
        protected void GdSliderImageManager_RowEditing(object sender, GridViewEditEventArgs e)
        {
            this.GdSliderImageManager.EditIndex = e.NewEditIndex;
            this.LoadGrid();
        }

        /// <summary>
        /// Slider Image Listing - Row Deleting
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.GridViewDeleteEventArgs"/> instance containing the event data.</param>
        protected void GdSliderImageManager_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow currentRow = (GridViewRow)this.GdSliderImageManager.Rows[e.RowIndex];
            System.Web.UI.WebControls.Label lblImageID = (System.Web.UI.WebControls.Label)currentRow.FindControl("lblImageID");
            var sliderImageDetail = this.slider.SliderIamgeDetail(Convert.ToInt32(lblImageID.Text));

            var path = Server.MapPath("/Images/Sliders/" + sliderImageDetail.SliderID + "/" + sliderImageDetail.SliderImageID + sliderImageDetail.SliderImageExt);
            File.Delete(path);

            this.slider.DeleteSliderImage(Convert.ToInt32(lblImageID.Text));
            SqlUrlBuilderProvider.ClearCachePageUrl(this.PageID);
            UrlBuilderHelper.ClearUrlElements(this.PageID);

            this.LoadGrid();
        }

        /// <summary>
        /// Slider Image Listing - Row Canceling Edit
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.GridViewCancelEditEventArgs"/> instance containing the event data.</param>
        protected void GdSliderImageManager_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            this.GdSliderImageManager.EditIndex = -1;
            this.LoadGrid();
        }

        /// <summary>
        /// Slider Image Listing - Row Data Bound
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void GdSliderImageManager_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Image thumbImage;
                SliderImageItem image = (SliderImageItem)e.Row.DataItem;
                if ((e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Alternate)) || (e.Row.RowState == DataControlRowState.Edit))
                {
                    thumbImage = (Image)e.Row.FindControl("imgSliderImageEdit") as Image;
                    thumbImage.ImageUrl = "/Images/Sliders/" + image.SliderID + "/" + image.SliderImageID + image.SliderImageExt + "?dt=" + DateTime.Now.ToString("ddMMyyyyHHmmss");
                }
                else if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate))
                {
                    thumbImage = (Image)e.Row.FindControl("imgSliderImage") as Image;
                    thumbImage.ImageUrl = "/Images/Sliders/" + image.SliderID + "/" + image.SliderImageID + image.SliderImageExt + "?dt=" + DateTime.Now.ToString("ddMMyyyyHHmmss");
                }
            }
        }

        /// <summary>
        /// Slider Image Listing - Row Updating
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.GridViewUpdateEventArgs"/> instance containing the event data.</param>
        protected void GdSliderImageManager_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = (GridViewRow)this.GdSliderImageManager.Rows[e.RowIndex];
            TextBox txtSliderCaption = (TextBox)row.FindControl("txtSliderCaption");
            CheckBox chkActive = (CheckBox)row.FindControl("chkActiveEdit");
            TextBox txtDisplayOrder = (TextBox)row.FindControl("txtDisplayOrder");

            System.Web.UI.WebControls.Label lblImageID = (System.Web.UI.WebControls.Label)row.FindControl("lblImageID");

            int sliderImageID = Convert.ToInt32(lblImageID.Text);

            var currentImage = this.slider.SliderIamgeDetail(sliderImageID);

            currentImage.SliderCaption = txtSliderCaption.Text;
            currentImage.Active = chkActive.Checked;
            currentImage.DisplayOrder = Convert.ToInt32(txtDisplayOrder.Text);
            currentImage.UpdatedDate = DateTime.Now;
            currentImage.UpdatedUserName = Appleseed.Framework.Site.Configuration.PortalSettings.CurrentUser.Identity.UserName;
            this.slider.UpdateImageDetail(currentImage);
            this.GdSliderImageManager.EditIndex = -1;
            this.LoadGrid();
        }

        /// <summary>
        /// Slider Image Listing - Row Command
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void GdSliderImageManager_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Upload")
            {
                this.SliderImageList = this.slider.SliderImages(this.SliderID).ToList();

                var currentImage = this.slider.SliderIamgeDetail(Convert.ToInt32(e.CommandArgument));

                for (int i = 0; i < this.SliderImageList.Count; i++)
                {
                    System.Web.UI.WebControls.Label lblImageID = (System.Web.UI.WebControls.Label)this.GdSliderImageManager.Rows[i].FindControl("lblImageID");

                    if (lblImageID.Text == e.CommandArgument.ToString())
                    {
                        FileUpload uploadImage = (FileUpload)this.GdSliderImageManager.Rows[i].FindControl("UploadImage");

                        string savepath = Server.MapPath("/Images/Sliders/" + this.SliderID + "/" + lblImageID.Text + Path.GetExtension(uploadImage.PostedFile.FileName));
                        uploadImage.SaveAs(savepath);
                        currentImage.SliderImageExt = Path.GetExtension(uploadImage.PostedFile.FileName);
                        currentImage.UpdatedDate = DateTime.Now;
                        currentImage.UpdatedUserName = Appleseed.Framework.Site.Configuration.PortalSettings.CurrentUser.Identity.UserName;
                        this.slider.UpdateImageDetail(currentImage);
                        this.LoadGrid();
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Slider Image Listing - Row Created
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void GdSliderImageManager_RowCreated(object sender, GridViewRowEventArgs e)
        {
            // Merge Two column in footer
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[6].ColumnSpan = 2;
            }
        }

        /// <summary>
        /// Add new Image into the Slider
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected void BtnAddNew_Click(object sender, EventArgs e)
        {
            SliderImageItem currentImage = new SliderImageItem();

            currentImage.SliderID = this.SliderID;
            currentImage.SliderCaption = txtSliderCaptionNew.Text;
            currentImage.SliderImageExt = Path.GetExtension(UploadNewImage.PostedFile.FileName);
            currentImage.CreatedDate = DateTime.Now;
            currentImage.CreatedUserName = Appleseed.Framework.Site.Configuration.PortalSettings.CurrentUser.Identity.UserName;
            currentImage.Active = this.chkActiveNew.Checked;
            currentImage.DisplayOrder = Convert.ToInt32(txtDisplayOrderNew.Text) > 0 ? Convert.ToInt32(txtDisplayOrderNew.Text) : 0;

            var newImageID = this.slider.SaveImageDetail(currentImage);
            string savepath = Server.MapPath("/Images/Sliders/" + this.SliderID + "/" + newImageID + Path.GetExtension(UploadNewImage.PostedFile.FileName));
            UploadNewImage.SaveAs(savepath);

            this.txtSliderCaptionNew.Text = string.Empty;
            this.txtDisplayOrderNew.Text = string.Empty;
            this.chkActiveNew.Checked = false;

            this.LoadGrid();
        }

        /// <summary>
        /// Return to Slider listing
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected void BtnReturn_Click(object sender, EventArgs e)
        {
            var queryString = HttpUtility.ParseQueryString(Request.Url.Query);
            queryString.Remove("slid");
            string url = Request.Url.AbsolutePath + "?" + queryString.ToString();
            Response.Redirect(url);
        }

        /// <summary>
        /// Load data into the Grid
        /// </summary>
        private void LoadGrid()
        {
            if (Appleseed.Framework.Security.PortalSecurity.HasEditPermissions(this.ModuleID))
            {
                panel1.Visible = this.SliderID == 0;
                panel2.Visible = this.SliderID > 0;

                // Divs are not being display until user not logged on
                this.DivAddNewImage.Visible = true;
                this.DivAddNewSLider.Visible = true;
                this.BtnReturn.Visible = true;
                this.TitleImage.Visible = true;
                this.TitleSlider.Visible = true;

                if (this.SliderID > 0)
                {
                    lblTitleSliderName.Text = this.SliderDetailByID.SliderName;
                    txtDisplayOrderNew.Text = this.MaxDisplayOrderBySliderID.ToString();
                    this.GdSliderImageManager.DataSource = this.slider.SliderImages(this.SliderID).ToList().OrderBy(ord => ord.DisplayOrder);
                    this.GdSliderImageManager.DataBind();
                }
                else
                {
                    this.GdSliderManager.DataSource = this.slider.AllSliders().ToList();
                    this.GdSliderManager.DataBind();
                }
            }
        }
    }
}