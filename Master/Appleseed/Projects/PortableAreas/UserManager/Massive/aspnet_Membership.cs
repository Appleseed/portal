using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserManager.Massive
{
    public class aspnet_Membership: DynamicModel
    {
        public aspnet_Membership()
            : base("ConnectionString", "aspnet_Membership")
        {
            PrimaryKeyField = "ApplicationId";
        }
    }
}