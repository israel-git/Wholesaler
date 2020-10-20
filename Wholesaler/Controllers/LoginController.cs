using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Wholesaler.Models;
using Wholesaler.Models.DB;
//UserName - simchaz123@gmail.com, pw - foo
namespace Wholesaler.Controllers
{
    [AllowAnonymous]
    public class LoginController : SuperController
    {
        //public WholesaleContext DB = new WholesaleContext();
        // GET: Login
        public ActionResult Index()
        {
            var model = new LoginIndexModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(LoginIndexModel model)
        {
            if (ModelState.IsValid)
            {
                var user = DB.Users.SingleOrDefault(x => x.Email == model.Email);
                if (user != null && !string.IsNullOrEmpty(user.Password))
                {
                    var hash = Cryptography.Hash(model.Password, user.ID);
                    if (user.Password == hash)
                    {
                        //set cookie
                        FormsAuthentication.SetAuthCookie(model.Email, model.RememberMe);
                        Session["permissionLevel"] = user.PermissionLevel;
                        Session["name"] = user.FirstName;
                        return RedirectToAction("Index", "Home");

                        //var hash = Cryptography.Hash(model.Password, user.ID);
                        //if (user.Password == hash)
                        //{
                        //    //set cookie
                        //    var guid = Cryptography.RandomGuidStr();
                        //    var cookie = new HttpCookie(Constants.LoginKey, guid);
                        //    if (model.RememberMe)
                        //        cookie.Expires = DateTime.UtcNow.AddDays(14);
                        //    Response.SetCookie(cookie);
                        //    //save in db
                        //    var login = new UserLogin
                        //    {
                        //        UserID = user.ID,
                        //        Guid = guid,
                        //        LoginTime = DateTime.Now
                        //    };
                        //    DB.UserLogins.Add(login);
                        //    DB.SaveChanges();
                        //    return RedirectToAction("Index", "Home");

                    }
                }
                ModelState.AddModelError("Password", "Your credentials were not recognized.");
            }

            return View();
        }
        public ActionResult Forgot()
        {
            var model = new LoginForgotModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Forgot(LoginForgotModel model)
        {
            if (ModelState.IsValid)
            {
                var user = DB.Users.SingleOrDefault(x => x.Email == model.Email);
                if (user == null)
                    ModelState.AddModelError("Email", "Email not found.");
                else
                {
                    //create reset code
                    var code = Cryptography.RandomGuidStr();
                    user.ResetCode = code;
                    user.ResetCodeExpiry = DateTime.Now.AddHours(2);
                    DB.SaveChanges();
                    //send email
                    string msg = string.Format("<a href='http://{0}{1}'>Click here</a> to reset your password. Note that this link will only be valid for 2 hours.", Request.Url.Host, Url.Action("reset", new { code = code }));
                    Email.Send(user.Email, "Password Recovery", msg, true);
                    return RedirectToAction("forgotmessage");
                }
            }
            return View(model);
        }
        public ActionResult ForgotMessage()
        {
            return View();
        }

        public ActionResult Reset(string code)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Reset(string code, LoginResetModel model)
        {
            if (ModelState.IsValid)
            {
                var user = DB.Users.SingleOrDefault(x => x.Email == model.Email);
                if (user == null || user.ResetCode != code || user.ResetCodeExpiry < DateTime.Now)
                    return RedirectToAction("resetinvalidmessage");
                //reset password
                var hash = Cryptography.Hash(model.Password, user.ID);
                user.Password = hash;
                user.ResetCode = null;
                user.ResetCodeExpiry = null;
                DB.SaveChanges();
                //login
                return RedirectToAction("");
            }
            return View(model);
        }

        public ActionResult ResetInvalidMessage()
        {
            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            //var logout = new UserLogin
            //{
            //    LogoutTime = DateTime.Now
            //};
            //DB.UserLogins.Add(logout);
            //DB.SaveChanges();
            return RedirectToAction("Index", "login");
        }
    }
}