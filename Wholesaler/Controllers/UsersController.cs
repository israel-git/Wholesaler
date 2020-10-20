using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wholesaler.Models;
using Wholesaler.Models.DB;

namespace Wholesaler.Controllers
{
    public class UsersController : SuperController
    {
        // GET: Users
        public ActionResult Index()
        {
            if (Session["permissionLevel"].ToString() == "Admin")
            {
                var model = new UsersIndexModel();
                model.UsersList = DB.Users.ToList();
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult AddUser()
        {
            if (Session["permissionLevel"].ToString() == "Admin")
            {
                var model = new UsersDetailsModel();
                model.User = new User();
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult AddUser(UsersDetailsModel model)
        {
            if (ModelState.IsValid)
            {
                DB.Users.Add(model.User);
                DB.SaveChanges();
                model.User.Password = Cryptography.Hash(model.User.Password, model.User.ID);
                model.User.ConfirmPassword = model.User.Password;
                DB.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }
        public ActionResult UserDetails(int? id)
        {
            if (Session["permissionLevel"].ToString() == "Admin")
            {
                User user;
                if (id != null)
                {
                    user = DB.Users.Single(x => x.ID == id);
                }
                else
                {
                    user = new User();
                    return RedirectToAction("Index");
                }
                var model = new UsersDetailsModel { User = user };
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpPost]
        public ActionResult UserDetails(int? id, UsersDetailsModel model)
        {
            if (ModelState.IsValid)
            {
                if (id != null)
                {
                    model.User.Password = Cryptography.Hash(model.User.Password, model.User.ID);
                    model.User.ConfirmPassword = model.User.Password;
                    DB.Users.Attach(model.User);
                    DB.Entry(model.User).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    DB.Users.Add(model.User);
                }
                DB.SaveChanges();
                return RedirectToAction("Index");
            }
              return View(model);
        }

        public ActionResult Delete(int id)
        {
            if (Session["permissionLevel"].ToString() == "Admin")
            {
                var user = DB.Users.Single(x => x.ID == id);
                DB.Users.Remove(user);
                DB.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}