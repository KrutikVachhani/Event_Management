using Event_Management.Areas.Service.Models;
using Event_Management.Areas.Venue.Models;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data;
using System.Data.Common;

namespace Event_Management.DAL.Service
{
    public class ServiceDALBase : DAL_Helper
    {
        #region SelectAll
        public DataTable PR_Service_SelectAll()
        {
            try
            {
                SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
                DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Service_SelectAll");
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
        public ServiceModel PR_Service_SelectByID(int ServiceID)
        {
            ServiceModel serviceModel = new ServiceModel();
            try
            {
                SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
                DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Service_SelectByID");
                sqlDatabase.AddInParameter(dbCommand, "ServiceID", DbType.Int32, ServiceID);
                DataTable dataTable = new DataTable();

                using (IDataReader dataReader = sqlDatabase.ExecuteReader(dbCommand))
                {
                    dataTable.Load(dataReader);
                }

                foreach (DataRow dr in dataTable.Rows)
                {
                    serviceModel.ServiceID = Convert.ToInt32(dr["ServiceID"]);
                    serviceModel.ServiceName = dr["ServiceName"].ToString();
                }
                return serviceModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion


        #region Insert/Update
        public bool ServiceSave(ServiceModel serviceModel)
        {
            SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
            try
            {
                if (serviceModel.ServiceID == 0)
                {
                    DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Service_Insert");
                    sqlDatabase.AddInParameter(dbCommand, "@ServiceName", DbType.String, serviceModel.ServiceName);
                    bool isSuccess = Convert.ToBoolean(sqlDatabase.ExecuteNonQuery(dbCommand));
                    return isSuccess;
                }
                else
                {
                    DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Service_Update");
                    sqlDatabase.AddInParameter(dbCommand, "@ServiceID", DbType.Int32, serviceModel.ServiceID);
                    sqlDatabase.AddInParameter(dbCommand, "@ServiceName", DbType.String, serviceModel.ServiceName);
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
        public bool PR_Service_Delete(int ServiceID)
        {
            try
            {
                SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
                DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Service_Delete");
                sqlDatabase.AddInParameter(dbCommand, "@ServiceID", DbType.Int32, ServiceID);
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
