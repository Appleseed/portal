// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ADHelper.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   This class contains functions for Active Directory support
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Helpers
{
    using System;
    using System.Collections;
    using System.Data;
    using System.DirectoryServices;
    using System.Linq;
    using System.Web.Caching;

    using Appleseed.Framework.DataTypes;
    using Appleseed.Framework.Settings;

    /// <summary>
    /// This class contains functions for Active Directory support
    /// </summary>
    /// <remarks>
    /// </remarks>
    [History("gman3001", "2004/10/26", 
        "Added Method to retrieve a list of all AD Groups that a user account is a member of.")]
    public class ADHelper
    {
        #region Enums

        /// <summary>
        /// The ad account type.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public enum ADAccountType
        {
            /// <summary>
            /// The none.
            /// </summary>
            None = 0, 

            /// <summary>
            /// The user.
            /// </summary>
            user = 1, 

            /// <summary>
            /// The group.
            /// </summary>
            group = 2
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This function returns an EmailAddressList object.
        /// </summary>
        /// <param name="Account">
        /// Windows user or group
        /// </param>
        /// <returns>
        /// EmailAddressList
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static EmailAddressList GetEmailAddresses(string Account)
        {
            // Lookup the domain in which we must look for the user or group
            var account = Account.Split("\\".ToCharArray());
            if (account.Length != 2)
            {
                return new EmailAddressList(); // not a valid windows account!
            }

            DirectoryEntry rootEntry = null;
            var domains = Config.ADdns.Split(";".ToCharArray());

            // jes1111 - ConfigurationSettings.AppSettings["ADdns"].Split(";".ToCharArray());
            // NT domains do not keep track of email addresses
            foreach (var t in domains.Where(t => !t.Trim().ToLower().StartsWith("winnt://")))
            {
                rootEntry = GetDomainRoot(t);
                if (GetNetbiosName(rootEntry).Trim().ToLower() == account[0].Trim().ToLower())
                {
                    break;
                }
                
                rootEntry = null;
            }

            // Unknown domain : return empty list
            if (rootEntry == null)
            {
                return new EmailAddressList();
            }

            // Domain found: lets lookup the object
            DirectoryEntry entry;
            using (var mySearcher = new DirectorySearcher(rootEntry))
            {
                mySearcher.Filter =
                    "(&(|(objectClass=group)(&(objectClass=user)(objectCategory=person)))(sAMAccountName=" + account[1] +
                    "))";
                mySearcher.PropertiesToLoad.Add("mail");
                mySearcher.PropertiesToLoad.Add("objectClass");
                mySearcher.PropertiesToLoad.Add("member");

                try
                {
                    entry = mySearcher.FindOne().GetDirectoryEntry();
                }
                catch
                {
                    throw new Exception(string.Format("Could not get users/groups from domain '{0}'.", account[0]));
                }
            }

            // determine account type
            var accounttype = GetAccountType(entry);

            var eal = new EmailAddressList();

            // object is user --> retrieve its email address and return
            if (accounttype == ADAccountType.user)
            {
                try
                {
                    eal.Add(entry.Properties["mail"][0]);
                }
                catch
                {
                }

                return eal;
            }

            // object is group --> retrieve all users that are contained 
            // in the group or in groups of the group 
            GetUsersInGroup(entry, eal, new ArrayList());
            return eal;
        }

        /// <summary>
        /// Gets the member list.
        /// </summary>
        /// <param name="Refresh">
        /// if set to <c>true</c> [refresh].
        /// </param>
        /// <param name="ADDomain">
        /// The AD domain.
        /// </param>
        /// <param name="AppCache">
        /// The app cache.
        /// </param>
        /// <returns>
        /// A System.Data.DataTable value...
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static DataTable GetMemberList(bool Refresh, string ADDomain, Cache AppCache)
        {
            // see if we want to refresh, if not, get it from the cache if available
            if (! Refresh)
            {
                var tmp = AppCache.Get("ADUsersAndGroups" + ADDomain);
                if (tmp != null)
                {
                    return ((DataSet)tmp).Tables[0];
                }
            }

            // create dataset
            using (var ds = new DataSet())
            {
                using (var dt = new DataTable())
                {
                    ds.Tables.Add(dt);

                    var dc = new DataColumn("DisplayName", typeof(string));
                    dt.Columns.Add(dc);
                    dc = new DataColumn("AccountName", typeof(string));
                    dt.Columns.Add(dc);
                    dc = new DataColumn("AccountType", typeof(string));
                    dt.Columns.Add(dc);

                    // add built in users first
                    dt.Rows.Add(new object[] { "Admins", "Admins", "group" });
                    dt.Rows.Add(new object[] { "All Users", "All Users", "group" });
                    dt.Rows.Add(new object[] { "Authenticated Users", "Authenticated Users", "group" });
                    dt.Rows.Add(new object[] { "Unauthenticated Users", "Unauthenticated Users", "group" });

                    // construct root entry
                    using (var rootEntry = GetDomainRoot(ADDomain))
                    {
                        if (ADDomain.Trim().ToLower().StartsWith("ldap://"))
                        {
                            var DomainName = GetNetbiosName(rootEntry);

                            // get users/groups
                            var mySearcher = new DirectorySearcher(rootEntry);
                            mySearcher.Filter = "(|(objectClass=group)(&(objectClass=user)(objectCategory=person)))";
                            mySearcher.PropertiesToLoad.Add("cn");
                            mySearcher.PropertiesToLoad.Add("objectClass");
                            mySearcher.PropertiesToLoad.Add("sAMAccountName");

                            SearchResultCollection mySearcherSearchResult;
                            try
                            {
                                mySearcherSearchResult = mySearcher.FindAll();
                                foreach (SearchResult resEnt in mySearcherSearchResult)
                                {
                                    var entry = resEnt.GetDirectoryEntry();
                                    var name = (string)entry.Properties["cn"][0];
                                    var abbreviation = (string)entry.Properties["sAMAccountName"][0];
                                    var accounttype = GetAccountType(entry);
                                    dt.Rows.Add(
                                        new object[] { name, DomainName + "\\" + abbreviation, accounttype.ToString() });
                                }
                            }
                            catch
                            {
                                throw new Exception("Could not get users/groups from domain '" + ADDomain + "'.");
                            }
                        }
                        else if (ADDomain.Trim().ToLower().StartsWith("winnt://"))
                        {
                            var DomainName = rootEntry.Name;

                            // Get the users
                            rootEntry.Children.SchemaFilter.Add("user");
                            foreach (DirectoryEntry user in rootEntry.Children)
                            {
                                var fullname = (string)user.Properties["FullName"][0];
                                var accountname = user.Name;
                                dt.Rows.Add(
                                    new object[]
                                        {
                                           fullname, DomainName + "\\" + fullname, ADAccountType.user.ToString() 
                                        });
                            }

                            // Get the users
                            rootEntry.Children.SchemaFilter.Add("group");
                            foreach (DirectoryEntry user in rootEntry.Children)
                            {
                                var fullname = user.Name;
                                var accountname = user.Name;
                                dt.Rows.Add(
                                    new object[]
                                        {
                                           fullname, DomainName + "\\" + fullname, ADAccountType.group.ToString() 
                                        });
                            }
                        }
                    }

                    // add dataset to the cache
                    AppCache.Insert("ADUsersAndGroups" + ADDomain, ds);

                    // return datatable
                    return dt;
                }
            }
        }

        // Added by gman3001: 2004/10/26
        /// <summary>
        /// This function returns an array of strings consisting of the AD groups that this user belongs to.
        /// </summary>
        /// <param name="UserAccount">
        /// Windows user or group
        /// </param>
        /// <returns>
        /// Groups string array
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static string[] GetUserGroups(string UserAccount)
        {
            var UGroups = new ArrayList();
            if (UserAccount != null)
            {
                // Lookup the domain in which we must look for the user or group
                var account = UserAccount.Split("\\".ToCharArray());
                if (account.Length != 2)
                {
                    return (string[])UGroups.ToArray(typeof(string)); // not a valid windows account!
                }

                DirectoryEntry rootEntry = null;
                var domains = Config.ADdns.Split(";".ToCharArray());

                // jes1111 - ConfigurationSettings.AppSettings["ADdns"].Split(";".ToCharArray());
                for (var i = 0; i < domains.Length; i++)
                {
                    if (domains[i].Trim().ToLower().StartsWith("winnt://"))
                    {
                        continue; // NT domains do not keep track of email addresses
                    }

                    rootEntry = GetDomainRoot(domains[i]);
                    if (GetNetbiosName(rootEntry).Trim().ToLower() == account[0].Trim().ToLower())
                    {
                        break;
                    }
                    else
                    {
                        rootEntry = null;
                    }
                }

                // Unknown domain : return empty list
                if (rootEntry == null)
                {
                    return (string[])UGroups.ToArray(typeof(string));
                }

                // Domain found: lets lookup the object
                var mySearcher = new DirectorySearcher(rootEntry);
                mySearcher.Filter =
                    "(&(|(objectClass=group)(&(objectClass=user)(objectCategory=person)))(sAMAccountName=" + account[1] +
                    "))";
                mySearcher.PropertiesToLoad.Add("objectClass");
                mySearcher.PropertiesToLoad.Add("member");
                mySearcher.PropertiesToLoad.Add("sAMAccountName");
                mySearcher.PropertiesToLoad.Add("displayName");

                DirectoryEntry entry = null;
                try
                {
                    entry = mySearcher.FindOne().GetDirectoryEntry();
                }
                catch
                {
                    throw new Exception(
                        "Could not get users/groups from domain '" + account[0] +
                        "'. Either the user/group does not exist, or you do not have the necessary permissions in the Active Directory.");
                }

                // no object found
                if (entry == null)
                {
                    return (string[])UGroups.ToArray(typeof(string));
                }

                // determine accounttype
                var accounttype = GetAccountType(entry);

                // object is user --> retrieve the name of the groups it is a member of and return
                if (accounttype == ADAccountType.user)
                {
                    try
                    {
                        var values = entry.Properties["sAMAccountName"];
                        var accountPath = string.Empty;
                        if (values != null && values.Count > 0)
                        {
                            accountPath = account[0] + "\\" + values[0];
                            UGroups.Add(accountPath);
                        }

                        // Add generic system groups to the group list, since this user was authenticated when they entered the website
                        // this user is in the 'Authenticated Users' and 'All Users' groups by default
                        UGroups.Add("Authenticated Users");
                        UGroups.Add("All Users");

                        var rootPath = entry.Path;
                        rootPath = rootPath.Substring(0, rootPath.IndexOf("/", 7) + 1);

                        for (var i = 0; i < entry.Properties["memberOf"].Count; i++)
                        {
                            var currentEntry = new DirectoryEntry(rootPath + entry.Properties["memberOf"][i]);
                            if (GetAccountType(currentEntry) == ADAccountType.group)
                            {
                                // add to group list if this is a group
                                values = currentEntry.Properties["sAMAccountName"];
                                if (values != null && values.Count > 0)
                                {
                                    var GroupName = account[0] + "\\" + values[0];
                                    if (!UGroups.Contains(GroupName))
                                    {
                                        UGroups.Add(GroupName);
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }

            return (string[])UGroups.ToArray(typeof(string));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the type of the account.
        /// </summary>
        /// <param name="entry">
        /// The entry.
        /// </param>
        /// <returns>
        /// A Appleseed.Framework.Helpers.ADHelper.ADAccountType value...
        /// </returns>
        /// <remarks>
        /// </remarks>
        private static ADAccountType GetAccountType(DirectoryEntry entry)
        {
            var accounttype = ADAccountType.user;
            var objectClass = entry.Properties["objectClass"];
            if (objectClass.Cast<string>().Any(t => t == "group"))
            {
                accounttype = ADAccountType.group;
            }

            return accounttype;
        }

        /// <summary>
        /// Gets the domain root.
        /// </summary>
        /// <param name="Domain">
        /// The domain.
        /// </param>
        /// <returns>
        /// A System.DirectoryServices.DirectoryEntry value...
        /// </returns>
        /// <remarks>
        /// </remarks>
        private static DirectoryEntry GetDomainRoot(string Domain)
        {
            // 2004-07-28, Leo Duran, Fix for IIS not being run on the AD Computer
            if (Config.EnableADUser)
            {
                return new DirectoryEntry(Domain, Config.ADUserName, Config.ADUserPassword);
            }
            else
            {
                return new DirectoryEntry(Domain);
            }

            // End 2004-07-28, Leo Duran
        }

        /// <summary>
        /// Gets the name of the netbios.
        /// </summary>
        /// <param name="root">
        /// The root.
        /// </param>
        /// <returns>
        /// A string value...
        /// </returns>
        /// <remarks>
        /// </remarks>
        private static string GetNetbiosName(DirectoryEntry root)
        {
            var path = root.Path.Substring(7);

            // find domain netbios name
            DirectoryEntry entry;

            // 2004-07-28, Leo Duran, Fix for IIS not being run on the AD Computer
            if (Config.EnableADUser)
            {
                entry =
                    new DirectoryEntry(
                        "LDAP://" + path.Insert(path.IndexOf("/") + 1, "CN=Partitions, CN=Configuration,"), 
                        Config.ADUserName, 
                        Config.ADUserPassword);
            }
            else
            {
                entry =
                    new DirectoryEntry(
                        "LDAP://" + path.Insert(path.IndexOf("/") + 1, "CN=Partitions, CN=Configuration,"));
            }

            // End 2004-07-28, Leo Duran
            var myS = new DirectorySearcher(entry);
            myS.Filter = "(&(objectClass=top)(nETBIOSName=*))";
            myS.PropertiesToLoad.Add("nETBIOSName");

            try
            {
                entry = myS.FindOne().GetDirectoryEntry();
                return (string)entry.Properties["nETBIOSName"][0];
            }
            catch (Exception ex)
            {
                throw new Exception("Domain could not be contacted.", ex);
            }
        }

        /// <summary>
        /// Gets the users in group.
        /// </summary>
        /// <param name="group">
        /// The group.
        /// </param>
        /// <param name="eal">
        /// The eal.
        /// </param>
        /// <param name="searchedGroups">
        /// The searched groups.
        /// </param>
        /// <remarks>
        /// </remarks>
        private static void GetUsersInGroup(DirectoryEntry group, EmailAddressList eal, ArrayList searchedGroups)
        {
            // Search all users/groups in directoryentry
            var rootPath = group.Path;
            rootPath = rootPath.Substring(0, rootPath.IndexOf("/", 7) + 1);
            searchedGroups.Add(group.Path);

            for (var i = 0; i < group.Properties["member"].Count; i++)
            {
                var currentEntry = new DirectoryEntry(rootPath + group.Properties["member"][i]);
                if (GetAccountType(currentEntry) == ADAccountType.user)
                {
                    // add to eal
                    var values = currentEntry.Properties["mail"];
                    if (values.Count > 0)
                    {
                        var email = (string)values[0];
                        if (!eal.Contains(email))
                        {
                            try
                            {
                                eal.Add(email);
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                else
                {
                    // see if we already had the group
                    if (!searchedGroups.Contains(currentEntry.Path))
                    {
                        GetUsersInGroup(currentEntry, eal, searchedGroups);
                    }
                }
            }
        }

        #endregion
    }
}