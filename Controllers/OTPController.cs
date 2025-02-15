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
            return View("OTP");
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

        //#region SendOTPBySMS

        //private static Dictionary<string, (string otp, DateTime expiry)> otpStorage = new Dictionary<string, (string, DateTime)>();
        //private SmsService _smsService = new SmsService();

        //[HttpPost]
        //public async Task<IActionResult> SendOtp(string phoneNumber)
        //{
        //    if (string.IsNullOrEmpty(phoneNumber))
        //    {
        //        ViewBag.Message = "Phone number is required.";
        //        return View("OTP"); ;
        //    }

        //    ViewBag.phonenumber = phoneNumber;

        //    string otp = new Random().Next(100000, 999999).ToString(); // Generate 6-digit OTP
        //    //otpStorage[phoneNumber] = otp;
        //    await _smsService.SendSms(phoneNumber, otp);

        //    ViewBag.Message = "OTP sent successfully!";
        //    return View("OTP");
        //}


        //[HttpPost]
        //public async Task<IActionResult> VerifyOTP(string phoneNumber, string otp)
        //{
        //    if (string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(otp))
        //    {
        //        ViewBag.Message = "Phone number and OTP are required.";
        //        return View("OTP");
        //    }

        //    if (otpStorage.ContainsKey(phoneNumber))
        //    {
        //        otpStorage.Remove(phoneNumber); // Remove OTP after verification
        //        ViewBag.Message = "OTP verified successfully!";
        //    }
        //    else
        //    {
        //        ViewBag.Message = "Invalid OTP. Please try again.";
        //    }

        //    return View("OTP");
        //}
        //#endregion
    }
}
