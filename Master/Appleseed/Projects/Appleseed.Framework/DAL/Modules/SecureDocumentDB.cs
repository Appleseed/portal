using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Appleseed.Framework;
using Appleseed.Framework.Data;
using Appleseed.Framework.Security;
using Appleseed.Framework.Settings;

namespace Appleseed.Framework.Content.Data
{
	/// <summary>
	/// Class that encapsulates all data logic necessary to add/query/delete
	/// documents within the Portal database.
	/// </summary>
	[History("José Viladiu", "2004/07/02", "Generate Documentation in correct format")]
    public class SecureDocumentDB 
    {
        /// <summary>
        /// The GetDocuments method returns a DataSet containing all of the
        /// documents for a specific portal module from the documents datatable.
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public DataSet GetDocuments(int moduleID, int userId) 
        {
			return GetDocuments(moduleID, WorkFlowVersion.Production, userId);
		}


        /// <summary>
        /// The GetDocuments method returns a DataSet containing all of the
        /// documents for a specific portal module from the documents datatable.
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        /// <param name="version">The version.</param>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public DataSet GetDocuments(int moduleID, WorkFlowVersion version, int userId) 
        {
            // Create Instance of Connection and Command Object
			SqlConnection myConnection = Config.SqlConnectionString;
			SqlDataAdapter myCommand = new SqlDataAdapter("rb_GetSecureDocuments", myConnection);

			// Mark the Command as a SPROC
			myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterModuleID = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
            parameterModuleID.Value = moduleID;
            myCommand.SelectCommand.Parameters.Add(parameterModuleID);

			// Change by Geert.Audenaert@Syntegra.Com
			// Date: 7/2/2003
			SqlParameter parameterWorkflowVersion = new SqlParameter("@WorkflowVersion", SqlDbType.Int, 4);
			parameterWorkflowVersion.Value = (int)version;
			myCommand.SelectCommand.Parameters.Add(parameterWorkflowVersion);
			// End Change Geert.Audenaert@Syntegra.Com

			// pass the user id
			SqlParameter parameteruserid = new SqlParameter("@userId", SqlDbType.Int, 4);
			parameteruserid.Value = userId;
			myCommand.SelectCommand.Parameters.Add(parameteruserid);


			// Create and Fill the DataSet
			DataSet myDataSet = new DataSet();
			try
			{
				myCommand.Fill(myDataSet);
			}
			finally
			{
				myConnection.Close(); //by Manu fix close bug #2
			}

			// Return the DataSet
			return myDataSet;
		}

        /// <summary>
        /// Get the files associated with this document
        /// </summary>
        /// <param name="documentId">The document id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
		public static SqlDataReader GetDocumentFiles(int documentId, int userId) 
		{
			return GetDocumentFiles(documentId, WorkFlowVersion.Production);
		}


        /// <summary>
        /// The GetDocuments method returns a DataSet containing all of the
        /// documents for a specific portal module from the documents datatable.
        /// </summary>
        /// <param name="documentId">The document id.</param>
        /// <param name="version">The version.</param>
        /// <returns>The required data reader</returns>
		public static SqlDataReader GetDocumentFiles(int documentId, WorkFlowVersion version) 
		{
			// Create Instance of Connection and Command Object
			SqlConnection myConnection = Config.SqlConnectionString;
			SqlCommand myCommand = new SqlCommand("rb_GetSecureDocumentFiles", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Add Parameters to SPROC
			SqlParameter parameterDocumentID = new SqlParameter("@DocumentID", SqlDbType.Int, 4);
			parameterDocumentID.Value = documentId;
			myCommand.Parameters.Add(parameterDocumentID);

			// Change by Geert.Audenaert@Syntegra.Com
			// Date: 7/2/2003
			SqlParameter parameterWorkflowVersion = new SqlParameter("@WorkflowVersion", SqlDbType.Int, 4);
			parameterWorkflowVersion.Value = (int)version;
			myCommand.Parameters.Add(parameterWorkflowVersion);
			// End Change Geert.Audenaert@Syntegra.Com

			myConnection.Open();
			SqlDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

			// Return the reader
			return result;
		}

        /// <summary>
        /// Get list of roles which can access the document with given id.
        /// </summary>
        /// <param name="itemId">The item id.</param>
        /// <returns></returns>
		public static string GetDocumentRoles(int itemId) 
		{
			if( itemId == 0 )
				return string.Empty;

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = Config.SqlConnectionString;
			SqlCommand myCommand = new SqlCommand("rb_GetSingleSecureDocumentRoles", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Add Parameters to SPROC
			SqlParameter parameteritemId = new SqlParameter("@itemId", SqlDbType.Int, 4);
			parameteritemId.Value = itemId;
			myCommand.Parameters.Add(parameteritemId);

			// Execute the command
			myConnection.Open();
			SqlDataReader drGetRoles = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
			string result = string.Empty;
			try
			{
				while( drGetRoles.Read() )
				{
					// if the role name is null, its a special case
					// so we need to do a look up on it to get its name
					if( drGetRoles["RoleName"] == DBNull.Value  )
						result += SpecialRoles.GetRoleName(int.Parse(drGetRoles["GroupID"].ToString())) + ";";
					
						// otherwise use the role name from the table
					else
						result += drGetRoles["RoleName"].ToString() + ";";
				}
			}
			finally
			{
				drGetRoles.Close();
				myConnection.Close();
			}

			// Return the datareader 
			return result;
		}


        /// <summary>
        /// Returns a SqlDataReader containing details about a
        /// specific document from the Documents database table.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <returns>The SqlDataReader</returns>
        public SqlDataReader GetSingleDocument(int itemID) 
        {
			return GetSingleDocument(itemID, WorkFlowVersion.Production);
		}

        /// <summary>
        /// Returns a SqlDataReader containing details about a
        /// specific document from the Documents database table.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <param name="version">The version.</param>
        /// <returns>The SqlDataReader</returns>
        public SqlDataReader GetSingleDocument(int itemID, WorkFlowVersion version) 
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_GetSingleSecureDocument", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
            parameterItemID.Value = itemID;
            myCommand.Parameters.Add(parameterItemID);

			// Change by Geert.Audenaert@Syntegra.Com
			// Date: 7/2/2003
			SqlParameter parameterWorkflowVersion = new SqlParameter("@WorkflowVersion", SqlDbType.Int, 4);
			parameterWorkflowVersion.Value = (int)version;
			myCommand.Parameters.Add(parameterWorkflowVersion);
			// End Change Geert.Audenaert@Syntegra.Com
			
            // Execute the command
            myConnection.Open();
            SqlDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
            
            // Return the datareader 
            return result;
        }

        /// <summary>
        /// Return the specified document content from datatable
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <returns>The SqlDataReader</returns>
        public SqlDataReader GetDocumentContent(int itemID) 
        {
			return GetDocumentContent(itemID, WorkFlowVersion.Production);
		}

        /// <summary>
        /// Return the specified document content from datatable
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <param name="version">The version.</param>
        /// <returns>The SqlDataReader</returns>
		public SqlDataReader GetDocumentContent(int itemID, WorkFlowVersion version) 
		{
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_GetSecureDocumentContent", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
            parameterItemID.Value = itemID;
            myCommand.Parameters.Add(parameterItemID);

			// Change by Geert.Audenaert@Syntegra.Com
			// Date: 7/2/2003
			SqlParameter parameterWorkflowVersion = new SqlParameter("@WorkflowVersion", SqlDbType.Int, 4);
			parameterWorkflowVersion.Value = (int)version;
			myCommand.Parameters.Add(parameterWorkflowVersion);
			// End Change Geert.Audenaert@Syntegra.Com
			
            // Execute the command
            myConnection.Open();
            SqlDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
            
            // Return the datareader 
            return result;
        }

        /// <summary>
        /// Deletes the specified document from the Documents database table
        /// and delete if exists the phisycal file from disk
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <param name="fileName">Name of the file.</param>
        public void DeleteDocument(int itemID, string fileName)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;

            // Delete from DB
            SqlCommand myCommand = new SqlCommand("rb_DeleteSecureDocument", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
            parameterItemID.Value = itemID;
            myCommand.Parameters.Add(parameterItemID);

            //Delete DB row
            myConnection.Open();
			try
			{
				myCommand.ExecuteNonQuery();
			}
			finally
			{
				myConnection.Close();
			}


		// Update this so that all the files are deleted!!!
		
			// Change by José Viladiu (02/07/2004). Delete phisical file
			// after sql delete if no errors in sql operation
			try
			{
				//File.Delete(@fileName);
			}
			catch
			{
				//TODO: Catch the error
			}
        }

        /// <summary>
        /// Insert or update the specified document within the Documents database table.
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        /// <param name="itemID">The item ID.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="name">The name.</param>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        public int UpdateDocument(int moduleID, int itemID, string userName, string name, string category) 
        {
            if (userName.Length < 1) 
            {
				userName = "unknown";
            }
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_UpdateSecureDocument", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
            parameterItemID.Value = itemID;
            myCommand.Parameters.Add(parameterItemID);

            SqlParameter parameterModuleID = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
            parameterModuleID.Value = moduleID;
            myCommand.Parameters.Add(parameterModuleID);

            SqlParameter parameterUserName = new SqlParameter("@UserName", SqlDbType.NVarChar, 100);
            parameterUserName.Value = userName;
            myCommand.Parameters.Add(parameterUserName);

            SqlParameter parameterName = new SqlParameter("@FileFriendlyName", SqlDbType.NVarChar, 150);
            parameterName.Value = name;
            myCommand.Parameters.Add(parameterName);

			SqlParameter parameterCategory = new SqlParameter("@Category", SqlDbType.NVarChar, 50);
            parameterCategory.Value = category;
            myCommand.Parameters.Add(parameterCategory);

			// id of the created record
			SqlParameter parameterRecordId = new SqlParameter("@recordId", SqlDbType.Int, 4);
			parameterRecordId.Direction = ParameterDirection.Output;
			myCommand.Parameters.Add(parameterRecordId);

            myConnection.Open();
			try
			{
				myCommand.ExecuteNonQuery();
			}
			finally
			{
				myConnection.Close();
			}

			return int.Parse(parameterRecordId.Value.ToString());
		}

        /// <summary>
        /// Add a file to the document
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        /// <param name="documentId">The document id.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="url">The URL.</param>
        /// <param name="content">The content.</param>
        /// <param name="size">The size.</param>
        /// <param name="contentType">Type of the content.</param>
		public void AddDocumentFile(int moduleID, int documentId, string filename, string url, byte[] content, int size, string contentType) 
		{
			// if there is no filename or url, bail
			if( filename.Trim() == string.Empty && url.Trim() == string.Empty )
				return;

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = Config.SqlConnectionString;
			SqlCommand myCommand = new SqlCommand("rb_UpdateSecureDocumentFile", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Add Parameters to SPROC
			SqlParameter parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
			parameterItemID.Value = 0;
			myCommand.Parameters.Add(parameterItemID);

			SqlParameter parameterDocumentId = new SqlParameter("@DocumentID", SqlDbType.Int, 4);
			parameterDocumentId.Value = documentId;
			myCommand.Parameters.Add(parameterDocumentId);

			SqlParameter parameterModuleID = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
			parameterModuleID.Value = moduleID;
			myCommand.Parameters.Add(parameterModuleID);

			SqlParameter parameterFileName = new SqlParameter("@FileName", SqlDbType.NVarChar, 250);
			parameterFileName.Value = filename;
			myCommand.Parameters.Add(parameterFileName);

			SqlParameter parameterFileUrl = new SqlParameter("@FileNameUrl", SqlDbType.NVarChar, 250);
			parameterFileUrl.Value = url;
			myCommand.Parameters.Add(parameterFileUrl);

			SqlParameter parameterContent = new SqlParameter("@Content", SqlDbType.Image);
			parameterContent.Value = content;
			myCommand.Parameters.Add(parameterContent);

			SqlParameter parameterContentType = new SqlParameter("@ContentType", SqlDbType.NVarChar, 50);
			parameterContentType.Value = contentType;
			myCommand.Parameters.Add(parameterContentType);

			SqlParameter parameterContentSize = new SqlParameter("@ContentSize", SqlDbType.Int, 4);
			parameterContentSize.Value = size;
			myCommand.Parameters.Add(parameterContentSize);

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
        /// Add access for the group to view the document
        /// </summary>
        /// <param name="itemId">The item id.</param>
        /// <param name="groupId">The group id.</param>
		public static void AddDocumentAccess( int itemId, int groupId) 
		{

			if( itemId < 1 )
				return;

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = Config.SqlConnectionString;
			SqlCommand myCommand = new SqlCommand("rb_AddSecureDocumentAccess", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Add Parameters to SPROC
			SqlParameter parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
			parameterItemID.Value = itemId;
			myCommand.Parameters.Add(parameterItemID);

			SqlParameter parameterGroupId = new SqlParameter("@groupId", SqlDbType.Int, 4);
			parameterGroupId.Value = groupId;
			myCommand.Parameters.Add(parameterGroupId);

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
        /// Delete all group access records for the document
        /// </summary>
        /// <param name="itemID">The item ID.</param>
		public static void DeleteDocumentAccess(int itemID) 
		{

			if (itemID<1)
				return;

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = Config.SqlConnectionString;
			SqlCommand myCommand = new SqlCommand("rb_DeleteSecureDocumentAccess", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Add Parameters to SPROC
			SqlParameter parameterItemID = new SqlParameter("@itemid", SqlDbType.Int, 4);
			parameterItemID.Value = itemID;
			myCommand.Parameters.Add(parameterItemID);

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
        /// Delete a file from a document
        /// </summary>
        /// <param name="itemID">The item ID.</param>
		public static void DeleteDocumentFile(int itemID) 
		{

			if (itemID<1)
				return;

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = Config.SqlConnectionString;
			SqlCommand myCommand = new SqlCommand("rb_DeleteSecureDocumentFile", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Add Parameters to SPROC
			SqlParameter parameterItemID = new SqlParameter("@itemid", SqlDbType.Int, 4);
			parameterItemID.Value = itemID;
			myCommand.Parameters.Add(parameterItemID);

			SqlParameter parameterFileNameUrl = new SqlParameter("@FileNameUrl", SqlDbType.NVarChar, 250);
			parameterFileNameUrl.Direction = ParameterDirection.Output;
			myCommand.Parameters.Add(parameterFileNameUrl);

			myConnection.Open();
			try
			{
				myCommand.ExecuteNonQuery();
			}
			finally
			{
				myConnection.Close();
			}

			// Change by José Viladiu (02/07/2004). Delete phisical file
			// after sql delete if no errors in sql operation
			try
			{
				File.Delete(@parameterFileNameUrl.Value.ToString());
			}
			catch(Exception e)
			{
				ErrorHandler.Publish(LogLevel.Error, e);
			}
		} 

    }
}
