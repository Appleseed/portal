// --------------------------------------------------------------------------------------------------------------------
// <summary>
//   The appleseed url rewriting provider.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.UrlRewriting
{
    using System.Web.SessionState;

    using UrlRewritingNet.Configuration.Provider;
    using UrlRewritingNet.Web;

    /// <summary>
    /// The appleseed url rewriting provider.
    /// </summary>
    public class AppleseedUrlRewritingProvider : UrlRewritingProvider, IRequiresSessionState
    {
        #region Public Methods

        /// <summary>
        /// Creates the rewrite rule.
        /// </summary>
        /// <returns>The rewrite rule.</returns>
        public override RewriteRule CreateRewriteRule()
        {
            return new AppleseedUrlRewritingRule();
        }

        #endregion
    }
}