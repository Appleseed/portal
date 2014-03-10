namespace Appleseed.Framework.Content.Security
{
    using System;
    using System.ComponentModel;
    using System.Web.UI.WebControls;

    /// <summary>
    /// The special roles.
    /// </summary>
    public class SpecialRoles
    {
        #region Enums

        /// <summary>
        /// Special roles used by the Appleseed system with arbitrary
        ///     value for their ID by Brian Kierstead 4/15/2005
        /// </summary>
        public enum SpecialPortalRoles
        {
            /// <summary>
            ///     All Users.
            /// </summary>
            [Description("All Users")]
            AllUsers = -1, 

            /// <summary>
            ///     Authenticated Users
            /// </summary>
            [Description("Authenticated Users")]
            AuthenticatedUsers = -2, 

            /// <summary>
            ///     Unauthenticated Users
            /// </summary>
            [Description("Unauthenticated Users")]
            UnauthenticatedUsers = -3
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieve the description tag from the enum
        /// </summary>
        /// <param name="value">
        /// The enum value to get the description for.
        /// </param>
        /// <returns>
        /// The description.
        /// </returns>
        public static string GetDescription(Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }

        /// <summary>
        /// Return the description for the enum entry with value index.
        /// </summary>
        /// <param name="index">
        /// The index to the role.
        /// </param>
        /// <returns>
        /// The role name.
        /// </returns>
        public static string GetRoleName(int index)
        {
            var s = Enum.GetName(typeof(SpecialPortalRoles), index);
            var desc = (SpecialPortalRoles)Enum.Parse(typeof(SpecialPortalRoles), s);
            return GetDescription(desc);
        }

        /// <summary>
        /// Add the special roles found in SpecialPortalRoles
        /// </summary>
        /// <param name="listRoles">
        /// A checkbox list.
        /// </param>
        /// TODO: Fix Name Violation after verifying that this isn't referenced anywhere in javascript, etc.
        public static void populateSpecialRoles(ref CheckBoxList listRoles)
        {
            foreach (var s in Enum.GetNames(typeof(SpecialPortalRoles)))
            {
                var desc = (SpecialPortalRoles)Enum.Parse(typeof(SpecialPortalRoles), s);
                var stringDesc = GetDescription(desc);
                listRoles.Items.Add(
                    new ListItem(stringDesc, ((int)Enum.Parse(typeof(SpecialPortalRoles), s)).ToString()));
            }
        }

        #endregion
    }
}