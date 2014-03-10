using System;
using System.Collections.Generic;
using System.Text;

namespace Content.API
{
    public class Statuses : Dictionary<int, Status>
    {
        /// <summary>
        /// Applications the statuses.
        /// </summary>
        /// <param name="applicationID">The application ID.</param>
        /// <returns></returns>
        public static Dictionary<int, Status> ApplicationStatuses(int applicationID)
        {
            Dictionary<int, Status> theList = null;
            return theList;
        }

        /// <summary>
        /// Items the statuses.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <returns></returns>
        public static Dictionary<int, Status> ItemStatuses(long itemID)
        {
            Dictionary<int, Status> theList = null;
            return theList;
        }

        /// <summary>
        /// Categories the statuses.
        /// </summary>
        /// <param name="categoryID">The category ID.</param>
        /// <returns></returns>
        public static Dictionary<int, Status> CategoryStatuses(int categoryID)
        {
            Dictionary<int, Status> theList = null;
            return theList;
        }
    }
}
