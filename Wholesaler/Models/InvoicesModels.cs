using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wholesaler.Models.DB;

namespace Wholesaler.Models
{
    public class InvoiceIndexModel
    {
        public List<Invoice> Invoices;
    }
    public class InvoiceDetailsModel
    {
        public Invoice Invoice { get; set; }
        public List<Company> CompaniesList { get; set; }
        public Document Document { get; set; }
        public List<Document> DocumentsList { get; set; }
        public Order Order { get; set; }
        public List<Payment> PaymentsList { get; set; }
        public List<Layout> LayoutsList { get; set; }
    }
}