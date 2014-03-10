// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DynamicModel.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   A class that wraps your database table in Dynamic Funtime
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Massive
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.Common;
    using System.Dynamic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// A class that wraps your database table in Dynamic Funtime
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class DynamicModel
    {
        #region Constants and Fields

        /// <summary>
        ///   The _connection string.
        /// </summary>
        private readonly string connectionString;

        /// <summary>
        ///   The _factory.
        /// </summary>
        private readonly DbProviderFactory factory;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicModel"/> class.
        /// </summary>
        /// <param name="connectionStringName">
        /// Name of the connection string.
        /// </param>
        /// <param name="tableName">
        /// Name of the table.
        /// </param>
        /// <param name="tableNamePrefix">
        /// The table name prefix.
        /// </param>
        /// <param name="primaryKeyField">
        /// The primary key field.
        /// </param>
        /// <remarks>
        /// </remarks>
        public DynamicModel(
            string connectionStringName = "", 
            string tableName = "", 
            string tableNamePrefix = "", 
            string primaryKeyField = "")
        {
            this.TableName = tableName == string.Empty
                                 ? tableNamePrefix + this.GetType().Name
                                 : tableNamePrefix + tableName;
            this.PrimaryKeyField = string.IsNullOrEmpty(primaryKeyField) ? "ID" : primaryKeyField;
            if (connectionStringName == string.Empty)
            {
                connectionStringName = ConfigurationManager.ConnectionStrings[0].Name;
            }

            var providerName = "System.Data.SqlClient";
            if (ConfigurationManager.ConnectionStrings[connectionStringName] != null)
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.ConnectionStrings[connectionStringName].ProviderName))
                {
                    providerName = ConfigurationManager.ConnectionStrings[connectionStringName].ProviderName;
                }
            }
            else
            {
                throw new InvalidOperationException(
                    string.Format("Can't find a connection string with the name '{0}'", connectionStringName));
            }

            this.factory = DbProviderFactories.GetFactory(providerName);
            this.connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the primary key field.
        /// </summary>
        /// <value>The primary key field.</value>
        /// <remarks>
        /// </remarks>
        public string PrimaryKeyField { get; set; }

        /// <summary>
        ///   Gets or sets the name of the table.
        /// </summary>
        /// <value>The name of the table.</value>
        /// <remarks>
        /// </remarks>
        public string TableName { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns all records complying with the passed-in WHERE clause and arguments,
        ///   ordered as specified, limited (TOP) by limit.
        /// </summary>
        /// <param name="where">
        /// The where.
        /// </param>
        /// <param name="orderBy">
        /// The order by.
        /// </param>
        /// <param name="limit">
        /// The limit.
        /// </param>
        /// <param name="columns">
        /// The columns.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <returns>
        /// </returns>
        /// <remarks>
        /// </remarks>
        public IEnumerable<object> All(
            string where = "", 
            string orderBy = "", 
            int limit = 0, 
            string columns = "*", 
            params object[] args)
        {
            var sql = limit > 0 ? string.Format("SELECT TOP {0} {{0}} FROM {{1}} ", limit) : "SELECT {0} FROM {1} ";
            if (!string.IsNullOrEmpty(where))
            {
                sql += where.Trim().StartsWith("where", StringComparison.CurrentCultureIgnoreCase)
                           ? where
                           : "WHERE " + where;
            }

            if (!String.IsNullOrEmpty(orderBy))
            {
                sql += orderBy.Trim().StartsWith("order by", StringComparison.CurrentCultureIgnoreCase)
                           ? orderBy
                           : " ORDER BY " + orderBy;
            }

            return this.Query(string.Format(sql, columns, this.TableName), args);
        }

        /// <summary>
        /// Builds a set of Insert and Update commands based on the passed-on objects.
        ///   These objects can be POCOs, Anonymous, NameValueCollections, or Expandos. Objects
        ///   With a PK property (whatever PrimaryKeyField is set to) will be created at UPDATEs
        /// </summary>
        /// <param name="things">
        /// The things.
        /// </param>
        /// <returns>
        /// </returns>
        /// <remarks>
        /// </remarks>
        public List<DbCommand> BuildCommands(params object[] things)
        {
            return
                things.Select(
                    item =>
                    this.HasPrimaryKey(item)
                        ? this.CreateUpdateCommand(item, this.GetPrimaryKey(item))
                        : this.CreateInsertCommand(item)).ToList();
        }

        /// <summary>
        /// Removes one or more records from the DB according to the passed-in WHERE
        /// </summary>
        /// <param name="where">
        /// The where.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <returns>
        /// </returns>
        /// <remarks>
        /// </remarks>
        public DbCommand CreateDeleteCommand(string where = "", object key = null, params object[] args)
        {
            var sql = string.Format("DELETE FROM {0} ", this.TableName);
            if (key != null)
            {
                sql += string.Format("WHERE {0}=@0", this.PrimaryKeyField);
                args = new[] { key };
            }
            else if (!string.IsNullOrEmpty(where))
            {
                sql += where.Trim().StartsWith("where", StringComparison.CurrentCultureIgnoreCase)
                           ? where
                           : "WHERE " + where;
            }

            return this.CreateCommand(sql, null, args);
        }

        /// <summary>
        /// Creates a command for use with transactions - internal stuff mostly, but here for you to play with
        /// </summary>
        /// <param name="o">
        /// The o.
        /// </param>
        /// <returns>
        /// </returns>
        /// <remarks>
        /// </remarks>
        public DbCommand CreateInsertCommand(object o)
        {
            var expando = o.ToExpando();
            var settings = (IDictionary<string, object>)expando;
            var sbKeys = new StringBuilder();
            var sbVals = new StringBuilder();
            const string Stub = "INSERT INTO {0} ({1}) \r\n VALUES ({2})";
            var result = this.CreateCommand(Stub, null);
            var counter = 0;
            foreach (var item in settings)
            {
                sbKeys.AppendFormat("{0},", item.Key);
                sbVals.AppendFormat("@{0},", counter);
                result.AddParam(item.Value);
                counter++;
            }

            if (counter > 0)
            {
                var keys = sbKeys.ToString().Substring(0, sbKeys.Length - 1);
                var vals = sbVals.ToString().Substring(0, sbVals.Length - 1);
                var sql = string.Format(Stub, this.TableName, keys, vals);
                result.CommandText = sql;
            }
            else
            {
                throw new InvalidOperationException(
                    "Can't parse this object to the database - there are no properties set");
            }

            return result;
        }

        /// <summary>
        /// Creates a command for use with transactions - internal stuff mostly, but here for you to play with
        /// </summary>
        /// <param name="o">
        /// The o.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// </returns>
        /// <remarks>
        /// </remarks>
        public DbCommand CreateUpdateCommand(object o, object key)
        {
            var expando = o.ToExpando();
            var settings = (IDictionary<string, object>)expando;
            var sbKeys = new StringBuilder();
            var stub = "UPDATE {0} SET {1} WHERE {2} = @{3}";
            var args = new List<object>();
            var result = this.CreateCommand(stub, null);
            var counter = 0;
            foreach (var item in settings)
            {
                var val = item.Value;
                if (!item.Key.Equals(this.PrimaryKeyField, StringComparison.CurrentCultureIgnoreCase) &&
                    item.Value != null)
                {
                    result.AddParam(val);
                    sbKeys.AppendFormat("{0} = @{1}, \r\n", item.Key, counter);
                    counter++;
                }
            }

            if (counter > 0)
            {
                // add the key
                result.AddParam(key);

                // strip the last commas
                var keys = sbKeys.ToString().Substring(0, sbKeys.Length - 4);
                result.CommandText = string.Format(stub, this.TableName, keys, this.PrimaryKeyField, counter);
            }
            else
            {
                throw new InvalidOperationException(
                    "No parsable object was sent in - could not divine any name/value pairs");
            }

            return result;
        }

        /// <summary>
        /// Removes one or more records from the DB according to the passed-in WHERE
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="where">
        /// The where.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <returns>
        /// The delete.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public int Delete(object key = null, string where = "", params object[] args)
        {
            return this.Execute(this.CreateDeleteCommand(where: where, key: key, args: args));
        }

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <returns>
        /// The execute.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public int Execute(DbCommand command)
        {
            return this.Execute(new[] { command });
        }

        /// <summary>
        /// Executes a series of DBCommands in a transaction
        /// </summary>
        /// <param name="commands">
        /// The commands.
        /// </param>
        /// <returns>
        /// The execute.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public int Execute(IEnumerable<DbCommand> commands)
        {
            var result = 0;
            using (var conn = this.OpenConnection())
            using (var tx = conn.BeginTransaction())
            {
                foreach (var cmd in commands)
                {
                    cmd.Connection = conn;
                    cmd.Transaction = tx;
                    result += cmd.ExecuteNonQuery();
                }

                tx.Commit();
            }

            return result;
        }

        /// <summary>
        /// Runs a query against the database
        /// </summary>
        /// <param name="sql">
        /// The SQL.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <returns>
        /// </returns>
        /// <remarks>
        /// </remarks>
        public IList<object> Fetch(string sql, params object[] args)
        {
            return this.Query(sql, args).ToList();
        }

        /// <summary>
        /// If the object passed in has a property with the same name as your PrimaryKeyField
        ///   it is returned here.
        /// </summary>
        /// <param name="o">
        /// The o.
        /// </param>
        /// <returns>
        /// The get primary key.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public object GetPrimaryKey(object o)
        {
            object result;
            o.ToDictionary().TryGetValue(this.PrimaryKeyField, out result);
            return result;
        }

        /// <summary>
        /// Conventionally introspects the object passed in for a field that
        ///   looks like a PK. If you've named your PrimaryKeyField, this becomes easy
        /// </summary>
        /// <param name="o">
        /// The o.
        /// </param>
        /// <returns>
        /// <c>true</c> if [has primary key] [the specified o]; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public bool HasPrimaryKey(object o)
        {
            return o.ToDictionary().ContainsKey(this.PrimaryKeyField);
        }

        /// <summary>
        /// Adds a record to the database. You can pass in an Anonymous object, an ExpandoObject,
        ///   A regular old POCO, or a NameValueColletion from a Request.Form or Request.QueryString
        /// </summary>
        /// <param name="o">
        /// The o.
        /// </param>
        /// <returns>
        /// The insert.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public object Insert(object o)
        {
            dynamic result = 0;
            using (var conn = this.OpenConnection())
            {
                var cmd = this.CreateInsertCommand(o);
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
                cmd.CommandText = "SELECT @@IDENTITY as newID";
                result = cmd.ExecuteScalar();
            }

            return result;
        }

        /// <summary>
        /// Returns and OpenConnection
        /// </summary>
        /// <returns>
        /// </returns>
        /// <remarks>
        /// </remarks>
        public DbConnection OpenConnection()
        {
            var conn = this.factory.CreateConnection();
            conn.ConnectionString = this.connectionString;
            conn.Open();
            return conn;
        }

        /// <summary>
        /// Returns a dynamic PagedResult. Result properties are Items, TotalPages, and TotalRecords.
        /// </summary>
        /// <param name="where">
        /// The where.
        /// </param>
        /// <param name="orderBy">
        /// The order by.
        /// </param>
        /// <param name="columns">
        /// The columns.
        /// </param>
        /// <param name="pageSize">
        /// Size of the page.
        /// </param>
        /// <param name="currentPage">
        /// The current page.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <returns>
        /// The paged.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public dynamic Paged(
            string where = "", 
            string orderBy = "", 
            string columns = "*", 
            int pageSize = 20, 
            int currentPage = 1, 
            params object[] args)
        {
            dynamic result = new ExpandoObject();
            var countSQL = string.Format("SELECT COUNT({0}) FROM {1}", this.PrimaryKeyField, this.TableName);
            if (String.IsNullOrEmpty(orderBy))
            {
                orderBy = this.PrimaryKeyField;
            }

            var sql =
                string.Format(
                    "SELECT {0} FROM (SELECT ROW_NUMBER() OVER (ORDER BY {2}) AS Row, {0} FROM {3}) AS Paged ", 
                    columns, 
                    pageSize, 
                    orderBy, 
                    this.TableName);
            var pageStart = (currentPage - 1) * pageSize;
            sql += string.Format(" WHERE Row >={0} AND Row <={1}", pageStart, pageStart + pageSize);
            var pagedWhere = string.Empty;
            if (!string.IsNullOrEmpty(where))
            {
                if (where.Trim().StartsWith("where", StringComparison.CurrentCultureIgnoreCase))
                {
                    pagedWhere = Regex.Replace(where, "where ", "and ", RegexOptions.IgnoreCase);
                }
            }

            sql += pagedWhere;
            countSQL += where;
            result.TotalRecords = this.Scalar(countSQL, args);
            result.TotalPages = result.TotalRecords / pageSize;
            if (result.TotalRecords % pageSize > 0)
            {
                result.TotalPages += 1;
            }

            result.Items = this.Query(string.Format(sql, columns, this.TableName), args);
            return result;
        }

        /// <summary>
        /// Enumerates the reader yielding the result - thanks to Jeroen Haegebaert
        /// </summary>
        /// <param name="sql">
        /// The SQL.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <returns>
        /// </returns>
        /// <remarks>
        /// </remarks>
        public IEnumerable<object> Query(string sql, params object[] args)
        {
            using (var conn = this.OpenConnection())
            {
                var rdr = this.CreateCommand(sql, conn, args).ExecuteReader(CommandBehavior.CloseConnection);
                while (rdr.Read())
                {
                    var e = new ExpandoObject();
                    var d = e as IDictionary<string, object>;
                    for (var i = 0; i < rdr.FieldCount; i++)
                    {
                        d.Add(rdr.GetName(i), rdr[i]);
                    }

                    yield return e;
                }
            }
        }

        /// <summary>
        /// Executes a set of objects as Insert or Update commands based on their property settings, within a transaction.
        ///   These objects can be POCOs, Anonymous, NameValueCollections, or Expandos. Objects
        ///   With a PK property (whatever PrimaryKeyField is set to) will be created at UPDATEs
        /// </summary>
        /// <param name="things">
        /// The things.
        /// </param>
        /// <returns>
        /// The save.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public int Save(params object[] things)
        {
            var commands = this.BuildCommands(things);
            return Execute(commands);
        }

        /// <summary>
        /// Returns a single result
        /// </summary>
        /// <param name="sql">
        /// The SQL.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <returns>
        /// The scalar.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public object Scalar(string sql, params object[] args)
        {
            object result;
            using (var conn = this.OpenConnection())
            {
                result = this.CreateCommand(sql, conn, args).ExecuteScalar();
            }

            return result;
        }

        /// <summary>
        /// Returns a single row from the database
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="columns">
        /// The columns.
        /// </param>
        /// <returns>
        /// The single.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public dynamic Single(object key, string columns = "*")
        {
            var sql = string.Format("SELECT {0} FROM {1} WHERE {2} = @0", columns, this.TableName, this.PrimaryKeyField);
            return this.Fetch(sql, key).FirstOrDefault();
        }

        /// <summary>
        /// Updates a record in the database. You can pass in an Anonymous object, an ExpandoObject,
        ///   A regular old POCO, or a NameValueCollection from a Request.Form or Request.QueryString
        /// </summary>
        /// <param name="o">
        /// The o.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The update.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public int Update(object o, object key)
        {
            return this.Execute(this.CreateUpdateCommand(o, key));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a DBCommand that you can use for loving your database.
        /// </summary>
        /// <param name="sql">
        /// The SQL.
        /// </param>
        /// <param name="conn">
        /// The conn.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <returns>
        /// </returns>
        /// <remarks>
        /// </remarks>
        private DbCommand CreateCommand(string sql, DbConnection conn, params object[] args)
        {
            var result = this.factory.CreateCommand();
            result.Connection = conn;
            result.CommandText = sql;
            if (args.Length > 0)
            {
                result.AddParams(args);
            }

            return result;
        }

        #endregion
    }
}