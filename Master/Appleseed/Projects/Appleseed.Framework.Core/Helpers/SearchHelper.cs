// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchHelper.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   SearchHelper
//   Original ideas from Jakob Hansen.
//   Reflection and pluggable interface by Manu
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Helpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Reflection;
    using System.Text;
    using System.Web;
    using System.Web.UI.WebControls;

    using Appleseed.Framework.Data;
    using Appleseed.Framework.Security;
    using Appleseed.Framework.Settings;
    using Appleseed.Framework.Web.UI.WebControls;

    /// <summary>
    /// SearchHelper
    ///   Original ideas from Jakob Hansen.
    ///   Reflection and pluggable interface by Manu
    /// </summary>
    public class SearchHelper
    {
        #region Public Methods

        /// <summary>
        /// Add most of the WHERE part to a search sql
        /// </summary>
        /// <param name="portalId">
        /// portalID
        /// </param>
        /// <param name="userId">
        /// userID
        /// </param>
        /// <param name="select">
        /// SQL string to add sql to
        /// </param>
        /// <param name="fieldName">
        /// Field to do IS NOT NULL test on
        /// </param>
        public static void AddSharedSQL(int portalId, int userId, ref StringBuilder select, string fieldName)
        {
            var context = HttpContext.Current;
            var asRoles = PortalSecurity.GetRoles();

            /*
            if (userID>-1) 
            select.Append(", rb_Roles, rb_UserRoles"); 
            select.Append(" WHERE itm." + fieldName + " IS NOT NULL"); 
            select.Append(" AND itm.ModuleID = mod.ModuleID"); 
            select.Append(" AND mod.ModuleDefID = modDef.ModuleDefID"); 
            select.Append(" AND modDef.PortalID = " + portalID.ToString()); 
            select.Append(" AND tab.PortalID = " + portalID.ToString()); 
            select.Append(" AND tab.TabID = mod.TabID"); 
*/

            if (userId > -1)
            {
                // select.Append(" AND rb_UserRoles.UserID = " + userID.ToString()); 
                // select.Append(" AND rb_UserRoles.RoleID = rb_Roles.RoleID"); 
                // select.Append(" AND rb_Roles.PortalID = " + portalID.ToString()); 
                select.Append(" AND ((mod.AuthorizedViewRoles LIKE '%All Users%') ");

                // - no tenia en cuenta el rol "Authenticated users" 
                select.Append(
                    context.Request.IsAuthenticated
                        ? " OR (mod.AuthorizedViewRoles LIKE '%Authenticated Users%')"
                        : " OR (mod.AuthorizedViewRoles LIKE '%Unauthenticated Users%')");

                foreach (var sRole in asRoles)
                {
                    select.AppendFormat(" OR (mod.AuthorizedViewRoles LIKE '%{0}%')", sRole.Name);
                }

                select.Append(")");
                select.Append(" AND ((tab.AuthorizedRoles LIKE '%All Users%')");
                if (context.Request.IsAuthenticated)
                {
                    // - no tenia en cuenta el rol "Authenticated users" 
                    select.Append(" OR (tab.AuthorizedRoles LIKE '%Authenticated Users%')");
                }

                foreach (var sRole in asRoles)
                {
                    select.AppendFormat(" OR (tab.AuthorizedRoles LIKE '%{0}%')", sRole.Name);
                }

                select.Append(")");
            }
            else
            {
                select.Append(" AND (mod.AuthorizedViewRoles LIKE '%All Users%')");
                select.Append(" AND (tab.AuthorizedRoles LIKE '%All Users%')");
            }
        }

        /// <summary>
        /// Adds the modules that can be searched to the dropdown list.
        ///   First entry is always the "All"
        /// </summary>
        /// <param name="portalId">
        /// The PortalID
        /// </param>
        /// <param name="ddList">
        /// The DropDown List
        /// </param>
        public static void AddToDropDownList(int portalId, ref DropDownList ddList)
        {
            using (var dataReader = GetSearchableModules(portalId))
            {
                ddList.Items.Add(new ListItem(General.GetString("ALL", "All", null), string.Empty));

                // TODO JLH!! Should be read from context!!
                try
                {
                    while (dataReader.Read())
                    {
                        ddList.Items.Add(
                            new ListItem(
                                dataReader["FriendlyName"].ToString(), 
                                string.Format("{0};{1}", dataReader["AssemblyName"], dataReader["ClassName"])));
                    }
                }
                finally
                {
                }
            }
        }

        // old search
        /* public static void AddSharedSQL(int portalID, int userID, ref StringBuilder select, string fieldName)
        {

            if (userID>-1)
                select.Append(", rb_Roles, rb_UserRoles");
            select.Append(" WHERE itm." + fieldName + " IS NOT NULL");
            select.Append(" AND itm.ModuleID = mod.ModuleID");
            select.Append(" AND mod.ModuleDefID = modDef.ModuleDefID");
            select.Append(" AND modDef.PortalID = " + portalID.ToString());
            select.Append(" AND tab.PortalID = " + portalID.ToString());
            select.Append(" AND tab.TabID = mod.TabID");

            if (userID>-1)
            {
                select.Append(" AND rb_UserRoles.UserID = " + userID.ToString());
                select.Append(" AND rb_UserRoles.RoleID = rb_Roles.RoleID");
                select.Append(" AND rb_Roles.PortalID = " + portalID.ToString());
                select.Append(" AND ((mod.AuthorizedViewRoles LIKE '%All Users%') OR (mod.AuthorizedViewRoles LIKE '%'+rb_Roles.RoleName+'%'))");
                select.Append(" AND ((tab.AuthorizedRoles LIKE '%All Users%') OR (tab.AuthorizedRoles LIKE '%'+rb_Roles.RoleName+'%'))");
            }

            else
            {
                select.Append(" AND (mod.AuthorizedViewRoles LIKE '%All Users%')");
                select.Append(" AND (tab.AuthorizedRoles LIKE '%All Users%')");
            }
        } */

        /// <summary>
        /// Creates search sql to the WHERE clause
        /// </summary>
        /// <param name="arrFields">
        /// Array of fieldnames to search e.g. ArrayList("fld1", "fld2")
        /// </param>
        /// <param name="strSearchwords">
        /// Whatever words the user entered to perform the search with e.g. "smukke jakob". Note: Must be seperated by spaces!
        /// </param>
        /// <param name="useAnd">
        /// Controls wether all or only one word in the searchstring strSearchwords should result in a hit.
        /// </param>
        /// <returns>
        /// (fld1 LIKE '%smukke%' OR fld2 LIKE '%smukke%') AND (fld1 LIKE '%jakob%' OR fld2 LIKE '%jakob%')
        /// </returns>
        public static string CreateTestSQL(ArrayList arrFields, string strSearchwords, bool useAnd)
        {
            var i = 0;
            var j = 0;
            string strWord;
            string strBoolOp;
            const string VbCrLf = "\r\n";

            strBoolOp = useAnd ? "AND" : "OR";

            var strTmp = string.Empty;

            if (strSearchwords.IndexOf('"') > -1)
            {
                MarkPhrase(ref strSearchwords);
            }

            strSearchwords = strSearchwords.Replace(" +", " ");
            var arrWord = strSearchwords.Split(' ');

            foreach (var strItem in arrWord)
            {
                strWord = strItem;
                strWord = strWord.Replace('=', ' '); // dephrase!

                string strNot;
                if (strWord.StartsWith("-"))
                {
                    strNot = "NOT";
                    strWord = strWord.Substring(1);
                }
                else
                {
                    strNot = string.Empty;
                }

                if (strTmp == string.Empty)
                {
                    strTmp = string.Format("{0}(", strNot);
                }

                if (j > 0)
                {
                    strTmp = string.Format("{0}){1} {2} {3}(", strTmp, VbCrLf, strBoolOp, strNot);
                }

                j = j + 1;

                foreach (string strField in arrFields)
                {
                    if (i > 0)
                    {
                        strTmp = string.Format("{0} OR ", strTmp);
                    }

                    strTmp = string.Format("{0}{1} LIKE N'%{2}%'", strTmp, strField, strWord);
                    i = i + 1;
                }

                i = 0;
            }

            return string.Format("({0}))", strTmp);
        }

        /// <summary>
        /// DeleteBeforeBody
        /// </summary>
        /// <param name="strHtml">
        /// strHtml
        /// </param>
        /// <returns>
        /// The delete before body.
        /// </returns>
        public static string DeleteBeforeBody(string strHtml)
        {
            var idxBody = strHtml.ToLower().IndexOf("<body");

            // Get rid of anything before the body tag
            if (idxBody != -1)
            {
                strHtml = strHtml.Substring(idxBody);
            }

            return strHtml;
        }

        /// <summary>
        /// The GetSearchableModules function returns a list of all Searchable modules
        /// </summary>
        /// <param name="portalId">
        /// </param>
        /// <returns>
        /// </returns>
        public static SqlDataReader GetSearchableModules(int portalId)
        {
            // Create Instance of Connection and Command Object
            var connection = Config.SqlConnectionString;
            var command = new SqlCommand("rb_GetSearchableModules", connection)
                {
                    CommandType = CommandType.StoredProcedure 
                };

            // Add Parameters to SPROC
            var parameterPortalId = new SqlParameter("@PortalID", SqlDbType.Int, 4) { Value = portalId };
            command.Parameters.Add(parameterPortalId);

            // Execute the command
            connection.Open();
            var result = command.ExecuteReader(CommandBehavior.CloseConnection);

            //Added by Ashish - Connection Pool Issue
            connection.Close();

            // return the data reader 
            return result;
        }

        /// <summary>
        /// Gets the topic list.
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <returns>
        /// A string[] value...
        /// </returns>
        public static string[] GetTopicList(int portalId)
        {
            var al = new List<string> { General.GetString("PORTALSEARCH_ALL", "All", null) };
            var dr =
                DBHelper.GetDataReader(
                    string.Format("SELECT DISTINCT rb_ModuleSettings.SettingValue FROM rb_ModuleSettings INNER JOIN rb_Modules ON rb_ModuleSettings.ModuleID = rb_Modules.ModuleID INNER JOIN rb_ModuleDefinitions ON rb_Modules.ModuleDefID = rb_ModuleDefinitions.ModuleDefID WHERE (rb_ModuleDefinitions.PortalID = {0}) AND (rb_ModuleSettings.SettingName = N'TopicName') AND (rb_ModuleSettings.SettingValue <> '') ORDER BY rb_ModuleSettings.SettingValue", portalId));

            try
            {
                while (dr.Read())
                {
                    al.Add(dr["SettingValue"].ToString());
                }
            }
            finally
            {
                dr.Close(); // by Manu fix close bug #2 found
            }

            return al.ToArray();
        }

        /// <summary>
        /// A phrase is marked with '"' as a stating and ending character.
        ///   If a phrase is discovered the '"' is replaced with '='.
        /// </summary>
        /// <param name="strWords">
        /// The staring of words that might contain a phrase
        /// </param>
        public static void MarkPhrase(ref string strWords)
        {
            var idxStartQuote = strWords.IndexOf('"');
            var idxEndQuote = strWords.IndexOf('"', idxStartQuote + 1);

            if (idxEndQuote == -1)
            {
                return;
            }

            string phrase = strWords.Substring(idxStartQuote + 1, (idxEndQuote - idxStartQuote) - 1);
            phrase = phrase.Replace(' ', '=');
            strWords = strWords.Substring(0, idxStartQuote) + phrase + strWords.Substring(idxEndQuote + 1);
        }

        /// <summary>
        /// Searches the portal.
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <param name="userId">
        /// The user ID.
        /// </param>
        /// <param name="searchModule">
        /// The search module.
        /// </param>
        /// <param name="searchString">
        /// The search string.
        /// </param>
        /// <param name="searchField">
        /// The search field.
        /// </param>
        /// <param name="sortField">
        /// The sort field.
        /// </param>
        /// <param name="sortDirection">
        /// The sort direction.
        /// </param>
        /// <param name="topicName">
        /// Name of the topic.
        /// </param>
        /// <param name="addExtraSql">
        /// The add extra SQL.
        /// </param>
        /// <returns>
        /// A System.Data.SqlClient.SqlDataReader value...
        /// </returns>
        public static SqlDataReader SearchPortal(
            int portalId, 
            Guid userId, 
            string searchModule, 
            string searchString, 
            string searchField, 
            string sortField, 
            string sortDirection, 
            string topicName, 
            string addExtraSql)
        {
            var select = new StringBuilder(string.Empty, 2048);
            searchString = searchString.Replace('?', '_');
            searchString = searchString.Replace(" AND ", " +");
            searchString = searchString.Replace(" NOT ", " -");
            searchString = searchString.Replace(" OR ", " ");
            searchString = searchString.Replace(" NEAR ", " ");

            if (addExtraSql.Length != 0)
            {
                searchString = searchString == string.Empty
                                   ? string.Format("AddExtraSQL:{0}", addExtraSql)
                                   : string.Format("AddExtraSQL:{0} SearchString:{1}", addExtraSql, searchString);
            }

            // TODO: This should be cached
            // move the code in a self cached property...
            var modulesList = new StringBuilder();

            if (searchModule == string.Empty)
            {
                // Search all
                using (var dataReader = GetSearchableModules(portalId))
                {
                    try
                    {
                        while (dataReader.Read())
                        {
                            modulesList.Append(dataReader["AssemblyName"].ToString());
                            modulesList.Append(";");
                            modulesList.Append(dataReader["ClassName"].ToString());
                            modulesList.Append(";");
                        }
                    }
                    finally
                    {
                    }
                }
            }
            else
            {
                modulesList.Append(searchModule);
                modulesList.Append(";");
            }

            // Builds searchable modules list (Assembly, Class)
            var arrModulesList = modulesList.ToString().TrimEnd(';').Split(';');

            // Cycle through modules and get search string
            var slqSelectQueries = new ArrayList();
            var modulesCount = arrModulesList.GetUpperBound(0);

            for (var i = 0; i <= modulesCount; i = i + 2)
            {
                // Use reflection

                // http://sourceforge.net/tracker/index.php?func=detail&aid=899424&group_id=66837&atid=515929
                var assemblyName = Path.WebPathCombine(Path.ApplicationRoot, "bin", arrModulesList[i]);
                assemblyName = HttpContext.Current.Server.MapPath(assemblyName);
                var a = Assembly.LoadFrom(assemblyName);

                // ISearchable
                var p = (ISearchable)a.CreateInstance(arrModulesList[i + 1]);

                if (p != null)
                {
                    // Call a method with arguments
                    var args = new object[] { portalId, userId, searchString, searchField };
                    var currentSearch =
                        (string)
                        p.GetType().InvokeMember(
                            "SearchSqlSelect", BindingFlags.Default | BindingFlags.InvokeMethod, null, p, args);

                    if (currentSearch.Length != 0)
                    {
                        slqSelectQueries.Add(currentSearch);
                    }
                }
            }

            var queriesCount = slqSelectQueries.Count;

            for (var i = 0; i < queriesCount; i++)
            {
                select.Append(slqSelectQueries[i]);

                if (i < (queriesCount - 1))
                {
                    // Avoid last
                    select.Append("\r\n\r\nUNION\r\n\r\n");
                }
            }

            if (sortField.Length != 0)
            {
                if (sortDirection == string.Empty)
                {
                    if (sortField == "CreatedDate")
                    {
                        sortDirection = "DESC";
                    }
                    else
                    {
                        sortDirection = "ASC";
                    }
                }

                select.AppendFormat(" ORDER BY {0} {1}", sortField, sortDirection);
            }

            // Topic implementation
            if (topicName.Length != 0)
            {
                select.Replace(
                    "%TOPIC_PLACEHOLDER_JOIN%", "INNER JOIN rb_ModuleSettings modSet ON mod.ModuleID = modSet.ModuleID");
                select.Replace(
                    "%TOPIC_PLACEHOLDER%", 
                    string.Format(" AND (modSet.SettingName = 'TopicName' AND modSet.SettingValue='{0}')", topicName));
            }
            else
            {
                select.Replace("%TOPIC_PLACEHOLDER_JOIN%", null);
                select.Replace("%TOPIC_PLACEHOLDER%", null);
            }

            var selectSql = select.ToString();

            // string connectionString = Portal.ConnectionString;
            var connectionString = Config.ConnectionString;
            var sqlConnection = new SqlConnection(connectionString);
            var sqlCommand = new SqlCommand(selectSql, sqlConnection);
            try
            {
                sqlCommand.Connection.Open();
                return sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception e)
            {
                ErrorHandler.Publish(LogLevel.Error, string.Format("Error in Search:SearchPortal()-> {0} {1}", e, select), e);
                throw new Exception("Error in Search selection.");
            }
            //Added by Ashish - Connection Pool Issue
            finally
            {
                sqlCommand.Connection.Close();
            }
        }

        #endregion
    }
}