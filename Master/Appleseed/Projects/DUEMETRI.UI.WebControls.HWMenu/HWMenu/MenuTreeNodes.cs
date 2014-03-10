using System;
using System.Text;
using System.Collections;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DUEMETRI.UI.WebControls.HWMenu
{
    /// <summary>
    /// MenuTreeNode collection.
    /// </summary>
    public class MenuTreeNodes : CollectionBase
    {
        /// <summary>
        /// Adds a MenuTreeNode item to the collection
        /// </summary>
        /// <param name="mn"></param>
        /// <returns></returns>
        public int Add(MenuTreeNode mn)
        {
            return InnerList.Add(mn);
        }

        /// <summary>
        /// Adds an object to the collection (must be of type MenuTreeNode)
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public int Add(object o)
        {
            MenuTreeNode mn = (MenuTreeNode)o;
            return InnerList.Add(mn);
        }

        /// <summary>
        /// Indexer
        /// </summary>
        public MenuTreeNode this[int index]
        {
            get
            {
                return ((MenuTreeNode)InnerList[index]);
            }
            set
            {
                InnerList[index] = (MenuTreeNode)value;
            }
        }

        /// <summary>
        /// Retrieves a string representing the current menu array
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns>The current menu array</returns>
        public string ToMenuArray(string prefix)
        {
            return ToMenuArray(prefix, this);
        }

        /// <summary>
        /// Retrieves a string representing the current menu array
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="mn"></param>
        /// <returns>The current menu array</returns>
        public string ToMenuArray(string prefix, MenuTreeNodes mn)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < mn.Count; i++)
            {
                sb.Append(prefix);
                sb.Append(i + 1);
                sb.Append(mn[i].ToMenuArray());
                sb.Append(ToMenuArray(prefix + (i + 1).ToString() + "_", mn[i].Childs));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Node Width
        /// </summary>
        public Unit Width
        {
            get
            {
                double w = 0;
                for (int i = 0; i < this.Count; i++)
                {
                    w += this[i].Width.Value;
                }
                return new Unit(w + 1); //added correction
            }
        }

        /// <summary>
        /// Node Height
        /// </summary>
        public Unit Height
        {
            get
            {
                double h = 0;
                for (int i = 0; i < this.Count; i++)
                {
                    h += this[i].Height.Value + 1; //added correction
                }
                return new Unit(h + 1); //added correction
            }
        }
    }
}