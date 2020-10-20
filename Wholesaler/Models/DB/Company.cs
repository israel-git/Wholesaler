using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Wholesaler.Models.DB
{
    public class Company
    {
        public Company()
        {
            CompanyAddresses = new HashSet<CompanyAddress>();
            CompanyContacts = new HashSet<CompanyContact>();
            CompanyPositions = new HashSet<CompanyPosition>();
            Orders = new HashSet<Order>();
            //Invoices = new HashSet<Invoice>();
        }

        public int ID { get; set; }
        [Required]
        public string CompanyName { get; set; }
        public string PhoneNum { get; set; }
        public string InternalComments { get; set; }
        public DateTime CreateDate { get; set; }
        public string Logo { get; set; }

        public virtual ICollection<CompanyAddress> CompanyAddresses { get; set; }
        public virtual ICollection<CompanyContact> CompanyContacts { get; set; }
        public virtual ICollection<CompanyPosition> CompanyPositions { get; set; }
        [InverseProperty("Company")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}