// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PagesDB.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   Class that encapsulates all data logic necessary to add/query/delete
//   Portals within the Portal database.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Site.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Xml;

    using Appleseed.Framework.Data;
    using Appleseed.Framework.Settings;
    using Appleseed.Framework.Site.Configuration;

    /// <summary>
    /// Class that encapsulates all data logic necessary to add/query/delete
    ///   Portals within the Portal database.
    /// </summary>
    [History("jminond", "2005/03/10", "Tab to page conversion")]
    public class PagesDB
    {
        #region Constants and Fields

        /// <summary>
        /// The bool show mobile.
        /// </summary>
        private const bool BoolShowMobile = false;

        /// <summary>
        /// The int parent page id.
        /// </summary>
        private const int IntParentPageId = 0; // SP will convert to NULL if 0

        /// <summary>
        /// The str all users.
        /// </summary>
        private const string StrAllUsers = "All Users;";

        /// <summary>
        /// The str mobile page name.
        /// </summary>
        private const string StrMobilePageName = ""; // NULL NOT ALLOWED IN TABLE.

        /// <summary>
        /// The str page id.
        /// </summary>
        private const string StrPageId = "@PageID";

        /// <summary>
        /// The str portal id.
        /// </summary>
        private const string StrPortalId = "@PortalID";

        /// <summary>
        /// The str portal id.
        /// </summary>
        private const string StrFriendlyURL = "@friendlyURL";

        /// <summary>
        /// The get store precedure result as output
        /// </summary>
        private const string output = "@result";

        #endregion

        #region Public Methods

        /// <summary>
        /// Return the portal home page in case you are on page id = 0
        /// </summary>
        /// <param name="portalId">
        /// The portal id.
        /// </param>
        /// <returns>
        /// The home page id.
        /// </returns>
        public static int PortalHomePageId(int portalId)
        {
            // TODO: Convert to stored procedure?
            // TODO: Appleseed.Framwork.Application.Site.Pages API
            var sql =
                string.Format(
                    "Select PageID  From rb_Pages  Where (PortalID = {0}) and (ParentPageID is null) and  (PageID > 0) and ( PageOrder < 2)",
                    portalId);

            var ret = Convert.ToInt32(DBHelper.ExecuteSqlScalar<int>(sql));

            return ret;
        }

        /// <summary>
        /// The AddPage method adds a new tab to the portal.<br/>
        ///   AddPage Stored Procedure
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <param name="pageName">
        /// Name of the page.
        /// </param>
        /// <param name="pageOrder">
        /// The page order.
        /// </param>
        /// <returns>
        /// The add page.
        /// </returns>
        public int AddPage(int portalId, string pageName, int pageOrder)
        {
            return this.AddPage(portalId, pageName, StrAllUsers, pageOrder);
        }

        /// <summary>
        /// The AddPage method adds a new tab to the portal.<br/>
        ///   AddPage Stored Procedure
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <param name="pageName">
        /// Name of the page.
        /// </param>
        /// <param name="roles">
        /// The roles.
        /// </param>
        /// <param name="pageOrder">
        /// The page order.
        /// </param>
        /// <returns>
        /// The add page.
        /// </returns>
        public int AddPage(int portalId, string pageName, string roles, int pageOrder)
        {
            // Change Method to use new all parameters method below
            // SP call moved to new method AddPage below.
            // Mike Stone - 30/12/2004
            return this.AddPage(
                portalId, IntParentPageId, pageName, pageOrder, StrAllUsers, BoolShowMobile, StrMobilePageName);
        }

        /// <summary>
        /// The AddPage method adds a new tab to the portal.<br/>
        ///   AddPage Stored Procedure
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <param name="parentPageId">
        /// The parent page ID.
        /// </param>
        /// <param name="pageName">
        /// Name of the page.
        /// </param>
        /// <param name="pageOrder">
        /// The page order.
        /// </param>
        /// <param name="authorizedRoles">
        /// The authorized roles.
        /// </param>
        /// <param name="showMobile">
        /// if set to <c>true</c> [show mobile].
        /// </param>
        /// <param name="mobilePageName">
        /// Name of the mobile page.
        /// </param>
        /// <returns>
        /// The add page.
        /// </returns>
        public int AddPage(
            int portalId,
            int parentPageId,
            string pageName,
            int pageOrder,
            string authorizedRoles,
            bool showMobile,
            string mobilePageName)
        {
            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (var command = new SqlCommand("rb_AddTab", connection))
            {
                // Mark the Command as a SPROC
                command.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                var parameterPortalId = new SqlParameter(StrPortalId, SqlDbType.Int, 4) { Value = portalId };
                command.Parameters.Add(parameterPortalId);

                var parameterParentPageId = new SqlParameter("@ParentPageID", SqlDbType.Int, 4) { Value = parentPageId };
                command.Parameters.Add(parameterParentPageId);

                // Fixes a missing tab name
                if (string.IsNullOrEmpty(pageName))
                {
                    pageName = "New Page";
                }

                var parameterTabName = new SqlParameter("@PageName", SqlDbType.NVarChar, 200)
                {
                    // Fixes tab name to long
                    Value = pageName.Length > 200 ? pageName.Substring(0, 199) : pageName
                };

                command.Parameters.Add(parameterTabName);

                var parameterTabOrder = new SqlParameter("@PageOrder", SqlDbType.Int, 4) { Value = pageOrder };
                command.Parameters.Add(parameterTabOrder);

                var parameterAuthRoles = new SqlParameter("@AuthorizedRoles", SqlDbType.NVarChar, 256)
                {
                    Value = authorizedRoles
                };
                command.Parameters.Add(parameterAuthRoles);

                var parameterShowMobile = new SqlParameter("@ShowMobile", SqlDbType.Bit, 1) { Value = showMobile };
                command.Parameters.Add(parameterShowMobile);

                var parameterMobileTabName = new SqlParameter("@MobilePageName", SqlDbType.NVarChar, 200)
                {
                    Value = mobilePageName
                };
                command.Parameters.Add(parameterMobileTabName);

                var parameterPageId = new SqlParameter(StrPageId, SqlDbType.Int, 4)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(parameterPageId);

                connection.Open();

                try
                {
                    command.ExecuteNonQuery();
                }
                finally
                {
                }

                return (int)parameterPageId.Value;
            }
        }

        /// <summary>
        /// The DeletePage method deletes the selected tab from the portal.<br/>
        ///   DeletePage Stored Procedure
        /// </summary>
        /// <param name="pageId">
        /// The page ID.
        /// </param>
        public void DeletePage(int pageId)
        {
            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (var sqlCommand = new SqlCommand("rb_DeleteTab", connection))
            {
                // Mark the Command as a SPROC
                sqlCommand.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                var parameterPageId = new SqlParameter(StrPageId, SqlDbType.Int, 4) { Value = pageId };
                sqlCommand.Parameters.Add(parameterPageId);

                // Open the database connection and execute the command
                connection.Open();

                try
                {
                    sqlCommand.ExecuteNonQuery();
                }
                finally
                {
                }
            }
        }

        /// <summary>
        /// This user control will render the breadcrumb navigation for the current tab.
        ///   Ver. 1.0 - 24. Dec. 2002 - First release by Cory Isakson
        /// </summary>
        /// <param name="pageId">
        /// ID of the page
        /// </param>
        /// <returns>
        /// A list of page items.
        /// </returns>
        public List<PageItem> GetPageCrumbs(int pageId)
        {
            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (var sqlCommand = new SqlCommand("rb_GetTabCrumbs", connection))
            {
                // Mark the Command as a SPROC
                sqlCommand.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                var parameterPageId = new SqlParameter(StrPageId, SqlDbType.Int, 4) { Value = pageId };
                sqlCommand.Parameters.Add(parameterPageId);
                var parameterCrumbs = new SqlParameter("@CrumbsXML", SqlDbType.NVarChar, 4000)
                {
                    Direction = ParameterDirection.Output
                };
                sqlCommand.Parameters.Add(parameterCrumbs);

                // Execute the command
                connection.Open();

                try
                {
                    sqlCommand.ExecuteNonQuery();
                }
                finally
                {
                }

                // Build a Hash table from the XML string returned
                var crumbXml = new XmlDocument();
                crumbXml.LoadXml(parameterCrumbs.Value.ToString().Replace("&", "&amp;"));

                // Iterate through the Crumbs XML
                // Return the Crumb Page Items as an array list 
                return
                    crumbXml.FirstChild.ChildNodes.Cast<XmlNode>().Where(node => node.Attributes != null).Select(
                        node =>
                        new PageItem
                        {
                            ID = Int16.Parse(node.Attributes.GetNamedItem("tabID").Value),
                            Name = node.InnerText,
                            Order = Int16.Parse(node.Attributes.GetNamedItem("level").Value)
                        }).ToList();
            }
        }

        /// <summary>
        /// Get a pages parentID
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <param name="pageId">
        /// The page ID.
        /// </param>
        /// <returns>
        /// The get page parent id.
        /// </returns>
        public int GetPageParentId(int portalId, int pageId)
        {
            var strSql = string.Format("rb_GetPagesParentTabID {0}, {1}", portalId, pageId);

            // Read the result set
            int parentId = Convert.ToInt32(DBHelper.ExecuteSqlScalar<int>(strSql));
            return parentId;
        }

        /// <summary>
        /// Get a pages tab order
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <param name="pageId">
        /// The page ID.
        /// </param>
        /// <returns>
        /// The get page tab order.
        /// </returns>
        public int GetPageTabOrder(int portalId, int pageId)
        {
            var strSql = string.Format(
                "select PageOrder from rb_Pages Where (PortalID = {0}) AND (PageID = {1})", portalId, pageId);

            // Read the result set
            var tabOrder = Convert.ToInt32(DBHelper.ExecuteSqlScalar<int>(strSql));
            return tabOrder;
        }

        /// <summary>
        /// Gets the pages by portal.
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <returns>
        /// A System.Data.SqlClient.SqlDataReader value...
        /// </returns>
        [Obsolete("Replace me")]
        public SqlDataReader GetPagesByPortal(int portalId)
        {
            // Create Instance of Connection and Command Object
            var connection = Config.SqlConnectionString;
            var sqlCommand = new SqlCommand("rb_GetTabsByPortal", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Mark the Command as a SPROC
            // Add Parameters to SPROC
            var parameterPortalId = new SqlParameter(StrPortalId, SqlDbType.Int, 4) { Value = portalId };
            sqlCommand.Parameters.Add(parameterPortalId);

            // Execute the command
            connection.Open();
            var result = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);

            // Return the data reader 
            return result;
        }

        /// <summary>
        /// Gets the pages flat.
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <returns>
        /// A list of page items.
        /// </returns>
        public List<PageItem> GetPagesFlat(int portalId)
        {
            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (var sqlCommand = new SqlCommand("rb_GetTabsFlat", connection))
            {
                // Mark the Command as a SPROC
                sqlCommand.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                var parameterPortalId = new SqlParameter("@PortalID", SqlDbType.Int, 4) { Value = portalId };
                sqlCommand.Parameters.Add(parameterPortalId);

                // Execute the command
                connection.Open();
                var result = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                var desktopPages = new List<PageItem>();

                // Read the result set
                try
                {
                    while (result.Read())
                    {

                        var tabItem = new PageItem
                        {
                            ID = (int)result["PageID"],
                            Name = (string)result["PageName"],
                            Order = (int)result["PageOrder"],
                            NestLevel = (int)result["NestLevel"]
                        };

                        if (result["NestLevel"] != null)
                        {
                            int parentId = 0;
                            int.TryParse(result["parentPageID"].ToString(), out parentId);
                            tabItem.ParentPageId = parentId;
                        }

                        desktopPages.Add(tabItem);
                    }
                }
                finally
                {
                    result.Close(); // by Manu, fixed bug 807858
                }

                return desktopPages;
            }
        }

        /// <summary>
        /// Gets the pages flat table.
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <returns>
        /// A data table.
        /// </returns>
        public DataTable GetPagesFlatTable(int portalId)
        {
            // Create Instance of Connection and Command Object
            using (var sqlConnection = Config.SqlConnectionString)
            {
                var commandText = "rb_GetPageTree " + portalId;
                var da = new SqlDataAdapter(commandText, sqlConnection);

                var dataTable = new DataTable("Pages");

                // Read the result set
                try
                {
                    da.Fill(dataTable);
                }
                finally
                {
                    da.Dispose();
                }

                return dataTable;
            }
        }

        /// <summary>
        /// Gets the pages parent.
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <param name="pageId">
        /// The page ID.
        /// </param>
        /// <returns>
        /// A System.Data.SqlClient.SqlDataReader value...
        /// </returns>
        public IList<PageItem> GetPagesParent(int portalId, int pageId)
        {
            // Create Instance of Connection and Command Object
            var connection = Config.SqlConnectionString;
            var sqlCommand = new SqlCommand("rb_GetTabsParent", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Mark the Command as a SPROC
            // Add Parameters to SPROC
            var parameterPortalId = new SqlParameter(StrPortalId, SqlDbType.Int, 4) { Value = portalId };
            sqlCommand.Parameters.Add(parameterPortalId);
            var parameterPageId = new SqlParameter(StrPageId, SqlDbType.Int, 4) { Value = pageId };
            sqlCommand.Parameters.Add(parameterPageId);

            // Execute the command
            connection.Open();
            var dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);

            IList<PageItem> result = new List<PageItem>();

            while (dr.Read())
            {
                var item = new PageItem { ID = Convert.ToInt32(dr["PageID"]), Name = (string)dr["PageName"] };
                result.Add(item);
            }
            // Added by Ashish - Connection Pool Issue
            if (dr != null)
            {
                dr.Close();

            }

            return result;
        }

        /// <summary>
        /// Gets the pages in page.
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <param name="pageId">
        /// The page ID.
        /// </param>
        /// <returns>
        /// A System.Collections.ArrayList value...
        /// </returns>
        public List<PageStripDetails> GetPagesinPage(int portalId, int pageId)
        {
            var desktopPages = new List<PageStripDetails>();

            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (var sqlCommand = new SqlCommand("rb_GetTabsinTab", connection))
            {
                // Mark the Command as a SPROC
                sqlCommand.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                var parameterPortalId = new SqlParameter(StrPortalId, SqlDbType.Int, 4) { Value = portalId };
                sqlCommand.Parameters.Add(parameterPortalId);
                var parameterPageId = new SqlParameter(StrPageId, SqlDbType.Int, 4) { Value = pageId };
                sqlCommand.Parameters.Add(parameterPageId);

                // Execute the command
                connection.Open();
                var result = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);

                // Read the result set
                try
                {
                    while (result.Read())
                    {
                        var tabDetails = new PageStripDetails
                        {
                            PageID = (int)result["PageID"],
                            ParentPageID = Int32.Parse("0" + result["ParentPageID"]),
                            PageName = (string)result["PageName"],
                            PageOrder = (int)result["PageOrder"],
                            AuthorizedRoles = (string)result["AuthorizedRoles"]
                        };

                        // Update the AuthorizedRoles Variable
                        desktopPages.Add(tabDetails);
                    }
                }
                finally
                {
                }
            }

            return desktopPages;
        }

        /// <summary>
        /// UpdatePage Method<br/>
        ///   UpdatePage Stored Procedure
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <param name="pageId">
        /// The page ID.
        /// </param>
        /// <param name="parentPageId">
        /// The parent page ID.
        /// </param>
        /// <param name="pageName">
        /// Name of the page.
        /// </param>
        /// <param name="pageOrder">
        /// The page order.
        /// </param>
        /// <param name="authorizedRoles">
        /// The authorized roles.
        /// </param>
        /// <param name="mobilePageName">
        /// Name of the mobile page.
        /// </param>
        /// <param name="showMobile">
        /// if set to <c>true</c> [show mobile].
        /// </param>
        /// <param name="friendlyURL">
        /// friendly url
        /// </param>
        public void UpdatePage(
          int portalId,
          int pageId,
          int parentPageId,
          string pageName,
          int pageOrder,
          string authorizedRoles,
          string mobilePageName,
          bool showMobile,
          string friendlyURL = "")
        {
            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (var sqlCommand = new SqlCommand("rb_UpdateTab", connection))
            {
                // Mark the Command as a SPROC
                sqlCommand.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                var parameterPortalId = new SqlParameter(StrPortalId, SqlDbType.Int, 4) { Value = portalId };
                sqlCommand.Parameters.Add(parameterPortalId);
                var parameterPageId = new SqlParameter(StrPageId, SqlDbType.Int, 4) { Value = pageId };
                sqlCommand.Parameters.Add(parameterPageId);
                var parameterParentPageId = new SqlParameter("@ParentPageID", SqlDbType.Int, 4) { Value = parentPageId };
                sqlCommand.Parameters.Add(parameterParentPageId);

                // Fixes a missing tab name
                if (string.IsNullOrEmpty(pageName))
                {
                    pageName = "&nbsp;";
                }

                var parameterTabName = new SqlParameter("@PageName", SqlDbType.NVarChar, 200)
                {
                    Value = pageName.Length > 200 ? pageName.Substring(0, 199) : pageName
                };

                sqlCommand.Parameters.Add(parameterTabName);
                var parameterTabOrder = new SqlParameter("@PageOrder", SqlDbType.Int, 4) { Value = pageOrder };
                sqlCommand.Parameters.Add(parameterTabOrder);
                var parameterAuthRoles = new SqlParameter("@AuthorizedRoles", SqlDbType.NVarChar, 256)
                {
                    Value = authorizedRoles
                };
                sqlCommand.Parameters.Add(parameterAuthRoles);
                var parameterMobileTabName = new SqlParameter("@MobilePageName", SqlDbType.NVarChar, 200)
                {
                    Value = mobilePageName
                };
                sqlCommand.Parameters.Add(parameterMobileTabName);
                var parameterShowMobile = new SqlParameter("@ShowMobile", SqlDbType.Bit, 1) { Value = showMobile };
                sqlCommand.Parameters.Add(parameterShowMobile);

                // HaptiX - Pass paramter for Friendly URL 
                var parameterFriendlyURL = new SqlParameter("@FriendlyURL", SqlDbType.NVarChar, 1024)
                {
                    Value = friendlyURL
                };
                sqlCommand.Parameters.Add(parameterFriendlyURL);

                connection.Open();
                sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Update Page Custom Settings
        /// </summary>
        /// <param name="pageId">
        /// The page ID.
        /// </param>
        /// <param name="key">
        /// The setting key.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        [Obsolete("UpdatePageCustomSettings was moved in PageSettings.UpdatePageSetting", false)]
        public void UpdatePageCustomSettings(int pageId, string key, string value)
        {
            PageSettings.UpdatePageSettings(pageId, key, value);
        }

        /// <summary>
        /// The UpdatePageOrder method changes the position of the tab with respect
        ///   to other tabs in the portal.<br/>
        ///   UpdatePageOrder Stored Procedure
        /// </summary>
        /// <param name="pageId">
        /// The page ID.
        /// </param>
        /// <param name="pageOrder">
        /// The page order.
        /// </param>
        public void UpdatePageOrder(int pageId, int pageOrder)
        {
            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (var sqlCommand = new SqlCommand("rb_UpdateTabOrder", connection))
            {
                // Mark the Command as a SPROC
                sqlCommand.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                var parameterPageId = new SqlParameter(StrPageId, SqlDbType.Int, 4) { Value = pageId };
                sqlCommand.Parameters.Add(parameterPageId);
                var parameterTabOrder = new SqlParameter("@PageOrder", SqlDbType.Int, 4) { Value = pageOrder };
                sqlCommand.Parameters.Add(parameterTabOrder);
                connection.Open();

                try
                {
                    sqlCommand.ExecuteNonQuery();
                }
                finally
                {
                }
            }
        }

        /// <summary>
        /// The UpdatePageOrder method changes the position of the tab with respect
        ///   to other tabs in the portal.<br/>
        ///   UpdatePageOrder Stored Procedure
        /// </summary>
        /// <param name="pageId">
        /// The page ID.
        /// </param>
        /// <param name="parentPageId">
        /// The parent page ID.
        /// </param>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        public void UpdatePageParent(int pageId, int parentPageId, int portalId)
        {
            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (var sqlCommand = new SqlCommand("rb_UpdateTabParent", connection))
            {
                // Mark the Command as a SPROC
                sqlCommand.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                var parameterPortalId = new SqlParameter(StrPortalId, SqlDbType.Int, 4) { Value = portalId };
                sqlCommand.Parameters.Add(parameterPortalId);
                var parameterPageId = new SqlParameter(StrPageId, SqlDbType.Int, 4) { Value = pageId };
                sqlCommand.Parameters.Add(parameterPageId);
                var parameterParentPageId = new SqlParameter("@ParentPageID", SqlDbType.Int, 4) { Value = parentPageId };
                sqlCommand.Parameters.Add(parameterParentPageId);

                connection.Open();

                try
                {
                    sqlCommand.ExecuteNonQuery();
                }
                finally
                {
                }
            }
        }

        #endregion

        #region FRIENDLY URL
        /// <summary>
        /// Use to update the Friendly URL 
        /// </summary>
        /// <param name="pageId">pageID</param>
        /// <param name="friendlyName">Friendly URL</param>
        /// <returns>0 or 1</returns>
        public string UpdateFriendlyURL(int pageId, string friendlyName)
        {
            using (var connection = Config.SqlConnectionString)
            using (var sqlCommand = new SqlCommand("rb_UpdateFriendlyURL", connection))
            {
                // Mark the Command as a SPROC
                sqlCommand.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                var parameterPageId = new SqlParameter(StrPageId, SqlDbType.Int, 4) { Value = pageId };
                sqlCommand.Parameters.Add(parameterPageId);

                var parametereFriendlyURL = new SqlParameter(StrFriendlyURL, SqlDbType.NVarChar, 512) { Value = friendlyName };
                sqlCommand.Parameters.Add(parametereFriendlyURL);

                var parameterOutput = new SqlParameter(output, SqlDbType.Int);
                parameterOutput.Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add(parameterOutput);
                connection.Open();
                try
                {
                    sqlCommand.ExecuteNonQuery();
                    return sqlCommand.Parameters["@result"].Value.ToString();
                }
                finally
                {
                }
            }
        }

        /// <summary>
        /// Get Frindly URL by page ID
        /// it is also used for URL rewrite 
        /// </summary>
        /// <param name="pageId">Page id</param>
        /// <returns>Friendly URL</returns>
        //Ashish.patel@haptix.biz - 2014/12/16 - Get FriendlyURL by PageID
        public string GetFriendlyURl(int pageId)
        {

            var connection = Config.SqlConnectionString;
            var sqlCommand = new SqlCommand("rb_GetFriendlyURLbyPageID", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Add Parameters to SPROC
            var parameterPageId = new SqlParameter(StrPageId, SqlDbType.Int, 4) { Value = pageId };
            sqlCommand.Parameters.Add(parameterPageId);

            // Execute the command
            connection.Open();
            var result = sqlCommand.ExecuteScalar().ToString();
            connection.Close();

            // Return the friendly url based on page id
            return result;
        }

        /// <summary>
        /// Get Friendly Url Pages
        /// </summary>
        /// <returns> returns data</returns>
        public DataTable GetFriendlyURlPages()
        {
            // Create Instance of Connection and Command Object
            using (var sqlConnection = Config.SqlConnectionString)
            {
                string commandText = "select * from rb_Pages where ISNULL(FriendlyURL, '') <> ''";
                var da = new SqlDataAdapter(commandText, sqlConnection);

                var dataTable = new DataTable("rb_Pages");

                // Read the result set
                try
                {
                    da.Fill(dataTable);
                }
                finally
                {
                    da.Dispose();
                }

                return dataTable;
            }
        }

        /// <summary>
        /// Delete friendly url
        /// </summary>
        /// <param name="pageId">page id</param>
        public void DeleteFriendlyUrl(int pageId)
        {
            using (var sqlConnection = Config.SqlConnectionString)
            {
                string commandText = "update rb_Pages set FriendlyURL='' where pageID='" + pageId + "'";

                SqlCommand sqlCommand = new SqlCommand(commandText, sqlConnection);
                sqlConnection.Open();
                try
                {
                    sqlCommand.ExecuteNonQuery();
                }
                finally
                {
                }
            }
        }

        /// <summary>
        /// Check for duplicate Friendly Url 
        /// </summary>
        /// <param name="friendlyURL"> Page Friendly URL</param>
        /// <param name="pageid">Page number</param>
        /// <returns>0 / 1 </returns>
        public bool IsAlreadyExistsFriendlyUrl(string friendlyURL, int pageid)
        {
            using (var connection = Config.SqlConnectionString)
            using (var sqlCommand = new SqlCommand("select count(*) from rb_Pages where ISNULL(FriendlyURL, '') = '" + friendlyURL + "' and PageId <> " + pageid, connection))
            {
                // Mark the Command as a SPROC
                sqlCommand.CommandType = CommandType.Text;
                // Add Parameters to SPROC
                connection.Open();
                try
                {
                    return Convert.ToBoolean(sqlCommand.ExecuteScalar());
                }

                finally
                {
                }
            }
        }
        #endregion

        #region Dynamic Page - Friendly URL
        /// <summary>
        /// Get Dynamic PageUrl
        /// </summary>
        /// <param name="pageUrl">page Url</param>
        /// <returns></returns>
        public string GetDynamicPageUrl(string pageUrl)
        {
            string redirectToUrl = "";
            try
            {
                using (var connection = Config.SqlConnectionString)
                using (var sqlCommand = new SqlCommand("select * from rb_Pages_DynamicRedirects where FriendlyUrl = '" + pageUrl + "'", connection))
                {

                    // Mark the Command as a SPROC
                    sqlCommand.CommandType = CommandType.Text;
                    // Add Parameters to SPROC
                    connection.Open();
                    try
                    {
                        SqlDataReader reder = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                        if (reder.HasRows && reder.Read())
                        {
                            redirectToUrl = reder["RedirectToUrl"].ToString();
                        }
                        reder.Close();
                    }

                    finally
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Publish(LogLevel.Warn, "Get Dynamic Page Url - ",ex);
            }
            return redirectToUrl;
        }

        /// <summary>
        /// Get Friendly Url Dynamic Pages
        /// </summary>
        /// <returns> returns data</returns>
        public DataTable GetFriendlyURlFromDynamicPages()
        {
            // Create Instance of Connection and Command Object
            using (var sqlConnection = Config.SqlConnectionString)
            {
                string commandText = "select *, ROW_NUMBER() OVER(ORDER BY RedirectToUrl) AS SRINDEX from rb_Pages_DynamicRedirects where ISNULL(FriendlyURL, '') <> ''";
                var da = new SqlDataAdapter(commandText, sqlConnection);

                var dataTable = new DataTable("rb_Pages_DynamicRedirects");

                // Read the result set
                try
                {
                    da.Fill(dataTable);
                }
                finally
                {
                    da.Dispose();
                }

                return dataTable;
            }
        }


        /// <summary>
        /// Delete friendly url
        /// </summary>
        /// <param name="dynamicPageID">dynamic Page ID</param>
        public void DeleteDynamicFriendlyUrl(int dynamicPageID)
        {
            using (var sqlConnection = Config.SqlConnectionString)
            {
                string commandText = "update rb_Pages_DynamicRedirects set FriendlyURL='' where DynamicPageID='" + dynamicPageID + "'";

                SqlCommand sqlCommand = new SqlCommand(commandText, sqlConnection);
                sqlConnection.Open();
                try
                {
                    sqlCommand.ExecuteNonQuery();
                }
                finally
                {
                }
            }
        }

        

        /// <summary>
        /// Update dynamic friendly URL
        /// </summary>
        /// <param name="redirectToUrl">redirectToUrl</param>
        /// <param name="friendlyUrl">friendlyUrl</param>
        /// <param name="dynamicPageId">dynamicPageId</param>
        /// <returns></returns>
        public string UpdateFriendlyURL(string redirectToUrl, string friendlyUrl, int dynamicPageId)
        {
            string dbRedirectToUrl = string.Empty;
            if (!IsAlreadyExistsDynamicFriendlyUrl(friendlyUrl, dynamicPageId))
            {
                using (var connection = Config.SqlConnectionString)
                using (var sqlCommand = new SqlCommand("update rb_Pages_DynamicRedirects  set RedirectToUrl='" + redirectToUrl + "' , FriendlyUrl='" + friendlyUrl + "' where DynamicPageID=" + dynamicPageId, connection))
                {
                    // Mark the Command as a Text
                    sqlCommand.CommandType = CommandType.Text;

                    connection.Open();
                    try
                    {
                        var result = sqlCommand.ExecuteScalar();
                    }
                    finally
                    {

                    }
                }
                return "1";
            }
            else
            {
                return "0";
            }
        }

        /// <summary>
        /// Check for duplicate Friendly Url 
        /// </summary>
        /// <param name="friendlyURL"> Page Friendly URL</param>
        /// <param name="dynamicPageID">dynamic Page ID</param>
        /// <returns>0 / 1 </returns>
        public bool IsAlreadyExistsDynamicFriendlyUrl(string friendlyURL, int dynamicPageID)
        {
            int result = 0;
            using (var connection = Config.SqlConnectionString)
            using (var sqlCommand = new SqlCommand("select count(*) from rb_Pages_DynamicRedirects where ISNULL(FriendlyUrl, '') = '" + friendlyURL + "' and dynamicPageID !=" + dynamicPageID, connection))
            {
                sqlCommand.CommandType = CommandType.Text;
                connection.Open();
                try
                {
                    result = Convert.ToInt32(sqlCommand.ExecuteScalar());
                    if (result == 0)
                    {
                        sqlCommand.CommandText = "select count(*) from rb_pages where ISNULL(FriendlyUrl, '') = '" + friendlyURL + "'";
                        result = Convert.ToInt32(sqlCommand.ExecuteScalar());
                    }
                }

                finally
                {
                }
            }

            return result > 0;
        }

        /// <summary>
        /// Get dynamicpage id by friendlyurl 
        /// </summary>
        /// <param name="friendlyURL"> Page Friendly URL</param>
        /// <returns>dyanamic page id </returns>
        public int GetDyanamicIDByFriendlyUrl(string friendlyURL)
        {
            using (var connection = Config.SqlConnectionString)
            using (var sqlCommand = new SqlCommand("select dynamicPageID from rb_Pages_DynamicRedirects where ISNULL(FriendlyUrl, '') = '" + friendlyURL + "' ", connection))
            {
                // Mark the Command as a SPROC
                sqlCommand.CommandType = CommandType.Text;
                // Add Parameters to SPROC
                connection.Open();
                try
                {
                    return Convert.ToInt32(sqlCommand.ExecuteScalar());
                }

                finally
                {
                }
            }
        }

        /// <summary>
        /// Create Friendly URL
        /// </summary>
        /// <param name="redirectToUrl"></param>
        /// <param name="friendlyUrl"></param>
        /// <param name="dynamicPageId"></param>
        /// <returns></returns>
        public string CreateFriendlyURL(string redirectToUrl, string friendlyUrl, int dynamicPageId)
        {
            if (!IsAlreadyExistsDynamicFriendlyUrl(friendlyUrl, dynamicPageId))
            {
                using (var connection = Config.SqlConnectionString)
                using (var sqlCommand = new SqlCommand("insert into rb_Pages_DynamicRedirects values('" + friendlyUrl + "','" + redirectToUrl + "')", connection))
                {
                    // Mark the Command as a Test
                    sqlCommand.CommandType = CommandType.Text;
                    connection.Open();
                    try
                    {
                        sqlCommand.ExecuteScalar();
                    }
                    finally
                    {

                    }
                }
                return "1";
            }
            else
            {
                return "0";
            }
        }

        #endregion

    }
}