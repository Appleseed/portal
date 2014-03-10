// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArticlesDB.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   Class that encapsulates all data logic necessary
//   articles within the Portal database.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Content.Data
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    using Appleseed.Framework.Settings;

    /// <summary>
    /// Class that encapsulates all data logic necessary
    ///   articles within the Portal database.
    /// </summary>
    public class ArticlesDB
    {
        #region Constants and Fields

        /// <summary>
        ///   This is used as a common setting from Articles
        /// </summary>
        public static string ImagesSetting = "ImageCollection";

        #endregion

        #region Public Methods

        /// <summary>
        /// The AddArticle method adds a new Article within the
        ///   Articles database table, and returns ItemID value as a result.
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <param name="userName">
        /// Name of the user.
        /// </param>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="subtitle">
        /// The subtitle.
        /// </param>
        /// <param name="articleAbstract">
        /// The article abstract.
        /// </param>
        /// <param name="description">
        /// The description.
        /// </param>
        /// <param name="startDate">
        /// The start date.
        /// </param>
        /// <param name="expireDate">
        /// The expire date.
        /// </param>
        /// <param name="isinNewsletter">
        /// if set to <c>true</c> [is in newsletter].
        /// </param>
        /// <param name="moreLink">
        /// The more link.
        /// </param>
        /// <returns>
        /// The article id.
        /// </returns>
        public int AddArticle(
            int moduleId, 
            string userName, 
            string title, 
            string subtitle, 
            string articleAbstract, 
            string description, 
            DateTime startDate, 
            DateTime expireDate, 
            bool isinNewsletter, 
            string moreLink)
        {
            if (userName.Length < 1)
            {
                userName = "unknown";
            }

            // Create Instance of Connection and Command Object
            var connection = Config.SqlConnectionString;
            var command = new SqlCommand("rb_AddArticle", connection)
                {
                    // Mark the Command as a SPROC
                    CommandType = CommandType.StoredProcedure
                };

            // Add Parameters to SPROC
            var parameterItemId = new SqlParameter("@ItemID", SqlDbType.Int, 4)
                {
                   Direction = ParameterDirection.Output 
                };
            command.Parameters.Add(parameterItemId);

            var parameterModuleId = new SqlParameter("@ModuleID", SqlDbType.Int, 4) { Value = moduleId };
            command.Parameters.Add(parameterModuleId);

            var parameterUserName = new SqlParameter("@UserName", SqlDbType.NVarChar, 100) { Value = userName };
            command.Parameters.Add(parameterUserName);

            var parameterTitle = new SqlParameter("@Title", SqlDbType.NVarChar, 100) { Value = title };
            command.Parameters.Add(parameterTitle);

            var parameterSubtitle = new SqlParameter("@Subtitle", SqlDbType.NVarChar, 200) { Value = subtitle };
            command.Parameters.Add(parameterSubtitle);

            var parameterAbstract = new SqlParameter("@Abstract", SqlDbType.NText) { Value = articleAbstract };
            command.Parameters.Add(parameterAbstract);

            var parameterDescription = new SqlParameter("@Description", SqlDbType.NText) { Value = description };
            command.Parameters.Add(parameterDescription);

            var parameterStartDate = new SqlParameter("@StartDate", SqlDbType.DateTime) { Value = startDate };
            command.Parameters.Add(parameterStartDate);

            var parameterExpireDate = new SqlParameter("@ExpireDate", SqlDbType.DateTime) { Value = expireDate };
            command.Parameters.Add(parameterExpireDate);

            var parameterIsInNewsletter = new SqlParameter("@IsInNewsletter", SqlDbType.Bit) { Value = isinNewsletter };
            command.Parameters.Add(parameterIsInNewsletter);

            var parameterMoreLink = new SqlParameter("@MoreLink", SqlDbType.NVarChar, 150) { Value = moreLink };
            command.Parameters.Add(parameterMoreLink);

            connection.Open();
            try
            {
                command.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }

            return (int)parameterItemId.Value;
        }

        /// <summary>
        /// The DeleteArticle method deletes a specified Article from
        ///   the Articles database table.
        /// </summary>
        /// <param name="itemId">
        /// The item ID.
        /// </param>
        public void DeleteArticle(int itemId)
        {
            // Create Instance of Connection and Command Object
            var connection = Config.SqlConnectionString;
            var command = new SqlCommand("rb_DeleteArticle", connection)
                {
                    // Mark the Command as a SPROC
                    CommandType = CommandType.StoredProcedure
                };

            // Add Parameters to SPROC
            var parameterItemId = new SqlParameter("@ItemID", SqlDbType.Int, 4) { Value = itemId };
            command.Parameters.Add(parameterItemId);

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

        /// <summary>
        /// The GetArticles method returns a SqlDataReader containing all of the
        ///   Articles for a specific portal module from the announcements
        ///   database.
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <param name="version">
        /// The version.
        /// </param>
        /// <returns>
        /// A SQL data reader.
        /// </returns>
        public SqlDataReader GetArticles(int moduleId, WorkFlowVersion version)
        {
            // Create Instance of Connection and Command Object
            var connection = Config.SqlConnectionString;
            var command = new SqlCommand("rb_GetArticles", connection) { CommandType = CommandType.StoredProcedure };

            // Mark the Command as a SPROC

            // Add Parameters to SPROC
            var parameterModuleId = new SqlParameter("@ModuleID", SqlDbType.Int, 4) { Value = moduleId };
            command.Parameters.Add(parameterModuleId);

            var parameterWorkflowVersion = new SqlParameter("@WorkflowVersion", SqlDbType.Int, 4)
                {
                    Value = (int)version 
                };
            command.Parameters.Add(parameterWorkflowVersion);

            // Execute the command
            connection.Open();
            var result = command.ExecuteReader(CommandBehavior.CloseConnection);

            // Return the datareader 
            return result;
        }

        /// <summary>
        /// The GetArticlesAll method returns a SqlDataReader containing all of the
        ///   Articles for a specific portal module from the announcements
        ///   database (including expired one).
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <param name="version">
        /// The version.
        /// </param>
        /// <returns>
        /// A SQL data reader.
        /// </returns>
        public SqlDataReader GetArticlesAll(int moduleId, WorkFlowVersion version)
        {
            // Create Instance of Connection and Command Object
            var connection = Config.SqlConnectionString;
            var command = new SqlCommand("rb_GetArticlesAll", connection)
                {
                    // Mark the Command as a SPROC
                    CommandType = CommandType.StoredProcedure
                };

            // Add Parameters to SPROC
            var parameterModuleId = new SqlParameter("@ModuleID", SqlDbType.Int, 4) { Value = moduleId };
            command.Parameters.Add(parameterModuleId);

            var parameterWorkflowVersion = new SqlParameter("@WorkflowVersion", SqlDbType.Int, 4)
                {
                   Value = (int)version 
                };
            command.Parameters.Add(parameterWorkflowVersion);

            // Execute the command
            connection.Open();
            var result = command.ExecuteReader(CommandBehavior.CloseConnection);

            // Return the data reader 
            return result;
        }

        /// <summary>
        /// The GetSingleArticle method returns a SqlDataReader containing details
        ///   about a specific Article from the Articles database table.
        /// </summary>
        /// <param name="itemId">
        /// The item ID.
        /// </param>
        /// <param name="version">
        /// The version.
        /// </param>
        /// <returns>
        /// A SQL data reader.
        /// </returns>
        public SqlDataReader GetSingleArticle(int itemId, WorkFlowVersion version)
        {
            // Create Instance of Connection and Command Object
            var connection = Config.SqlConnectionString;
            var command = new SqlCommand("rb_GetSingleArticle", connection)
                {
                    // Mark the Command as a SPROC
                    CommandType =
                        CommandType.StoredProcedure
                };

            // Add Parameters to SPROC
            var parameterItemId = new SqlParameter("@ItemID", SqlDbType.Int, 4) { Value = itemId };
            command.Parameters.Add(parameterItemId);

            var parameterWorkflowVersion = new SqlParameter("@WorkflowVersion", SqlDbType.Int, 4)
                {
                   Value = (int)version 
                };
            command.Parameters.Add(parameterWorkflowVersion);

            // Execute the command
            connection.Open();
            var result = command.ExecuteReader(CommandBehavior.CloseConnection);

            // Return the data reader 
            return result;
        }

        /// <summary>
        /// The UpdateArticle method updates a specified Article within
        ///   the Articles database table.
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <param name="itemId">
        /// The item ID.
        /// </param>
        /// <param name="userName">
        /// Name of the user.
        /// </param>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="subtitle">
        /// The subtitle.
        /// </param>
        /// <param name="articleAbstract">
        /// The article abstract.
        /// </param>
        /// <param name="description">
        /// The description.
        /// </param>
        /// <param name="startDate">
        /// The start date.
        /// </param>
        /// <param name="expireDate">
        /// The expire date.
        /// </param>
        /// <param name="isinNewsletter">
        /// if set to <c>true</c> [is in newsletter].
        /// </param>
        /// <param name="moreLink">
        /// The more link.
        /// </param>
        public void UpdateArticle(
            int moduleId, 
            int itemId, 
            string userName, 
            string title, 
            string subtitle, 
            string articleAbstract, 
            string description, 
            DateTime startDate, 
            DateTime expireDate, 
            bool isinNewsletter, 
            string moreLink)
        {
            if (userName.Length < 1)
            {
                userName = "unknown";
            }

            // Create Instance of Connection and Command Object
            var connection = Config.SqlConnectionString;
            var command = new SqlCommand("rb_UpdateArticle", connection)
                {
                    // Mark the Command as a SPROC
                    CommandType = CommandType.StoredProcedure
                };

            // Add Parameters to SPROC
            var parameterItemId = new SqlParameter("@ItemID", SqlDbType.Int, 4) { Value = itemId };
            command.Parameters.Add(parameterItemId);

            var parameterModuleId = new SqlParameter("@ModuleID", SqlDbType.Int, 4) { Value = moduleId };
            command.Parameters.Add(parameterModuleId);

            var parameterUserName = new SqlParameter("@UserName", SqlDbType.NVarChar, 100) { Value = userName };
            command.Parameters.Add(parameterUserName);

            var parameterTitle = new SqlParameter("@Title", SqlDbType.NVarChar, 100) { Value = title };
            command.Parameters.Add(parameterTitle);

            var parameterSubtitle = new SqlParameter("@Subtitle", SqlDbType.NVarChar, 200) { Value = subtitle };
            command.Parameters.Add(parameterSubtitle);

            var parameterAbstract = new SqlParameter("@Abstract", SqlDbType.NText) { Value = articleAbstract };
            command.Parameters.Add(parameterAbstract);

            var parameterDescription = new SqlParameter("@Description", SqlDbType.NText) { Value = description };
            command.Parameters.Add(parameterDescription);

            var parameterStartDate = new SqlParameter("@StartDate", SqlDbType.DateTime) { Value = startDate };
            command.Parameters.Add(parameterStartDate);

            var parameterExpireDate = new SqlParameter("@ExpireDate", SqlDbType.DateTime) { Value = expireDate };
            command.Parameters.Add(parameterExpireDate);

            var parameterIsInNewsletter = new SqlParameter("@IsInNewsletter", SqlDbType.Bit) { Value = isinNewsletter };
            command.Parameters.Add(parameterIsInNewsletter);

            var parameterMoreLink = new SqlParameter("@MoreLink", SqlDbType.NVarChar, 150) { Value = moreLink };
            command.Parameters.Add(parameterMoreLink);

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

        #endregion
    }
}