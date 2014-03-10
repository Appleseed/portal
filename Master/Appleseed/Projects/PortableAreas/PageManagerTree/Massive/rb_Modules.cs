using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PageManagerTree.Massive
{
    public class rb_Modules: DynamicModel {
        public rb_Modules()
            : base("ConnectionString", "rb_Modules")
        {
                PrimaryKeyField = "ModuleID";
        }
    }
}