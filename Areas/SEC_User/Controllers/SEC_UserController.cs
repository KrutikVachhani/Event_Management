using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Event_Management.Areas.SEC_User.Models;
using System.Configuration;
using Event_Management.DAL.SCE_User;
using Event_Management.BAL;

namespace Event_Management.Areas.SEC_User.Controllers
{
    [Area("SEC_User")]
    [Route("SEC_User/[controller]/[action]")]
    public class SEC_UserController : Controller
    {
        public IConfiguration Configuration;
        public SEC_UserController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IActionResult SEC_UserLogin()
        {
            return View();
        }

        public IActionResult SEC_UserRegister()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(SEC_UserModel sEC_UserModel)
        {
            string error = null;

            if (sEC_UserModel.UserName == null)
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
                DataTable dt = sEC_UserDAL.dbo_PR_User_SelectByUserNamePassword(sEC_UserModel.UserName, sEC_UserModel.Password);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Console.WriteLine(dr);
                        HttpContext.Session.SetString("UserName", dr["UserName"].ToString());
                        HttpContext.Session.SetString("UserID", dr["UserID"].ToString());
                        HttpContext.Session.SetString("Password", dr["Password"].ToString());
                        break;
                    }
                }
                else
                {
                    TempData["Error"] = "User Name or Password is invalid!";
                    return RedirectToAction("SEC_UserLogin");
                }
                if (HttpContext.Session.GetString("UserName") != null && HttpContext.Session.GetString("Password") != null && HttpContext.Session.GetString("UserName") == "Krutik")
                {
                    return RedirectToAction("SEC_AdminView", "SEC_Admin", new { area = "SEC_Admin" });
                }
                else if (HttpContext.Session.GetString("UserName") != null && HttpContext.Session.GetString("Password") != null)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Index");
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("SEC_UserLogin");
        }

        public IActionResult Register(SEC_UserModel sEC_UserModel)
        {
            SEC_UserDAL sEC_UserDAL = new SEC_UserDAL();
            bool IsSuccess = sEC_UserDAL.dbo_PR_SEC_User_Register(sEC_UserModel.UserName, sEC_UserModel.Password, sEC_UserModel.FirstName, sEC_UserModel.LastName, sEC_UserModel.PhotoPath, sEC_UserModel.EmailAddress);
            if (IsSuccess)
            {
                return RedirectToAction("SEC_UserLogin");
            }
            else
            {
                return RedirectToAction("SEC_UserRegister");
            }
        }
    }
}
