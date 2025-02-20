using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Event_Management.Areas.SEC_User.Models;
using System.Configuration;
using Event_Management.DAL.SCE_User;
using Event_Management.BAL;
using BCrypt.Net;

namespace Event_Management.Areas.SEC_User.Controllers
{
    [Area("SEC_User")]
    [Route("SEC_User/[controller]/[action]")]
    public class SEC_UserController : Controller
    {
        #region Configuration

        public IConfiguration Configuration;
        public SEC_UserController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #endregion

        #region User Login Page

        public IActionResult SEC_UserLogin()
        {
            return View();
        }
        #endregion

        #region User Register Page
        public IActionResult SEC_UserRegister()
        {
            return View();
        }
        #endregion

        #region Login
        public IActionResult Login(SEC_UserModel sEC_UserModel)
        {
            string error = null;

            if (sEC_UserModel.EmailAddress == null)
            {
                error += "User Name is required";
            }
            if (sEC_UserModel.Password == null)
            {
                error += "<br/>Password is required";
            }

            if (error != null)
            {
                TempData["Error"] = error;
                return RedirectToAction("SEC_UserLogin");
            }
            else
            {
                SEC_UserDAL sEC_UserDAL = new SEC_UserDAL();
                DataTable dt = sEC_UserDAL.dbo_PR_User_SelectByUserNamePassword(sEC_UserModel.EmailAddress, sEC_UserModel.Password);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Console.WriteLine(dr);
                        HttpContext.Session.SetString("UserID", dr["UserID"].ToString());
                        HttpContext.Session.SetString("UserName", dr["UserName"].ToString());
                        HttpContext.Session.SetString("Password", dr["Password"].ToString());
                        HttpContext.Session.SetString("IsAdmin", dr["IsAdmin"].ToString());
                        HttpContext.Session.SetString("IsClient", dr["IsClient"].ToString());
                        HttpContext.Session.SetString("FirstName", dr["FirstName"].ToString());
                        HttpContext.Session.SetString("LastName", dr["LastName"].ToString());
                        HttpContext.Session.SetString("PhotoPath", dr["PhotoPath"].ToString());
                        HttpContext.Session.SetString("EmailAddress", dr["EmailAddress"].ToString());
                        break;
                    }
                }
                else
                {
                    TempData["Error"] = "User Name or Password is invalid!";
                    return RedirectToAction("SEC_UserLogin");
                }
                if (HttpContext.Session.GetString("UserName") != null && HttpContext.Session.GetString("Password") != null && HttpContext.Session.GetString("IsAdmin") == "True")
                {
                    return RedirectToAction("SEC_AdminView", "SEC_Admin", new { area = "SEC_Admin" });
                }
                if (HttpContext.Session.GetString("UserName") != null && HttpContext.Session.GetString("Password") != null && HttpContext.Session.GetString("IsClient") == "True")
                {
                    return RedirectToAction("SEC_ClientView", "SEC_Client", new { area = "SEC_Client" });
                }
                else if (HttpContext.Session.GetString("UserName") != null && HttpContext.Session.GetString("Password") != null)
                {
                    //return RedirectToAction("OTP", "OTP");
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("OTP", "OTPVerification");
        }
        #endregion

        #region Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("SEC_UserLogin");
        }
        #endregion

        #region Register
        public IActionResult Register(SEC_UserModel sEC_UserModel)
        {
            SEC_UserDAL sEC_UserDAL = new SEC_UserDAL();
            sEC_UserModel.Password = BCrypt.Net.BCrypt.HashPassword(sEC_UserModel.Password);

            bool IsSuccess = sEC_UserDAL.dbo_PR_SEC_User_Register(sEC_UserModel.UserName, sEC_UserModel.Password, sEC_UserModel.FirstName, sEC_UserModel.LastName, sEC_UserModel.PhotoPath, sEC_UserModel.EmailAddress);
            if (IsSuccess)
            {
                return RedirectToAction("SEC_UserLogin");
            }
            else
            {
                ViewBag.Msg = "Email Already Exist!";
                return RedirectToAction("SEC_UserRegister");
            }
        }
        #endregion
    }
}
