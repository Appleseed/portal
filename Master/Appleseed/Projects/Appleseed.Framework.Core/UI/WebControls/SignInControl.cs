using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Appleseed.Framework.Web.UI.WebControls;

namespace Appleseed.Framework.UI.WebControls
{
    public abstract class SignInControl : PortalModuleControl
    {
        public virtual void Logoff() { }
    }
}
