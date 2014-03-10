namespace Appleseed.Framework.Content.Data
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    using Appleseed.Framework.Settings;

    /// <summary>
    /// This class encapsulates the basic attributes of a Question, and is used
    ///     by the administration pages when manipulating questions. QuestionItem implements
    ///     the IComparable interface so that an ArrayList of QuestionItems may be sorted
    ///     by TabOrder, using the ArrayList//s Sort() method.
    /// </summary>
    public class QuestionItem : IComparable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the question id.
        /// </summary>
        public int QuestionID { get; set; }

        /// <summary>
        /// Gets or sets the question name.
        /// </summary>
        public string QuestionName { get; set; }

        /// <summary>
        /// Gets or sets the question order.
        /// </summary>
        public int QuestionOrder { get; set; }

        /// <summary>
        /// Gets or sets the type option.
        /// </summary>
        public string TypeOption { get; set; }

        #endregion

        #region Implemented Interfaces

        #region IComparable


        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <remarks>
        /// public virtual int IComparable.CompareTo:CompareTo:(object value)
        /// </remarks>
        /// <returns>Int for comparison.</returns>
        public virtual int CompareTo(object value)
        {
            if (value == null)
            {
                return 1;
            }

            var compareOrder = ((QuestionItem)value).QuestionOrder;

            if (this.QuestionOrder == compareOrder)
            {
                return 0;
            }

            if (this.QuestionOrder < compareOrder)
            {
                return -1;
            }

            if (this.QuestionOrder > compareOrder)
            {
                return 1;
            }

            return 0;
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// This class encapsulates the basic attributes of an Option, and is used
    ///     by the administration pages when manipulating questions/options.  OptionItem implements 
    ///     the IComparable interface so that an ArrayList of OptionItems may be sorted
    ///     by TabOrder, using the ArrayList//s Sort() method.
    /// </summary>
    public class OptionItem : IComparable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the option id.
        /// </summary>
        public int OptionID { get; set; }

        /// <summary>
        /// Gets or sets the option name.
        /// </summary>
        public string OptionName { get; set; }

        /// <summary>
        /// Gets or sets the option order.
        /// </summary>
        public int OptionOrder { get; set; }

        #endregion

        #region Implemented Interfaces

        #region IComparable

        /// <summary>
        /// The compare to.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The compare to.</returns>
        /// <remarks>
        /// public virtual int : IComparable.CompareTo CompareTo( object value)  // JLH!!
        /// </remarks>
        public virtual int CompareTo(object value)
        {
            if (value == null)
            {
                return 1;
            }

            var compareOrder = ((OptionItem)value).OptionOrder;

            if (this.OptionOrder == compareOrder)
            {
                return 0;
            }

            if (this.OptionOrder < compareOrder)
            {
                return -1;
            }

            if (this.OptionOrder > compareOrder)
            {
                return 1;
            }

            return 0;
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// IBS Tasks module
    ///     Class that encapsulates all data logic necessary to add/query/delete
    ///     surveys within the Portal database.
    ///     Moved into Appleseed by Jakob Hansen
    /// </summary>
    public class SurveyDB
    {
        #region Public Methods

        /// <summary>
        /// The AddAnswer method add a record in rb_SurveyAnswers table
        ///     for a specific SurveyID and QuestionID.
        ///     Other relevant sources:
        ///     rb_AddSurveyAnswer Stored Procedure
        /// </summary>
        /// <param name="surveyId">
        /// The survey ID.
        /// </param>
        /// <param name="questionId">
        /// The question ID.
        /// </param>
        /// <param name="optionId">
        /// The option ID.
        /// </param>
        /// <returns>
        /// The add answer.
        /// </returns>
        public int AddAnswer(int surveyId, int questionId, int optionId)
        {
            // Create Instance of Connection and Command object
            var connection = Config.SqlConnectionString;
            var command = new SqlCommand("rb_AddSurveyAnswer", connection) { CommandType = CommandType.StoredProcedure };

            // Add Parameters to SPROC
            var parameterSurveyID = new SqlParameter("@SurveyID", SqlDbType.Int, 4) { Value = surveyId };
            command.Parameters.Add(parameterSurveyID);

            var parameterQuestionID = new SqlParameter("@QuestionID", SqlDbType.Int, 4) { Value = questionId };
            command.Parameters.Add(parameterQuestionID);

            var parameterOptionID = new SqlParameter("@OptionID", SqlDbType.Int, 4) { Value = optionId };
            command.Parameters.Add(parameterOptionID);

            var parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int, 4)
                { Direction = ParameterDirection.Output };
            command.Parameters.Add(parameterItemID);
            connection.Open();
            try
            {
                command.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }

            return (int)parameterItemID.Value;
        }

        /// <summary>
        /// The AddOption method add a record in rb_SurveyOptions table
        ///     for a specific QuestionID.
        ///     Other relevant sources:
        ///     rb_AddSurveyOption Stored Procedure
        /// </summary>
        /// <param name="QuestionID">
        /// The question ID.
        /// </param>
        /// <param name="OptionDesc">
        /// The option desc.
        /// </param>
        /// <param name="ViewOrder">
        /// The view order.
        /// </param>
        /// <returns>
        /// The add option.
        /// </returns>
        public int AddOption(int QuestionID, string OptionDesc, int ViewOrder)
        {
            // Create Instance of Connection and Command object
            var connection = Config.SqlConnectionString;
            var command = new SqlCommand("rb_AddSurveyOption", connection);
            command.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            var parameterQuestionID = new SqlParameter("@QuestionID", SqlDbType.Int, 4);
            parameterQuestionID.Value = QuestionID;
            command.Parameters.Add(parameterQuestionID);

            var parameterOptionDesc = new SqlParameter("@OptionDesc", SqlDbType.NVarChar, 500);
            parameterOptionDesc.Value = OptionDesc;
            command.Parameters.Add(parameterOptionDesc);

            var parameterViewOrder = new SqlParameter("@ViewOrder", SqlDbType.Int, 4);
            parameterViewOrder.Value = ViewOrder;
            command.Parameters.Add(parameterViewOrder);

            var parameterOptionID = new SqlParameter("@OptionID", SqlDbType.Int, 4);
            parameterOptionID.Direction = ParameterDirection.Output;
            command.Parameters.Add(parameterOptionID);
            connection.Open();
            try
            {
                command.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }

            return (int)parameterOptionID.Value;
        }

        /// <summary>
        /// The AddQuestion method add a record in rb_SurveyQuestions table
        ///     for a specific SurveyID.
        ///     Other relevant sources:
        ///     rb_AddSurveyQuestion Stored Procedure
        /// </summary>
        /// <param name="ModuleID">
        /// The module ID.
        /// </param>
        /// <param name="Question">
        /// The question.
        /// </param>
        /// <param name="ViewOrder">
        /// The view order.
        /// </param>
        /// <param name="TypeOption">
        /// The type option.
        /// </param>
        /// <returns>
        /// The add question.
        /// </returns>
        public int AddQuestion(int ModuleID, string Question, int ViewOrder, string TypeOption)
        {
            // Create Instance of Connection and Command object
            var connection = Config.SqlConnectionString;
            var command = new SqlCommand("rb_AddSurveyQuestion", connection);
            command.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            var parameterModuleId = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
            parameterModuleId.Value = ModuleID;
            command.Parameters.Add(parameterModuleId);

            var parameterQuestion = new SqlParameter("@Question", SqlDbType.NVarChar, 500);
            parameterQuestion.Value = Question;
            command.Parameters.Add(parameterQuestion);

            var parameterViewOrder = new SqlParameter("@ViewOrder", SqlDbType.Int, 4);
            parameterViewOrder.Value = ViewOrder;
            command.Parameters.Add(parameterViewOrder);

            var parameterTypeOption = new SqlParameter("@TypeOption", SqlDbType.NVarChar, 2);
            parameterTypeOption.Value = TypeOption;
            command.Parameters.Add(parameterTypeOption);

            var parameterQuestionID = new SqlParameter("@QuestionID", SqlDbType.Int, 4);
            parameterQuestionID.Direction = ParameterDirection.Output;
            command.Parameters.Add(parameterQuestionID);
            connection.Open();
            try
            {
                command.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }

            return (int)parameterQuestionID.Value;
        }

        /// <summary>
        /// The DelOption method delete a record in rb_SurveyOptions table
        ///     for a specific OptionID.
        ///     Other relevant sources:
        ///     rb_DelSurveyOption Stored Procedure
        /// </summary>
        /// <param name="OptionID">
        /// The option ID.
        /// </param>
        /// <returns>
        /// The del option.
        /// </returns>
        public int DelOption(int OptionID)
        {
            // Create Instance of Connection and Command object
            var connection = Config.SqlConnectionString;
            var command = new SqlCommand("rb_DelSurveyOption", connection);
            command.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            var parameterOptionID = new SqlParameter("@OptionID", SqlDbType.Int, 4);
            parameterOptionID.Value = OptionID;
            command.Parameters.Add(parameterOptionID);

            connection.Open();
            try
            {
                command.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }

            return 1;
        }

        /// <summary>
        /// The DelQuestion method delete a record in rb_SurveyQuestions table
        ///     for a specific QuestionID.
        ///     Other relevant sources:
        ///     rb_DelSurveyQuestion Stored Procedure
        /// </summary>
        /// <param name="QuestionID">
        /// The question ID.
        /// </param>
        /// <returns>
        /// The del question.
        /// </returns>
        public int DelQuestion(int QuestionID)
        {
            // Create Instance of Connection and Command object
            var connection = Config.SqlConnectionString;
            var command = new SqlCommand("rb_DelSurveyQuestion", connection);
            command.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            var parameterQuestionID = new SqlParameter("@QuestionID", SqlDbType.Int, 4);
            parameterQuestionID.Value = QuestionID;
            command.Parameters.Add(parameterQuestionID);

            connection.Open();
            try
            {
                command.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }

            return 1;
        }

        /// <summary>
        /// The ExistAddSurvey method checks whether the Survey exists in rb_Surveys
        ///     table for a specific ModuleID, if not it creates a new one.
        ///     Other relevant sources:
        ///     rb_ExistAddSurvey Stored Procedure
        /// </summary>
        /// <param name="ModuleID">
        /// The module ID.
        /// </param>
        /// <param name="CreatedByUser">
        /// The created by user.
        /// </param>
        /// <returns>
        /// The exist add survey.
        /// </returns>
        public string ExistAddSurvey(int ModuleID, string CreatedByUser)
        {
            // Create Instance of Connection and Command object
            var connection = Config.SqlConnectionString;
            var command = new SqlCommand("rb_ExistAddSurvey", connection);
            command.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            var parameterModuleId = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
            parameterModuleId.Value = ModuleID;
            command.Parameters.Add(parameterModuleId);

            var parameterCreatedByUser = new SqlParameter("@CreatedByUser", SqlDbType.NVarChar, 100);
            parameterCreatedByUser.Value = CreatedByUser;
            command.Parameters.Add(parameterCreatedByUser);

            var parameterSurveyDesc = new SqlParameter("@SurveyDesc", SqlDbType.NVarChar, 500);
            parameterSurveyDesc.Direction = ParameterDirection.Output;
            command.Parameters.Add(parameterSurveyDesc);

            connection.Open();
            try
            {
                command.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }

            return (string)parameterSurveyDesc.Value;
        }

        /// <summary>
        /// The ExistSurvey method checks whether the Survey exists in rb_Surveys
        ///     table for a specific ModuleID.
        ///     Other relevant sources:
        ///     rb_ExistSurvey Stored Procedure
        /// </summary>
        /// <param name="ModuleID">
        /// The module ID.
        /// </param>
        /// <returns>
        /// The exist survey.
        /// </returns>
        public int ExistSurvey(int ModuleID)
        {
            // Create Instance of Connection and Command object
            var connection = Config.SqlConnectionString;
            var command = new SqlCommand("rb_ExistSurvey", connection);

            // Mark the Command as a SPROC
            command.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            var parameterModuleId = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
            parameterModuleId.Value = ModuleID;
            command.Parameters.Add(parameterModuleId);

            var parameterRowCount = new SqlParameter("@RowCount", SqlDbType.Int, 4);
            parameterRowCount.Direction = ParameterDirection.Output;
            command.Parameters.Add(parameterRowCount);

            connection.Open();
            try
            {
                command.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }

            return (int)parameterRowCount.Value;
        }

        /// <summary>
        /// The GetAnswerNum method get the number of answers
        ///     for a specific SurveyID and QuestionID.
        ///     Other relevant sources:
        ///     rb_GetSurveyAnswersNum Stored Procedure
        /// </summary>
        /// <param name="SurveyID">
        /// The survey ID.
        /// </param>
        /// <param name="QuestionID">
        /// The question ID.
        /// </param>
        /// <returns>
        /// The get answer num.
        /// </returns>
        public int GetAnswerNum(int SurveyID, int QuestionID)
        {
            // Create Instance of Connection and Command object
            var connection = Config.SqlConnectionString;
            var command = new SqlCommand("rb_GetSurveyAnswersNum", connection);
            command.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            var parameterSurveyID = new SqlParameter("@SurveyID", SqlDbType.Int, 4);
            parameterSurveyID.Value = SurveyID;
            command.Parameters.Add(parameterSurveyID);

            var parameterQuestionID = new SqlParameter("@QuestionID", SqlDbType.Int, 4);
            parameterQuestionID.Value = QuestionID;
            command.Parameters.Add(parameterQuestionID);

            var parameterNum = new SqlParameter("@NumAnswer", SqlDbType.Int, 4);
            parameterNum.Direction = ParameterDirection.Output;
            command.Parameters.Add(parameterNum);

            connection.Open();
            try
            {
                command.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }

            return (int)parameterNum.Value;
        }

        /// <summary>
        /// The GetAnswers method returns a SqlDataReader containing all of the
        ///     answers for a specific SurveyID.
        ///     Other relevant sources:
        ///     rb_GetSurveyAnswers Stored Procedure
        /// </summary>
        /// <param name="surveyId">
        /// The survey ID.
        /// </param>
        /// <returns>
        /// </returns>
        public SqlDataReader GetAnswers(int surveyId)
        {
            // Create Instance of Connection and Command object
            var connection = Config.SqlConnectionString;
            var command = new SqlCommand("rb_GetSurveyAnswers", connection)
                {
                   CommandType = CommandType.StoredProcedure 
                };

            // Add Parameters to SPROC
            var parameterSurveyId = new SqlParameter("@SurveyID", SqlDbType.Int, 4) { Value = surveyId };
            command.Parameters.Add(parameterSurveyId);

            // Execute the command and return the datareader 
            connection.Open();
            var result = command.ExecuteReader(CommandBehavior.CloseConnection);
            return result;
        }

        /// <summary>
        /// The GetDimArrays method get the dimensionof the arrays
        ///     for a specific ModuleID and TypeOption.
        ///     Other relevant sources:
        ///     rb_GetSurveyDimArray Stored Procedure
        /// </summary>
        /// <param name="ModuleID">
        /// The module ID.
        /// </param>
        /// <param name="TypeOption">
        /// The type option.
        /// </param>
        /// <returns>
        /// The get dim array.
        /// </returns>
        public int GetDimArray(int ModuleID, string TypeOption)
        {
            // Create Instance of Connection and Command object
            var connection = Config.SqlConnectionString;
            var command = new SqlCommand("rb_GetSurveyDimArray", connection);
            command.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            var parameterModuleId = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
            parameterModuleId.Value = ModuleID;
            command.Parameters.Add(parameterModuleId);

            var parameterTypeOption = new SqlParameter("@TypeOption", SqlDbType.NChar, 2);
            parameterTypeOption.Value = TypeOption;
            command.Parameters.Add(parameterTypeOption);

            var parameterDimArray = new SqlParameter("@DimArray", SqlDbType.Int, 4);
            parameterDimArray.Direction = ParameterDirection.Output;
            command.Parameters.Add(parameterDimArray);

            connection.Open();
            try
            {
                command.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }

            return (int)parameterDimArray.Value;
        }

        /// <summary>
        /// The GetOptionList method returns a SqlDataReader containing all of the
        ///     options for a specific QuestionID.
        ///     Other relevant sources:
        ///     GetSurveyOptionList Stored Procedure
        /// </summary>
        /// <param name="questionId">
        /// The question ID.
        /// </param>
        /// <returns>
        /// </returns>
        public SqlDataReader GetOptionList(int questionId)
        {
            // Create Instance of Connection and Command object
            var connection = Config.SqlConnectionString;
            var command = new SqlCommand("rb_GetSurveyOptionList", connection);
            command.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            var parameterQuestionId = new SqlParameter("@QuestionID", SqlDbType.Int, 4) { Value = questionId };
            command.Parameters.Add(parameterQuestionId);

            // Execute the command and return the datareader 
            connection.Open();
            var result = command.ExecuteReader(CommandBehavior.CloseConnection);
            return result;
        }

        /// <summary>
        /// The GetOptions method returns a SqlDataReader containing all of the
        ///     options for a specific portal module.
        ///     Other relevant sources:
        ///     GetSurveyOptions Stored Procedure
        /// </summary>
        /// <param name="moduleID">
        /// The module ID.
        /// </param>
        /// <param name="TypeOption">
        /// The type option.
        /// </param>
        /// <returns>
        /// </returns>
        public SqlDataReader GetOptions(int moduleID, string TypeOption)
        {
            // Create Instance of Connection and Command object
            var connection = Config.SqlConnectionString;
            var command = new SqlCommand("rb_GetSurveyOptions", connection);
            command.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            var parameterModuleId = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
            parameterModuleId.Value = moduleID;
            command.Parameters.Add(parameterModuleId);

            var parameterTypeOption = new SqlParameter("@TypeOption", SqlDbType.NVarChar, 2);
            parameterTypeOption.Value = TypeOption;
            command.Parameters.Add(parameterTypeOption);

            // Execute the command and return the datareader 
            connection.Open();
            var result = command.ExecuteReader(CommandBehavior.CloseConnection);
            return result;
        }

        /// <summary>
        /// The GetQuestionList method returns a SqlDataReader containing all of the
        ///     questions for a specific SurveyID.
        ///     Other relevant sources:
        ///     GetSurveyQuestionList Stored Procedure
        /// </summary>
        /// <param name="ModuleID">
        /// The module ID.
        /// </param>
        /// <returns>
        /// </returns>
        public SqlDataReader GetQuestionList(int ModuleID)
        {
            // Create Instance of Connection and Command object
            var connection = Config.SqlConnectionString;
            var command = new SqlCommand("rb_GetSurveyQuestionList", connection);
            command.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            var parameterModuleId = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
            parameterModuleId.Value = ModuleID;
            command.Parameters.Add(parameterModuleId);

            // Execute the command and return the datareader 
            connection.Open();
            var result = command.ExecuteReader(CommandBehavior.CloseConnection);
            return result;
        }

        /// <summary>
        /// The GetQuestions method returns a SqlDataReader containing all of the
        ///     questions for a specific portal module.
        ///     Other relevant sources:
        ///     GetSurveyQuestions Stored Procedure
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <returns>
        /// </returns>
        public SqlDataReader GetQuestions(int moduleId)
        {
            // Create Instance of Connection and Command object
            var connection = Config.SqlConnectionString;
            var command = new SqlCommand("rb_GetSurveyQuestions", connection)
                {
                   CommandType = CommandType.StoredProcedure 
                };

            // Add Parameters to SPROC
            var parameterModuleId = new SqlParameter("@ModuleID", SqlDbType.Int, 4) { Value = moduleId };
            command.Parameters.Add(parameterModuleId);

            // Execute the command and return the datareader 
            connection.Open();
            var result = command.ExecuteReader(CommandBehavior.CloseConnection);
            return result;
        }

        /// <summary>
        /// The GetSurveyID method returns the SurveyID from rb_Surveys table
        ///     for a specific ModuleID.
        ///     Other relevant sources:
        ///     rb_GetSurveyID Stored Procedure
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <returns>
        /// The get survey id.
        /// </returns>
        public int GetSurveyID(int moduleId)
        {
            // Create Instance of Connection and Command object
            var connection = Config.SqlConnectionString;
            var command = new SqlCommand("rb_GetSurveyID", connection);
            command.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            var parameterModuleId = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
            parameterModuleId.Value = moduleId;
            command.Parameters.Add(parameterModuleId);

            var parameterSurveyID = new SqlParameter("@SurveyID", SqlDbType.Int, 4);
            parameterSurveyID.Direction = ParameterDirection.Output;
            command.Parameters.Add(parameterSurveyID);

            connection.Open();
            try
            {
                command.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }

            return (int)parameterSurveyID.Value;
        }

        /// <summary>
        /// The UpdateOptionOrder method set the new ViewOrder in the
        ///     rb_SurveyOptions table for a specific OptionID.
        ///     Other relevant sources:
        ///     rb_UpdateSurveyOptionOrder Stored Procedure
        /// </summary>
        /// <param name="OptionID">
        /// The option ID.
        /// </param>
        /// <param name="Order">
        /// The order.
        /// </param>
        /// <returns>
        /// The update option order.
        /// </returns>
        public int UpdateOptionOrder(int OptionID, int Order)
        {
            // Create Instance of Connection and Command object
            var connection = Config.SqlConnectionString;
            var command = new SqlCommand("rb_UpdateSurveyOptionOrder", connection);
            command.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            var parameteroptionID = new SqlParameter("@OptionID", SqlDbType.Int, 4);
            parameteroptionID.Value = OptionID;
            command.Parameters.Add(parameteroptionID);

            var parameterOrder = new SqlParameter("@Order", SqlDbType.Int, 4);
            parameterOrder.Value = Order;
            command.Parameters.Add(parameterOrder);

            connection.Open();
            try
            {
                command.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }

            return 1;
        }

        /// <summary>
        /// The UpdateQuestionOrder method set the new ViewOrder in the
        ///     rb_SurveyQuestions table for a specific QuestionID.
        ///     Other relevant sources:
        ///     rb_UpdateSurveyQuestionOrder Stored Procedure
        /// </summary>
        /// <param name="QuestionID">
        /// The question ID.
        /// </param>
        /// <param name="Order">
        /// The order.
        /// </param>
        /// <returns>
        /// The update question order.
        /// </returns>
        public int UpdateQuestionOrder(int QuestionID, int Order)
        {
            // Create Instance of Connection and Command object
            var connection = Config.SqlConnectionString;
            var command = new SqlCommand("rb_UpdateSurveyQuestionOrder", connection);
            command.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            var parameterQuestionID = new SqlParameter("@QuestionID", SqlDbType.Int, 4);
            parameterQuestionID.Value = QuestionID;
            command.Parameters.Add(parameterQuestionID);

            var parameterOrder = new SqlParameter("@Order", SqlDbType.Int, 4);
            parameterOrder.Value = Order;
            command.Parameters.Add(parameterOrder);

            connection.Open();
            try
            {
                command.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }

            return 1;
        }

        #endregion
    }
}