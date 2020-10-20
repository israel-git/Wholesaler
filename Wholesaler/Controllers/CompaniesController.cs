using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wholesaler.Models;
using Wholesaler.Models.DB;

namespace Wholesaler.Controllers
{
    public class CompaniesController : SuperController
    {
        public ActionResult Company()
        {
            var model = new CompanyIndexModel
            {
                Companies = DB.Companies.OrderBy(x => x.CompanyName).ToList()
            };
            return View(model);
        }
        public ActionResult Details(int? id)
        {
            Company company;
            if (id != null)
                company = DB.Companies.Single(x => x.ID == id);
            else
            {
                company = new Company();
                return RedirectToAction("Company");
            }
            
            var model = new CompanyDetailsModel {Company = company };
            return View(model);
        }

        [HttpPost]
        public ActionResult Details(int? id, CompanyDetailsModel model, HttpPostedFileBase logo)
        {
            if (ModelState.IsValid)
            {
                if (id != null)
                {
                    DB.Companies.Attach(model.Company);
                    DB.Entry(model.Company).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    DB.Companies.Add(model.Company);
                }
                
                if (logo != null)
                {
                    var guid = Guid.NewGuid();
                    var fileExtension = Path.GetExtension(logo.FileName);
                    if (fileExtension.ToLower() != ".png" && fileExtension.ToLower() != ".doc" &&
                        fileExtension.ToLower() != ".docx" && fileExtension.ToLower() != ".pdf" &&
                        fileExtension.ToLower() != ".jpg" && fileExtension.ToLower() != ".jpeg" &&
                        fileExtension.ToLower() != ".txt" && fileExtension.ToLower() != ".img")
                    {
                        ModelState.AddModelError("Company.Logo", "File type must be .png, .img, .jpg/.jpeg, .doc/.docx, .pdf or .txt");
                        return View(model);
                    }
                    var path = Path.Combine(Server.MapPath("~/Uploads/Logos"), guid + fileExtension);
                    logo.SaveAs(path);
                    model.Company.Logo = guid + fileExtension;
                }
                DB.SaveChanges();
                return RedirectToAction("Company");
            }
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var company = DB.Companies.Single(x => x.ID == id);
            DB.Companies.Remove(company);
            DB.SaveChanges();
            return RedirectToAction("Company");
        }

        public ActionResult AddCompany()
        {
            var model = new CompanyDetailsModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult AddCompany(HttpPostedFileBase compLogo, CompanyDetailsModel model)
        {
               
                if (ModelState.IsValid)
            {
                var guid = Guid.NewGuid();
                if (compLogo != null)
                {
                    var fileExtension = Path.GetExtension(compLogo.FileName);
                    if (fileExtension.ToLower() != ".png" && fileExtension.ToLower() != ".doc" &&
                        fileExtension.ToLower() != ".docx" && fileExtension.ToLower() != ".pdf" &&
                        fileExtension.ToLower() != ".jpg" && fileExtension.ToLower() != ".jpeg" &&
                        fileExtension.ToLower() != ".txt" && fileExtension.ToLower() != ".img")
                    {
                        ModelState.AddModelError("Company.Logo", "File type must be .png, .img, .jpg/.jpeg, .doc/.docx, .pdf or .txt");
                        return View(model);
                    }
                    var path = Path.Combine(Server.MapPath("~/Uploads/Logos"), guid + fileExtension);
                    compLogo.SaveAs(path);
                    model.Company.Logo = guid + fileExtension;
                }
                model.Company.CreateDate = DateTime.Now;
                DB.Companies.Add(model.Company);
                DB.SaveChanges();
                return RedirectToAction("Company");
            }
            return View(model);
        }
    }
}