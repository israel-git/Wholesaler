using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wholesaler.Models.DB;

namespace Wholesaler.Models
{
    public class CompanyPositionsIndexModel
    {
        public List<CompanyPosition> CompanyPositions;
    }
    public class CompanyPositionsDetailsModel
    {
        public CompanyPosition CompanyPosition { get; set; }
    }
}