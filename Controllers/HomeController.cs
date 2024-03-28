using Microsoft.AspNetCore.Mvc;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using System.Data;
using Event_Management.BAL;

namespace Event_Management.Controllers
{
    public class HomeController : Controller
    {
        public static string connectionstr = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("myConnectionString");

        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            try
            {
                SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
                DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Price_SelectAll");
                DataTable dataTable = new DataTable();

                using (IDataReader dataReader = sqlDatabase.ExecuteReader(dbCommand))
                {
                    dataTable.Load(dataReader);
                }
                return View("Index", dataTable);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IActionResult BuyTicket()
        {
            try
            {
                SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
                DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Price_SelectAll");
                DataTable dataTable = new DataTable();

                using (IDataReader dataReader = sqlDatabase.ExecuteReader(dbCommand))
                {
                    dataTable.Load(dataReader);
                }
                return View("BuyTicket", dataTable);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IActionResult Speakers()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult EventDetails()
        {
            return View();
        }

        public IActionResult EventByID()
        {
            return View();
        }

        public IActionResult FeedbackSave()
        {
            return View();
        }
    }
}