using Microsoft.AspNetCore.Mvc;

namespace Event_Management.Areas.SEC_Client.Controllers
{
    [Area("SEC_Client")]
    [Route("SEC_Client/[controller]/[action]")]
    public class SEC_ClientController : Controller
    {
        public IActionResult SEC_ClientView()
        {
            return View();
        }
    }
}
