namespace Appleseed.Framework.DAL
{
    using Appleseed.Framework.Settings;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Class for Import/Export XML
    /// </summary>
    [History("Ashish Patel", "2014/10/02", "Import/Export XML")]
    public class XMLImportExportDB
    {
        /// <summary>
        /// To get page detail by PageID
        /// </summary>
        /// <param name="PageID">pass page id to get detail</param>
        /// /// <param name="lang">language</param>
        /// <returns>Dataset fill by StoreProcedure</returns>
        public DataSet GetPageByID(int PageID, string lang)
        {
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_XMLExportData", myConnection);
            myCommand.Parameters.Add(new SqlParameter() { ParameterName = "@PageID", Value = PageID, SqlDbType = SqlDbType.Int });
            myCommand.Parameters.Add(new SqlParameter() { ParameterName = "@PortalLanguage", Value = lang, SqlDbType = SqlDbType.NVarChar });
            myCommand.CommandType = CommandType.StoredProcedure;
            myConnection.Open();
            SqlDataAdapter dtAdpter = new SqlDataAdapter(myCommand);
            DataSet dtSet = new DataSet();
            dtAdpter.Fill(dtSet);
            myConnection.Close(); // Added By Ashish - Connection Pool Issue
            return dtSet;
        }

        /// <summary>
        /// To get page detail by PageID
        /// </summary>
        /// <param name="PortalID">pass portal id</param>
        /// <returns>Dataset fill by StoreProcedure</returns>
        public DataSet GetPageTree(int PortalID)
        {
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_GetPageTree", myConnection);
            myCommand.Parameters.Add(new SqlParameter() { ParameterName = "@PortalID", Value = PortalID, SqlDbType = SqlDbType.Int });
            myCommand.CommandType = CommandType.StoredProcedure;
            myConnection.Open();
            SqlDataAdapter dtAdpter = new SqlDataAdapter(myCommand);
            DataSet dateTree = new DataSet();
            dtAdpter.Fill(dateTree);
            myConnection.Close(); // Added By Ashish - Connection Pool Issue
            return dateTree;
        }

        /// <summary>
        /// Pass the XML TO StoreProcedure
        /// </summary>
        /// <param name="importXML">Pass xml string</param>
        /// <returns>Log table</returns>
        public SqlDataReader ImportXmlFile(string importXML)
        {
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_XMLImportData", myConnection);
            myCommand.Parameters.Add(new SqlParameter() { ParameterName = "@XMLString", Value = importXML, SqlDbType = SqlDbType.Xml });
            myCommand.CommandType = CommandType.StoredProcedure;
            myConnection.Open();
            myCommand.ExecuteNonQuery();
            return myCommand.ExecuteReader(CommandBehavior.CloseConnection);
        }
    }
}


