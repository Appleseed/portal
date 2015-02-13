// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HtmlTextDB.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   Class that encapsulates all data logic necessary to add/query/delete
//   HTML/text within the Portal database.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Content.Data
{
    using System.Data;
    using System.Data.SqlClient;

    using Appleseed.Framework.Settings;
    using System;

    /// <summary>
    /// Class that encapsulates all data logic necessary to add/query/delete
    ///     HTML/text within the Portal database.
    /// </summary>
    public class HtmlTextDB
    {
        #region Public Methods

        /// <summary>
        /// The GetHtmlText method returns a SqlDataReader containing details
        ///     about a specific item from the HtmlText database table.
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <returns>
        /// A sql data reader
        /// </returns>
        public SqlDataReader GetHtmlText(int moduleId)
        {
            // Change by Geert.Audenaert@Syntegra.Com
            // Date: 6/2/2003
            // Get prod version by default
            return this.GetHtmlText(moduleId, WorkFlowVersion.Production, 1);

            // End Change Geert.Audenaert@Syntegra.Com
        }

        /// <summary>
        /// The GetHtmlText method returns a SqlDataReader containing details
        ///     about a specific item from the HtmlText database table.
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <param name="version">
        /// The version.
        /// </param>
        /// <returns>
        /// A sql data reader
        /// </returns>
        public SqlDataReader GetHtmlText(int moduleId, WorkFlowVersion version, int versionsModule)
        {
            // Create Instance of Connection and Command Object
            var connection = Config.SqlConnectionString;

            // Mark the Command as a SPROC
            var command = new SqlCommand("rb_GetHtmlText", connection) { CommandType = CommandType.StoredProcedure };

            // Add Parameters to SPROC
            var parameterModuleId = new SqlParameter("@ModuleID", SqlDbType.Int, 4) { Value = moduleId };
            command.Parameters.Add(parameterModuleId);

            // Change by Geert.Audenaert@Syntegra.Com
            // Date: 6/2/2003
            var parameterWorkflowVersion = new SqlParameter("@WorkflowVersion", SqlDbType.Int, 4)
                {
                    Value = (int)version
                };
            command.Parameters.Add(parameterWorkflowVersion);

            var moduleVersion = new SqlParameter("@VersionNo", SqlDbType.Int, 4) { Value = versionsModule };
            command.Parameters.Add(moduleVersion);
            // End Change Geert.Audenaert@Syntegra.Com

            // Execute the command
            connection.Open();
            var result = command.ExecuteReader(CommandBehavior.CloseConnection);

            // Return the datareader 
            return result;
        }

        /// <summary>
        /// Get Published HtmlText 
        /// </summary>
        /// <param name="moduleId">ModuleID</param>
        /// <param name="version">workflow version</param>
        /// <returns></returns>
        public string GetPublishedVersionHtmlText(int moduleId, WorkFlowVersion version)
        {
            var strDesktopHtml = string.Empty;

            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            {
                using (var command = new SqlCommand("rb_GetPublishedVersionHtmlText", connection))
                {
                    // Mark the Command as a SPROC
                    command.CommandType = CommandType.StoredProcedure;

                    // Add Parameters to SPROC
                    var parameterModuleId = new SqlParameter("@ModuleID", SqlDbType.Int, 4) { Value = moduleId };
                    command.Parameters.Add(parameterModuleId);

                    // Change by Geert.Audenaert@Syntegra.Com
                    // Date: 6/2/2003
                    var parameterWorkflowVersion = new SqlParameter("@WorkflowVersion", SqlDbType.Int, 4)
                    {
                        Value = (int)version
                    };
                    command.Parameters.Add(parameterWorkflowVersion);

                    // End Change Geert.Audenaert@Syntegra.Com

                    // Execute the command
                    connection.Open();

                    using (var result = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        try
                        {
                            if (result.Read())
                            {
                                strDesktopHtml = result["DesktopHtml"].ToString();
                            }
                        }
                        finally
                        {
                            // Close the datareader
                            result.Close();
                        }
                    }
                }
            }

            return strDesktopHtml;
        }


        /// <summary>
        /// Get Html Text String
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <param name="version">
        /// The version.
        /// </param>
        /// <param name="mobileSummary">
        /// The mobile Summary.
        /// </param>
        /// <param name="mobileDetails">
        /// The mobile Details.
        /// </param>
        /// <returns>
        /// The get html text string.
        /// </returns>
        public string GetHtmlTextString(
            int moduleId, WorkFlowVersion version, out string mobileSummary, out string mobileDetails)
        {
            var strDesktopHtml = string.Empty;

            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (var command = new SqlCommand("rb_GetHtmlText", connection))
            {
                // Mark the Command as a SPROC
                command.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                var parameterModuleId = new SqlParameter("@ModuleID", SqlDbType.Int, 4) { Value = moduleId };
                command.Parameters.Add(parameterModuleId);

                // Change by Geert.Audenaert@Syntegra.Com
                // Date: 6/2/2003
                var parameterWorkflowVersion = new SqlParameter("@WorkflowVersion", SqlDbType.Int, 4)
                    {
                        Value = (int)version
                    };
                command.Parameters.Add(parameterWorkflowVersion);

                // End Change Geert.Audenaert@Syntegra.Com

                // Execute the command
                connection.Open();

                using (var result = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    try
                    {
                        if (result.Read())
                        {
                            strDesktopHtml = result["DesktopHtml"].ToString();
                            mobileSummary = result["MobileSummary"].ToString();
                            mobileDetails = result["MobileDetails"].ToString();
                        }
                        else
                        {
                            mobileSummary = string.Empty;
                            mobileDetails = string.Empty;
                        }
                    }
                    finally
                    {
                        // Close the datareader
                        result.Close();
                    }
                }
            }

            return strDesktopHtml;
        }

        /// <summary>
        /// Get Html Text String
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <param name="version">
        /// The version.
        /// </param>
        /// <returns>
        /// The get html text string.
        /// </returns>
        [History("Ashish.patel@haptix.biz", "2014/12/23", "Get published version content")]
        public string GetHtmlTextString(int moduleId, WorkFlowVersion version)
        {
            var strDesktopHtml = string.Empty;

            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            {
                using (var command = new SqlCommand("rb_GetHtmlText", connection))
                {
                    // Mark the Command as a SPROC
                    command.CommandType = CommandType.StoredProcedure;

                    // Add Parameters to SPROC
                    var parameterModuleId = new SqlParameter("@ModuleID", SqlDbType.Int, 4) { Value = moduleId };
                    command.Parameters.Add(parameterModuleId);

                    // Change by Geert.Audenaert@Syntegra.Com
                    // Date: 6/2/2003
                    var parameterWorkflowVersion = new SqlParameter("@WorkflowVersion", SqlDbType.Int, 4)
                        {
                            Value = (int)version
                        };
                    command.Parameters.Add(parameterWorkflowVersion);

                    //Add by Ashish.patel@haptix.biz
                    // Date: 12/23/2014
                    // Get published version content
                    int publishedVersion = 1;
                    SqlDataReader sqlDatard = this.GetHtmlTextRecord(moduleId);
                    if (sqlDatard.HasRows)
                    {
                        while (sqlDatard.Read())
                        {
                            if (Convert.ToBoolean(sqlDatard["Published"]))
                            {
                                publishedVersion = Convert.ToInt32(sqlDatard["VersionNo"]);
                                break;
                            }
                        }
                    }

                    var moduleVersion = new SqlParameter("@VersionNo", SqlDbType.Int, 4) { Value = publishedVersion };
                    command.Parameters.Add(moduleVersion);

                    // End Change Geert.Audenaert@Syntegra.Com

                    // Execute the command
                    connection.Open();

                    using (var result = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        try
                        {
                            if (result.Read())
                            {
                                strDesktopHtml = result["DesktopHtml"].ToString();
                            }
                        }
                        finally
                        {
                            // Close the datareader
                            result.Close();
                        }
                    }
                }
            }

            return strDesktopHtml;
        }


        /// <summary>
        /// The UpdateHtmlText method updates a specified item within
        ///     the HtmlText database table.
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <param name="desktopHtml">
        /// The desktop HTML.
        /// </param>
        /// <param name="mobileSummary">
        /// The mobile summary.
        /// </param>
        /// <param name="mobileDetails">
        /// The mobile details.
        /// </param>
        /// <param name="version">
        /// Item version
        /// </param>
        /// <param name="published">
        /// Published status (1/0)
        /// </param>
        /// <param name="createdDate">
        /// Created Date
        /// </param>
        /// <param name="createdByUserName">
        /// Name who create this html blog
        /// </param>
        /// <param name="modifiedDate">
        /// Modified date this html blog
        /// </param>
        /// <param name="modifiedByUserName">
        /// Name Who modified this html blog
        /// </param>

        [History("Ashish.patel@haptix.biz", "2014/11/20", "Modifed and add two paremeters")]
        public void UpdateHtmlText(int moduleId, string desktopHtml, string mobileSummary, string mobileDetails, int version, Boolean published, DateTime createdDate, string createdByUserName, DateTime modifiedDate, string modifiedByUserName)
        {
            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (var command = new SqlCommand("rb_UpdateHtmlText", connection))
            {
                // Mark the Command as a SPROC
                command.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                var parameterModuleId = new SqlParameter("@ModuleID", SqlDbType.Int, 4) { Value = moduleId };
                command.Parameters.Add(parameterModuleId);

                var parameterDesktopHtml = new SqlParameter("@DesktopHtml", SqlDbType.NText) { Value = desktopHtml };
                command.Parameters.Add(parameterDesktopHtml);

                var parameterMobileSummary = new SqlParameter("@MobileSummary", SqlDbType.NText) { Value = mobileSummary };
                command.Parameters.Add(parameterMobileSummary);

                var parameterMobileDetails = new SqlParameter("@MobileDetails", SqlDbType.NText) { Value = mobileDetails };
                command.Parameters.Add(parameterMobileDetails);

                // It will increase the version for same module
                var versionNo = new SqlParameter("@VersionNo", SqlDbType.Int) { Value = version };
                command.Parameters.Add(versionNo);

                var publishedVersion = new SqlParameter("@Published", SqlDbType.Bit) { Value = published };
                command.Parameters.Add(publishedVersion);

                var CreatedDate = new SqlParameter("@CreatedDate", SqlDbType.DateTime) { Value = createdDate };
                command.Parameters.Add(CreatedDate);

                var prcreatedByUserName = new SqlParameter("@CreatedByUserName", SqlDbType.NVarChar) { Value = createdByUserName };
                command.Parameters.Add(prcreatedByUserName);

                var ModifiedDate = new SqlParameter("@ModifiedDate", SqlDbType.DateTime) { Value = modifiedDate };
                command.Parameters.Add(ModifiedDate);

                var prmodifiedByUserName = new SqlParameter("@ModifiedByUserName", SqlDbType.NVarChar) { Value = modifiedByUserName };
                command.Parameters.Add(prmodifiedByUserName);

                // SqlParameter parameterCulture = new SqlParameter("@Culture", SqlDbType.NVarChar, 8);
                // parameterCulture.Value = culture.Name;
                // command.Parameters.Add(parameterCulture);
                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Method is used to get all version of given moduleId
        /// </summary>
        /// <param name="moduleID">ModuleID</param>
        /// <returns>List of versions</returns>
        [History("Ashish.patel@haptix.biz", "2014/11/20", "Get all versionList by moduleID")]
        public SqlDataReader GetVersionList(int moduleID)
        {
            var connection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_HtmlTextVersionList", connection);
            myCommand.Parameters.Add(new SqlParameter() { ParameterName = "@ModuleID", Value = moduleID, SqlDbType = SqlDbType.Int });
            myCommand.CommandType = CommandType.StoredProcedure;
            connection.Open();
            return myCommand.ExecuteReader(CommandBehavior.CloseConnection);
        }


        /// <summary>
        /// Method is used to get all version of given moduleId
        /// </summary>
        /// <param name="moduleID">ModuleID</param>
        /// <returns>List of versions</returns>
        [History("Ashish.patel@haptix.biz", "2014/11/20", "Get all version history by moduleID")]
        public DataSet GetVersionHistory(int moduleID)
        {
            var connection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_GetHtmlTextVersionHistory", connection);
            myCommand.Parameters.Add(new SqlParameter() { ParameterName = "@ModuleID", Value = moduleID, SqlDbType = SqlDbType.Int });
            myCommand.CommandType = CommandType.StoredProcedure;
            connection.Open();
            SqlDataAdapter dtAdpter = new SqlDataAdapter(myCommand);
            DataSet dtSet = new DataSet();
            dtAdpter.Fill(dtSet);
            return dtSet;
        }

        /// <summary>
        /// Method is used to get records for selected module
        /// </summary>
        /// <param name="moduleID">ModuleID</param>
        /// <returns>List of versions</returns>
        [History("Ashish.patel@haptix.biz", "2014/11/20", "Get all record")]
        public SqlDataReader GetHtmlTextRecord(int moduleID)
        {
            var connection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("select * from rb_HtmlText_st where ModuleID=" + moduleID, connection);
            myCommand.CommandType = CommandType.Text;
            connection.Open();
            return myCommand.ExecuteReader(CommandBehavior.CloseConnection);
        }


        #endregion
    }
}