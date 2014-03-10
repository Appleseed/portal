using System;
using System.Collections.Generic;
using System.Text;

namespace Appleseed.Framework.Content
{
    class AppleseedItemEventArgs : EventArgs
    {
        /// <summary>
        /// ThreadHandler, which has generated the event.
        /// </summary>
        private AppleseedItem _item;

        /// <summary>
        /// The ThreadHandler from where this event was generated.
        /// </summary>
        public AppleseedItem Handler
        {
            get
            {
                return _item;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="threadHandler">ThreadHandler object</param>
        public AppleseedItemEventArgs(AppleseedItem AppleseedItem)
        {
            _item = AppleseedItem;
        }
    }
}
