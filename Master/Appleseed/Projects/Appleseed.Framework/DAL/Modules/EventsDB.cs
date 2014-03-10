using System;
using System.Data;
using System.Data.SqlClient;
using Appleseed.Framework.Settings;

namespace Appleseed.Framework.Content.Data
{
    /// <summary>
    /// Class that encapsulates all data logic necessary to add/query/delete
    /// events within the Portal database.
    /// </summary>
    public class EventsDB
    {
        /// <summary>
        /// The GetEvents method returns a DataSet containing all of the
        /// events for a specific portal module from the events
        /// database.
        /// NOTE: A DataSet is returned from this method to allow this method to support
        /// both desktop and mobile Web UI.
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        public DataSet GetEvents(int moduleID, WorkFlowVersion version)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlDataAdapter myCommand = new SqlDataAdapter("rb_GetEvents", myConnection);

            // Mark the Command as a SPROC
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterModuleID = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
            parameterModuleID.Value = moduleID;
            myCommand.SelectCommand.Parameters.Add(parameterModuleID);

            // Change by Geert.Audenaert@Syntegra.Com
            // Date: 7/2/2003
            SqlParameter parameterWorkflowVersion = new SqlParameter("@WorkflowVersion", SqlDbType.Int, 4);
            parameterWorkflowVersion.Value = (int) version;
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
        /// The GetSingleEvent method returns a SqlDataReader containing details
        /// about a specific event from the events database.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        public SqlDataReader GetSingleEvent(int itemID, WorkFlowVersion version)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_GetSingleEvent", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
            parameterItemID.Value = itemID;
            myCommand.Parameters.Add(parameterItemID);

            // Change by Geert.Audenaert@Syntegra.Com
            // Date: 7/2/2003
            SqlParameter parameterWorkflowVersion = new SqlParameter("@WorkflowVersion", SqlDbType.Int, 4);
            parameterWorkflowVersion.Value = (int) version;
            myCommand.Parameters.Add(parameterWorkflowVersion);
            // End Change Geert.Audenaert@Syntegra.Com

            // Execute the command
            myConnection.Open();
            SqlDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            // Return the datareader 
            return result;
        }

        /// <summary>
        /// The DeleteEvent method deletes a specified event from
        /// the events database.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        public void DeleteEvent(int itemID)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_DeleteEvent", myConnection);

            // Mark the Command as a SPROC
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
        /// The AddEvent method adds a new event within the Events database table,
        /// and returns the ItemID value as a result.
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        /// <param name="itemID">The item ID.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="title">The title.</param>
        /// <param name="expireDate">The expire date.</param>
        /// <param name="description">The description.</param>
        /// <param name="wherewhen">The wherewhen.</param>
        /// <param name="AllDay">if set to <c>true</c> [all day].</param>
        /// <param name="StartDate">The start date.</param>
        /// <param name="StartTime">The start time.</param>
        /// <returns></returns>
        public int AddEvent(int moduleID, int itemID, string userName, string title, DateTime expireDate,
                            string description, string wherewhen, bool AllDay, string StartDate, string StartTime)
        {
            if (userName.Length < 1)
            {
                userName = "unknown";
            }

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_AddEvent", myConnection);

            // Mark the Command as a SPROC
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

            SqlParameter parameterTitle = new SqlParameter("@Title", SqlDbType.NVarChar, 100);
            parameterTitle.Value = title;
            myCommand.Parameters.Add(parameterTitle);

            SqlParameter parameterWhereWhen = new SqlParameter("@WhereWhen", SqlDbType.NVarChar, 100);
            parameterWhereWhen.Value = wherewhen;
            myCommand.Parameters.Add(parameterWhereWhen);

            // devsolution@yahoo.com 6/13/2003: start changes for Calendar
            SqlParameter parameterAllDay = new SqlParameter("@AllDay", SqlDbType.Bit, 1);
            parameterAllDay.Value = AllDay;
            myCommand.Parameters.Add(parameterAllDay);

            SqlParameter parameterStartDate = new SqlParameter("@StartDate", SqlDbType.DateTime, 8);
            if (StartDate == string.Empty)
                parameterStartDate.Value = DBNull.Value;
            else
                parameterStartDate.Value = StartDate;
            myCommand.Parameters.Add(parameterStartDate);

            SqlParameter parameterStartTime = new SqlParameter("@StartTime", SqlDbType.NVarChar, 8);
            parameterStartTime.Value = StartTime;
            myCommand.Parameters.Add(parameterStartTime);
            // devsolution@yahoo.com 6/13/2003: end changes for Calendar

            SqlParameter parameterExpireDate = new SqlParameter("@ExpireDate", SqlDbType.DateTime, 8);
            parameterExpireDate.Value = expireDate;
            myCommand.Parameters.Add(parameterExpireDate);

            SqlParameter parameterDescription = new SqlParameter("@Description", SqlDbType.NVarChar, 2000);
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

            // Return the new Event ItemID
            return (int) parameterItemID.Value;
        }

        /// <summary>
        /// The UpdateEvent method updates the specified event within
        /// the Events database table.
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        /// <param name="itemID">The item ID.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="title">The title.</param>
        /// <param name="expireDate">The expire date.</param>
        /// <param name="description">The description.</param>
        /// <param name="wherewhen">The wherewhen.</param>
        /// <param name="AllDay">if set to <c>true</c> [all day].</param>
        /// <param name="StartDate">The start date.</param>
        /// <param name="StartTime">The start time.</param>
        public void UpdateEvent(int moduleID, int itemID, string userName, string title, DateTime expireDate,
                                string description, string wherewhen, bool AllDay, string StartDate, string StartTime)
        {
            if (userName.Length < 1)
            {
                userName = "unknown";
            }

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_UpdateEvent", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
            parameterItemID.Value = itemID;
            myCommand.Parameters.Add(parameterItemID);

            SqlParameter parameterUserName = new SqlParameter("@UserName", SqlDbType.NVarChar, 100);
            parameterUserName.Value = userName;
            myCommand.Parameters.Add(parameterUserName);

            SqlParameter parameterTitle = new SqlParameter("@Title", SqlDbType.NVarChar, 100);
            parameterTitle.Value = title;
            myCommand.Parameters.Add(parameterTitle);

            SqlParameter parameterWhereWhen = new SqlParameter("@WhereWhen", SqlDbType.NVarChar, 100);
            parameterWhereWhen.Value = wherewhen;
            myCommand.Parameters.Add(parameterWhereWhen);

            // devsolution@yahoo.com 6/13/2003: start changes for Calendar
            SqlParameter parameterAllDay = new SqlParameter("@AllDay", SqlDbType.Bit, 1);
            parameterAllDay.Value = AllDay;
            myCommand.Parameters.Add(parameterAllDay);

            SqlParameter parameterStartDate = new SqlParameter("@StartDate", SqlDbType.DateTime, 8);
            if (StartDate == string.Empty)
                parameterStartDate.Value = DBNull.Value;
            else
                parameterStartDate.Value = StartDate;
            myCommand.Parameters.Add(parameterStartDate);

            SqlParameter parameterStartTime = new SqlParameter("@StartTime", SqlDbType.NVarChar, 8);
            parameterStartTime.Value = StartTime;
            myCommand.Parameters.Add(parameterStartTime);
            // devsolution@yahoo.com 6/13/2003: end changes for Calendar

            SqlParameter parameterExpireDate = new SqlParameter("@ExpireDate", SqlDbType.DateTime, 8);
            parameterExpireDate.Value = expireDate;
            myCommand.Parameters.Add(parameterExpireDate);

            SqlParameter parameterDescription = new SqlParameter("@Description", SqlDbType.NVarChar, 2000);
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