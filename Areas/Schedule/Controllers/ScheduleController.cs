using Event_Management.Areas.Event.Models;
using Event_Management.DAL.Schedule;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data;

namespace Event_Management.Areas.Schedule.Controllers
{
    [Area("Schedule")]
    [Route("Schedule/[Controller]/[Action]")]
    public class ScheduleController : Controller
    {
        public IActionResult Schedule()
        {
            ScheduleDALBase scheduleDAL = new ScheduleDALBase();
            DataTable dataTable = scheduleDAL.Schedule();
            return View(dataTable);
        }

        public IActionResult ScheduleSearch(DateTime Date)
        {
            ScheduleDALBase scheduleDAL = new ScheduleDALBase();
            EventModel eventModel = scheduleDAL.Schedule_Search(Date);
            if(eventModel != null)
            {
                return View("Schedule",eventModel);
            }
            else
            {
                return View("Schedule", eventModel);
            }
        }
    }
}
