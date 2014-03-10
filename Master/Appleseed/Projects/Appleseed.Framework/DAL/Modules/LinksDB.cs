using System.Data;
using System.Data.SqlClient;
using Appleseed.Framework.Settings;

namespace Appleseed.Framework.Content.Data
{
    /// <summary>
    /// Class that encapsulates all data logic necessary to add/query/delete
    /// links within the Portal database.
    /// </summary>
    public class LinkDB
    {
        /// <summary>
        /// The GetLinks method returns a SqlDataReader containing all of the
        /// links for a specific portal module from the announcements
        /// database.
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        public SqlDataReader GetLinks(int moduleID, WorkFlowVersion version)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_GetLinks", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterModuleID = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
            parameterModuleID.Value = moduleID;
            myCommand.Parameters.Add(parameterModuleID);

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
        /// The GetSingleLink method returns a SqlDataReader containing details
        /// about a specific link from the Links database table.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        public SqlDataReader GetSingleLink(int itemID, WorkFlowVersion version)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_GetSingleLink", myConnection);

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
        /// The DeleteLink method deletes a specified link from
        /// the Links database table.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        public void DeleteLink(int itemID)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_DeleteLink", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
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
        /// The AddLink method adds a new link within the
        /// links database table, and returns ItemID value as a result.
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        /// <param name="itemID">The item ID.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="title">The title.</param>
        /// <param name="url">The URL.</param>
        /// <param name="mobileUrl">The mobile URL.</param>
        /// <param name="viewOrder">The view order.</param>
        /// <param name="description">The description.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public int AddLink(int moduleID, int itemID, string userName, string title, string url, string mobileUrl,
                           int viewOrder, string description, string target)
        {
            if (userName.Length < 1)
            {
                userName = "unknown";
            }

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_AddLink", myConnection);

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

            SqlParameter parameterDescription = new SqlParameter("@Description", SqlDbType.NVarChar, 100);
            parameterDescription.Value = description;
            myCommand.Parameters.Add(parameterDescription);

            SqlParameter parameterTarget = new SqlParameter("@Target", SqlDbType.NVarChar, 10);
            parameterTarget.Value = target;
            myCommand.Parameters.Add(parameterTarget);

            SqlParameter parameterUrl = new SqlParameter("@Url", SqlDbType.NVarChar, 800);
            parameterUrl.Value = url;
            myCommand.Parameters.Add(parameterUrl);

            SqlParameter parameterMobileUrl = new SqlParameter("@MobileUrl", SqlDbType.NVarChar, 250);
            parameterMobileUrl.Value = mobileUrl;
            myCommand.Parameters.Add(parameterMobileUrl);

            SqlParameter parameterViewOrder = new SqlParameter("@ViewOrder", SqlDbType.Int, 4);
            parameterViewOrder.Value = viewOrder;
            myCommand.Parameters.Add(parameterViewOrder);

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

        /// <summary>
        /// The UpdateLink method updates a specified link within
        /// the Links database table.
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        /// <param name="itemID">The item ID.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="title">The title.</param>
        /// <param name="url">The URL.</param>
        /// <param name="mobileUrl">The mobile URL.</param>
        /// <param name="viewOrder">The view order.</param>
        /// <param name="description">The description.</param>
        /// <param name="target">The target.</param>
        public void UpdateLink(int moduleID, int itemID, string userName, string title, string url, string mobileUrl,
                               int viewOrder, string description, string target)
        {
            if (userName.Length < 1)
            {
                userName = "unknown";
            }

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_UpdateLink", myConnection);

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

            SqlParameter parameterDescription = new SqlParameter("@Description", SqlDbType.NVarChar, 100);
            parameterDescription.Value = description;
            myCommand.Parameters.Add(parameterDescription);

            SqlParameter parameterUrl = new SqlParameter("@Url", SqlDbType.NVarChar, 800);
            parameterUrl.Value = url;
            myCommand.Parameters.Add(parameterUrl);

            SqlParameter parameterMobileUrl = new SqlParameter("@MobileUrl", SqlDbType.NVarChar, 250);
            parameterMobileUrl.Value = mobileUrl;
            myCommand.Parameters.Add(parameterMobileUrl);

            SqlParameter parameterViewOrder = new SqlParameter("@ViewOrder", SqlDbType.Int, 4);
            parameterViewOrder.Value = viewOrder;
            myCommand.Parameters.Add(parameterViewOrder);

            SqlParameter parameterTarget = new SqlParameter("@Target", SqlDbType.NVarChar, 10);
            parameterTarget.Value = target;
            myCommand.Parameters.Add(parameterTarget);


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