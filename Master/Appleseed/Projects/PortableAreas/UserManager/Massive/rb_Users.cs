using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserManager.Massive
{
    public class rb_Users : DynamicModel
    {
        public rb_Users()
            : base("ConnectionString", "rb_Users")
        {
            PrimaryKeyField = "UserID";
        }
    }
}
