namespace Appleseed.Framework.Content.Security
{
    using Appleseed.Framework.Security;
    using Appleseed.Framework.Site.Configuration;

    /// <summary>
    /// The discussion permissions.
    /// </summary>
    public class DiscussionPermissions
    {
        #region Public Methods

        /// <summary>
        /// See whether the current user has permissions to add a post to the discussion thread
        /// </summary>
        /// <param name="moduleId">
        /// ID of the current Discussion Module
        /// </param>
        /// <returns>
        /// Returns true or flase
        /// </returns>
        public static bool HasAddPermissions(int moduleId)
        {
            return PortalSecurity.HasAddPermissions(moduleId);
        }

        /// <summary>
        /// Determines whether [has delete permissions] [the specified module ID].
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <param name="itemId">
        /// The item ID.
        /// </param>
        /// <param name="itemUserEmail">
        /// The item user email.
        /// </param>
        /// <returns>
        /// <c>true</c> if [has delete permissions] [the specified module ID]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasDeletePermissions(int moduleId, int itemId, string itemUserEmail)
        {
            // string currentUserEmail = PortalSettings.CurrentUser.Identity.Email;
            // if true then
            // || currentUserEmail == itemUserEmail))
            // also need to check for NUMBER of children
            // so someone doesn't delte a post with children
            // or just reattach the children
            return PortalSecurity.HasDeletePermissions(moduleId);
        }

        /// <summary>
        /// Determines whether [has edit permissions] [the specified module ID].
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <param name="itemUserEmail">
        /// The item user email.
        /// </param>
        /// <returns>
        /// <c>true</c> if [has edit permissions] [the specified module ID]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasEditPermissions(int moduleId, string itemUserEmail)
        {
            // this approach willnot be safe when we change from UserEmail to UserID
            // if UserID is not unique accross ALL portal instances on a given database
            var currentUserEmail = PortalSettings.CurrentUser.Identity.Email;

            return PortalSecurity.HasEditPermissions(moduleId) || currentUserEmail == itemUserEmail;
        }

        #endregion
    }
}