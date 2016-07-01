// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EvolAdvModSettings.aspx.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The html edit.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.DesktopModules.CoreModules.EvolutilityAdvanced.ModuleRenderer
{
    using Appleseed.Framework;
    using Appleseed.Framework.Site.Data;
    using Appleseed.Framework.Web.UI;
    using Appleseed.Framework.Web.UI.WebControls;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    /// The Evolutility advanced Module renderer settings.
    /// </summary>
    [History("Ashish Patel","2014/09/06","Evolutility Advanced Module settings")]
    public partial class EvolAdvModSettings : EditItemPage
    {
        /// <summary>
        /// Add buttons 
        /// </summary>
        /// <param name="e">That contain Events </param>
        protected override void OnInit(EventArgs e)
        {
            // Controls must be created here
            this.UpdateButton = new Appleseed.Framework.Web.UI.WebControls.LinkButton();
            this.CancelButton = new Appleseed.Framework.Web.UI.WebControls.LinkButton();

            this.UpdateButton.CssClass = "CommandButton";
            this.PlaceHolderButtons.Controls.Add(this.UpdateButton);
            this.PlaceHolderButtons.Controls.Add(new LiteralControl("&#160;"));
            this.CancelButton.CssClass = "CommandButton";
            this.PlaceHolderButtons.Controls.Add(this.CancelButton);
            base.OnInit(e);
        }

        /// <summary>
        /// The Page_Load even on this page is used to get Model Settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">That contains Event </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            EvolutilityModuleDB evolSettings = new EvolutilityModuleDB();
            SqlDataReader reader = evolSettings.EvolutilityAdvGetModelSettings(Convert.ToInt32(Request.QueryString["mid"]));
            if (reader.Read())
            {
                txtModelID.Text = reader["ModelID"].ToString();
                txtModelLabel.Text = reader["ModelLabel"].ToString();
                txtEntity.Text = reader["ModelEntity"].ToString();
                txtEntities.Text = reader["ModelEntities"].ToString();
                txtLeadFields.Text = reader["ModelLeadField"].ToString();
                txtElements.Text = reader["ModelElements"].ToString();
            }
            reader.Close();
        }

        /// <summary>
        /// The UpdateBtn_Click event handler on this Page is used to save
        ///   the text changes to the database.
        /// </summary>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        protected override void OnUpdate(EventArgs e)
        {
            base.OnUpdate(e);
            EvolutilityModuleDB evolSettings = new EvolutilityModuleDB();
            evolSettings.EvolutilityAdvUpdateModelSettings(Convert.ToInt32(Request.QueryString["mid"]), txtModelID.Text, txtModelLabel.Text, txtEntity.Text, txtEntities.Text, txtLeadFields.Text, txtElements.Text);

            if (Request.QueryString.GetValues("ModalChangeMaster") != null)
                Response.Write("<script type=\"text/javascript\">window.parent.location = window.parent.location.href;</script>");
            else
                this.RedirectBackToReferringPage();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnCancel(EventArgs e)
        {
            base.OnCancel(e);
        }

    }
}