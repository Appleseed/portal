using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appleseed.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public class ModuleInstance
    {
        /// <summary>
        /// 
        /// </summary>
        public ModuleItem ModuleItem { get; set; }

        /// <summary>
        /// Module ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Page name
        /// </summary>
        public string PageName { get; set; }

        /// <summary>
        /// Module title
        /// </summary>
        public string ModuleTitle { get; set; }

        /// <summary>
        /// Date
        /// </summary>
        public string Date { get; set; }
    }
}
