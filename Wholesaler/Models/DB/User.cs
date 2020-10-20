using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Wholesaler.Models.DB
{
    public class User
    {
        public int ID { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Password { get; set; }
        [NotMapped]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string ResetCode { get; set;}
        public DateTime? ResetCodeExpiry { get; set; }
  
        public PermissionLevelEnum PermissionLevel { get; set; }

        public enum PermissionLevelEnum
        {
            Admin = 1,
            Secondary = 2
        }
    }
}