using System;
using System.Data;
using System.Data.SqlClient;
using Appleseed.Framework.Settings;

namespace Appleseed.Framework.Content.Data
{
    /// <summary>
    /// IBS Tasks module
    /// Class that encapsulates all data logic necessary to add/query/delete
    /// tasks within the Portal database.
    /// Written by: ??? (the guy did not write his name in the original code)
    /// Moved into Appleseed by Jakob Hansen, hansen3000@hotmail.com
    /// EHN - By Mike Stone Change Description field to ntext to remove the 3000 char limit
    /// </summary>
    public class TasksDB
    {
        /// <summary>
        /// GetTasks
        /// NOTE: A DataSet is returned from this method to allow this method to support
        /// both desktop and mobile Web UI.
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        /// <returns>A DataSet</returns>
        public DataSet GetTasks(int moduleID)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlDataAdapter myCommand = new SqlDataAdapter("rb_GetTasks", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterModuleID = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
            parameterModuleID.Value = moduleID;
            myCommand.SelectCommand.Parameters.Add(parameterModuleID);

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
        /// GetSingleTask
        /// </summary>
        /// <param name="itemID">ItemID</param>
        /// <returns>A SqlDataReader</returns>
        public SqlDataReader GetSingleTask(int itemID)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_GetSingleTask", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
            parameterItemID.Value = itemID;
            myCommand.Parameters.Add(parameterItemID);

            // Execute the command
            myConnection.Open();
            SqlDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            // Return the datareader 
            return result;
        }


        /// <summary>
        /// Deletes the task.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        public void DeleteTask(int itemID)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_DeleteTask", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
            parameterItemID.Value = itemID;
            myCommand.Parameters.Add(parameterItemID);

            // Open the database connection and execute SQL Command
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
        /// Adds the task.
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        /// <param name="itemID">The item ID.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="title">The title.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="description">The description.</param>
        /// <param name="status">The status.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="assignedto">The assignedto.</param>
        /// <param name="dueDate">The due date.</param>
        /// <param name="percentComplete">The percent complete.</param>
        /// <returns></returns>
        public int AddTask(int moduleID, int itemID, string userName, string title, DateTime startDate,
                           string description, string status, string priority, string assignedto, DateTime dueDate,
                           int percentComplete)
        {
            if (userName.Length < 1)
            {
                userName = "unknown";
            }

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_AddTask", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
            parameterItemID.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterItemID);

            SqlParameter parameterModuleID = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
            parameterModuleID.Value = moduleID;
            myCommand.Parameters.Add(parameterModuleID);

            SqlParameter parameterUserName = new SqlParameter("@UserName", SqlDbType.NVarChar, 100);
            parameterUserName.Value = userName;
            myCommand.Parameters.Add(parameterUserName);

            SqlParameter parameterStatus = new SqlParameter("@Status", SqlDbType.NVarChar, 20);
            parameterStatus.Value = status;
            myCommand.Parameters.Add(parameterStatus);

            SqlParameter parameterPercentComplete = new SqlParameter("@PercentComplete", SqlDbType.Int, 4);
            parameterPercentComplete.Value = percentComplete;
            myCommand.Parameters.Add(parameterPercentComplete);

            SqlParameter parameterPriority = new SqlParameter("@Priority", SqlDbType.NVarChar, 20);
            parameterPriority.Value = priority;
            myCommand.Parameters.Add(parameterPriority);

            SqlParameter parameterTitle = new SqlParameter("@Title", SqlDbType.NVarChar, 100);
            parameterTitle.Value = title;
            myCommand.Parameters.Add(parameterTitle);

            SqlParameter parameterAssignedTo = new SqlParameter("@AssignedTo", SqlDbType.NVarChar, 100);
            parameterAssignedTo.Value = assignedto;
            myCommand.Parameters.Add(parameterAssignedTo);

            SqlParameter parameterStartDate = new SqlParameter("@StartDate", SqlDbType.DateTime, 8);
            parameterStartDate.Value = startDate;
            myCommand.Parameters.Add(parameterStartDate);

            SqlParameter parameterDueDate = new SqlParameter("@DueDate", SqlDbType.DateTime, 8);
            parameterDueDate.Value = dueDate;
            myCommand.Parameters.Add(parameterDueDate);

            SqlParameter parameterDescription = new SqlParameter("@Description", SqlDbType.NText);
            parameterDescription.Value = description;
            myCommand.Parameters.Add(parameterDescription);

            // Open the database connection and execute SQL Command
            myConnection.Open();
            try
            {
                myCommand.ExecuteNonQuery();
            }
            finally
            {
                myConnection.Close();
            }

            // Return the new Task ItemID
            return (int) parameterItemID.Value;
        }


        /// <summary>
        /// Updates the task.
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        /// <param name="itemID">The item ID.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="title">The title.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="description">The description.</param>
        /// <param name="status">The status.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="assignedto">The assignedto.</param>
        /// <param name="dueDate">The due date.</param>
        /// <param name="percentComplete">The percent complete.</param>
        public void UpdateTask(int moduleID, int itemID, string userName, string title, DateTime startDate,
                               string description, string status, string priority, string assignedto, DateTime dueDate,
                               int percentComplete)
        {
            if (userName.Length < 1)
            {
                userName = "unknown";
            }

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_UpdateTask", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
            parameterItemID.Value = itemID;
            myCommand.Parameters.Add(parameterItemID);

            SqlParameter parameterUserName = new SqlParameter("@UserName", SqlDbType.NVarChar, 100);
            parameterUserName.Value = userName;
            myCommand.Parameters.Add(parameterUserName);

            SqlParameter parameterStatus = new SqlParameter("@Status", SqlDbType.NVarChar, 20);
            parameterStatus.Value = status;
            myCommand.Parameters.Add(parameterStatus);

            SqlParameter parameterPercentComplete = new SqlParameter("@PercentComplete", SqlDbType.Int, 4);
            parameterPercentComplete.Value = percentComplete;
            myCommand.Parameters.Add(parameterPercentComplete);

            SqlParameter parameterPriority = new SqlParameter("@Priority", SqlDbType.NVarChar, 20);
            parameterPriority.Value = priority;
            myCommand.Parameters.Add(parameterPriority);

            SqlParameter parameterTitle = new SqlParameter("@Title", SqlDbType.NVarChar, 100);
            parameterTitle.Value = title;
            myCommand.Parameters.Add(parameterTitle);

            SqlParameter parameterAssignedTo = new SqlParameter("@AssignedTo", SqlDbType.NVarChar, 100);
            parameterAssignedTo.Value = assignedto;
            myCommand.Parameters.Add(parameterAssignedTo);

            SqlParameter parameterStartDate = new SqlParameter("@StartDate", SqlDbType.DateTime, 8);
            parameterStartDate.Value = startDate;
            myCommand.Parameters.Add(parameterStartDate);

            SqlParameter parameterDueDate = new SqlParameter("@DueDate", SqlDbType.DateTime, 8);
            parameterDueDate.Value = dueDate;
            myCommand.Parameters.Add(parameterDueDate);

            SqlParameter parameterDescription = new SqlParameter("@Description", SqlDbType.NText);
            parameterDescription.Value = description;
            myCommand.Parameters.Add(parameterDescription);

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