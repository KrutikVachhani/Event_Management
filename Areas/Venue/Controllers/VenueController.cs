using Event_Management.Areas.Venue.Models;
using Event_Management.DAL.Venue;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Event_Management.Areas.Venue.Controllers
{
    [Area("Venue")]
    [Route("Venue/[Controller]/[Action]")]
    public class VenueController : Controller
    {
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

                    return RedirectToAction("VenueList");

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


    }
}
