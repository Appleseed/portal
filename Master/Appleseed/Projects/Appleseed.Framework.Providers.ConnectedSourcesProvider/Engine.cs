using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Appleseed.Framework.Providers.ConnectedSourcesProvider
{
    public class Engine
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public Guid typeid { get; set; }
    }
}