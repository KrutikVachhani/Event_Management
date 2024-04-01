using Event_Management.Areas.Venue.Models;
using Event_Management.DAL.Venue;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace Event_Management.Areas.Venue.Controllers
{
    [Area("Venue")]
    [Route("Venue/[Controller]/[Action]")]
    public class VenueController : Controller
    {

        public IConfiguration Configuration;
        public VenueController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        VenueDALBase venueDALBase = new VenueDALBase();

        #region Venue List
        public IActionResult VenueList()
        {
            DataTable dataTable = venueDALBase.PR_Venue_SelectAll();
            return View(dataTable);
        }
        #endregion

        #region Category By ID
        public IActionResult VenueAdd(int VenueID)
        {
            VenueModel venueModel = venueDALBase.PR_Venue_SelectByID(VenueID);
            if (venueModel != null)
            {
                return View("VenueAddEdit", venueModel);
            }
            else
            {
                return View("VenueAddEdit");
            }
        }
        #endregion


        #region Category Save
        public IActionResult VenueSave(VenueModel venueModel)
        {
            if (ModelState.IsValid)
            {
                if (venueDALBase.VenueSave(venueModel))
                {
                    TempData["Message"] = "Data Inserted Successfully";
                    return RedirectToAction("VenueList");
                }
            }
            return View("VenueAddEdit");
        }
        #endregion


        #region Category Delete
        public IActionResult VenueDelete(int VenueID)
        {
            bool isSuccess = venueDALBase.PR_Venue_Delete(VenueID);
            if (isSuccess)
            {
                return RedirectToAction("VenueList");
            }
            return RedirectToAction("VenueList");
        }
        #endregion

        #region Venue Filter

        public IActionResult VenueFilter(VenueModel model)
        {
            string connectionstr = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection sqlConnection = new SqlConnection(connectionstr);
            sqlConnection.Open();

            SqlCommand ObjCmd = sqlConnection.CreateCommand();

            ObjCmd.CommandType = CommandType.StoredProcedure;
            ObjCmd.CommandText = "PR_Venue_Filter";

            if (model.VenueName == null)
            {
                ObjCmd.Parameters.AddWithValue("@VenueName", DBNull.Value);
            }
            else
            {
                ObjCmd.Parameters.AddWithValue("@VenueName", model.VenueName);
            }

            if (model.Capacity == null)
            {
                ObjCmd.Parameters.AddWithValue("@Capacity", DBNull.Value);
            }
            else
            {
                ObjCmd.Parameters.AddWithValue("@Capacity", model.Capacity);
            }

            if (model.Location == null)
            {
                ObjCmd.Parameters.AddWithValue("@Location", DBNull.Value);
            }
            else
            {
                ObjCmd.Parameters.AddWithValue("@Location", model.Location);
            }


            SqlDataReader sqlDataReader = ObjCmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(sqlDataReader);

            return View("VenueList", dt);
        }

        #endregion
    }
}
