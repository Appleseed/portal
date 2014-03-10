using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using Appleseed.Framework.Providers.AppleseedMembershipProvider;
using System.Collections.Specialized;
using System.Web.Security;
using System.Data.SqlClient;

namespace Appleseed.Tests
{

    [TestFixture]
    public class MembershipProviderTest
    {

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            // Set up initial database environment for testing purposes
            TestHelper.TearDownDB();
            TestHelper.RecreateDBSchema();
        }

        [Test]
        public void Foo()
        {
            Console.WriteLine("This should pass. It only writes to the Console.");
        }

        #region Config properties

        [Test]
        public void ApplicationNameTest()
        {
            try
            {
                string appName = Membership.ApplicationName;
                Assert.AreEqual(appName, "Appleseed");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error retrieving ApplicationName property", ex);
            }
        }

        [Test]
        public void EnablePasswordResetTest()
        {
            try
            {
                bool enablePwdReset = Membership.EnablePasswordReset;
                Assert.AreEqual(enablePwdReset, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error retrieving EnablePasswordReset property", ex);
            }
        }

        [Test]
        public void EnablePasswordRetrievalTest()
        {
            try
            {
                bool enablePwdRetrieval = Membership.EnablePasswordRetrieval;
                Assert.AreEqual(enablePwdRetrieval, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error retrieving EnablePasswordRetrieval property", ex);
            }
        }

        [Test]
        public void HashAlgorithmTypeTest()
        {
            try
            {
                string hashAlgType = Membership.HashAlgorithmType;
                Assert.AreEqual(hashAlgType, "SHA1");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error retrieving HashAlgorithmType property", ex);
            }
        }

        [Test]
        public void MaxInvalidPasswordAttemptsTest()
        {
            try
            {
                int maxInvalidPwdAttempts = Membership.MaxInvalidPasswordAttempts;
                Assert.AreEqual(maxInvalidPwdAttempts, 5);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error retrieving MaxInvalidPasswordAttempts property", ex);
            }
        }

        [Test]
        public void MinRequiredNonAlphanumericCharactersTest()
        {
            try
            {
                int minReqNonAlpha = Membership.MinRequiredNonAlphanumericCharacters;
                Assert.AreEqual(minReqNonAlpha, 1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error retrieving MinRequiredNonAlphanumericCharacters property", ex);
            }
        }

        [Test]
        public void MinRequiredPasswordLengthTest()
        {
            try
            {
                int minReqPwdLength = Membership.MinRequiredPasswordLength;
                Assert.AreEqual(minReqPwdLength, 5);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error retrieving MinRequiredPasswordLength property", ex);
            }
        }

        [Test]
        public void PasswordAttemptWindowTest()
        {
            try
            {
                int pwdAttemptWindow = Membership.PasswordAttemptWindow;
                Assert.AreEqual(pwdAttemptWindow, 15);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error retrieving PasswordAttemptWindows property", ex);
            }
        }

        [Test]
        public void PasswordStrengthRegularExpressionTest()
        {
            try
            {
                string pwdStrengthRegex = Membership.PasswordStrengthRegularExpression;
                Assert.AreEqual(pwdStrengthRegex, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error retrieving PasswordStrengthRegularExpression property", ex);
            }
        }

        [Test]
        public void ProviderTest()
        {
            try
            {
                MembershipProvider provider = Membership.Provider;
                Assert.AreEqual(provider.GetType(), typeof(Appleseed.Framework.Providers.AppleseedMembershipProvider.AppleseedSqlMembershipProvider));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error retrieving Provider property", ex);
            }
        }

        [Test]
        public void ProvidersTest()
        {
            try
            {
                MembershipProviderCollection providers = Membership.Providers;
                Assert.AreEqual(providers.Count, 1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error retrieving Providers property", ex);
            }
        }

        [Test]
        public void RequiresQuestionAndAnswerTest()
        {
            try
            {
                bool reqQuestionAndAnswer = Membership.RequiresQuestionAndAnswer;
                Assert.AreEqual(reqQuestionAndAnswer, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error retrieving RequiresQuestionAndAnswer property", ex);
            }
        }

        #endregion

        #region Membership provider methods

        [Test]
        public void GetAllUsersTest()
        {
            try
            {
                int totalRecords;
                Membership.GetAllUsers();
                Membership.GetAllUsers(0, 1, out totalRecords);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error in GetAllUsersTest", ex);
            }
        }

        [Test]
        public void GetNumberOfUsersOnlineTest()
        {
            try
            {
                Membership.GetNumberOfUsersOnline();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error in GetNumberOfUsersOnlineTest", ex);
            }
        }

        [Test]
        public void GetPasswordTest()
        {
            try
            {
                if (Membership.EnablePasswordRetrieval)
                {
                    string pwd = Membership.Provider.GetPassword("admin@Appleseedportal.net", "answer");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error in GetPasswordTest", ex);
            }
        }

        [Test]
        public void GetUserTest()
        {
            try
            {
                MembershipUser user = Membership.GetUser("admin@Appleseedportal.net");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error in GetUserTest", ex);
            }
        }

        [Test]
        public void GetUserNameByEmailValidUserTest()
        {
            try
            {
                string userName = Membership.GetUserNameByEmail("admin@Appleseedportal.net");
                Assert.AreEqual(userName, "admin@Appleseedportal.net");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error in GetUserNameByEmailValidUserTest", ex);
            }
        }

        [Test]
        public void GetUserNameByEmailInvalidUserTest()
        {
            try
            {
                string userName = Membership.GetUserNameByEmail("invaliduser@doesnotexist.com");
                Assert.AreEqual(userName, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error in GetUserNameByEmailInvalidUserTest", ex);
            }
        }

        [Test]
        public void FindUsersByNameTest1()
        {
            try
            {
                MembershipUserCollection users = Membership.FindUsersByName("admin@Appleseedportal.net");
                Assert.AreEqual(users.Count, 1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error in FindUsersByNameTest1", ex);
            }
        }

        [Test]
        public void FindUsersByNameTest2()
        {
            try
            {
                MembershipUserCollection users = Membership.FindUsersByName("invaliduser@doesnotexist.com");
                Assert.IsEmpty(users);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error in FindUsersByNameTest2", ex);
            }
        }

        [Test]
        public void FindUsersByNameTest3()
        {
            try
            {
                int totalRecords = -1;
                MembershipUserCollection users = Membership.FindUsersByName("admin@Appleseedportal.net", 0, 10, out totalRecords);
                Assert.AreEqual(users.Count, 1);
                Assert.Greater(totalRecords, 0);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error in FindUsersByNameTest3", ex);
            }
        }

        [Test]
        public void FindUsersByNameTest4()
        {
            try
            {
                int totalRecords = -1;
                MembershipUserCollection users = Membership.FindUsersByName("invaliduser@doesnotexist.com", 0, 10, out totalRecords);
                Assert.IsEmpty(users);
                Assert.AreEqual(totalRecords, 0);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error in FindUsersByNameTest4", ex);
            }
        }

        [Test]
        public void FindUsersByEmailTest1()
        {
            try
            {
                MembershipUserCollection users = Membership.FindUsersByEmail("admin@Appleseedportal.net");
                Assert.AreEqual(users.Count, 1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error in FindUsersByEmailTest1", ex);
            }
        }

        [Test]
        public void FindUsersByEmailTest2()
        {
            try
            {
                MembershipUserCollection users = Membership.FindUsersByName("invaliduser@doesnotexist.com");
                Assert.IsEmpty(users);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error in FindUsersByEmailTest2", ex);
            }
        }

        [Test]
        public void FindUsersByEmailTest3()
        {
            try
            {
                int totalRecords = -1;
                MembershipUserCollection users = Membership.FindUsersByEmail("admin@Appleseedportal.net", 0, 10, out totalRecords);
                Assert.AreEqual(users.Count, 1);
                Assert.Greater(totalRecords, 0);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error in FindUsersByEmailTest3", ex);
            }
        }

        [Test]
        public void FindUsersByEmailTest4()
        {
            try
            {
                int totalRecords = -1;
                MembershipUserCollection users = Membership.FindUsersByEmail("invaliduser@doesnotexist.com", 0, 10, out totalRecords);
                Assert.IsEmpty(users);
                Assert.AreEqual(totalRecords, 0);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error in FindUsersByEmailTes4", ex);
            }
        }

        [Test]
        public void ValidateUserTest1()
        {
            try
            {
                bool isValid = Membership.ValidateUser("admin@Appleseedportal.net", "notavalidpwd");
                Assert.IsFalse(isValid);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error in ValidateUserTest1", ex);
            }
        }

        [Test]
        public void ValidateUserTest2()
        {
            try
            {
                bool isValid = Membership.ValidateUser("admin@Appleseedportal.net", "admin");
                Assert.IsTrue(isValid);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error in ValidateUserTest2", ex);
            }
        }

        [Test]
        public void ValidateUserTest3()
        {
            try
            {
                bool isValid = Membership.ValidateUser("invaliduser@doesnotexist.com", "notavalidpwd");
                Assert.IsFalse(isValid);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error in ValidateUserTest3", ex);
            }
        }

        [Test]
        public void ValidateUserTest4()
        {
            try
            {
                bool isValid = Membership.ValidateUser("invaliduser@doesnotexist.com", "admin");
                Assert.IsFalse(isValid);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error in ValidateUserTest4", ex);
            }
        }

        [Test]
        public void ChangePasswordQuestionAndAnswerTest1()
        {
            try
            {
                bool pwdChanged = Membership.Provider.ChangePasswordQuestionAndAnswer("admin@Appleseedportal.net", "admin", "newPasswordQuestion", "newPasswordAnswer");
                Assert.IsTrue(pwdChanged);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error in ChangePasswordQuestionAndAnswer1", ex);
            }
        }

        [Test]
        public void ChangePasswordQuestionAndAnswerTest2()
        {
            try
            {
                bool pwdChanged = Membership.Provider.ChangePasswordQuestionAndAnswer("admin@Appleseedportal.net", "invalidPwd", "newPasswordQuestion", "newPasswordAnswer");
                Assert.IsFalse(pwdChanged);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error in ChangePasswordQuestionAndAnswer2", ex);
            }
        }

        [Test]
        public void ChangePasswordQuestionAndAnswerTest3()
        {
            try
            {
                bool pwdChanged = Membership.Provider.ChangePasswordQuestionAndAnswer("invaliduser@doesnotexist.com", "InvalidPwd", "newPasswordQuestion", "newPasswordAnswer");
                Assert.IsFalse(pwdChanged);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error in ChangePasswordQuestionAndAnswer3", ex);
            }
        }

        [Test]
        public void UnlockUserTest1()
        {
            try
            {
                bool unlocked = Membership.Provider.UnlockUser("admin@Appleseedportal.net");
                Assert.IsTrue(unlocked);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error in UnlockUserTest1", ex);
            }
        }

        [Test]
        public void UnlockUserTest2()
        {
            try
            {
                bool unlocked = Membership.Provider.UnlockUser("invaliduser@doesnotexist.com");
                Assert.IsFalse(unlocked);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error in UnlockUserTest2", ex);
            }
        }

        [Test]
        public void CreateUserTest1()
        {
            try
            {
                MembershipCreateStatus status;
                MembershipUser user = Membership.CreateUser("Admin@Appleseedportal.net", "admin", "Admin@Appleseedportal.net", "question", "answer", true, out status);
                Assert.IsNull(user);
                Assert.AreEqual(status, MembershipCreateStatus.DuplicateUserName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error in CreateUserTest1", ex);
            }
        }

        [Test]
        public void CreateUserTest2()
        {
            try
            {
                MembershipCreateStatus status;
                MembershipUser user = Membership.CreateUser("Tito", "tito", "tito@tito.com", "question", "answer", true, out status);

                Assert.IsNotNull(user);
                Assert.AreEqual(status, MembershipCreateStatus.Success);
                Assert.AreEqual(user.UserName, "Tito");
                Assert.AreEqual(user.Email, "tito@tito.com");
                Assert.AreEqual(user.PasswordQuestion, "question");
                Assert.IsTrue(user.IsApproved);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error in CreateUserTest2", ex);
            }
        }

        [Test]
        public void ChangePasswordToExistingUserTest()
        {
            try
            {
                bool sucess = Membership.Provider.ChangePassword("Tito", "tito", "newPassword");

                Assert.IsTrue(sucess);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error in ChangePasswordTest1", ex);
            }
        }

        [Test]
        public void ChangePasswordToInexistentUserTest()
        {
            try
            {
                bool sucess = Membership.Provider.ChangePassword("invaliduser@doesnotexist.com", "pwd", "newPassword");

                Assert.IsFalse(sucess);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error in ChangePasswordTest2", ex);
            }
        }

        [Test]
        public void ChangePasswordInvalidCurrentPassorwdTest()
        {
            try
            {
                bool sucess = Membership.Provider.ChangePassword("Admin@Appleseedportal.net", "invalidPwd", "newPassword");

                Assert.IsFalse(sucess);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error in ChangePasswordTest3", ex);
            }
        }

        [Test]
        public void UpdateExistingUserTest()
        {
            try
            {
                AppleseedUser user = (AppleseedUser)Membership.GetUser("Tito");

                Assert.AreEqual(user.Email, "tito@tito.com");
                Assert.IsTrue(user.IsApproved);

                user.Email = "newEmail@tito.com";
                user.IsApproved = false;
                user.LastLoginDate = new DateTime(1982, 2, 6);

                Membership.UpdateUser(user);

                user = (AppleseedUser)Membership.GetUser("Tito");
                Assert.AreEqual(user.Email, "newEmail@tito.com");
                Assert.IsFalse(user.IsApproved);
                Assert.AreEqual(new DateTime(1982, 2, 6), user.LastLoginDate);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error in UpdateUserTest1", ex);
            }
        }

        [Test]
        public void UpdateUserTest2()
        {
            try
            {
                AppleseedUser user = new AppleseedUser(Membership.Provider.Name, "invalidUserName", Guid.NewGuid(), "tito@tito.com", "question", "answer", true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.MinValue);

                Membership.UpdateUser(user);

                Assert.Fail("UpdateUser didn't throw an exception even though userName was invalid");
            }
            catch (AppleseedMembershipProviderException)
            {
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error in UpdateUserTest1", ex);
            }
        }

        [Test]
        public void ResetPasswordTest1()
        {
            try
            {
                string newPwd = Membership.Provider.ResetPassword("invalidUser", "answer");

                Assert.Fail("ResetPassword went ok with invalid user name");
            }
            catch (AppleseedMembershipProviderException)
            {
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error in ResetPasswordTest1", ex);
            }
        }

        [Test]
        public void ResetPasswordTest2()
        {
            try
            {
                string newPwd = Membership.Provider.ResetPassword("Tito", "invalidAnswer");
                Assert.Fail("ResetPassword went ok with invalid password answer");
            }
            catch (MembershipPasswordException)
            {
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error in ResetPasswordTest2", ex);
            }
        }

        [Test]
        public void ResetPasswordTest3()
        {
            try
            {
                string newPwd = Membership.Provider.ResetPassword("Tito", "answer");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error in ResetPasswordTest3", ex);
            }
        }

        [Test]
        public void DeleteUserTest1()
        {
            try
            {
                bool success = Membership.DeleteUser("invalidUser");
                Assert.IsFalse(success);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error in DeleteUserTest1", ex);
            }
        }

        [Test]
        public void DeleteUserTest2()
        {
            try
            {
                bool success = Membership.DeleteUser("Tito");
                Assert.IsTrue(success);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                Assert.Fail("Error in DeleteUserTest2", ex);
            }
        }
        #endregion
    }
}
