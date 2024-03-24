using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using System.Data;
using Event_Management.Areas.Users.Models;
using Event_Management.Areas.Venue.Models;
using Microsoft.AspNetCore.Mvc;
using DocumentFormat.OpenXml.Spreadsheet;
using MessagePack;

namespace Event_Management.DAL.Users
{
    public class UserDALBase : DAL_Helper
    {
        #region SelectAll
        public DataTable PR_Users_SelectAll()
        {
            try
            {
                SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
                DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Users_SelectAll");
                DataTable dataTable = new DataTable();

                using (IDataReader dataReader = sqlDatabase.ExecuteReader(dbCommand))
                {
                    dataTable.Load(dataReader);
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Admin SelectAll
        public DataTable PR_Admin_SelectAll()
        {
            try
            {
                SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
                DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Admin_SelectAll");
                DataTable dataTable = new DataTable();

                using (IDataReader dataReader = sqlDatabase.ExecuteReader(dbCommand))
                {
                    dataTable.Load(dataReader);
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Client SelectAll
        public DataTable PR_Client_SelectAll()
        {
            try
            {
                SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
                DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Client_SelectAll");
                DataTable dataTable = new DataTable();

                using (IDataReader dataReader = sqlDatabase.ExecuteReader(dbCommand))
                {
                    dataTable.Load(dataReader);
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Customer_SelectAll
        public DataTable PR_Customer_SelectAll()
        {
            try
            {
                SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
                DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Customer_SelectAll");
                DataTable dataTable = new DataTable();

                using (IDataReader dataReader = sqlDatabase.ExecuteReader(dbCommand))
                {
                    dataTable.Load(dataReader);
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region SelectByID
        public UserModel PR_Users_SelectByID(int UserID)
        {
            UserModel userModel = new UserModel();
            try
            {
                SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
                DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Users_SelectByPK");
                sqlDatabase.AddInParameter(dbCommand, "UserID", DbType.Int32, UserID);
                DataTable dataTable = new DataTable();

                using (IDataReader dataReader = sqlDatabase.ExecuteReader(dbCommand))
                {
                    dataTable.Load(dataReader);
                }

                foreach (DataRow dr in dataTable.Rows)
                {
                    userModel.UserID = Convert.ToInt32(dr["UserID"]);
                    userModel.UserName = dr["UserName"].ToString();
                    userModel.Password = dr["Password"].ToString();
                    userModel.FirstName = dr["FirstName"].ToString();
                    userModel.LastName = dr["LastName"].ToString();
                    userModel.EmailAddress = dr["EmailAddress"].ToString();
                    userModel.PhotoPath = dr["PhotoPath"].ToString();
                    userModel.Created = DateTime.Parse(dr["Created"].ToString());
                }
                return userModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Insert/Update
        public bool UsersSave(UserModel userModel)
        {
            SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
            try
            {
                if (userModel.UserID == 0)
                {
                    #region File Upload
                    if (userModel.File != null)
                    {
                        string FilePath = "wwwroot\\Upload";
                        string path = Path.Combine(Directory.GetCurrentDirectory(), FilePath);
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string fileNameWithPath = Path.Combine(path, userModel.File.FileName);

                        userModel.PhotoPath = "~" + FilePath.Replace("wwwroot\\", "/") + "/" + userModel.File.FileName;

                        using (FileStream fileStream = new FileStream(fileNameWithPath, FileMode.Create))
                        {
                            userModel.File.CopyTo(fileStream);
                        }
                    }
                    #endregion

                    DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Users_Insert");
                    sqlDatabase.AddInParameter(dbCommand, "@UserName", DbType.String, userModel.UserName);
                    sqlDatabase.AddInParameter(dbCommand, "@Password", DbType.String, userModel.Password);
                    sqlDatabase.AddInParameter(dbCommand, "@FirstName", DbType.String, userModel.FirstName);
                    sqlDatabase.AddInParameter(dbCommand, "@LastName", DbType.String, userModel.LastName);
                    sqlDatabase.AddInParameter(dbCommand, "@PhotoPath", DbType.String, userModel.PhotoPath);
                    sqlDatabase.AddInParameter(dbCommand, "@EmailAddress", DbType.String, userModel.EmailAddress);
                    bool isSuccess = Convert.ToBoolean(sqlDatabase.ExecuteNonQuery(dbCommand));
                    return isSuccess;
                }
                else
                {
                    #region File Upload
                    if (userModel.File != null)
                    {
                        string FilePath = "wwwroot\\Upload";
                        string path = Path.Combine(Directory.GetCurrentDirectory(), FilePath);
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string fileNameWithPath = Path.Combine(path, userModel.File.FileName);

                        userModel.PhotoPath = "~" + FilePath.Replace("wwwroot\\", "/") + "/" + userModel.File.FileName;

                        using (FileStream fileStream = new FileStream(fileNameWithPath, FileMode.Create))
                        {
                            userModel.File.CopyTo(fileStream);
                        }
                    }
                    #endregion

                    DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Users_Update");
                    sqlDatabase.AddInParameter(dbCommand, "@UserID", DbType.Int32, userModel.UserID);
                    sqlDatabase.AddInParameter(dbCommand, "@UserName", DbType.String, userModel.UserName);
                    sqlDatabase.AddInParameter(dbCommand, "@Password", DbType.String, userModel.Password);
                    sqlDatabase.AddInParameter(dbCommand, "@FirstName", DbType.String, userModel.FirstName);
                    sqlDatabase.AddInParameter(dbCommand, "@LastName", DbType.String, userModel.LastName);
                    sqlDatabase.AddInParameter(dbCommand, "@PhotoPath", DbType.String, userModel.PhotoPath);
                    sqlDatabase.AddInParameter(dbCommand, "@EmailAddress", DbType.String, userModel.EmailAddress);
                    bool isSuccess = Convert.ToBoolean(sqlDatabase.ExecuteNonQuery(dbCommand));
                    return isSuccess;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region Delete
        public bool PR_Users_Delete(int UserID)
        {
            try
            {
                SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
                DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Users_Delete");
                sqlDatabase.AddInParameter(dbCommand, "@UserID", DbType.Int32, UserID);
                bool isSuccess = Convert.ToBoolean(sqlDatabase.ExecuteNonQuery(dbCommand));
                return isSuccess;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        #endregion

        #region User Filter

        public DataTable UserFilter(UserModel model)
        {
            SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
            DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Users_Filter");

            if (model.UserName == null)
            {
                sqlDatabase.AddInParameter(dbCommand, "UserName", DbType.String, DBNull.Value);
            }
            else
            {
                sqlDatabase.AddInParameter(dbCommand, "UserName", DbType.String, model.UserName);
            }

            if (model.FirstName == null)
            {
                sqlDatabase.AddInParameter(dbCommand, "FirstName", DbType.String, DBNull.Value);
            }
            else
            {
                sqlDatabase.AddInParameter(dbCommand, "FirstName", DbType.String, model.FirstName);
            }

            if (model.LastName == null)
            {
                sqlDatabase.AddInParameter(dbCommand, "LastName", DbType.String, DBNull.Value);
            }
            else
            {
                sqlDatabase.AddInParameter(dbCommand, "LastName", DbType.String, model.LastName);
            }

            DataTable dt = new DataTable();

            using (IDataReader dataReader = sqlDatabase.ExecuteReader(dbCommand))
            {
                dt.Load(dataReader);
            }
            return dt;
        }

        #endregion
    }
}
