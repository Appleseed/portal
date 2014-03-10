// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Utility.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   Utility Helper class for Appleseed Framework Monitoring purposes.
//   You get some static methods like
//   <list type="string">
//   <item>
//   int GetTotalPortalHits(int portalId)
//   </item>
//   <item>
//   DataSet GetMonitoringStats(DateTime startDate)
//   </item>
//   </list>
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Monitoring
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Web;
    using System.Web.Security;

    using Appleseed.Framework.Data;
    using Appleseed.Framework.Providers.AppleseedMembershipProvider;
    using Appleseed.Framework.Settings;

    /// <summary>
    /// Utility Helper class for Appleseed Framework Monitoring purposes.
    ///   You get some static methods like 
    /// <list type="string">
    /// <item>
    /// int GetTotalPortalHits(int portalId)
    /// </item>
    /// <item>
    /// DataSet GetMonitoringStats(DateTime startDate)
    /// </item>
    /// </list>
    /// </summary>
    public static class Utility
    {
        #region Public Methods

        /// <summary>
        /// Get Users Online
        ///   Add to the Cache
        ///   HttpContext.Current.Cache.Insert("WhoIsOnlineAnonUserCount", anonUserCount, null, DateTime.Now.AddMinutes(cacheTimeout), TimeSpan.Zero);
        ///   HttpContext.Current.Cache.Insert("WhoIsOnlineRegUserCount", regUsersOnlineCount, null, DateTime.Now.AddMinutes(cacheTimeout), TimeSpan.Zero);
        ///   HttpContext.Current.Cache.Insert("WhoIsOnlineRegUsersString", regUsersString, null, DateTime.Now.AddMinutes(cacheTimeout), TimeSpan.Zero);
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <param name="minutesToCheckForUsers">
        /// The minutes to check for users.
        /// </param>
        /// <param name="cacheTimeout">
        /// The cache timeout.
        /// </param>
        /// <param name="anonUserCount">
        /// The anon user count.
        /// </param>
        /// <param name="regUsersOnlineCount">
        /// The registered users online count.
        /// </param>
        /// <param name="regUsersString">
        /// The registered users string.
        /// </param>
        public static void FillUsersOnlineCache(
            int portalId, 
            int minutesToCheckForUsers, 
            int cacheTimeout, 
            out int anonUserCount, 
            out int regUsersOnlineCount, 
            out string regUsersString)
        {
            // Read from the cache if available
            if (true
                
                /* for test purposes, lets comment the cache functionality
                HttpContext.Current.Cache["WhoIsOnlineAnonUserCount"] == null ||
                HttpContext.Current.Cache["WhoIsOnlineRegUserCount"] == null ||
                HttpContext.Current.Cache["WhoIsOnlineRegUsersString"] == null */)
            {
                var onlineUsers = ((AppleseedMembershipProvider)Membership.Provider).GetOnlineUsers();
                regUsersString = string.Join(", ", onlineUsers);
                regUsersOnlineCount = onlineUsers.Count;

                // until we get an efficient method to obtain all the profiles online (including users and not users), we are going to return zero anonymous users.
                anonUserCount = onlineUsers.Count - onlineUsers.Count;

                // Add to the Cache
                HttpContext.Current.Cache.Insert(
                    "WhoIsOnlineAnonUserCount", 
                    anonUserCount, 
                    null, 
                    DateTime.Now.AddMinutes(cacheTimeout), 
                    TimeSpan.Zero);
                HttpContext.Current.Cache.Insert(
                    "WhoIsOnlineRegUserCount", 
                    regUsersOnlineCount, 
                    null, 
                    DateTime.Now.AddMinutes(cacheTimeout), 
                    TimeSpan.Zero);
                HttpContext.Current.Cache.Insert(
                    "WhoIsOnlineRegUsersString", 
                    regUsersString, 
                    null, 
                    DateTime.Now.AddMinutes(cacheTimeout), 
                    TimeSpan.Zero);
            }
            
            // else
            // {
            //     anonUserCount = (int)HttpContext.Current.Cache["WhoIsOnlineAnonUserCount"];
            //     regUsersOnlineCount = (int)HttpContext.Current.Cache["WhoIsOnlineRegUserCount"];
            //     regUsersString = (string)HttpContext.Current.Cache["WhoIsOnlineRegUsersString"];
            // }
        }

        /// <summary>
        /// Return a dataset of stats for a given data range and portal
        ///   Written by Paul Yarrow, paul@paulyarrow.com
        /// </summary>
        /// <param name="startDate">
        /// the first date you want to see stats from
        /// </param>
        /// <param name="endDate">
        /// the last date you want to see stats up to
        /// </param>
        /// <param name="reportType">
        /// type of report you are requesting
        /// </param>
        /// <param name="currentTabId">
        /// page id you are requesting stats for
        /// </param>
        /// <param name="includeMonitoringPage">
        /// include the monitoring page in the stats
        /// </param>
        /// <param name="includePageRequests">
        /// include page hits
        /// </param>
        /// <param name="includeLogon">
        /// include the logon page
        /// </param>
        /// <param name="includeLogoff">
        /// include logoff page
        /// </param>
        /// <param name="includeMyIpAddress">
        /// include the current IP address
        /// </param>
        /// <param name="portalId">
        /// portal id to get stats for
        /// </param>
        /// <returns>
        /// The data set.
        /// </returns>
        public static DataSet GetMonitoringStats(
            DateTime startDate, 
            DateTime endDate, 
            string reportType, 
            long currentTabId, 
            bool includeMonitoringPage, 
            bool includePageRequests, 
            bool includeLogon, 
            bool includeLogoff, 
            bool includeMyIpAddress, 
            int portalId)
        {
            endDate = endDate.AddDays(1);

            // Firstly get the logged in users
            var connection = Config.SqlConnectionString;
            var command = new SqlDataAdapter("rb_GetMonitoringEntries", connection)
                {
                    SelectCommand =
                    {
                        CommandType = CommandType.StoredProcedure 
                    } 
                };

            // Add Parameters to SPROC
            var parameterPortalId = new SqlParameter("@PortalID", SqlDbType.Int, 4) { Value = portalId };
            command.SelectCommand.Parameters.Add(parameterPortalId);

            var parameterStartDate = new SqlParameter("@StartDate", SqlDbType.DateTime, 8) { Value = startDate };
            command.SelectCommand.Parameters.Add(parameterStartDate);

            var parameterEndDate = new SqlParameter("@EndDate", SqlDbType.DateTime, 8) { Value = endDate };
            command.SelectCommand.Parameters.Add(parameterEndDate);

            var parameterReportType = new SqlParameter("@ReportType", SqlDbType.VarChar, 50) { Value = reportType };
            command.SelectCommand.Parameters.Add(parameterReportType);

            var parameterCurrentTabId = new SqlParameter("@CurrentTabID", SqlDbType.BigInt, 8) { Value = currentTabId };
            command.SelectCommand.Parameters.Add(parameterCurrentTabId);

            var parameterIncludeMoni = new SqlParameter("@IncludeMonitorPage", SqlDbType.Bit, 1)
                {
                    Value = includeMonitoringPage 
                };
            command.SelectCommand.Parameters.Add(parameterIncludeMoni);

            var parameterIncludePageRequests = new SqlParameter("@IncludePageRequests", SqlDbType.Bit, 1)
                {
                    Value = includePageRequests 
                };
            command.SelectCommand.Parameters.Add(parameterIncludePageRequests);

            var parameterIncludeLogon = new SqlParameter("@IncludeLogon", SqlDbType.Bit, 1) { Value = includeLogon };
            command.SelectCommand.Parameters.Add(parameterIncludeLogon);

            var parameterIncludeLogoff = new SqlParameter("@IncludeLogoff", SqlDbType.Bit, 1) { Value = includeLogoff };
            command.SelectCommand.Parameters.Add(parameterIncludeLogoff);

            var parameterIncludeIpAddress = new SqlParameter("@IncludeIPAddress", SqlDbType.Bit, 1)
                {
                    Value = includeMyIpAddress 
                };
            command.SelectCommand.Parameters.Add(parameterIncludeIpAddress);

            var parameterIpAddress = new SqlParameter("@IPAddress", SqlDbType.VarChar, 16)
                {
                    Value = HttpContext.Current.Request.UserHostAddress 
                };
            command.SelectCommand.Parameters.Add(parameterIpAddress);

            // Create and Fill the DataSet
            var dataSet = new DataSet();
            try
            {
                command.Fill(dataSet);
            }
            finally
            {
                connection.Close();
            }

            // Return the DataSet
            return dataSet;
        }

        /// <summary>
        /// returns the total hit count for a portal
        /// </summary>
        /// <param name="portalId">
        /// portal id to get stats for
        /// </param>
        /// <returns>
        /// total number of hits to the portal of all types
        /// </returns>
        public static int GetTotalPortalHits(int portalId)
        {
            // TODO: This should not display, login, logout, or administrator hits.
            var sql = string.Format("Select count(ID) as hits  from rb_monitoring  where [PortalID] = {0} ", portalId);

            return DBHelper.ExecuteSqlScalar<int>(sql);
        }

        #endregion
    }
}