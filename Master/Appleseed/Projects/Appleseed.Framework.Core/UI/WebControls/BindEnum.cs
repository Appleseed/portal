namespace Appleseed.Framework.Web.UI.WebControls
{
    /// <summary>
    /// 
    /// </summary>
    [History("jperry", "2003/05/02", "Added BindOptionChildren and BindOptionSiblings and edited comments.")]
    public enum BindOption : int
    {
        /// <summary>
        /// No bind. Control will be filled by external procedure
        /// </summary>
        BindOptionNone = 0,
        /// <summary>
        /// First level will be binded. The topmost series of tabs (Home and sibilings)
        /// If structure permits, all decendents will also be bound.
        /// </summary>
        BindOptionTop = 1,
        /// <summary>
        /// Bind level under root level. (2nd level)
        /// If structure permits, all decendents will also be bound.
        /// </summary>
        BindOptionCurrentChilds = 2,
        /// <summary>
        /// If at root, bind children.  Otherwise bind siblings.
        /// If structure permits, all decendents will also be bound.
        /// </summary>
        BindOptionSubtabSibling = 3,
        /// <summary>
        /// Children of current tab will be bound. (All the tabs that have current tab as parent)
        /// If structure permits, all decendents will also be bound.
        /// </summary>
        BindOptionChildren = 4,
        /// <summary>
        /// Siblings of current tab will be bound. (Along with current tab)
        /// If structure permits, all decendents will also be bound.
        /// </summary>
        BindOptionSiblings = 5,
        //MH: added 30/04/2003 by mario@hartmann.net
        /// <summary>
        /// Use the variable BindParent.
        /// </summary>
        BindOptionDefinedParent = 6,
        //MH: added 02/10/2003 by mario@hartmann.net
        /// <summary>
        /// 
        /// </summary>
        BindOptionTabSibling = 7
        //MH: end
    }
}