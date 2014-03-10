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
            return this.GetHtmlText(moduleId, WorkFlowVersion.Production);

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
        public SqlDataReader GetHtmlText(int moduleId, WorkFlowVersion version)
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

            // End Change Geert.Audenaert@Syntegra.Com

            // Execute the command
            connection.Open();
            var result = command.ExecuteReader(CommandBehavior.CloseConnection);

            // Return the datareader 
            return result;
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
        public void UpdateHtmlText(int moduleId, string desktopHtml, string mobileSummary, string mobileDetails)
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

                var parameterMobileSummary = new SqlParameter("@MobileSummary", SqlDbType.NText)
                    { Value = mobileSummary };
                command.Parameters.Add(parameterMobileSummary);

                var parameterMobileDetails = new SqlParameter("@MobileDetails", SqlDbType.NText)
                    { Value = mobileDetails };
                command.Parameters.Add(parameterMobileDetails);

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

        #endregion
    }
}