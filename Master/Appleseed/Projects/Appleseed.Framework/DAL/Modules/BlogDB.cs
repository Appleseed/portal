using System;
using System.Data;
using System.Data.SqlClient;
using Appleseed.Framework.Settings;

namespace Appleseed.Framework.Content.Data
{
    /// <summary>
    /// Author:					Joe Audette
    /// Created:				1/18/2004
    /// Last Modified:			2/5/2004
    /// 
    /// Class that encapsulates all data logic necessary
    /// for Blogs within the Portal database.
    /// </summary>
    public class BlogDB
    {
        /// <summary>
        /// This is used as a common setting from Blogs
        /// </summary>
        public static string ImagesSetting = "ImageCollection";

        /// <summary>
        /// The GetBlogs method returns a SqlDataReader containing all of the
        /// Blog entries for a specific blog module from the database.
        /// <a href="GetBlogs.htm" style="color:green">GetBlogs Stored Procedure</a>
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        /// <returns></returns>
        public SqlDataReader GetBlogs(int moduleID)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_BlogsGet", myConnection);

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
        /// The GetBlogsStat method returns a SqlDataReader containing all of the
        /// Blog statistics for a specific blog module from the database.
        /// <a href="GetBlogs.htm" style="color:green">GetBlogStats Stored Procedure</a>
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        /// <returns></returns>
        public SqlDataReader GetBlogStats(int moduleID)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_BlogStatsGet", myConnection);

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
        /// The GetBlogMonthArchive method returns a SqlDataReader containing all of the
        /// Blog statistics for a specific blog module from the database.
        /// <a href="GetBlogs.htm" style="color:green">GetBlogMonthArchive Stored Procedure</a>
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        /// <returns></returns>
        public SqlDataReader GetBlogMonthArchive(int moduleID)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_BlogsByMonthArchiveGet", myConnection);

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
        /// The GetBlogEntriesByMonth method returns a SqlDataReader containing all of the
        /// Blog statistics for a specific blog module from the database.
        /// <a href="GetBlogs.htm" style="color:green">GetBlogEntriesByMonth Stored Procedure</a>
        /// </summary>
        /// <param name="month">The month.</param>
        /// <param name="year">The year.</param>
        /// <param name="moduleID">The module ID.</param>
        /// <returns></returns>
        public SqlDataReader GetBlogEntriesByMonth(int month, int year, int moduleID)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_BlogsByMonthGet", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterMonth = new SqlParameter("@Month", SqlDbType.Int, 4);
            parameterMonth.Value = month;
            myCommand.Parameters.Add(parameterMonth);

            SqlParameter parameterYear = new SqlParameter("@Year", SqlDbType.Int, 4);
            parameterYear.Value = year;
            myCommand.Parameters.Add(parameterYear);

            SqlParameter parameterModuleID = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
            parameterModuleID.Value = moduleID;
            myCommand.Parameters.Add(parameterModuleID);

            myConnection.Open();
            SqlDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            return result;
        }

        /// <summary>
        /// The GetSingleBlog method returns a SqlDataReader containing details
        /// about a specific Blog from the Blogs database table.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <returns></returns>
        public SqlDataReader GetSingleBlog(int itemID)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_BlogGetSingle", myConnection);

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
        /// The GetSingleBlogWithImages method returns a SqlDataReader containing details
        /// about a specific Blog from the Blogs database table.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <param name="variation">The variation.</param>
        /// <returns></returns>
        public DataSet GetSingleBlogWithImages(int itemID, string variation)
        {
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_BlogGetSingleWithImages", myConnection);

            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
            parameterItemID.Value = itemID;
            myCommand.Parameters.Add(parameterItemID);

            SqlParameter parameterVariation = new SqlParameter("@Variation", SqlDbType.VarChar, 50);
            parameterVariation.Value = variation;
            myCommand.Parameters.Add(parameterVariation);

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = myCommand;

            DataSet ds = new DataSet();

            myConnection.Open();
            try
            {
                da.Fill(ds);
            }
            finally
            {
                myConnection.Close(); //by Manu fix close bug #2
            }
            return ds;
        }

        /// <summary>
        /// The DeleteBlog method deletes a specified Blog from
        /// the Blogs database table.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        public void DeleteBlog(int itemID)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_BlogDelete", myConnection);

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
        /// The AddBlog method adds a new Blog within the
        /// Blogs database table, and returns ItemID value as a result.
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="title">The title.</param>
        /// <param name="excerpt">The excerpt.</param>
        /// <param name="description">The description.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="isInNewsletter">if set to <c>true</c> [is in newsletter].</param>
        /// <returns></returns>
        public int AddBlog(int moduleID, string userName, string title, string excerpt, string description,
                           DateTime startDate, bool isInNewsletter)
        {
            if (userName.Length < 1)
            {
                userName = "unknown";
            }

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_BlogAdd", myConnection);

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

            SqlParameter parameterExcerpt = new SqlParameter("@Excerpt", SqlDbType.NVarChar, 512);
            parameterExcerpt.Value = excerpt;
            myCommand.Parameters.Add(parameterExcerpt);

            SqlParameter parameterDescription = new SqlParameter("@Description", SqlDbType.NText);
            parameterDescription.Value = description;
            myCommand.Parameters.Add(parameterDescription);

            SqlParameter parameterStartDate = new SqlParameter("@StartDate", SqlDbType.DateTime);
            parameterStartDate.Value = startDate;
            myCommand.Parameters.Add(parameterStartDate);

            SqlParameter parameterIsInNewsletter = new SqlParameter("@IsInNewsletter", SqlDbType.Bit);
            parameterIsInNewsletter.Value = isInNewsletter;
            myCommand.Parameters.Add(parameterIsInNewsletter);


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
        /// The UpdateBlog method updates a specified Blog within
        /// the Blogs database table.
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        /// <param name="itemID">The item ID.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="title">The title.</param>
        /// <param name="excerpt">The excerpt.</param>
        /// <param name="description">The description.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="isInNewsletter">if set to <c>true</c> [is in newsletter].</param>
        public void UpdateBlog(int moduleID, int itemID, string userName, string title, string excerpt,
                               string description, DateTime startDate, bool isInNewsletter)
        {
            if (userName.Length < 1)
            {
                userName = "unknown";
            }

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_BlogUpdate", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
            parameterItemID.Value = itemID;
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

            SqlParameter parameterExcerpt = new SqlParameter("@Excerpt", SqlDbType.NVarChar, 512);
            parameterExcerpt.Value = excerpt;
            myCommand.Parameters.Add(parameterExcerpt);

            SqlParameter parameterDescription = new SqlParameter("@Description", SqlDbType.NText);
            parameterDescription.Value = description;
            myCommand.Parameters.Add(parameterDescription);

            SqlParameter parameterStartDate = new SqlParameter("@StartDate", SqlDbType.DateTime);
            parameterStartDate.Value = startDate;
            myCommand.Parameters.Add(parameterStartDate);

            SqlParameter parameterIsInNewsletter = new SqlParameter("@IsInNewsletter", SqlDbType.Bit);
            parameterIsInNewsletter.Value = isInNewsletter;
            myCommand.Parameters.Add(parameterIsInNewsletter);

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
        /// Adds a new comment to the BlogComments database table for the Blog
        /// Entry with the given moduleID and itemID.
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        /// <param name="itemID">The item ID.</param>
        /// <param name="name">The name.</param>
        /// <param name="title">The title.</param>
        /// <param name="url">The URL.</param>
        /// <param name="comment">The comment.</param>
        public void AddBlogComment(int moduleID, int itemID, string name, string title,
                                   string url, string comment)
        {
            if (name.Length < 1)
            {
                name = "unknown";
            }
            if (title.Length > 100)
            {
                title = title.Substring(0, 100);
            }

            if (name.Length > 100)
            {
                name = name.Substring(0, 100);
            }

            if (url.Length > 200)
            {
                url = url.Substring(0, 200);
            }

            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_BlogCommentAdd", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterModuleID = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
            parameterModuleID.Value = moduleID;
            myCommand.Parameters.Add(parameterModuleID);

            SqlParameter parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
            parameterItemID.Value = itemID;
            myCommand.Parameters.Add(parameterItemID);

            SqlParameter parameterUserName = new SqlParameter("@Name", SqlDbType.NVarChar, 100);
            parameterUserName.Value = name;
            myCommand.Parameters.Add(parameterUserName);

            SqlParameter parameterTitle = new SqlParameter("@Title", SqlDbType.NVarChar, 100);
            parameterTitle.Value = title;
            myCommand.Parameters.Add(parameterTitle);

            SqlParameter parameterUrl = new SqlParameter("@URL", SqlDbType.NVarChar, 200);
            parameterUrl.Value = url;
            myCommand.Parameters.Add(parameterUrl);

            SqlParameter parameterComment = new SqlParameter("@Comment", SqlDbType.NText);
            parameterComment.Value = comment;
            myCommand.Parameters.Add(parameterComment);

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
        /// deletes a specified Blog Comment from
        /// the Blogs database table.
        /// </summary>
        /// <param name="commentID">The comment ID.</param>
        public void DeleteBlogComment(int commentID)
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_BlogCommentDelete", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterCommentID = new SqlParameter("@BlogCommentID", SqlDbType.Int, 4);
            parameterCommentID.Value = commentID;
            myCommand.Parameters.Add(parameterCommentID);

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
        /// returns a SqlDataReader containing all of the
        /// Comments for the Blog entry specified by the moduleID and itemID.
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        /// <param name="itemID">The item ID.</param>
        /// <returns></returns>
        public SqlDataReader GetBlogComments(int moduleID, int itemID)
        {
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_BlogCommentsGet", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterModuleID = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
            parameterModuleID.Value = moduleID;
            myCommand.Parameters.Add(parameterModuleID);

            SqlParameter parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
            parameterItemID.Value = itemID;
            myCommand.Parameters.Add(parameterItemID);

            myConnection.Open();
            SqlDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
            return result;
        }
    }
}