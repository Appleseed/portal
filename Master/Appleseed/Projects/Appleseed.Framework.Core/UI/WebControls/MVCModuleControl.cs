// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MVCModuleControl.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The MVC module control.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Web.UI.WebControls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Web.Routing;

    /// <summary>
    /// The MVC module control.
    /// </summary>
    public class MVCModuleControl : PortalModuleControl
    {
        #region Properties

        /// <summary>
        ///   Gets or sets ActionName.
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        ///   Gets or sets AreaName.
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        ///   Gets or sets ControllerName.
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        ///   Gets or sets ModID.
        /// </summary>
        public int ModID { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The initialize.
        /// </summary>
        public void Initialize()
        {
            if (String.IsNullOrEmpty(this.ActionName))
            {
                return;
            }

            var routes = RouteTable.Routes;

            foreach (var t in routes)
            {
                if (t.GetType().Name != "Route" || ((Route)t).DataTokens == null)
                {
                    continue;
                }

                var strArray = (string[])((Route)t).DataTokens["namespaces"];
                if (strArray == null || !strArray[0].Contains(this.AreaName))
                {
                    continue;
                }

                var hashtable2 = GetMVCModuleSettingsDefinitions(
                    strArray[0], this.AreaName, this.ControllerName, this.ActionName);

                foreach (var key in hashtable2.Keys.Where(key => !this.BaseSettings.ContainsKey(key)))
                {
                    this.BaseSettings.Add(key, hashtable2[key]);
                }
            }

            this.ModuleID = this.ModID;

            // var s = this.Settings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get mvc module settings definitions.
        /// </summary>
        /// <param name="controllerNamespace">
        /// The controller namespace.
        /// </param>
        /// <param name="areaName">
        /// The area name.
        /// </param>
        /// <param name="controllerName">
        /// The controller name.
        /// </param>
        /// <param name="actionName">
        /// The action name.
        /// </param>
        /// <returns>
        /// A hash table of the settings.
        /// </returns>
        private static Dictionary<string, ISettingItem> GetMVCModuleSettingsDefinitions(
            string controllerNamespace, string areaName, string controllerName, string actionName)
        {
            var hashtable = new Dictionary<string, ISettingItem>();
            var index = controllerNamespace.ToLower().IndexOf(".mvc");
            if (index < 0)
            {
                index = controllerNamespace.ToLower().IndexOf(".areas");
            }

            if (index < 0)
            {
                // Caso para las portableareas
                index = controllerNamespace.ToLower().IndexOf(".controllers");
            }

            if (index >= 0)
            {
                var str = controllerNamespace.Substring(0, index);
                try
                {
                    var target =
                        Assembly.LoadFile(string.Format("{0}/bin/{1}.dll", AppDomain.CurrentDomain.BaseDirectory, str)).
                            CreateInstance(string.Format("{0}.{1}Controller", controllerNamespace, controllerName));

                    if (target != null)
                    {
                        var hashtable2 =
                            (Dictionary<string, ISettingItem>)
                            target.GetType().InvokeMember(
                                actionName + "_SettingsDefinitions", BindingFlags.InvokeMethod, null, target, null, null);
                        hashtable = hashtable2;
                    }
                }
                catch (Exception exc)
                {
                    ErrorHandler.Publish(
                        LogLevel.Debug, 
                        String.Format(
                            "Error al obtener los settings para {0}/{1}/{2} con namespace: {3}", 
                            areaName, 
                            controllerName, 
                            actionName, 
                            controllerNamespace), 
                        exc);
                }
            }

            return hashtable;
        }

        #endregion
    }
}