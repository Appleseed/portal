using Appleseed.Framework.Configuration.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appleseed.Framework.DAL
{
    /// <summary>
    /// proxy DB class to perform database operations
    /// </summary>
    public class ProxyDB
    {
        /// <summary>
        /// get all proxy settings
        /// </summary>
        /// <returns></returns>
        public List<ProxyItem> GetAll()
        {
            return Appleseed.Framework.Data.DBHelper.DataReaderToObjectList<ProxyItem>("SELECT * from rb_Proxy");
        }

        /// <summary>
        /// get proxy setting based on passed service id
        /// </summary>
        /// <param name="id">service id</param>
        /// <returns>ProxyItem object with all details</returns>
        public ProxyItem GetById(int id)
        {
            var proxyList = Appleseed.Framework.Data.DBHelper.DataReaderToObjectList<ProxyItem>("SELECT * from rb_Proxy where ServiceId = " + id);
            if (proxyList != null && proxyList.Count > 0)
                return proxyList[0];
            return null;
        }

        /// <summary>
        /// perform insert/update proxy settings into database
        /// </summary>
        /// <param name="proxyItem"></param>
        public void InsertProxy(ProxyItem proxyItem)
        {
            if (proxyItem.ServiceId > 0)
            {
                Appleseed.Framework.Data.DBHelper.ExeSQL("Update rb_Proxy set ServiceTitle ='" + proxyItem.ServiceTitle + "',ServiceUrl='" + proxyItem.ServiceUrl + "',ForwardHeaders='" + proxyItem.ForwardHeaders + "',EnabledContentAccess='"+proxyItem.EnabledContentAccess + "',ContentAccessRoles='" + proxyItem.ContentAccessRoles + "' where ServiceId=" + proxyItem.ServiceId + "");
            }
            else
            {
                Appleseed.Framework.Data.DBHelper.ExeSQL("Insert into rb_Proxy values ('" + proxyItem.ServiceTitle + "','" + proxyItem.ServiceUrl + "','" + proxyItem.ForwardHeaders + "','" + proxyItem.EnabledContentAccess + "','" + proxyItem.ContentAccessRoles + "')");
            }
        }

        /// <summary>
        /// Delete proxy settings from database
        /// </summary>
        /// <param name="id"></param>
        public void DeleteProxy(int id)
        {
            Appleseed.Framework.Data.DBHelper.ExeSQL("Delete from rb_Proxy where ServiceId = " + id);
        }
    }
}
