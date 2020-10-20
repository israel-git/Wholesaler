using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wholesaler.Models.DB;

namespace Wholesaler.Models
{
    public class CompanyAddressIndexModel
    {
        public List<CompanyAddress> CompanyAddresses;
    }

    public class CompanyAddressDetailsModel
    {
        public CompanyAddress CompanyAddress { get; set; }
    }
}