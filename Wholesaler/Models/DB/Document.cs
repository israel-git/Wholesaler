using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Wholesaler.Models.DB
{
    public class Document
    {
        public int ID { get; set; }
        public string DocumentName { get; set; }
        public string GUID { get; set; }
        public CategoryEnum Category { get; set; } 

        public int ForeignID { get; set; }
        [ForeignKey("ForeignID")]
        public virtual PurchaseOrder PurchaseOrder { get; set; }

        public enum CategoryEnum
        {
            Company = 1,
            Proposal = 2,
            PurchaseOrder = 3,
            Invoice = 4,
            Payment = 5
        }
    }
}