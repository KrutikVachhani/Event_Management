using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using System.Data;
using Event_Management.Areas.Users.Models;

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
    }
}
