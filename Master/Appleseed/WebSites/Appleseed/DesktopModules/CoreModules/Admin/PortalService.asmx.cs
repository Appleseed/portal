// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PortalService.asmx.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The portal service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.DesktopModules.CoreModules.Admin
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Web.Script.Services;
    using System.Web.Services;

    using Appleseed.Framework.Core.Model;

    /// <summary>
    /// The portal service.
    /// </summary>
    /// <remarks>
    /// </remarks>
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class PortalService : WebService
    {
        #region Public Methods

        /// <summary>
        /// Reorders the specified data.
        /// </summary>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <returns>
        /// The reorder.
        /// </returns>
        /// <remarks>
        /// </remarks>
        [WebMethod]
        public string Reorder(string data)
        {
            var result = "OK";
            try
            {
                char[] separator = { ';' };
                char[] separator2 = { ',' };

                var modules = data.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                var newModulesOrder = new Dictionary<string, ArrayList>();
                foreach (var s in modules)
                {
                    var arrobaIdx = s.IndexOf('@');
                    var paneName = s.Substring(0, arrobaIdx);
                    var modulesId = s.Substring(arrobaIdx + 1).Split(separator2, StringSplitOptions.RemoveEmptyEntries);

                    var moduleArray = new ArrayList();
                    foreach (var modId in modulesId)
                    {
                        moduleArray.Add(Convert.ToInt32(modId.ToLower().Replace("mid", string.Empty)));
                    }

                    newModulesOrder.Add(paneName, moduleArray);
                }

                ModelServices.Reorder(newModulesOrder);
            }
            catch (Exception exc)
            {
                result = "ERROR:" + exc.Message;
            }

            return result;
        }

        #endregion
    }
}