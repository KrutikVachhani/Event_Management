using Event_Management.DAL.SEC_Admin;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Event_Management.Areas.SEC_Admin.Controllers
{
    [Area("SEC_Admin")]
    [Route("SEC_Admin/[controller]/[action]")]
    public class SEC_AdminController : Controller
    {
        #region AdminView
        public IActionResult SEC_AdminView()
        {
            SEC_AdminDALBase sEC_AdminDALBase = new SEC_AdminDALBase();
            DataTable dataTable = sEC_AdminDALBase.PR_Overview_Counter();
            return View(dataTable);
        }
        #endregion
    }
}
