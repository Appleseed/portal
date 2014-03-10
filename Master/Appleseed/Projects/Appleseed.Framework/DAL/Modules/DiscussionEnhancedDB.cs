using System;
using System.Data;
using System.Data.SqlClient;
using Appleseed.Framework.Settings;

namespace Appleseed.Framework.Content.Data
{
    /// <summary>
    /// DiscussionEnhanced is an enhanced version of the standard threaded discussion.
    /// The goal here is to build up to the power of a simple threaded disucssion by
    /// adding grouping and some other features.
    /// 
    /// jminond - 3/2005
    /// </summary>
    public class DiscussionEnhancedDB
    {
        #region Groups

        /// <summary>
        /// Gets the groups in site.
        /// </summary>
        /// <param name="PortalID">The portal ID.</param>
        /// <returns></returns>
        public SqlDataReader GetGroupsInSite(int PortalID)
        {
            throw new NotImplementedException("GetGroupsInSite");
        }

        /// <summary>
        /// Gets the groups in module.
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        /// <returns></returns>
        public SqlDataReader GetGroupsInModule(int moduleID)
        {
            throw new NotImplementedException("GetGroupsInModule");
        }

        /// <summary>
        /// Return top level messages within a group
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        /// <returns></returns>
        public SqlDataReader GetTopLevelMessages(int moduleID)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_DiscussionGetTopLevelMessages", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterModuleID = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
            parameterModuleID.Value = moduleID;
            myCommand.Parameters.Add(parameterModuleID);

            // Execute the command
            myConnection.Open();
            SqlDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            // Return the datareader 
            return result;
        }

        /// <summary>
        /// Gets the group threads.
        /// </summary>
        /// <param name="GroupID">The group ID.</param>
        /// <returns></returns>
        public SqlDataReader GetGroupThreads(int GroupID)
        {
            throw new NotImplementedException("GetGroupThreads");
        }

        /// <summary>
        /// Adds the new group.
        /// </summary>
        /// <returns></returns>
        public int AddNewGroup()
        {
            throw new NotImplementedException("AddNewGroup");
        }

        #endregion

        #region Threads

        /// <summary>
        /// GetThreadMessages Method
        /// Returns details for all of the messages the thread, as identified by the Parent id string.
        /// displayOrder csan be the full display order of any post, it will be truncated
        /// by the stored procedure to find the root of the thread, and then all children
        /// are returned
        /// Other relevant sources:
        /// + <a href="GetThreadMessages.htm" style="color:green">GetThreadMessages Stored Procedure</a>
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <param name="showRoot">The show root.</param>
        /// <returns></returns>
        public SqlDataReader GetThreadMessages(int itemID, char showRoot)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_DiscussionGetThreadMessages", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterParent = new SqlParameter("@ItemID", SqlDbType.NVarChar, 750);
            parameterParent.Value = itemID;
            myCommand.Parameters.Add(parameterParent);

            // Add Parameters to SPROC
            SqlParameter parameterShowRoot = new SqlParameter("@IncludeRoot", SqlDbType.Char);
            parameterShowRoot.Value = showRoot;
            myCommand.Parameters.Add(parameterShowRoot);

            // Execute the command
            myConnection.Open();
            SqlDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            // Return the datareader 
            return result;
        }


        /// <summary>
        /// Deletes the single message.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        public void DeleteSingleMessage(int itemID)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_DiscussionDeleteMessage", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
            parameterItemID.Value = itemID;
            myCommand.Parameters.Add(parameterItemID);

            // Execute the command
            myConnection.Open();
            SqlDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            // Return the datareader 
            return;
        }

        /// <summary>
        /// Increments the view count.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        public void IncrementViewCount(int itemID)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_DiscussionIncrementViewCount", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
            parameterItemID.Value = itemID;
            myCommand.Parameters.Add(parameterItemID);

            // Execute the command
            myConnection.Open();
            SqlDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            // Return the datareader 
            return;
        }

        /// <summary>
        /// Deletes the children.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <returns></returns>
        public int DeleteChildren(int itemID)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_DiscussionDeleteChildren", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
            parameterItemID.Value = itemID;
            myCommand.Parameters.Add(parameterItemID);

            SqlParameter parameterNumDeletedMessages = new SqlParameter("@NumDeletedMessages", SqlDbType.Int, 4);
            parameterNumDeletedMessages.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterNumDeletedMessages);

            // Execute the command
            myConnection.Open();
            SqlDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            // Return the datareader 
            return (int) parameterNumDeletedMessages.Value;
        }

        /// <summary>
        /// GetSingleMessage Method
        /// The GetSingleMessage method returns the details for the message
        /// specified by the itemID parameter.
        /// Other relevant sources:
        /// + <a href="GetSingleMessage.htm" style="color:green">GetSingleMessage Stored Procedure</a>
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <returns></returns>
        public SqlDataReader GetSingleMessage(int itemID)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_DiscussionGetMessage", myConnection);

            // Mark the Command as a SPROC
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
        /// AddMessage Method
        /// The AddMessage method adds a new message within the
        /// Discussions database table, and returns ItemID value as a result.
        /// Other relevant sources:
        /// + <a href="AddMessage.htm" style="color:green">AddMessage Stored Procedure</a>
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        /// <param name="parentID">The parent ID.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="title">The title.</param>
        /// <param name="body">The body.</param>
        /// <param name="mode">The mode.</param>
        /// <returns></returns>
        public int AddMessage(int moduleID, int parentID, string userName, string title, string body, string mode)
        {
            /* ParentID = actual ItemID if this is an edit operation */

            if (userName.Length < 1)
            {
                userName = "unknown";
            }

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_DiscussionAddMessage", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterMode = new SqlParameter("@Mode", SqlDbType.Text, 20);
            parameterMode.Value = mode;
            myCommand.Parameters.Add(parameterMode);

            SqlParameter parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
            parameterItemID.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterItemID);

            SqlParameter parameterTitle = new SqlParameter("@Title", SqlDbType.NVarChar, 100);
            parameterTitle.Value = title;
            myCommand.Parameters.Add(parameterTitle);

            SqlParameter parameterBody = new SqlParameter("@Body", SqlDbType.NVarChar, 3000);
            parameterBody.Value = body;
            myCommand.Parameters.Add(parameterBody);

            SqlParameter parameterParentID = new SqlParameter("@ParentID", SqlDbType.Int, 4);
            parameterParentID.Value = parentID;
            myCommand.Parameters.Add(parameterParentID);

            SqlParameter parameterUserName = new SqlParameter("@UserName", SqlDbType.NVarChar, 100);
            parameterUserName.Value = userName;
            myCommand.Parameters.Add(parameterUserName);

            SqlParameter parameterModuleID = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
            parameterModuleID.Value = moduleID;
            myCommand.Parameters.Add(parameterModuleID);

            myConnection.Open();
            try
            {
                myCommand.ExecuteNonQuery();
            }
            finally
            {
                myConnection.Close();
            }

            return (int) parameterItemID.Value;
        }

        #endregion
    }
}