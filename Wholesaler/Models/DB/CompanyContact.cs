using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Wholesaler.Models.DB
{
    public class CompanyContact
    {
        public int ID { get; set; }
        [Required]
        public string ContactName { get; set; }
        public string PhoneNum { get; set; }
        public string Email { get; set; }
        public int Position { get; set; }
        public string InternalComments { get; set; }

        public int CompanyID { get; set; }
        [ForeignKey("CompanyID")]
        public virtual Company Company { get; set; }
    }
}