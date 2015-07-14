

namespace Appleseed.Framework.Configuration.Items
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Class is for JsTree render
    /// </summary>
   public class JsTreeItem
    {
       /// <summary>
       /// ID
       /// </summary>
       public string id;

       /// <summary>
       ///Text
       /// </summary>
       public string text;

       /// <summary>
       /// Node Data
       /// </summary>
       public string data;

       /// <summary>
       /// Node attribute 
       /// </summary>
       public JsTreeAttribute attr;

       /// <summary>
       /// Node childern
       /// </summary>
       public bool children = true;

       /// <summary>
       /// Mode - open/close
       /// </summary>
       public state state = new state() { opened = false };

       /// <summary>
       /// Position
       /// </summary>
       public string position;

       /// <summary>
       /// Last pos
       /// </summary>
       public string lastpos;

       /// <summary>
       /// Check is pane
       /// </summary>
       public bool isPane = false;

       /// <summary>
       /// Node Type
       /// </summary>
       public string nodeType;

      /// <summary>
      /// Node icon
      /// </summary>
       public string icon;

       /// <summary>
       /// Node type
       /// </summary>
       public string type;

       /// <summary>
       /// Node visible or not
       /// </summary>
       public bool IsVisible
       {
           get;
           set;
       }
   }

    /// <summary>
    /// Node status
    /// </summary>
   public class state
   {

       /// <summary>
       /// Open state
       /// </summary>
       public bool opened { get; set; }

       /// <summary>
       /// Dispable state
       /// </summary>
       public bool disabled { get; set; }

       /// <summary>
       /// Selected State
       /// </summary>
       public bool selected { get; set; }
   }

/// <summary>
///jstree atribiute clss
/// </summary>
   public class JsTreeAttribute
   {
       /// <summary>
       /// Attribute id
       /// </summary>
       public string id;

       /// <summary>
       ///  Attribute selected
       /// </summary>
       public bool selected;

       /// <summary>
       /// Attrubite rel
       /// </summary>
       public string rel;
   }

}
