using Event_Management.Areas.Venue.Models;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using System.Data;
using Event_Management.Areas.Event.Models;

namespace Event_Management.DAL.Event
{
    public class EventDALBase : DAL_Helper
    {
        #region SelectAll
        public DataTable PR_Event_SelectAll()
        {
            try
            {
                SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
                DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Event_SelectAll");
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
        public EventModel PR_Event_SelectByID(int EventID)
        {
            EventModel eventModel = new EventModel();
            try
            {
                SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
                DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Event_SelectByID");
                sqlDatabase.AddInParameter(dbCommand, "EventID", DbType.Int32, EventID);
                DataTable dataTable = new DataTable();

                using (IDataReader dataReader = sqlDatabase.ExecuteReader(dbCommand))
                {
                    dataTable.Load(dataReader);
                }

                foreach (DataRow dr in dataTable.Rows)
                {
                    eventModel.EventID = Convert.ToInt32(dr["EventID"]);
                    eventModel.EventName = dr["EventName"].ToString();
                    eventModel.EventDateTime = DateTime.Parse(dr["EventDateTime"].ToString());
                    eventModel.IsPrivate = Convert.ToBoolean(dr["IsPrivate"]);
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
        public bool EventSave(EventModel eventModel)
        {
            SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
            try
            {
                if (eventModel.EventID == 0)
                {
                    DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Event_Insert");
                    sqlDatabase.AddInParameter(dbCommand, "@EventName", DbType.String, eventModel.EventName);
                    sqlDatabase.AddInParameter(dbCommand, "@EventDateTime", DbType.DateTime, eventModel.EventDateTime);
                    sqlDatabase.AddInParameter(dbCommand, "@IsPrivate", DbType.Int32, eventModel.IsPrivate);
                    sqlDatabase.AddInParameter(dbCommand, "@VenueID", DbType.String, eventModel.VenueID);
                    bool isSuccess = Convert.ToBoolean(sqlDatabase.ExecuteNonQuery(dbCommand));
                    return isSuccess;
                }
                else
                {
                    DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Event_Update");
                    sqlDatabase.AddInParameter(dbCommand, "@EventID", DbType.Int32, eventModel.EventID);
                    sqlDatabase.AddInParameter(dbCommand, "@EventName", DbType.String, eventModel.EventName);
                    sqlDatabase.AddInParameter(dbCommand, "@EventDateTime", DbType.DateTime, eventModel.EventDateTime);
                    sqlDatabase.AddInParameter(dbCommand, "@IsPrivate", DbType.Int32, eventModel.IsPrivate);
                    sqlDatabase.AddInParameter(dbCommand, "@VenueID", DbType.String, eventModel.VenueID);
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
        public bool PR_Event_Delete(int EventID)
        {
            try
            {
                SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
                DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Event_Delete");
                sqlDatabase.AddInParameter(dbCommand, "@EventID", DbType.Int32, EventID);
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
