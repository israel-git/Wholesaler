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
    public class PaymentsController : SuperController
    {
        public ActionResult PaymentsList(int? id)
        {
            if (id != null)
            {
                var model = new OrderDetailsModel
                {
                    Order = DB.Orders.Where(x => x.ID == id).FirstOrDefault(),
                    DocumentsList = DB.Documents.Where(x => x.Category == Document.CategoryEnum.Payment).ToList(),
                    PaymentsList = DB.Payments.Where(x => x.OrderID == id).ToList()
                };
                return View(model);
            }
            else
                return RedirectToAction("Company", "Companies");
        }
        public ActionResult AddPayment(int? id)
        {
            if (id != null)
            {
                ViewBag.orderID = id;
                var model = new PaymentDetailsModel
                {
                    Payment = new Payment(),
                    Document = new Document(),
                    Order = DB.Orders.Single(x => x.ID == id),
                    PaymentsList = DB.Payments.Where(x => x.OrderID == id).ToList()
                };
                
                ViewBag.companyID = DB.Orders.Where(x => x.ID == id).FirstOrDefault();
                return View(model);
            }
            else
                return RedirectToAction("Company", "Companies");
        }

        [HttpPost]
        public ActionResult AddPayment(PaymentDetailsModel model, int id, HttpPostedFileBase documentFile)
        {
            if (ModelState.IsValid)
            {
                model.Payment.Date = DateTime.Now;
                DB.Payments.Add(model.Payment);
                DB.SaveChanges();
                if (documentFile != null)
                {
                    model.Document.Category = Document.CategoryEnum.Payment;
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
                    model.Document.ForeignID = model.Payment.ID;
                    DB.Documents.Add(model.Document);
                    DB.SaveChanges();
                }
                return RedirectToAction("PaymentsList/" + model.Payment.OrderID);
            }
            ViewBag.orderID = id;
            ViewBag.companyID = DB.Orders.Where(x => x.ID == id).FirstOrDefault();
            return View(model);
        }
        //The id for payment details is the payment ID and not the orderID
        public ActionResult PaymentDetails(int? id)
        {
            var currentPayment = DB.Payments.Where(x => x.ID == id).FirstOrDefault();
            ViewBag.companyID = DB.Orders.Where(x => x.ID == currentPayment.OrderID).FirstOrDefault();
            Payment payment;
            if (id != null)
                payment = DB.Payments.Single(x => x.ID == id);
            else
                payment = new Payment();
            var model = new PaymentDetailsModel
            {
                Payment = payment,
                DocumentsList = DB.Documents.Where(x => x.ForeignID == id && x.Category == Document.CategoryEnum.Payment).ToList(),
                Document = new Document(),
                Order = DB.Orders.Single(x => x.ID == currentPayment.OrderID),
                PaymentsList = DB.Payments.Where(x => x.OrderID == currentPayment.OrderID).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult PaymentDetails(int? id, PaymentDetailsModel model, HttpPostedFileBase documentFile)
        {
            var orderID = model.Payment.OrderID;
            if (ModelState.IsValid)
            {
                if (id != null)
                {
                    DB.Payments.Attach(model.Payment);
                    DB.Entry(model.Payment).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    DB.Payments.Add(model.Payment);
                }
                DB.SaveChanges();
                if (documentFile != null)
                {
                    model.Document.Category = Document.CategoryEnum.Payment;
                    var guid = Guid.NewGuid();
                    var fileExtension = Path.GetExtension(documentFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/Uploads/Documents"), guid + fileExtension);
                    documentFile.SaveAs(path);
                    model.Document.GUID = guid + fileExtension;
                    model.Document.ForeignID = model.Payment.ID;
                    DB.Documents.Add(model.Document);
                    DB.SaveChanges();
                }
                return RedirectToAction("PaymentsList/" + orderID);
            }
            var currentPayment = DB.Payments.Where(x => x.ID == id).FirstOrDefault();
            model = new PaymentDetailsModel
            {
                Payment = DB.Payments.Single(x => x.ID == id),
                DocumentsList = DB.Documents.Where(x => x.ForeignID == id && x.Category == Document.CategoryEnum.Payment).ToList(),
                Document = new Document(),
                Order = DB.Orders.Single(x => x.ID == currentPayment.OrderID),
                PaymentsList = DB.Payments.Where(x => x.OrderID == currentPayment.OrderID).ToList()
            };
            ViewBag.companyID = DB.Orders.Where(x => x.ID == orderID).FirstOrDefault();
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var payment = DB.Payments.Single(x => x.ID == id);
            DB.Payments.Remove(payment);
            DB.SaveChanges();
            var orderID = payment.OrderID;
            return RedirectToAction("PaymentsList/" + orderID);
        }
        public ActionResult DeleteDocument(int id)
        {
            var document = DB.Documents.Single(x => x.ID == id);
            var payment = DB.Payments.Single(x => x.ID == document.ForeignID);
            DB.Documents.Remove(document);
            DB.SaveChanges();
            var orderID = payment.OrderID;
            return RedirectToAction("PaymentsList/" + orderID);
        }
    }
}