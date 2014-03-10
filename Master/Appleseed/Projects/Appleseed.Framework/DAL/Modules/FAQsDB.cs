using System;
using System.Data;
using System.Data.SqlClient;

using Appleseed.Framework.Settings;
using Appleseed.Framework.Web.UI.WebControls;
using Appleseed.Framework.Data;
namespace Appleseed.Framework.Content.Data
{

	/// <summary>
	/// IBS Portal FAQ module
	/// (c)2002 by Christopher S Judd, CDP &amp; Horizons, LLC
	/// Moved into Appleseed by Jakob Hansen, hansen3000@hotmail.com
	/// </summary>
	public class FAQsDB
	{

        /// <summary>
        /// The AddFAQ function is used to ADD FAQs to the Database
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        /// <param name="itemID">The item ID.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="question">The question.</param>
        /// <param name="answer">The answer.</param>
        /// <returns></returns>
		public int AddFAQ(int moduleID, int itemID, string userName, string question, string answer) 
		{
            //  Create Instance of Connection and Command Object
			SqlConnection myConnection = Config.SqlConnectionString;
			SqlCommand myCommand = new SqlCommand("rb_AddFAQ", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            //  Add Parameters to SPROC
            SqlParameter parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
            parameterItemID.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterItemID);

            SqlParameter parameterModuleID = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
            parameterModuleID.Value = moduleID;
            myCommand.Parameters.Add(parameterModuleID);

			SqlParameter parameterUserName = new SqlParameter("@UserName", SqlDbType.NVarChar, 100);
			parameterUserName.Value = userName;
			myCommand.Parameters.Add(parameterUserName);

            SqlParameter parameterQuestion = new SqlParameter("@Question", SqlDbType.NVarChar, 500);
            parameterQuestion.Value = question;
            myCommand.Parameters.Add(parameterQuestion);

			//Changed to Ntext from NVarChar, 4000
            SqlParameter parameterAnswer = new SqlParameter("@Answer", SqlDbType.NText);
            parameterAnswer.Value = answer;
            myCommand.Parameters.Add(parameterAnswer);

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
        /// The GetFAQ function is used to get all the FAQs in the module
        /// </summary>
        /// <param name="moduleID">moduleID</param>
        /// <returns>A DataSet</returns>
		public DataSet GetFAQ(int moduleID) 
		{
            //  Create Instance of Connection and Command Object
			SqlConnection myConnection = Config.SqlConnectionString;
			SqlDataAdapter myCommand = new SqlDataAdapter("rb_GetFAQ", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            //  Add Parameters to SPROC
            SqlParameter parameterModuleID = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
            parameterModuleID.Value = moduleID;
            myCommand.SelectCommand.Parameters.Add(parameterModuleID);

            //  Create and Fill the DataSet
            DataSet myDataSet = new DataSet();
			try
			{
				myCommand.Fill(myDataSet);
			}
			finally
			{
				myConnection.Close();
			}
            //  return the DataSet
            return myDataSet;
        }


        /// <summary>
        /// The GetSingleFAQ function is used to Get a single FAQ
        /// from the database for display/edit
        /// </summary>
        /// <param name="itemID">itemID</param>
        /// <returns>A SqlDataReader</returns>
		public SqlDataReader GetSingleFAQ(int itemID) 
		{

            //  Create Instance of Connection and Command Object
			SqlConnection myConnection = Config.SqlConnectionString;
			SqlCommand myCommand = new SqlCommand("rb_GetSingleFAQ", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            //  Add Parameters to SPROC
            SqlParameter parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
            parameterItemID.Value = itemID;
            myCommand.Parameters.Add(parameterItemID);

            //  Execute the command
            myConnection.Open();
            SqlDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            //  return the datareader 
            return result;
        }


        /// <summary>
        /// The DeleteFAQ function is used to remove FAQs from the Database
        /// </summary>
        /// <param name="itemID">itemID</param>
		public void DeleteFAQ(int itemID)
		{
            //  Create Instance of Connection and Command Object
			SqlConnection myConnection = Config.SqlConnectionString;
			SqlCommand myCommand = new SqlCommand("rb_DeleteFAQ", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            //  Add Parameters to SPROC
            SqlParameter parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
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
        /// The UpdateFAQ function is used to update changes to the FAQs
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        /// <param name="itemID">The item ID.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="question">The question.</param>
        /// <param name="answer">The answer.</param>
		public void UpdateFAQ(int moduleID, int itemID, string userName, string question, string answer)
		{
            //  Create Instance of Connection and Command Object
			SqlConnection myConnection = Config.SqlConnectionString;
			SqlCommand myCommand = new SqlCommand("rb_UpdateFAQ", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            //  Add Parameters to SPROC
            SqlParameter parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
            parameterItemID.Value = itemID;
            myCommand.Parameters.Add(parameterItemID);

			SqlParameter parameterUserName = new SqlParameter("@UserName", SqlDbType.NVarChar, 100);
			parameterUserName.Value = userName;
			myCommand.Parameters.Add(parameterUserName);

            SqlParameter parameterQuestion = new SqlParameter("@Question", SqlDbType.NVarChar, 500);
            parameterQuestion.Value = question;
            myCommand.Parameters.Add(parameterQuestion);

			// Changed to NText from NVarChar, 4000
            SqlParameter parameterAnswer = new SqlParameter("@Answer", SqlDbType.NText);
            parameterAnswer.Value = answer;
            myCommand.Parameters.Add(parameterAnswer);
            
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
