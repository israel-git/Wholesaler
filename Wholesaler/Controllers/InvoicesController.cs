using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wholesaler.Models;
using Wholesaler.Models.DB;

namespace Wholesaler.Controllers
{
    public class InvoicesController : SuperController
    {
        // GET: Invoices
        public ActionResult InvoicesList(int? id)
        {
            if (id != null)
            {
                var model = new OrderDetailsModel
                {
                    Order = DB.Orders.Where(x => x.ID == id).FirstOrDefault(),
                    DocumentsList = DB.Documents.Where(x => x.Category == Document.CategoryEnum.Invoice).ToList(),
                    PaymentsList = DB.Payments.Where(x => x.OrderID == id).ToList()
                };
                return View(model);
            }
            else
                return RedirectToAction("Company", "Companies");
        }
        public ActionResult AddInvoice(int? id)
        {
            if (id != null)
            {
                ViewBag.orderID = id;
                var model = new InvoiceDetailsModel
                {
                    Invoice = new Invoice(),
                    CompaniesList = DB.Companies.ToList(),
                    Document = new Document(),
                    Order = DB.Orders.Single(x => x.ID == id),
                    PaymentsList = DB.Payments.Where(x => x.OrderID == id).ToList(),
                    LayoutsList = DB.Layouts.ToList()
                };
                
                ViewBag.companyID = DB.Orders.Where(x => x.ID == id).FirstOrDefault();
                return View(model);
            }
            else
                return RedirectToAction("Company", "Companies");
        }

        [HttpPost]
        public ActionResult AddInvoice(InvoiceDetailsModel model, int id, HttpPostedFileBase documentFile)
        {
            if (ModelState.IsValid)
            {
                model.Invoice.Date = DateTime.Now;
                DB.Invoices.Add(model.Invoice);
                DB.SaveChanges();
                if (documentFile != null)
                {
                    model.Document.Category = Document.CategoryEnum.Proposal;
                    var guid = Guid.NewGuid();
                    var fileExtension = Path.GetExtension(documentFile.FileName);

                    if (fileExtension.ToLower() != ".png" && fileExtension.ToLower() != ".doc" &&
                       fileExtension.ToLower() != ".docx" && fileExtension.ToLower() != ".pdf" &&
                       fileExtension.ToLower() != ".jpg" && fileExtension.ToLower() != ".jpeg" &&
                       fileExtension.ToLower() != ".txt" && fileExtension.ToLower() != ".img")
                    {
                        ModelState.AddModelError("Company.Logo", "File type must be .png, .img, .jpg/.jpeg, .doc/.docx, .pdf or .txt");
                        return View(model);
                    }

                    var path = Path.Combine(Server.MapPath("~/Uploads/Documents"), guid + fileExtension);
                    documentFile.SaveAs(path);
                    model.Document.GUID = guid + fileExtension;
                    model.Document.ForeignID = model.Invoice.ID;
                    DB.Documents.Add(model.Document);
                    DB.SaveChanges();
                }
                return RedirectToAction("InvoicesList/" + model.Invoice.OrderID);
            }
            ViewBag.orderID = id;
            ViewBag.companyID = DB.Orders.Where(x => x.ID == id).FirstOrDefault();
            model = new InvoiceDetailsModel
            {
                Invoice = new Invoice(),
                CompaniesList = DB.Companies.ToList(),
                Document = new Document(),
                Order = DB.Orders.Single(x => x.ID == id),
                PaymentsList = DB.Payments.Where(x => x.OrderID == id).ToList(),
                LayoutsList = DB.Layouts.ToList()
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult getCompanyDetailsAJAX(string name)
        {
            var company = DB.Companies.Where(x => x.CompanyName == name).FirstOrDefault();
            var companyDetails = DB.CompanyAddresses.Where(x => x.CompanyID == company.ID).FirstOrDefault();
            var companyAddress = new
            {
                companyDetails.AddressName,
                companyDetails.Address,
                companyDetails.Address2,
                companyDetails.City,
                companyDetails.State,
                companyDetails.Zip
            };
            return Json(companyAddress);
        }
        //The id for invoice details is the invoice ID and not the orderID
        public ActionResult InvoiceDetails(int? id)
        {
            var currentInvoice = DB.Invoices.Where(x => x.ID == id).FirstOrDefault();
            ViewBag.companyID = DB.Orders.Where(x => x.ID == currentInvoice.OrderID).FirstOrDefault();
            Invoice invoice;
            if (id != null)
                invoice = DB.Invoices.Single(x => x.ID == id);
            else
                invoice = new Invoice();
            var model = new InvoiceDetailsModel
            {
                Invoice = invoice,
                DocumentsList = DB.Documents.Where(x => x.ForeignID == id && x.Category == Document.CategoryEnum.Invoice).ToList(),
                Document = new Document(),
                Order = DB.Orders.Single(x => x.ID == currentInvoice.OrderID),
                PaymentsList = DB.Payments.Where(x => x.OrderID == currentInvoice.OrderID).ToList(),
                LayoutsList = DB.Layouts.ToList()
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult InvoiceDetails(int? id, InvoiceDetailsModel model, HttpPostedFileBase documentFile)
        {
            var orderID = model.Invoice.OrderID;
            if (ModelState.IsValid)
            {
                if (id != null)
                {
                    DB.Invoices.Attach(model.Invoice);
                    DB.Entry(model.Invoice).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    DB.Invoices.Add(model.Invoice);
                }
                DB.SaveChanges();
                if (documentFile != null)
                {
                    model.Document.Category = Document.CategoryEnum.Invoice;
                    var guid = Guid.NewGuid();
                    var fileExtension = Path.GetExtension(documentFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/Uploads/Documents"), guid + fileExtension);
                    documentFile.SaveAs(path);
                    model.Document.GUID = guid + fileExtension;
                    model.Document.ForeignID = model.Invoice.ID;
                    DB.Documents.Add(model.Document);
                    DB.SaveChanges();
                }
        
                return RedirectToAction("InvoicesList/" + orderID);
            }
            var currentInvoice = DB.Invoices.Where(x => x.ID == id).FirstOrDefault();
    
            ViewBag.companyID = DB.Orders.Where(x => x.ID == orderID).FirstOrDefault();

            model = new InvoiceDetailsModel
            {
                Invoice = DB.Invoices.Single(x => x.ID == id),
                DocumentsList = DB.Documents.Where(x => x.ForeignID == id && x.Category == Document.CategoryEnum.Invoice).ToList(),
                Document = new Document(),
                Order = DB.Orders.Single(x => x.ID == currentInvoice.OrderID),
                PaymentsList = DB.Payments.Where(x => x.OrderID == currentInvoice.OrderID).ToList(),
                LayoutsList = DB.Layouts.ToList()
            };
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var invoice = DB.Invoices.Single(x => x.ID == id);
            DB.Invoices.Remove(invoice);
            DB.SaveChanges();
            var orderID = invoice.OrderID;
            return RedirectToAction("InvoicesList/" + orderID);
        }
        public ActionResult DeleteDocument(int id)
        {
            var document = DB.Documents.Single(x => x.ID == id);
            var invoice = DB.Invoices.Single(x => x.ID == document.ForeignID);
            DB.Documents.Remove(document);
            DB.SaveChanges();
            var orderID = invoice.OrderID;
            return RedirectToAction("InvoicesList/" + orderID);
        }
        public ActionResult Export(int id)
        {
            var invoice = DB.Invoices.Single(x => x.ID == id);
            var orderID = invoice.OrderID;
            var order = DB.Orders.Single(x => x.ID == orderID);
            var companyAddress = DB.CompanyAddresses.Where(x => x.CompanyID == order.CompanyID).OrderBy(x => x.OrderBy).FirstOrDefault();

            var proposalPOInvoice = new ProposalPOInvoice
            {
                Category = "Invoice",
                ID = invoice.ID,
                OrderID = invoice.OrderID,
                Number = invoice.InvoiceNumber,
                Date = invoice.Date.Value,
                BillToCompany = invoice.BillToCompany,
                BillToAddress = invoice.BillToAddress,
                BillToCity = invoice.BillToCity,
                BillToState = invoice.BillToState,
                BillToZip = invoice.BillToZip,
                BillToName = invoice.BillToName,

                ShipToCompany = invoice.ShipToCompany,
                ShipToAddress = invoice.ShipToAddress,
                ShipToCity = invoice.ShipToCity,
                ShipToState = invoice.ShipToState,
                ShipToZip = invoice.ShipToZip,
                ShipToName = invoice.ShipToName,

                Shipping = order.Shipping,
                TaxRate = invoice.TaxRate,
                FooterNotes = invoice.FooterNotes,
                Terms = invoice.Terms
            };
            string fileName = @"~\Uploads\Documents\exampleWord.docx";
            var guid = GenerateInvoice(companyAddress, proposalPOInvoice);
            Process.Start("WINWORD.EXE", fileName);

            var model = new InvoiceDetailsModel();
            model.Document = new Document();
            model.Document.DocumentName = "Invoice For Invoice #" + invoice.ID;
            model.Document.GUID = guid + ".docx";
            model.Document.Category = Document.CategoryEnum.Invoice;
            model.Document.ForeignID = invoice.ID;
            DB.Documents.Add(model.Document);
            DB.SaveChanges();

            return RedirectToAction("InvoicesList/" + orderID);
        }
    }
}
