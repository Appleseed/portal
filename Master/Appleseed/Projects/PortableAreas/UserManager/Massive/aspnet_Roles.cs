using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserManager.Massive
{
    public class aspnet_Roles : DynamicModel
    {
        public aspnet_Roles()
            : base("ConnectionString", "aspnet_Roles")
        {
            PrimaryKeyField = "RoleId";
        }
    }
}