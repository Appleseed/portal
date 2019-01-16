// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppleseedSqlSiteMapProvider.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The appleseed sql site map provider.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Providers.AppleseedSiteMapProvider
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration.Provider;
    using System.Data;
    using System.Data.SqlClient;
    using System.Security.Permissions;
    using System.Threading;
    using System.Web;
    using System.Web.Caching;
    using System.Web.Configuration;

    using Appleseed.Context;
    using Appleseed.Framework.Settings;

    using Reader = Appleseed.Context.Reader;
    using Site.Configuration;

    /// <summary>
    /// The appleseed sql site map provider.
    /// </summary>
    /// <remarks>
    /// </remarks>
    [SqlClientPermission(SecurityAction.Demand, Unrestricted = true)]
    public class AppleseedSqlSiteMapProvider : AppleseedSiteMapProvider
    {
        #region Constants and Fields

        /// <summary>
        ///   The cache dependency name.
        /// </summary>
        public const string CacheDependencyName = "__SiteMapCacheDependency";

        /// <summary>
        ///   The errmsg 1.
        /// </summary>
        private const string Errmsg1 = "Missing node ID";

        /// <summary>
        ///   The errmsg 2.
        /// </summary>
        private const string Errmsg2 = "Duplicate node ID";

        /*
        /// <summary>
        ///   The errmsg 4.
        /// </summary>
        private const string Errmsg4 = "Invalid parent ID: {0} on this list: {1}";
*/

        /// <summary>
        ///   The errmsg 5.
        /// </summary>
        private const string Errmsg5 = "Empty or missing connectionStringName";

        /// <summary>
        ///   The errmsg 6.
        /// </summary>
        private const string Errmsg6 = "Missing connection string";

        /// <summary>
        ///   The errmsg 7.
        /// </summary>
        private const string Errmsg7 = "Empty connection string";

        /// <summary>
        ///   The errmsg 8.
        /// </summary>
        private const string Errmsg8 = "Invalid sqlCacheDependency";

        /// <summary>
        ///   The root node id.
        /// </summary>
        private const int RootNodeId = -1;

        /// <summary>
        ///   The lock.
        /// </summary>
        private readonly object _theLock = new object();

        /// <summary>
        ///   The nodes.
        /// </summary>
        private readonly Dictionary<int, SiteMapNode> theNodes = new Dictionary<int, SiteMapNode>(16);

        /// <summary>
        ///   The connect.
        /// </summary>
        private string connect; // Database connection string

        /// <summary>
        ///   The database.
        /// </summary>
        private string database; // Database info for SQL Server 7/2000 cache dependency

        /// <summary>
        ///   The dependency 2005.
        /// </summary>
        private bool dependency2005; // Database info for SQL Server 2005 cache dependency

        /// <summary>
        ///   The index authorized roles.
        /// </summary>
        private int indexAuthorizedRoles;

        /// <summary>
        ///   The index page description.
        /// </summary>
        private int indexPageDescription;

        /// <summary>
        ///   The index page id.
        /// </summary>
        private int indexPageId;

        /// <summary>
        ///   The index page layout.
        /// </summary>
        private int indexPageLayout;

        /// <summary>
        ///   The index page name.
        /// </summary>
        private int indexPageName;

        /// <summary>
        ///   The index page order.
        /// </summary>
        private int indexPageOrder;

        /// <summary>
        ///   The index parent page id.
        /// </summary>
        private int indexParentPageId;

        /// <summary>
        ///   The index portal id.
        /// </summary>
        private int indexPortalId;

        /// <summary>
        ///   The root.
        /// </summary>
        private SiteMapNode root;

        /// <summary>
        ///   The table.
        /// </summary>
        private string table; // Database info for SQL Server 7/2000 cache dependency

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the portal ID.
        /// </summary>
        /// <remarks>
        /// </remarks>
        private static string PortalId
        {
            get
            {
                var contextReader = new Reader(new WebContextReader());
                var context = contextReader.Current;
                return context.Items["PortalID"].ToString();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Loads the site map information from rb_Pages table and builds the site map information
        ///   in memory.
        /// </summary>
        /// <returns>
        /// The root System.Web.SiteMapNode of the site map navigation structure.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override SiteMapNode BuildSiteMap()
        {
            lock (this._theLock)
            {
                // Return immediately if this method has been called before
                // if (_root != null) {
                // if (_root["PortalID"] == PortalID) {
                // return _root;
                // } else {
                this.Clear();

                var pS = (PortalSettings)HttpContext.Current.Items["PortalSettings"];
                if (pS.CustomSettings["ENABLE_PRIVATE_SITE"].Value.ToString() == "True" && !HttpContext.Current.Request.IsAuthenticated)
                {
                    this.root = new SiteMapNode(
                            this,
                            RootNodeId.ToString(),
                            HttpUrlBuilder.BuildUrl(),
                            string.Empty,
                            string.Empty,
                            new[] { "All Users" },
                            null,
                            null,
                            null);
                    this.root["PortalID"] = PortalId;
                    this.theNodes.Add(RootNodeId, this.root);
                    this.AddNode(this.root, null);
                    return this.root;
                } 

                // }
                // }

                // Query the database for site map nodes
                var connection = new SqlConnection(this.connect);

                try
                {
                    var command = new SqlCommand(BuildSiteMapQuery(), connection) { CommandType = CommandType.Text };
                    command.CommandType = CommandType.StoredProcedure;
                    var parameterPageId = new SqlParameter("@PortalID", SqlDbType.Int, 4) { Value = PortalId };
                    command.Parameters.Add(parameterPageId);
                    //var parameterCulture = new SqlParameter("@Culture", SqlDbType.VarChar, 5) { Value = Thread.CurrentThread.CurrentUICulture.ToString() };
                    //command.Parameters.Add(parameterCulture);

                    //Thread.CurrentThread.CurrentUICulture + PortalId
                
                    // Create a SQL cache dependency if requested
                    SqlCacheDependency dependency = null;

                    if (this.dependency2005)
                    {
                        dependency = new SqlCacheDependency(command);
                    }
                    else if (!String.IsNullOrEmpty(this.database) && !string.IsNullOrEmpty(this.table))
                    {
                        dependency = new SqlCacheDependency(this.database, this.table);
                    }

                    connection.Open();

                    var reader = command.ExecuteReader();
                    this.indexPageId = reader.GetOrdinal("PageID");
                    this.indexParentPageId = reader.GetOrdinal("ParentPageID");
                    this.indexPageOrder = reader.GetOrdinal("PageOrderInt");
                    this.indexPortalId = reader.GetOrdinal("PortalID");
                    this.indexPageName = reader.GetOrdinal("PageName");
                    this.indexAuthorizedRoles = reader.GetOrdinal("AuthorizedRoles");
                    this.indexPageLayout = reader.GetOrdinal("PageLayout");
                    this.indexPageDescription = reader.GetOrdinal("PageDescription");

                    if (reader.Read())
                    {
                        // Create an empty root node and add it to the site map
                        this.root = new SiteMapNode(
                            this, 
                            RootNodeId.ToString(), 
                            HttpUrlBuilder.BuildUrl(), 
                            string.Empty, 
                            string.Empty, 
                            new[] { "All Users" }, 
                            null, 
                            null, 
                            null);
                        this.root["PortalID"] = PortalId;
                        this.theNodes.Add(RootNodeId, this.root);
                        this.AddNode(this.root, null);

                        // Build a tree of SiteMapNodes underneath the root node
                        do
                        {
                            // Create another site map node and add it to the site map
                            var node = this.CreateSiteMapNodeFromDataReader(reader);
                            var parentNode = this.GetParentNodeFromDataReader(reader);
                            //ErrorHandler.Publish(LogLevel.Info, node.Url);
                            if (parentNode != null)
                            {
                                try
                                {
                                    this.AddNode(node, parentNode);
                                    //ErrorHandler.Publish(LogLevel.Info, "parentNode>"+ parentNode.Url);

                                }
                                catch (Exception ex)
                                {
                                    //ErrorHandler.Publish(LogLevel.Info, "NOT Added Error - parentNode>" + parentNode.Url);
                                    //node.Url = node.Url.Contains("?")
                                    //               ? string.Format("{0}&lnkId={1}", node.Url, node.Key)
                                    //               : string.Format("{0}?lnkId={1}", node.Url, node.Key);
                                    node.Url = "/default.aspx?lnkId=" + node.Key;
                                    this.AddNode(node, parentNode);
                                }
                            }
                            else
                            {
                                //ErrorHandler.Publish(LogLevel.Info, " parentNode NULL");
                            }
                        }
                        while (reader.Read());

                        // Use the SQL cache dependency
                        if (dependency != null) {
                            HttpRuntime.Cache.Insert(
                                CacheDependencyName + PortalId,
                                new object(),
                                dependency,
                                Cache.NoAbsoluteExpiration,
                                Cache.NoSlidingExpiration,
                                CacheItemPriority.NotRemovable,
                                this.OnSiteMapChanged);
                        } else { 

                        }
                    }
                }
                finally
                {
                    connection.Close();
                }

                // Return the root SiteMapNode
                return this.root;
            }
        }

        /// <summary>
        /// Removes all elements in the collections of child and parent site map nodes
        ///   that the System.Web.StaticSiteMapProvider tracks as part of its state.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public override void ClearCache()
        {
            this.Clear();
        }

        /// <summary>
        /// Initializes the provider.
        /// </summary>
        /// <param name="name">
        /// The friendly name of the provider.
        /// </param>
        /// <param name="config">
        /// A collection of the name/value pairs representing the provider-specific attributes specified in the configuration for this provider.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The name of the provider is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        /// The name of the provider has a length of zero.
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// An attempt is made to call <see cref="M:System.Configuration.Provider.ProviderBase.Initialize(System.String,System.Collections.Specialized.NameValueCollection)"/> on a provider after the provider has already been initialized.
        /// </exception>
        /// <remarks>
        /// </remarks>
        public override void Initialize(string name, NameValueCollection config)
        {
            // Verify that config isn't null
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            // Assign the provider a default name if it doesn't have one
            if (String.IsNullOrEmpty(name))
            {
                name = "AppleseedSqlSiteMapProvider";
            }

            // Add a default "description" attribute to config if the
            // attribute doesn’t exist or is empty
            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Appleseed SQL site map provider");
            }

            // Call the base class's Initialize method
            base.Initialize(name, config);

            // Initialize _connect
            var connectionStringName = config["connectionStringName"];

            if (String.IsNullOrEmpty(connectionStringName))
            {
                throw new ProviderException(Errmsg5);
            }

            config.Remove("connectionStringName");

            if (WebConfigurationManager.ConnectionStrings[connectionStringName] == null)
            {
                throw new ProviderException(Errmsg6);
            }

            this.connect = WebConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;

            if (String.IsNullOrEmpty(this.connect))
            {
                throw new ProviderException(Errmsg7);
            }

            // Initialize SQL cache dependency info
            var dependency = config["sqlCacheDependency"];

            if (!String.IsNullOrEmpty(dependency))
            {
                if (String.Equals(dependency, "CommandNotification", StringComparison.InvariantCultureIgnoreCase))
                {
                    SqlDependency.Start(this.connect);
                    this.dependency2005 = true;
                }
                else
                {
                    // If not "CommandNotification", then extract database and table names
                    var info = dependency.Split(new[] { ':' });
                    if (info.Length != 2)
                    {
                        throw new ProviderException(Errmsg8);
                    }

                    this.database = info[0];
                    this.table = info[1];
                }

                config.Remove("sqlCacheDependency");
            }

            // SiteMapProvider processes the securityTrimmingEnabled
            // attribute but fails to remove it. Remove it now so we can
            // check for unrecognized configuration attributes.
            if (config["securityTrimmingEnabled"] != null)
            {
                config.Remove("securityTrimmingEnabled");
            }

            // Throw an exception if unrecognized attributes remain
            if (config.Count > 0)
            {
                var attr = config.GetKey(0);
                if (!String.IsNullOrEmpty(attr))
                {
                    throw new ProviderException("Unrecognized attribute: " + attr);
                }
            }
        }

        /// <summary>
        /// Retrieves a Boolean value indicating whether the specified <see cref="T:System.Web.SiteMapNode"/> object can be viewed by the user in the specified context.
        /// </summary>
        /// <param name="context">
        /// The <see cref="T:System.Web.HttpContext"/> that contains user information.
        /// </param>
        /// <param name="node">
        /// The <see cref="T:System.Web.SiteMapNode"/> that is requested by the user.
        /// </param>
        /// <returns>
        /// true if security trimming is enabled and <paramref name="node"/> can be viewed by the user or security trimming is not enabled; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="context"/> is null.- or -<paramref name="node"/> is null.
        /// </exception>
        /// <remarks>
        /// </remarks>
        public override bool IsAccessibleToUser(HttpContext context, SiteMapNode node)
        {
            var isVisible = false;

            if (node.Roles != null)
            {
                if (context.User != null && context.User.Identity.IsAuthenticated)
                {
                    if (node.Roles.Contains("All Users") || node.Roles.Contains("Authenticated Users") || context.User.IsInRole("Admins"))
                    {
                        isVisible = true;
                    }
                    else
                    {
                        var enumerator = node.Roles.GetEnumerator();
                        while (!isVisible && enumerator.MoveNext())
                        {
                            isVisible = context.User.IsInRole((string)enumerator.Current);
                        }
                    }
                }
                else
                {
                    isVisible = node.Roles.Contains("All Users") || node.Roles.Contains("Unauthenticated Users");
                }
            }

            return isVisible;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Removes all elements in the collections of child and parent site map nodes
        ///   that the System.Web.StaticSiteMapProvider tracks as part of its state.
        /// </summary>
        /// <remarks>
        /// </remarks>
        protected override void Clear()
        {
            base.Clear();
            this.theNodes.Clear();
            this.root = null;
        }

        /// <summary>
        /// Returns the root node.
        /// </summary>
        /// <returns>
        /// The root node.
        /// </returns>
        /// <remarks>
        /// </remarks>
        protected override SiteMapNode GetRootNodeCore()
        {
            lock (this._theLock)
            {
                this.BuildSiteMap();
                return this.root;
            }
        }

        /// <summary>
        /// Builds the site map query.
        /// </summary>
        /// <returns>
        /// The SQL string.
        /// </returns>
        /// <remarks>
        /// </remarks>
        private static string BuildSiteMapQuery()
        {
            var s =
                @"
                SELECT	[PageID], [ParentPageID], [PageOrder], [PortalID], COALESCE (
                   (SELECT SettingValue
                    FROM   rb_TabSettings
                    WHERE  TabID = rb_Pages.PageID 
                       AND SettingName = @Culture
                       AND Len(SettingValue) > 0), 
                    PageName)  AS [PageName],[AuthorizedRoles], [PageLayout], [PageDescription]
                FROM  [dbo].[rb_Pages] 
                WHERE [PortalID] = @PortalID
                ORDER BY ParentPageid, [PageOrder]
            ";

            s = "rb_GetPageTree";
            return s;
        }

        /// <summary>
        /// Creates the site map node from data reader.
        /// </summary>
        /// <param name="reader">
        /// The reader.
        /// </param>
        /// <returns>
        /// A site map node.
        /// </returns>
        /// <remarks>
        /// </remarks>
        private SiteMapNode CreateSiteMapNodeFromDataReader(IDataRecord reader)
        {
            // Make sure the node ID is present
            if (reader.IsDBNull(this.indexPageId))
            {
                throw new ProviderException(Errmsg1);
            }

            // Get the node ID from the DataReader
            var id = reader.GetInt32(this.indexPageId);

            // Make sure the node ID is unique
            if (this.theNodes.ContainsKey(id))
            {
                throw new ProviderException(Errmsg2);
            }

            var name = reader.IsDBNull(this.indexPageName) ? null : reader.GetString(this.indexPageName).Trim();
            var description = reader.IsDBNull(this.indexPageDescription)
                                  ? null
                                  : reader.GetString(this.indexPageDescription).Trim();
            var roles = reader.IsDBNull(this.indexAuthorizedRoles)
                            ? null
                            : reader.GetString(this.indexAuthorizedRoles).Trim();

            var url = HttpUrlBuilder.BuildUrl(id);
            if (!string.IsNullOrEmpty(url) && !url.StartsWith("/") && !url.StartsWith(Path.ApplicationFullPath))
            {
                url = HttpUrlBuilder.BuildUrl("~/" + HttpUrlBuilder.DefaultPage, "sitemapTargetPage=" + id);
            }

            // If roles were specified, turn the list into a string array
            string[] rolelist = null;
            if (!String.IsNullOrEmpty(roles))
            {
                rolelist = roles.Split(new[] { ',', ';' }, 512);
            }

            if (rolelist != null)
            {
                var rolelistLength = rolelist.Length;
                if (rolelistLength > 0)
                {
                    if (rolelist[rolelistLength - 1].Equals(string.Empty))
                    {
                        var auxrolelist = new string[rolelistLength - 1];
                        for (var i = 0; i < rolelistLength - 1; i++)
                        {
                            auxrolelist[i] = rolelist[i];
                        }

                        rolelist = auxrolelist;
                    }
                }
            }

            // Create a SiteMapNode
            var node = new SiteMapNode(this, id.ToString(), url, name, description, rolelist, null, null, null);

            // Record the node in the theNodes dictionary
            this.theNodes.Add(id, node);

            // Return the node        
            return node;
        }

        /// <summary>
        /// Gets the parent node from data reader.
        /// </summary>
        /// <param name="reader">
        /// The reader.
        /// </param>
        /// <returns>
        /// A site map node.
        /// </returns>
        /// <remarks>
        /// </remarks>
        private SiteMapNode GetParentNodeFromDataReader(IDataRecord reader)
        {
            // Make sure the parent ID is present
            if (reader.IsDBNull(this.indexParentPageId))
            {
                return this.theNodes[RootNodeId];
            }

            // Get the parent ID from the DataReader
            var pid = reader.GetInt32(this.indexParentPageId);

            // Make sure the parent ID is valid
            if (!this.theNodes.ContainsKey(pid))
            {
                // string list = string.Empty;
                // foreach (int key in theNodes.Keys) {
                // list += String.Format("{0}-{1};", key, theNodes[key].Key);
                // }
                // throw new ProviderException(String.Format(_errmsg4, pid, list));
                return null;
            }

            // Return the parent SiteMapNode
            return this.theNodes[pid];
        }

        /// <summary>
        /// Called when [site map changed].
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <param name="reason">
        /// The reason.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void OnSiteMapChanged(string key, object item, CacheItemRemovedReason reason)
        {
            lock (this._theLock)
            {
                if (key != CacheDependencyName || reason != CacheItemRemovedReason.DependencyChanged)
                {
                    return;
                }

                // Refresh the site map
                this.Clear();
                this.theNodes.Clear();
                this.root = null;
            }
        }

        #endregion
    }
}