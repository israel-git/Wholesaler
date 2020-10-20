using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wholesaler.Models;
using Wholesaler.Models.DB;
using Xceed.Words.NET;

namespace Wholesaler.Controllers
{
    public class SuperController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["permissionLevel"] == null)
            {
                Session["permissionLevel"] = 2;
            }
        }
        
    protected WholesaleContext DB = new WholesaleContext();

        protected Guid GenerateInvoice(CompanyAddress companyAddress, ProposalPOInvoice proposalPOInvoice)
        {
            var orderID = proposalPOInvoice.OrderID;
            var orderedProducts = DB.OrderedProducts.Where(x => x.OrderID == orderID).ToList();
            var expenses = DB.Expenses.Where(x => x.OrderID == orderID && x.Internal == false).ToList();
            DocX g_document;
            //g_document = DocX.Load(@"C:\Users\powerPC\source\repos\Wholesaler\Wholesaler\Uploads\Templates\new-file.docx");
            g_document = DocX.Load(Server.MapPath(@"~\Uploads\Templates\new-file.docx"));
            g_document.ReplaceText("##Purchase Order##", proposalPOInvoice.Category);
            g_document.ReplaceText("##Company Name##", companyAddress.AddressName ?? "");
            g_document.ReplaceText("##Company Address##", companyAddress.Address ?? "");
            g_document.ReplaceText("##Company Address 2##", companyAddress.Address2 ?? "");
            g_document.ReplaceText("##Company CityStateZip##", companyAddress.City ?? "" + ", " + companyAddress.State ?? "" + " " + companyAddress.Zip ?? "");
            g_document.ReplaceText("##Company Phone##", companyAddress.Phone ?? "");
            g_document.ReplaceText("##Company Email##", companyAddress.Email ?? "");

            g_document.ReplaceText("##BillTo 1##", proposalPOInvoice.BillToCompany ?? "");
            g_document.ReplaceText("##BillTo 2##", proposalPOInvoice.BillToAddress ?? "");
            g_document.ReplaceText("##BillTo 3##", proposalPOInvoice.BillToCity ?? "" + ", " + proposalPOInvoice.ShipToState ?? "");
            g_document.ReplaceText("##BillTo 4##", proposalPOInvoice.BillToZip ?? "");
            g_document.ReplaceText("##BillTo 5##", proposalPOInvoice.BillToName ?? "");

            g_document.ReplaceText("##ShipTo Line 1##", proposalPOInvoice.ShipToCompany ?? "");
            g_document.ReplaceText("##ShipTo Line 2##", proposalPOInvoice.ShipToAddress ?? "");
            g_document.ReplaceText("##ShipTo Line 3##", proposalPOInvoice.ShipToCity ?? "" + ", " + proposalPOInvoice.ShipToState ?? "");
            g_document.ReplaceText("##ShipTo Line 4##", proposalPOInvoice.ShipToZip ?? "");
            g_document.ReplaceText("##ShipTo Line 5##", proposalPOInvoice.ShipToName ?? "");

            g_document.ReplaceText("##PO Date##", proposalPOInvoice.Date.ToString());
            g_document.ReplaceText("##PO ID##", proposalPOInvoice.ID.ToString());
            g_document.ReplaceText("##PO Num##", proposalPOInvoice.Number ?? "");
            g_document.ReplaceText("##Terms##", proposalPOInvoice.Terms ?? "");
            g_document.ReplaceText("##FooterNotes##", proposalPOInvoice.FooterNotes ?? "");

            int i = 1;
            decimal subtotal = 0;
            //Loop through the ordered items and put them on the form
            foreach (var orderedProd in orderedProducts)
            {

                decimal price = orderedProd.Price ?? 0;
                int quantity = orderedProd.Quantity ?? 0;
                decimal amount = price * quantity;
                subtotal += amount;

                g_document.ReplaceText("##Desc" + i + "##", orderedProd.Description);
                g_document.ReplaceText("##Price" + i + "##", price.ToString("C"));
                g_document.ReplaceText("##Qty" + i + "##", quantity.ToString());
                g_document.ReplaceText("##Amnt" + i + "##", amount.ToString("C"));
                i++;
            }
            //Loop through the expenses and continue populating the form
            decimal totalExpenses = 0;
            foreach (var expense in expenses)
            {
                decimal expAmount = expense.Amount ?? 0;
                totalExpenses += expAmount;
                g_document.ReplaceText("##Desc" + i + "##", expense.Description);
                g_document.ReplaceText("##Price" + i + "##", expAmount.ToString("C"));
                g_document.ReplaceText("##Qty" + i + "##", "");
                g_document.ReplaceText("##Amnt" + i + "##", expAmount.ToString("C"));
                i++;
            }
            //Erase the extra ## tags
            for (i = 1; i < 10; i++)
            {
                g_document.ReplaceText("##Desc" + i + "##", "");
                g_document.ReplaceText("##Price" + i + "##", "");
                g_document.ReplaceText("##Qty" + i + "##", "");
                g_document.ReplaceText("##Amnt" + i + "##", "");
            }
            decimal subtotalWExpenses = subtotal + totalExpenses;
            g_document.ReplaceText("##Subtotal##", subtotalWExpenses.ToString("C"));
            decimal taxRate = proposalPOInvoice.TaxRate ?? 0;
            decimal taxRateAsDecimal = taxRate / 100;
            decimal totalTax = taxRateAsDecimal * subtotal;
            g_document.ReplaceText("##Tax##", totalTax.ToString("C"));
            decimal freight = proposalPOInvoice.Shipping ?? 0;
            g_document.ReplaceText("##Freight##", freight.ToString("C"));
            decimal total = subtotal + totalTax + totalExpenses + freight;
            g_document.ReplaceText("##Total##", total.ToString("C"));

            var guid = Guid.NewGuid();
            var path = Path.Combine(Server.MapPath("~/Uploads/Documents"), guid + ".docx");

            g_document.SaveAs(path);
            return guid;
        }
    }
}