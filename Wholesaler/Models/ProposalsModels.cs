using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wholesaler.Models.DB;

namespace Wholesaler.Models
{
    public class ProposalIndexModel
    {
        public List<Proposal> Proposals { get; set; }
    }
    public class ProposalDetailsModel
    {
        public Proposal Proposal { get; set; }
        public List<Company> CompaniesList { get; set; }
        public List<Document> DocumentsList { get; set; }
        public Document Document { get; set; }
        public Order Order { get; set; }
        public List<Payment> PaymentsList { get; set; }
    }
}