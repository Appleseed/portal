using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Appleseed.Framework.UrlRewriting
{
    using System.Web.SessionState;

    using UrlRewritingNet.Configuration.Provider;
    using UrlRewritingNet.Web;

    class AppleseedSeoUrlRewritingProvider : UrlRewritingProvider, IRequiresSessionState
    {

        public override RewriteRule CreateRewriteRule()
        {
            return new AppleseedSeoUrlRewritingRule();
        }

    }
}
