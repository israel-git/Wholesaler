using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wholesaler.Models;
using Wholesaler.Models.DB;

namespace Wholesaler.Controllers
{
    public class CompanyContactsController : SuperController
    {
        public ActionResult ContactsList(int? id)
        {
            if (id != null)
            {
                var model = new CompanyDetailsModel
                {
                    Company = DB.Companies.Where(x => x.ID == id).FirstOrDefault()
                };
                return View(model);
            }
            else
                return RedirectToAction("Company", "Companies");
        }

        public ActionResult ContactDetails(int? id)
        {
            CompanyContact companyContact;
            if (id != null)
                companyContact = DB.CompanyContacts.Single(x => x.ID == id);
            else
                companyContact = new CompanyContact();
            var model = new CompanyContactsDetailsModel { CompanyContact = companyContact };
            return View(model);
        }

        [HttpPost]
        public ActionResult ContactDetails(int? id, CompanyContactsDetailsModel model)
        {
            if (ModelState.IsValid)
            {
                if (id != null)
                {
                    DB.CompanyContacts.Attach(model.CompanyContact);
                    DB.Entry(model.CompanyContact).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    DB.CompanyContacts.Add(model.CompanyContact);
                }
                DB.SaveChanges();
                var companyID = model.CompanyContact.CompanyID;
                return RedirectToAction("ContactsList/" + companyID);
            }
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var companyContact = DB.CompanyContacts.Single(x => x.ID == id);
            DB.CompanyContacts.Remove(companyContact);
            DB.SaveChanges();
            var companyID = companyContact.CompanyID;
            return RedirectToAction("ContactsList/" + companyID);
        }

        public ActionResult AddContact(int? id)
        {
            if (id != null)
            {
                ViewBag.compID = id;
                var model = new CompanyContactsDetailsModel();
                model.CompanyContact = new CompanyContact();
                return View(model);
            }
            else
                return RedirectToAction("ContactsList", "CompanyContacts");

        }

        [HttpPost]
        public ActionResult AddContact(CompanyContactsDetailsModel model)
        {
            if (ModelState.IsValid)
            {
                DB.CompanyContacts.Add(model.CompanyContact);
                DB.SaveChanges();
                return RedirectToAction("ContactsList/" + model.CompanyContact.CompanyID);
            }
            return View(model);
        }
    }
}