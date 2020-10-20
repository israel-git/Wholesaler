using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wholesaler.Models;
using Wholesaler.Models.DB;

namespace Wholesaler.Controllers
{
    public class CompanyPositionsController : SuperController
    {
        public ActionResult PositionsList(int? id)
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
        public ActionResult PositionDetails(int? id)
        {
            CompanyPosition companyposition;
            if (id != null)
                companyposition = DB.CompanyPositions.Single(x => x.ID == id);
            else
                companyposition = new CompanyPosition();
            var model = new CompanyPositionsDetailsModel { CompanyPosition = companyposition };
            return View(model);
        }

        [HttpPost]
        public ActionResult PositionDetails(int? id, CompanyPositionsDetailsModel model)
        {
            if (ModelState.IsValid)
            {
                if (id != null)
                {
                    DB.CompanyPositions.Attach(model.CompanyPosition);
                    DB.Entry(model.CompanyPosition).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    DB.CompanyPositions.Add(model.CompanyPosition);
                }
                DB.SaveChanges();
                var companyID = model.CompanyPosition.CompanyID;
                return RedirectToAction("PositionsList/" + companyID);
            }
            return View(model);
        }

        public ActionResult DeletePosition(int id)
        {
            var companyPosition = DB.CompanyPositions.Single(x => x.ID == id);
            DB.CompanyPositions.Remove(companyPosition);
            DB.SaveChanges();
            var companyID = companyPosition.CompanyID;
            return RedirectToAction("PositionsList/" + companyID);
        }

        public ActionResult AddPosition(int? id)
        {
            if (id != null)
            {
                ViewBag.compID = id;
                var model = new CompanyPositionsDetailsModel();
                model.CompanyPosition = new CompanyPosition();
                return View(model);
            }
            else
                return RedirectToAction("Company", "Companies");
        }

        [HttpPost]
        public ActionResult AddPosition(CompanyPositionsDetailsModel model)
        {
            if (ModelState.IsValid)
            {
                DB.CompanyPositions.Add(model.CompanyPosition);
                DB.SaveChanges();
                return RedirectToAction("PositionsList/" + model.CompanyPosition.CompanyID);
            }
            return View(model);
        }
    }
}