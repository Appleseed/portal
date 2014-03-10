using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Appleseed.Framework.Web.UI;
using Appleseed.Framework.Security;
using Appleseed.Framework.Providers.AppleseedMembershipProvider;
using Appleseed.Framework;
using Appleseed.Framework.Helpers;


namespace Appleseed.DesktopModules.CoreModules.Admin
{
    public partial class ChangePassword : Appleseed.Framework.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            SaveBtn.Click += new EventHandler(SaveBtn_Click);
            CancelBtn.Click += new EventHandler(CancelBtn_Click);
            GoHomeBtn.Click += new EventHandler(GoHomeBtn_Click);
        }

        private Guid userId = Guid.Empty;
        private Guid token = Guid.Empty;


        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Request.QueryString["usr"] == null || Request.QueryString["tok"] == null)
            {
                ShowError("CHANGE_PWD_INVALID_URL_ERROR", "You are not allowed to use this functionality witout the correct parameters.");
                return;
            }
            if (!Guid.TryParse(Request.QueryString["usr"].ToString(), out userId) ||
                !Guid.TryParse(Request.QueryString["tok"].ToString(), out token))
            {
                ShowError("CHANGE_PWD_INVALID_URL_ERROR", "You are not allowed to use this functionality witout the correct parameters.");
                return;
            }
            if (!Page.IsPostBack)
            {
                Membership.ApplicationName = this.PortalSettings.PortalAlias;
                var membership = (AppleseedMembershipProvider)Membership.Provider;
                if (!membership.VerifyTokenForUser(userId, token))
                {
                    ShowError("CHANGE_PWD_NO_TOKEN_ERROR", "The link that brought you here either was not created by us, or it it was already used for changing that account password.");
                    return;
                }
                lblMessage.Text = General.GetString("CHANGE_PWD_USR_EXPLANATION", "Insert your new password for your account. Once you save the changes you would be able to logon with it.");
            }
        }


        void GoHomeBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect(HttpUrlBuilder.BuildUrl());
        }


        void CancelBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect(HttpUrlBuilder.BuildUrl());
        }


        void SaveBtn_Click(object sender, EventArgs e)
        {
            if (txtPass.Text != txtPass2.Text) {
                lblMessage.Text = General.GetString("CHANGE_PWD_NOT_SAME_TWICE_ERROR", "The second password entered is not the same as the first one. Please write them again.");
                return;
            }
            try
            {
                Membership.ApplicationName = this.PortalSettings.PortalAlias;
                var membership = (AppleseedMembershipProvider)Membership.Provider;
                var user = membership.GetUser(userId, false);
                if (!membership.ChangePassword(user.UserName, token, txtPass.Text))
                {
                    throw new ApplicationException("Error while trying to change the user password");
                }
                lblMessage.Text = General.GetString("CHANGE_PWD_NEW_PWD_SET_MSG", "Your new password has been set. You can logon with it now.");
                SaveBtn.Visible = false;
                CancelBtn.Visible = false;
                trFields.Visible = false;
                GoHomeBtn.Visible = true;
            }
            catch (Exception ex)
            {
                ShowError("CHANGE_PWD_UNEXPECTED_ERROR", "An error ocurred while trying to update your password.");
                LogHelper.Logger.Log(
                    LogLevel.Error, 
                    string.Format(@"Error while trying to update the password for user with id '{0}'. Token used: '{1}'.", userId, token)
                    , ex);
            }

        }





        private void ShowError(string errorKey, string defaultValue)
        {
            lblMessage.Text = General.GetString(errorKey, defaultValue);
            trFields.Visible = false;
            SaveBtn.Visible = false;
            CancelBtn.Visible = false;
            GoHomeBtn.Visible = true;
        }
    }
}