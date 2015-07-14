namespace Appleseed.Framework.Configuration.Items
{
    using System;

    /// <summary>
    /// Assiged permission to Item
    /// </summary>
    public class AssignedPermissionItem
    {
        /// <summary>
        /// Gets or sets RoleID
        /// </summary>
        public Guid RoleID { get; set; }

        /// <summary>
        /// Gets or sets PermissionID
        /// </summary>
        public int PermissionID { get; set; }
    }
}
