using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.SqlClient;
using Event_Management.Areas.SEC_User.Models;
using System.Net.Mail;
using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Policy;

namespace Event_Management.DAL.SCE_User
{
    public class SEC_UserDALBase : DAL_Helper
    {
        #region SelectByUserNamePassword
        public DataTable dbo_PR_User_SelectByUserNamePassword(string EmailAddress, string Password)
        {
            try
            {
                //string hashedPassword = dbo_PR_User_GetPassword(EmailAddress);
                SqlDatabase sqlDB = new SqlDatabase(connectionstr);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_User_SelectByUserNamePassword");
                sqlDB.AddInParameter(dbCMD, "EmailAddress", SqlDbType.VarChar, EmailAddress);

                sqlDB.AddInParameter(dbCMD, "Password", SqlDbType.VarChar, Password);

                DataTable dt = new DataTable();
                using (IDataReader dr = sqlDB.ExecuteReader(dbCMD))
                {
                    dt.Load(dr);
                }

                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region GetPassword

        public string dbo_PR_User_GetPassword(string EmailAddress)
        {
            try
            {
                string result = null;
                SqlDatabase sqlDB = new SqlDatabase(connectionstr);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_User_GetPassword");
                sqlDB.AddInParameter(dbCMD, "EmailAddress", SqlDbType.VarChar, EmailAddress);
                DataTable dt = new DataTable();

                using (IDataReader reader = sqlDB.ExecuteReader(dbCMD))
                {
                    if (reader.Read())
                    {
                        result = reader.GetString(0);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region User_Register
        public bool dbo_PR_SEC_User_Register(string UserName, string Password, string FirstName, string LastName, string PhotoPath, string EmailAddress)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(connectionstr);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("PR_User_SelectByEmail");
                sqlDB.AddInParameter(dbCMD, "EmailAddress", SqlDbType.VarChar, EmailAddress);
                DataTable dataTable = new DataTable();
                using (IDataReader dataReader = sqlDB.ExecuteReader(dbCMD))
                {
                    dataTable.Load(dataReader);
                }
                if (dataTable.Rows.Count > 0)
                {
                    return false;
                }
                else
                {
                    DbCommand dbCMD1 = sqlDB.GetStoredProcCommand("PR_Users_Insert");
                    sqlDB.AddInParameter(dbCMD1, "UserName", SqlDbType.VarChar, UserName);
                    sqlDB.AddInParameter(dbCMD1, "Password", SqlDbType.VarChar, Password);
                    sqlDB.AddInParameter(dbCMD1, "FirstName", SqlDbType.VarChar, FirstName);
                    sqlDB.AddInParameter(dbCMD1, "LastName", SqlDbType.VarChar, LastName);
                    sqlDB.AddInParameter(dbCMD1, "PhotoPath", SqlDbType.VarChar, PhotoPath);
                    sqlDB.AddInParameter(dbCMD1, "EmailAddress", SqlDbType.VarChar, EmailAddress);
                    if (Convert.ToBoolean(sqlDB.ExecuteNonQuery(dbCMD1)))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region UpdatePassword

        public bool dbo_PR_User_UpdatePassword(string EmailAddress, string Password)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(connectionstr);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_User_UpdatePassword");
                sqlDB.AddInParameter(dbCMD, "EmailAddress", SqlDbType.VarChar, EmailAddress);
                sqlDB.AddInParameter(dbCMD, "Password", SqlDbType.VarChar, Password);
                DataTable dt = new DataTable();

                bool isSuccess = Convert.ToBoolean(sqlDB.ExecuteNonQuery(dbCMD));
                return isSuccess;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        #endregion

        #region Hash Password
        public string HashPassword(string password)
        {
            byte[] salt = new byte[16];
            using(var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            byte[] hashed = Rfc2898DeriveBytes.Pbkdf2(
                password: password,
                salt: salt,
                iterations: 100000,
                hashAlgorithm: HashAlgorithmName.SHA256,
                outputLength:32
                );
            return BitConverter.ToString(salt).Replace("-", "") + ":" + BitConverter.ToString(hashed).Replace("-", "");
        }

        #endregion

        #region Verify Password

        public bool VerifyPassword(string password, string storedPassword)
        {
            string[] parts = storedPassword.Trim().Split(':');
            if(parts.Length != 2) return false;
            try
            {
                byte[] salt = Convert.FromBase64String(parts[0]);
                byte[] hashBytes = Convert.FromBase64String(parts[1]);

                byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                    password: password,
                    salt: salt,
                    iterations: 100000,
                    hashAlgorithm: HashAlgorithmName.SHA256,
                    outputLength: 32);

                return CryptographicOperations.FixedTimeEquals(hash, hashBytes);
            }
            catch(FormatException ex)
            {
                return false;
            }
        }

        #endregion
    }
}
