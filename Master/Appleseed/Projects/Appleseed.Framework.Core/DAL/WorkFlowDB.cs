namespace Appleseed.Framework.Site.Data
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    using Appleseed.Framework.Settings;

    /// <summary>
    /// Class that encapsulates all data logic necessary 
    ///     to publish a module using workflow
    /// </summary>
    public class WorkFlowDB
    {
        #region Public Methods

        /// <summary>
        /// This function puts the status of a module to approved
        /// </summary>
        /// <param name="moduleId">The module id.</param>
        public static void Approve(int moduleId)
        {
            // Create Instance of Connection and Command Object
            var connection = Config.SqlConnectionString;

            // Mark the Command as a SPROC
            var command = new SqlCommand("rb_Approve", connection) { CommandType = CommandType.StoredProcedure };
            
            // Add Parameters to SPROC
            var parameterModuleId = new SqlParameter("@moduleID", SqlDbType.Int, 4) { Value = moduleId };
            command.Parameters.Add(parameterModuleId);

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
        /// The get last modified.
        /// </summary>
        /// <param name="moduleId">
        /// The module id.
        /// </param>
        /// <param name="version">
        /// The version.
        /// </param>
        /// <param name="email">
        /// The email.
        /// </param>
        /// <param name="timestamp">
        /// The timestamp.
        /// </param>
        public static void GetLastModified(
            int moduleId, WorkFlowVersion version, ref string email, ref DateTime timestamp)
        {
            // Create Instance of Connection and Command Object
            var connection = Config.SqlConnectionString;

            // Mark the Command as a SPROC
            var command = new SqlCommand("rb_GetLastModified", connection) { CommandType = CommandType.StoredProcedure };

            // Add Parameters to SPROC
            var parameterModuleId = new SqlParameter("@ModuleID", SqlDbType.Int, 4) { Value = moduleId };
            command.Parameters.Add(parameterModuleId);

            var parameterWorkflowVersion = new SqlParameter("@WorkflowVersion", SqlDbType.Int, 4)
                {
                    Value = (int)version 
                };
            command.Parameters.Add(parameterWorkflowVersion);

            var parameterEmail = new SqlParameter("@LastModifiedBy", SqlDbType.NVarChar, 256)
                {
                    Direction = ParameterDirection.Output 
                };
            command.Parameters.Add(parameterEmail);

            var parameterDate = new SqlParameter("@LastModifiedDate", SqlDbType.DateTime, 8)
                {
                    Direction = ParameterDirection.Output 
                };
            command.Parameters.Add(parameterDate);

            connection.Open();
            try
            {
                command.ExecuteNonQuery();
                email = Convert.IsDBNull(parameterEmail.Value) ? string.Empty : (string)parameterEmail.Value;
                timestamp = Convert.IsDBNull(parameterDate.Value) ? DateTime.MinValue : (DateTime)parameterDate.Value;
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// This function publishes the staging data of a module.
        /// </summary>
        /// <param name="moduleId">The module id.</param>
        public static void Publish(int moduleId)
        {
            // Create Instance of Connection and Command Object
            var connection = Config.SqlConnectionString;

            // Mark the Command as a SPROC
            var command = new SqlCommand("rb_Publish", connection) { CommandType = CommandType.StoredProcedure };

            // Add Parameters to SPROC
            var parameterModuleId = new SqlParameter("@ModuleID", SqlDbType.Int, 4) { Value = moduleId };
            command.Parameters.Add(parameterModuleId);

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
        /// This function puts the status of a module back to working
        /// </summary>
        /// <param name="moduleId">The module ID.</param>
        public static void Reject(int moduleId)
        {
            // Create Instance of Connection and Command Object
            var connection = Config.SqlConnectionString;

            // Mark the Command as a SPROC
            var command = new SqlCommand("rb_Reject", connection) { CommandType = CommandType.StoredProcedure };
            
            // Add Parameters to SPROC
            var parameterModuleId = new SqlParameter("@moduleID", SqlDbType.Int, 4) { Value = moduleId };
            command.Parameters.Add(parameterModuleId);

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
        /// This function puts the status of a module to request approval
        /// </summary>
        /// <param name="moduleId">The module ID.</param>
        public static void RequestApproval(int moduleId)
        {
            // Create Instance of Connection and Command Object
            var connection = Config.SqlConnectionString;

            // Mark the Command as a SPROC
            var command = new SqlCommand("rb_RequestApproval", connection) { CommandType = CommandType.StoredProcedure };
            
            // Add Parameters to SPROC
            var parameterModuleId = new SqlParameter("@moduleID", SqlDbType.Int, 4) { Value = moduleId };
            command.Parameters.Add(parameterModuleId);

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
        /// This function reverts the staging data to the content in production of a module.
        /// </summary>
        /// <param name="moduleId">The module ID.</param>
        public static void Revert(int moduleId)
        {
            // Create Instance of Connection and Command Object
            var connection = Config.SqlConnectionString;

            // Mark the Command as a SPROC
            var command = new SqlCommand("rb_Revert", connection) { CommandType = CommandType.StoredProcedure };

            // Add Parameters to SPROC
            var parameterModuleId = new SqlParameter("@ModuleID", SqlDbType.Int, 4) { Value = moduleId };
            command.Parameters.Add(parameterModuleId);

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
        /// The set last modified.
        /// </summary>
        /// <param name="moduleId">
        /// The module id.
        /// </param>
        /// <param name="email">
        /// The email.
        /// </param>
        public static void SetLastModified(int moduleId, string email)
        {
            // Create Instance of Connection and Command Object
            var connection = Config.SqlConnectionString;

            // Mark the Command as a SPROC
            var command = new SqlCommand("rb_SetLastModified", connection) { CommandType = CommandType.StoredProcedure };
            
            // Add Parameters to SPROC
            var parameterModuleId = new SqlParameter("@ModuleID", SqlDbType.Int, 4) { Value = moduleId };
            command.Parameters.Add(parameterModuleId);

            var parameterEmail = new SqlParameter("@LastModifiedBy", SqlDbType.NVarChar, 256) { Value = email };
            command.Parameters.Add(parameterEmail);

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
