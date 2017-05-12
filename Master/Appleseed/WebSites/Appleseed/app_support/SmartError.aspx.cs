// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SmartError.aspx.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   Smart Error page - Jes1111
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Error
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Caching;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Xml;

    using Appleseed.Framework;
    using Appleseed.Framework.Helpers;
    using Appleseed.Framework.Settings;
    using Appleseed.Framework.Settings.Cache;

    using Page = Appleseed.Framework.Web.UI.Page;
    using Path = Appleseed.Framework.Settings.Path;
    using Utils = Appleseed.Framework.Helpers.Utilities;

    /// <summary>
    /// Smart Error page - Jes1111
    /// </summary>
    /// <remarks>
    /// </remarks>
    public partial class SmartError : Page
    {
        // protected PlaceHolder PageContent;
        #region Constants and Fields

        /// <summary>
        /// The _ gui d_.
        /// </summary>
        protected const int _GUID_ = 1;

        /// <summary>
        /// The _ logleve l_.
        /// </summary>
        protected const int _LOGLEVEL_ = 0;

        /// <summary>
        /// The _ renderedeven t_.
        /// </summary>
        protected const int _RENDEREDEVENT_ = 2;

        /// <summary>
        /// The label 1.
        /// </summary>
        protected Label Label1;

        /// <summary>
        /// The label 2.
        /// </summary>
        protected Label Label2;

        /// <summary>
        /// The label 3.
        /// </summary>
        protected Label Label3;

        // protected Esperantus.WebControls.HyperLink ReturnHome;

        /// <summary>
        /// The my test.
        /// </summary>
        protected Label myTest;

        /// <summary>
        /// The my test 2.
        /// </summary>
        protected Label myTest2;

        #endregion

        #region Public Methods

        /// <summary>
        /// Handles the Error event of the Page control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        public void Page_Error(object sender, EventArgs e)
        {
            this.Response.Redirect("~/app_support/SimpleError.aspx", true);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Raises the Init event.
        /// </summary>
        /// <param name="e">
        /// An <see cref="T:System.EventArgs"></see> that contains the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();

            // ReturnHome.NavigateUrl = HttpUrlBuilder.BuildUrl();
            base.OnInit(e);
        }

        /// <summary>
        /// Handles OnLoad event at Page level<br/>
        ///   Performs OnLoad actions that are common to all Pages.
        /// </summary>
        /// <param name="e">
        /// The <see cref="T:System.EventArgs"/> object that contains the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // load the dedicated CSS
            if (!this.IsCssFileRegistered("SmartError"))
            {
                this.RegisterCssFile("Mod_SmartError");
            }

            List<object> storedError = null;
            var sb = new StringBuilder(); // to build response text
            var httpStatusCode = (int)HttpStatusCode.InternalServerError; // default value
            const string ValidStatus = "301;307;403;404;410;500;501;502;503;504";

            if (this.Request.QueryString.Count > 0 && this.Request.QueryString[0] != null)
            {
                // is this a "MagicUrl" request
                if (this.Request.QueryString[0].StartsWith("404;http://"))
                {
                    var redirectUrl = string.Empty;
                    var qPart = string.Empty;
                    var qPartPos = this.Request.QueryString[0].LastIndexOf("/") + 1;
                    qPart = qPartPos < this.Request.QueryString[0].Length
                                ? this.Request.QueryString[0].Substring(qPartPos)
                                : string.Empty;
                    if (qPart.Length > 0)
                    {
                        if (Utils.IsInteger(qPart))
                        {
                            redirectUrl = HttpUrlBuilder.BuildUrl(Int32.Parse(qPart));
                        }
                        else
                        {
                            Hashtable magicUrlList = this.GetMagicUrlList(Portal.UniqueID);
                            if (magicUrlList != null && magicUrlList.ContainsKey(HttpUtility.HtmlEncode(qPart)))
                            {
                                redirectUrl =
                                    HttpUtility.HtmlDecode(magicUrlList[HttpUtility.HtmlEncode(qPart)].ToString());
                                if (Utils.IsInteger(redirectUrl))
                                {
                                    redirectUrl = HttpUrlBuilder.BuildUrl(Int32.Parse(redirectUrl));
                                }
                            }
                        }

                        if (redirectUrl.Length != 0)
                        {
                            this.Response.Redirect(redirectUrl, true);
                        }
                        else
                        {
                            httpStatusCode = (int)HttpStatusCode.NotFound;
                        }
                    }
                }
                else if (Utils.IsInteger(this.Request.QueryString[0]) &&
                         ValidStatus.IndexOf(this.Request.QueryString[0]) > -1)
                {
                    // get status code from query string
                    httpStatusCode = int.Parse(this.Request.QueryString[0]);
                }
            }

            // get stored error
            if (this.Request.QueryString["eid"] != null && this.Request.QueryString["eid"].Length > 0)
            {
                storedError = (List<object>)CurrentCache.Get(this.Request.QueryString["eid"]);
            }

            string renderedEvent = storedError != null && storedError[_RENDEREDEVENT_] != null
                                       ? storedError[_RENDEREDEVENT_].ToString()
                                       : @"<p>No exception event stored or cache has expired.</p>";

            // get home link
            var homeUrl = HttpUrlBuilder.BuildUrl();

            // try localizing message
            try
            {
                switch (httpStatusCode)
                {
                    case (int)HttpStatusCode.NotFound: // 404
                    case (int)HttpStatusCode.Gone: // 410
                    case (int)HttpStatusCode.MovedPermanently: // 301
                    case (int)HttpStatusCode.TemporaryRedirect: // 307
                        sb.AppendFormat(
                            "<h3>{0}</h3>", General.GetString("SMARTERROR_404HEADING", "Page Not Found", null));
                        sb.AppendFormat(
                            "<p>{0}</p>", 
                            General.GetString(
                                "SMARTERROR_404TEXT", 
                                "We're sorry, but there is no page that matches your entry. It is possible you typed the address incorrectly, or the page may no longer exist. You may wish to try another entry or choose from the links below, which we hope will help you find what you’re looking for.", 
                                null));
                        break;
                    case (int)HttpStatusCode.Forbidden: // 403
                        sb.AppendFormat(
                            "<h3>{0}</h3>", General.GetString("SMARTERROR_403HEADING", "Not Authorised", null));
                        sb.AppendFormat(
                            "<p>{0}</p>", 
                            General.GetString(
                                "SMARTERROR_403TEXT", 
                                "You do not have the required authority for the requested page or action.", 
                                null));
                        break;
                    default:
                        sb.AppendFormat(
                            "<h3>{0}</h3>", General.GetString("SMARTERROR_500HEADING", "Our Apologies", null));
                        sb.AppendFormat(
                            "<p>{0}</p>", 
                            General.GetString(
                                "SMARTERROR_500TEXT", 
                                "We're sorry, but we were unable to service your request. It's possible that the problem is a temporary condition.", 
                                null));
                        break;
                }
                if (AppleseedMaster.IsModalChangeMaster)
                {
                    sb.AppendFormat("<p><a href=\"javascript:void(0);\" onclick=\"window.parent.location='{0}';\">{1}</a></p>", homeUrl, General.GetString("HOME", "Home Page", null));
                }
                else
                {
                    sb.AppendFormat("<p><a href=\"{0}\">{1}</a></p>", homeUrl, General.GetString("HOME", "Home Page", null));
                }
            }
            catch
            {
                // default to english message
                switch (httpStatusCode)
                {
                    case (int)HttpStatusCode.NotFound:
                        sb.Append("<h3>Page Not Found</h3>");
                        sb.Append(
                            "<p>We're sorry, but there is no page that matches your entry. It is possible you typed the address incorrectly, or the page may no longer exist. You may wish to try another entry or choose from the links below, which we hope will help you find what you’re looking for.</p>");
                        break;
                    case (int)HttpStatusCode.Forbidden:
                        sb.Append("<h3>Not Authorised</h3>");
                        sb.Append("<p>You do not have the required authority for the requested page or action.</p>");
                        break;
                    default:
                        sb.Append("<h3>Our Apologies</h3>");
                        sb.AppendFormat(
                            "<p>We're sorry, but we were unable to service your request. It's possible that the problem is a temporary condition.</p>");
                        break;
                }

                //sb.AppendFormat("<p><a href=\"{0}\">{1}</a></p>", homeUrl, "Home Page");
                if (AppleseedMaster.IsModalChangeMaster)
                {
                    sb.AppendFormat("<p><a href=\"javascript:void(0);\" onclick=\"window.parent.location='{0}';\">{1}</a></p>", homeUrl, General.GetString("HOME", "Home Page", null));
                }
                else
                {
                    sb.AppendFormat("<p><a href=\"{0}\">{1}</a></p>", homeUrl, General.GetString("HOME", "Home Page", null));
                }
            }

            // find out if user is on allowed IP Address
            if (this.Request.UserHostAddress != null && this.Request.UserHostAddress.Length > 0)
            {
                // construct IPList
                var lockKeyHolders = Config.LockKeyHolders.Split(new[] { ';' });
                    
                    // ConfigurationSettings.AppSettings["LockKeyHolders"].Split(new char[]{';'});
                var ipList = new IPList();
                try
                {
                    foreach (var lockKeyHolder in lockKeyHolders)
                    {
                        if (lockKeyHolder.IndexOf("-") > -1)
                        {
                            ipList.AddRange(
                                lockKeyHolder.Substring(0, lockKeyHolder.IndexOf("-")), 
                                lockKeyHolder.Substring(lockKeyHolder.IndexOf("-") + 1));
                        }
                        else
                        {
                            ipList.Add(lockKeyHolder);
                        }
                    }

                    // check if it has to show the full detail error message
                    bool showError = false;
                    if (this.PortalSettings.CustomSettings["DETAIL_ERROR_MESSAGE"] != null)
                        {
                            showError =  bool.Parse(this.PortalSettings.CustomSettings["DETAIL_ERROR_MESSAGE"].ToString());
                        }
                    // check if requestor's IP address is in allowed list
                    if (ipList.CheckNumber(this.Request.UserHostAddress) || showError)
                    {
                        // we can show error details
                        sb.AppendFormat(
                            "<h3>{0} - {1}</h3>", 
                            General.GetString("SMARTERROR_SUPPORTDETAILS_HEADING", "Support Details", null), 
                            httpStatusCode);
                        sb.Append(renderedEvent); 
                    }
                }
                catch
                {
                    // if there was a problem, let's assume that user is not authorized
                }
            }

            var pageContent = this.FindControl("PageContent");
            if (pageContent == null)
            {
                pageContent = this.Master.FindControl("Content").FindControl("PageContent");
            }

            pageContent.Controls.Add(new LiteralControl(sb.ToString()));
            //this.Response.StatusCode = httpStatusCode;
            this.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        }

        /// <summary>
        /// Gets the magic URL list.
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <returns>
        /// </returns>
        /// <remarks>
        /// </remarks>
        private Hashtable GetMagicUrlList(string portalId)
        {
            var result = new Hashtable();

            if (this.Cache["Appleseed_MagicUrlList_" + Portal.UniqueID] == null)
            {
                var myPath =
                    this.Server.MapPath(
                        Path.WebPathCombine(this.PortalSettings.PortalFullPath, "MagicUrl/MagicUrlList.xml"));
                if (File.Exists(myPath))
                {
                    var xmlDoc = new XmlDocument();
                    if (myPath != null)
                    {
                        xmlDoc.Load(myPath);
                    }
                    var xnl = xmlDoc.SelectNodes("/MagicUrlList/MagicUrl");
                    if (xnl != null)
                    {
                        foreach (XmlNode node in xnl)
                        {
                            try
                            {
                                result.Add(
                                    node.Attributes["key"].Value, HttpUtility.HtmlDecode(node.Attributes["value"].Value));
                            }
                            catch
                            {
                            }
                        }
                    }

                    this.Cache.Insert("Appleseed_MagicUrlList_" + Portal.UniqueID, result, new CacheDependency(myPath));
                }
            }
            else
            {
                result = (Hashtable)this.Cache["Appleseed_MagicUrlList_" + Portal.UniqueID];
            }

            return result;
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        ///   the contents of this method with the code editor.
        /// </summary>
        /// <remarks>
        /// </remarks>
        private void InitializeComponent()
        {
            this.Error += this.Page_Error;
        }

        #endregion
    }
}