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
    }
}
