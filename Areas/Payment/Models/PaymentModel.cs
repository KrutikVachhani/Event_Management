﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Event_Management.Areas.Payment.Models
{
    public class PaymentModel
    {
        public int? CardDetailID { get; set; }

        public string CardNumber { get; set; }

        public string CardHolderName { get; set; }

        public DateTime CardExpiryDate { get; set; }

        public int CardCVVNumber { get; set; }

        public int UserID { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public int PriceID { get; set; }

        public int Price { get; set; }
        [NotMapped]
        public string TransactionId { get; set; }
        [NotMapped]
        public string OrderId { get; set; }

        public int? PaymentID { get; set; }

        public DateOnly? PaymentDate { get; set; }
    }
}
