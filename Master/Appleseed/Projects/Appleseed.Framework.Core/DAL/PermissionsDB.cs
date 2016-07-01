namespace Appleseed.Framework.DAL
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;
    using System.Web;
    using Appleseed.Framework.Configuration.Items;
    using Appleseed.Framework.Security;
    using Appleseed.Framework.Settings;

    /// <summary>
    /// Manage permissions to role
    /// </summary>
    public class PermissionsDB
    {
        #region Access Permissions
        /// <summary>
        /// User specific permissions
        /// </summary>
        /// <param name="username">username</param>
        /// <returns>List of assigned Permissions</returns>
        public static List<AccessPermissions> GetUserPermissions(string username)
        {
            List<AccessPermissions> permissions = new List<AccessPermissions>();

            if (HttpContext.Current.Cache["UserPermissions_" + username] != null)
            {
                permissions = (List<AccessPermissions>)HttpContext.Current.Cache["UserPermissions_" + username];
            }
            else
            {
                string query = "select ap.* from aspnet_Permission ap inner join aspnet_RolePermissions rp on rp.PermissionID = ap.PermissionID inner join aspnet_UsersInRoles ur on ur.RoleId = rp.RoleID inner join aspnet_Users au on au.UserId = ur.UserId where au.UserName = '" + username + "'";
                var reader = Appleseed.Framework.Data.DBHelper.GetDataReader(query);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string pid = reader["PermissionId"].ToString();
                        AccessPermissions ap = AccessPermissions.None;
                        if (Enum.TryParse<AccessPermissions>(reader["PermissionId"].ToString(), out ap))
                        {
                            permissions.Add(ap);
                        }
                    }
                }
                reader.Close();
                HttpContext.Current.Cache["UserPermissions_" + username] = permissions;
            }

            return permissions;
        }

        /// <summary>
        /// Clear permiossion cache
        /// </summary>
        public static void ClearPermissionsCache()
        {
            string query = "select * from aspnet_Users";
            var reader = Appleseed.Framework.Data.DBHelper.GetDataReader(query);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    HttpContext.Current.Cache.Remove("UserPermissions_" + reader["UserName"].ToString());
                }
            }
            reader.Close();
        }

        #endregion

        /// <summary>
        /// Get Permissions List
        /// </summary>
        /// <returns>permission List</returns>
        public List<PermissionInfo> Permissions()
        {
            return Appleseed.Framework.Data.DBHelper.DataReaderToObjectList<PermissionInfo>("SELECT * from aspnet_Permission");
        }

        /// <summary>
        /// Get assigned permissions
        /// </summary>
        /// <returns>Assigned permissions</returns>
        public List<AssignedPermissionItem> AssignedPermissions()
        {
            return Appleseed.Framework.Data.DBHelper.DataReaderToObjectList<AssignedPermissionItem>("SELECT * from aspnet_RolePermissions");
        }

        /// <summary>
        /// Assign new permissions
        /// </summary>
        /// <param name="permissionIdRoleId">comma separated permissionID and RoleID</param>
        /// /// <param name="RoleID">RoleID</param>
        /// <returns>Processed success or not</returns>
        public bool AssignPermissions(string permissionIdRoleId, Guid RoleID)
        {
            try
            {
                this.DeleteAllPermissions(RoleID);
                if (!string.IsNullOrEmpty(permissionIdRoleId))
                {
                    string[] stringPermissionAndRole = permissionIdRoleId.Split(',');

                    foreach (var item in stringPermissionAndRole)
                    {
                        string[] permissionGuidRoleGuid = item.Split('#');
                        AssignedPermissionItem assignedPermission = new AssignedPermissionItem();
                        assignedPermission.RoleID = new Guid(permissionGuidRoleGuid[1]);
                        assignedPermission.PermissionID = Convert.ToInt32(permissionGuidRoleGuid[0]);
                        this.AddNewPermission(assignedPermission);
                    }
                }

                PermissionsDB.ClearPermissionsCache();
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Assign new permission
        /// </summary>
        /// <param name="permissions">object of AssignedPermissionItem </param>
        private void AddNewPermission(AssignedPermissionItem permissions)
        {
            using (var connection = Config.SqlConnectionString)
            using (var sqlCommand = new SqlCommand("INSERT INTO aspnet_RolePermissions (RoleID, PermissionID) VALUES (@RoleID, @PermissionID)", connection))
            {
                // Mark the Command as a SPROC
                sqlCommand.CommandType = CommandType.Text;
                var parameterSliderName = new SqlParameter("@RoleID", SqlDbType.UniqueIdentifier) { Value = permissions.RoleID };
                sqlCommand.Parameters.Add(parameterSliderName);

                sqlCommand.CommandType = CommandType.Text;
                var parameterCreatedDate = new SqlParameter("@PermissionID", SqlDbType.Int) { Value = permissions.PermissionID };
                sqlCommand.Parameters.Add(parameterCreatedDate);

                connection.Open();
                try
                {
                    sqlCommand.ExecuteNonQuery();
                }
                finally
                {
                }
            }
        }

        /// <summary>
        /// Delete all assigned permissions
        /// </summary>
        private void DeleteAllPermissions(Guid RoleID)
        {

            using (var connection = Config.SqlConnectionString)
            using (var sqlCommand = new SqlCommand("DELETE FROM aspnet_RolePermissions", connection))
            {
                connection.Open();
                try
                {
                    if (RoleID != Guid.Empty)
                    {
                        sqlCommand.CommandText = "DELETE FROM aspnet_RolePermissions WHERE RoleID = '" + RoleID.ToString() + "'";
                    }
                    sqlCommand.ExecuteNonQuery();
                }
                finally
                {
                }
            }
        }
    }
}
