using System;
using System.Data;
using NUnit.Framework;
using System.Data.SqlClient;
using System.Configuration;

namespace Appleseed.Tests
{
    /// <summary>
    /// Summary description for DiagnoseEnvironment.
    /// </summary>
    [TestFixture]
    public class DiagnoseEnvironment
    {

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {

            // Set up initial database environment for testing purposes
            TestHelper.TearDownDB();
            TestHelper.RecreateDBSchema();
        }

        [Test]
        public void SimpleRun()
        {
            Console.WriteLine("This should pass. It only writes to the Console.");
        }

        [Test]
        public void AccessDatabase()
        {
            DataTable dt;
            dt = ExecuteSql("select * from UnitTest");
            Assert.IsTrue(dt.Columns.Count > 0);
        }

        [Test]
        public void RunScripts()
        {
            ExecuteSql("delete from UnitTest");
            TestHelper.RunDataScript("Test.sql");

            //check DB values:
            //check scalars:
            int intCount = Convert.ToInt32(ExecuteSql("select count(*) from unittest").Rows[0][0]);
            Assert.AreEqual(1, intCount);
            string strValue = ExecuteSql("select vchrval from unittest").Rows[0][0].ToString();
            Assert.AreEqual("DiagnosticTest1", strValue);
        }

        /// <summary>
        ///	Executes a string of SQL against the database and returns a 
        /// datatable if it was a select statement. For non-select statements, 
        /// it returns an empty datatable.
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        static internal DataTable ExecuteSql(string strSQL)
        {
            DataTable dt;
            SqlConnection myConnection = null;

            try
            {
                // 1. Create a connection
                string strDbCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                myConnection = new SqlConnection(strDbCon);
                myConnection.Open();

                // 2. Create a command object for the query
                SqlCommand myCommand = new SqlCommand(strSQL, myConnection);

                // 3. Create/Populate the DataSet
                dt = new DataTable();
                SqlDataAdapter myAdapter = new SqlDataAdapter(myCommand);
                myAdapter.Fill(dt);
            }
            finally
            {
                if (myConnection != null)
                    myConnection.Close();
            }

            return dt;

        }
    }
}
