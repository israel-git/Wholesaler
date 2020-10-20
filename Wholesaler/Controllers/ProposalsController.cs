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
    public class ProposalsController : SuperController
    {
        // GET: Proposals
        public ActionResult ProposalsList(int? id)
        {
            if (id != null)
            {
                var model = new OrderDetailsModel
                {
                    Order = DB.Orders.Where(x => x.ID == id).FirstOrDefault(),
                    DocumentsList = DB.Documents.Where(x => x.Category == Document.CategoryEnum.Proposal).ToList(),
                    PaymentsList = DB.Payments.Where(x => x.OrderID == id).ToList()
                };
                return View(model);
            }
            else
                return RedirectToAction("Company", "Companies");
        }
        public ActionResult AddProposal(int? id)
        {
            if (id != null)
            {
                ViewBag.orderID = id;
                var model = new ProposalDetailsModel
                {
                    Proposal = new Proposal(),
                    CompaniesList = DB.Companies.ToList(),
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
        public ActionResult AddProposal(ProposalDetailsModel model, int id, HttpPostedFileBase documentFile)
        {
            //The next 3 lines are needed for rendering the view.
            ViewBag.orderID = id;
            model.CompaniesList = DB.Companies.ToList();
            ViewBag.companyID = DB.Orders.Where(x => x.ID == id).FirstOrDefault();

            if (ModelState.IsValid)
            {
                model.Proposal.Date = DateTime.Now;
                DB.Proposals.Add(model.Proposal);
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
                        ModelState.AddModelError("Document.GUID", "File type must be .png, .img, .jpg/.jpeg, .doc/.docx, .pdf or .txt");
                        model = new ProposalDetailsModel
                        {
                            Proposal = new Proposal(),
                            CompaniesList = DB.Companies.ToList(),
                            Order = DB.Orders.Single(x => x.ID == id),
                            PaymentsList = DB.Payments.Where(x => x.OrderID == id).ToList()
                        };
                        return View(model);
                    }

                    var path = Path.Combine(Server.MapPath("~/Uploads/Documents"), guid + fileExtension);
                    documentFile.SaveAs(path);
                    model.Document.GUID = guid + fileExtension;
                    model.Document.ForeignID = model.Proposal.ID;
                    DB.Documents.Add(model.Document);
                    DB.SaveChanges();
                }
                return RedirectToAction("ProposalsList/" + model.Proposal.OrderID);
            }
            model = new ProposalDetailsModel
            {
                Proposal = new Proposal(),
                CompaniesList = DB.Companies.ToList(),
                Order = DB.Orders.Single(x => x.ID == id),
                PaymentsList = DB.Payments.Where(x => x.OrderID == id).ToList()
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
        //The id for proposal details is the proposal ID and not the orderID
        public ActionResult ProposalDetails(int? id)
        {
            var currentProposal = DB.Proposals.Where(x => x.ID == id).FirstOrDefault();
            ViewBag.companyID = DB.Orders.Where(x => x.ID == currentProposal.OrderID).FirstOrDefault();
            Proposal proposal;
            if (id != null)
                proposal = DB.Proposals.Single(x => x.ID == id);
            else
                proposal = new Proposal();
            var model = new ProposalDetailsModel
            {
                Proposal = proposal,
                DocumentsList = DB.Documents.Where(x => x.ForeignID == id && x.Category == Document.CategoryEnum.Proposal).ToList(),
                Document = new Document(),
                Order = DB.Orders.Single(x => x.ID == currentProposal.OrderID),
                PaymentsList = DB.Payments.Where(x => x.OrderID == currentProposal.OrderID).ToList()
            };
            return View(model);
        }
        
        [HttpPost]
        public ActionResult ProposalDetails(int? id, ProposalDetailsModel model, HttpPostedFileBase documentFile)
        {
            var orderID = model.Proposal.OrderID;
            if (ModelState.IsValid)
            {
                if (id != null)
                {
                    DB.Proposals.Attach(model.Proposal);
                    DB.Entry(model.Proposal).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    DB.Proposals.Add(model.Proposal);
                }
                DB.SaveChanges();
                if (documentFile != null)
                {
                    model.Document.Category = Document.CategoryEnum.Proposal;
                    var guid = Guid.NewGuid();
                    var fileExtension = Path.GetExtension(documentFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/Uploads/Documents"), guid + fileExtension);
                    documentFile.SaveAs(path);
                    model.Document.GUID = guid + fileExtension;
                    model.Document.ForeignID = model.Proposal.ID;
                    DB.Documents.Add(model.Document);
                    DB.SaveChanges();
                }
                return RedirectToAction("ProposalsList/" + orderID);
            }

            var currentProposal = DB.Proposals.Where(x => x.ID == id).FirstOrDefault();
            ViewBag.companyID = DB.Orders.Where(x => x.ID == currentProposal.OrderID).FirstOrDefault();

            model = new ProposalDetailsModel
            {
                Proposal = DB.Proposals.Single(x => x.ID == id),
                DocumentsList = DB.Documents.Where(x => x.ForeignID == id && x.Category == Document.CategoryEnum.Proposal).ToList(),
                Document = new Document(),
                Order = DB.Orders.Single(x => x.ID == currentProposal.OrderID),
                PaymentsList = DB.Payments.Where(x => x.OrderID == currentProposal.OrderID).ToList()
            };
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var proposal = DB.Proposals.Single(x => x.ID == id);
            DB.Proposals.Remove(proposal);
            DB.SaveChanges();
            var orderID = proposal.OrderID;
            return RedirectToAction("ProposalsList/" + orderID);
        }
        public ActionResult DeleteDocument(int id)
        {
            var document = DB.Documents.Single(x => x.ID == id);
            var proposal = DB.Proposals.Single(x => x.ID == document.ForeignID);
            //var document = DB.Documents.Single(x => x.ID == id);
            DB.Documents.Remove(document);
            DB.SaveChanges();
            var orderID = proposal.OrderID;
            return RedirectToAction("ProposalsList/" + orderID);
        }
        public ActionResult Export(int id)
        {
            var proposal = DB.Proposals.Single(x => x.ID == id);
            var orderID = proposal.OrderID;
            var order = DB.Orders.Single(x => x.ID == orderID);
            var companyAddress = DB.CompanyAddresses.Where(x => x.CompanyID == order.CompanyID).OrderBy(x => x.OrderBy).FirstOrDefault();

            var proposalPOInvoice = new ProposalPOInvoice
            {
                Category = "Proposal",
                ID = proposal.ID,
                OrderID = proposal.OrderID,
                Number = proposal.ProposalNumber,
                Date = proposal.Date.Value,
                BillToCompany = proposal.BillToCompany,
                BillToAddress = proposal.BillToAddress,
                BillToCity = proposal.BillToCity,
                BillToState = proposal.BillToState,
                BillToZip = proposal.BillToZip,
                BillToName = proposal.BillToName,

                ShipToCompany = proposal.ShipToCompany,
                ShipToAddress = proposal.ShipToAddress,
                ShipToCity = proposal.ShipToCity,
                ShipToState = proposal.ShipToState,
                ShipToZip = proposal.ShipToZip,
                ShipToName = proposal.ShipToName,

                Shipping = order.Shipping,
                TaxRate = proposal.TaxRate,
                FooterNotes = proposal.FooterNotes,
                Terms = proposal.Terms
            };
            string fileName = @"~\Uploads\Documents\exampleWord.docx";
            var guid = GenerateInvoice(companyAddress, proposalPOInvoice);

            Process.Start("WINWORD.EXE", fileName);
            var model = new ProposalDetailsModel();
            model.Document = new Document();
            model.Document.DocumentName = "Proposal for Proposal #" + proposal.ID;
            model.Document.GUID = guid + ".docx";
            model.Document.Category = Document.CategoryEnum.Proposal;
            model.Document.ForeignID = proposal.ID;
            DB.Documents.Add(model.Document);
            DB.SaveChanges();

            return RedirectToAction("ProposalsList/" + orderID);
        }
    }
}