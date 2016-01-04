using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Web.Security;
using Appleseed.Framework.Settings;

namespace Appleseed.Framework.Helpers
{
    /// <summary>
    /// Summary description for CryptoHelper.
    /// </summary>
    public class CryptoHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:CryptoHelper"/> class.
        /// </summary>
        public CryptoHelper()
        {
        }

        /// <summary>
        /// Creates the salt.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public string CreateSalt(int size)
        {
            // generate cryptographic rand numb.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);
            // return base64 string representation of random number
            return Convert.ToBase64String(buff);
        }

        /// <summary>
        /// Creates the password hash.
        /// </summary>
        /// <param name="pwd">The PWD.</param>
        /// <param name="salt">The salt.</param>
        /// <returns></returns>
        public string CreatePasswordHash(string pwd, string salt)
        {
            string saltAndPwd = String.Concat(pwd, salt);
            string hashedPwd = FormsAuthentication.HashPasswordForStoringInConfigFile(saltAndPwd, "SHA1");
            return hashedPwd;
        }

        /// <summary>
        /// Hashes the passwords.
        /// </summary>
        public void HashPasswords()
        {
            string salt;
            //string password;
            SqlParameter parameterPassword;
            SqlParameter parameterSalt;
            SqlParameter parameterEmail;
            using (SqlConnection myConnection = Config.SqlConnectionString)
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT Email, Password FROM rb_Users", myConnection);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                DataSet ds = new DataSet();
                myConnection.Open();
                adapter.Fill(ds, "rb_Users");
                foreach (DataRow dr in ds.Tables["rb_Users"].Rows)
                {
                    using (SqlCommand insertCommand = new SqlCommand("rb_HashPasswords", myConnection))
                    {
                        insertCommand.CommandType = CommandType.StoredProcedure;

                        salt = CreateSalt(5);
                        parameterEmail = new SqlParameter("@Email", SqlDbType.NVarChar, 100);
                        parameterEmail.Value = dr["Email"];
                        insertCommand.Parameters.Add(parameterEmail);
                        parameterPassword = new SqlParameter("@Password", SqlDbType.VarChar, 40);
                        parameterPassword.Value = CreatePasswordHash(dr["Password"].ToString(), salt);
                        insertCommand.Parameters.Add(parameterPassword);
                        parameterSalt = new SqlParameter("@Salt", SqlDbType.VarChar, 10);
                        parameterSalt.Value = salt;
                        insertCommand.Parameters.Add(parameterSalt);

                        insertCommand.ExecuteNonQuery();
                    }
                }//Added by Ashish - Connection Pool Issue
                myConnection.Close();
            }
        }

        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="randomPassword">The random password.</param>
        /// <remarks>update db with random generated password</remarks>
        public void ResetPassword(string email, string randomPassword)
        {
            string salt;
            SqlParameter parameterPassword;
            SqlParameter parameterSalt;
            SqlParameter parameterEmail;
            using (SqlConnection myConnection = Config.SqlConnectionString)
            {
                using (SqlCommand insertCommand = new SqlCommand("rb_HashPasswords", myConnection))
                {
                    // Mark the Command as a SPROC
                    insertCommand.CommandType = CommandType.StoredProcedure;
                    // Open the database connection and execute the command
                    myConnection.Open();

                    salt = CreateSalt(5);
                    parameterEmail = new SqlParameter("@Email", SqlDbType.NVarChar, 100);
                    parameterEmail.Value = email;
                    insertCommand.Parameters.Add(parameterEmail);
                    parameterPassword = new SqlParameter("@Password", SqlDbType.VarChar, 40);
                    parameterPassword.Value = CreatePasswordHash(randomPassword, salt);
                    insertCommand.Parameters.Add(parameterPassword);
                    parameterSalt = new SqlParameter("@Salt", SqlDbType.VarChar, 10);
                    parameterSalt.Value = salt;
                    insertCommand.Parameters.Add(parameterSalt);

                    insertCommand.ExecuteNonQuery();
                    myConnection.Close(); //Added by Ashish - Connection Pool Issue
                }
            }
        }
    }
}