using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Wholesaler.Models.DB
{
    public class WholesaleContext : DbContext
    {
        public WholesaleContext() : base("name=WholesaleContext")
        {
            Database.SetInitializer<WholesaleContext>(null);
        }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyAddress> CompanyAddresses { get; set; }
        public DbSet<CompanyContact> CompanyContacts { get; set; }
        public DbSet<CompanyPosition> CompanyPositions { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Proposal> Proposals { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<OrderedProduct> OrderedProducts { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Layout> Layouts { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
    }
}