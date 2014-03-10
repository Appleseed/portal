using System.Data;
using System.Data.SqlClient;
using Appleseed.Framework.Settings;

namespace Appleseed.Framework.Content.Data
{
    /// <summary>
    /// Books DB
    /// </summary>
    public class BooksDB
    {
        /// <summary>
        /// GetSinglerb_BookList
        /// </summary>
        /// <param name="ItemID">ItemID</param>
        /// <returns>A SqlDataReader</returns>
        public SqlDataReader GetSinglerb_BookList(int ItemID)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_GetSingleBook", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int);
            parameterItemID.Value = ItemID;
            myCommand.Parameters.Add(parameterItemID);

            // Execute the command
            myConnection.Open();
            SqlDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            // Return the datareader
            return result;
        }


        /// <summary>
        /// Getrb_BookList
        /// </summary>
        /// <param name="ModuleId">ModuleId</param>
        /// <returns>A SqlDataReader</returns>
        public DataSet Getrb_BookList(int ModuleId)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlDataAdapter myCommand = new SqlDataAdapter("rb_GetBooks", myConnection);

            // Mark the Command as a SPROC
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterModuleId = new SqlParameter("@ModuleID", SqlDbType.Int);
            parameterModuleId.Value = ModuleId;
            myCommand.SelectCommand.Parameters.Add(parameterModuleId);

            DataSet myDataSet = new DataSet();
            myCommand.Fill(myDataSet);

            return myDataSet;
        }


        /// <summary>
        /// Deleterb_BookList
        /// </summary>
        /// <param name="ItemID">ItemID</param>
        public void Deleterb_BookList(int ItemID)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_DeleteBook", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int);
            parameterItemID.Value = ItemID;
            myCommand.Parameters.Add(parameterItemID);

            // Execute the command
            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }


        /// <summary>
        /// Addrb_BookList
        /// </summary>
        /// <param name="ModuleID">The module ID.</param>
        /// <param name="CreatedByUser">The created by user.</param>
        /// <param name="ISBN">The ISBN.</param>
        /// <param name="Caption">The caption.</param>
        /// <returns>The newly created ID</returns>
        public int Addrb_BookList(int ModuleID, string CreatedByUser, string ISBN, string Caption)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_AddBook", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int);
            parameterItemID.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterItemID);

            SqlParameter parameterModuleID = new SqlParameter("@ModuleID", SqlDbType.Int);
            parameterModuleID.Value = ModuleID;
            myCommand.Parameters.Add(parameterModuleID);

            SqlParameter parameterCreatedByUser = new SqlParameter("@CreatedByUser", SqlDbType.NVarChar, 100);
            parameterCreatedByUser.Value = CreatedByUser;
            myCommand.Parameters.Add(parameterCreatedByUser);

            SqlParameter parameterISBN = new SqlParameter("@ISBN", SqlDbType.NVarChar, 10);
            parameterISBN.Value = ISBN;
            myCommand.Parameters.Add(parameterISBN);

            SqlParameter parameterCaption = new SqlParameter("@Caption", SqlDbType.NText);
            parameterCaption.Value = Caption;
            myCommand.Parameters.Add(parameterCaption);

            // Open the database connection and execute the command
            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();

            // Return the newly created ID
            return (int) parameterItemID.Value;
        }


        /// <summary>
        /// Updaterb_BookList
        /// </summary>
        /// <param name="ItemID">The item ID.</param>
        /// <param name="CreatedByUser">The created by user.</param>
        /// <param name="ISBN">The ISBN.</param>
        /// <param name="Caption">The caption.</param>
        public void Updaterb_BookList(int ItemID, string CreatedByUser, string ISBN, string Caption)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_UpdateBook", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Update Parameters to SPROC
            SqlParameter parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int);
            parameterItemID.Value = ItemID;
            myCommand.Parameters.Add(parameterItemID);

            SqlParameter parameterCreatedByUser = new SqlParameter("@CreatedByUser", SqlDbType.NVarChar, 100);
            parameterCreatedByUser.Value = CreatedByUser;
            myCommand.Parameters.Add(parameterCreatedByUser);

            SqlParameter parameterISBN = new SqlParameter("@ISBN", SqlDbType.NVarChar, 10);
            parameterISBN.Value = ISBN;
            myCommand.Parameters.Add(parameterISBN);

            SqlParameter parameterCaption = new SqlParameter("@Caption", SqlDbType.NText);
            parameterCaption.Value = Caption;
            myCommand.Parameters.Add(parameterCaption);

            // Execute the command
            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }
    }
}