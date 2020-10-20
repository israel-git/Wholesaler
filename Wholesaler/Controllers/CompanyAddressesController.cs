using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wholesaler.Models;
using Wholesaler.Models.DB;

namespace Wholesaler.Controllers
{
    public class CompanyAddressesController : SuperController
    {
        public ActionResult Addresses(int? id)
        {
            //var model = new CompanyAddressIndexModel
            //{
            //    CompanyAddresses = DB.CompanyAddresses.OrderBy(x => x.AddressName).ToList()
            //};
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

        public ActionResult Details(int? id)
        {
            CompanyAddress companyAddress;
            if (id != null)
                companyAddress = DB.CompanyAddresses.Single(x => x.ID == id);
            else
                companyAddress = new CompanyAddress();
            var model = new CompanyAddressDetailsModel { CompanyAddress = companyAddress };
            return View(model);
        }

        [HttpPost]
        public ActionResult Details(int? id, CompanyAddressDetailsModel model)
        {
            if (ModelState.IsValid)
            {
                if (id != null)
                {
                    DB.CompanyAddresses.Attach(model.CompanyAddress);
                    DB.Entry(model.CompanyAddress).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    DB.CompanyAddresses.Add(model.CompanyAddress);
                }
                DB.SaveChanges();
                var companyID = model.CompanyAddress.CompanyID;
                return RedirectToAction("Addresses/" + companyID);
            }
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var companyAddress = DB.CompanyAddresses.Single(x => x.ID == id);
            DB.CompanyAddresses.Remove(companyAddress);
            DB.SaveChanges();
            var companyID = companyAddress.CompanyID;
            return RedirectToAction("Addresses/" + companyID);
        }

        public ActionResult AddAddress(int? id)
        {
            if (id != null)
            {
                ViewBag.compID = id;
                var model = new CompanyAddressDetailsModel();
                model.CompanyAddress = new CompanyAddress();
                return View(model);
            }
            else
                return RedirectToAction("Company", "Companies");
            
        }

        [HttpPost]
        public ActionResult AddAddress(CompanyAddressDetailsModel model)
        {
            if (ModelState.IsValid)
            {
                DB.CompanyAddresses.Add(model.CompanyAddress);
                DB.SaveChanges();
                return RedirectToAction("Addresses/" + model.CompanyAddress.CompanyID);
            }
            return View(model);
        }
    }
}