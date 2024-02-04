using Event_Management.Areas.Comment.Models;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using System.Data;

namespace Event_Management.DAL.Comment
{
    public class CommentDALBase : DAL_Helper
    {
        #region SelectAll
        public DataTable PR_Comment_SelectAll()
        {
            try
            {
                SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
                DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Comment_SelectAll");
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
        public CommentModel PR_Comment_SelectByID(int CommentID)
        {
            CommentModel commentModel = new CommentModel();
            try
            {
                SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
                DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Comment_SelectByPK");
                sqlDatabase.AddInParameter(dbCommand, "CommentID", DbType.Int32, CommentID);
                DataTable dataTable = new DataTable();

                using (IDataReader dataReader = sqlDatabase.ExecuteReader(dbCommand))
                {
                    dataTable.Load(dataReader);
                }

                foreach (DataRow dr in dataTable.Rows)
                {
                    commentModel.CommentID = Convert.ToInt32(dr["CommentID"]);
                    commentModel.Name = dr["Name"].ToString();
                    commentModel.Email = dr["Email"].ToString();
                    commentModel.PhoneNumber = dr["PhoneNumber"].ToString();
                    commentModel.Message = dr["Message"].ToString();
                    commentModel.Created = DateTime.Parse(dr["Created"].ToString());
                }
                return commentModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion


        #region Insert/Update
        public bool CommentSave(CommentModel commentModel)
        {
            SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
            try
            {
                if (commentModel.CommentID == 0)
                {
                    DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Comment_Insert");
                    sqlDatabase.AddInParameter(dbCommand, "@Name", DbType.String, commentModel.Name);
                    sqlDatabase.AddInParameter(dbCommand, "@EventID", DbType.Int32, commentModel.EventID);
                    sqlDatabase.AddInParameter(dbCommand, "@Email", DbType.String, commentModel.Email);
                    sqlDatabase.AddInParameter(dbCommand, "@PhoneNumber", DbType.String, commentModel.PhoneNumber);
                    sqlDatabase.AddInParameter(dbCommand, "@Message", DbType.String, commentModel.Message);
                    bool isSuccess = Convert.ToBoolean(sqlDatabase.ExecuteNonQuery(dbCommand));
                    return isSuccess;
                }
                else
                {
                    DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Comment_Update");
                    sqlDatabase.AddInParameter(dbCommand, "@UserID", DbType.Int32, commentModel.CommentID);
                    sqlDatabase.AddInParameter(dbCommand, "@UserName", DbType.String, commentModel.Name);
                    sqlDatabase.AddInParameter(dbCommand, "@Password", DbType.String, commentModel.Email);
                    sqlDatabase.AddInParameter(dbCommand, "@FirstName", DbType.String, commentModel.PhoneNumber);
                    sqlDatabase.AddInParameter(dbCommand, "@LastName", DbType.String, commentModel.Message);
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
        public bool PR_Comment_Delete(int CommentID)
        {
            try
            {
                SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
                DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Comment_Delete");
                sqlDatabase.AddInParameter(dbCommand, "@CommentID", DbType.Int32, CommentID);
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
