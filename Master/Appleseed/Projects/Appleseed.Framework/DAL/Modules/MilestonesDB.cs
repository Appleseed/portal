// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MilestonesDB.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   The milestones db.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Content.Data
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    using Appleseed.Framework.Settings;

    /// <summary>
    /// The milestones db.
    /// </summary>
    public class MilestonesDB
    {
        #region Public Methods

        /// <summary>
        /// Add Milestones
        /// </summary>
        /// <param name="itemId">
        /// The ItemID
        /// </param>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <param name="createdByUser">
        /// The created by user.
        /// </param>
        /// <param name="createdDate">
        /// The created date.
        /// </param>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="estCompleteDate">
        /// The estimated complete date.
        /// </param>
        /// <param name="status">
        /// The status.
        /// </param>
        /// <returns>
        /// The newly created ID
        /// </returns>
        public int AddMilestones(
            int itemId, 
            int moduleId, 
            string createdByUser, 
            DateTime createdDate, 
            string title, 
            DateTime estCompleteDate, 
            string status)
        {
            // Create Instance of Connection and Command Object
            var connection = Config.SqlConnectionString;
            var command = new SqlCommand("rb_AddMilestones", connection)
                {
                    // Mark the Command as a SPROC
                    CommandType = CommandType.StoredProcedure
                };

            // Add Parameters to SPROC
            var parameterItemId = new SqlParameter("@ItemID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            command.Parameters.Add(parameterItemId);

            var parameterModuleId = new SqlParameter("@ModuleID", SqlDbType.Int) { Value = moduleId };
            command.Parameters.Add(parameterModuleId);

            var parameterCreatedByUser = new SqlParameter("@CreatedByUser", SqlDbType.NVarChar, 100)
                {
                   Value = createdByUser 
                };
            command.Parameters.Add(parameterCreatedByUser);

            var parameterCreatedDate = new SqlParameter("@CreatedDate", SqlDbType.DateTime) { Value = createdDate };
            command.Parameters.Add(parameterCreatedDate);

            var parameterTitle = new SqlParameter("@Title", SqlDbType.NVarChar, 100) { Value = title };
            command.Parameters.Add(parameterTitle);

            var parameterEstCompleteDate = new SqlParameter("@EstCompleteDate", SqlDbType.DateTime)
                {
                   Value = estCompleteDate 
                };
            command.Parameters.Add(parameterEstCompleteDate);

            var parameterStatus = new SqlParameter("@Status", SqlDbType.NVarChar, 100) { Value = status };
            command.Parameters.Add(parameterStatus);

            // Execute the command
            connection.Open();
            
            // var result =
                command.ExecuteReader(CommandBehavior.CloseConnection);

            // Return the data reader
            return (int)parameterItemId.Value;
        }

        /// <summary>
        /// Delete Milestones
        /// </summary>
        /// <param name="itemId">
        /// The ItemID
        /// </param>
        public void DeleteMilestones(int itemId)
        {
            // Create Instance of Connection and Command Object
            var connection = Config.SqlConnectionString;
            var sqlCommand = new SqlCommand("rb_DeleteMilestones", connection)
                {
                    // Mark the Command as a SPROC
                    CommandType =
                        CommandType.StoredProcedure
                };

            // Add Parameters to SPROC
            var parameterItemId = new SqlParameter("@ItemID", SqlDbType.Int) { Value = itemId };
            sqlCommand.Parameters.Add(parameterItemId);

            // Execute the command
            connection.Open();
            try
            {
                sqlCommand.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Get Milestones
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <param name="version">
        /// The version.
        /// </param>
        /// <returns>
        /// A SqlDataReader
        /// </returns>
        /// <remarks>
        /// change by David.Verberckmoes@syntegra.com in order to support workflow
        ///   Date: 20030324
        /// </remarks>
        public SqlDataReader GetMilestones(int moduleId, WorkFlowVersion version)
        {
            // Create Instance of Connection and Command Object
            var connection = Config.SqlConnectionString;
            var command = new SqlCommand("rb_GetMilestones", connection)
                {
                    // Mark the Command as a SPROC
                    CommandType =
                        CommandType.StoredProcedure
                };

            // Add Parameters to SPROC
            var parameterModuleId = new SqlParameter("@ModuleID", SqlDbType.Int) { Value = moduleId };
            command.Parameters.Add(parameterModuleId);

            // Change by David.Verberckmoes@Syntegra.com on 20030324
            var parameterWorkflowVersion = new SqlParameter("@WorkflowVersion", SqlDbType.Int, 4)
                {
                   Value = (int)version 
                };
            command.Parameters.Add(parameterWorkflowVersion);

            // End Change David.Verberckmoes@Syntegra.com 
            // Execute the command
            connection.Open();
            var result = command.ExecuteReader(CommandBehavior.CloseConnection);

            // Return the data reader
            return result;
        }

        /// <summary>
        /// Get Single Milestones
        /// </summary>
        /// <param name="itemId">
        /// The ItemID
        /// </param>
        /// <param name="version">
        /// The version.
        /// </param>
        /// <returns>
        /// A SqlDataReader
        /// </returns>
        /// <remarks>
        /// change by David.Verberckmoes@syntegra.com in order to support workflow
        ///   Date: 20030324
        /// </remarks>
        public SqlDataReader GetSingleMilestones(int itemId, WorkFlowVersion version)
        {
            // Create Instance of Connection and Command Object
            var connection = Config.SqlConnectionString;
            var command = new SqlCommand("rb_GetSingleMilestones", connection)
                {
                    // Mark the Command as a SPROC
                    CommandType =
                        CommandType.StoredProcedure
                };

            // Add Parameters to SPROC
            var parameterItemId = new SqlParameter("@ItemID", SqlDbType.Int) { Value = itemId };
            command.Parameters.Add(parameterItemId);

            // Change by David.Verberckmoes@Syntegra.com on 20030324
            var parameterWorkflowVersion = new SqlParameter("@WorkflowVersion", SqlDbType.Int, 4)
                {
                   Value = (int)version 
                };
            command.Parameters.Add(parameterWorkflowVersion);

            // End Change David.Verberckmoes@Syntegra.com 
            // Execute the command
            connection.Open();
            var result = command.ExecuteReader(CommandBehavior.CloseConnection);

            // Return the data reader
            return result;
        }

        /// <summary>
        /// Update Milestones
        /// </summary>
        /// <param name="itemId">
        /// The ItemID
        /// </param>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <param name="createdByUser">
        /// The created by user.
        /// </param>
        /// <param name="createdDate">
        /// The created date.
        /// </param>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="estCompleteDate">
        /// The estimated complete date.
        /// </param>
        /// <param name="status">
        /// The status.
        /// </param>
        public void UpdateMilestones(
            int itemId, 
            int moduleId, 
            string createdByUser, 
            DateTime createdDate, 
            string title, 
            DateTime estCompleteDate, 
            string status)
        {
            // Create Instance of Connection and Command Object
            var connection = Config.SqlConnectionString;
            var command = new SqlCommand("rb_UpdateMilestones", connection)
                {
                    // Mark the Command as a SPROC
                    CommandType =
                        CommandType.StoredProcedure
                };

            // Update Parameters to SPROC
            var parameterItemId = new SqlParameter("@ItemID", SqlDbType.Int) { Value = itemId };
            command.Parameters.Add(parameterItemId);

            var parameterModuleId = new SqlParameter("@ModuleID", SqlDbType.Int) { Value = moduleId };
            command.Parameters.Add(parameterModuleId);

            var parameterCreatedByUser = new SqlParameter("@CreatedByUser", SqlDbType.NVarChar, 100)
                {
                   Value = createdByUser 
                };
            command.Parameters.Add(parameterCreatedByUser);

            var parameterCreatedDate = new SqlParameter("@CreatedDate", SqlDbType.DateTime) { Value = createdDate };
            command.Parameters.Add(parameterCreatedDate);

            var parameterTitle = new SqlParameter("@Title", SqlDbType.NVarChar, 100) { Value = title };
            command.Parameters.Add(parameterTitle);

            var parameterEstCompleteDate = new SqlParameter("@EstCompleteDate", SqlDbType.DateTime)
                {
                   Value = estCompleteDate 
                };
            command.Parameters.Add(parameterEstCompleteDate);

            var parameterStatus = new SqlParameter("@Status", SqlDbType.NVarChar, 100) { Value = status };
            command.Parameters.Add(parameterStatus);

            // Execute the command
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