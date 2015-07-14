namespace Appleseed.Framework.Site.Data
{
    using Appleseed.Framework.Settings;
    using System.Data;
    using System.Data.SqlClient;

    /// <summary>
    /// Evolutility module DB
    /// </summary>
    public class EvolutilityModuleDB
    {
        /// <summary>
        /// This method is used to get data from Evolutility Wizazrd model
        /// </summary>
        /// <returns></returns>
        public SqlDataReader GetEvolutilyModuleList()
        {
            SqlConnection myConnection = Config.EvolutilitySqlConnectionString;
            SqlCommand myCommand = new SqlCommand("getModuleList", myConnection);

            myCommand.CommandType = CommandType.StoredProcedure;

            myConnection.Open();
            return myCommand.ExecuteReader(CommandBehavior.CloseConnection);
        }


        /// <summary>
        /// This method is used to get data for Evolutility Advanced model
        /// </summary>
        /// <returns></returns>
        public SqlDataReader EvolitityAdvedGetModelData(int moduleID)
        {
            SqlConnection myConnection = Config.EvolutilitySqlConnectionString;
            SqlCommand myCommand = new SqlCommand("EvolitityAdved_GetModelData", myConnection);
            myCommand.Parameters.Add(new SqlParameter() { ParameterName = "@ModuleID", Value = moduleID, SqlDbType = SqlDbType.Int });
            myCommand.CommandType = CommandType.StoredProcedure;
            myConnection.Open();
            return myCommand.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// This method is used to Save and Update data for Evolutility Advanced Model.
        /// </summary>
        /// <param name="moduleID">Pass module id</param>
        /// <param name="modelData">Pass data</param>
        public void EvolitityAdvedUpdateModelData(int moduleID, string modelData)
        {
            SqlConnection myConnection = Config.EvolutilitySqlConnectionString;
            SqlCommand myCommand = new SqlCommand("EvolitityAdved_UpdateModelData", myConnection);
            myCommand.Parameters.Add(new SqlParameter() { ParameterName = "@ModuleID", Value = moduleID, SqlDbType = SqlDbType.Int });
            myCommand.Parameters.Add(new SqlParameter() { ParameterName = "@ModelData", Value = modelData, SqlDbType = SqlDbType.NText });
            myCommand.CommandType = CommandType.StoredProcedure;
            myConnection.Open();
            myCommand.ExecuteNonQuery();
        }

        /// <summary>
        /// This method is used to get Settings for Evolutility Advanced model
        /// </summary>
        /// <returns></returns>
        public SqlDataReader EvolutilityAdvGetModelSettings(int moduleID)
        {
            SqlConnection myConnection = Config.EvolutilitySqlConnectionString;
            SqlCommand myCommand = new SqlCommand("EvolutilityAdv_GetModelSettings", myConnection);
            myCommand.Parameters.Add(new SqlParameter() { ParameterName = "@ModuleID", Value = moduleID, SqlDbType = SqlDbType.Int });
            myCommand.CommandType = CommandType.StoredProcedure;
            myConnection.Open();
            return myCommand.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// This method is used to Save and Update Settings for Evolutility Advanced Model
        /// </summary>
        /// <param name="ModuleID">Module ID</param>
        /// <param name="ModelID">Model ID</param>
        /// <param name="ModelLabel">Model label</param>
        /// <param name="ModelEntity">Model Entity</param>
        /// <param name="ModelEntities">Model entities</param>
        /// <param name="ModelLeadField">Model lead field</param>
        /// <param name="ModelElements">Model elements</param>
        public void EvolutilityAdvUpdateModelSettings(int ModuleID, string ModelID, string ModelLabel, string ModelEntity, string ModelEntities, string ModelLeadField, string ModelElements)
        {
            SqlConnection myConnection = Config.EvolutilitySqlConnectionString;
            SqlCommand myCommand = new SqlCommand("EvolutilityAdv_UpdateModelSettings", myConnection);
            myCommand.Parameters.Add(new SqlParameter() { ParameterName = "@ModuleID", Value = ModuleID, SqlDbType = SqlDbType.Int });
            myCommand.Parameters.Add(new SqlParameter() { ParameterName = "@ModelID", Value = ModelID, SqlDbType = SqlDbType.NVarChar, Size = 100 });
            myCommand.Parameters.Add(new SqlParameter() { ParameterName = "@ModelLabel", Value = ModelLabel, SqlDbType = SqlDbType.NVarChar, Size = 100 });
            myCommand.Parameters.Add(new SqlParameter() { ParameterName = "@ModelEntity", Value = ModelEntity, SqlDbType = SqlDbType.NVarChar, Size = 100 });
            myCommand.Parameters.Add(new SqlParameter() { ParameterName = "@ModelEntities", Value = ModelEntities, SqlDbType = SqlDbType.NVarChar, Size = 100 });
            myCommand.Parameters.Add(new SqlParameter() { ParameterName = "@ModelLeadField", Value = ModelLeadField, SqlDbType = SqlDbType.NVarChar, Size = 800 });
            myCommand.Parameters.Add(new SqlParameter() { ParameterName = "@ModelElements", Value = ModelElements, SqlDbType = SqlDbType.NText });
            myCommand.CommandType = CommandType.StoredProcedure;
            myConnection.Open();
            myCommand.ExecuteNonQuery();
        }

    }
}
