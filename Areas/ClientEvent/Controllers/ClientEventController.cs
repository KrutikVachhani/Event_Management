using Event_Management.Areas.ClientEvent.Models;
using Event_Management.Areas.Venue.Models;
using Event_Management.DAL.ClientEvent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;

namespace Event_Management.Areas.ClientEvent.Controllers
{
    [Area("ClientEvent")]
    [Route("ClientEvent/[Controller]/[Action]")]
    public class ClientEventController : Controller
    {
        #region Configuration

        public IConfiguration Configuration;
        public ClientEventController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #endregion

        #region DAL Object

        ClientEventDALBase clientEventDALBase = new ClientEventDALBase();

        #endregion

        #region Event List
        public IActionResult ClientEventList()
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
            DataTable dataTable = clientEventDALBase.PR_ClientEvent_SelectAll();
            return View(dataTable);
        }
        #endregion

        #region EventByID
        public IActionResult EventByID(int ClientEventID)
        {
            ClientEventModel eventModel = clientEventDALBase.PR_ClientEvent_SelectByID(ClientEventID);

            if (eventModel != null)
            {
                return View("EventByID", eventModel);
            }
            else
            {
                return RedirectToAction("ClientEventList");
            }
        }
        #endregion

        #region Event Add
        public IActionResult EventAdd(int ClientEventID)
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

            ClientEventModel eventModel = clientEventDALBase.PR_ClientEvent_SelectByID(ClientEventID);
            if (eventModel != null)
            {
                return View("ClientEventAddEdit", eventModel);
            }
            else
            {
                return View("ClientEventAddEdit");
            }
        }
        #endregion

        #region Event Save
        public IActionResult EventSave(ClientEventModel eventModel)
        {
                if (clientEventDALBase.ClientEventSave(eventModel))

                    return RedirectToAction("ClientEventList");

            
            return View("ClientEventAddEdit");
        }
        #endregion
        
        #region Event Delete
        public IActionResult EventDelete(int ClientEventID)
        {
            bool isSuccess = clientEventDALBase.PR_ClientEvent_Delete(ClientEventID);
            if (isSuccess)
            {
                return RedirectToAction("ClientEventList");
            }
            return RedirectToAction("ClientEventList");
        }
        #endregion

        #region Event Filter

        public IActionResult EventFilter(ClientEventModel model)
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
            ObjCmd.CommandText = "PR_ClientEvent_Filter";

            if (model.CEventName == null)
            {
                ObjCmd.Parameters.AddWithValue("@CEventName", DBNull.Value);
            }
            else
            {
                ObjCmd.Parameters.AddWithValue("@CEventName", model.CEventName);
            }

            if (model.CEventDateTime == DateTime.MinValue)
            {
                ObjCmd.Parameters.AddWithValue("@CEventDateTime", DBNull.Value);
            }
            else
            {
                ObjCmd.Parameters.AddWithValue("@CEventDateTime", model.CEventDateTime);
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

            return View("ClientEventList", dt);
        }

        #endregion
    }
}
