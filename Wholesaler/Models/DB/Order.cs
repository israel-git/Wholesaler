using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Wholesaler.Models.DB
{
    public class Order
    {
        public Order()
        {
            Proposals = new HashSet<Proposal>();
            Invoices = new HashSet<Invoice>();
            PurchaseOrders = new HashSet<PurchaseOrder>();
            OrderedProducts = new HashSet<OrderedProduct>();
            Expenses = new HashSet<Expense>();
            Payments = new HashSet<Payment>();
        }

        public int ID { get; set; }
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public int OrderStatusID { get; set; }
        [ForeignKey("OrderStatusID")]
        public virtual OrderStatus OrderStatuses { get; set; }

        public decimal? Shipping { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public int? FromCompanyID { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public int? ToCompanyID { get; set; }
      
        public int CompanyID { get; set; }
        [ForeignKey("CompanyID")]
        public virtual Company Company { get; set; }
        public virtual Company FromCompany { get; set; }
        public virtual Company ToCompany { get; set; }

        public virtual ICollection<Proposal> Proposals { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }
        public virtual ICollection<OrderedProduct> OrderedProducts { get; set; } 
        public virtual ICollection<Expense> Expenses { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
            
    }
}