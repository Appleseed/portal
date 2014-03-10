using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserManager.Massive
{
    public class aspnet_UsersInRoles : DynamicModel
    {
        public aspnet_UsersInRoles()
            : base("ConnectionString", "aspnet_UsersInRoles")
        {
            PrimaryKeyField = "UserId";
        }
    }
}