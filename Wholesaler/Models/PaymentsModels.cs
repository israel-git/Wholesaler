using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wholesaler.Models.DB;

namespace Wholesaler.Models
{
    public class PaymentDetailsModel
    {
        public Payment Payment { get; set; }
        public Document Document { get; set; }
        public List<Document> DocumentsList { get; set; }
        public Order Order { get; set; }
        public List<Payment> PaymentsList { get; set; }
    }
}