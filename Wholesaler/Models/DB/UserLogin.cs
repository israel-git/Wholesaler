using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Wholesaler.Models.DB
{
    public class UserLogin
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        [StringLength(32), Column(TypeName = "CHAR"), Required]
        public string Guid { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }

        public virtual User User { get; set; }
    }
}