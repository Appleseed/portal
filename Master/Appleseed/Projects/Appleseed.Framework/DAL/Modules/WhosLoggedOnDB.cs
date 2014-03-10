using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using Appleseed.Configuration;
using Appleseed.Settings;

namespace Appleseed.DesktopModules 
{
	/// <summary>
	/// DB Code for the Who's Logged On module
	/// Written by Paul Yarrow, paul@paulyarrow.com
	/// </summary>
	public class WhosLoggedOnDB
	{
		/// <summary>
		/// GetUsersOnline
		/// </summary>
		/// <param name="portalID"></param>
		/// <param name="minutesToCheckForUsers"></param>
		/// <param name="cacheTimeout"></param>
		/// <param name="anonUserCount"></param>
		/// <param name="regUsersOnlineCount"></param>
		/// <param name="regUsersString"></param>
		public void GetUsersOnline(	int portalID,
									int minutesToCheckForUsers,
									int cacheTimeout,
									out int anonUserCount,
									out int regUsersOnlineCount,
									out string regUsersString)
		{
			// Read from the cache if available
			if (HttpContext.Current.Cache["WhoIsOnlineAnonUserCount"] == null ||
				HttpContext.Current.Cache["WhoIsOnlineRegUserCount"] == null ||
				HttpContext.Current.Cache["WhoIsOnlineRegUsersString"] == null) 
			{
				// Firstly get the logged in users
				SqlCommand sqlComm1 = new SqlCommand();
				SqlConnection sqlConn1 = new SqlConnection(Config.ConnectionString);
				sqlComm1.Connection = sqlConn1;
				sqlComm1.CommandType = CommandType.StoredProcedure;
				sqlComm1.CommandText = "rb_GetLoggedOnUsers";
				SqlDataReader result;

				// Add Parameters to SPROC
				SqlParameter  parameterPortalID = new SqlParameter("@PortalID", SqlDbType.Int, 4);
				parameterPortalID.Value = portalID;
				sqlComm1.Parameters.Add(parameterPortalID);

				SqlParameter  parameterMinutesToCheck = new SqlParameter("@MinutesToCheck", SqlDbType.Int, 4);
				parameterMinutesToCheck.Value = minutesToCheckForUsers;
				sqlComm1.Parameters.Add(parameterMinutesToCheck);

				sqlConn1.Open();
				result = sqlComm1.ExecuteReader();

				string onlineUsers = string.Empty;
				int onlineUsersCount = 0;
				try
				{
					while (result.Read()) 
					{
						if (Convert.ToString(result.GetValue(2)) != "Logoff")
						{
							onlineUsersCount ++;
							onlineUsers += result.GetValue(1) + ", ";
						}
					}
				}
				finally
				{
					result.Close(); //by Manu, fixed bug 807858
				}

				if (onlineUsers.Length > 0)
				{
					onlineUsers = onlineUsers.Remove(onlineUsers.Length - 2, 2);
				}

				regUsersString = onlineUsers;
				regUsersOnlineCount = onlineUsersCount;

				result.Close();



				// Add Parameters to SPROC
				SqlParameter  parameterNumberOfUsers = new SqlParameter("@NoOfUsers", SqlDbType.Int, 4);
				parameterNumberOfUsers.Direction = ParameterDirection.Output;
				sqlComm1.Parameters.Add(parameterNumberOfUsers);

				// Re-use the same result set to get the no of unregistered users
				sqlComm1.CommandText =	"rb_GetNumberOfActiveUsers";
				
				// [The Bitland Prince] 8-1-2005
				// If this query generates an exception, connection might be left open
				try
				{
					sqlComm1.ExecuteNonQuery();
				}
				catch (Exception Ex)
				{
					// This takes care to close connection then throws a new
					// exception (because I don't know if it's safe to go on...)
					sqlConn1.Close();
					throw new Exception ("Unable to retrieve logged users. Error : " + Ex.Message);  
				}

				int allUsersCount = Convert.ToInt32(parameterNumberOfUsers.Value);

				sqlConn1.Close();

				anonUserCount = allUsersCount - onlineUsersCount;
				if (anonUserCount < 0)
				{
					anonUserCount = 0;
				}

				// Add to the Cache
				HttpContext.Current.Cache.Insert("WhoIsOnlineAnonUserCount", anonUserCount, null, DateTime.Now.AddMinutes(cacheTimeout), TimeSpan.Zero);
				HttpContext.Current.Cache.Insert("WhoIsOnlineRegUserCount", regUsersOnlineCount, null, DateTime.Now.AddMinutes(cacheTimeout), TimeSpan.Zero);
				HttpContext.Current.Cache.Insert("WhoIsOnlineRegUsersString", regUsersString, null, DateTime.Now.AddMinutes(cacheTimeout), TimeSpan.Zero);
			}
			else
			{
				anonUserCount = (int) HttpContext.Current.Cache["WhoIsOnlineAnonUserCount"];
				regUsersOnlineCount = (int) HttpContext.Current.Cache["WhoIsOnlineRegUserCount"];
				regUsersString = (string) HttpContext.Current.Cache["WhoIsOnlineRegUsersString"];
			}
		}
	
    
    }
}
