﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Wholesaler.Models.DB
{
    public class PurchaseOrder
    {
        public PurchaseOrder()
        {
            Documents = new HashSet<Document>();
        }

        public int ID { get; set; }
        [Required]
        public string PurchaseOrderNumber { get; set; }
        public DateTime? Date { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public int LayoutID { get; set; }
        [ForeignKey("LayoutID")]
        public virtual Layout Layouts { get; set; }

        public string BillToCompany { get; set; }
        public string BillToName { get; set; }
        public string BillToAddress { get; set; }
        public string BillToCity { get; set; }
        public string BillToState { get; set; }
        public string BillToZip { get; set; }
        public string ShipToCompany { get; set; }
        public string ShipToName { get; set; }
        public string ShipToAddress { get; set; }
        public string ShipToCity { get; set; }
        public string ShipToState { get; set; }
        public string ShipToZip { get; set; }
        public bool Taxable { get; set; }
        public decimal? TaxRate { get; set; }
        public string FooterNotes { get; set; }
        public string Terms { get; set; }

        public int OrderID { get; set; }
        [ForeignKey("OrderID")]
        public virtual Order Order { set; get; }
        public virtual ICollection<Document> Documents { get; set; }
    }
}