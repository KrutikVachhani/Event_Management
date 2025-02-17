using Microsoft.AspNetCore.Mvc;

namespace Event_Management.Areas.OTPVerification.Controllers
{
    public class OTPVerification : Controller
    {
        public IActionResult OTP()
        {
            return View();
        }
    }
}
