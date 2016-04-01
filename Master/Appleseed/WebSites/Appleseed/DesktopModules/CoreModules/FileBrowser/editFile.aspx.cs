using Appleseed.Framework.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Appleseed.DesktopModules.CoreModules.FileBrowser
{
    public partial class EditFile : System.Web.UI.Page
    {
        private string filePath
        {
            get { return Server.MapPath(Request.QueryString["filepath"]); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!UserProfile.HasEditThisPageAccess() && !UserProfile.HasAdminPageAccess())
            {
                PortalSecurity.AccessDenied();
                return;
            }

            if (!IsPostBack)
            {
                loadData();
            }
        }

        private void loadData()
        {
            txtData.Text = GetFileContent(filePath);
            hdnFileExtention.Value = Path.GetExtension(filePath).Replace(".", string.Empty).ToLower();
        }

        public string GetFileContent(string filepath)
        {
            try
            {
                return System.IO.File.ReadAllText(filepath);
            }
            catch
            {
                return string.Empty;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            System.IO.File.WriteAllText(filePath, txtData.Text);
            hdnSaved.Value = "1";
        }
    }
}