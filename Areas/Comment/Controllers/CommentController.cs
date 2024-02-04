using Microsoft.AspNetCore.Mvc;
using Event_Management.Areas.Comment.Models;
using Event_Management.Areas.Users.Models;
using Event_Management.DAL.Comment;
using System.Data;

namespace Event_Management.Areas.Comment.Controllers
{
    [Area("Comment")]
    [Route("Comment/[Controller]/[Action]")]
    public class CommentController : Controller
    {
        public IConfiguration Configuration;
        public CommentController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        CommentDALBase commentDALBase = new CommentDALBase();

        #region User List
        public IActionResult CommentView()
        {
            DataTable dataTable = commentDALBase.PR_Comment_SelectAll();
            return View(dataTable);
        }

        public IActionResult CommentList()
        {
            DataTable dataTable = commentDALBase.PR_Comment_SelectAll();
            return View(dataTable);
        }
        #endregion

        #region User By ID
        public IActionResult CommentAdd(int CommentID)
        {
            CommentModel commentModel = commentDALBase.PR_Comment_SelectByID(CommentID);
            if (commentModel != null)
            {
                return View("CommentAddEdit", commentModel);
            }
            else
            {
                return View("CommentAddEdit");
            }
        }
        #endregion


        #region Event Save
        public IActionResult CommentSave(CommentModel commentModel)
        {
            if (ModelState.IsValid)
            {
                if (commentDALBase.CommentSave(commentModel))

                    return RedirectToAction("CommentView");
                else
                {
                    return RedirectToAction("CommentView");
                }
            }
            return View("CommentView");
        }
        #endregion


        #region Event Delete
        public IActionResult CommentDelete(int CommentID)
        {
            bool isSuccess = commentDALBase.PR_Comment_Delete(CommentID);
            if (isSuccess)
            {
                return RedirectToAction("CommentView");
            }
            return RedirectToAction("CommentView");
        }
        #endregion
    }
}
