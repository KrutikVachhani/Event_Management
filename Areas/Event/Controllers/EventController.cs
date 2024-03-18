using DocumentFormat.OpenXml.Office2013.Excel;
using Event_Management.Areas.Event.Models;
using Event_Management.Areas.Venue.Models;
using Event_Management.DAL.Event;
using Event_Management.DAL.Venue;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;

namespace Event_Management.Areas.Event.Controllers
{
    [Area("Event")]
    [Route("Event/[Controller]/[Action]")]
    public class EventController : Controller
    {
        #region Configuration

        public IConfiguration Configuration;
        public EventController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #endregion

        #region DAL Object

        EventDALBase eventDALBase = new EventDALBase();

        #endregion

        #region Event List
        public IActionResult EventList()
        {
            #region Venue DropDown
            string connectionstr = this.Configuration.GetConnectionString("myConnectionString");
            DataTable dt = new DataTable();
            SqlConnection sqlConnection = new SqlConnection(connectionstr);
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "PR_Venue_SelectAll";
            SqlDataReader Op_sqlDataReader = sqlCommand.ExecuteReader();
            DataTable Op_dt = new DataTable();
            Op_dt.Load(Op_sqlDataReader);

            List<VenueModel> venueModels = new List<VenueModel>();

            foreach (DataRow dr in Op_dt.Rows)
            {
                VenueModel venue = new VenueModel();
                venue.VenueID = int.Parse(dr["VenueID"].ToString());
                venue.VenueName = dr["VenueName"].ToString();
                venueModels.Add(venue);
            }
            ViewBag.VenueList = venueModels;
            #endregion
            DataTable dataTable = eventDALBase.PR_Event_SelectAll();
            return View(dataTable);
        }
        #endregion

        #region EventByID
        public IActionResult EventByID(int EventID)
        {
            EventModel eventModel = eventDALBase.PR_Event_SelectByID(EventID);

            if (eventModel != null)
            {
                return View("EventByID", eventModel);
            }
            else
            {
                return RedirectToAction("EventList");
            }
        }
        #endregion

        #region Event Add
        public IActionResult EventAdd(int EventID)
        {
            #region Venue DropDown
            string connectionstr = this.Configuration.GetConnectionString("myConnectionString");
            DataTable dt = new DataTable();
            SqlConnection sqlConnection = new SqlConnection(connectionstr);
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "PR_Venue_SelectAll";
            SqlDataReader Op_sqlDataReader = sqlCommand.ExecuteReader();
            DataTable Op_dt = new DataTable();
            Op_dt.Load(Op_sqlDataReader);

            List<VenueModel> venueModels = new List<VenueModel>();

            foreach (DataRow dr in Op_dt.Rows)
            {
                VenueModel venue = new VenueModel();
                venue.VenueID = int.Parse(dr["VenueID"].ToString());
                venue.VenueName = dr["VenueName"].ToString();
                venueModels.Add(venue);
            }
            ViewBag.VenueList = venueModels;
            #endregion

            #region Service DropDown
            string connection = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection sqlConnections = new SqlConnection(connection);
            sqlConnections.Open();
            SqlCommand sqlCommands = sqlConnection.CreateCommand();
            sqlCommands.CommandType = CommandType.StoredProcedure;
            sqlCommands.CommandText = "PR_Service_SelectAll";
            SqlDataReader sqlDataReader = sqlCommands.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(sqlDataReader);

            List<ServiceModel> serviceModels = new List<ServiceModel>();

            foreach (DataRow dr in dataTable.Rows)
            {
                ServiceModel service = new ServiceModel();
                service.ServiceID = int.Parse(dr["ServiceID"].ToString());
                service.ServiceName = dr["ServiceName"].ToString();
                serviceModels.Add(service);
            }
            ViewBag.ServiceList = serviceModels;
            #endregion

            EventModel eventModel = eventDALBase.PR_Event_SelectByID(EventID);
            if (eventModel != null)
            {
                return View("EventAddEdit", eventModel);
            }
            else
            {
                return View("EventAddEdit");
            }
        }
        #endregion

        #region Event Save
        public IActionResult EventSave(EventModel eventModel)
        {
                if (eventDALBase.EventSave(eventModel))

                    return RedirectToAction("EventList");

            
            return View("EventAddEdit");
        }
        #endregion
        
        #region Event Delete
        public IActionResult EventDelete(int EventID)
        {
            bool isSuccess = eventDALBase.PR_Event_Delete(EventID);
            if (isSuccess)
            {
                return RedirectToAction("EventList");
            }
            return RedirectToAction("EventList");
        }
        #endregion

        #region Event Filter

        public IActionResult EventFilter(EventModel model)
        {
            #region Venue DropDown
            string connection = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection sqlConnections = new SqlConnection(connection);
            sqlConnections.Open();
            SqlCommand sqlCommand = sqlConnections.CreateCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "PR_Venue_SelectAll";
            SqlDataReader Op_sqlDataReader = sqlCommand.ExecuteReader();
            DataTable Op_dt = new DataTable();
            Op_dt.Load(Op_sqlDataReader);

            List<VenueModel> venueModels = new List<VenueModel>();

            foreach (DataRow dr in Op_dt.Rows)
            {
                VenueModel venue = new VenueModel();
                venue.VenueID = int.Parse(dr["VenueID"].ToString());
                venue.VenueName = dr["VenueName"].ToString();
                venueModels.Add(venue);
            }
            ViewBag.VenueList = venueModels;
            #endregion

            string connectionstr = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection sqlConnection = new SqlConnection(connectionstr);
            sqlConnection.Open();

            SqlCommand ObjCmd = sqlConnection.CreateCommand();

            ObjCmd.CommandType = CommandType.StoredProcedure;
            ObjCmd.CommandText = "PR_EventDetails_Filter";

            if (model.EventName == null)
            {
                ObjCmd.Parameters.AddWithValue("@EventName", DBNull.Value);
            }
            else
            {
                ObjCmd.Parameters.AddWithValue("@EventName", model.EventName);
            }

            if (model.EventDateTime == DateTime.MinValue)
            {
                ObjCmd.Parameters.AddWithValue("@EventDateTime", DBNull.Value);
            }
            else
            {
                ObjCmd.Parameters.AddWithValue("@EventDateTime", model.EventDateTime);
            }


            if (model.VenueID == 0)
            {
                ObjCmd.Parameters.AddWithValue("@VenueID", DBNull.Value);
            }
            else
            {
                ObjCmd.Parameters.AddWithValue("@VenueID", model.VenueID);
            }

            SqlDataReader sqlDataReader = ObjCmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(sqlDataReader);

            return View("EventList", dt);
        }

        #endregion
    }
}
