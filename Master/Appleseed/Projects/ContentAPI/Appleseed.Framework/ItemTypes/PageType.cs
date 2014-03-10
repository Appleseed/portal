using System;
using System.Collections.Generic;
using System.Text;

using Content.API;
using Content.API.Types;
namespace Appleseed.Framework.Content.ItemTypes
{
    class PageType : ItemType
    {
        private Guid _typeGUID = new Guid("{6A9BA176-1487-4bc7-AC89-C009DA32D454}");

        /// <summary>
        /// Initializes a new instance of the <see cref="T:PageType"/> class.
        /// </summary>
        /// <param name="typeID"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="xmlSettings"></param>
        /// <param name="baseType"></param>
        public PageType(int typeID, string title, string description, string xmlSettings, int baseType)
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
