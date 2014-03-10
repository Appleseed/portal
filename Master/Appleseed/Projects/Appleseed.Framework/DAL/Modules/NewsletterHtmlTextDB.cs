using System.Data;
using System.Data.SqlClient;

using Appleseed.Framework.Settings;
using Appleseed.Framework;
using Appleseed.Framework.Data;
namespace Appleseed.Framework.Content.Data
{
	/// <summary>
	/// Class that encapsulates all data logic necessary to add/query/delete
	/// HTML/text within the Portal database.
	/// </summary>
	public class NewsletterHtmlTextDB 
	{
        /// <summary>
        /// Gets the HTML text string.
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        /// <param name="version">The version.</param>
        /// <returns></returns>
		public string GetHtmlTextString(int moduleID, WorkFlowVersion version) 
		{
			string strDesktopHtml = string.Empty;
			
			// Create Instance of Connection and Command Object
			using (SqlConnection myConnection = Config.SqlConnectionString)
			{
				using (SqlCommand myCommand = new SqlCommand("rb_GetHtmlText", myConnection))
				{

					// Mark the Command as a SPROC
					myCommand.CommandType = CommandType.StoredProcedure;

					// Add Parameters to SPROC
					SqlParameter parameterModuleID = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
					parameterModuleID.Value = moduleID;
					myCommand.Parameters.Add(parameterModuleID);

					// Change by Geert.Audenaert@Syntegra.Com
					// Date: 6/2/2003
					SqlParameter parameterWorkflowVersion = new SqlParameter("@WorkflowVersion", SqlDbType.Int, 4);
					parameterWorkflowVersion.Value = (int)version;
					myCommand.Parameters.Add(parameterWorkflowVersion);
					// End Change Geert.Audenaert@Syntegra.Com

					// Execute the command
					myConnection.Open();
					
					using (SqlDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection)) 
					{
						try
						{
							if (result.Read())
							{
								strDesktopHtml = result["DesktopHtml"].ToString();
							}
						}
						finally
						{
							// Close the datareader
							result.Close();
						}
					}
				}
			}

			return strDesktopHtml;
		}

        /// <summary>
        /// The GetHtmlText method returns a SqlDataReader containing details
        /// about a specific item from the HtmlText database table.
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        /// <returns></returns>
		public SqlDataReader GetHtmlText(int moduleID) 
		{
			// Change by Geert.Audenaert@Syntegra.Com
			// Date: 6/2/2003
			// Get prod version by default
			return GetHtmlText(moduleID, WorkFlowVersion.Production);
			// End Change Geert.Audenaert@Syntegra.Com
		}

        /// <summary>
        /// The GetHtmlText method returns a SqlDataReader containing details
        /// about a specific item from the HtmlText database table.
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        /// <param name="version">The version.</param>
        /// <returns></returns>
		public SqlDataReader GetHtmlText(int moduleID, WorkFlowVersion version) 
		{
			// Create Instance of Connection and Command Object
			SqlConnection myConnection = Config.SqlConnectionString;
			
			SqlCommand myCommand = new SqlCommand("rb_GetHtmlText", myConnection);
				

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Add Parameters to SPROC
			SqlParameter parameterModuleID = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
			parameterModuleID.Value = moduleID;
			myCommand.Parameters.Add(parameterModuleID);

			// Change by Geert.Audenaert@Syntegra.Com
			// Date: 6/2/2003
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
        /// The UpdateHtmlText method updates a specified item within
        /// the HtmlText database table.
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        /// <param name="desktopHtml">The desktop HTML.</param>
        /// <param name="mobileSummary">The mobile summary.</param>
        /// <param name="mobileDetails">The mobile details.</param>
		public void UpdateHtmlText(int moduleID, string desktopHtml, string mobileSummary, string mobileDetails) 
		{
			// Create Instance of Connection and Command Object
			using (SqlConnection myConnection = Config.SqlConnectionString)
			{
				using (SqlCommand myCommand = new SqlCommand("rb_UpdateHtmlText", myConnection))
				{

					// Mark the Command as a SPROC
					myCommand.CommandType = CommandType.StoredProcedure;

					// Add Parameters to SPROC
					SqlParameter parameterModuleID = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
					parameterModuleID.Value = moduleID;
					myCommand.Parameters.Add(parameterModuleID);

					SqlParameter parameterDesktopHtml = new SqlParameter("@DesktopHtml", SqlDbType.NText);
					parameterDesktopHtml.Value = desktopHtml;
					myCommand.Parameters.Add(parameterDesktopHtml);

					SqlParameter parameterMobileSummary = new SqlParameter("@MobileSummary", SqlDbType.NText);
					parameterMobileSummary.Value = mobileSummary;
					myCommand.Parameters.Add(parameterMobileSummary);

					SqlParameter parameterMobileDetails = new SqlParameter("@MobileDetails", SqlDbType.NText);
					parameterMobileDetails.Value = mobileDetails;
					myCommand.Parameters.Add(parameterMobileDetails);
					//
					//            SqlParameter parameterCulture = new SqlParameter("@Culture", SqlDbType.NVarChar, 8);
					//            parameterCulture.Value = culture.Name;
					//            myCommand.Parameters.Add(parameterCulture);

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
	}
}
