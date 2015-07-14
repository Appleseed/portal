using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace PageManagerTree.Models
{
    public class JsTreeModel
    {
        public string id;
        public string text;

        public string data;
        public JsTreeAttribute attr;
        //public JsTreeModel[] children2;
        public bool children = true;
        public state state = new state() { opened = false };
        public string position;
        public string lastpos;
        public bool isPane = false;
        public string nodeType;
        public string icon;
        public string type;

        public bool IsVisible
        {
            get;
            set;
        }
    }
    public class state
    {
        public bool opened { get; set; }
        public bool disabled { get; set; }
        public bool selected { get; set; }
    }
    public class JsTreeAttribute
    {
        public string id;
        public bool selected;
        public string rel;
    }

    public class Permissions
    {
        public bool HasPageList { get; set; }
        public bool HasPageCreatePermission { get; set; }
        public bool HasPageUpdatePermission { get; set; }
        public bool HasPageDeletePermission { get; set; }
    }
}