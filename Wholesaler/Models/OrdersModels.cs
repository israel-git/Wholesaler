using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wholesaler.Models.DB;

namespace Wholesaler.Models
{
    public class OrdersIndexModel
    {
        public List<Order> Orders;
    }
    public class OrderDetailsModel
    {
        public Order Order { get; set; }
        public List<Company> CompaniesList { get; set; }
        public List<Document> DocumentsList { get; set; }
        public OrderedProduct OrderedProduct { get; set; }
       // public List<OrderedProduct> OrderedProductsList { get; set; }
        public List<Product> ProductsList { get; set; }
        //public List<Expense> ExpensesList { get; set; }
        public Expense Expense { get; set; }
        public List<Payment> PaymentsList { get; set; }
        public List<OrderStatus> OrderStatusesList { get; set; }
    }
    public class OrderedProductDetailsModel
        {
            public OrderedProduct OrderedProduct { get; set; }
            public Order Order { get; set; }
    }
    public class ExpenseDetailsModel
    {
        public Expense Expense { get; set; }
        public Order Order { get; set; }
    }
    public class ProposalPOInvoice
    {
        public string Category { get; set; }
        public int ID { get; set; }
        public int OrderID { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
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
        public decimal? Shipping { get; set; }
        public decimal? TaxRate { get; set; }
        public string FooterNotes { get; set; }
        public string Terms { get; set; }
    }
}