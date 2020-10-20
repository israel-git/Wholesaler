using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Wholesaler.Models.DB
{
    public class Expense
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public decimal? Amount { get; set; }
        public bool Internal { get; set; }

        public int OrderID { get; set; }
        [ForeignKey("OrderID")]
        public virtual Order Order { get; set; }
    }
}