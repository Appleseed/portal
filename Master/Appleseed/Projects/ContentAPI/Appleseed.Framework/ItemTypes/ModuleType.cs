using System;
using System.Collections.Generic;
using System.Text;

using Content.API;
using Content.API.Types;
namespace Appleseed.Framework.Content.ItemTypes
{
    class ModuleType : ItemType
    {
        private Guid _typeGUID = new Guid("{4A007274-187D-4b39-87B8-3F726B60D93D}");

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ModuleType"/> class.
        /// </summary>
        /// <param name="typeID"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="xmlSettings"></param>
        /// <param name="baseType"></param>
        public ModuleType(int typeID, string title, string description, string xmlSettings, int baseType) 
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
