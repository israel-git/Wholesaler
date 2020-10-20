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
    public class CompanyDocumentsController : SuperController
    { 
    public ActionResult DocumentsList(int? id)
    {
        if (id != null)
        {
                var model = new CompanyDetailsModel
            {
                Company = DB.Companies.Where(x => x.ID == id).FirstOrDefault(),
                DocumentsList = DB.Documents.Where(x => x.Category == Document.CategoryEnum.Company && x.ForeignID == id).ToList()
            };
            return View(model);
        }
        else
            return RedirectToAction("Company", "Companies");
    }

    public ActionResult Delete(int id)
    {
            var document = DB.Documents.Single(x => x.ID == id);
            DB.Documents.Remove(document);
        DB.SaveChanges();
            var companyID = document.ForeignID;
        return RedirectToAction("DocumentsList/" + companyID);
    }
        //
        public ActionResult DeleteDocument(int id)
        {
            var document = DB.Documents.Single(x => x.ID == id);
            var purchaseOrder = DB.PurchaseOrders.Single(x => x.ID == document.ForeignID);
            //var document = DB.Documents.Single(x => x.ID == id);
            DB.Documents.Remove(document);
            DB.SaveChanges();
            var orderID = purchaseOrder.OrderID;
            return RedirectToAction("PurchaseOrdersList/" + orderID);
        }
     
        public ActionResult AddDocument(int? id)
    {
        if (id != null)
        {
            ViewBag.compID = id;
                //var model = new CompanyDetailsModel();
                //    model.Company = new Company();
                //    model.Document = new Document();
                var model = new CompanyDetailsModel
                {
                    Company = new Company(),
                    Document = new Document()
                };
                return View(model);
        }
        else
            return RedirectToAction("Company", "Companies");

    }
       
    [HttpPost]
    public ActionResult AddDocument(HttpPostedFileBase documentFile, CompanyDetailsModel model, int id)
    {
            if (ModelState.IsValid)
        {
                if (documentFile != null)
                {
                    model.Document.Category = Document.CategoryEnum.Company;
                    var guid = Guid.NewGuid();
                    var fileExtension = Path.GetExtension(documentFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/Uploads/Documents"), guid + fileExtension);
                    documentFile.SaveAs(path);
                    model.Document.GUID = guid + fileExtension;
                    model.Document.ForeignID = id;
                    DB.Documents.Add(model.Document);
                    DB.SaveChanges();
                }
            return RedirectToAction("DocumentsList/" + id);
        }
        
        return View(model);
        }
    }
}
