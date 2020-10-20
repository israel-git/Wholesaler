using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wholesaler.Models;
using Wholesaler.Models.DB;

namespace Wholesaler.Controllers
{
    public class OrdersController : SuperController
    {
        // GET: Orders
        public ActionResult OrdersList(int? id)
        {
            if (id != null)
            {
                var model = new CompanyDetailsModel
                {
                    Company = DB.Companies.Where(x => x.ID == id).FirstOrDefault(),
                    OrdersList = DB.Orders.Where(x => x.CompanyID == id).ToList(),
                    OrderedProductsList = DB.OrderedProducts.ToList(),
                    ExpensesList = DB.Expenses.ToList(),
                    PaymentsList = DB.Payments.ToList()
                };
                return View(model);
            }
            else
                return RedirectToAction("Company", "Companies");
        }
        public ActionResult OrderDetails(int? id)
        {
            Order order;
            if (id != null)
                order = DB.Orders.SingleOrDefault(x => x.ID == id);
            else
                order = new Order();
            var model = new OrderDetailsModel
            {
                Order = order,
                OrderedProduct = new OrderedProduct(),
                //OrderedProductsList = DB.OrderedProducts.Where(x => x.OrderID == id).ToList(),
                ProductsList = DB.Products.OrderBy(x => x.Name).ToList(),
                CompaniesList = DB.Companies.ToList(),
                // ExpensesList = DB.Expenses.Where(x => x.OrderID == id).ToList()
                Expense = new Expense(),
                PaymentsList = DB.Payments.Where(x => x.OrderID == id).ToList(),
                OrderStatusesList = DB.OrderStatuses.ToList()
        };
            ViewBag.id = model.Order.CompanyID;
            return View(model);
        }

        [HttpPost]
        public ActionResult OrderDetails(int? id, OrderDetailsModel model)
        {
            var companyID = model.Order.CompanyID;
            if (ModelState.IsValid)
            {
                if (id != null)
                {
                    var companies = DB.Companies.ToList();
                    ViewBag.companies = companies;
                    DB.Orders.Attach(model.Order);
                    DB.Entry(model.Order).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    DB.Orders.Add(model.Order);
                }
                DB.SaveChanges();
                return RedirectToAction("OrdersList/" + companyID);
            }
          
            return RedirectToAction("OrdersList/" + companyID);
        }
        //id here is orderedProductID
        public ActionResult OrderedProductDetails(int? id)
        {
            OrderedProduct orderedProduct;
            Order order;
            if (id != null)
            {
                orderedProduct = DB.OrderedProducts.Single(x => x.ID == id);
                order = DB.Orders.Single(x => x.ID == orderedProduct.OrderID);
            }
            else
            {
                order = new Order();
                orderedProduct = new OrderedProduct { Order = order };
                
            }
            var model = new OrderedProductDetailsModel
            {
                OrderedProduct = orderedProduct,
                Order = order
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult OrderedProductDetails(int? id, OrderedProductDetailsModel model)
        {
            var orderID = model.OrderedProduct.OrderID;
            //if (!ModelState.IsValid)
            //{
            //    var errors = ViewData.ModelState.Where(n => n.Value.Errors.Count > 0).ToList();
            //}
            if (ModelState.IsValid)
            {
                if (id != null)
                {
                    model.OrderedProduct.ID = id.Value;
                    DB.OrderedProducts.Attach(model.OrderedProduct);
                    DB.Entry(model.OrderedProduct).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    DB.OrderedProducts.Add(model.OrderedProduct);
                }
                DB.SaveChanges();
                return RedirectToAction("OrderDetails/" + orderID);
               
            }
            return RedirectToAction("OrderDetails/" + orderID);
            
        }
        //id here is expense id, not orderID
        public ActionResult ExpenseDetails(int? id)
        {
            Expense expense;
            Order order;
            if (id != null)
            {
                expense = DB.Expenses.Single(x => x.ID == id);
                order = DB.Orders.Single(x => x.ID == expense.OrderID);
            }
            else
            {
                order = new Order();
                expense = new Expense { Order = order };

            }
            var model = new ExpenseDetailsModel
            {
                Expense = expense,
                Order = order
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult ExpenseDetails(int? id, ExpenseDetailsModel model)
        {
            var orderID = model.Expense.OrderID;
            //if (!ModelState.IsValid)
            //{
            //    var errors = ViewData.ModelState.Where(n => n.Value.Errors.Count > 0).ToList();
            //}
            if (ModelState.IsValid)
            {
                if (id != null)
                {
                    model.Expense.ID = id.Value;
                    DB.Expenses.Attach(model.Expense);
                    DB.Entry(model.Expense).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    DB.Expenses.Add(model.Expense);
                }
                DB.SaveChanges();
                return RedirectToAction("OrderDetails/" + orderID);

            }
            return RedirectToAction("OrderDetails/" + orderID);

        }

        public ActionResult AddOrder(int? id)
        {
            if (id != null)
            {
                ViewBag.compID = id;
                var model = new OrderDetailsModel
                {
                    Order = new Order(),
                    CompaniesList = DB.Companies.ToList(),
                    OrderStatusesList = DB.OrderStatuses.ToList()
            };
                
                return View(model);
            }
            else
                return RedirectToAction("Company", "Companies");
        }

        [HttpPost]
        public ActionResult AddOrder(OrderDetailsModel model)
        {
            if (ModelState.IsValid)
            {
                model.Order.Date = DateTime.Now;
                DB.Orders.Add(model.Order);
                DB.SaveChanges();
                return RedirectToAction("OrdersList/" + model.Order.CompanyID);
            }

            model = new OrderDetailsModel
            {
                Order = new Order(),
                CompaniesList = DB.Companies.ToList(),
                OrderStatusesList = DB.OrderStatuses.ToList()
            };
            return View(model);
        }
        public ActionResult DeleteOrder(int id)
        {
            var order = DB.Orders.Single(x => x.ID == id);
            DB.Orders.Remove(order);
            DB.SaveChanges();
            var companyID = order.CompanyID;
            return RedirectToAction("OrdersList/" + companyID);
        }
        public ActionResult DeleteProduct(int id)
        {
            //var order = DB.Orders.Single(x => x.ID == id);
            var orderedProduct = DB.OrderedProducts.Single(x => x.ID == id);
            DB.OrderedProducts.Remove(orderedProduct);
            DB.SaveChanges();
            var orderID = orderedProduct.OrderID;
            return RedirectToAction("OrderDetails/" + orderID);
        }
        public ActionResult DeleteExpense(int id)
        {
            var expense = DB.Expenses.Single(x => x.ID == id);
            DB.Expenses.Remove(expense);
            DB.SaveChanges();
            var orderID = expense.OrderID;
            return RedirectToAction("OrderDetails/" + orderID);
        }
        [HttpPost]
        public ActionResult getProductDetailsAJAX(int productID)
        {
            var productInDB = DB.Products.Single(x => x.ID == productID);
            //Not sure why, but if I put into the product object productInDB.CaseQuant, it shows up in th view as undefined.
            //I thought because a number doesn't have quotation marks, but cost and price show up fine.
            var caseQuant = productInDB.CaseQuantity;
            var product = new
            {
                productInDB.Name,
                productInDB.SKU,
                productInDB.Cost,
                productInDB.Price,
                caseQuant

            };

            return Json(product);
        }

        [HttpPost]
        public ActionResult addOrderedProductAJAX(int orderID, int productID, string description, decimal cost, decimal price, int quantity)
        {
            var orderedProduct = new OrderedProduct
            {
                OrderID = orderID,
                ProductID = productID,
                Description = description,
                Quantity = quantity,
                Cost = cost,
                Price = price
            };
            DB.OrderedProducts.Add(orderedProduct);
            DB.SaveChanges();
            return Json(Url.Action("OrderDetails/" + orderID, "Orders"));
        }

        [HttpPost]
        public ActionResult addExpenseAJAX(int orderID, string description, decimal amount, bool intern)
        {
            var expense = new Expense
            {
                OrderID = orderID,
                Description = description,
                Amount = amount,
                Internal = intern
            };
            DB.Expenses.Add(expense);
            DB.SaveChanges();
            return Json(Url.Action("OrderDetails/" + orderID, "Orders"));
        }
    }
}