using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Wholesaler.Models.DB
{
    public class OrderedProduct
    {
        public int ID { get; set; }
        [Required]
        public int ProductID { get; set; }
        public string Description { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }

        public int OrderID { get; set; }
        [ForeignKey("OrderID")]
        public virtual Order Order { get; set; }
        public virtual Product OrderedProductForID { get; set; }
    }
}