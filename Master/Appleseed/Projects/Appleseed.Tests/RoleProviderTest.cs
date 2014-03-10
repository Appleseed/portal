using System;
using System.Collections.Generic;
using System.Web.Security;
using NUnit.Framework;
using Appleseed.Framework.Providers.AppleseedMembershipProvider;
using Appleseed.Framework.Providers.AppleseedRoleProvider;

namespace Appleseed.Tests
{
    [TestFixture]
    public class RoleProviderTest
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
                Console.WriteLine(ex.Message);
                Assert.Fail("Error retrieving ApplicationName property" + ex.Message, ex);
            }
        }

        #endregion

        [Test]
        public void GetAllRolesTest1()
        {
            try
            {
                string[] roles = Roles.GetAllRoles();
                Assert.AreEqual(4, roles.Length);
                Assert.AreEqual("All Users", roles[0]);
                Assert.AreEqual("Authenticated Users", roles[1]);
                Assert.AreEqual("Unauthenticated Users", roles[2]);
                Assert.AreEqual("Admins", roles[3]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail("Error in GetAllRolesTest1" + ex.Message, ex);
            }
        }

        [Test]
        public void GetAllRolesTest2()
        {
            try
            {
                AppleseedRoleProvider provider = Roles.Provider as AppleseedRoleProvider;

                IList<AppleseedRole> roles = provider.GetAllRoles(Roles.ApplicationName);
                Assert.AreEqual(4, roles.Count);
                Assert.AreEqual("All Users", roles[0].Name);
                Assert.AreEqual("Authenticated Users", roles[1].Name);
                Assert.AreEqual("Unauthenticated Users", roles[2].Name);
                Assert.AreEqual("Admins", roles[3].Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail("Error in GetAllRolesTest2" + ex.Message, ex);
            }
        }

        [Test]
        public void GetRolesForUserTest1()
        {
            try
            {
                AppleseedRoleProvider provider = Roles.Provider as AppleseedRoleProvider;

                Guid userId = new Guid("BE7DC028-7238-45D3-AF35-DD3FE4AEFB7E"); //"admin@Appleseedportal.net" 

                IList<AppleseedRole> roles = provider.GetRolesForUser(Roles.ApplicationName, userId);
                Assert.AreEqual(1, roles.Count);
                Assert.AreEqual("Admins", roles[0].Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail("Error in GetRolesForUserTest1" + ex.Message, ex);
            }
        }

        [Test]
        public void GetRolesForUserTest2()
        {
            try
            {
                AppleseedRoleProvider provider = Roles.Provider as AppleseedRoleProvider;

                IList<AppleseedRole> roles = provider.GetRolesForUser(Roles.ApplicationName, new Guid());
                Assert.Fail();
            }
            catch (AppleseedRoleProviderException ex) { }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail("Error in GetRolesForUserTest2" + ex.Message, ex);
            }
        }

        [Test]
        public void GetRolesForUserTest3()
        {
            try
            {
                AppleseedRoleProvider provider = Roles.Provider as AppleseedRoleProvider;

                Guid userId = new Guid("34ADB714-92B0-47ff-B5AF-5DB2E0D124A9"); //"user@user.com"
                IList<AppleseedRole> roles = provider.GetRolesForUser(Roles.ApplicationName, userId);
                Assert.AreEqual(roles.Count, 0);
            }
            catch (AppleseedRoleProviderException) { }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail("Error in GetRolesForUserTest3" + ex.Message, ex);
            }
        }

        [Test]
        public void CreateRoleTest1()
        {
            try
            {
                AppleseedRoleProvider provider = Roles.Provider as AppleseedRoleProvider;

                provider.CreateRole(Roles.ApplicationName, "editors");
                provider.CreateRole(Roles.ApplicationName, "clerks");
                provider.CreateRole(Roles.ApplicationName, "salesman");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail("Error in CreateRoleTest1" + ex.Message, ex);
            }
        }

        [Test]
        public void CreateRoleTest2()
        {
            try
            {
                AppleseedRoleProvider provider = Roles.Provider as AppleseedRoleProvider;

                provider.CreateRole(Roles.ApplicationName, "Admins");
                Assert.Fail();
            }
            catch (AppleseedRoleProviderException) { }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail("Error in CreateRoleTest2" + ex.Message, ex);
            }
        }

        [Test]
        public void CreateRoleTest3()
        {
            try
            {
                AppleseedRoleProvider provider = Roles.Provider as AppleseedRoleProvider;

                provider.CreateRole(Roles.ApplicationName, "Admins,editors");
                Assert.Fail();
            }
            catch (AppleseedRoleProviderException) { }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail("Error in CreateRoleTest3" + ex.Message, ex);
            }
        }

        [Test]
        public void IsUserInRoleTest1()
        {
            try
            {
                AppleseedRoleProvider provider = Roles.Provider as AppleseedRoleProvider;

                bool isInRole = provider.IsUserInRole("admin@Appleseedportal.net", "Admins");
                Assert.IsTrue(isInRole);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail("Error in IsUserInRoleTest1" + ex.Message, ex);
            }
        }

        [Test]
        public void IsUserInRoleTest2()
        {
            try
            {
                AppleseedRoleProvider provider = Roles.Provider as AppleseedRoleProvider;

                bool isInRole = provider.IsUserInRole("invalid@user.com", "Admins");
                Assert.Fail();
            }
            catch (AppleseedRoleProviderException) { }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail("Error in IsUserInRoleTest2" + ex.Message, ex);
            }
        }

        [Test]
        public void IsUserInRoleTest3()
        {
            try
            {
                AppleseedRoleProvider provider = Roles.Provider as AppleseedRoleProvider;

                bool isInRole = provider.IsUserInRole("admin@Appleseedportal.net", "invalidRole");
                Assert.Fail();
            }
            catch (AppleseedRoleProviderException) { }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail("Error in IsUserInRoleTest3" + ex.Message, ex);
            }
        }

        [Test]
        public void IsUserInRoleTest4()
        {
            try
            {
                AppleseedRoleProvider provider = Roles.Provider as AppleseedRoleProvider;

                bool isInRole = provider.IsUserInRole("admin@Appleseedportal.net", "editors");
                Assert.IsFalse(isInRole);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail("Error in IsUserInRoleTest4" + ex.Message, ex);
            }
        }

        [Test]
        public void RoleExistsTest1()
        {
            try
            {
                AppleseedRoleProvider provider = Roles.Provider as AppleseedRoleProvider;

                bool isInRole = provider.RoleExists("editors");
                Assert.IsTrue(isInRole);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail("Error in RoleExistsTest1" + ex.Message, ex);
            }
        }

        [Test]
        public void RoleExistsTestInvalidRoleName()
        {
            try
            {
                AppleseedRoleProvider provider = Roles.Provider as AppleseedRoleProvider;

                bool isInRole = provider.RoleExists("invalidRole");
                Assert.IsFalse(isInRole);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail("Error in RoleExistsTest2" + ex.Message, ex);
            }
        }

        [Test]
        public void GetUsersInRoleTest1()
        {
            try
            {
                AppleseedRoleProvider provider = Roles.Provider as AppleseedRoleProvider;

                string[] names = provider.GetUsersInRole("Admins");
                Assert.AreEqual(1, names.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail("Error in GetUsersInRoleTest1" + ex.Message, ex);
            }
        }


        [Test]
        public void GetUsersInRoleTest2()
        {
            try
            {
                AppleseedRoleProvider provider = Roles.Provider as AppleseedRoleProvider;

                string[] names = provider.GetUsersInRole("editors");
                Assert.AreEqual(0, names.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail("Error in GetUsersInRoleTest2" + ex.Message, ex);
            }
        }

        [Test]
        public void GetUsersInRoleTest3()
        {
            try
            {
                AppleseedRoleProvider provider = Roles.Provider as AppleseedRoleProvider;

                string[] names = provider.GetUsersInRole("invalidRole");
                Assert.Fail();
            }
            catch (AppleseedRoleProviderException) { }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail("Error in GetUsersInRoleTest3" + ex.Message, ex);
            }
        }

        [Test]
        public void AddUsersToRolesTest1()
        {
            try
            {
                AppleseedRoleProvider provider = Roles.Provider as AppleseedRoleProvider;

                string[] users = new string[1];
                users[0] = "invalid@user.com";

                string[] roles = new string[1];
                roles[0] = "Admins";

                provider.AddUsersToRoles(users, roles);
                Assert.Fail();
            }
            catch (AppleseedMembershipProviderException) { }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail("Error in AddUsersToRolesTest1" + ex.Message, ex);
            }
        }

        [Test]
        public void AddUsersToRolesTest2()
        {
            try
            {
                AppleseedRoleProvider provider = Roles.Provider as AppleseedRoleProvider;

                string[] users = new string[1];
                users[0] = "admin@Appleseedportal.net";

                string[] roles = new string[1];
                roles[0] = "invalidRole";

                provider.AddUsersToRoles(users, roles);
                Assert.Fail();
            }
            catch (AppleseedRoleProviderException) { }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail("Error in AddUsersToRolesTest2" + ex.Message, ex);
            }
        }

        [Test]
        public void AddUsersToRolesTest3()
        {
            try
            {
                AppleseedRoleProvider provider = Roles.Provider as AppleseedRoleProvider;

                string[] users = new string[1];
                users[0] = "admin@Appleseedportal.net";

                string[] roles = new string[1];
                roles[0] = "editors";
                provider.AddUsersToRoles(users, roles);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail("Error in AddUsersToRolesTest3" + ex.Message, ex);
            }
        }

        [Test]
        public void AddUsersToRolesTest4()
        {
            try
            {
                AppleseedRoleProvider provider = Roles.Provider as AppleseedRoleProvider;

                Guid[] users = new Guid[1];
                users[0] = Guid.NewGuid();

                Guid[] roles = new Guid[1];
                roles[0] = new Guid("F6A4ADDA-8450-4F9A-BE86-D0719B239A8D"); // Admins

                provider.AddUsersToRoles("Appleseed", users, roles);
                Assert.Fail();
            }
            catch (AppleseedMembershipProviderException) { }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail("Error in AddUsersToRolesTest4" + ex.Message, ex);
            }
        }

        [Test]
        public void AddUsersToRolesTest5()
        {
            try
            {
                AppleseedRoleProvider provider = Roles.Provider as AppleseedRoleProvider;

                Guid[] users = new Guid[1];
                users[0] = new Guid("BE7DC028-7238-45D3-AF35-DD3FE4AEFB7E"); //"admin@Appleseedportal.net";

                Guid[] roles = new Guid[1];
                roles[0] = Guid.NewGuid();

                provider.AddUsersToRoles("Appleseed", users, roles);
                Assert.Fail();
            }
            catch (AppleseedRoleProviderException) { }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail("Error in AddUsersToRolesTest5" + ex.Message, ex);
            }
        }

        [Test]
        public void AddUsersToRolesTest6()
        {
            try
            {
                AppleseedRoleProvider provider = Roles.Provider as AppleseedRoleProvider;

                AppleseedUser user = (AppleseedUser)Membership.GetUser("admin@Appleseedportal.net");
                Guid[] users = new Guid[1];
                users[0] = user.ProviderUserKey;

                AppleseedRole role = provider.GetRoleByName("Appleseed", "clerks");
                Guid[] roles = new Guid[1];
                roles[0] = role.Id;

                provider.AddUsersToRoles("Appleseed", users, roles);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail("Error in AddUsersToRolesTest6" + ex.Message, ex);
            }
        }

        [Test]
        public void RemoveUsersFromRolesTest1()
        {
            try
            {
                AppleseedRoleProvider provider = Roles.Provider as AppleseedRoleProvider;

                string[] users = new string[1];
                users[0] = "invalid@user.com";

                string[] roles = new string[1];
                roles[0] = "Admins";

                provider.RemoveUsersFromRoles(users, roles);
                Assert.Fail();
            }
            catch (AppleseedMembershipProviderException) { }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail("Error in RemoveUsersFromRolesTest1" + ex.Message, ex);
            }
        }

        [Test]
        public void RemoveUsersFromRolesTest2()
        {
            try
            {
                AppleseedRoleProvider provider = Roles.Provider as AppleseedRoleProvider;

                string[] users = new string[1];
                users[0] = "admin@Appleseedportal.net";

                string[] roles = new string[1];
                roles[0] = "invalidRole";

                provider.RemoveUsersFromRoles(users, roles);
                Assert.Fail();
            }
            catch (AppleseedRoleProviderException) { }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail("Error in RemoveUsersFromRolesTest2" + ex.Message, ex);
            }
        }

        [Test]
        public void RemoveUsersFromRolesTest3()
        {
            try
            {
                AppleseedRoleProvider provider = Roles.Provider as AppleseedRoleProvider;

                string[] users = new string[1];
                users[0] = "admin@Appleseedportal.net";

                string[] roles = new string[1];
                roles[0] = "editors";
                provider.RemoveUsersFromRoles(users, roles); // admin is in editors role
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail("Error in RemoveUsersFromRolesTest3" + ex.Message, ex);
            }
        }

        [Test]
        [Ignore("Temporarily until it will be fixed")]
        public void RemoveUsersFromRolesTest4()
        {
            try
            {
                AppleseedRoleProvider provider = Roles.Provider as AppleseedRoleProvider;

                Guid[] users = new Guid[1];
                users[0] = Guid.NewGuid();

                Guid[] roles = new Guid[1];
                roles[0] = new Guid("F6A4ADDA-8450-4F9A-BE86-D0719B239A8D"); // Admins

                provider.RemoveUsersFromRoles("Appleseed", users, roles);
                Assert.Fail();
            }
            catch (AppleseedRoleProviderException) { }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail("Error in RemoveUsersFromRolesTest4" + ex.Message, ex);
            }
        }

        [Test]
        public void RemoveUsersFromRolesTest5()
        {
            try
            {
                AppleseedRoleProvider provider = Roles.Provider as AppleseedRoleProvider;

                Guid[] users = new Guid[1];
                users[0] = new Guid("BE7DC028-7238-45D3-AF35-DD3FE4AEFB7E"); //"admin@Appleseedportal.net";

                Guid[] roles = new Guid[1];
                roles[0] = Guid.NewGuid();

                provider.RemoveUsersFromRoles("Appleseed", users, roles);
                Assert.Fail();
            }
            catch (AppleseedRoleProviderException) { }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail("Error in RemoveUsersFromRolesTest5" + ex.Message, ex);
            }
        }

        [Test]
        [Ignore("Temporarily until it will be fixed")]
        public void RemoveUsersFromRolesTest6()
        {
            try
            {
                AppleseedRoleProvider provider = Roles.Provider as AppleseedRoleProvider;

                Guid[] users = new Guid[1];
                users[0] = new Guid("BE7DC028-7238-45D3-AF35-DD3FE4AEFB7E"); //"admin@Appleseedportal.net";

                AppleseedRole editors = provider.GetRoleByName("Appleseed", "salesman");
                Guid[] roles = new Guid[1];
                roles[0] = editors.Id;

                provider.RemoveUsersFromRoles("Appleseed", users, roles);
                Assert.Fail();
            }
            catch (AppleseedRoleProviderException) { }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail("Error in RemoveUsersFromRolesTest6" + ex.Message, ex);
            }
        }

        [Test]
        [Ignore("Temporarily until it will be fixed")]
        public void RemoveUsersFromRolesTest7()
        {
            try
            {
                AppleseedRoleProvider provider = Roles.Provider as AppleseedRoleProvider;

                Guid userId = new Guid("34ADB714-92B0-47ff-B5AF-5DB2E0D124A9"); // user@user.com;

                Guid[] users = new Guid[] { userId };

                AppleseedRole editors = provider.GetRoleByName("Appleseed", "editors");
                Guid[] roles = new Guid[1];
                roles[0] = editors.Id;

                provider.AddUsersToRoles("Appleseed", users, roles);
                Assert.IsTrue(provider.IsUserInRole("Appleseed", userId, editors.Id));

                provider.RemoveUsersFromRoles("Appleseed", users, roles);
                Assert.IsFalse(provider.IsUserInRole("Appleseed", userId, editors.Id));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail("Error in RemoveUsersFromRolesTest7" + ex.Message, ex);
            }
        }

        [Test]
        [Ignore("Temporarily until it will be fixed")]
        public void RemoveUsersFromRolesTest8()
        {
            try
            {
                AppleseedRoleProvider provider = Roles.Provider as AppleseedRoleProvider;

                string[] users = new string[] { "user@user.com" };

                string[] roles = new string[] { "editors" };

                provider.AddUsersToRoles(users, roles);
                Assert.IsTrue(provider.IsUserInRole("user@user.com", "editors"));

                provider.RemoveUsersFromRoles(users, roles);
                Assert.IsFalse(provider.IsUserInRole("user@user.com", "editors"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail("Error in RemoveUsersFromRolesTest8" + ex.Message, ex);
            }
        }

        [Test]
        public void DeleteRoleTest1()
        {
            try
            {
                AppleseedRoleProvider provider = Roles.Provider as AppleseedRoleProvider;

                provider.DeleteRole("Admins", true);
                Assert.Fail(); // Admins has users
            }
            catch (AppleseedRoleProviderException) { }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail("Error in DeleteRoleTest1" + ex.Message, ex);
            }
        }

        [Test]
        public void DeleteRoleTest2()
        {
            try
            {
                AppleseedRoleProvider provider = Roles.Provider as AppleseedRoleProvider;

                provider.CreateRole("tempRole1");
                provider.DeleteRole("tempRole1", true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail("Error in DeleteRoleTest2" + ex.Message, ex);
            }
        }

        [Test]
        public void DeleteRoleTest3()
        {
            try
            {
                AppleseedRoleProvider provider = Roles.Provider as AppleseedRoleProvider;

                provider.CreateRole("tempRole2");

                provider.AddUsersToRoles(new string[] { "user@user.com" }, new string[] { "tempRole2" });

                provider.DeleteRole("tempRole2", false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail("Error in DeleteRoleTest3" + ex.Message, ex);
            }
        }

        [Test]
        public void DeleteRoleTest4()
        {
            try
            {
                AppleseedRoleProvider provider = Roles.Provider as AppleseedRoleProvider;

                AppleseedRole editors = provider.GetRoleByName("Appleseed", "editors");
                provider.DeleteRole("invalidApp", editors.Id, true);
                Assert.Fail();
            }
            catch (AppleseedRoleProviderException) { }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail("Error in DeleteRoleTest4" + ex.Message, ex);
            }
        }

        [Test]
        public void DeleteRoleTest5()
        {
            try
            {
                AppleseedRoleProvider provider = Roles.Provider as AppleseedRoleProvider;

                provider.DeleteRole("invalidRole", true);
                Assert.Fail();
            }
            catch (AppleseedRoleProviderException) { }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail("Error in DeleteRoleTest5" + ex.Message, ex);
            }
        }

        /* 

        public abstract string[] FindUsersInRole( string portalAlias, string roleName, string usernameToMatch );

        rename role
         
         */
    }
}