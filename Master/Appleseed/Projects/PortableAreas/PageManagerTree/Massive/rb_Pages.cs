using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PageManagerTree.Massive
{
    public class rb_Pages : DynamicModel {
        public rb_Pages()
            : base("ConnectionString", "rb_Pages") {
                PrimaryKeyField = "PageID";
        }
    }
}
