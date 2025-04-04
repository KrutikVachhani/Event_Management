using Event_Management.Areas.Payment.Models;
using Event_Management.Areas.Venue.Models;
using Event_Management.DAL.Payment;
using Event_Management.DAL.Venue;
using Microsoft.AspNetCore.Mvc;
using Event_Management.BAL;
using System.Data;
using Event_Management.Areas.Payment.Models;
using System.Data.SqlClient;

using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Event_Management.Areas.Users.Models;
using Event_Management.DAL.Users;
//using DocumentFormat.OpenXml.Drawing.Charts;
//using System.Web.Mvc;

namespace Event_Management.Areas.Payment.Controllers
{
    [CheckAccess]
    [Area("Payment")]
    [Route("Payment/[Controller]/[Action]")]
    public class PaymentController : Controller
    {
        [BindProperty]
        public PaymentModel _PaymentDetails {  get; set; }

        public IConfiguration Configuration;
        public PaymentController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        PaymentDALBase paymentDALBase = new PaymentDALBase();

        #region Payment List
        public IActionResult PaymentList()
        {
            DataTable dataTable = paymentDALBase.PR_Payment_SelectAll();
            return View(dataTable);
        }
        #endregion

        #region Payment Successfull Screen
        public IActionResult Demo()
        {
            return View();
        }
        #endregion

        #region Select By ID
        public IActionResult PaymentAdd(int PriceID)
        {
            PaymentModel paymentModel = paymentDALBase.PR_Payment_SelectByID(PriceID);
            if (paymentModel != null)
            {
                return View("PaymentPage", paymentModel);
            }
            else
            {
                return View("PaymentSuccesfull");
            }
        }
        #endregion

        #region Payment Save
        public IActionResult PaymentSave(PaymentModel paymentModel)
        {
            if (ModelState.IsValid)
            {
                if (paymentDALBase.PaymentSave(paymentModel))

                    return RedirectToAction("PaymentSuccesfull");

            }
            return View("PaymentAdd");
        }
        #endregion

        #region Initiate Payment
        public IActionResult InitiatePayment(int PriceID, int Price, int UserID)
        {
            Random _random = new Random();
            string TransactionId = _random.Next(0, 10000).ToString();

            PaymentModel model = new PaymentModel();
            model.UserID = UserID;
            model.PriceID = PriceID;


            if (paymentDALBase.PaymentInsert(model.PriceID))
            {
                Console.WriteLine("Insert Successful");
            }
            else
            {
                Console.WriteLine("Insert Unsuccessful");
            }

             //decimal amount = Convert.ToDecimal(_PaymentDetails.Price) * 100;
            Dictionary<string, object> input = new Dictionary<string, object>();
            input.Add("amount", Price*100); // Amount is in currency subunits. Default currency is INR. Hence, 50000 refers to 50000 paise
            input.Add("currency", "INR");
            input.Add("receipt", TransactionId);

            string key = "rzp_test_VosEaeQ7hvpQAB";
            string secret = "TBVVerdhaMAsCTbSP9hsDgcQ";

            RazorpayClient client = new RazorpayClient(key, secret);

            Razorpay.Api.Order order = client.Order.Create(input);
            ViewBag.orderId = order["id"].ToString();

            return View("Payment");
        }

        #endregion

        #region Payment Successful
        [HttpPost]
        public IActionResult PaymentSuccesfull(string razorpay_payment_id, string razorpay_order_id, string razorpay_signature, int UserID)
        {
            Dictionary<string, string> attributes = new Dictionary<string, string>();
            attributes.Add("razorpay_payment_id", razorpay_payment_id);
            attributes.Add("razorpay_order_id", razorpay_order_id);
            attributes.Add("razorpay_signature", razorpay_signature);

            //Utils.verifyPaymentSignature(attributes);

            PaymentModel model = new PaymentModel();
            model.TransactionId = razorpay_payment_id;
            model.OrderId = razorpay_order_id;

            if(paymentDALBase.PaymentUpdate(model))
            {
                return View("PaymentSuccesfull", model);
            }
            else
            {
                return View("PaymentSuccesfull");
            }
        }

        #endregion

        #region Payment Filter
        public IActionResult PaymentFilter(PaymentModel model)
        {
            DataTable dataTable = paymentDALBase.PaymentFilter(model);
            return View("PaymentList", dataTable);
        }

        #endregion



    }
}
