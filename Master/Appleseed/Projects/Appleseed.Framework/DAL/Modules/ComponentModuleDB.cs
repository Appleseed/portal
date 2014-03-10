// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComponentModuleDB.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   The component module db.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Content.Data
{
    using System.Data;
    using System.Data.SqlClient;

    using Appleseed.Framework.Settings;

    /// <summary>
    /// The component module db.
    /// </summary>
    public class ComponentModuleDB
    {
        #region Public Methods

        /// <summary>
        /// Get Component Module
        /// </summary>
        /// <param name="moduleId">
        /// The ModuleID
        /// </param>
        /// <returns>
        /// A SqlDataReader
        /// </returns>
        public SqlDataReader GetComponentModule(int moduleId)
        {
            // Create Instance of Connection and Command Object
            var connection = Config.SqlConnectionString;
            var sqlCommand = new SqlCommand("rb_GetComponentModule", connection)
                {
                    CommandType = CommandType.StoredProcedure 
                };

            // Add Parameters to SPROC
            var parameterModuleId = new SqlParameter("@ModuleID", SqlDbType.Int) { Value = moduleId };
            sqlCommand.Parameters.Add(parameterModuleId);

            // Execute the command
            connection.Open();
            var result = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);

            // Return the data reader
            return result;
        }

        /// <summary>
        /// Update Component Module
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <param name="createdByUser">
        /// The created by user.
        /// </param>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="component">
        /// The component (void).
        /// </param>
        public void UpdateComponentModule(int moduleId, string createdByUser, string title, string component)
        {
            // Create Instance of Connection and Command Object
            var connection = Config.SqlConnectionString;
            var sqlCommand = new SqlCommand("rb_UpdateComponentModule", connection)
                {
                   CommandType = CommandType.StoredProcedure 
                };

            // Update Parameters to SPROC
            var parameterModuleId = new SqlParameter("@ModuleID", SqlDbType.Int) { Value = moduleId };
            sqlCommand.Parameters.Add(parameterModuleId);

            var parameterCreatedByUser = new SqlParameter("@CreatedByUser", SqlDbType.NVarChar, 100)
                {
                   Value = createdByUser 
                };
            sqlCommand.Parameters.Add(parameterCreatedByUser);

            var parameterTitle = new SqlParameter("@Title", SqlDbType.NVarChar, 100) { Value = title };
            sqlCommand.Parameters.Add(parameterTitle);

            var parameterComponent = new SqlParameter("@Component", SqlDbType.NVarChar, -1) { Value = component };
            sqlCommand.Parameters.Add(parameterComponent);

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

        #endregion
    }
}