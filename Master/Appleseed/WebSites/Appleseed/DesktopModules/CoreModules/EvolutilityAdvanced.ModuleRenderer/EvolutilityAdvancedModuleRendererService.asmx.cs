namespace Appleseed.DesktopModules.CoreModules.EvolutilityAdvanced.ModuleRenderer
{
    using Appleseed.Framework.Site.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Script.Serialization;
    using System.Web.Script.Services;
    using System.Web.Services;

    /// <summary>
    /// Summary description for EvolutilityAdvancedModuleRendererService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class EvolutilityAdvancedModuleRendererService : System.Web.Services.WebService
    {
        /// <summary>
        /// Use to save and edit data
        /// </summary>
        /// <param name="moduleid">Pass module id</param>
        /// <param name="data">Pass data</param>
        /// <returns></returns>
        [WebMethod]
        public string UpdateData(string moduleid, string data)
        {
            data = data.Replace("&quot;", "\"").Replace("&sqbleft;", "[").Replace("&sqbright;", "]").Replace("&crbleft;", "{").Replace("&crbright;", "}");
            EvolutilityModuleDB dbObject = new EvolutilityModuleDB();
            dbObject.EvolitityAdvedUpdateModelData(Convert.ToInt32(moduleid), data);
            return "Updated";
        }

        /// <summary>
        /// get data from db and return
        /// </summary>
        /// <param name="moduleid">Pass module id to get data </param>
        /// <returns>If null then return empty square bracket.</returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SelectAllData(string moduleid)
        {
            string data = "[]";
            EvolutilityModuleDB dbObject = new EvolutilityModuleDB();
            var sqlreader = dbObject.EvolitityAdvedGetModelData(Convert.ToInt32(moduleid));
            if (sqlreader.Read())
            {
                data = sqlreader["ModelData"].ToString();
            }
            //Added by Ashish - Connection Pool Issue
            if(sqlreader != null)
            {
                sqlreader.Close();
            }
            return data;
        }

    }
}

