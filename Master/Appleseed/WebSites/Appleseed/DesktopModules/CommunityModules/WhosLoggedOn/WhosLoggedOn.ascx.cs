namespace Appleseed.Content.Web.Modules
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Web.UI.WebControls;

    using Appleseed.Framework;
    using Appleseed.Framework.Data;
    using Appleseed.Framework.DataTypes;
    using Appleseed.Framework.Monitoring;
    using Appleseed.Framework.Web.UI.WebControls;

    using Label = Appleseed.Framework.Web.UI.WebControls.Label;

    /// <summary>
    /// Who's Logged On Module - Uses the monitoring database table to work
    /// out and display who is currently logged on, anonymous or otherwise
    /// Written by Paul Yarrow, paul@paulyarrow.com
    /// </summary>
    public partial class WhosLoggedOn : PortalModuleControl
    {
        #region Declarations

        /// <summary>
        /// 
        /// </summary>
        protected Label Label2;

        /// <summary>
        /// 
        /// </summary>
        protected Label Label1;

        /// <summary>
        /// 
        /// </summary>
        protected Label Label5;

        /// <summary>
        /// 
        /// </summary>
        private int minutesToCheckForUsers = 30;

        #endregion

        /// <summary>
        /// Initial Revision by Paul Yarrow, paul@paulyarrow.com, 2003-07-29
        /// </summary>
        public WhosLoggedOn()
        {
            var cacheTime = new SettingItem<int, TextBox>();
            cacheTime.Required = true;
            cacheTime.Order = 0;
            cacheTime.Value = 1;
            cacheTime.MinValue = 0;
            cacheTime.MaxValue = 60000;
            cacheTime.Description =
                General.GetString("WHOSLOGGEDONCACHETIMEOUT",
                                  "Specify an amount of time the who's logged on module will wait before checking again (0 - 60000)",
                                  this);
            this.BaseSettings.Add("CacheTimeout", cacheTime);
        }

        /// <summary>
        /// Initial Revision by Paul Yarrow, paul@paulyarrow.com, 2003-07-13
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void AppleseedVersion_Load(object sender, EventArgs e)
        {
            int cacheTime = ((SettingItem<int, TextBox>)Settings["CacheTimeout"]).Value;

            int anonUserCount, regUsersOnlineCount;
            string regUsersString;
            Utility.FillUsersOnlineCache(
                this.PortalSettings.PortalID,
                this.minutesToCheckForUsers,
                cacheTime,
                                         out anonUserCount,
                                         out regUsersOnlineCount,
                                         out regUsersString);

            //LabelAnonUsersCount.Text = Convert.ToString(anonUserCount);
            LabelRegUsersOnlineCount.Text = Convert.ToString(regUsersOnlineCount);
            LabelRegUserNames.Text = regUsersString;
        }

        /// <summary>
        /// Initial Revision by Paul Yarrow, paul@paulyarrow.com, 2003-07-13
        /// </summary>
        /// <value></value>
        public override Guid GuidID
        {
            get { return new Guid("{52AD3A51-121D-48bc-9782-02076E0D6A69}"); }
        }

        # region Install / Uninstall Implementation

        /// <summary>
        /// Unknown
        /// </summary>
        /// <param name="stateSaver"></param>
        public override void Install(IDictionary stateSaver)
        {
            string currentScriptName = Path.Combine(Server.MapPath(TemplateSourceDirectory), "install.sql");


            List<string> errors = DBHelper.ExecuteScript(currentScriptName, true);
            if (errors.Count > 0)
            {
                // Call rollback
                throw new Exception("Error occurred:" + errors[0].ToString());
            }
        }

        /// <summary>
        /// Unknown
        /// </summary>
        /// <param name="stateSaver"></param>
        public override void Uninstall(IDictionary stateSaver)
        {
            string currentScriptName = Path.Combine(Server.MapPath(TemplateSourceDirectory), "uninstall.sql");
            List<string> errors = DBHelper.ExecuteScript(currentScriptName, true);
            if (errors.Count > 0)
            {
                // Call rollback
                throw new Exception("Error occurred:" + errors[0].ToString());
            }
        }

        #endregion

        #region Web Form Designer generated code

        /// <summary>
        /// Raises Init event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(this.AppleseedVersion_Load);
            base.OnInit(e);
        }

        #endregion
    }
}