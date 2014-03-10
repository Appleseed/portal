////////////////////////////////////////////////////
// DataAccessLayerBase.cs
// Author: Andrey Shchurov, shchurov@gmail.com, 2005
////////////////////////////////////////////////////
#region using
using System;
using System.Data;
using System.Data.Common;
//using System.Data.SqlClient;
using System.Reflection;
using System.Collections;
using System.Diagnostics;
#endregion

namespace Content.API.Data
{
	/// <summary>
	/// Summary description for DataAccessLayerBase.
	/// </summary>
	public abstract class DataAccessLayerBase : IDisposable
	{
		#region IDisposable implementation
		
		/// <summary>
		/// Disposed flag.
		/// </summary>
		protected bool m_disposed = false;

		/// <summary>
		/// Implementation of method of IDisposable interface.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Dispose method with a boolean parameter indicating the source of calling.
		/// </summary>
		/// <param name="calledbyuser">Indicates from whare the method is called.</param>
		protected void Dispose(bool calledbyuser)
		{
			if(!m_disposed)
			{
				if(calledbyuser)
				{
					InnerDispose();
				}
				m_disposed = true;
			}
		}

		/// <summary>
		/// Inner implementation of Dispose method.
		/// </summary>
		protected void InnerDispose()
		{
			if(m_connection != null)
			{
				if((m_connection.State != System.Data.ConnectionState.Closed) && m_ownsConnection) 
				{
					try
					{
						m_connection.Close();
					}
					catch{}
				}
			}
			m_connection = null;
			m_transaction = null;
			UpdateAllWrapped();
		}

		/// <summary>
		/// Class destructor.
		/// </summary>
		~DataAccessLayerBase()
		{
			Dispose(false);
		}
		#endregion

		#region private members
		/// <summary>
		/// Connection object.
		/// </summary>
        protected DbConnection m_connection = null;

		/// <summary>
		/// Transaction object.
		/// </summary>
        protected DbTransaction m_transaction = null;

		/// <summary>
		/// Indicates that <see cref="DataAccessLayerBase"/> object owns the connection.
		/// </summary>
		protected bool m_ownsConnection = false;

		/// <summary>
		/// Indicates that the connection must be closed each time after a command execution.
		/// </summary>
		protected bool m_autoCloseConnection = false;


		/// <summary>
		/// Contains objects generated in <see cref="GenerateAllWrapped"/> method.
		/// </summary>
		protected Hashtable m_swTypes = new Hashtable();


		#endregion
        private DatabaseHelper db = null;
        private DatabaseHelper DB
        {
            get
            {
                if(db == null)
                    db = new DatabaseHelper();

                return db;
            }
        }

		#region public members

		/// <summary>
		/// Executes sql string and returns DataSet object.
		/// </summary>
		/// <param name="sql">Sql string.</param>
		/// <returns></returns>
		public DataSet ExecuteDataSet(string sql)
		{
            return (DataSet)SWExecutor.ExecuteCommand(DB.Command, typeof(DataSet), m_autoCloseConnection);
		}

		/// <summary>
		/// Executes sql string and returns DataTable object.
		/// </summary>
		/// <param name="sql">Sql string.</param>
		/// <returns></returns>
		public DataTable ExecuteDataTable(string sql)
		{
            return (DataTable)SWExecutor.ExecuteCommand(DB.Command, typeof(DataTable), m_autoCloseConnection);
		}

		/// <summary>
		/// Executes sql string and returns scalar value.
		/// </summary>
		/// <param name="sql">Sql string.</param>
		/// <returns></returns>
		public object ExecuteScalar(string sql)
		{
            return SWExecutor.ExecuteCommand(DB.Command, typeof(object), m_autoCloseConnection);
		}

		/// <summary>
		/// Executes sql string and returns no result.
		/// </summary>
		/// <param name="sql">Sql string.</param>
		public void ExecuteNonQuery(string sql)
		{
            SWExecutor.ExecuteCommand(DB.Command, typeof(void), m_autoCloseConnection);
		}
		#endregion


		#region Constructors

		/// <summary>
		/// A constructor with no parameters.
		/// </summary>
		public DataAccessLayerBase()
		{
		}


		/// <summary>
		/// Initializes the object. 
		/// </summary>
		/// <param name="connection">Connection parameter.</param>
		/// <param name="autoCloseConnection">AutoCloseConnection parameter.</param>
		/// <param name="ownsConnection">OwnsConnection parameter.</param>
        public void Init(DbConnection connection, bool autoCloseConnection, bool ownsConnection)
		{
			InnerDispose();

			m_connection = connection;
			m_autoCloseConnection = autoCloseConnection;
			m_ownsConnection = ownsConnection;

			GenerateAllWrapped();
			UpdateAllWrapped();
		}

		/// <summary>
		/// Initializes the object. 
		/// </summary>
		/// <param name="connectionString">Sql connection string parameter.</param>
		/// <param name="autoCloseConnection">AutoCloseConnection parameter.</param>
		public void Init(string connectionString, bool autoCloseConnection)
		{
			Init(DB.Command.Connection, autoCloseConnection, true);
		}

		/// <summary>
		/// Generates all wrapped objects mention in public properties 
		/// and derived from <see cref="ISqlWrapperBase"/> interface.
		/// </summary>
		private void GenerateAllWrapped()
		{
			MethodInfo[] mis = this.GetType().GetMethods();
			for(int i = 0; i < mis.Length; ++i)
			{
				Type type = mis[i].ReturnType;
				if(type.GetInterface(typeof(ISqlWrapperBase).FullName) == typeof(ISqlWrapperBase))
				{
					if(mis[i].Name.StartsWith("get_"))
					{
						if(!m_swTypes.ContainsKey(mis[i].Name))
						{
							ISqlWrapperBase sw = WrapFactory.Create(type);
							m_swTypes[mis[i].Name] = sw;
						}
					}
				}
			}
		}

		#endregion

		#region Public members
		/// <summary>
		/// It true then the object owns its connection
		/// and disposes it on its own disposal.
		/// </summary>
		public bool OwnsConnection{get{return m_ownsConnection;}}

		/// <summary>
		/// If true then the object's connection is closed each time 
		/// after sql command execution.
		/// </summary>
		public bool AutoCloseConnection{get{return m_autoCloseConnection;}}

		/// <summary>
		/// Sql connection property.
		/// </summary>
		public DbConnection Connection{get{return m_connection;}}

		/// <summary>
		/// Sql transaction property.
		/// </summary>
		public DbTransaction Transaction{get{return m_transaction;}}

		/// <summary>
		/// Begins sql transaction with a default (<see cref="IsolationLevel.ReadCommitted"/>) isolation level.
		/// </summary>
		/// <returns></returns>
        public DbTransaction BeginTransaction()
		{
			return BeginTransaction(IsolationLevel.ReadCommitted);
		}

		/// <summary>
		/// Begins sql transaction with a specified isolation level.
		/// </summary>
		/// <param name="iso"></param>
		/// <returns></returns>
        public DbTransaction BeginTransaction(IsolationLevel iso)
		{
			if(m_transaction != null)
			{
				throw new ApplicationException("A previous transaction is not closed");
			}
			m_transaction = m_connection.BeginTransaction(iso);
			UpdateAllWrapped();
			return m_transaction;
		}

		/// <summary>
		/// Rolls back the current transaction.
		/// </summary>
		public void RollbackTransaction()
		{
			if(m_transaction == null)
			{
				throw new ApplicationException("A transaction has not been opened");
			}
			m_transaction.Rollback();
			m_transaction = null;
			UpdateAllWrapped();
		}

		/// <summary>
		/// Commits the current transaction.
		/// </summary>
		public void CommitTransaction()
		{
			if(m_transaction == null)
			{
				throw new ApplicationException("A transaction has not been opened");
			}
			m_transaction.Commit();
			m_transaction = null;
			UpdateAllWrapped();
		}
		#endregion
		
		#region Protected members

		/// <summary>
		/// Update values of a wrapped object.
		/// </summary>
		/// <param name="wrapped">A wrapped object.</param>
		protected virtual void UpdateWrapped(ISqlWrapperBase wrapped)
		{
			wrapped.Connection = m_connection;
			wrapped.Transaction = m_transaction;
			wrapped.AutoCloseConnection = m_autoCloseConnection;
		}

		/// <summary>
		/// Updates all generated objects properties.
		/// </summary>
		protected virtual void UpdateAllWrapped()
		{
			foreach(object sw in m_swTypes.Values)
			{
				UpdateWrapped((ISqlWrapperBase)sw);
			}
		}

		/// <summary>
		/// Returns a generated object.
		/// </summary>
		/// <returns></returns>
		protected ISqlWrapperBase GetWrapped()
		{
			MethodInfo mi = (MethodInfo)(new StackTrace().GetFrame(1).GetMethod());
			ISqlWrapperBase res = (ISqlWrapperBase)m_swTypes[mi.Name];
			if(res == null)
			{
				throw new SqlWrapperException("The object is not initialized.");
			}
			return res;
		}
		#endregion

		#region SqlWrapped Properties
		//public SqlWrapperDerived SqlWrapperDerived{get{return (SqlWrapperDerived)GetWrapped();}}
		#endregion

	}
}
