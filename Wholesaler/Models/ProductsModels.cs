using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wholesaler.Models.DB;

namespace Wholesaler.Models
{
    public class ProductIndexModel
    {
        public List<Product> Products;
    }
    public class ProductDetailsModel
    {
        public Product Product { get; set; }
    }
}