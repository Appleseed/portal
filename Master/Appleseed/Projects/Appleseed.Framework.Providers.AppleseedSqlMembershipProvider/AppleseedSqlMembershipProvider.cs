// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppleseedSqlMembershipProvider.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   SQL-specific implementation of
//   <code>
//   AppleseedMembershipProvider
//   </code>
//   API
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Providers.AppleseedMembershipProvider
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;
    using System.Web.Hosting;
    using System.Web.Security;

    using Appleseed.Framework.Providers.AppleseedSqlMembershipProvider;
    using Appleseed.Framework.Site.Data;

    /// <summary>
    /// SQL-specific implementation of 
    ///   <code>
    /// AppleseedMembershipProvider
    ///   </code>
    /// API
    /// </summary>
    public class AppleseedSqlMembershipProvider : AppleseedMembershipProvider
    {
        #region Constants and Fields

        /// <summary>
        ///   The connection string.
        /// </summary>
        protected string ConnectionString;

        /// <summary>
        ///   The p application name.
        /// </summary>
        protected string pApplicationName;

        /// <summary>
        ///   The p enable password reset.
        /// </summary>
        protected bool pEnablePasswordReset;

        /// <summary>
        ///   The p enable password retrieval.
        /// </summary>
        protected bool pEnablePasswordRetrieval;

        /// <summary>
        ///   The p max invalid password attempts.
        /// </summary>
        protected int pMaxInvalidPasswordAttempts;

        /// <summary>
        ///   The p password attempt window.
        /// </summary>
        protected int pPasswordAttemptWindow;

        /// <summary>
        ///   The p password format.
        /// </summary>
        protected MembershipPasswordFormat pPasswordFormat;

        /// <summary>
        ///   The p requires question and answer.
        /// </summary>
        protected bool pRequiresQuestionAndAnswer;

        /// <summary>
        ///   The p requires unique email.
        /// </summary>
        protected bool pRequiresUniqueEmail;

        /// <summary>
        ///   The encryption key.
        /// </summary>
        private const string EncryptionKey = "BE09F72BFF7A4566";

        /// <summary>
        ///   The error code incorrect password answer.
        /// </summary>
        private const int ErrorCodeIncorrectPasswordAnswer = 3;

        /// <summary>
        ///   The error code user locked out.
        /// </summary>
        private const int ErrorCodeUserLockedOut = 99;

        /// <summary>
        ///   The error code user not found.
        /// </summary>
        private const int ErrorCodeUserNotFound = 1;

        /// <summary>
        ///   The new password length.
        /// </summary>
        private const int NewPasswordLength = 8;

        /// <summary>
        ///   The p min required non alphanumeric characters.
        /// </summary>
        private int pMinRequiredNonAlphanumericCharacters;

        /// <summary>
        ///   The p min required password length.
        /// </summary>
        private int pMinRequiredPasswordLength;

        /// <summary>
        ///   The p password strength regular expression.
        /// </summary>
        private string pPasswordStrengthRegularExpression;

        #endregion

        #region Properties

        /// <summary>
        ///   The name of the application using the membership provider. 
        ///   ApplicationName is used to scope membership data so that applications can choose whether to share membership data with other applications. 
        ///   This property can be read and written.
        /// </summary>
        public override string ApplicationName
        {
            get
            {
                return HttpContext.Current != null
                           ? (string)HttpContext.Current.Items["Membership.ApplicationName"]
                           : this.pApplicationName;
            }

            set
            {
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Items["Membership.ApplicationName"] = value;
                }

                this.pApplicationName = value;
            }
        }

        /// <summary>
        ///   Indicates whether passwords can be reset using the provider's ResetPassword method. This property is read-only.
        /// </summary>
        public override bool EnablePasswordReset
        {
            get
            {
                return this.pEnablePasswordReset;
            }
        }

        /// <summary>
        ///   Indicates whether passwords can be retrieved using the provider's GetPassword method. This property is read-only.
        /// </summary>
        public override bool EnablePasswordRetrieval
        {
            get
            {
                return this.pEnablePasswordRetrieval;
            }
        }

        /// <summary>
        ///   Works in conjunction with PasswordAttemptWindow to provide a safeguard against password guessing. 
        ///   If the number of consecutive invalid passwords or password questions ("invalid attempts") submitted 
        ///   to the provider for a given user reaches MaxInvalidPasswordAttempts within the number of minutes specified 
        ///   by PasswordAttemptWindow, the user is locked out of the system. The user remains locked out until the 
        ///   provider's UnlockUser method is called to remove the lock.
        ///   The count of consecutive invalid attempts is incremented when an invalid password or password answer is 
        ///   submitted to the provider's ValidateUser, ChangePassword, ChangePasswordQuestionAndAnswer, GetPassword, and ResetPassword methods.
        ///   If a valid password or password answer is supplied before the MaxInvalidPasswordAttempts is reached, 
        ///   the count of consecutive invalid attempts is reset to zero. 
        ///   If the RequiresQuestionAndAnswer property is false, invalid password answer attempts are not tracked.
        ///   This property is read-only.
        /// </summary>
        public override int MaxInvalidPasswordAttempts
        {
            get
            {
                return this.pMaxInvalidPasswordAttempts;
            }
        }

        /// <summary>
        ///   The minimum number of non-alphanumeric characters required in a password. This property is read-only.
        /// </summary>
        public override int MinRequiredNonAlphanumericCharacters
        {
            get
            {
                return this.pMinRequiredNonAlphanumericCharacters;
            }
        }

        /// <summary>
        ///   The minimum number of characters required in a password. This property is read-only.
        /// </summary>
        public override int MinRequiredPasswordLength
        {
            get
            {
                return this.pMinRequiredPasswordLength;
            }
        }

        /// <summary>
        ///   For a description, see MaxInvalidPasswordAttempts. This property is read-only.
        /// </summary>
        /// <see cref = "MaxInvalidPasswordAttempts" />
        public override int PasswordAttemptWindow
        {
            get
            {
                return this.pPasswordAttemptWindow;
            }
        }

        /// <summary>
        ///   Indicates what format that passwords are stored in: clear (plaintext), encrypted, or hashed.
        ///   Clear and encrypted passwords can be retrieved; hashed passwords cannot. This property is read-only.
        /// </summary>
        public override MembershipPasswordFormat PasswordFormat
        {
            get
            {
                return this.pPasswordFormat;
            }
        }

        /// <summary>
        ///   A regular expression specifying a pattern to which passwords must conform. This property is read-only.
        /// </summary>
        public override string PasswordStrengthRegularExpression
        {
            get
            {
                return this.pPasswordStrengthRegularExpression;
            }
        }

        /// <summary>
        ///   Indicates whether a password answer must be supplied when calling the provider's GetPassword and ResetPassword methods. This property is read-only.
        /// </summary>
        public override bool RequiresQuestionAndAnswer
        {
            get
            {
                return this.pRequiresQuestionAndAnswer;
            }
        }

        /// <summary>
        ///   Indicates whether each registered user must have a unique e-mail address. This property is read-only.
        /// </summary>
        public override bool RequiresUniqueEmail
        {
            get
            {
                return this.pRequiresUniqueEmail;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether [write exceptions to event log].
        /// </summary>
        /// <value><c>true</c> if [write exceptions to event log]; otherwise, <c>false</c>.</value>
        /// <remarks>
        ///   If false, exceptions are thrown to the caller. If true,
        ///   exceptions are written to the event log.
        /// </remarks>
        public bool WriteExceptionsToEventLog { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Processes a request to update the password for a membership user.
        /// </summary>
        /// <param name="username">
        /// The user to update the password for.
        /// </param>
        /// <param name="oldPassword">
        /// The current password for the specified user.
        /// </param>
        /// <param name="newPassword">
        /// The new password for the specified user.
        /// </param>
        /// <returns>
        /// true if the password was updated successfully; otherwise, false.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            return this.ChangePassword(this.ApplicationName, username, oldPassword, newPassword);
        }

        /// <summary>
        /// Takes, as input, a user name, a password (the user's current password), and a new password and updates
        ///   the password in the membership data source.
        ///   Before changing a password, ChangePassword calls the provider's virtual OnValidatingPassword method to
        ///   validate the new password. It then changes the password or cancels the action based on the outcome of the call.
        ///   If the user name, password, new password, or password answer is not valid, ChangePassword
        ///   does not throw an exception; it simply returns false.
        ///   Following a successful password change, ChangePassword updates the user's LastPasswordChangedDate.
        /// </summary>
        /// <param name="portalAlias">
        /// Appleseed's portal alias
        /// </param>
        /// <param name="username">
        /// The user's name
        /// </param>
        /// <param name="oldPassword">
        /// The user's old password
        /// </param>
        /// <param name="newPassword">
        /// The user's new password
        /// </param>
        /// <returns>
        /// ChangePassword returns true if the password was updated successfully.
        ///   Otherwise, it returns false.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override bool ChangePassword(string portalAlias, string username, string oldPassword, string newPassword)
        {
            return this.ValidateUser(username, oldPassword) &&
                   this.ChangeUserPassword(portalAlias, username, newPassword);
        }


        /// <summary>
        /// If its and admin, he can change the password of a user
        /// </summary>
        /// <param name="username">
        /// The user's name
        /// </param>       
        /// <param name="Password">
        /// The user's new password
        /// </param>
        /// <returns>
        /// ChangePassword returns true if the password was updated successfully.
        ///     Otherwise, it returns false.
        /// </returns>
        public override bool AdminChangePassword(string username, string Password)
        {
            if (Appleseed.Framework.Security.PortalSecurity.IsInRole("Admins"))
                return this.ChangeUserPassword(this.ApplicationName, username, Password);
            else
                return false;
        }

        /// <summary>
        /// Changes the user password.
        /// </summary>
        /// <param name="username">
        /// The user username
        /// </param>
        /// <param name="tokenId">
        /// The token.
        /// </param>
        /// <param name="newPassword">
        /// The new password the user wants
        /// </param>
        /// <returns>
        /// True if the password is changed, false otherwise
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override bool ChangePassword(string username, Guid tokenId, string newPassword)
        {
            using (var entities = new AppleseedMembershipEntities(ConfigurationManager.ConnectionStrings["AppleseedMembershipEntities"].ConnectionString))
            {
                var token =
                    entities.aspnet_ResetPasswordTokens.Include("aspnet_Membership").FirstOrDefault(
                        t =>
                        t.TokenId == tokenId && t.aspnet_Membership.aspnet_Users.LoweredUserName == username.ToLower() &&
                        t.aspnet_Membership.aspnet_Applications.LoweredApplicationName == this.ApplicationName.ToLower());
                if (token == null)
                {
                    return false;
                }

                var result = this.ChangeUserPassword(this.ApplicationName, username, newPassword);
                entities.aspnet_ResetPasswordTokens.DeleteObject(token);
                entities.SaveChanges();
                return result;
            }
        }

        /// <summary>
        /// Processes a request to update the password question and answer for a membership user.
        /// </summary>
        /// <param name="username">
        /// The user to change the password question and answer for.
        /// </param>
        /// <param name="password">
        /// The password for the specified user.
        /// </param>
        /// <param name="newPasswordQuestion">
        /// The new password question for the specified user.
        /// </param>
        /// <param name="newPasswordAnswer">
        /// The new password answer for the specified user.
        /// </param>
        /// <returns>
        /// true if the password question and answer are updated successfully; otherwise, false.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override bool ChangePasswordQuestionAndAnswer(
            string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            return this.ChangePasswordQuestionAndAnswer(
                this.ApplicationName, username, password, newPasswordQuestion, newPasswordAnswer);
        }

        /// <summary>
        /// Takes, as input, a user name, password, password question, and password answer and updates the password question and answer
        ///   in the data source if the user name and password are valid.
        /// </summary>
        /// <param name="portalAlias">
        /// Appleseed's portal alias
        /// </param>
        /// <param name="username">
        /// The user's name
        /// </param>
        /// <param name="password">
        /// The user's password
        /// </param>
        /// <param name="newPasswordQuestion">
        /// The user's new password question
        /// </param>
        /// <param name="newPasswordAnswer">
        /// The user's new password answer
        /// </param>
        /// <returns>
        /// This method returns true if the password question and answer
        ///   are successfully updated. It returns false if either the user name or password is invalid.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override bool ChangePasswordQuestionAndAnswer(
            string portalAlias, string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            if (!this.ValidateUser(username, password))
            {
                return false;
            }

            var cmd = new SqlCommand
            {
                CommandText = "aspnet_Membership_ChangePasswordQuestionAndAnswer",
                CommandType = CommandType.StoredProcedure,
                Connection = new SqlConnection(this.ConnectionString)
            };

            cmd.Parameters.Add("@ApplicationName", SqlDbType.NVarChar, 256).Value = portalAlias;
            cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 255).Value = username;
            cmd.Parameters.Add("@NewPasswordQuestion", SqlDbType.NVarChar, 255).Value = newPasswordQuestion;
            cmd.Parameters.Add("@NewPasswordAnswer", SqlDbType.NVarChar, 255).Value = newPasswordAnswer;

            var returnCode = cmd.Parameters.Add("@ReturnCode", SqlDbType.Int);
            returnCode.Direction = ParameterDirection.ReturnValue;

            try
            {
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();

                return ((int)returnCode.Value) == 0;
            }
            catch (SqlException e)
            {
                if (this.WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "ChangePasswordQuestionAndAnswer");
                }

                throw new AppleseedMembershipProviderException(
                    "Error executing aspnet_Membership_ChangePasswordQuestionAndAnswer stored proc", e);
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        /// <summary>
        /// Create a reset password token for the user in order to allow him to change his password if he lost it.
        /// </summary>
        /// <param name="userId">
        /// The user id
        /// </param>
        /// <returns>
        /// The token created.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override Guid CreateResetPasswordToken(Guid userId)
        {
            var newTokenId = Guid.NewGuid();
            using (var entities = new AppleseedMembershipEntities(ConfigurationManager.ConnectionStrings["AppleseedMembershipEntities"].ConnectionString))
            {
                var newToken = new aspnet_ResetPasswordTokens
                {
                    TokenId = newTokenId,
                    UserId = userId,
                    CreationDate = DateTime.UtcNow
                };
                entities.aspnet_ResetPasswordTokens.AddObject(newToken);
                entities.SaveChanges();
            }

            return newTokenId;
        }

        /// <summary>
        /// Adds a new membership user to the data source.
        /// </summary>
        /// <param name="username">
        /// The user name for the new user.
        /// </param>
        /// <param name="password">
        /// The password for the new user.
        /// </param>
        /// <param name="email">
        /// The e-mail address for the new user.
        /// </param>
        /// <param name="passwordQuestion">
        /// The password question for the new user.
        /// </param>
        /// <param name="passwordAnswer">
        /// The password answer for the new user
        /// </param>
        /// <param name="isApproved">
        /// Whether or not the new user is approved to be validated.
        /// </param>
        /// <param name="providerUserKey">
        /// The unique identifier from the membership data source for the user.
        /// </param>
        /// <param name="status">
        /// A <see cref="T:System.Web.Security.MembershipCreateStatus"/> enumeration value indicating whether the user was created successfully.
        /// </param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUser"/> object populated with the information for the newly created user.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override MembershipUser CreateUser(
            string username,
            string password,
            string email,
            string passwordQuestion,
            string passwordAnswer,
            bool isApproved,
            object providerUserKey,
            out MembershipCreateStatus status)
        {
            return CreateUser(
                this.ApplicationName,
                username,
                password,
                email,
                passwordQuestion,
                passwordAnswer,
                isApproved,
                out status);
        }

        /// <summary>
        /// Creates the user.
        /// </summary>
        /// <param name="portalAlias">
        /// The portal alias.
        /// </param>
        /// <param name="username">
        /// The username.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <param name="email">
        /// The email.
        /// </param>
        /// <param name="passwordQuestion">
        /// The password question.
        /// </param>
        /// <param name="passwordAnswer">
        /// The password answer.
        /// </param>
        /// <param name="isApproved">
        /// if set to <c>true</c> [is approved].
        /// </param>
        /// <param name="status">
        /// The status.
        /// </param>
        /// <returns>
        /// A membership user.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override MembershipUser CreateUser(
            string portalAlias,
            string username,
            string password,
            string email,
            string passwordQuestion,
            string passwordAnswer,
            bool isApproved,
            out MembershipCreateStatus status)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                status = MembershipCreateStatus.InvalidUserName;
                return null;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                status = MembershipCreateStatus.InvalidEmail;
                return null;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            var args = new ValidatePasswordEventArgs(username, password, true);
            this.OnValidatingPassword(args);

            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            var passwordSalt = string.Empty;
            var encodedPassword = this.PasswordFormat == MembershipPasswordFormat.Hashed
                                      ? this.EncodePassword(passwordSalt + password)
                                      : this.EncodePassword(password);

            var cmd = new SqlCommand
            {
                CommandText = "aspnet_Membership_CreateUser",
                CommandType = CommandType.StoredProcedure,
                Connection = new SqlConnection(this.ConnectionString)
            };

            cmd.Parameters.Add("@ApplicationName", SqlDbType.NVarChar, 256).Value = portalAlias;
            cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 255).Value = username;
            cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 255).Value = encodedPassword;
            cmd.Parameters.Add("@PasswordSalt", SqlDbType.NVarChar, 255).Value = passwordSalt;
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 256).Value = email;
            cmd.Parameters.Add("@PasswordQuestion", SqlDbType.NVarChar, 255).Value = passwordQuestion;
            cmd.Parameters.Add("@PasswordAnswer", SqlDbType.NVarChar, 255).Value = passwordAnswer;
            cmd.Parameters.Add("@IsApproved", SqlDbType.Bit).Value = isApproved;
            cmd.Parameters.Add("@UniqueEmail", SqlDbType.Int).Value = this.RequiresUniqueEmail;
            cmd.Parameters.Add("@PasswordFormat", SqlDbType.Int).Value = this.PasswordFormat;
            cmd.Parameters.Add("@CreateDate", SqlDbType.DateTime).Value = DateTime.Now;
            cmd.Parameters.Add("@CurrentTimeUTC", SqlDbType.DateTime).Value = DateTime.UtcNow;

            var newUserIdParam = cmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier);
            newUserIdParam.Direction = ParameterDirection.Output;

            var returnCode = cmd.Parameters.Add("@ReturnCode", SqlDbType.Int);
            returnCode.Direction = ParameterDirection.ReturnValue;

            try
            {
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();

                status = (MembershipCreateStatus)Enum.Parse(typeof(MembershipCreateStatus), returnCode.Value.ToString());

                if (((int)returnCode.Value) == 0)
                {
                    // everything went OK
                    var user = (AppleseedUser)this.GetUser(newUserIdParam.Value, false);
                    this.SaveUserProfile(user);
                    return user;
                }
                else
                {
                    return null;
                }
            }
            catch (SqlException e)
            {
                if (this.WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "CreateUser");
                }

                status = MembershipCreateStatus.ProviderError;
                return null;
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        /// <summary>
        /// Removes a user from the membership data source.
        /// </summary>
        /// <param name="username">
        /// The name of the user to delete.
        /// </param>
        /// <param name="deleteAllRelatedData">
        /// true to delete data related to the user from the database; false to leave data related to the user in the database.
        /// </param>
        /// <returns>
        /// true if the user was successfully deleted; otherwise, false.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            return this.DeleteUser(this.ApplicationName, username, deleteAllRelatedData);
        }

        /// <summary>
        /// Takes, as input, a user name and deletes that user from the membership data source.
        /// </summary>
        /// <param name="portalAlias">
        /// Appleseed's portal alias
        /// </param>
        /// <param name="username">
        /// The user's name
        /// </param>
        /// <param name="deleteAllRelatedData">
        /// Specifies whether
        ///   related data for that user should be deleted also. If deleteAllRelatedData is true, DeleteUser
        ///   should delete role data, profile data, and all other data associated with that user.
        /// </param>
        /// <returns>
        /// DeleteUser returns true if the user was successfully deleted. Otherwise, it returns false.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override bool DeleteUser(string portalAlias, string username, bool deleteAllRelatedData)
        {
            this.DeleteUserProfile(username);

            var cmd = new SqlCommand
            {
                CommandText = "aspnet_Users_DeleteUser",
                CommandType = CommandType.StoredProcedure,
                Connection = new SqlConnection(this.ConnectionString)
            };

            cmd.Parameters.Add("@ApplicationName", SqlDbType.NVarChar, 256).Value = portalAlias;
            cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 255).Value = username;
            cmd.Parameters.Add("@TablesToDeleteFrom", SqlDbType.Int).Value = deleteAllRelatedData ? 0xF : 1;

            var tablesDeletedFrom = cmd.Parameters.Add("@NumTablesDeletedFrom", SqlDbType.Int);
            tablesDeletedFrom.Direction = ParameterDirection.Output;

            var returnCode = cmd.Parameters.Add("@ReturnCode", SqlDbType.Int);
            returnCode.Direction = ParameterDirection.ReturnValue;

            try
            {
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();

                return (((int)tablesDeletedFrom.Value) > 0) && (((int)returnCode.Value) == 0);
            }
            catch (SqlException e)
            {
                if (this.WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "DeleteUser");
                }

                throw new AppleseedMembershipProviderException("Error executing aspnet_Users_DeleteUser stored proc", e);
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        /// <summary>
        /// Gets a collection of membership users where the e-mail address contains the specified e-mail address to match.
        /// </summary>
        /// <param name="emailToMatch">
        /// The e-mail address to search for.
        /// </param>
        /// <param name="pageIndex">
        /// The index of the page of results to return. <paramref name="pageIndex"/> is zero-based.
        /// </param>
        /// <param name="pageSize">
        /// The size of the page of results to return.
        /// </param>
        /// <param name="totalRecords">
        /// The total number of matched users.
        /// </param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUserCollection"/> collection that contains a page of <paramref name="pageSize"/><see cref="T:System.Web.Security.MembershipUser"/> objects beginning at the page specified by <paramref name="pageIndex"/>.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override MembershipUserCollection FindUsersByEmail(
            string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            return this.FindUsersByEmail(this.ApplicationName, emailToMatch, pageIndex, pageSize, out totalRecords);
        }

        /// <summary>
        /// Returns a MembershipUserCollection containing MembershipUser objects representing users whose e-mail
        ///   addresses match the emailToMatch input parameter. Wildcard syntax is data source-dependent. MembershipUser
        ///   objects in the MembershipUserCollection are sorted by e-mail address.
        ///   For an explanation of the pageIndex, pageSize, and totalRecords parameters, see the GetAllUsers method.
        /// </summary>
        /// <param name="portalAlias">
        /// Appleseed's portal alias
        /// </param>
        /// <param name="emailToMatch">
        /// The email to match.
        /// </param>
        /// <param name="pageIndex">
        /// Page index to retrieve
        /// </param>
        /// <param name="pageSize">
        /// Page size.
        /// </param>
        /// <param name="totalRecords">
        /// Holds a count of all records.
        /// </param>
        /// <returns>
        /// A
        ///   <code>
        /// MembershipUserCollection
        ///   </code>
        /// . If FindUsersByEmail finds no
        ///   matching users, it returns an empty MembershipUserCollection.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override MembershipUserCollection FindUsersByEmail(
            string portalAlias, string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            var cmd = new SqlCommand
            {
                CommandText = "aspnet_Membership_FindUsersByEmail",
                CommandType = CommandType.StoredProcedure,
                Connection = new SqlConnection(this.ConnectionString)
            };

            cmd.Parameters.Add("@ApplicationName", SqlDbType.NVarChar, 256).Value = portalAlias;
            cmd.Parameters.Add("@EmailToMatch", SqlDbType.NVarChar, 256).Value = emailToMatch;
            cmd.Parameters.Add("@PageIndex", SqlDbType.Int).Value = pageIndex;
            cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;

            var returnValue = cmd.Parameters.Add("@ReturnValue", SqlDbType.Int);
            returnValue.Direction = ParameterDirection.ReturnValue;

            var users = new MembershipUserCollection();

            SqlDataReader reader = null;

            try
            {
                cmd.Connection.Open();

                using (reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var u = this.GetUserFromReader(reader);
                        this.LoadUserProfile(u);
                        users.Add(u);
                    }

                    reader.Close();
                    totalRecords = (int)returnValue.Value;
                }
            }
            catch (SqlException e)
            {
                if (this.WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "FindUsersByEmail");

                    throw new AppleseedMembershipProviderException(
                        "Error executing aspnet_Membership_FindUsersByEmail stored proc", e);
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                cmd.Connection.Close();
            }

            return users;
        }

        /// <summary>
        /// Gets a collection of membership users where the user name contains the specified user name to match.
        /// </summary>
        /// <param name="usernameToMatch">
        /// The user name to search for.
        /// </param>
        /// <param name="pageIndex">
        /// The index of the page of results to return. <paramref name="pageIndex"/> is zero-based.
        /// </param>
        /// <param name="pageSize">
        /// The size of the page of results to return.
        /// </param>
        /// <param name="totalRecords">
        /// The total number of matched users.
        /// </param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUserCollection"/> collection that contains a page of <paramref name="pageSize"/><see cref="T:System.Web.Security.MembershipUser"/> objects beginning at the page specified by <paramref name="pageIndex"/>.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override MembershipUserCollection FindUsersByName(
            string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            return this.FindUsersByName(this.ApplicationName, usernameToMatch, pageIndex, pageSize, out totalRecords);
        }

        /// <summary>
        /// Returns a MembershipUserCollection containing MembershipUser objects representing users whose user names match
        ///   the usernameToMatch input parameter. Wildcard syntax is data source-dependent. MembershipUser objects in the
        ///   MembershipUserCollection are sorted by user name.
        ///   For an explanation of the pageIndex, pageSize, and totalRecords parameters, see the GetAllUsers method.
        /// </summary>
        /// <param name="portalAlias">
        /// Appleseed's portal alias
        /// </param>
        /// <param name="usernameToMatch">
        /// The username to match.
        /// </param>
        /// <param name="pageIndex">
        /// Page index to retrieve
        /// </param>
        /// <param name="pageSize">
        /// Page size.
        /// </param>
        /// <param name="totalRecords">
        /// Holds a count of all records.
        /// </param>
        /// <returns>
        /// A
        ///   <code>
        /// MembershipUserCollection
        ///   </code>
        /// . If FindUsersByName finds no matching users, it returns an
        ///   empty MembershipUserCollection.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override MembershipUserCollection FindUsersByName(
            string portalAlias, string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            var cmd = new SqlCommand
            {
                CommandText = "aspnet_Membership_FindUsersByName",
                CommandType = CommandType.StoredProcedure,
                Connection = new SqlConnection(this.ConnectionString)
            };

            cmd.Parameters.Add("@ApplicationName", SqlDbType.NVarChar, 256).Value = portalAlias;
            cmd.Parameters.Add("@UserNameToMatch", SqlDbType.NVarChar, 256).Value = usernameToMatch;
            cmd.Parameters.Add("@PageIndex", SqlDbType.Int).Value = pageIndex;
            cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;

            var returnCode = cmd.Parameters.Add("@ReturnCode", SqlDbType.Int);
            returnCode.Direction = ParameterDirection.ReturnValue;

            var users = new MembershipUserCollection();

            SqlDataReader reader = null;

            try
            {
                cmd.Connection.Open();

                using (reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var u = this.GetUserFromReader(reader);
                        this.LoadUserProfile(u);
                        users.Add(u);
                    }

                    reader.Close();
                    totalRecords = (int)returnCode.Value;
                }
            }
            catch (SqlException e)
            {
                if (this.WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "FindUsersByName");
                }

                throw new AppleseedMembershipProviderException(
                    "Error executing aspnet_Membership_FindUsersByName stored proc", e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                cmd.Connection.Close();
            }

            return users;
        }

        /// <summary>
        /// Gets a collection of all the users in the data source in pages of data.
        /// </summary>
        /// <param name="pageIndex">
        /// The index of the page of results to return. <paramref name="pageIndex"/> is zero-based.
        /// </param>
        /// <param name="pageSize">
        /// The size of the page of results to return.
        /// </param>
        /// <param name="totalRecords">
        /// The total number of matched users.
        /// </param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUserCollection"/> collection that contains a page of <paramref name="pageSize"/><see cref="T:System.Web.Security.MembershipUser"/> objects beginning at the page specified by <paramref name="pageIndex"/>.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            return this.GetAllUsers(this.ApplicationName, pageIndex, pageSize, out totalRecords);
        }

        /// <summary>
        /// The results returned by GetAllUsers are constrained by the pageIndex and pageSize input parameters.
        ///   pageSize specifies the maximum number of MembershipUser objects to return. pageIndex
        ///   identifies which page of results to return. Page indexes are 0-based.
        ///   GetAllUsers also takes an out parameter (in Visual Basic, ByRef) named totalRecords that, on return, holds a count of all registered users.
        /// </summary>
        /// <param name="portalAlias">
        /// Appleseed's portal alias
        /// </param>
        /// <returns>
        /// Returns a MembershipUserCollection containing MembershipUser objects representing all registered users.
        ///   If there are no registered users, GetAllUsers returns an empty MembershipUserCollection.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override MembershipUserCollection GetAllUsers(string portalAlias)
        {
            int records;
            return this.GetAllUsers(portalAlias, 0, int.MaxValue, out records);
        }

        /// <summary>
        /// The results returned by GetAllUsers are constrained by the pageIndex and pageSize input parameters.
        ///   pageSize specifies the maximum number of MembershipUser objects to return. pageIndex
        ///   identifies which page of results to return. Page indexes are 0-based.
        ///   GetAllUsers also takes an out parameter (in Visual Basic, ByRef) named totalRecords that, on return, holds a count of all registered users.
        /// </summary>
        /// <param name="portalAlias">
        /// Appleseed's portal alias
        /// </param>
        /// <param name="pageIndex">
        /// Page index to retrieve
        /// </param>
        /// <param name="pageSize">
        /// Page size.
        /// </param>
        /// <param name="totalRecords">
        /// Holds a count of all records.
        /// </param>
        /// <returns>
        /// Returns a MembershipUserCollection containing MembershipUser objects representing all registered users.
        ///   If there are no registered users, GetAllUsers returns an empty MembershipUserCollection.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override MembershipUserCollection GetAllUsers(
            string portalAlias, int pageIndex, int pageSize, out int totalRecords)
        {
            var users = new MembershipUserCollection();

            var cmd = new SqlCommand
            {
                CommandText = "aspnet_Membership_GetAllUsers",
                CommandType = CommandType.StoredProcedure,
                Connection = new SqlConnection(this.ConnectionString)
            };

            cmd.Parameters.Add("@ApplicationName", SqlDbType.NVarChar, 256).Value = portalAlias;
            cmd.Parameters.Add("@PageIndex", SqlDbType.Int).Value = pageIndex;
            cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;

            var totalRecordsParam = cmd.Parameters.Add("@TotalRecords", SqlDbType.Int);
            totalRecordsParam.Direction = ParameterDirection.ReturnValue;

            SqlDataReader reader = null;
            try
            {
                cmd.Connection.Open();

                using (reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var u = this.GetUserFromReader(reader);
                        this.LoadUserProfile(u);
                        if (users[u.UserName] == null)
                        {
                            users.Add(u);
                        }

                    }

                    reader.Close();
                    totalRecords = (int)totalRecordsParam.Value;
                }

                return users;
            }
            catch (SqlException e)
            {
                if (this.WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetAllUsers");
                }

                throw new AppleseedMembershipProviderException(
                    "Error executing aspnet_Membership_GetAllUsers stored proc", e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                cmd.Connection.Close();
            }
        }

        /// <summary>
        /// Gets the number of users online.
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <returns>
        /// The number of users online.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override int GetNumberOfUsersOnline(int portalId)
        {
            var totalNumberOfUsers = 0;

            var portalsDb = new PortalsDB();
            var dr = portalsDb.GetPortals();
            try
            {
                while (dr.Read())
                {
                    if ((int)dr["PortalID"] == portalId)
                    {
                        totalNumberOfUsers = GetNumberOfUsersOnline(dr["PortalAlias"].ToString());
                    }
                }
            }
            finally
            {
                dr.Close(); // by Manu, fixed bug 807858
            }

            return totalNumberOfUsers;
        }

        /// <summary>
        /// Returns a count of users that are currently online; that is, whose LastActivityDate is
        ///   greater than the current date and time minus the value of the membership service's
        ///   UserIsOnlineTimeWindow property, which can be read from Membership.UserIsOnlineTimeWindow.
        ///   UserIsOnlineTimeWindow specifies a time in minutes and is set using the
        ///   <code>
        /// &lt;membership&gt;
        ///   </code>
        /// element's userIsOnlineTimeWindow attribute.
        /// </summary>
        /// <param name="portalAlias">
        /// Appleseed's portal alias
        /// </param>
        /// <returns>
        /// Returns a count of users that are currently online
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override int GetNumberOfUsersOnline(string portalAlias)
        {
            var cmd = new SqlCommand
            {
                CommandText = "aspnet_Membership_GetNumberOfUsersOnline",
                CommandType = CommandType.StoredProcedure,
                Connection = new SqlConnection(this.ConnectionString)
            };

            cmd.Parameters.Add("@ApplicationName", SqlDbType.NVarChar, 256).Value = portalAlias;
            cmd.Parameters.Add("@MinutesSinceLastInActive", SqlDbType.Int).Value = Membership.UserIsOnlineTimeWindow;
            cmd.Parameters.Add("@CurrentTimeUtc", SqlDbType.DateTime).Value = DateTime.UtcNow;

            int numOnline;

            try
            {
                cmd.Connection.Open();

                var prueba = cmd.ExecuteScalar();
                numOnline = Convert.ToInt32(prueba);
            }
            catch (SqlException e)
            {
                if (this.WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetNumberOfUsersOnline");
                }

                throw new AppleseedMembershipProviderException(
                    "Error executing aspnet_Membership_GetNumberOfUsersOnline stored proc", e);
            }
            finally
            {
                cmd.Connection.Close();
            }

            return numOnline;
        }

        /// <summary>
        /// Gets the number of users currently accessing the application.
        /// </summary>
        /// <returns>
        /// The number of users currently accessing the application.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override int GetNumberOfUsersOnline()
        {
            var totalNumberOfUsers = 0;

            var portalsDb = new PortalsDB();
            var dr = portalsDb.GetPortals();
            try
            {
                while (dr.Read())
                {
                    totalNumberOfUsers += GetNumberOfUsersOnline(dr["PortalAlias"].ToString());
                }
            }
            finally
            {
                dr.Close(); // by Manu, fixed bug 807858
            }

            return totalNumberOfUsers;
        }

        /// <summary>
        /// Returns the usernames of all the users that are currently online; that is, whose LastActivityDate is
        ///   greater than the current date and time minus the value of the membership service's
        ///   UserIsOnlineTimeWindow property, which can be read from Membership.UserIsOnlineTimeWindow.
        ///   UserIsOnlineTimeWindow specifies a time in minutes and is set using the
        ///   <code>
        /// &lt;membership&gt;
        ///   </code>
        /// element's userIsOnlineTimeWindow attribute.
        /// </summary>
        /// <returns>
        /// Returns a list containing the usernames of all the users that are currently online
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override IList<string> GetOnlineUsers()
        {
            var dateActive = DateTime.UtcNow.AddMinutes(-1 * Membership.UserIsOnlineTimeWindow);
            using (var entities = new AppleseedMembershipEntities(ConfigurationManager.ConnectionStrings["AppleseedMembershipEntities"].ConnectionString))
            {
                var users = entities.aspnet_Users.Include("aspnet_Membership").Include("aspnet_Application");

                // to avoid lazy loading
                return
                    users.Where(u => u.aspnet_Applications.LoweredApplicationName == this.ApplicationName.ToLower()).
                        Where(u => u.LastActivityDate > dateActive).Select(u => u.UserName).ToList();
            }
        }

        /// <summary>
        /// Gets the password for the specified user name from the data source.
        /// </summary>
        /// <param name="username">
        /// The user to retrieve the password for.
        /// </param>
        /// <param name="answer">
        /// The password answer for the user.
        /// </param>
        /// <returns>
        /// The password for the specified user name.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override string GetPassword(string username, string answer)
        {
            return this.GetPassword(this.ApplicationName, username, answer);
        }

        /// <summary>
        /// Takes, as input, a user name and a password answer and returns that user's password.
        ///   Before retrieving a password, GetPassword verifies that EnablePasswordRetrieval is true.
        ///   GetPassword also checks the value of the RequiresQuestionAndAnswer property before retrieving a password.
        ///   If RequiresQuestionAndAnswer is true, GetPassword compares the supplied password answer to the stored password answer
        ///   and throws a MembershipPasswordException if the two don't match.
        /// </summary>
        /// <param name="portalAlias">
        /// Appleseed's portal alias
        /// </param>
        /// <param name="username">
        /// The user's name
        /// </param>
        /// <param name="answer">
        /// The password answer
        /// </param>
        /// <returns>
        /// Returns the user's password
        /// </returns>
        /// <exception cref="System.Configuration.Provider.ProviderException">
        /// If the user name is not valid, GetPassword throws a ProviderException.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// If EnablePasswordRetrieval is false, GetPassword throws a NotSupportedException.
        /// </exception>
        /// <exception cref="System.Configuration.Provider.ProviderException">
        /// If EnablePasswordRetrieval  is true but the password format is hashed, GetPassword throws a
        ///   ProviderException since hashed passwords cannot, by definition, be retrieved.
        /// </exception>
        /// <exception cref="System.Web.Security.MembershipPasswordException">
        /// GetPassword also throws a MembershipPasswordException
        ///   if the user whose password is being retrieved is currently locked out.
        /// </exception>
        /// <exception cref="System.Web.Security.MembershipPasswordException">
        /// If RequiresQuestionAndAnswer is true, GetPassword compares the supplied password answer to the stored password answer
        ///   and throws a MembershipPasswordException if the two don't match.
        /// </exception>
        /// <remarks>
        /// </remarks>
        public override string GetPassword(string portalAlias, string username, string answer)
        {
            if (!this.EnablePasswordRetrieval)
            {
                throw new AppleseedMembershipProviderException("Password Retrieval Not Enabled.");
            }

            if (this.PasswordFormat == MembershipPasswordFormat.Hashed)
            {
                throw new AppleseedMembershipProviderException("Cannot retrieve Hashed passwords.");
            }

            var cmd = new SqlCommand
            {
                CommandText = "aspnet_Membership_GetPassword",
                CommandType = CommandType.StoredProcedure,
                Connection = new SqlConnection(this.ConnectionString)
            };

            cmd.Parameters.Add("@ApplicationName", SqlDbType.NVarChar, 256).Value = portalAlias;
            cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 256).Value = username;
            cmd.Parameters.Add("@MaxInvalidPasswordAttempts", SqlDbType.Int).Value = this.MaxInvalidPasswordAttempts;
            cmd.Parameters.Add("@PasswordAttemptWindow", SqlDbType.Int).Value = this.PasswordAttemptWindow;
            cmd.Parameters.Add("@CurrentTimeUtc", SqlDbType.DateTime).Value = DateTime.UtcNow;
            cmd.Parameters.Add("@PasswordAnswer", SqlDbType.NVarChar, 128).Value = answer;

            var returnCodeParam = cmd.Parameters.Add("@ReturnCode", SqlDbType.Int);
            returnCodeParam.Direction = ParameterDirection.ReturnValue;

            var password = string.Empty;
            SqlDataReader reader = null;

            try
            {
                cmd.Connection.Open();

                using (reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (reader.HasRows)
                    {
                        reader.Read();

                        password = reader.GetString(0);
                    }

                    reader.Close();

                    var returnCode = (int)returnCodeParam.Value;
                    switch (returnCode)
                    {
                        case ErrorCodeUserNotFound:
                            throw new AppleseedMembershipProviderException("The supplied user name was not found.");
                        case ErrorCodeIncorrectPasswordAnswer:
                            throw new MembershipPasswordException("Incorrect password answer.");
                        case ErrorCodeUserLockedOut:
                            throw new MembershipPasswordException("User is currently locked out");
                        case -1:
                            throw new AppleseedMembershipProviderException(
                                "Error executing aspnet_Membership_GetPassword stored proc");
                    }

                    if (this.PasswordFormat == MembershipPasswordFormat.Encrypted)
                    {
                        password = this.UnEncodePassword(password);
                    }

                    return password;
                }
            }
            catch (SqlException e)
            {
                if (this.WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetPassword");
                }

                throw new AppleseedMembershipProviderException(
                    "Error executing aspnet_Membership_GetPassword stored proc", e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                cmd.Connection.Close();
            }
        }

        /// <summary>
        /// Gets user information from the data source based on the unique identifier for the membership user. Provides an option to update the last-activity date/time stamp for the user.
        /// </summary>
        /// <param name="providerUserKey">
        /// The unique identifier for the membership user to get information for.
        /// </param>
        /// <param name="userIsOnline">
        /// true to update the last-activity date/time stamp for the user; false to return user information without updating the last-activity date/time stamp for the user.
        /// </param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUser"/> object populated with the specified user's information from the data source.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            var cmd = new SqlCommand
            {
                CommandText = "aspnet_Membership_GetUserByUserId",
                CommandType = CommandType.StoredProcedure,
                Connection = new SqlConnection(this.ConnectionString)
            };

            cmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value = providerUserKey;
            cmd.Parameters.Add("@CurrentTimeUtc", SqlDbType.DateTime).Value = DateTime.UtcNow;
            cmd.Parameters.Add("@UpdateLastActivity", SqlDbType.Bit).Value = userIsOnline ? 1 : 0;

            AppleseedUser u = null;
            SqlDataReader reader = null;

            try
            {
                cmd.Connection.Open();

                using (reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();

                        var email = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                        var passwordQuestion = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                        var comment = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                        var isApproved = reader.IsDBNull(3) ? false : reader.GetBoolean(3);
                        var creationDate = reader.IsDBNull(4) ? DateTime.Now : reader.GetDateTime(4);
                        var lastLoginDate = reader.IsDBNull(5) ? DateTime.Now : reader.GetDateTime(5);
                        var lastActivityDate = reader.IsDBNull(6) ? DateTime.Now : reader.GetDateTime(6);
                        var lastPasswordChangedDate = reader.IsDBNull(7) ? DateTime.Now : reader.GetDateTime(7);
                        var userName = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);
                        var isLockedOut = reader.IsDBNull(9) ? false : reader.GetBoolean(9);
                        var lastLockedOutDate = reader.IsDBNull(10) ? DateTime.Now : reader.GetDateTime(10);

                        u = this.InstantiateNewUser(
                            this.Name,
                            userName,
                            (Guid)providerUserKey,
                            email,
                            passwordQuestion,
                            comment,
                            isApproved,
                            isLockedOut,
                            creationDate,
                            lastLoginDate,
                            lastActivityDate,
                            lastPasswordChangedDate,
                            lastLockedOutDate);

                        this.LoadUserProfile(u);
                    }
                }
                reader.Close();

                cmd = new SqlCommand
                {
                    CommandText = "SELECT * FROM aspnet_Membership WHERE UserId = '" + providerUserKey + "'",
                    CommandType = CommandType.Text,
                    Connection = new SqlConnection(this.ConnectionString)
                };

                cmd.Connection.Open();

                using (reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        u.FailedPasswordAttemptCount = (int)reader["FailedPasswordAttemptCount"];
                    }
                }
                reader.Close();
            }
            catch (SqlException e)
            {
                if (this.WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetUser(object, Boolean)");
                }

                throw new AppleseedMembershipProviderException(
                    "Error executing aspnet_Membership_GetUserByUserId stored proc", e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                cmd.Connection.Close();
            }

            return u;
        }

        /// <summary>
        /// Gets information from the data source for a user. Provides an option to update the last-activity date/time stamp for the user.
        /// </summary>
        /// <param name="username">
        /// The name of the user to get information for.
        /// </param>
        /// <param name="userIsOnline">
        /// true to update the last-activity date/time stamp for the user; false to return user information without updating the last-activity date/time stamp for the user.
        /// </param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUser"/> object populated with the specified user's information from the data source.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            return this.GetUser(this.ApplicationName, username, userIsOnline);
        }

        /// <summary>
        /// Takes, as input, a user name or user ID (the method is overloaded) and a Boolean value indicating whether to
        ///   update the user's LastActivityDate to show that the user is currently online.
        /// </summary>
        /// <param name="portalAlias">
        /// Appleseed's portal alias
        /// </param>
        /// <param name="username">
        /// The user's name
        /// </param>
        /// <param name="userIsOnline">
        /// Whether user is online.
        /// </param>
        /// <returns>
        /// GetUser returns a
        ///   MembershipUser object representing the specified user. If the user name or user ID is invalid (that is,
        ///   if it doesn't represent a registered user) GetUser returns null (Nothing in Visual Basic).
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override MembershipUser GetUser(string portalAlias, string username, bool userIsOnline)
        {
            if (string.IsNullOrEmpty(username))
            {
                return null;
            }

            var cmd = new SqlCommand
            {
                CommandText = "aspnet_Membership_GetUserByName",
                CommandType = CommandType.StoredProcedure,
                Connection = new SqlConnection(this.ConnectionString)
            };

            cmd.Parameters.Add("@ApplicationName", SqlDbType.NVarChar, 256).Value = portalAlias;
            cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 256).Value = username;
            cmd.Parameters.Add("@CurrentTimeUtc", SqlDbType.DateTime).Value = DateTime.UtcNow;
            cmd.Parameters.Add("@UpdateLastActivity", SqlDbType.Bit).Value = userIsOnline ? 1 : 0;

            AppleseedUser u = null;
            SqlDataReader reader = null;
            var providerUserKey = Guid.Empty;
            try
            {
                cmd.Connection.Open();

                using (reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (reader.HasRows)
                    {
                        reader.Read();

                        var email = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                        var passwordQuestion = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                        var comment = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                        var isApproved = reader.IsDBNull(3) ? false : reader.GetBoolean(3);
                        var creationDate = reader.IsDBNull(4) ? DateTime.Now : reader.GetDateTime(4);
                        var lastLoginDate = reader.IsDBNull(5) ? DateTime.Now : reader.GetDateTime(5);
                        var lastActivityDate = reader.IsDBNull(6) ? DateTime.Now : reader.GetDateTime(6);
                        var lastPasswordChangedDate = reader.IsDBNull(7) ? DateTime.Now : reader.GetDateTime(7);

                        providerUserKey = new Guid(reader.GetValue(8).ToString());
                        var isLockedOut = reader.IsDBNull(9) ? false : reader.GetBoolean(9);
                        var lastLockedOutDate = reader.IsDBNull(10) ? DateTime.Now : reader.GetDateTime(10);

                        u = this.InstantiateNewUser(
                            this.Name,
                            username,
                            providerUserKey,
                            email,
                            passwordQuestion,
                            comment,
                            isApproved,
                            isLockedOut,
                            creationDate,
                            lastLoginDate,
                            lastActivityDate,
                            lastPasswordChangedDate,
                            lastLockedOutDate);
                        this.LoadUserProfile(u);
                    }
                    reader.Close();

                    var cmdQur = new SqlCommand
                    {
                        CommandText = "SELECT * FROM aspnet_Membership WHERE UserId = '" + providerUserKey.ToString().Replace("{", string.Empty).Replace("}", string.Empty) + "'",
                        CommandType = CommandType.Text,
                        Connection = new SqlConnection(this.ConnectionString)
                    };

                    cmdQur.Connection.Open();

                    using (reader = cmdQur.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            u.FailedPasswordAttemptCount = (int)reader["FailedPasswordAttemptCount"];
                        }
                    }
                    reader.Close();
                }
            }
            catch (SqlException e)
            {
                if (this.WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetUser(String, Boolean)");
                }

                throw new AppleseedMembershipProviderException(
                    "Error executing aspnet_Membership_GetUserByName stored proc", e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                cmd.Connection.Close();
            }

            return u;
        }

        /// <summary>
        /// Gets the user name associated with the specified e-mail address.
        /// </summary>
        /// <param name="email">
        /// The e-mail address to search for.
        /// </param>
        /// <returns>
        /// The user name associated with the specified e-mail address. If no match is found, return null.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override string GetUserNameByEmail(string email)
        {
            return this.GetUserNameByEmail(this.ApplicationName, email);
        }

        /// <summary>
        /// Takes, as input, an e-mail address and returns the first registered user name whose e-mail address matches the one supplied.
        ///   If it doesn't find a user with a matching e-mail address, GetUserNameByEmail returns an empty string.
        /// </summary>
        /// <param name="portalAlias">
        /// Appleseed's portal alias
        /// </param>
        /// <param name="email">
        /// The email address.
        /// </param>
        /// <returns>
        /// The first registered user name whose e-mail address matches the one supplied.
        ///   If it doesn't find a user with a matching e-mail address, GetUserNameByEmail returns an empty string.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override string GetUserNameByEmail(string portalAlias, string email)
        {
            var cmd = new SqlCommand
            {
                CommandText = "aspnet_Membership_GetUserByEmail",
                CommandType = CommandType.StoredProcedure,
                Connection = new SqlConnection(this.ConnectionString)
            };

            cmd.Parameters.Add("@ApplicationName", SqlDbType.NVarChar, 256).Value = portalAlias;
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = email;

            try
            {
                cmd.Connection.Open();

                var username = (string)cmd.ExecuteScalar() ?? string.Empty;
                return username;
            }
            catch (SqlException e)
            {
                if (this.WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetUserNameByEmail");
                }

                throw new AppleseedMembershipProviderException(
                    "Error executing aspnet_Membership_GetUserByEmail stored proc", e);
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        /// <summary>
        /// Initializes the provider.
        /// </summary>
        /// <param name="name">
        /// The friendly name of the provider.
        /// </param>
        /// <param name="config">
        /// A collection of the name/value pairs representing the provider-specific attributes specified in the configuration for this provider.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The name of the provider is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        /// The name of the provider has a length of zero.
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// An attempt is made to call <see cref="M:System.Configuration.Provider.ProviderBase.Initialize(System.String,System.Collections.Specialized.NameValueCollection)"/> on a provider after the provider has already been initialized.
        /// </exception>
        /// <remarks>
        /// </remarks>
        public override void Initialize(string name, NameValueCollection config)
        {
            // Initialize values from web.config.
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            if (String.IsNullOrEmpty(name))
            {
                name = "AppleseedSqlMembershipProvider";
            }

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Appleseed SQL Membership provider");
            }

            // Initialize the abstract base class.
            base.Initialize(name, config);

            this.pApplicationName = GetConfigValue(config["applicationName"], HostingEnvironment.ApplicationVirtualPath);
            this.pMaxInvalidPasswordAttempts = Convert.ToInt32(
                GetConfigValue(config["maxInvalidPasswordAttempts"], "5"));
            this.pPasswordAttemptWindow = Convert.ToInt32(GetConfigValue(config["passwordAttemptWindow"], "10"));
            this.pMinRequiredNonAlphanumericCharacters =
                Convert.ToInt32(GetConfigValue(config["minRequiredNonAlphanumericCharacters"], "1"));
            this.pMinRequiredPasswordLength = Convert.ToInt32(GetConfigValue(config["minRequiredPasswordLength"], "7"));
            this.pPasswordStrengthRegularExpression =
                Convert.ToString(GetConfigValue(config["passwordStrengthRegularExpression"], string.Empty));
            this.pEnablePasswordReset = Convert.ToBoolean(GetConfigValue(config["enablePasswordReset"], "true"));
            this.pEnablePasswordRetrieval = Convert.ToBoolean(GetConfigValue(config["enablePasswordRetrieval"], "true"));
            this.pRequiresQuestionAndAnswer =
                Convert.ToBoolean(GetConfigValue(config["requiresQuestionAndAnswer"], "false"));
            this.pRequiresUniqueEmail = Convert.ToBoolean(GetConfigValue(config["requiresUniqueEmail"], "true"));
            this.WriteExceptionsToEventLog =
                Convert.ToBoolean(GetConfigValue(config["writeExceptionsToEventLog"], "true"));

            var tempFormat = config["passwordFormat"] ?? "Hashed";

            switch (tempFormat)
            {
                case "Hashed":
                    this.pPasswordFormat = MembershipPasswordFormat.Hashed;
                    break;
                case "Encrypted":
                    this.pPasswordFormat = MembershipPasswordFormat.Encrypted;
                    break;
                case "Clear":
                    this.pPasswordFormat = MembershipPasswordFormat.Clear;
                    break;
                default:
                    throw new AppleseedMembershipProviderException("Password format not supported.");
            }

            // Initialize SqlConnection.
            var connectionStringSettings = ConfigurationManager.ConnectionStrings[config["connectionStringName"]];

            if (connectionStringSettings == null ||
                connectionStringSettings.ConnectionString.Trim().Equals(string.Empty))
            {
                throw new AppleseedMembershipProviderException("Connection string cannot be blank.");
            }

            this.ConnectionString = connectionStringSettings.ConnectionString;

            if (String.IsNullOrEmpty(this.ConnectionString) || this.ConnectionString == "foo")
            {
                //Create the XmlDocument.
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc.Load(HttpContext.Current.Server.MapPath("/web.config"));

                //Display all the book titles.
                System.Xml.XmlNodeList elemList = doc.GetElementsByTagName("connectionStrings");
                this.ConnectionString = elemList[0].ChildNodes[0].Attributes["connectionString"].Value;
            }

            if (this.EnablePasswordRetrieval && (this.PasswordFormat == MembershipPasswordFormat.Hashed))
            {
                throw new AppleseedMembershipProviderException(
                    "Can't enable password retrieval when using hashed passwords");
            }
        }

        /// <summary>
        /// Resets a user's password to a new, automatically generated password.
        /// </summary>
        /// <param name="username">
        /// The user to reset the password for.
        /// </param>
        /// <param name="answer">
        /// The password answer for the specified user.
        /// </param>
        /// <returns>
        /// The new password for the specified user.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override string ResetPassword(string username, string answer)
        {
            return this.ResetPassword(this.ApplicationName, username, answer);
        }

        /// <summary>
        /// Takes, as input, a user name and a password answer and replaces the user's current password with a new,
        ///   random password.  A convenient mechanism for generating a random password is the Membership.GeneratePassword method.
        ///   ResetPassword also checks the value of the RequiresQuestionAndAnswer property before resetting a password.
        ///   Before resetting a password, ResetPassword verifies that EnablePasswordReset is true.
        ///   Before resetting a password, ResetPassword calls the provider's virtual OnValidatingPassword method to
        ///   validate the new password. It then resets the password or cancels the action based on the outcome of
        ///   the call.
        ///   Following a successful password reset, ResetPassword updates the user's LastPasswordChangedDate.
        /// </summary>
        /// <param name="portalAlias">
        /// Appleseed's portal alias
        /// </param>
        /// <param name="username">
        /// The user's name
        /// </param>
        /// <param name="answer">
        /// The password answer
        /// </param>
        /// <returns>
        /// ResetPassword then returns the new password.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// If EnablePasswordReset is false, ResetPassword throws a NotSupportedException.
        /// </exception>
        /// <exception cref="System.Configuration.Provider.ProviderException">
        /// If the user name is not valid, ResetPassword throws a ProviderException.
        /// </exception>
        /// <exception cref="System.Configuration.Provider.ProviderException">
        /// If the new password is invalid, ResetPassword throws a ProviderException.
        /// </exception>
        /// <exception cref="System.Web.Security.MembershipPasswordException">
        /// If the user whose password is being changed is currently locked out, ResetPassword throws a MembershipPasswordException.
        /// </exception>
        /// <exception cref="System.Web.Security.MembershipPasswordException">
        /// If RequiresQuestionAndAnswer is true, ResetPassword compares the supplied password
        ///   answer to the stored password answer and throws a MembershipPasswordException if the two don't match.
        /// </exception>
        /// <remarks>
        /// </remarks>
        public override string ResetPassword(string portalAlias, string username, string answer)
        {
            if (!this.EnablePasswordReset)
            {
                throw new NotSupportedException("Password reset is not enabled.");
            }

            if (answer == null)
            {
                answer = string.Empty;
            }

            var newPassword = Membership.GeneratePassword(NewPasswordLength, this.MinRequiredNonAlphanumericCharacters);

            var args = new ValidatePasswordEventArgs(username, newPassword, false);

            this.OnValidatingPassword(args);

            if (args.Cancel)
            {
                throw args.FailureInformation ??
                      new AppleseedMembershipProviderException(
                          "Reset password canceled due to password validation failure.");
            }

            var passwordSalt = string.Empty;
            var encodedPassword = this.PasswordFormat == MembershipPasswordFormat.Hashed
                                      ? this.EncodePassword(passwordSalt + newPassword)
                                      : this.EncodePassword(newPassword);

            var conn = new SqlConnection(this.ConnectionString);

            var cmd = new SqlCommand
            {
                CommandText = "aspnet_Membership_ResetPassword",
                CommandType = CommandType.StoredProcedure,
                Connection = conn
            };

            cmd.Parameters.Add("@ApplicationName", SqlDbType.NVarChar, 256).Value = portalAlias;
            cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 256).Value = username;
            cmd.Parameters.Add("@NewPassword", SqlDbType.NVarChar, 128).Value = encodedPassword;
            cmd.Parameters.Add("@MaxInvalidPasswordAttempts", SqlDbType.Int).Value = this.MaxInvalidPasswordAttempts;
            cmd.Parameters.Add("@PasswordAttemptWindow", SqlDbType.Int).Value = this.PasswordAttemptWindow;
            cmd.Parameters.Add("@PasswordSalt", SqlDbType.NVarChar, 128).Value = passwordSalt;
            cmd.Parameters.Add("@CurrentTimeUtc", SqlDbType.DateTime).Value = DateTime.UtcNow;
            cmd.Parameters.Add("@PasswordFormat", SqlDbType.Int).Value = this.PasswordFormat;
            cmd.Parameters.Add("@PasswordAnswer", SqlDbType.NVarChar, 128).Value = answer;

            var returnCodeParam = cmd.Parameters.Add("@ReturnCode", SqlDbType.Int);
            returnCodeParam.Direction = ParameterDirection.ReturnValue;

            try
            {
                conn.Open();

                cmd.ExecuteNonQuery();

                var returnCode = (int)returnCodeParam.Value;

                switch (returnCode)
                {
                    case ErrorCodeUserNotFound:
                        throw new AppleseedMembershipProviderException("The supplied user name is not found.");
                    case ErrorCodeIncorrectPasswordAnswer:
                        throw new MembershipPasswordException("The supplied password answer is incorrect.");
                    case ErrorCodeUserLockedOut:
                        throw new AppleseedMembershipProviderException("The supplied user is locked out.");
                    case -1:
                        throw new AppleseedMembershipProviderException("Error resetting password");
                }

                return newPassword;
            }
            catch (SqlException e)
            {
                if (this.WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "ResetPassword");
                }

                throw new AppleseedMembershipProviderException(
                    "Error executing aspnet_Membership_ResetPassword stored proc", e);
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Clears a lock so that the membership user can be validated.
        /// </summary>
        /// <param name="userName">
        /// The membership user whose lock status you want to clear.
        /// </param>
        /// <returns>
        /// true if the membership user was successfully unlocked; otherwise, false.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override bool UnlockUser(string userName)
        {
            return this.UnlockUser(this.ApplicationName, userName);
        }

        /// <summary>
        /// Unlocks (that is, restores login privileges for) the specified user.
        /// </summary>
        /// <param name="portalAlias">
        /// Appleseed's portal alias
        /// </param>
        /// <param name="username">
        /// The user's name
        /// </param>
        /// <returns>
        /// UnlockUser returns true if the user is successfully
        ///   unlocked. Otherwise, it returns false. If the user is already unlocked, UnlockUser simply returns true.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override bool UnlockUser(string portalAlias, string username)
        {
            var cmd = new SqlCommand
            {
                CommandText = "aspnet_Membership_UnlockUser",
                CommandType = CommandType.StoredProcedure,
                Connection = new SqlConnection(this.ConnectionString)
            };

            cmd.Parameters.Add("@ApplicationName", SqlDbType.NVarChar, 256).Value = portalAlias;
            cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 256).Value = username;

            var returnCodeParam = cmd.Parameters.Add("@ReturnCode", SqlDbType.Int);
            returnCodeParam.Direction = ParameterDirection.ReturnValue;

            try
            {
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();

                var returnCode = (int)returnCodeParam.Value;
                return returnCode == 0;
            }
            catch (SqlException e)
            {
                if (this.WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "UnlockUser");
                }

                throw new AppleseedMembershipProviderException(
                    "Error executing aspnet_Membership_UnlockUser stored proc", e);
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        /// <summary>
        /// Updates information about a user in the data source.
        /// </summary>
        /// <param name="user">
        /// A <see cref="T:System.Web.Security.MembershipUser"/> object that represents the user to update and the updated information for the user.
        /// </param>
        /// <remarks>
        /// </remarks>
        public override void UpdateUser(MembershipUser user)
        {
            this.UpdateUser(this.ApplicationName, user);
        }

        /// <summary>
        /// Takes, as input, a MembershipUser object representing a registered user and updates the information stored
        ///   for that user in the membership data source.
        ///   Note that UpdateUser is not obligated to allow all the data that can be encapsulated in a
        ///   MembershipUser object to be updated in the data source.
        /// </summary>
        /// <param name="portalAlias">
        /// Appleseed's portal alias
        /// </param>
        /// <param name="user">
        /// A MembershipUser object representing a registered user
        /// </param>
        /// <exception cref="System.Configuration.Provider.ProviderException">
        /// If any of the input submitted in the MembershipUser object
        ///   is not valid, UpdateUser throws a ProviderException.
        /// </exception>
        /// <remarks>
        /// </remarks>
        public override void UpdateUser(string portalAlias, MembershipUser user)
        {
            var cmd = new SqlCommand
            {
                CommandText = "aspnet_Membership_UpdateUser",
                CommandType = CommandType.StoredProcedure,
                Connection = new SqlConnection(this.ConnectionString)
            };

            cmd.Parameters.Add("@ApplicationName", SqlDbType.NVarChar, 256).Value = portalAlias;
            cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 256).Value = user.UserName;
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 256).Value = user.Email;
            cmd.Parameters.Add("@Comment", SqlDbType.NText).Value = user.Comment;
            cmd.Parameters.Add("@IsApproved", SqlDbType.Bit).Value = user.IsApproved;
            cmd.Parameters.Add("@LastLoginDate", SqlDbType.DateTime).Value = user.LastLoginDate;
            cmd.Parameters.Add("@LastActivityDate", SqlDbType.DateTime).Value = user.LastActivityDate;
            cmd.Parameters.Add("@UniqueEmail", SqlDbType.Bit).Value = this.RequiresUniqueEmail;
            cmd.Parameters.Add("@CurrentTimeUtc", SqlDbType.DateTime).Value = DateTime.UtcNow;

            var totalRecordsParam = cmd.Parameters.Add("@TotalRecords", SqlDbType.Int);
            totalRecordsParam.Direction = ParameterDirection.ReturnValue;

            try
            {
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();

                this.SaveUserProfile((AppleseedUser)user);
                if (((int)totalRecordsParam.Value) != 0)
                {
                    throw new AppleseedMembershipProviderException("Error updating user");
                }
            }
            catch (SqlException e)
            {
                if (this.WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "UpdateUser");
                }

                throw new AppleseedMembershipProviderException(
                    "Error executing aspnet_Membership_UpdateUser stored proc", e);
            }
            catch (Exception e)
            {
                if (this.WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "UpdateUser");
                }

                throw new AppleseedMembershipProviderException("Error updating user (2)", e);
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        /// <summary>
        /// Verifies that the specified user name and password exist in the data source.
        /// </summary>
        /// <param name="username">
        /// The name of the user to validate.
        /// </param>
        /// <param name="password">
        /// The password for the specified user.
        /// </param>
        /// <returns>
        /// true if the specified username and password are valid; otherwise, false.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override bool ValidateUser(string username, string password)
        {
            return this.ValidateUser(this.ApplicationName, username, password);
        }

        /// <summary>
        /// Takes, as input, a user name and a password and verifies that they are valid-that is, that the membership
        ///   data source contains a matching user name and password. ValidateUser returns true if the user name and
        ///   password are valid, if the user is approved (that is, if MembershipUser.IsApproved is true), and if the
        ///   user isn't currently locked out. Otherwise, it returns false.
        ///   Following a successful validation, ValidateUser updates the user's LastLoginDate and fires an
        ///   AuditMembershipAuthenticationSuccess Web event. Following a failed validation, it fires an
        ///   AuditMembershipAuthenticationFailure Web event.
        /// </summary>
        /// <param name="portalAlias">
        /// Appleseed's portal alias
        /// </param>
        /// <param name="username">
        /// The user's name
        /// </param>
        /// <param name="password">
        /// The user's password
        /// </param>
        /// <returns>
        /// The validate user.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override bool ValidateUser(string portalAlias, string username, string password)
        {
            var conn = new SqlConnection(this.ConnectionString);

            var cmd = new SqlCommand
            {
                CommandText = "aspnet_Membership_GetPasswordWithFormat",
                CommandType = CommandType.StoredProcedure,
                Connection = conn
            };

            cmd.Parameters.Add("@ApplicationName", SqlDbType.NVarChar, 256).Value = portalAlias;
            cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 256).Value = username;
            cmd.Parameters.Add("@UpdateLastLoginActivityDate", SqlDbType.Int).Value = 1;
            cmd.Parameters.Add("@CurrentTimeUtc", SqlDbType.DateTime).Value = DateTime.UtcNow;

            var returnCode = cmd.Parameters.Add("@ReturnCode", SqlDbType.Int);
            returnCode.Direction = ParameterDirection.ReturnValue;

            SqlDataReader reader = null;
            var dbPassword = string.Empty;
            var dbPasswordSalt = string.Empty;
            var passwordFormat = MembershipPasswordFormat.Clear;

            int failedPasswordAttemptCount = 0;
            int failedPasswordAnswerAttemptCount = 0;
            bool isApproved;
            DateTime lastLoginDate = DateTime.Now;
            DateTime lastActivityDate = DateTime.Now;

            try
            {
                cmd.Connection.Open();

                using (reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (reader.HasRows)
                    {
                        reader.Read();

                        dbPassword = reader.GetString(0);
                        passwordFormat =
                            (MembershipPasswordFormat)
                            Enum.Parse(typeof(MembershipPasswordFormat), reader.GetInt32(1).ToString());
                        dbPasswordSalt = reader.GetString(2);

                        failedPasswordAttemptCount = reader.GetInt32(3);
                        failedPasswordAnswerAttemptCount = reader.GetInt32(4);
                        isApproved = reader.GetBoolean(5);
                        lastLoginDate = reader.GetDateTime(6);
                        lastActivityDate = reader.GetDateTime(7);
                    }

                    reader.Close();
                }

                var flag = this.CheckPassword(password, dbPassword, dbPasswordSalt, passwordFormat);

                if (flag && failedPasswordAttemptCount == 0 && failedPasswordAnswerAttemptCount == 0)
                    return true;

                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Membership_UpdateUserInfo", conn);
                DateTime utcNow = DateTime.UtcNow;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add("@ApplicationName", SqlDbType.NVarChar, 256).Value = portalAlias;
                sqlCommand.Parameters.Add("@UserName", SqlDbType.NVarChar, 256).Value = username;
                sqlCommand.Parameters.Add("@IsPasswordCorrect", SqlDbType.Bit).Value = (object)Convert.ToBoolean(flag ? 1 : 0);
                sqlCommand.Parameters.Add("@UpdateLastLoginActivityDate", SqlDbType.Bit).Value = (object)true;
                sqlCommand.Parameters.Add("@MaxInvalidPasswordAttempts", SqlDbType.Int).Value = (object)this.MaxInvalidPasswordAttempts;
                sqlCommand.Parameters.Add("@PasswordAttemptWindow", SqlDbType.Int).Value = (object)this.PasswordAttemptWindow;
                sqlCommand.Parameters.Add("@CurrentTimeUtc", SqlDbType.DateTime).Value = (object)utcNow;
                sqlCommand.Parameters.Add("@LastLoginDate", SqlDbType.DateTime).Value = (object)(flag ? utcNow : lastLoginDate);
                sqlCommand.Parameters.Add("@LastActivityDate", SqlDbType.DateTime).Value = (object)(flag ? utcNow : lastActivityDate);
                SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
                sqlParameter.Direction = ParameterDirection.ReturnValue;
                sqlCommand.Parameters.Add(sqlParameter);
                sqlCommand.ExecuteNonQuery();

                return flag;

            }
            catch (SqlException e)
            {
                if (this.WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "ValidateUser");
                }

                throw new AppleseedMembershipProviderException("Error validating user", e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                if (conn != null)
                    conn.Close();
            }
        }

        /// <summary>
        /// Checks if the users has that token associated.
        /// </summary>
        /// <param name="userId">
        /// The user id
        /// </param>
        /// <param name="tokenId">
        /// The token
        /// </param>
        /// <returns>
        /// True if the user has the token specified or false otherwise
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override bool VerifyTokenForUser(Guid userId, Guid tokenId)
        {
            using (var entities = new AppleseedMembershipEntities(ConfigurationManager.ConnectionStrings["AppleseedMembershipEntities"].ConnectionString))
            {
                var maxDays = 7;

                try
                {
                    maxDays = int.Parse(ConfigurationManager.AppSettings["MaxTokenDays"]);
                }
                catch (Exception)
                {
                    maxDays = 7;
                }
                try
                {
                    var token = entities.aspnet_ResetPasswordTokens.Include("aspnet_Membership").Single(t => t.UserId == userId && t.TokenId == tokenId
                        && t.aspnet_Membership.aspnet_Applications.ApplicationName.ToLower() == this.ApplicationName.ToLower());

                    if (token.CreationDate >= DateTime.Now.AddDays(-maxDays))
                    {
                        return true;

                    }
                    else
                    {
                        // The token is old

                        entities.aspnet_ResetPasswordTokens.DeleteObject(token);
                        entities.SaveChanges();
                        return false;
                    }
                }
                catch (Exception e)
                {

                    ErrorHandler.Publish(LogLevel.Error, e);
                    return false;
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// A helper function that takes the current row from the SqlDataReader and hydrates a MembershiUser from the values.
        ///   Called by the MembershipUser.GetUser implementation.
        /// </summary>
        /// <param name="reader">
        /// The reader.
        /// </param>
        /// <returns>
        /// A user.
        /// </returns>
        /// <remarks>
        /// </remarks>
        protected virtual AppleseedUser GetUserFromReader(SqlDataReader reader)
        {
            var username = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
            var email = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
            var passwordQuestion = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
            var comment = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
            var isApproved = reader.IsDBNull(4) ? false : reader.GetBoolean(4);
            var creationDate = reader.IsDBNull(5) ? DateTime.Now : reader.GetDateTime(5);
            var lastLoginDate = reader.IsDBNull(6) ? DateTime.Now : reader.GetDateTime(6);
            var lastActivityDate = reader.IsDBNull(7) ? DateTime.Now : reader.GetDateTime(7);
            var lastPasswordChangedDate = reader.IsDBNull(8) ? DateTime.Now : reader.GetDateTime(8);

            var providerUserKey = reader.GetGuid(9);
            var isLockedOut = reader.IsDBNull(10) ? false : reader.GetBoolean(10);
            var lastLockedOutDate = reader.IsDBNull(11) ? DateTime.Now : reader.GetDateTime(11);

            var u = this.InstantiateNewUser(
                this.Name,
                username,
                providerUserKey,
                email,
                passwordQuestion,
                comment,
                isApproved,
                isLockedOut,
                creationDate,
                lastLoginDate,
                lastActivityDate,
                lastPasswordChangedDate,
                lastLockedOutDate);

            return u;
        }

        /// <summary>
        /// A helper function to retrieve config values from the configuration file.
        /// </summary>
        /// <param name="configValue">
        /// The config value.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The get config value.
        /// </returns>
        /// <remarks>
        /// </remarks>
        private static string GetConfigValue(string configValue, string defaultValue)
        {
            return String.IsNullOrEmpty(configValue) ? defaultValue : configValue;
        }

        /// <summary>
        /// Converts a hexadecimal string to a byte array. Used to convert encryption key values from the configuration.
        /// </summary>
        /// <param name="hexString">The hex string.</param>
        /// <returns>The byte array.</returns>
        /// <remarks></remarks>
        private static byte[] HexToByte(string hexString)
        {
            var returnBytes = new byte[hexString.Length / 2];
            for (var i = 0; i < returnBytes.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return returnBytes;
        }

        /// <summary>
        /// Changes the user password.
        /// </summary>
        /// <param name="portalAlias">
        /// The portal alias.
        /// </param>
        /// <param name="username">
        /// The username.
        /// </param>
        /// <param name="newPassword">
        /// The new password.
        /// </param>
        /// <returns>
        /// The change user password.
        /// </returns>
        /// <remarks>
        /// </remarks>
        private bool ChangeUserPassword(string portalAlias, string username, string newPassword)
        {
            var args = new ValidatePasswordEventArgs(username, newPassword, true);

            this.OnValidatingPassword(args);

            if (args.Cancel)
            {
                throw args.FailureInformation ??
                      new MembershipPasswordException(
                          "Change password canceled due to new password validation failure.");
            }

            var passwordSalt = string.Empty;
            var encodedPassword = this.PasswordFormat == MembershipPasswordFormat.Hashed
                                      ? this.EncodePassword(passwordSalt + newPassword)
                                      : this.EncodePassword(newPassword);

            var cmd = new SqlCommand
            {
                CommandText = "aspnet_Membership_SetPassword",
                CommandType = CommandType.StoredProcedure,
                Connection = new SqlConnection(this.ConnectionString)
            };

            cmd.Parameters.Add("@ApplicationName", SqlDbType.NVarChar, 256).Value = portalAlias;
            cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 255).Value = username;
            cmd.Parameters.Add("@NewPassword", SqlDbType.NVarChar, 255).Value = encodedPassword;
            cmd.Parameters.Add("@PasswordSalt", SqlDbType.NVarChar, 255).Value = passwordSalt;
            cmd.Parameters.Add("@CurrentTimeUtc", SqlDbType.DateTime).Value = DateTime.UtcNow;
            cmd.Parameters.Add("@PasswordFormat", SqlDbType.Int).Value = this.PasswordFormat;

            var returnCode = cmd.Parameters.Add("@ReturnCode", SqlDbType.Int);
            returnCode.Direction = ParameterDirection.ReturnValue;

            try
            {
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();

                return ((int)returnCode.Value) == 0;
            }
            catch (SqlException e)
            {
                if (this.WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "ChangePassword");
                }

                throw new AppleseedMembershipProviderException(
                    "Error executing aspnet_Membership_SetPassword stored proc", e);
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        /// <summary>
        /// Compares password values based on the MembershipPasswordFormat.
        /// </summary>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <param name="dbpassword">
        /// The db password.
        /// </param>
        /// <param name="passwordSalt">
        /// The password Salt.
        /// </param>
        /// <param name="passwordFormat">
        /// The password Format.
        /// </param>
        /// <returns>
        /// The check password.
        /// </returns>
        /// <remarks>
        /// </remarks>
        private bool CheckPassword(
            string password, string dbpassword, string passwordSalt, MembershipPasswordFormat passwordFormat)
        {
            var pass1 = password;
            var pass2 = dbpassword;

            switch (passwordFormat)
            {
                case MembershipPasswordFormat.Encrypted:
                    pass1 = this.EncodePassword(dbpassword);
                    break;
                case MembershipPasswordFormat.Hashed:
                    pass1 = this.EncodePassword(passwordSalt + password);
                    break;
                default:
                    break;
            }

            return pass1.Equals(pass2);
        }

        /// <summary>
        /// Encrypts, Hashes, or leaves the password clear based on the PasswordFormat.
        /// </summary>
        /// <param name="password">
        /// the password
        /// </param>
        /// <returns>
        /// The encode password.
        /// </returns>
        private string EncodePassword(string password)
        {
            var encodedPassword = password;

            switch (this.PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    encodedPassword = Convert.ToBase64String(this.EncryptPassword(Encoding.Unicode.GetBytes(password)));
                    break;
                case MembershipPasswordFormat.Hashed:
                    var hash = new HMACSHA1 { Key = HexToByte(EncryptionKey) };
                    encodedPassword = Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)));
                    break;
                default:
                    throw new AppleseedMembershipProviderException("Unsupported password format.");
            }

            return encodedPassword;
        }

        /// <summary>
        /// Decrypts or leaves the password clear based on the PasswordFormat.
        /// </summary>
        /// <param name="encodedPassword">
        /// The encoded password.
        /// </param>
        /// <returns>
        /// The un encode password.
        /// </returns>
        /// <remarks>
        /// </remarks>
        private string UnEncodePassword(string encodedPassword)
        {
            var password = encodedPassword;

            switch (this.PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    password = Encoding.Unicode.GetString(this.DecryptPassword(Convert.FromBase64String(password)));
                    break;
                case MembershipPasswordFormat.Hashed:
                    throw new AppleseedMembershipProviderException("Cannot unencode a hashed password.");
                default:
                    throw new AppleseedMembershipProviderException("Unsupported password format.");
            }

            return password;
        }

        /// <summary>
        /// A helper function that writes exception detail to the event log. Exceptions are written to the event log as a security
        ///   measure to avoid private database details from being returned to the browser. If a method does not return a status
        ///   or Boolean indicating the action succeeded or failed, a generic exception is also thrown by the caller.
        /// </summary>
        /// <param name="e">
        /// The exception.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <remarks>
        /// </remarks>
        private static void WriteToEventLog(Exception e, string action)
        {
            var message = "An exception occurred communicating with the data source.\n\n";
            message += string.Format("Action: {0}\n\n", action);
            message += string.Format("Exception: {0}", e);
            ErrorHandler.Publish(LogLevel.Error, message, e);
        }

        #endregion
    }
}