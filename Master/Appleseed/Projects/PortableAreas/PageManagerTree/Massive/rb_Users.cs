using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PageManagerTree.Massive
{
    public class rb_Users : DynamicModel {
        public rb_Users()
            : base("ConnectionString", "rb_Users") {
                PrimaryKeyField = "UserID";
        }
    }
}
