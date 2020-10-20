using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wholesaler.Models.DB;

namespace Wholesaler.Models
{
    public class CompanyIndexModel
    {
        public List<Company> Companies;
    }
    public class CompanyDetailsModel
    {
        public Company Company { get; set; }
        public List<Document> DocumentsList { get; set; }
        public Document Document { get; set; }
        public List<Order> OrdersList { get; set; }
        public List<OrderedProduct> OrderedProductsList { get; set; }
        public List<Expense> ExpensesList { get; set; }
        public List<Payment> PaymentsList { get; set; }
    }
}