using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Appleseed.Framework.Web.UI.WebControls;

namespace Appleseed.Framework.UI.WebControls
{
    /// <summary>
    /// Sign in control
    /// </summary>
    public abstract class SignInControl : PortalModuleControl
    {
        /// <summary>
        /// Logo off
        /// </summary>
        public virtual void Logoff() { }
    }
}
