using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wholesaler.Models;
using Wholesaler.Models.DB;
using Xceed.Words.NET;

namespace Wholesaler.Controllers
{
    public class PurchaseOrdersController : SuperController
    {
        // GET: PurchaseOrders
        public ActionResult PurchaseOrdersList(int? id)
        {
            if (id != null)
            {
                var model = new OrderDetailsModel
                {
                    Order = DB.Orders.Where(x => x.ID == id).FirstOrDefault(),
                    DocumentsList = DB.Documents.Where(x => x.Category == Document.CategoryEnum.PurchaseOrder).ToList(),
                    PaymentsList = DB.Payments.Where(x => x.OrderID == id).ToList()
                };
                return View(model);
            }
            else
                return RedirectToAction("Company", "Companies");
        }
        public ActionResult AddPurchaseOrder(int? id)
        {
            if (id != null)
            {
                ViewBag.orderID = id;
                var model = new PurchaseOrdersDetailsModel
                {
                    PurchaseOrder = new PurchaseOrder(),
                    Document = new Document(),
                    CompaniesList = DB.Companies.ToList(),
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
        public ActionResult AddPurchaseOrder(PurchaseOrdersDetailsModel model, int id, HttpPostedFileBase documentFile)
        {
            if (ModelState.IsValid)
            {
                model.PurchaseOrder.Date = DateTime.Now;
                DB.PurchaseOrders.Add(model.PurchaseOrder);
                DB.SaveChanges();
                if (documentFile != null)
                {
                    model.Document.Category = Document.CategoryEnum.PurchaseOrder;
                    var guid = Guid.NewGuid();
                    var fileExtension = Path.GetExtension(documentFile.FileName);

                    if (fileExtension.ToLower() != ".png" && fileExtension.ToLower() != ".doc" &&
                       fileExtension.ToLower() != ".docx" && fileExtension.ToLower() != ".pdf" &&
                       fileExtension.ToLower() != ".jpg" && fileExtension.ToLower() != ".jpeg" &&
                       fileExtension.ToLower() != ".txt" && fileExtension.ToLower() != ".img")
                    {
                        ModelState.AddModelError("Document.CategoryEnum.PurchaseOrder", "File type must be .png, .img, .jpg/.jpeg, .doc/.docx, .pdf or .txt");
                        return View(model);
                    }

                    var path = Path.Combine(Server.MapPath("~/Uploads/Documents"), guid + fileExtension);
                    documentFile.SaveAs(path);
                    model.Document.GUID = guid + fileExtension;
                    model.Document.ForeignID = model.PurchaseOrder.ID;
                    DB.Documents.Add(model.Document);
                    DB.SaveChanges();
                }
                return RedirectToAction("PurchaseOrdersList/" + model.PurchaseOrder.OrderID);
            }
            ViewBag.orderID = id;
            ViewBag.companyID = DB.Orders.Where(x => x.ID == id).FirstOrDefault();
            model = new PurchaseOrdersDetailsModel
            {
                PurchaseOrder = new PurchaseOrder(),
                Document = new Document(),
                CompaniesList = DB.Companies.ToList(),
                Order = DB.Orders.Single(x => x.ID == id),
                PaymentsList = DB.Payments.Where(x => x.OrderID == id).ToList(),
                LayoutsList = DB.Layouts.ToList()
            };
            return View(model);
        }
        //The id for po details is the po ID and not the orderID
        public ActionResult PurchaseOrderDetails(int? id)
        {
            var currentPO = DB.PurchaseOrders.Where(x => x.ID == id).FirstOrDefault();
            ViewBag.companyID = DB.Orders.Where(x => x.ID == currentPO.OrderID).FirstOrDefault();
            PurchaseOrder purchaseOrder;
            if (id != null)
                purchaseOrder = DB.PurchaseOrders.Single(x => x.ID == id);
            else
                purchaseOrder = new PurchaseOrder();
            var model = new PurchaseOrdersDetailsModel
            { PurchaseOrder = purchaseOrder,
                DocumentsList = DB.Documents.Where(x => x.ForeignID == id && x.Category == Document.CategoryEnum.PurchaseOrder).ToList(),
                Document = new Document(),
                Order = DB.Orders.Single(x => x.ID == currentPO.OrderID),
                PaymentsList = DB.Payments.Where(x => x.OrderID == currentPO.OrderID).ToList(),
                LayoutsList = DB.Layouts.ToList()
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult PurchaseOrderDetails(int? id, PurchaseOrdersDetailsModel model, HttpPostedFileBase documentFile)
        {
            var orderID = model.PurchaseOrder.OrderID;
            if (ModelState.IsValid)
            {
                if (id != null)
                {
                    DB.PurchaseOrders.Attach(model.PurchaseOrder);
                    DB.Entry(model.PurchaseOrder).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    DB.PurchaseOrders.Add(model.PurchaseOrder);
                }
                DB.SaveChanges();
                if (documentFile != null)
                {
                    model.Document.Category = Document.CategoryEnum.PurchaseOrder;
                    var guid = Guid.NewGuid();
                    var fileExtension = Path.GetExtension(documentFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/Uploads/Documents"), guid + fileExtension);
                    documentFile.SaveAs(path);
                    model.Document.GUID = guid + fileExtension;
                    model.Document.ForeignID = model.PurchaseOrder.ID;
                    DB.Documents.Add(model.Document);
                    DB.SaveChanges();
                }
                return RedirectToAction("PurchaseOrdersList/" + orderID);
            }
            ViewBag.companyID = DB.Orders.Where(x => x.ID == orderID).FirstOrDefault();
            var currentPO = DB.PurchaseOrders.Where(x => x.ID == id).FirstOrDefault();

            model = new PurchaseOrdersDetailsModel
            {
                PurchaseOrder = DB.PurchaseOrders.Single(x => x.ID == id),
                DocumentsList = DB.Documents.Where(x => x.ForeignID == id && x.Category == Document.CategoryEnum.PurchaseOrder).ToList(),
                Document = new Document(),
                Order = DB.Orders.Single(x => x.ID == currentPO.OrderID),
                PaymentsList = DB.Payments.Where(x => x.OrderID == currentPO.OrderID).ToList(),
                LayoutsList = DB.Layouts.ToList()
            };

            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var purchaseOrder = DB.PurchaseOrders.Single(x => x.ID == id);
            DB.PurchaseOrders.Remove(purchaseOrder);
            DB.SaveChanges();
            var orderID = purchaseOrder.OrderID;
            return RedirectToAction("PurchaseOrdersList/" + orderID);
        }
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
        public ActionResult Export(int id)
        {
            var purchaseOrder = DB.PurchaseOrders.Single(x => x.ID == id);
            var orderID = purchaseOrder.OrderID;
            var order = DB.Orders.Single(x => x.ID == orderID);
            //var orderedProducts = DB.OrderedProducts.Where(x => x.OrderID == orderID).ToList();
            //var expenses = DB.Expenses.Where(x => x.OrderID == orderID && x.Internal == false).ToList();
            var companyAddress = DB.CompanyAddresses.Where(x => x.CompanyID == order.CompanyID).OrderBy(x => x.OrderBy).FirstOrDefault();

            var proposalPOInvoice = new ProposalPOInvoice
            {
                Category = "Purchase Order",
                ID = purchaseOrder.ID,
                OrderID = purchaseOrder.OrderID,
                Number = purchaseOrder.PurchaseOrderNumber,
                Date = purchaseOrder.Date.Value,
                BillToCompany = purchaseOrder.BillToCompany,
                BillToAddress = purchaseOrder.BillToAddress,
                BillToCity = purchaseOrder.BillToCity,
                BillToState = purchaseOrder.BillToState,
                BillToZip = purchaseOrder.BillToZip,
                BillToName = purchaseOrder.BillToName,

                ShipToCompany = purchaseOrder.ShipToCompany,
                ShipToAddress = purchaseOrder.ShipToAddress,
                ShipToCity = purchaseOrder.ShipToCity,
                ShipToState = purchaseOrder.ShipToState,
                ShipToZip = purchaseOrder.ShipToZip,
                ShipToName = purchaseOrder.ShipToName,

                Shipping = order.Shipping,
                TaxRate = purchaseOrder.TaxRate,
                FooterNotes = purchaseOrder.FooterNotes,
                Terms = purchaseOrder.Terms
            };
            ////string fileName = @"C:\Users\isman\Documents\invoices\generated\exampleWord.docx";
            string fileName = @"~\Uploads\Documents\exampleWord.docx";
            //DocX g_document;
            //g_document = DocX.Load(@"C:\Users\powerPC\source\repos\Wholesaler\Wholesaler\Uploads\Templates\new-file.docx");

            //g_document.ReplaceText("##Purchase Order##", "Purchase Order");
            //g_document.ReplaceText("##Company Name##", companyAddress.AddressName);
            //g_document.ReplaceText("##Company Address##", companyAddress.Address);
            //g_document.ReplaceText("##Company Address 2##", companyAddress.Address2);
            //g_document.ReplaceText("##Company CityStateZip##", companyAddress.City + ", " + companyAddress.State + " " + companyAddress.Zip);
            //g_document.ReplaceText("##Company Phone##", companyAddress.Phone);
            //g_document.ReplaceText("##Company Email##", companyAddress.Email);

            //g_document.ReplaceText("##BillTo 1##", purchaseOrder.BillToCompany);
            //g_document.ReplaceText("##BillTo 2##", purchaseOrder.BillToAddress);
            //g_document.ReplaceText("##BillTo 3##", purchaseOrder.BillToCity + ", " + purchaseOrder.ShipToState);
            //g_document.ReplaceText("##BillTo 4##", purchaseOrder.BillToZip);
            //g_document.ReplaceText("##BillTo 5##", purchaseOrder.BillToName);

            //g_document.ReplaceText("##ShipTo Line 1##", purchaseOrder.ShipToCompany);
            //g_document.ReplaceText("##ShipTo Line 2##", purchaseOrder.ShipToAddress);
            //g_document.ReplaceText("##ShipTo Line 3##", purchaseOrder.ShipToCity + ", " + purchaseOrder.ShipToState);
            //g_document.ReplaceText("##ShipTo Line 4##", purchaseOrder.ShipToZip);
            //g_document.ReplaceText("##ShipTo Line 5##", purchaseOrder.ShipToName);

            //g_document.ReplaceText("##PO Date##", purchaseOrder.Date.ToShortDateString());
            //g_document.ReplaceText("##PO ID##", purchaseOrder.ID.ToString());
            //g_document.ReplaceText("##PO Num##", purchaseOrder.PurchaseOrderNumber);
            //g_document.ReplaceText("##Terms##", purchaseOrder.Terms);
            //var footerNotes = purchaseOrder.FooterNotes ?? "";
            //g_document.ReplaceText("##FooterNotes##", footerNotes);

            //int i = 1;
            //decimal subtotal = 0;
            ////Loop through the ordered items and put them on the form
            //foreach (var orderedProd in orderedProducts)
            //{

            //    decimal price = orderedProd.Price ?? 0;
            //    int quantity = orderedProd.Quantity ?? 0;
            //    decimal amount = price * quantity;
            //    subtotal += amount;

            //    g_document.ReplaceText("##Desc" + i + "##", orderedProd.Description);
            //    g_document.ReplaceText("##Price" + i + "##", price.ToString("C"));
            //    g_document.ReplaceText("##Qty" + i + "##", quantity.ToString());
            //    g_document.ReplaceText("##Amnt" + i + "##", amount.ToString("C"));
            //    i++;
            //}
            ////Loop through the expenses and continue populating the form
            //decimal totalExpenses = 0;
            //foreach (var expense in expenses)
            //{
            //    decimal expAmount = expense.Amount ?? 0;
            //    totalExpenses += expAmount;
            //    g_document.ReplaceText("##Desc" + i + "##", expense.Description);
            //    g_document.ReplaceText("##Price" + i + "##", expAmount.ToString("C"));
            //    g_document.ReplaceText("##Qty" + i + "##", "");
            //    g_document.ReplaceText("##Amnt" + i + "##", expAmount.ToString("C"));
            //    i++;
            //}
            ////Erase the extra ## tags
            //for (i = 1; i < 10; i++)
            //{
            //    g_document.ReplaceText("##Desc" + i + "##", "");
            //    g_document.ReplaceText("##Price" + i + "##", "");
            //    g_document.ReplaceText("##Qty" + i + "##", "");
            //    g_document.ReplaceText("##Amnt" + i + "##", "");
            //}
            //decimal subtotalWExpenses = subtotal + totalExpenses;
            //g_document.ReplaceText("##Subtotal##", subtotalWExpenses.ToString("C"));
            //decimal taxRate = purchaseOrder.TaxRate ?? 0;
            //decimal taxRateAsDecimal = taxRate / 100;
            //decimal totalTax = taxRateAsDecimal * subtotal;
            ////decimal tax = subtotal + totalTax;
            //g_document.ReplaceText("##Tax##", totalTax.ToString("C"));
            //decimal freight = order.Shipping ?? 0;
            //g_document.ReplaceText("##Freight##", freight.ToString("C"));
            //decimal total = subtotal + totalTax + totalExpenses + freight;
            //g_document.ReplaceText("##Total##", total.ToString("C"));

            ////var guid = Guid.NewGuid();
            //var path = Path.Combine(Server.MapPath("~/Uploads/Documents"), guid + ".docx");

            //g_document.SaveAs(path);
            //g_document.SaveAs(fileName);

            var guid = GenerateInvoice(companyAddress, proposalPOInvoice);

            Process.Start("WINWORD.EXE", fileName);
            var model = new PurchaseOrdersDetailsModel();
            model.Document = new Document();
            model.Document.DocumentName = "PO For PO #" + purchaseOrder.ID;
            model.Document.GUID = guid + ".docx";
            model.Document.Category = Document.CategoryEnum.PurchaseOrder;
            model.Document.ForeignID = purchaseOrder.ID;
            DB.Documents.Add(model.Document);
            DB.SaveChanges();

            return RedirectToAction("PurchaseOrdersList/" + orderID);
        }
    }
}