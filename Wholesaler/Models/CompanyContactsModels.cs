using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wholesaler.Models.DB;

namespace Wholesaler.Models
{
    public class CompanyContactsIndexModel
    {
        public List<CompanyContact> CompanyContacts;
    }
    public class CompanyContactsDetailsModel
    {
        public CompanyContact CompanyContact { get; set; }
    }
}