using System;
using System.Data;
using System.Data.SqlClient;
using Appleseed.Framework.Settings;

namespace Appleseed.Framework.Content.Data
{
    /// <summary>
    /// IBS Portal Picture module
    /// (c)2002 by Ender Malkoc
    /// </summary>
    public class PicturesDB
    {
        /// <summary>
        /// The AddPicture function is used to ADD Pictures to the Database
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        /// <param name="itemID">The item ID.</param>
        /// <param name="displayOrder">The display order.</param>
        /// <param name="metadataXml">The metadata XML.</param>
        /// <param name="shortDescription">The short description.</param>
        /// <param name="keywords">The keywords.</param>
        /// <param name="CreatedByUser">The created by user.</param>
        /// <param name="CreatedDate">The created date.</param>
        /// <returns></returns>
        public int AddPicture(int moduleID, int itemID, int displayOrder, string metadataXml, string shortDescription,
                              string keywords, string CreatedByUser, DateTime CreatedDate)
        {
            //  Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_AddPicture", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            //  Add Parameters to SPROC
            SqlParameter parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
            parameterItemID.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterItemID);

            SqlParameter parameterModuleID = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
            parameterModuleID.Value = moduleID;
            myCommand.Parameters.Add(parameterModuleID);

            SqlParameter parameterDisplayOrder = new SqlParameter("@DisplayOrder", SqlDbType.Int, 4);
            parameterDisplayOrder.Value = displayOrder;
            myCommand.Parameters.Add(parameterDisplayOrder);

            SqlParameter parameterMetadataXml = new SqlParameter("@MetadataXml", SqlDbType.NVarChar);
            parameterMetadataXml.Value = metadataXml;
            myCommand.Parameters.Add(parameterMetadataXml);

            SqlParameter parameterShortDescription = new SqlParameter("@ShortDescription", SqlDbType.NVarChar);
            parameterShortDescription.Value = shortDescription;
            myCommand.Parameters.Add(parameterShortDescription);

            SqlParameter parameterKeywords = new SqlParameter("@Keywords", SqlDbType.NVarChar);
            parameterKeywords.Value = keywords;
            myCommand.Parameters.Add(parameterKeywords);

            SqlParameter parameterCreatedByUser = new SqlParameter("@CreatedByUser", SqlDbType.NVarChar, 100);
            parameterCreatedByUser.Value = CreatedByUser;
            myCommand.Parameters.Add(parameterCreatedByUser);

            SqlParameter parameterCreatedDate = new SqlParameter("@CreatedDate", SqlDbType.DateTime);
            parameterCreatedDate.Value = CreatedDate;
            myCommand.Parameters.Add(parameterCreatedDate);

            myConnection.Open();
            try
            {
                myCommand.ExecuteNonQuery();
            }
            finally
            {
                myConnection.Close();
            }

            return Convert.ToInt32(parameterItemID.Value);
        }

        /// <summary>
        /// The GetPicture function is used to get all the Pictures in the module
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        /// <param name="page">The page.</param>
        /// <param name="recordsPerPage">The records per page.</param>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        public DataSet GetPicturesPaged(int moduleID, int page, int recordsPerPage, WorkFlowVersion version)
        {
            //  Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlDataAdapter myCommand = new SqlDataAdapter("rb_GetPicturesPaged", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            //  Add Parameters to SPROC
            SqlParameter parameterModuleID = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
            parameterModuleID.Value = moduleID;
            myCommand.SelectCommand.Parameters.Add(parameterModuleID);

            SqlParameter parameterPage = new SqlParameter("@Page", SqlDbType.Int, 4);
            parameterPage.Value = page;
            myCommand.SelectCommand.Parameters.Add(parameterPage);

            SqlParameter parameterRecordsPerPage = new SqlParameter("@RecordsPerPage", SqlDbType.Int, 4);
            parameterRecordsPerPage.Value = recordsPerPage;
            myCommand.SelectCommand.Parameters.Add(parameterRecordsPerPage);

            SqlParameter parameterWorkflowVersion = new SqlParameter("@WorkflowVersion", SqlDbType.Int, 4);
            parameterWorkflowVersion.Value = (int) version;
            myCommand.SelectCommand.Parameters.Add(parameterWorkflowVersion);

            //  Create and Fill the DataSet
            DataSet myDataSet = new DataSet();
            try
            {
                myCommand.Fill(myDataSet);
            }
            finally
            {
                myConnection.Close(); //by Manu fix close bug #2
            }

            //  return the DataSet
            return myDataSet;
        }

        /// <summary>
        /// The GetSinglePicture function is used to Get a single Picture
        /// from the database for display/edit
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        public SqlDataReader GetSinglePicture(int itemID, WorkFlowVersion version)
        {
            //  Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_GetSinglePicture", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            //  Add Parameters to SPROC
            SqlParameter parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
            parameterItemID.Value = itemID;
            myCommand.Parameters.Add(parameterItemID);

            SqlParameter parameterWorkflowVersion = new SqlParameter("@WorkflowVersion", SqlDbType.Int, 4);
            parameterWorkflowVersion.Value = (int) version;
            myCommand.Parameters.Add(parameterWorkflowVersion);

            //  Execute the command
            myConnection.Open();
            SqlDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            //  return the datareader 
            return result;
        }

        /// <summary>
        /// The DeletePicture function is used to remove Pictures from the Database
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        public void DeletePicture(int itemID)
        {
            //  Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_DeletePicture", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            //  Add Parameters to SPROC
            SqlParameter parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
            parameterItemID.Value = itemID;
            myCommand.Parameters.Add(parameterItemID);

            //  Execute the command
            myConnection.Open();
            try
            {
                myCommand.ExecuteNonQuery();
            }
            finally
            {
                myConnection.Close();
            }
        }

        /// <summary>
        /// The UpdatePicture function is used to update changes to the Pictures
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        /// <param name="itemID">The item ID.</param>
        /// <param name="displayOrder">The display order.</param>
        /// <param name="metadataXml">The metadata XML.</param>
        /// <param name="shortDescription">The short description.</param>
        /// <param name="keywords">The keywords.</param>
        /// <param name="CreatedByUser">The created by user.</param>
        /// <param name="CreatedDate">The created date.</param>
        public void UpdatePicture(int moduleID, int itemID, int displayOrder, string metadataXml,
                                  string shortDescription, string keywords, string CreatedByUser, DateTime CreatedDate)
        {
            //  Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_UpdatePicture", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            //  Add Parameters to SPROC
            SqlParameter parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
            parameterItemID.Value = itemID;
            myCommand.Parameters.Add(parameterItemID);

            SqlParameter parameterModuleID = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
            parameterModuleID.Value = moduleID;
            myCommand.Parameters.Add(parameterModuleID);

            SqlParameter parameterDisplayOrder = new SqlParameter("@DisplayOrder", SqlDbType.Int, 4);
            parameterDisplayOrder.Value = displayOrder;
            myCommand.Parameters.Add(parameterDisplayOrder);

            SqlParameter parameterMetadataXml = new SqlParameter("@MetadataXml", SqlDbType.NVarChar);
            parameterMetadataXml.Value = metadataXml;
            myCommand.Parameters.Add(parameterMetadataXml);

            SqlParameter parameterShortDescription = new SqlParameter("@ShortDescription", SqlDbType.NVarChar);
            parameterShortDescription.Value = shortDescription;
            myCommand.Parameters.Add(parameterShortDescription);

            SqlParameter parameterKeywords = new SqlParameter("@Keywords", SqlDbType.NVarChar);
            parameterKeywords.Value = keywords;
            myCommand.Parameters.Add(parameterKeywords);

            SqlParameter parameterCreatedByUser = new SqlParameter("@CreatedByUser", SqlDbType.NVarChar, 100);
            parameterCreatedByUser.Value = CreatedByUser;
            myCommand.Parameters.Add(parameterCreatedByUser);

            SqlParameter parameterCreatedDate = new SqlParameter("@CreatedDate", SqlDbType.DateTime);
            parameterCreatedDate.Value = CreatedDate;
            myCommand.Parameters.Add(parameterCreatedDate);

            //  Execute the command
            myConnection.Open();
            try
            {
                myCommand.ExecuteNonQuery();
            }
            finally
            {
                myConnection.Close();
            }
        }
    }
}