using System;
using System.Collections.Generic;
using System.Text;

using Content.API;
using Content.API.Types;

namespace Appleseed.Framework.Content.ItemTypes
{
    class PortalType : ItemType
    {
        private Guid _typeGUID = new Guid("{A8EA828B-ACEA-4b54-AE63-9924EF1F71B3}");

        /// <summary>
        /// Initializes a new instance of the <see cref="T:PortalType"/> class.
        /// </summary>
        /// <param name="typeID"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="xmlSettings"></param>
        /// <param name="baseType"></param>
        public PortalType(int typeID, string title, string description, string xmlSettings, int baseType)
            : base(typeID, title, description, xmlSettings, baseType)
        {
            base.TypeGUID = _typeGUID;
        }

        /// <summary>
        /// Fetches the type settings.
        /// </summary>
        /// <param name="xmlSettings">The XML settings.</param>
        protected override void FetchTypeSettings(string xmlSettings)
        {
            base.FetchTypeSettings(xmlSettings);
        }
    }
}
