using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Wholesaler.Models.DB
{
    public class Payment
    {
        public int ID { get; set; }
        
        public DateTime Date { get; set; }
        [Required]
        public decimal? Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionID { get; set; }
        public string ReferenceNumber { get; set; }
        public bool? Cleared { get; set; }
        public string Notes { get; set; }

        public int OrderID { get; set; }
        [ForeignKey("OrderID")]
        public virtual Order Order { get; set; }
    }
}