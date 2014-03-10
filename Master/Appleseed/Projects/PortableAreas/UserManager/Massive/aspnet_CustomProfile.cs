using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserManager.Massive
{
    public class aspnet_CustomProfile : DynamicModel
    {
        public aspnet_CustomProfile()
            : base("ConnectionString", "aspnet_CustomProfile")
        {
            PrimaryKeyField = "UserId";
        }
    }
}