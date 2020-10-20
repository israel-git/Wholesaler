using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wholesaler.Models
{
        public class LoginIndexModel
        {
            [Required]
            public string Email { get; set; }
            [Required]
            public string Password { get; set; }
            public bool RememberMe { get; set; }
        }
        public class LoginForgotModel
        {
            [Required, DataType(DataType.EmailAddress)]
            public string Email { get; set; }
        }
        public class LoginResetModel
        {
            [Required, DataType(DataType.EmailAddress)]
            public string Email { get; set; }
            [Required]
            public string Password { get; set; }
            [Compare("Password")]
            public string PasswordConfirm { get; set; }
        }
}