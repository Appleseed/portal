using System.Data;
using System.Data.SqlClient;
using System.Text;
using Appleseed.Framework.Site.Configuration;
using Appleseed.Framework.Helpers;
using Appleseed.Framework.Settings;
using Appleseed.Framework;
using Appleseed.Framework.Data;

namespace Appleseed.Framework.Content.Data
{

	/// <summary>
	/// Blacklist module
	/// Class that encapsulates all data logic necessary to add/query/delete
	/// blackliste users within the Portal database.
	/// Written by: Manu and Jakob Hansen
	/// </summary>
    public class BlacklistDB 
	{

        /// <summary>
        /// The AddToBlackList adds specified email to current Blacklist.
        /// Uses AddToBlackList Stored Procedure.
        /// </summary>
        /// <param name="portalID">The portal ID.</param>
        /// <param name="EMail">The E mail.</param>
        /// <param name="Reason">The reason.</param>
        static public void AddToBlackList(int portalID, string EMail, string Reason) 
        {
			if (Config.UseSingleUserBase) portalID = 0;
			
			// Create Instance of Connection and Command Object
        	SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_AddToBlackList", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterPortalID = new SqlParameter("@PortalID", SqlDbType.Int, 4);
            parameterPortalID.Value = portalID;
            myCommand.Parameters.Add(parameterPortalID);

            SqlParameter parameterEMail = new SqlParameter("@EMail", SqlDbType.NVarChar, 100);
            parameterEMail.Value = EMail;
            myCommand.Parameters.Add(parameterEMail);

            SqlParameter parameterReason = new SqlParameter("@Reason", SqlDbType.NVarChar, 150);
            parameterReason.Value = Reason;
            myCommand.Parameters.Add(parameterReason);

            // Open the database connection and execute the command
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
        /// The DeleteFromBlackList deletes a specified email from current Blacklist.
        /// Uses DeleteFromBlackList Stored Procedure.
        /// </summary>
        /// <param name="portalID">The portal ID.</param>
        /// <param name="EMail">The E mail.</param>
		static public void DeleteFromBlackList(int portalID, string EMail) 
		{
			if (Config.UseSingleUserBase) portalID = 0;
			
			// Create Instance of Connection and Command Object
			SqlConnection myConnection = Config.SqlConnectionString;
			SqlCommand myCommand = new SqlCommand("rb_DeleteFromBlackList", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			// Add Parameters to SPROC
			SqlParameter parameterPortalID = new SqlParameter("@PortalID", SqlDbType.Int, 4);
			parameterPortalID.Value = portalID;
			myCommand.Parameters.Add(parameterPortalID);

			SqlParameter parameterEMail = new SqlParameter("@EMail", SqlDbType.NVarChar, 100);
			parameterEMail.Value = EMail;
			myCommand.Parameters.Add(parameterEMail);

			// Open the database connection and execute the command
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
        /// Gets the blacklist.
        /// </summary>
        /// <param name="portalID">The portal ID.</param>
        /// <param name="showAllUsers">if set to <c>true</c> [show all users].</param>
        /// <param name="SendNewsletterOnly">if set to <c>true</c> [send newsletter only].</param>
        /// <returns></returns>
		public DataSet GetBlacklist(int portalID, bool showAllUsers, bool SendNewsletterOnly) 
		{
			if (Config.UseSingleUserBase) portalID = 0;
			
			StringBuilder select;
			select = new StringBuilder();
			select.Append(" SELECT prof.Name, usr.Email, prof.SendNewsletter");
			select.Append(", bl.Email AS Blacklisted, bl.Date, ISNULL(bl.Reason, '-') AS Reason");
            select.Append(" FROM [aspnet_Membership] usr, [aspnet_CustomProfile] prof, ");
            select.Append("[aspnet_Applications] app, [rb_Portals] por, [rb_BlackList] bl");
            select.Append(" WHERE por.PortalID = " + portalID.ToString());
            select.Append(" AND LOWER(por.PortalAlias) = app.LoweredApplicationName");
            select.Append(" AND app.ApplicationId = usr.ApplicationId");
            select.Append(" AND usr.UserId = prof.UserId");
            if (showAllUsers)
				select.Append(" AND usr.Email *= bl.Email");  // Note: the easy outer join!
			else
				select.Append(" AND usr.Email = bl.Email");
			if (SendNewsletterOnly)
				select.Append(" AND usr.SendNewsletter = 1");

            select.Append(" ORDER BY bl.Date DESC, prof.Name");

			return DBHelper.GetDataSet(select.ToString());
		}
	}
}