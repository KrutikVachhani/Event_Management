using Microsoft.AspNetCore.Mvc;
using System;
//using System.Web.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Event_Management.Controllers
{
    public class OTPController : Controller
    {
        private static Dictionary<string, (string otp, DateTime expiry)> otpStorage = new Dictionary<string, (string, DateTime)>();
        private readonly EmailService _emailService;

        public OTPController()
        {
            _emailService = new EmailService(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build());
        }

        public IActionResult OTP()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        #region SendOTPByEmail
        [HttpPost]
        public async Task<ActionResult> SendOTP(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ViewBag.Message = "Email is required.";
                return View("OTP");
            }

            ViewBag.Email = email;

            Random random = new Random();
            string otp = random.Next(100000, 999999).ToString();
            otpStorage[email] = (otp, DateTime.Now.AddMinutes(5));

            string message = $"Your OTP is: {otp}. It is valid for 5 minutes.";
            await _emailService.SendEmailAsync(email, "Your OTP Code", message);

            //return Json(new { success = true, message = "OTP sent successfully!" });
            return View("OTPVerify");
        }
        #endregion

        #region VerifyOTPByEmail
        [HttpPost]
        public ActionResult verifyOTP(string email, string otp)
        {

            if (otpStorage.ContainsKey(email))
            {
                var (storedOtp, expiry) = otpStorage[email];

                if (DateTime.Now > expiry)
                {
                    return Json(new { success = false, message = "OTP expired. Request a new one." });
                }
                if (storedOtp == otp)
                {
                    otpStorage.Remove(email);
                    return View("Index", "Home");
                }
                else
                {
                    return Json(new { success = false, message = "Invalid OTP. Try again." });
                }
            }
            return View("Index", "Home");
        }
        #endregion
    }
}
