using Event_Management.Areas.Venue.Models;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using System.Data;
using Event_Management.Areas.Event.Models;
using Event_Management.Areas.ClientEvent.Models;
using Newtonsoft.Json;
using Event_Management.BAL;

namespace Event_Management.DAL.ClientEvent
{
    public class ClientEventDALBase : DAL_Helper
    {
        #region SelectAll
        public DataTable PR_ClientEvent_SelectAll()
        {
            try
            {
                SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
                DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_ClientEvent_SelectAll");
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
        public ClientEventModel PR_ClientEvent_SelectByID(int ClientEventID)
        {
            ClientEventModel eventModel = new ClientEventModel();
            try
            {
                SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
                DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_ClientEvent_SelectByID");
                sqlDatabase.AddInParameter(dbCommand, "@ClientEventID", DbType.Int32, ClientEventID);
                DataTable dataTable = new DataTable();

                using (IDataReader dataReader = sqlDatabase.ExecuteReader(dbCommand))
                {
                    dataTable.Load(dataReader);
                }

                foreach (DataRow dr in dataTable.Rows)
                {
                    eventModel.ClientEventID = Convert.ToInt32(dr["ClientEventID"]);
                    eventModel.CEventName = dr["CEventName"].ToString();
                    eventModel.CEventDateTime = DateTime.Parse(dr["CEventDateTime"].ToString());
                    eventModel.CIsPrivate = Convert.ToBoolean(dr["CIsPrivate"]);
                    eventModel.VenueID = Convert.ToInt32(dr["VenueID"]);
                    eventModel.Created = DateTime.Parse(dr["Created"].ToString());
                    eventModel.Modified = DateTime.Parse(dr["Modified"].ToString());
                }
                return eventModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion


        #region Insert/Update
        public bool ClientEventSave(ClientEventModel eventModel)
        {
            SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
            try
            {
                if (eventModel.ClientEventID == 0)
                {
                    DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_ClientEvent_Insert");
                    sqlDatabase.AddInParameter(dbCommand, "@CEventName", DbType.String, eventModel.CEventName);
                    sqlDatabase.AddInParameter(dbCommand, "@CEventDateTime", DbType.DateTime, eventModel.CEventDateTime);
                    sqlDatabase.AddInParameter(dbCommand, "@CIsPrivate", DbType.Int32, eventModel.CIsPrivate);
                    sqlDatabase.AddInParameter(dbCommand, "@VenueID", DbType.String, eventModel.VenueID);
                    sqlDatabase.AddInParameter(dbCommand, "@UserID", DbType.String, CommonVariables.UserID());
                    string selectValue = string.Join(",", eventModel.SelectedService);
                    sqlDatabase.AddInParameter(dbCommand, "@SelectedService", DbType.String, selectValue);
                    bool isSuccess = Convert.ToBoolean(sqlDatabase.ExecuteNonQuery(dbCommand));
                    return isSuccess;
                }
                else
                {
                    DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_ClientEvent_Update");
                    sqlDatabase.AddInParameter(dbCommand, "@ClientEventID", DbType.Int32, eventModel.ClientEventID);
                    sqlDatabase.AddInParameter(dbCommand, "@CEventName", DbType.String, eventModel.CEventName);
                    sqlDatabase.AddInParameter(dbCommand, "@CEventDateTime", DbType.DateTime, eventModel.CEventDateTime);
                    sqlDatabase.AddInParameter(dbCommand, "@CIsPrivate", DbType.Int32, eventModel.CIsPrivate);
                    sqlDatabase.AddInParameter(dbCommand, "@VenueID", DbType.String, eventModel.VenueID);
                    string selectValue = string.Join(",", eventModel.SelectedService);
                    sqlDatabase.AddInParameter(dbCommand, "@SelectedService", DbType.String, selectValue);
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
        public bool PR_ClientEvent_Delete(int ClientEventID)
        {
            try
            {
                SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
                DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_ClientEvent_Delete");
                sqlDatabase.AddInParameter(dbCommand, "@ClientEventID", DbType.Int32, ClientEventID);
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
