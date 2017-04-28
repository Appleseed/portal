using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Appleseed.Framework.Providers.ConnectedSourcesProvider
{
    public class EngineItem
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public Guid engineid { get; set; }
        public string locationurl { get; set; }
        public string type { get; set; }
        public string collectionname { get; set; }
        public string indexpath { get; set; }
    }
}