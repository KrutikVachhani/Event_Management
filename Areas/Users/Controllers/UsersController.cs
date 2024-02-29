using ClosedXML.Excel;
using Event_Management.Areas.Users.Models;
using Event_Management.CF;
using Microsoft.EntityFrameworkCore;
using Event_Management.DAL.Event;
using Event_Management.DAL.Users;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using Event_Management.Areas.Venue.Models;
using Pages = Event_Management.CF.Pages;
using DocumentFormat.OpenXml.Bibliography;

namespace Event_Management.Areas.Users.Controllers
{
    [Area("Users")]
    [Route("Users/[Controller]/[Action]")]
    public class UsersController : Controller
    {
        #region Configuration

        public IConfiguration Configuration;
        public UsersController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #endregion

        #region DAL Object

        UserDALBase userDALBase = new UserDALBase();

        #endregion

        #region Pagination
        //public IActionResult Pages(int page = 1, int pageSize = 10)
        //{
        //    DataTable dataTable = userDALBase.PR_Users_SelectAll();

        //    var rows = dataTable.Rows.Cast<DataRow>().ToList();


        //    // Calculate total number of items and total number of pages
        //    var totalItems = rows.Count;
        //    var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        //    // Ensure the page number is within valid bounds
        //    page = Math.Max(1, Math.Min(page, totalPages));

        //    // Calculate the starting index and number of items to take for the current page
        //    var startIndex = (page - 1) * pageSize;
        //    var itemsToTake = Math.Min(pageSize, totalItems - startIndex);

        //    // Retrieve the subset of rows for the current page
        //    var data = rows.Skip(startIndex).Take(itemsToTake).ToList();

        //    // Create PaginationModel object
        //    var model = new PageModel<DataRow>
        //    {
        //        Data = data,
        //        CurrentPage = page,
        //        TotalPages = totalPages
        //    };

        //    return View(model);
        //}
        #endregion

        #region User List
        public IActionResult UserList()
        {
            DataTable dataTable = userDALBase.PR_Users_SelectAll();

            return View(dataTable);
        }
        #endregion

        #region Add User
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

        #region User Save
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

        #region User Delete
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

        #region User Filter
        public IActionResult UsersFilter(UserModel model)
        {
            DataTable dataTable = userDALBase.UserFilter(model);
            return View("UserList", dataTable);
        }

        #endregion

        #region Export Excel

        public List<UserModel> GetStudentModels()
        {
            List<UserModel> userModels = new List<UserModel>();
            string myconnStr = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection connection = new SqlConnection(myconnStr);
            connection.Open();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_Users_SelectAll";
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    UserModel userModel = new UserModel
                    {
                        UserName = reader["UserName"].ToString(),
                        Password = reader["Password"].ToString(),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        PhotoPath = reader["PhotoPath"].ToString(),
                        EmailAddress = reader["EmailAddress"].ToString(),
                        Created = Convert.ToDateTime(reader["Created"].ToString()),
                        Modified = Convert.ToDateTime(reader["Modified"].ToString())

                        // Add other properties as needed
                    };
                    userModels.Add(userModel);
                }
                return userModels;
            }
        }

        public IActionResult ExportStudentsToExcel()
        {
            List<UserModel> userModels = GetStudentModels();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Users");
                // Add headers
                worksheet.Cell(1, 1).Value = "UserName";
                worksheet.Cell(1, 2).Value = "Password";
                worksheet.Cell(1, 3).Value = "First Name";
                worksheet.Cell(1, 4).Value = "Last Name";
                worksheet.Cell(1, 5).Value = "PhotoPath";
                worksheet.Cell(1, 6).Value = "EmailAddress";
                worksheet.Cell(1, 7).Value = "Created";
                worksheet.Cell(1, 8).Value = "Modified";
                // Add data
                int row = 2;
                foreach (var userModel in userModels)
                {
                    worksheet.Cell(row, 1).Value = userModel.UserName;
                    worksheet.Cell(row, 2).Value = userModel.Password;
                    worksheet.Cell(row, 3).Value = userModel.FirstName;
                    worksheet.Cell(row, 4).Value = userModel.LastName;
                    worksheet.Cell(row, 5).Value = userModel.PhotoPath;
                    worksheet.Cell(row, 6).Value = userModel.EmailAddress;
                    worksheet.Cell(row, 7).Value = userModel.Created.ToString("yyyy-MM-dd");
                    worksheet.Cell(row, 8).Value = userModel.Modified.ToString("yyyy-MM-dd");
                    // Add other properties...
                    row++;
                }
                // Set content type and filename
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                var fileName = "StudentData.xlsx";
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }
        #endregion

        #region User Profile
        public IActionResult UserProfile()
        {
            return View();
        }
        #endregion
    }
}
