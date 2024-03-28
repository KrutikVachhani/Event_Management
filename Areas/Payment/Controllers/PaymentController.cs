using Event_Management.Areas.Payment.Models;
using Event_Management.Areas.Venue.Models;
using Event_Management.DAL.Payment;
using Event_Management.DAL.Venue;
using Microsoft.AspNetCore.Mvc;
using Event_Management.BAL;

namespace Event_Management.Areas.Payment.Controllers
{
    [CheckAccess]
    [Area("Payment")]
    [Route("Payment/[Controller]/[Action]")]
    public class PaymentController : Controller
    {
        public IConfiguration Configuration;
        public PaymentController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        PaymentDALBase paymentDALBase = new PaymentDALBase();

        #region Payment Successfull Screen
        public IActionResult PaymentSuccesfull()
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

    }
}
