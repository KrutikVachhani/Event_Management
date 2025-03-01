﻿using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.SqlClient;

namespace Event_Management.DAL.SCE_User
{
    public class SEC_UserDALBase : DAL_Helper
    {
        #region SelectByUserNamePassword
        public DataTable dbo_PR_User_SelectByUserNamePassword(string EmailAddress, string Password)
        {
            try
            {
                string hashedPassword = dbo_PR_User_GetPassword(EmailAddress);
                SqlDatabase sqlDB = new SqlDatabase(connectionstr);
                DbCommand dbCMD = sqlDB.GetStoredProcCommand("dbo.PR_User_SelectByUserNamePassword");
                sqlDB.AddInParameter(dbCMD, "EmailAddress", SqlDbType.VarChar, EmailAddress);

                sqlDB.AddInParameter(dbCMD, "Password", SqlDbType.VarChar, hashedPassword);

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


    }
}
