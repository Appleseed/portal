using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text;

using Appleseed.Framework;
using Appleseed.Framework.Security;
using Appleseed.Framework.Settings;
using Appleseed.Framework.Site.Configuration;
using Appleseed.Framework.Web.UI.WebControls;
using History = Appleseed.Framework.History;
using Page = Appleseed.Framework.Web.UI.Page;

namespace Appleseed.ChangePassword.DesktopModules.CoreModules.ChangePassword
{
    //public partial class ChangePassword : System.Web.UI.UserControl
    public partial class ChangePassword : PortalModuleControl
    {
        string _UserEmail = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                _UserEmail = HttpContext.Current.User.Identity.Name.ToString();
            }
        }
        /// <summary>
        /// When the button is clicked all of the validation happens here 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void validateTextBoxes(object sender, EventArgs e)
        {   string message = null;

            if (_UserEmail != null)
            {
                if (tbNewPassword1.Text.Trim() == tbNewPassword.Text.Trim())
                {
                    if (checkCurrentPassowrd(tbCurrentPW.Text.Trim(), _UserEmail))
                    {
                        setNewPassword(tbNewPassword.Text.Trim());
                        message += "Your Password has been changed successfully.";
                    }
                    else
                    {
                        message += "You password is not correct." + Environment.NewLine;

                        tbCurrentPW.BackColor = System.Drawing.Color.LightSalmon;
                    }
                }
                else
                {
                    message += "You passwords do not match." + Environment.NewLine;

                    tbNewPassword1.BackColor = System.Drawing.Color.LightSalmon;
                    tbNewPassword.BackColor = System.Drawing.Color.LightSalmon;
                }

            }
            else
            {
                message += "You must fist Login to change your password." + Environment.NewLine;
            }

            if (message != null)
            {
                lblMessage.Text = message;
            }

        }
        /// <summary>
        /// A basic degator function that sets the passwords
        /// </summary>
        /// <param name="NewPassword"></param>
        private void setNewPassword(string NewPassword)
        {
            string error = "";
            error = setAppleseedPassword(NewPassword);
            //error = setAlternatePassword(NewPassword);
        }

        private string setAlternatePassword(string NewPassword)
        {
            string password = string.Empty;
            StringBuilder sb = new StringBuilder();
            string strCon = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["AlternateDB name"].ConnectionString;
            SqlConnection connection = new SqlConnection(strCon);
            try
            {
                connection.Open();
                SqlCommand query = new SqlCommand("SFP_sp_updatePassword", connection);
                query.CommandType = CommandType.StoredProcedure;

                query.Parameters.AddWithValue("@Email", _UserEmail);
                query.Parameters.AddWithValue("@Password", NewPassword);
                query.ExecuteNonQuery();

            }
            catch (SqlException ex)
            {
                for (int i = 0; i < ex.Errors.Count; i++)
                {
                    sb.Append("Index #" + i + "\n" +
                        "Message: " + ex.Errors[i].Message + "\n" +
                        "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                        "Source: " + ex.Errors[i].Source + "\n" +
                        "Procedure: " + ex.Errors[i].Procedure + "\n");
                }

            }
            connection.Close();


            return sb.ToString();
        }

        private string setAppleseedPassword(string NewPassword)
        {
            string password = string.Empty;
            StringBuilder sb = new StringBuilder();
            string strCon = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["Conectionstring"].ConnectionString;
            SqlConnection connection = new SqlConnection(strCon);
            try
            {
                connection.Open();
                SqlCommand query = new SqlCommand("AS_sp_updatePassword", connection);
                query.CommandType = CommandType.StoredProcedure;

                query.Parameters.AddWithValue("@Email", _UserEmail);
                query.Parameters.AddWithValue("@Password", NewPassword);
                query.ExecuteNonQuery();

            }
            catch (SqlException ex)
            {
                for (int i = 0; i < ex.Errors.Count; i++)
                {
                    sb.Append("Index #" + i + "\n" +
                        "Message: " + ex.Errors[i].Message + "\n" +
                        "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                        "Source: " + ex.Errors[i].Source + "\n" +
                        "Procedure: " + ex.Errors[i].Procedure + "\n");
                }

            }
            connection.Close();


            return sb.ToString();
        }

        private bool checkCurrentPassowrd(string currentPassword, string UserEmail)
        {
            
            string password = string.Empty;
            StringBuilder sb = new StringBuilder();
            string strCon = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["SocietyFP_app_dev"].ConnectionString;
            SqlConnection connection = new SqlConnection(strCon);
            try
            {
                connection.Open();
                SqlCommand query = new SqlCommand("SFP_sp_checkEmail", connection);
                query.CommandType = CommandType.StoredProcedure;

                query.Parameters.AddWithValue("@Email", UserEmail);
                
                using (SqlDataReader sqlReader = query.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        Int32 fellowsPassword = sqlReader.GetOrdinal("Password");
                        password = (!sqlReader.IsDBNull(fellowsPassword)) ? sqlReader.GetString(fellowsPassword) : String.Empty;
                    }
                }
            }
            catch (SqlException ex)
            {
                for (int i = 0; i < ex.Errors.Count; i++)
                {
                    sb.Append("Index #" + i + "\n" +
                        "Message: " + ex.Errors[i].Message + "\n" +
                        "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                        "Source: " + ex.Errors[i].Source + "\n" +
                        "Procedure: " + ex.Errors[i].Procedure + "\n");
                }
             
            }
            connection.Close();


            return (password != currentPassword) ? false : true;

        }
    }
}