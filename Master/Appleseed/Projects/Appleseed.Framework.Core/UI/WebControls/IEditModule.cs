using System;
using Appleseed.KickStarter.CommonClasses;

namespace Appleseed.Framework.Web.UI.WebControls
{
    /// <summary>
    /// Default interface for edit modules
    /// </summary>
    public interface IEditModule
    {
        #region Events

        /// <summary>
        /// Purpose: Property to Allow Control to add records.
        /// </summary>
        /// <value><c>true</c> if [allow add]; otherwise, <c>false</c>.</value>
        bool AllowAdd { set; get; }

        /// <summary>
        /// Purpose: Property to Allow Control to update records.
        /// </summary>
        /// <value><c>true</c> if [allow update]; otherwise, <c>false</c>.</value>
        bool AllowUpdate { set; get; }

        /// <summary>
        /// Purpose: Property to Allow Control to delete records.
        /// </summary>
        /// <value><c>true</c> if [allow delete]; otherwise, <c>false</c>.</value>
        bool AllowDelete { set; get; }

        #endregion

        #region Events

        /// <summary>
        /// Purpose: Notify when Record has been Selected, Inserted, Updated or Deleted.
        /// </summary>
        /// <delegate>DataChangeStartedEventHandler</delegate>
        event DataChangeEventHandler DataActionStart;

        /// <summary>
        /// Purpose: Notify when Record has been Selected, Inserted, Updated or Deleted.
        /// </summary>
        /// <delegate>DataChangeEndedEventHandler</delegate>
        event DataChangeEventHandler DataActionEnd;

        /// <summary>
        /// Purpose: Notify when Editing has been canceled.
        /// </summary>
        /// <delegate>EventHandler</delegate>
        event EventHandler CancelEdit;

        #endregion

        #region Methods

        /// <summary>
        /// The module select the requested item and starts editing the module.
        /// </summary>
        /// <param name="itemID">The param is string to be more general. Usually it contains a number.</param>
        void StartEdit(string itemID);

        /// <summary>
        /// Purpose: Method the List control after data has been updated by the Edit control.
        /// </summary>
        void Reset();

        /// <summary>
        /// Purpose: Displays any error messages.
        /// </summary>
        /// <param name="e">The Exception to display.</param>
        void HandleError(Exception e);

        #endregion
    }
}