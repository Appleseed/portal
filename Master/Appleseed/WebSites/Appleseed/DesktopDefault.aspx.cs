// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DesktopDefault.aspx.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   The DesktopDefault.aspx page is used
//   to load and populate each Portal View.
//   It accomplishes this by reading the layout configuration
//   of the portal from the Portal Configuration system,
//   and then using this information to dynamically
//   instantiate portal modules (each implemented
//   as an ASP.NET User Control), and then inject them into the page.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Text;

namespace Appleseed
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    using Appleseed.Framework;
    using Appleseed.Framework.Core.Model;
    using Appleseed.Framework.Design;
    using Appleseed.Framework.Security;

    using Page = Appleseed.Framework.Web.UI.Page;
    using System.Collections;
    using System.Data.SqlClient;

    /// <summary>
    /// The DesktopDefault.aspx page is used 
    ///   to load and populate each Portal View.
    ///   It accomplishes this by reading the layout configuration 
    ///   of the portal from the Portal Configuration system,
    ///   and then using this information to dynamically 
    ///   instantiate portal modules (each implemented 
    ///   as an ASP.NET User Control), and then inject them into the page.
    /// </summary>
    public partial class DesktopDefault : Page
    {
        // private bool isMasterPageLayout = false;
        #region Constants and Fields

        /// <summary>
        /// The layout base page.
        /// </summary>
        private const string LayoutBasePage = "DesktopDefault.ascx";

        #endregion

        #region Methods

        /// <summary>
        /// Handles the OnInit event at Page level<br/>
        /// Performs OnInit events that are common to all Pages<br/>
        /// Can be overridden
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            // this.Load += new EventHandler(this.DesktopDefault_Load);
            base.OnInit(e);
            this.DesktopDefault_Load(this, null);
        }


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            //Javascript loading at the bottom of the page - Done by Ashish - 28/05/15
            //Appleseed.Framework.Site.Data.TabSettings objdr = new Appleseed.Framework.Site.Data.TabSettings();
            SqlDataReader SRJS = Appleseed.Framework.Site.Data.TabSettings.JSDataReader(PortalSettings.ActivePage.PageID, "TabLinkJS");
            var sb = new StringBuilder();
            while (SRJS.Read())
            {
                sb.AppendLine("<script type=\"text/javascript\">");
                sb.AppendLine(SRJS[0].ToString());
                sb.AppendLine("</script>");
            }
            this.ClientScript.RegisterStartupScript(this.GetType(), "JS", sb.ToString());
        }

        /// <summary>
        /// Raises the <see cref="System.Web.UI.Page.PreInit"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnPreInit(EventArgs e)
        {
            this.MasterpageBasePage = "PanesMaster.master";
            base.OnPreInit(e);
        }

        /// <summary>
        /// Determines whether the specified HTML control has children.
        /// </summary>
        /// <param name="htmlControl">
        /// The HTML control.
        /// </param>
        /// <param name="pageModules">
        /// The page modules.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified HTML control has children; otherwise, <c>false</c>.
        /// </returns>
        private static bool HasChilds(
            Control htmlControl,
            IDictionary<string, List<Control>> pageModules)
        {

            var s = htmlControl;

            return
                htmlControl.Controls.OfType<Control>().Any(
                control => pageModules.ContainsKey(String.IsNullOrEmpty(control.ID) ? "null" : control.ID.ToLower())
                        || HasChilds(control, pageModules));
        }

        /// <summary>
        /// Hides the not filled.
        /// </summary>
        /// <param name="topPlaceHolder">
        /// The top place holder.
        /// </param>
        /// <param name="pageModulesByPlaceHolder">
        /// The page modules by place holder.
        /// </param>
        private static void HideNotFilled(
            Control topPlaceHolder,
            IDictionary<string, List<Control>> pageModulesByPlaceHolder)
        {
            foreach (var htmlControl in
                topPlaceHolder.Controls.OfType<HtmlControl>().Where(
                    htmlControl => htmlControl.Attributes["hideWhenEmpty"] != null).Where(
                        htmlControl =>
                        Convert.ToBoolean(htmlControl.Attributes["hideWhenEmpty"]) &&
                        !HasChilds(htmlControl, pageModulesByPlaceHolder)))
            {
                htmlControl.Style["display"] = "none";
            }
        }

        /// <summary>
        /// The all place holders in control.
        /// </summary>
        /// <param name="placeHolder">
        /// The place holder.
        /// </param>
        /// <returns>
        /// A list of content place holders.
        /// </returns>
        private List<ContentPlaceHolder> AllPlaceHoldersInControl(Control placeHolder)
        {
            var result = new List<ContentPlaceHolder>();
            foreach (Control control in placeHolder.Controls)
            {
                if (control is ContentPlaceHolder)
                {
                    result.Add(control as ContentPlaceHolder);
                }
                else
                {
                    result.AddRange(this.AllPlaceHoldersInControl(control));
                }
            }

            return result;
        }

        /// <summary>
        /// Handles the Load event of the DesktopDefault control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void DesktopDefault_Load(object sender, EventArgs e)
        {
            int pageId = 0;
            string lnkid = Request.QueryString["lnkid"];
            if (!string.IsNullOrEmpty(lnkid))
            {

                if (int.TryParse(lnkid, out pageId))
                {
                    this.Response.Redirect(Appleseed.Framework.HttpUrlBuilder.BuildUrl(pageId), true);
                }
            }

            if (!string.IsNullOrEmpty(Request.Params["panelist"]))
            {
                this.RenderContentAreaList();
            }
            // intento obtener el id de la pagina desde el query
            string query = Request.Url.Query;
            //  int pageId = 0;
            if (query.Contains("?") && query.ToLower().Contains("pageid"))
            {
                int index = query.IndexOf('?');
                int indexPageId = query.ToLower().IndexOf("pageid") + 5;
                if (index < indexPageId - 5)
                {
                    query = query.Substring(indexPageId + 2, query.Length - indexPageId - 2);
                    index = query.IndexOf('&');
                    if (index > 0) // no va hasta el final el numero de pagina
                        query = query.Substring(0, index);
                    try
                    {
                        pageId = int.Parse(query);
                    }
                    catch (Exception)
                    {
                        pageId = 0;
                    }
                }
                else
                {
                    pageId = 0;
                }
            }
            else
                pageId = this.PortalSettings.ActivePage.PageID;

            if (pageId == 0)
            {
                pageId = Convert.ToInt32(SiteMap.RootNode.ChildNodes[0].Key);
                this.Response.Redirect(HttpUrlBuilder.BuildUrl(pageId));
            }

            string urlToRedirect = "";
            bool redirect = HttpUrlBuilder.ValidateProperUrl(pageId, ref urlToRedirect);
            if (!redirect)
            {
                this.Response.Redirect(urlToRedirect);
            }

            SecurePages page;
            Enum.TryParse<SecurePages>(pageId.ToString(), out page);

            if (this.PortalSettings.EnabledPrivateSite && !Request.IsAuthenticated)
            {
                PortalSecurity.AccessDenied();
            }
            else if (!PortalSecurity.IsInRoles(this.PortalSettings.ActivePage.AuthorizedRoles) &&
                !this.User.IsInRole("Admins") && !UserProfile.HasPageAccess(page))
            {
                PortalSecurity.AccessDenied();
            }
            else
            {
                if (this.Request.Params["r"] == null || this.Request.Params["r"] != "0")
                {
                    var user = Membership.GetUser();
                }

                var userName = this.Request.Params["u"];
                var pass = this.Request.Params["p"];
                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(pass))
                {
                    // PortalSecurity.SignOn(userName, pass, false, "~/DesktopDefault.aspx");
                    var rem = (this.Request.Params["rem"] ?? "0").Equals("1") ? true : false;
                    PortalSecurity.SignOn(userName, pass, rem, "~/DesktopDefault.aspx");
                    this.Response.Redirect("~/DesktopDefault.aspx");
                }

                Framework.Site.Configuration.PortalSettings.GetPortalSettingsbyPageID(this.PageID, this.PortalSettings.PortalAlias);
                if (string.IsNullOrEmpty(Request.Params["panelist"]))
                {
                    this.LoadPage();
                }
            }
        }

        private void RenderContentAreaList()
        {
            Response.Clear();
            // Obtain top master page
            var controllist = GetPlaceholderControllist();
            var sb = new StringBuilder();
            //Response.Write("<ul>");
            // for each top placeholder
            foreach (ContentPlaceHolder placeHolder in controllist)
            {
                foreach (var contentPlaceHolder in AllPlaceHoldersInControl(placeHolder))
                {
                    sb.AppendFormat(contentPlaceHolder.ID + "+");
                    //Response.Write("<li>" + contentPlaceHolder.ID + "</li>");
                }
            }
            //Response.Write("</ul>");
            var json = sb.ToString();
            json = json.Substring(0, json.Length - 1);
            Response.ContentType = "application/json";
            Response.StatusCode = 200;
            Response.Write(json);
            Response.Flush();
            Response.End();
        }

        private List<Control> GetPlaceholderControllist()
        {
            var mp = this.GetTopMasterPage();

            var controllist = new List<Control>();

            // Obtain top master page placeholders
            foreach (var control in mp.Controls.OfType<HtmlForm>())
            {
                controllist.AddRange(control.Controls.OfType<ContentPlaceHolder>());
            }
            return controllist;
        }

        /// <summary>
        /// Gets the top master page.
        /// </summary>
        /// <returns>
        /// The master page.
        /// </returns>
        private MasterPage GetTopMasterPage()
        {
            var mp = this.Master;
            if (mp == null)
            {
                return null;
            }

            while (mp.Master != null)
            {
                mp = mp.Master;
            }

            return mp;
        }

        /// <summary>
        /// Loads the page.
        /// </summary>
        private void LoadPage()
        {
            if (this.IsMasterPageLayout)
            {
                // Obtain page modules by placeholder
                var pageModulesByPlaceHolder = ModelServices.GetCurrentPageModules();

                // Obtain top master page
                var controllist = GetPlaceholderControllist();

                // for each top placeholder
                foreach (ContentPlaceHolder placeHolder in controllist)
                {
                    var insidePlaceHolders = this.AllPlaceHoldersInControl(placeHolder);

                    // for each pane placeholder in the page
                    foreach (var pageModuleInPlaceHolder in pageModulesByPlaceHolder)
                    {
                        // find out if current top placeholder contains current pane
                        var container = placeHolder.FindControl(pageModuleInPlaceHolder.Key);
                        if (container == null)
                        {
                            continue;
                        }

                        // wrap current pane modules them inside custom span (to drag-n-drop)
                        container.Controls.Clear();
                        var span = new HtmlGenericControl("div");
                        span.Attributes.Add("id", pageModuleInPlaceHolder.Key);
                        span.Attributes.Add("class", "draggable-container");
                        foreach (var control in pageModuleInPlaceHolder.Value)
                        {
                            span.Controls.Add(control);
                        }

                        container.Controls.Add(span);

                        var holder = pageModuleInPlaceHolder;
                        insidePlaceHolders.RemoveAll(d => d.ID.ToLower() == holder.Key.ToLower());
                    }

                    foreach (var v in insidePlaceHolders)
                    {
                        var container = placeHolder.FindControl(v.ID);
                        container.Controls.Clear();
                        var span = new HtmlGenericControl("div");
                        span.Attributes.Add("id", v.ID.ToLowerInvariant());
                        span.Attributes.Add("class", "draggable-container");
                        span.Style["display"] = "none";
                        container.Controls.Add(span);
                    }

                    // then, hide empty top placeholders
                    HideNotFilled(placeHolder, pageModulesByPlaceHolder);
                }
            }
            else
            {
                var defaultLayoutPath = string.Concat(LayoutManager.WebPath, "/Default/", LayoutBasePage);

                try
                {
                    var layoutPath = string.Concat(this.PortalSettings.PortalLayoutPath, LayoutBasePage);
                    var layoutControl = this.Page.LoadControl(layoutPath);

                    AppleseedMaster.InsertAllScripts(Page, Context);

                    if (layoutControl != null)
                    {
                        this.LayoutPlaceHolder.Controls.Add(layoutControl);
                    }
                    else
                    {
                        throw new FileNotFoundException(
                            string.Format(
                                "While loading {1} layoutControl is null, control not found in path {0}!!",
                                layoutPath,
                                this.Request.RawUrl));
                    }
                }
                catch (HttpException ex)
                {
                    ErrorHandler.Publish(LogLevel.Error, "FileOrDirectoryNotFound", ex);
                    this.LayoutPlaceHolder.Controls.Add(this.Page.LoadControl(defaultLayoutPath));
                }
                catch (DirectoryNotFoundException ex)
                {
                    ErrorHandler.Publish(LogLevel.Error, "DirectoryNotFound", ex);
                    this.LayoutPlaceHolder.Controls.Add(this.Page.LoadControl(defaultLayoutPath));
                }
                catch (FileNotFoundException ex)
                {
                    ErrorHandler.Publish(LogLevel.Error, "FileNotFound", ex);
                    this.LayoutPlaceHolder.Controls.Add(this.Page.LoadControl(defaultLayoutPath));
                }
            }
        }


        #endregion
    }
}