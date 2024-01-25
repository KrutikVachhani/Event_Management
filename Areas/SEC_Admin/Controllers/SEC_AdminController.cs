using Microsoft.AspNetCore.Mvc;

namespace Event_Management.Areas.SEC_Admin.Controllers
{
    [Area("SEC_Admin")]
    [Route("SEC_Admin/[controller]/[action]")]
    public class SEC_AdminController : Controller
    {
        public IActionResult SEC_AdminView()
        {
            return View();
        }
    }
}
