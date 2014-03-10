using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PageManagerTree.Massive
{
    public class rb_ModuleSettings: DynamicModel {
        public rb_ModuleSettings()
            : base("ConnectionString", "rb_ModuleSettings")
        {
            PrimaryKeyField = "ModuleDefID";       
        }
    }
}