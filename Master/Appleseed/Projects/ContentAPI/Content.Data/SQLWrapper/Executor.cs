////////////////////////////////////////////////////
// Executor.cs
// Author: Andrey Shchurov, shchurov@gmail.com, 2005
////////////////////////////////////////////////////
#region using
using System;
using System.Reflection;
using System.Data;
using System.Data.Common;

using Debug = System.Diagnostics.Debug;
using StackTrace = System.Diagnostics.StackTrace;
#endregion

namespace Content.API.Data
{
	/// <summary>
	/// This class contains static method <see cref="ExecuteMethodAndGetResult"/> to create and execute Sql commands
	/// </summary>
	public sealed class SWExecutor
	{
		/// <summary>
		/// Private constructor. This is a static class and it cannot be created.
		/// </summary>
		private SWExecutor() 
		{
		}


		/// <summary>
		/// Executes Sql command and returns execution result. 
		/// Command text, type and parameters are taken from method using reflection.
		/// Command parameter values are taken from method parameter values.
		/// </summary>
		/// <param name="connection">Connection property.</param>
		/// <param name="transaction">Transaction property.</param>
		/// <param name="method"><see cref="MethodInfo"/> type object from which the command object is built.</param>
		/// <param name="values">Array of values for the command parameters.</param>
		/// <param name="autoCloseConnection">Determines if the connection must be closed after the command execution.</param>
		/// <returns></returns>
        public static object ExecuteMethodAndGetResult(DbConnection connection, DbTransaction transaction, MethodInfo method, object[] values, bool autoCloseConnection)
		{
			if (method == null)
			{
				// this is done because this method can be called explicitly from code.
				method = (MethodInfo) (new StackTrace().GetFrame(1).GetMethod());
			}

			// create command object
            DatabaseHelper db = new DatabaseHelper();
            DbCommand command = db.Command;
			command.Connection = connection;
			command.Transaction = transaction;


			// define default command properties (command text, command type and missing schema action)
			string commandText = method.Name;
			SWCommandType swCommandType = SWCommandType.StoredProcedure;
			MissingSchemaAction missingSchemaAction = MissingSchemaAction.Add;

			// try to get command properties from calling method attribute
			SWCommandAttribute commandAttribute = (SWCommandAttribute)Attribute.GetCustomAttribute(method, typeof(SWCommandAttribute));
			if(commandAttribute != null)
			{
				if(commandAttribute.CommandText.Length > 0)
				{
					commandText = commandAttribute.CommandText;
				}
				swCommandType = commandAttribute.CommandType;
				missingSchemaAction = commandAttribute.MissingSchemaAction;
			}

			// set command text
			command.CommandText = commandText;

			// set command type
			switch(swCommandType)
			{
				case SWCommandType.InsertUpdate: command.CommandType = CommandType.Text; break;
				case SWCommandType.StoredProcedure: command.CommandType = CommandType.StoredProcedure; break;
				case SWCommandType.Text: command.CommandType = CommandType.Text; break;
				default: command.CommandType = CommandType.Text; break;
			}

			// define command parameters.
			// In this step command text can be changed.
			int[] indexes = new int[values.Length];
			GenerateCommandParameters(command, method, values, indexes, swCommandType);


			// execute command
			object result = null;
			result = ExecuteCommand(command, method.ReturnType, autoCloseConnection, missingSchemaAction);

			// return command parameter values
			for(int i = 0; i < values.Length; ++i)
			{
				int sqlParamIndex = indexes[i];
				if(sqlParamIndex >= 0)
				{
					values[i] = command.Parameters[i].Value;
				}
			}

			// adjust null result
			if(result == null || result == System.DBNull.Value)
			{
				if(commandAttribute != null)
				{
					result = commandAttribute.ReturnIfNull;
					if(method.ReturnType == typeof(DateTime))
					{
						result = new DateTime((int)commandAttribute.ReturnIfNull);
					}
				}
			}

			return result;

		}


		/// <summary>
		/// Generates command parameters. 
		/// For some command types the command text can be changed during parameter generating.
		/// </summary>
		/// <param name="command">Command object.</param>
		/// <param name="method"><see cref="MethodInfo"/> type object</param>
		/// <param name="values">Array of values for the command parameters.</param>
		/// <param name="swCommandType"><see cref="SWCommandType"/> enumeration value</param>
		/// <param name="indexes">Array of parameter indices.</param>
		private static void GenerateCommandParameters(DbCommand command, MethodInfo method, object[] values, int[] indexes, SWCommandType swCommandType)
		{
			#region InsertUpdate parts declaration
			string sUpdatePart1 = "";
			string sUpdatePart2 = "";
			string sUpdate = "";
			string sInsertPart1 = "";
			string sInsertPart2 = "";
			string sInsert = "";
			string sAutoincrementColumnName = "";
			#endregion


            //DatabaseHelper db = new DatabaseHelper();
			ParameterInfo[] methodParameters = method.GetParameters();
            
			int sqlParamIndex = 0;
			for(int paramIndex = 0; paramIndex < methodParameters.Length; ++paramIndex)
			{
				indexes[paramIndex] = -1;
				ParameterInfo paramInfo = methodParameters[paramIndex];

				// create command parameter
                DbParameter sqlParameter = command.CreateParameter();

				// set default values
				string paramName = paramInfo.Name;
				SWParameterType paramCustType = SWParameterType.Default;
				object v = values[paramIndex];

				// get parameter attribute and set command parameter settings
				SWParameterAttribute paramAttribute = (SWParameterAttribute) Attribute.GetCustomAttribute(paramInfo, typeof(SWParameterAttribute));
				if(paramAttribute != null)
				{
					paramCustType = paramAttribute.ParameterType;

					if (paramAttribute.IsNameDefined)
						paramName = sqlParameter.ParameterName;

					if (paramAttribute.IsTypeDefined)
						sqlParameter.DbType = (DbType)paramAttribute.SqlDbType;

					if (paramAttribute.IsSizeDefined)
						sqlParameter.Size = paramAttribute.Size;

					//if (paramAttribute.IsScaleDefined)
					//	sqlParameter.Scale = paramAttribute.Scale;

					//if (paramAttribute.IsPrecisionDefined)
					//	sqlParameter..Precision = paramAttribute.Precision;

					if(CompareTreatAsNullValues(paramAttribute.TreatAsNull, v, paramInfo.ParameterType))
					{
						v = DBNull.Value;
					}

				}


				// parameter direction
				if(paramCustType == SWParameterType.SPReturnValue)
				{
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlParameter.DbType = DbType.Int32;
				}
				else if (paramInfo.ParameterType.IsByRef)
				{
					sqlParameter.Direction = paramInfo.IsOut ? ParameterDirection.Output : ParameterDirection.InputOutput;
				}
				else
				{
					sqlParameter.Direction = ParameterDirection.Input;
				}

				// generate parts of InsertUpdate expresion
				#region generate parts of InsertUpdate expresion
				if(paramCustType == SWParameterType.Identity)
				{
					if(sAutoincrementColumnName.Length > 0)
					{
						throw new SqlWrapperException("Only one identity parameter is possible");
					}
					sAutoincrementColumnName = paramName;
					Type reftype = GetRefType(paramInfo.ParameterType);
					if(reftype == null)
					{
						throw new SqlWrapperException("Identity parameter must be ByRef parameter");
					}

					// check default value
					if(paramAttribute.TreatAsNull.ToString() == SWParameterAttribute.NullReturnValueToken)
					{
						if(Convert.ToInt64(v) <= 0)
						{
							v = DBNull.Value;
						}
					}
				}

				if(swCommandType == SWCommandType.InsertUpdate)
				{
					string fieldName = "[" + paramName + "]";
					string cmdparamName = "@" + paramName;

					if(paramCustType != SWParameterType.Identity)
					{
						sInsertPart1 = AddWithDelim(sInsertPart1, ", ", fieldName);
						sInsertPart2 = AddWithDelim(sInsertPart2, ", ", cmdparamName);
					}
					if((paramCustType == SWParameterType.Key) || (paramCustType == SWParameterType.Identity))
					{
						sUpdatePart2 = AddWithDelim(sUpdatePart2, " and ", fieldName + "=" + cmdparamName);
					}
					if(paramCustType != SWParameterType.Identity)
					{
						sUpdatePart1 = AddWithDelim(sUpdatePart1, ", ", fieldName + "=" + cmdparamName);
					}
				}
				#endregion

				// set parameter name
				sqlParameter.ParameterName = "@" + paramName;


				// set parameter value
				if(v == null)
				{
					v = DBNull.Value;
				}
				sqlParameter.Value = values[paramIndex];// this is to set a proper data type
				sqlParameter.Value = v;

				// add parameter to the command object
				command.Parameters.Add(sqlParameter);
				indexes[paramIndex] = sqlParamIndex;
				sqlParamIndex++;
			}

			// in case of InsertUpdate command type compile new command text
			#region generate InsertUpdate expresion 
			if(swCommandType == SWCommandType.InsertUpdate)
			{
				string TableName = command.CommandText;
				string CommandText = "";

				if(sUpdatePart2 == "")
				{
					throw new SqlWrapperException("No Identity or Autoincrement field is defined.");
				}

				sInsert = String.Format(" insert into [{0}]({1}) values({2}) ", TableName, sInsertPart1, sInsertPart2);
				sUpdate = String.Format(" update [{0}] set {1} where {2} ", TableName, sUpdatePart1, sUpdatePart2);

				if(sAutoincrementColumnName == "")
				{
					CommandText += String.Format("{0} if (@@rowcount = 0) {1}", sUpdate, sInsert);
				}
				else
				{
					CommandText += String.Format("if(@{0} is NULL) begin {1} select @{0} = SCOPE_IDENTITY() end ", sAutoincrementColumnName, sInsert);
					CommandText += String.Format("else begin {0} end", sUpdate);
				}

				command.CommandText = CommandText;
			}
			#endregion

		}

		/// <summary>
		/// Concats two strings with a delimiter.
		/// </summary>
		/// <param name="s1">string 1</param>
		/// <param name="delim">delimiter</param>
		/// <param name="s2">string 2</param>
		/// <returns></returns>
		private static string AddWithDelim(string s1, string delim, string s2)
		{
			if(s1 == "") return s2;
			else return s1 + delim + s2;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cmd"></param>
		/// <param name="retType"></param>
		/// <param name="autoCloseConnection"></param>
		/// <returns></returns>
		internal static object ExecuteCommand(DbCommand cmd, Type retType, bool autoCloseConnection)
		{
			return ExecuteCommand(cmd, retType, autoCloseConnection, MissingSchemaAction.Add);
		}

		/// <summary>
		/// Executes a command object according to the return type.
		/// </summary>
		/// <param name="cmd">The command object.</param>
		/// <param name="retType">Return type</param>
		/// <param name="autoCloseConnection">Determines if the connection must be closed after the command execution.</param>
		/// <param name="missingSchemaAction">Determines <see cref="MissingSchemaAction"/> type value in case of filling a datasets.</param>
		/// <returns></returns>
        private static object ExecuteCommand(DbCommand cmd, Type retType, bool autoCloseConnection, MissingSchemaAction missingSchemaAction)
		{
			object result = null;

			lock(cmd.Connection)
			{
                if ((cmd.Connection.State != System.Data.ConnectionState.Open) && (cmd.Connection.State == System.Data.ConnectionState.Closed))
					cmd.Connection.Open();

				if(retType.FullName == "System.Void")
				{
					cmd.ExecuteNonQuery();
					result = null;
				}
				else if(retType == typeof(DataSet))
				{
                    result = new DatabaseHelper().ExecuteDataSet(cmd.CommandText, cmd.CommandType);
				}
				else if(retType == typeof(DataTable))
				{
					result =  new DatabaseHelper().ExecuteDataSet(cmd.CommandText, cmd.CommandType).Tables[0];
				}
                else if (retType == typeof(DbDataReader))
				{
                    DbDataReader dr = null;
					if(autoCloseConnection)
						dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
					else
						dr = cmd.ExecuteReader();

					result = dr;
				}
                else if (retType == typeof(DbDataAdapter))
				{
                    throw new System.NotImplementedException();
                    /*
                    DbDataAdapter da = new DbDataAdapter(cmd);
					da.MissingSchemaAction = missingSchemaAction;
					result = da;
                     * */
				}
				else if(retType == typeof(DbCommand))
				{
					result = cmd;
				}
				else
				{
                    result = cmd.ExecuteScalar();
				}

                if (autoCloseConnection && !(retType == typeof(DbDataReader)))
					cmd.Connection.Close();
			}

			return result;
		}

		/// <summary>
		/// Returns type from reference type.
		/// </summary>
		/// <param name="type">Reference type value.</param>
		/// <returns></returns>
		internal static Type GetRefType(Type type)
		{
			Type reftype = null;

			string typeName = type.FullName;
			if(typeName.EndsWith("&"))
			{
				reftype = Type.GetType(typeName.Substring(0, typeName.Length-1));
			}

			return reftype;
		}

		/// <summary>
		/// Compares parameter value with a value that must be treated as DBNull.
		/// </summary>
		/// <param name="TreatAsNull">The value that must be treated as DBNull</param>
		/// <param name="ParamValue">The parameter value.</param>
		/// <param name="ParamType">Type of the parameter value.</param>
		/// <returns></returns>
		private static bool CompareTreatAsNullValues(object TreatAsNull, object ParamValue, Type ParamType)
		{
			bool b = false;
			if(TreatAsNull.ToString() == SWParameterAttribute.NullReturnValueToken) return false;
			if(TreatAsNull == null) return false;
			
			if(ParamType == typeof(DateTime))
			{
				DateTime d = new DateTime((int)TreatAsNull);
				b = (d == (DateTime)ParamValue);
			}
			else if(ParamType == typeof(byte) || ParamType == typeof(int) || ParamType == typeof(long))
			{
				long v = Convert.ToInt64(TreatAsNull);
				b = v == Convert.ToInt64(ParamValue);
			}
			else if(ParamType == typeof(float) || ParamType == typeof(double))
			{
				double v = Convert.ToDouble(TreatAsNull);
				b = v == Convert.ToDouble(ParamValue);
			}
			else if(TreatAsNull.Equals(ParamValue))
			{
				b = true;
			}
			return b;
		}
	}
}



