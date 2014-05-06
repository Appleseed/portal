// ===============================================================================
// 
// This code has been generated by KickStarter :- http://kickstarter.net
// 
// Version :- 2.0.24.14705
// Date    :- 16/09/2003
// Time    :- 14.32
// 
// ===============================================================================
// 
// Copyright (C) 2002 - 2003 KickStarter
// 
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR
// FITNESS FOR A PARTICULAR PURPOSE.
// 
// ===============================================================================
using System.Data.SqlClient;

namespace Appleseed.KickStarter.CommonClasses {
	/// <summary>
	///     
	/// </summary>
	/// <remarks>
	///     
	/// </remarks>
	public class Error 
	{
        /// <summary>
        /// SQLs the message.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <param name="Object">The object.</param>
        /// <returns>A string value...</returns>
		public static string SqlMessage(SqlException e, string Object) 
		{
			
			switch (e.Number) {
				case 2627 :
				case 2601 :
					return "You cant add a duplicate record.";

				case 50000 :
					return e.Message;

				default :
					if (e.Errors[e.Errors.Count - 1].Number == 50000) {
						return e.Errors[e.Errors.Count - 1].Message;
					}

					return e.Number.ToString() + " : " + e.Message;
			}
		}
	}
}