using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wholesaler.Models.DB;
using Wholesaler.Models;
using System.Web.Security;

namespace Wholesaler.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            if (Session["permissionLevel"] == null)
            {
                Session["permissionLevel"] = 2;
            }
            var model = new UsersDetailsModel { User = new Models.DB.User()};
            return View(model);   
        }

    }
}