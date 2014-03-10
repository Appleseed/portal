using System;
using System.Data;
using System.Data.SqlClient;
using Appleseed.Framework.Settings;

namespace Appleseed.Framework.Content.Data
{
    /// <summary>
    /// User Defined Table module
    /// The User Defined Table allows the portal administrator to create custom tables of 
    /// information.
    /// Written by: Shaun Walker (IbuySpy Workshop)
    /// Moved into Appleseed by Jakob Hansen, hansen3000@hotmail.com
    /// </summary>
    public class UserDefinedTableDB
    {
        /// <summary>
        /// Gets the user defined fields.
        /// </summary>
        /// <param name="ModuleID">The module ID.</param>
        /// <returns></returns>
        public SqlDataReader GetUserDefinedFields(int ModuleID)
        {
            // Create Instance of Connection and Command object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_GetUserDefinedFields", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterModuleID = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
            parameterModuleID.Value = ModuleID;
            myCommand.Parameters.Add(parameterModuleID);

            // Execute the command
            myConnection.Open();
            SqlDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
            return result;
        }


        /// <summary>
        /// Gets the single user defined field.
        /// </summary>
        /// <param name="UserDefinedFieldID">The user defined field ID.</param>
        /// <returns></returns>
        public SqlDataReader GetSingleUserDefinedField(int UserDefinedFieldID)
        {
            // Create Instance of Connection and Command object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_GetSingleUserDefinedField", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterModuleID = new SqlParameter("@UserDefinedFieldID", SqlDbType.Int, 4);
            parameterModuleID.Value = UserDefinedFieldID;
            myCommand.Parameters.Add(parameterModuleID);

            // Execute the command
            myConnection.Open();
            SqlDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
            return result;
        }


        /// <summary>
        /// Gets the single user defined row.
        /// </summary>
        /// <param name="UserDefinedRowID">The user defined row ID.</param>
        /// <param name="ModuleID">The module ID.</param>
        /// <returns></returns>
        public SqlDataReader GetSingleUserDefinedRow(int UserDefinedRowID, int ModuleID)
        {
            // Create Instance of Connection and Command object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_GetSingleUserDefinedRow", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterUserDefinedRowID = new SqlParameter("@UserDefinedRowID", SqlDbType.Int, 4);
            parameterUserDefinedRowID.Value = UserDefinedRowID;
            myCommand.Parameters.Add(parameterUserDefinedRowID);

            SqlParameter parameterModuleID = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
            parameterModuleID.Value = ModuleID;
            myCommand.Parameters.Add(parameterModuleID);

            // Execute the command
            myConnection.Open();
            SqlDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
            return result;
        }


        /// <summary>
        /// Deletes the user defined field.
        /// </summary>
        /// <param name="UserDefinedFieldID">The user defined field ID.</param>
        public void DeleteUserDefinedField(int UserDefinedFieldID)
        {
            // Create Instance of Connection and Command object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_DeleteUserDefinedField", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterUserDefinedFieldID = new SqlParameter("@UserDefinedFieldID", SqlDbType.Int, 4);
            parameterUserDefinedFieldID.Value = UserDefinedFieldID;
            myCommand.Parameters.Add(parameterUserDefinedFieldID);

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
        /// Adds the user defined field.
        /// </summary>
        /// <param name="ModuleID">The module ID.</param>
        /// <param name="FieldTitle">The field title.</param>
        /// <param name="Visible">if set to <c>true</c> [visible].</param>
        /// <param name="FieldType">Type of the field.</param>
        public void AddUserDefinedField(int ModuleID, string FieldTitle, bool Visible, string FieldType)
        {
            // Create Instance of Connection and Command object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_AddUserDefinedField", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterModuleID = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
            parameterModuleID.Value = ModuleID;
            myCommand.Parameters.Add(parameterModuleID);

            SqlParameter parameterFieldTitle = new SqlParameter("@FieldTitle", SqlDbType.VarChar, 50);
            parameterFieldTitle.Value = FieldTitle;
            myCommand.Parameters.Add(parameterFieldTitle);

            SqlParameter parameterVisible = new SqlParameter("@Visible", SqlDbType.Bit);
            parameterVisible.Value = Visible;
            myCommand.Parameters.Add(parameterVisible);

            SqlParameter parameterFieldType = new SqlParameter("@FieldType", SqlDbType.VarChar, 20);
            parameterFieldType.Value = FieldType;
            myCommand.Parameters.Add(parameterFieldType);

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
        /// Updates the user defined field.
        /// </summary>
        /// <param name="UserDefinedFieldID">The user defined field ID.</param>
        /// <param name="FieldTitle">The field title.</param>
        /// <param name="Visible">if set to <c>true</c> [visible].</param>
        /// <param name="FieldType">Type of the field.</param>
        public void UpdateUserDefinedField(int UserDefinedFieldID, string FieldTitle, bool Visible, string FieldType)
        {
            // Create Instance of Connection and Command object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_UpdateUserDefinedField", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterUserDefinedFieldID = new SqlParameter("@UserDefinedFieldID", SqlDbType.Int, 4);
            parameterUserDefinedFieldID.Value = UserDefinedFieldID;
            myCommand.Parameters.Add(parameterUserDefinedFieldID);

            SqlParameter parameterFieldTitle = new SqlParameter("@FieldTitle", SqlDbType.VarChar, 50);
            parameterFieldTitle.Value = FieldTitle;
            myCommand.Parameters.Add(parameterFieldTitle);

            SqlParameter parameterVisible = new SqlParameter("@Visible", SqlDbType.Bit);
            parameterVisible.Value = Visible;
            myCommand.Parameters.Add(parameterVisible);

            SqlParameter parameterFieldType = new SqlParameter("@FieldType", SqlDbType.VarChar, 20);
            parameterFieldType.Value = FieldType;
            myCommand.Parameters.Add(parameterFieldType);

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
        /// Gets the user defined rows.
        /// </summary>
        /// <param name="ModuleID">The module ID.</param>
        /// <returns></returns>
        public DataSet GetUserDefinedRows(int ModuleID)
        {
            // Create Instance of Connection and Command object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_GetUserDefinedRows", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterModuleID = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
            parameterModuleID.Value = ModuleID;
            myCommand.Parameters.Add(parameterModuleID);

            // Execute the command
            myConnection.Open();
            SqlDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            string strFields = string.Empty;
            SqlDataReader dr = GetUserDefinedFields(ModuleID);
            try
            {
                while (dr.Read())
                {
                    if (strFields.Length != 0)
                        strFields += ",";
                    strFields += dr["FieldTitle"] + "|" + dr["FieldType"];
                }
            }
            finally
            {
                dr.Close();
            }

            return
                BuildCrossTabDataSet("UserDefinedData", result, "UserDefinedRowID|Int32", strFields, "UserDefinedRowID",
                                     "FieldTitle", string.Empty, "FieldValue", string.Empty);
        }


        /// <summary>
        /// Deletes the user defined row.
        /// </summary>
        /// <param name="UserDefinedRowID">The user defined row ID.</param>
        public void DeleteUserDefinedRow(int UserDefinedRowID)
        {
            // Create Instance of Connection and Command object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_DeleteUserDefinedRow", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterUserDefinedRowID = new SqlParameter("@UserDefinedRowID", SqlDbType.Int, 4);
            parameterUserDefinedRowID.Value = UserDefinedRowID;
            myCommand.Parameters.Add(parameterUserDefinedRowID);

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
        /// Adds the user defined row.
        /// </summary>
        /// <param name="ModuleID">The module ID.</param>
        /// <param name="UserDefinedRowID">The user defined row ID.</param>
        /// <returns></returns>
        public int AddUserDefinedRow(int ModuleID, out int UserDefinedRowID)
        {
            UserDefinedRowID = 0;

            // Create Instance of Connection and Command object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_AddUserDefinedRow", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterModuleID = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
            parameterModuleID.Value = ModuleID;
            myCommand.Parameters.Add(parameterModuleID);

            SqlParameter parameterUserDefinedRowID = new SqlParameter("@UserDefinedRowID", SqlDbType.Int, 4);
            parameterUserDefinedRowID.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterUserDefinedRowID);

            myConnection.Open();
            try
            {
                myCommand.ExecuteNonQuery();
            }
            finally
            {
                myConnection.Close();
            }

            UserDefinedRowID = (int) parameterUserDefinedRowID.Value;
            return UserDefinedRowID;
        }


        /// <summary>
        /// Updates the user defined row.
        /// </summary>
        /// <param name="UserDefinedRowID">The user defined row ID.</param>
        public void UpdateUserDefinedRow(int UserDefinedRowID)
        {
            // Create Instance of Connection and Command object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_UpdateUserDefinedRow", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterUserDefinedRowID = new SqlParameter("@UserDefinedRowID", SqlDbType.Int, 4);
            parameterUserDefinedRowID.Value = UserDefinedRowID;
            myCommand.Parameters.Add(parameterUserDefinedRowID);

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
        /// Updates the user defined data.
        /// </summary>
        /// <param name="UserDefinedRowID">The user defined row ID.</param>
        /// <param name="UserDefinedFieldID">The user defined field ID.</param>
        /// <param name="FieldValue">The field value.</param>
        public void UpdateUserDefinedData(int UserDefinedRowID, int UserDefinedFieldID, string FieldValue)
        {
            // Create Instance of Connection and Command object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_UpdateUserDefinedData", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterUserDefinedRowID = new SqlParameter("@UserDefinedRowID", SqlDbType.Int, 4);
            parameterUserDefinedRowID.Value = UserDefinedRowID;
            myCommand.Parameters.Add(parameterUserDefinedRowID);

            SqlParameter parameterUserDefinedFieldID = new SqlParameter("@UserDefinedFieldID", SqlDbType.Int, 4);
            parameterUserDefinedFieldID.Value = UserDefinedFieldID;
            myCommand.Parameters.Add(parameterUserDefinedFieldID);

            SqlParameter parameterFieldValue = new SqlParameter("@FieldValue", SqlDbType.VarChar, 2000);
            parameterFieldValue.Value = FieldValue;
            myCommand.Parameters.Add(parameterFieldValue);

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
        /// Updates the user defined data.
        /// </summary>
        /// <param name="UserDefinedRowID">The user defined row ID.</param>
        /// <param name="UserDefinedFieldID">The user defined field ID.</param>
        public void UpdateUserDefinedData(int UserDefinedRowID, int UserDefinedFieldID)
        {
            // Create Instance of Connection and Command object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_UpdateUserDefinedData", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterUserDefinedRowID = new SqlParameter("@UserDefinedRowID", SqlDbType.Int, 4);
            parameterUserDefinedRowID.Value = UserDefinedRowID;
            myCommand.Parameters.Add(parameterUserDefinedRowID);

            SqlParameter parameterUserDefinedFieldID = new SqlParameter("@UserDefinedFieldID", SqlDbType.Int, 4);
            parameterUserDefinedFieldID.Value = UserDefinedFieldID;
            myCommand.Parameters.Add(parameterUserDefinedFieldID);

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
        /// Updates the user defined field order.
        /// </summary>
        /// <param name="UserDefinedFieldID">The user defined field ID.</param>
        /// <param name="Direction">The direction.</param>
        public void UpdateUserDefinedFieldOrder(int UserDefinedFieldID, int Direction)
        {
            // Create Instance of Connection and Command object
            SqlConnection myConnection = Config.SqlConnectionString;
            SqlCommand myCommand = new SqlCommand("rb_UpdateUserDefinedFieldOrder", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterUserDefinedFieldID = new SqlParameter("@UserDefinedFieldID", SqlDbType.Int, 4);
            parameterUserDefinedFieldID.Value = UserDefinedFieldID;
            myCommand.Parameters.Add(parameterUserDefinedFieldID);

            SqlParameter parameterDirection = new SqlParameter("@Direction", SqlDbType.Int, 4);
            parameterDirection.Value = Direction;
            myCommand.Parameters.Add(parameterDirection);

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


        // convert datareader to crosstab dataset
        /// <summary>
        /// Builds the cross tab data set.
        /// </summary>
        /// <param name="DataSetName">Name of the data set.</param>
        /// <param name="result">The result.</param>
        /// <param name="FixedColumns">The fixed columns.</param>
        /// <param name="VariableColumns">The variable columns.</param>
        /// <param name="KeyColumn">The key column.</param>
        /// <param name="FieldColumn">The field column.</param>
        /// <param name="FieldTypeColumn">The field type column.</param>
        /// <param name="StrValueColumn">The STR value column.</param>
        /// <param name="NumericValueColumn">The numeric value column.</param>
        /// <returns></returns>
        public DataSet BuildCrossTabDataSet(
            string DataSetName, SqlDataReader result,
            string FixedColumns,
            string VariableColumns,
            string KeyColumn,
            string FieldColumn,
            string FieldTypeColumn,
            string StrValueColumn,
            string NumericValueColumn)
        {
            string[] arrFixedColumns;
            string[] arrVariableColumns = null;
            string[] arrField;
            //string FieldName;
            string FieldType;
            int intColumn;
            int intKeyColumn;

            // create dataset
            DataSet crosstab = new DataSet(DataSetName);
            crosstab.Namespace = "NetFrameWork";


            // create table
            DataTable tab = new DataTable(DataSetName);

            // split fixed columns
            arrFixedColumns = FixedColumns.Split(",".ToCharArray());

            // add fixed columns to table
            for (intColumn = arrFixedColumns.GetLowerBound(0);
                 intColumn <= arrFixedColumns.GetUpperBound(0);
                 intColumn++)
            {
                arrField = arrFixedColumns[intColumn].Split("|".ToCharArray());
                DataColumn col = new DataColumn(arrField[0], Type.GetType("System." + arrField[1].ToString()));
                tab.Columns.Add(col);
            } // intColumn

            // split variable columns
            if (VariableColumns.Length != 0)
            {
                arrVariableColumns = VariableColumns.Split(",".ToCharArray());

                // add varible columns to table
                for (intColumn = arrVariableColumns.GetLowerBound(0);
                     intColumn <= arrVariableColumns.GetUpperBound(0);
                     intColumn++)
                {
                    arrField = arrVariableColumns[intColumn].Split("|".ToCharArray());
                    DataColumn col = null;
                    if (arrField[1].ToString() == "File" || arrField[1].ToString() == "Image")
                    {
                        col = new DataColumn(arrField[0], Type.GetType("System.String"));
                    }
                    else
                    {
                        col = new DataColumn(arrField[0], Type.GetType("System." + arrField[1].ToString()));
                    }
                    col.AllowDBNull = true;
                    tab.Columns.Add(col);
                } // intColumn
            }

            // add table to dataset
            crosstab.Tables.Add(tab);

            // add rows to table
            intKeyColumn = -1;
            DataRow row = null;
            try
            {
                while (result.Read())
                {
                    // loop using KeyColumn as control break
                    if (int.Parse(result[KeyColumn].ToString()) != intKeyColumn)
                    {
                        // add row
                        if (intKeyColumn != -1)
                        {
                            tab.Rows.Add(row);
                        }

                        // create new row
                        row = tab.NewRow();

                        // assign fixed column values
                        for (intColumn = arrFixedColumns.GetLowerBound(0);
                             intColumn <= arrFixedColumns.GetUpperBound(0);
                             intColumn++)
                        {
                            arrField = arrFixedColumns[intColumn].Split("|".ToCharArray());
                            row[arrField[0]] = result[arrField[0]];
                        } // intColumn

                        // initialize variable column values
                        if (VariableColumns.Length != 0)
                        {
                            for (intColumn = arrVariableColumns.GetLowerBound(0);
                                 intColumn <= arrVariableColumns.GetUpperBound(0);
                                 intColumn++)
                            {
                                arrField = arrVariableColumns[intColumn].Split("|".ToCharArray());
                                switch (arrField[1].ToString())
                                {
                                    case "Decimal":
                                        row[arrField[0]] = 0;
                                        break;
                                    case "String":
                                        row[arrField[0]] = string.Empty;
                                        break;
                                }
                            } // intColumn
                        }

                        intKeyColumn = int.Parse(result[KeyColumn].ToString());
                    }

                    // assign pivot column value
                    if (FieldTypeColumn.Length != 0)
                    {
                        FieldType = result[FieldTypeColumn].ToString();
                    }
                    else
                    {
                        FieldType = "String";
                    }
                    switch (FieldType)
                    {
                        case "Decimal": // decimal;
                            row[int.Parse(result[FieldColumn].ToString())] = result[NumericValueColumn];
                            break;
                        case "String": // string;
                            row[result[FieldColumn].ToString()] = result[StrValueColumn];
                            break;
                    }
                }
            }
            finally
            {
                result.Close();
            }

            // add row
            if (intKeyColumn != -1)
            {
                tab.Rows.Add(row);
            }

            // finalize dataset
            crosstab.AcceptChanges();

            // return the dataset
            return crosstab;
        }
    }
}