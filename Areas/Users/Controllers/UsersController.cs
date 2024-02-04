using Event_Management.Areas.Users.Models;
using Event_Management.DAL.Event;
using Event_Management.DAL.Users;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Event_Management.Areas.Users.Controllers
{
    [Area("Users")]
    [Route("Users/[Controller]/[Action]")]
    public class UsersController : Controller
    {
        public IConfiguration Configuration;
        public UsersController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        UserDALBase userDALBase = new UserDALBase();

        #region User List
        public IActionResult UserList()
        {
            DataTable dataTable = userDALBase.PR_Users_SelectAll();
            return View(dataTable);
        }
        #endregion

        #region User By ID
        public IActionResult UserAdd(int UserID)
        {
            UserModel userModel = userDALBase.PR_Users_SelectByID(UserID);
            if (userModel != null)
            {
                return View("UserAddEdit", userModel);
            }
            else
            {
                return View("UserAddEdit");
            }
        }
        #endregion


        #region Event Save
        public IActionResult UsersSave(UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                if (userDALBase.UsersSave(userModel))

                    return RedirectToAction("UserList");

            }
            return View("UserAddEdit" +
                "");
        }
        #endregion


        #region Event Delete
        public IActionResult UserDelete(int UserID)
        {
            bool isSuccess = userDALBase.PR_Users_Delete(UserID);
            if (isSuccess)
            {
                return RedirectToAction("UserList");
            }
            return RedirectToAction("UserList");
        }
        #endregion
    }
}
