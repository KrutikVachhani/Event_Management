using Event_Management.Areas.Event.Models;
using Event_Management.Areas.Venue.Models;
using Event_Management.DAL.Event;
using Event_Management.DAL.Venue;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Event_Management.Areas.Event.Controllers
{
    [Area("Event")]
    [Route("Event/[Controller]/[Action]")]
    public class EventController : Controller
    {
        public IConfiguration Configuration;
        public EventController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        EventDALBase eventDALBase = new EventDALBase();

        #region Event List
        public IActionResult EventList()
        {
            DataTable dataTable = eventDALBase.PR_Event_SelectAll();
            return View(dataTable);
        }
        #endregion

        #region Event By ID
        public IActionResult EventAdd(int EventID)
        {
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
            if (ModelState.IsValid)
            {
                if (eventDALBase.EventSave(eventModel))

                    return RedirectToAction("EventList");

            }
            return View("EventAddEdit");
        }
        #endregion


        #region Event Delete
        public IActionResult EventDelete(int VenueID)
        {
            bool isSuccess = eventDALBase.PR_Event_Delete(VenueID);
            if (isSuccess)
            {
                return RedirectToAction("EventList");
            }
            return RedirectToAction("EventList");
        }
        #endregion
    }
}
