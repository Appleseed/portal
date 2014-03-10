using System.Data;
using System.Data.SqlClient;
using Appleseed.Framework.Settings;

namespace Appleseed.Framework.Content.Data
{
    /// <summary>
    /// Appleseed EnhancedHtml Module
    /// Written by: José Viladiu, jviladiu@portalServices.net
    /// Class that encapsulates all data logic necessary to add/query/delete
    /// EnhancedHtml Pages within the Portal database.
    /// </summary>
    public class EnhancedHtmlDB
    {
        /// <summary>
        /// The GetAllPages method returns a SqlDataReader containing all of the
        /// pages for a specific portal module from database.
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        public SqlDataReader GetAllPages(int moduleID, WorkFlowVersion version)
        {
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_GetEnhancedHtml", myConnection);

            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterModuleID = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
            parameterModuleID.Value = moduleID;
            myCommand.Parameters.Add(parameterModuleID);

            SqlParameter parameterWorkflowVersion = new SqlParameter("@WorkflowVersion", SqlDbType.Int, 4);
            parameterWorkflowVersion.Value = (int) version;
            myCommand.Parameters.Add(parameterWorkflowVersion);

            myConnection.Open();
            SqlDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            return result;
        }

        /// <summary>
        /// The GetAllPages method returns a SqlDataReader containing all of the
        /// pages for a specific portal module from database.
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        /// <param name="cultureCode">The culture code.</param>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        public SqlDataReader GetLocalizedPages(int moduleID, int cultureCode, WorkFlowVersion version)
        {
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_GetEnhancedLocalizedHtml", myConnection);

            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterModuleID = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
            parameterModuleID.Value = moduleID;
            myCommand.Parameters.Add(parameterModuleID);

            SqlParameter parameterCultureCode = new SqlParameter("@CultureCode", SqlDbType.Int, 4);
            parameterCultureCode.Value = cultureCode;
            myCommand.Parameters.Add(parameterCultureCode);

            SqlParameter parameterWorkflowVersion = new SqlParameter("@WorkflowVersion", SqlDbType.Int, 4);
            parameterWorkflowVersion.Value = (int) version;
            myCommand.Parameters.Add(parameterWorkflowVersion);

            myConnection.Open();
            SqlDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            return result;
        }

        /// <summary>
        /// The GetSinglePage method returns a SqlDataReader containing details
        /// about a specific page from the EnhancedHtml database table.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        public SqlDataReader GetSinglePage(int itemID, WorkFlowVersion version)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_GetSingleEnhancedHtml", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
            parameterItemID.Value = itemID;
            myCommand.Parameters.Add(parameterItemID);

            SqlParameter parameterWorkflowVersion = new SqlParameter("@WorkflowVersion", SqlDbType.Int, 4);
            parameterWorkflowVersion.Value = (int) version;
            myCommand.Parameters.Add(parameterWorkflowVersion);

            // Execute the command
            myConnection.Open();
            SqlDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            // Return the datareader 
            return result;
        }

        /// <summary>
        /// The DeleteLink method deletes a specified page from
        /// the EnhancedHtml database table.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        public void DeletePage(int itemID)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_DeleteEnhancedHtml", myConnection);

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
        /// The AddPage method adds a new page within the
        /// EnhancedHtml database table, and returns ItemID value as a result.
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        /// <param name="itemID">The item ID.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="title">The title.</param>
        /// <param name="viewOrder">The view order.</param>
        /// <param name="cultureCode">The culture code.</param>
        /// <param name="desktopHtml">The desktop HTML.</param>
        /// <returns></returns>
        public int AddPage(int moduleID, int itemID, string userName, string title, int viewOrder, int cultureCode,
                           string desktopHtml)
        {
            if (userName.Length < 1)
            {
                userName = "unknown";
            }
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_AddEnhancedHtml", myConnection);

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

            SqlParameter parameterViewOrder = new SqlParameter("@ViewOrder", SqlDbType.Int, 4);
            parameterViewOrder.Value = viewOrder;
            myCommand.Parameters.Add(parameterViewOrder);

            SqlParameter parameterCultureCode = new SqlParameter("@CultureCode", SqlDbType.Int, 4);
            parameterCultureCode.Value = cultureCode;
            myCommand.Parameters.Add(parameterCultureCode);

            SqlParameter parameterDesktopHtml = new SqlParameter("@DesktopHtml", SqlDbType.NText);
            parameterDesktopHtml.Value = desktopHtml;
            myCommand.Parameters.Add(parameterDesktopHtml);

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
        /// The UpdateLink method updates a specified page within
        /// the EnhancedHtml database table.
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        /// <param name="itemID">The item ID.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="title">The title.</param>
        /// <param name="viewOrder">The view order.</param>
        /// <param name="cultureCode">The culture code.</param>
        /// <param name="desktopHtml">The desktop HTML.</param>
        public void UpdatePage(int moduleID, int itemID, string userName, string title, int viewOrder, int cultureCode,
                               string desktopHtml)
        {
            if (userName.Length < 1)
            {
                userName = "unknown";
            }

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_UpdateEnhancedHtml", myConnection);

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

            SqlParameter parameterViewOrder = new SqlParameter("@ViewOrder", SqlDbType.Int, 4);
            parameterViewOrder.Value = viewOrder;
            myCommand.Parameters.Add(parameterViewOrder);

            SqlParameter parameterCultureCode = new SqlParameter("@CultureCode", SqlDbType.Int, 4);
            parameterCultureCode.Value = cultureCode;
            myCommand.Parameters.Add(parameterCultureCode);

            SqlParameter parameterDesktopHtml = new SqlParameter("@DesktopHtml", SqlDbType.NText);
            parameterDesktopHtml.Value = desktopHtml;
            myCommand.Parameters.Add(parameterDesktopHtml);

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