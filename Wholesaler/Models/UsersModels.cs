using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wholesaler.Models.DB;

namespace Wholesaler.Models
{
    public class UsersIndexModel
    {
        public List<User> UsersList; 
    }
    public class UsersDetailsModel
    {
        public User User { get; set; }
    }
}