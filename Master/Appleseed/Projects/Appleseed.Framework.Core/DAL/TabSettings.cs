using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Xml;

using Appleseed.Framework.Data;
using Appleseed.Framework.Settings;
using Appleseed.Framework.Site.Configuration;

namespace Appleseed.Framework.Site.Data
{
    /// <summary>
    /// tab settings
    /// </summary>
    public class TabSettings
    {
       /// <summary>
       /// Get css from db
       /// </summary>
       /// <param name="TabID">tabid</param>
       /// <param name="CS">css key</param>
       /// <returns>css from db</returns>
        public static SqlDataReader CSSDataReader(int TabID,String CS)
        {
            
            var sql =
                string.Format(
                    "Select SettingValue From rb_TabSettings Where TabID = {0} and SettingName = '{1}'",
                    TabID, CS);

            var reader = Appleseed.Framework.Data.DBHelper.GetDataReader(sql);

            return reader;

       }

        /// <summary>
        /// get perticulare value from tabsetttings
        /// </summary>
        /// <param name="TabID">tab id</param>
        /// <param name="JS">js</param>
        /// <returns>settings value</returns>
        public static SqlDataReader JSDataReader(int TabID, String JS)
        {

            var sql =
                string.Format(
                    "Select SettingValue From rb_TabSettings Where TabID = {0} and SettingName = '{1}'",
                    TabID, JS);

            var reader = Appleseed.Framework.Data.DBHelper.GetDataReader(sql);

            return reader;
        }
    }
}
