using System;
namespace Appleseed.Framework.Security
{
    /// <summary>
    /// Implement this interface in custom edit / register profile classes
    /// </summary>
    public interface IEditUserProfile
    {
        /// <summary>
        /// Returns true when from control is on edit mode
        /// </summary>
        bool EditMode { get; }

        /// <summary>
        /// Stores the page where to redirect user on save
        /// </summary>
        string RedirectPage { get; set; }

        /// <summary>
        /// This procedure should persist the user data on db
        /// </summary>
        /// <returns>The user id</returns>
        Guid SaveUserData();
    }
}