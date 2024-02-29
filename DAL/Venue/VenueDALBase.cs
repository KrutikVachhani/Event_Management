using Event_Management.Areas.Venue.Models;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data;
using System.Data.Common;

namespace Event_Management.DAL.Venue
{
    public class VenueDALBase : DAL_Helper
    {
        #region SelectAll
        public DataTable PR_Venue_SelectAll()
        {
            try
            {
                SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
                DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Venue_SelectAll");
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
        public VenueModel PR_Venue_SelectByID(int VenueID)
        {
            VenueModel venueModel = new VenueModel();
            try
            {
                SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
                DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Venue_SelectByID");
                sqlDatabase.AddInParameter(dbCommand, "VenueID", DbType.Int32, VenueID);
                DataTable dataTable = new DataTable();

                using (IDataReader dataReader = sqlDatabase.ExecuteReader(dbCommand))
                {
                    dataTable.Load(dataReader);
                }

                foreach (DataRow dr in dataTable.Rows)
                {
                    venueModel.VenueID = Convert.ToInt32(dr["VenueID"]);
                    venueModel.VenueName = dr["VenueName"].ToString();
                    venueModel.Location = dr["Location"].ToString();
                    venueModel.Capacity = Convert.ToInt32(dr["Capacity"]);
                    venueModel.ContactPerson = dr["ContactPerson"].ToString();
                    venueModel.ContactNumber = Convert.ToDouble(dr["ContactNumber"]);
                }
                return venueModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion


        #region Insert/Update
        public bool VenueSave(VenueModel venueModel)
        {
            SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
            try
            {
                if (venueModel.VenueID == 0)
                {
                    DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Venue_Insert");
                    sqlDatabase.AddInParameter(dbCommand, "@VenueName", DbType.String, venueModel.VenueName);
                    sqlDatabase.AddInParameter(dbCommand, "@Location", DbType.String, venueModel.Location);
                    sqlDatabase.AddInParameter(dbCommand, "@Capacity", DbType.Int32, venueModel.Capacity);
                    sqlDatabase.AddInParameter(dbCommand, "@ContactPerson", DbType.String, venueModel.ContactPerson);
                    sqlDatabase.AddInParameter(dbCommand, "@ContactNumber", DbType.Double, venueModel.ContactNumber);
                    bool isSuccess = Convert.ToBoolean(sqlDatabase.ExecuteNonQuery(dbCommand));
                    return isSuccess;
                }
                else
                {
                    DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Venue_Update");
                    sqlDatabase.AddInParameter(dbCommand, "@VenueID", DbType.Int32, venueModel.VenueID);
                    sqlDatabase.AddInParameter(dbCommand, "@VenueName", DbType.String, venueModel.VenueName);
                    sqlDatabase.AddInParameter(dbCommand, "@Location", DbType.String, venueModel.Location);
                    sqlDatabase.AddInParameter(dbCommand, "@Capacity", DbType.Int32, venueModel.Capacity);
                    sqlDatabase.AddInParameter(dbCommand, "@ContactPerson", DbType.String, venueModel.ContactPerson);
                    sqlDatabase.AddInParameter(dbCommand, "@ContactNumber", DbType.Double, venueModel.ContactNumber);
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
        public bool PR_Venue_Delete(int VenueID)
        {
            try
            {
                SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
                DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Venue_Delete");
                sqlDatabase.AddInParameter(dbCommand, "@VenueID", DbType.Int32, VenueID);
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
