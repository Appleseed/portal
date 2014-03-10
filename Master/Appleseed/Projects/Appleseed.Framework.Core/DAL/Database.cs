// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Database.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   Database
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Settings
{
    using System;
    using System.Data.SqlClient;
    using System.Web;

    using Appleseed.Framework.Data;
    using Appleseed.Framework.Exceptions;

    /// <summary>
    /// The Database
    /// </summary>
    public class Database
    {
        #region Properties

        /// <summary>
        ///   Gets the database version.
        /// </summary>
        /// <value>The database version.</value>
        public static int DatabaseVersion
        {
            // by Manu 16/10/2003
            // Added 2 mods:
            // 1) Rb version is created if it is missed.
            // This is especially good for empty databases.
            // Be aware that this can break compatibility with 1613 version
            // 2) Connection problems are thrown immediately as errors.
            get
            {
                if (HttpContext.Current.Application[dbKey] == null)
                {
                    try
                    {
                        // Create rb version if it is missing
                        const string CreateRbVersions =
                            "IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N\'[rb_Versions]\') AND OBJECTPROPERTY(id, N\'IsUserTable\') = 1) " +
                            "CREATE TABLE [rb_Versions] ([Release] [int] NOT NULL , [Version] [nvarchar] (50) NULL , [ReleaseDate] [datetime] NULL ) ON [PRIMARY]";
                        DBHelper.ExeSQL(CreateRbVersions);
                    }
                    catch (SqlException ex)
                    {
                        throw new DatabaseUnreachableException(
                            "Failed to get Database Version - most likely cannot connect to db or no permission.", ex);

                        // Jes1111
                        // Appleseed.Framework.Configuration.ErrorHandler.HandleException("If this fails most likely cannot connect to db or no permission", ex);
                        // If this fails most likely cannot connect to db or no permission
                        // throw;
                    }

                    var version =
                        DBHelper.ExecuteSqlScalar<int?>("SELECT TOP 1 Release FROM rb_Versions ORDER BY Release DESC");

                    // Caches db version
                    var curVersion = version != null ? Int32.Parse(version.ToString()) : 1110;
                    HttpContext.Current.Application.Lock();
                    HttpContext.Current.Application[dbKey] = curVersion;
                    HttpContext.Current.Application.UnLock();
                }

                return (int)HttpContext.Current.Application[dbKey];
            }
        }

        /// <summary>
        ///   Gets the db key.
        /// </summary>
        /// <value>The db key.</value>
        public static string dbKey
        {
            get
            {
                var currentdatabase = "CurrentDatabase";
                if (Config.EnableMultiDbSupport)
                {
                    currentdatabase = string.Format(
                        "DatabaseVersion_{0}_{1}", 
                        Config.SqlConnectionString.DataSource, 
                        Config.SqlConnectionString.Database); // For multi-db support
                }

                return currentdatabase;
            }
        }

        #endregion
    }
}