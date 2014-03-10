using System;
using System.Collections.Generic;
using System.Text;

using Content.API;
using Appleseed.Framework.Content;
using Appleseed.Framework.Content.Items;

namespace Appleseed.Framework.Content.ItemTypes
{
    public class HtmlText : ModuleItem
    {
        private Guid _typeGUID = new Guid("{7733DEC0-483D-4ed9-9386-D422574C868E}");
        private int _editor = 0;
        private WorkFlowVersions _workFlow = WorkFlowVersions.Post;
        private long _moduleID = 0;
        private ModuleItem _containerModule = null;

        /// <summary>
        /// Gets or sets the work flow.
        /// Default is Post
        /// </summary>
        /// <value>The work flow.</value>
        public WorkFlowVersions WorkFlow
        {
            get { return _workFlow; }
            set { 
                _workFlow = value; 
            }
        }

        /// <summary>
        /// Gets the module ID.
        /// </summary>
        /// <value>The module ID.</value>
        public long ModuleID
        {
            get { return _moduleID; }
        }

        /// <summary>
        /// Gets the module.
        /// </summary>
        /// <value>The module.</value>
        public ModuleItem Module
        {
            get
            {
                if (_containerModule == null && _moduleID > 0)
                    _containerModule = new ModuleItem(_moduleID);

                return _containerModule;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ModuleType"/> class.
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        public HtmlText(int moduleID) 
            : base(moduleID)
        {
            base.ItemType = _typeGUID;

            // if(
        }
    }
}
