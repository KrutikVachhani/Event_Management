using Microsoft.AspNetCore.Mvc;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Event_Management.Areas.Schedule.Models;
using System.Data;
using System.Data.Common;
using Event_Management.Areas.Event.Models;

namespace Event_Management.DAL.Schedule
{
    public class ScheduleDALBase : DAL_Helper
    {
        public DataTable Schedule()
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

        public EventModel Schedule_Search(DateTime Date)
        {
            EventModel eventModel = new EventModel();
            try
            {
                SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
                DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Event_Search");
                sqlDatabase.AddInParameter(dbCommand, "EventDateTime", DbType.DateTime, Date);
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
            catch(Exception ex) {
                return null;
            }
        }
    }
}
