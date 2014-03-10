using System;
using System.Collections.Generic;
using System.Text;
using Content.API;

namespace Appleseed.Framework.Content
{
    [Serializable]
    public class AppleseedItem : Item
    {
        private long _portalID = 0;
        private bool _shareWithAllPortals = false;
        private List<string> _allowedViewRoles = null;
        private List<string> _allowedDeleteRoles = null;
        private List<string> _allowedRecycleRoles = null;
        private List<string> _allowedUpdateRoles = null;
        private int _order = 0;


        /// <summary>
        /// Gets a value indicating whether [share with all porals].
        /// </summary>
        /// <value><c>true</c> if [share with all porals]; otherwise, <c>false</c>.</value>
        public bool ShareWithAllPorals
        {
            get { return _shareWithAllPortals; }
        }

        /// <summary>
        /// Gets the portal ID.
        /// </summary>
        /// <value>The portal ID.</value>
        public long PortalID
        {
            get { return _portalID; }
        }

        public long ItemID
        {
            get
            {
                return base.ItemID;
            }
            set
            {
                if (base.ItemID > 0)
                    base.ItemID = value;
                else
                    throw new Exception("Not allowed to set ItemID to 0 or negative.");


            }

        }

        /// <summary>
        /// Gets the order.
        /// </summary>
        /// <value>The order.</value>
        public int Order
        {
            get { return _order; }
        }

        /// <summary>
        /// Gets the allowed view roles.
        /// </summary>
        /// <value>The allowed view roles.</value>
        public List<string> AllowedViewRoles
        {
            get { return _allowedViewRoles; }
        }
        /// <summary>
        /// Gets the allowed delete roles.
        /// </summary>
        /// <value>The allowed delete roles.</value>
        public List<string> AllowedDeleteRoles
        {
            get { return _allowedDeleteRoles; }
        }
        /// <summary>
        /// Gets the allowed recycle roles.
        /// </summary>
        /// <value>The allowed recycle roles.</value>
        public List<string> AllowedRecycleRoles
        {
            get { return _allowedRecycleRoles; }
        }
        /// <summary>
        /// Gets the allowed update roles.
        /// </summary>
        /// <value>The allowed update roles.</value>
        public List<string> AllowedUpdateRoles
        {
            get { return _allowedUpdateRoles; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AppleseedItem"/> class.
        /// </summary>
        public AppleseedItem() : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AppleseedItem"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        public AppleseedItem(long id) : base(id)
        {
            
        }

        /// <summary>
        /// Recycles the item.
        /// </summary>
        /// <returns></returns>
        public override bool RecycleItem()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Saves the state.
        /// </summary>
        /// <returns></returns>
        public override bool SaveState()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        /// <returns></returns>
        public override bool Reset()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Adds the new item.
        /// </summary>
        /// <returns></returns>
        public override long AddNewItem()
        {
            return 0;
        }

        /// <summary>
        /// Removes the item.
        /// </summary>
        /// <returns></returns>
        public override bool RemoveItem()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Updates the item.
        /// </summary>
        /// <returns></returns>
        public override bool UpdateItem()
        {
            throw new System.NotImplementedException();
        }

    }
}
