using System;
using System.ComponentModel;
using System.Reflection;
using System.Web.UI.WebControls;

namespace Appleseed.Framework.Security
{
    /// <summary>
    /// Summary description for SpecialRoles.
    /// </summary>
    public class SpecialRoles
    {
        /// <summary>
        /// Special roles used by the Appleseed system with arbitrary
        /// value for their ID by Brian Kierstead 4/15/2005
        /// </summary>
        public enum SpecialPortalRoles
        {
            /// <summary>
            /// 
            /// </summary>
            [Description("All Users")] AllUsers = -1,
            /// <summary>
            /// 
            /// </summary>
            [Description("Authenticated Users")] AuthenticatedUsers = -2,
            /// <summary>
            /// 
            /// </summary>
            [Description("Unauthenticated Users")] UnauthenticatedUsers = -3
        }

        /// <summary>
        /// Add the special roles found in SpecialPortalRoles
        /// </summary>
        /// <param name="listRoles">The list roles.</param>
        public static void populateSpecialRoles(ref CheckBoxList listRoles)
        {
            foreach (string s in Enum.GetNames(typeof (SpecialPortalRoles)))
            {
                SpecialPortalRoles desc = (SpecialPortalRoles) Enum.Parse(typeof (SpecialPortalRoles), s);
                string stringDesc = GetDescription(desc);
                listRoles.Items.Add(new ListItem
                                        (stringDesc, ((int) Enum.Parse(typeof (SpecialPortalRoles), s)).ToString()));
            }
        }

        /// <summary>
        /// Retrieve the description tag from the enum
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string GetDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes =
                (DescriptionAttribute[]) fi.GetCustomAttributes(
                                             typeof (DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }

        /// <summary>
        /// Return the description for the enum entry with value index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public static string GetRoleName(int index)
        {
            string s = Enum.GetName(typeof (SpecialPortalRoles), index);
            SpecialPortalRoles desc = (SpecialPortalRoles) Enum.Parse(typeof (SpecialPortalRoles), s);
            return GetDescription(desc);
        }
    }
}