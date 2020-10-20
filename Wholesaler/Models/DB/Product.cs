using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wholesaler.Models.DB
{
    public class Product
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public string SKU { get; set; }
        public string UPC { get; set; }
        public string EAN { get; set; }
        public string DisplayName { get; set; }
        public decimal? Price { get; set; }
        public decimal? Cost { get; set; }
        public int? CaseQuantity { get; set; }

    }
}