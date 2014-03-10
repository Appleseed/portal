// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Logon.aspx.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   Single click logon, useful for email and newsletters
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Admin
{
    using System;

    using Appleseed.Framework;
    using Appleseed.Framework.Security;
    using Appleseed.Framework.Web.UI;

    /// <summary>
    /// Single click logon, useful for email and newsletters
    /// </summary>
    public partial class LogonPage : Page
    {
        #region Methods

        /// <summary>
        /// Handles the OnInit event at Page level<br/>
        /// Performs OnInit events that are common to all Pages<br/>
        /// Can be overridden
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        /// <remarks></remarks>
        protected override void OnInit(EventArgs e)
        {
            this.Load += this.Page_Load;
            base.OnInit(e);
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="T:System.EventArgs"/> instance containing the event data.
        /// </param>
        private void Page_Load(object sender, EventArgs e)
        {
            var _user = string.Empty;
            var _password = string.Empty;
            var _alias = string.Empty;
            var _pageId = 0;

            // Get Login User from querystring
            if (this.Request.Params["usr"] != null)
            {
                _user = this.Request.Params["usr"];

                // Get Login Password from querystring
                if (this.Request.Params["pwd"] != null)
                {
                    _password = this.Request.Params["pwd"];
                }

                // Get portalaias
                if (this.Request.Params["alias"] != null)
                {
                    _alias = HttpUrlBuilder.BuildUrl("~/" + HttpUrlBuilder.DefaultPage, 0, string.Empty, this.Request.Params["alias"]);
                }

                if (this.Request.Params["pageId"] != null)
                {
                    try
                    {
                        _pageId = int.Parse(this.Request.Params["pageId"]);
                        _alias = HttpUrlBuilder.BuildUrl(_pageId);
                    }
                    catch
                    {
                        PortalSecurity.AccessDenied();
                    }
                }

                // try to validate logon
                if (PortalSecurity.SignOn(_user, _password, true, _alias) == null)
                {
                    // Login failed
                    PortalSecurity.AccessDenied();
                }
            }
            else
            {
                // if user has logged on
                if (this.Request.IsAuthenticated)
                {
                    // Redirect user back to the Portal Home Page
                    PortalSecurity.PortalHome();
                }
                else
                {
                    // User not provided, display logon
                    var controlStr = "~/DesktopModules/CoreModules/SignIn/Signin.ascx";
                    if (this.PortalSettings.CustomSettings.ContainsKey("SITESETTINGS_LOGIN_TYPE"))
                    {
                        controlStr = Convert.ToString(this.PortalSettings.CustomSettings["SITESETTINGS_LOGIN_TYPE"]);
                    }

                    try
                    {
                        this.signIn.Controls.Add(this.LoadControl(controlStr));
                    }
                    catch (Exception exc)
                    {
                        ErrorHandler.Publish(LogLevel.Error, exc);
                        this.signIn.Controls.Add(this.LoadControl("~/DesktopModules/CoreModules/SignIn/Signin.ascx"));
                    }
                }
            }
        }

        #endregion
    }
}