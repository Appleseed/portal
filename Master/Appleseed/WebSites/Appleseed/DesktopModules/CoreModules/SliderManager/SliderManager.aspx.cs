using Appleseed.Framework.Configuration.Items;
using Appleseed.Framework.Site.Data;
using Appleseed.Framework.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Appleseed.DesktopModules.CoreModules.SliderManager
{
    public partial class SliderManagerPage : EditItemPage
    {
        SliderDB sdb = new SliderDB();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSliders();
            }
        }

        private void LoadSliders()
        {
            gdvSliders.DataSource = sdb.AllSliders(this.ModuleID);
            gdvSliders.DataBind();
        }

        protected void lnkAddNew_Click(object sender, EventArgs e)
        {
            hidAddEditId.Value = 0.ToString();
            txtBGColor.Text = string.Empty;
            txtBGImageUrl.Text = string.Empty;
            txtClientFirstName.Text = string.Empty;
            txtClientLastName.Text = string.Empty;
            txtClientQuote.Text = string.Empty;
            txtClientWorkPosition.Text = string.Empty;
            plcAddEdit.Visible = true;
            plcSliderList.Visible = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SliderItem slider = new SliderItem()
                {
                    BackgroudColor = txtBGColor.Text,
                    BackgroudImageUrl = txtBGImageUrl.Text,
                    ClientFirstName = txtClientFirstName.Text,
                    ClientLastName = txtClientLastName.Text,
                    ClientQuote = txtClientQuote.Text,
                    ClientWorkPosition = txtClientWorkPosition.Text,
                    ModuleId = this.ModuleID,
                    Id = Convert.ToInt32(hidAddEditId.Value)
                };
                if (slider.Id > 0)
                {
                    sdb.UpdateSlider(slider);
                }
                else
                {
                    sdb.AddNewSlider(slider);
                }

                Response.Redirect(Request.Url.PathAndQuery);
            }
        }



        protected void gdvSliders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EDIT")
            {
                var slider = sdb.GetSliderByID(Convert.ToInt32(e.CommandArgument.ToString()));
                hidAddEditId.Value = slider.Id.ToString();
                txtBGColor.Text = slider.BackgroudColor;
                txtBGImageUrl.Text = slider.BackgroudImageUrl;
                txtClientFirstName.Text = slider.ClientFirstName;
                txtClientLastName.Text = slider.ClientLastName;
                txtClientQuote.Text = slider.ClientQuote;
                txtClientWorkPosition.Text = slider.ClientWorkPosition;
                plcAddEdit.Visible = true;
                plcSliderList.Visible = false;
            }
            else if (e.CommandName == "DELETE")
            {
                sdb.DeleteSlider(Convert.ToInt32(e.CommandArgument.ToString()));
                Response.Redirect(Request.Url.PathAndQuery);
            }
        }

        protected void gdvSliders_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
    }
}