using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wholesaler.Models.DB
{
    public class Layout
    {
        public Layout()
        {
            PurchaseOrders = new HashSet<PurchaseOrder>();
        }

        public int LayoutID { get; set; }
        public string LayoutName { get; set; }

        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }
    }
}