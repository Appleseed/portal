using System.Data;
using System.Data.SqlClient;
using System.IO;
using Appleseed.Framework.Site.Configuration;
using Appleseed.Framework;
using Appleseed.Framework.Settings;
using Appleseed.Framework.Web.UI.WebControls;
using Appleseed.Framework.Data;
namespace Appleseed.Framework.Content.Data
{
	/// <summary>
	/// Class that encapsulates all data logic necessary to add/query/delete
	/// documents within the Portal database.
	/// </summary>
	[History("José Viladiu", "2004/07/02", "Generate Documentation in correct format")]
    public class DocumentDB 
    {
        /// <summary>
        /// The GetDocuments method returns a DataSet containing all of the
        /// documents for a specific portal module from the documents datatable.
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        /// <returns>The required DataSet</returns>
        public DataSet GetDocuments(int moduleID) 
        {
			return GetDocuments(moduleID, WorkFlowVersion.Production);
		}

        /// <summary>
        /// The GetDocuments method returns a DataSet containing all of the
        /// documents for a specific portal module from the documents datatable.
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        /// <param name="version">The version.</param>
        /// <returns>The required DataSet</returns>
        public DataSet GetDocuments(int moduleID, WorkFlowVersion version) 
        {
            // Create Instance of Connection and Command Object
			SqlConnection myConnection = Config.SqlConnectionString;
			SqlDataAdapter myCommand = new SqlDataAdapter("rb_GetDocuments", myConnection);

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
            SqlCommand myCommand = new SqlCommand("rb_GetSingleDocument", myConnection);

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
            SqlCommand myCommand = new SqlCommand("rb_GetDocumentContent", myConnection);

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
            SqlCommand myCommand = new SqlCommand("rb_DeleteDocument", myConnection);

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

			// Change by José Viladiu (02/07/2004). Delete phisical file
			// after sql delete if no errors in sql operation
			try
			{
				File.Delete(@fileName);
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
        /// <param name="url">The URL.</param>
        /// <param name="category">The category.</param>
        /// <param name="content">The content.</param>
        /// <param name="size">The size.</param>
        /// <param name="contentType">Type of the content.</param>
        public void UpdateDocument(int moduleID, int itemID, string userName, string name, string url, string category, byte[] content, int size, string contentType) 
        {
            if (userName.Length < 1) 
            {
				userName = "unknown";
            }
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_UpdateDocument", myConnection);

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

            SqlParameter parameterFileUrl = new SqlParameter("@FileNameUrl", SqlDbType.NVarChar, 250);
            parameterFileUrl.Value = url;
            myCommand.Parameters.Add(parameterFileUrl);

            SqlParameter parameterCategory = new SqlParameter("@Category", SqlDbType.NVarChar, 50);
            parameterCategory.Value = category;
            myCommand.Parameters.Add(parameterCategory);

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
    }
}
