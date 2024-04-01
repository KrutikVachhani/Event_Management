using Event_Management.DAL.SEC_Admin;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Event_Management.Areas.SEC_Client.Controllers
{
    [Area("SEC_Client")]
    [Route("SEC_Client/[controller]/[action]")]
    public class SEC_ClientController : Controller
    {
        public IActionResult SEC_ClientView()
        {
            SEC_AdminDALBase sEC_AdminDALBase = new SEC_AdminDALBase();
            DataTable dataTable = sEC_AdminDALBase.PR_Overview_Counter();
            return View(dataTable);
        }
    }
}
