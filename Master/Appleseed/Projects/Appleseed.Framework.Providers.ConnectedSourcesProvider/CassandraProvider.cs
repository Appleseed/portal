using Appleseed.Framework;
using Cassandra;
using System;
using System.Collections.Generic;

namespace Appleseed.Framework.Providers.ConnectedSourcesProvider
{
    public class CassandraProvider
    {
        public string ConnectHost { get; set; }
        private Cluster cluster;
        private ISession session;
        private const string KEYSPACE = "appleseed_search_engines";

        private static bool LoadedDatabase = false;

        public CassandraProvider(string connectHost)
        {
            this.InitCluster(connectHost);

            if (!LoadedDatabase)
            {
                this.LoadDatabaseSchema();
            }

            try
            {
                this.session = cluster.Connect(KEYSPACE);
            }
            catch (Exception ex)
            {
                ErrorHandler.Publish(LogLevel.Error, "CassandraProvider Error:" + ex.Message, ex);
            }
        }
        private void InitCluster(string connectHost)
        {
            try
            {
                this.ConnectHost = "localhost";
                if (!string.IsNullOrEmpty(connectHost))
                {
                    this.ConnectHost = connectHost;
                }
                this.cluster = Cluster.Builder().AddContactPoint(this.ConnectHost).Build();
            }
            catch (Exception ex)
            {
                ErrorHandler.Publish(LogLevel.Error, "CassandraProvider Error:" + ex.Message, ex);
            }
        }
        private void LoadDatabaseSchema()
        {
            try
            {
                ISession session = cluster.Connect();
                session.Execute("CREATE KEYSPACE IF NOT EXISTS " + KEYSPACE + " WITH REPLICATION = { 'class' : 'SimpleStrategy', 'replication_factor' : 3 };");
                session = cluster.Connect(KEYSPACE);
                session.Execute("CREATE TABLE IF NOT EXISTS enginetypes (id text PRIMARY KEY, name text);");
                RowSet rowsEnTypes = session.Execute("select * from enginetypes;");
                bool isIndexRwExts = false;
                bool isRssRwExts = false;
                bool isSMRwExts = false;
                foreach (Row row in rowsEnTypes)
                {
                    if (row["name"].ToString() == "index")
                    {
                        isIndexRwExts = true;
                    }
                    else if (row["name"].ToString() == "rss")
                    {
                        isRssRwExts = true;
                    }
                    else if (row["name"].ToString() == "sitemap")
                    {
                        isSMRwExts = true;
                    }
                }
                if (!isIndexRwExts)
                    session.Execute("insert into enginetypes (id, name) values ('" + Guid.NewGuid() + "','index');");
                if (!isRssRwExts)
                    session.Execute("insert into enginetypes (id, name) values ('" + Guid.NewGuid() + "','rss');");
                if (!isSMRwExts)
                    session.Execute("insert into enginetypes (id, name) values ('" + Guid.NewGuid() + "','sitemap');");
                session.Execute("CREATE TABLE IF NOT EXISTS engines (id text PRIMARY KEY, name text, typeid text);");
                session.Execute("CREATE TABLE IF NOT EXISTS engineitems (id text PRIMARY KEY, name text, engineid text, locationurl text, type text, collectionname text, indexpath text);");

                LoadedDatabase = true;
            }
            catch (Exception ex)
            {
                ErrorHandler.Publish(LogLevel.Error, "CassandraProvider Error:" + ex.Message, ex);
            }
        }

        #region Engine Types
        public List<EngineType> GetEngineTypes()
        {
            List<EngineType> engines = new List<EngineType>();
            try
            {
                session = cluster.Connect(KEYSPACE);
                RowSet rows = session.Execute("select * from enginetypes");
                foreach (Row item in rows)
                {
                    engines.Add(new EngineType() { id = Guid.Parse(item["id"].ToString()), name = item["name"].ToString() });
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Publish(LogLevel.Error, "CassandraProvider Error:" + ex.Message, ex);
            }

            return engines;
        }
        #endregion

        #region Engine
        public Engine GetEngine(Guid engineId)
        {
            Engine engine = new Engine();
            try
            {
                session = cluster.Connect(KEYSPACE);
                RowSet rows = session.Execute("select * from engines where id = '"+ engineId.ToString() + "'");
                foreach (Row item in rows)
                {
                    return (new Engine() { id = Guid.Parse(item["id"].ToString()), name = item["name"].ToString(), typeid = Guid.Parse(item["typeid"].ToString()) });
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Publish(LogLevel.Error, "CassandraProvider Error:" + ex.Message, ex);
            }

            return null;
        }
        public List<Engine> GetEngines(Guid typeId)
        {
            List<Engine> engines = new List<Engine>();
            try
            {
                session = cluster.Connect(KEYSPACE);
                RowSet rows = session.Execute("select * from engines where typeid = '"+ typeId.ToString() + "' ALLOW FILTERING;");
                foreach (Row item in rows)
                {
                    engines.Add(new Engine() { id = Guid.Parse(item["id"].ToString()), name = item["name"].ToString(), typeid = Guid.Parse(item["typeid"].ToString()) });
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Publish(LogLevel.Error, "CassandraProvider Error:" + ex.Message, ex);
            }

            return engines;
        }
        public List<Engine> GetEngines()
        {
            List<Engine> engines = new List<Engine>();
            try
            {
                session = cluster.Connect(KEYSPACE);
                RowSet rows = session.Execute("select * from engines;");
                foreach (Row item in rows)
                {
                    engines.Add(new Engine() { id = Guid.Parse(item["id"].ToString()), name = item["name"].ToString(), typeid = Guid.Parse(item["typeid"].ToString()) });
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Publish(LogLevel.Error, "CassandraProvider Error:" + ex.Message, ex);
            }

            return engines;
        }
        public Guid AddNewEngine(Engine engine)
        {
            try
            {
                var newId = Guid.NewGuid();
                session = cluster.Connect(KEYSPACE);
                session.Execute(string.Format("insert into engines (id, name, typeid) values ('{0}','{1}', '{2}');", newId.ToString(), engine.name, engine.typeid.ToString()));
                return newId;
            }
            catch (Exception ex)
            {
                ErrorHandler.Publish(LogLevel.Error, "CassandraProvider Error:" + ex.Message, ex);
            }

            return Guid.Empty;
        }

        public void UpdateEngine(Engine engine)
        {
            try
            {
                session = cluster.Connect(KEYSPACE);
                session.Execute(string.Format("update engines set name = '{0}', typeid = '{1}' where id = '{2}';", engine.name, engine.typeid.ToString(), engine.id.ToString()));
            }
            catch (Exception ex)
            {
                ErrorHandler.Publish(LogLevel.Error, "CassandraProvider Error:" + ex.Message, ex);
            }
        }

        public void DeleteEngine(Engine engine)
        {
            try
            {
                session = cluster.Connect(KEYSPACE);
                var items = GetEngineItems(engine);
                foreach (var item in items)
                {
                    DeleteEngineItem(item);
                }

                session.Execute(string.Format("delete from engines where id = '{0}';", engine.id.ToString()));
            }
            catch (Exception ex)
            {
                ErrorHandler.Publish(LogLevel.Error, "CassandraProvider Error:" + ex.Message, ex);
            }
        }
        #endregion

        #region Engine Items
        public EngineItem GetEngineItem(Guid engineItemId)
        {
            EngineItem engine = new EngineItem();
            try
            {
                session = cluster.Connect(KEYSPACE);
                RowSet rows = session.Execute("select * from engineitems where id = '"+ engineItemId.ToString() + "';");
                foreach (Row item in rows)
                {
                    return (new EngineItem() { id = Guid.Parse(item["id"].ToString()), name = item["name"].ToString(), engineid = Guid.Parse(item["engineid"].ToString()), collectionname = item["collectionname"].ToString(), indexpath = item["indexpath"].ToString(), locationurl = item["locationurl"].ToString(), type = item["type"].ToString() });
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Publish(LogLevel.Error, "CassandraProvider Error:" + ex.Message, ex);
            }

            return null;
        }
        public List<EngineItem> GetEngineItems()
        {
            List<EngineItem> engines = new List<EngineItem>();
            try
            {
                session = cluster.Connect(KEYSPACE);
                RowSet rows = session.Execute("select * from engineitems;");
                foreach (Row item in rows)
                {
                    engines.Add(new EngineItem() { id = Guid.Parse(item["id"].ToString()), name = item["name"].ToString(), engineid = Guid.Parse(item["engineid"].ToString()),  collectionname = item["collectionname"].ToString(),  indexpath = item["indexpath"].ToString(), locationurl = item["locationurl"].ToString(), type = item["type"].ToString() });
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Publish(LogLevel.Error, "CassandraProvider Error:" + ex.Message, ex);
            }

            return engines;
        }

        public List<EngineItem> GetEngineItems(Engine engine)
        {
            List<EngineItem> engines = new List<EngineItem>();
            try
            {
                session = cluster.Connect(KEYSPACE);
                RowSet rows = session.Execute("select * from engineitems where engineid = '"+ engine.id.ToString() + "' ALLOW FILTERING;");
                foreach (Row item in rows)
                {
                    engines.Add(new EngineItem() { id = Guid.Parse(item["id"].ToString()), name = item["name"].ToString(), engineid = Guid.Parse(item["engineid"].ToString()), collectionname = item["collectionname"].ToString(), indexpath = item["indexpath"].ToString(), locationurl = item["locationurl"].ToString(), type = item["type"].ToString() });
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Publish(LogLevel.Error, "CassandraProvider Error:" + ex.Message, ex);
            }

            return engines;
        }

        public Guid AddNewEngineItem(EngineItem item)
        {
            try
            {
                var newId = Guid.NewGuid();
                session = cluster.Connect(KEYSPACE);
                session.Execute(string.Format("insert into engineitems (id, name, engineid, locationurl, type, collectionname, indexpath) values ('{0}','{1}', '{2}', '{3}', '{4}', '{5}', '{6}');",
                    newId.ToString(), item.name, item.engineid.ToString(), item.locationurl, item.type, item.collectionname, item.indexpath));

                return newId;
            }
            catch (Exception ex)
            {
                ErrorHandler.Publish(LogLevel.Error, "CassandraProvider Error:" + ex.Message, ex);
            }

            return Guid.Empty;
        }

        public void UpdateEngineItem(EngineItem item)
        {
            try
            {
                session = cluster.Connect(KEYSPACE);
                session.Execute(string.Format("update engineitems set name = '{1}', engineid = '{2}', locationurl = '{3}', type = '{4}', collectionname = '{5}', indexpath = '{6}' where id = '{0}';",
                    item.id.ToString(), item.name, item.engineid.ToString(), item.locationurl, item.type, item.collectionname, item.indexpath));
            }
            catch (Exception ex)
            {
                ErrorHandler.Publish(LogLevel.Error, "CassandraProvider Error:" + ex.Message, ex);
            }
        }

        public void DeleteEngineItem(EngineItem item)
        {
            try
            {
                session = cluster.Connect(KEYSPACE);
                session.Execute(string.Format("delete from engineitems where id = '{0}';", item.id.ToString()));
            }
            catch (Exception ex)
            {
                ErrorHandler.Publish(LogLevel.Error, "CassandraProvider Error:" + ex.Message, ex);
            }
        }

        #endregion
    }
}