using System;
using System.Web.UI;
using System.Web.Mvc;

namespace Appleseed.Admin
{
    public partial class RequireRegistration : ViewUserControl
    {
        #region Web Form Designer generated code

        /// <summary>
        /// Raises the Init event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            SignInHyperLink.NavigateUrl = Appleseed.Framework.HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/Admin/Logon.aspx");
            RegisterHyperlink.NavigateUrl =
                Appleseed.Framework.HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/Register/Register.aspx");

            base.OnInit(e);
        }

        #endregion
    }
}