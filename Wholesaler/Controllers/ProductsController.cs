using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wholesaler.Models;
using Wholesaler.Models.DB;

namespace Wholesaler.Controllers
{
    public class ProductsController : SuperController
    {
        public ActionResult ProductsList(int? id)
        {
            ViewBag.id = id;
            var model = new ProductIndexModel()
            {
                Products = DB.Products.OrderBy(x => x.Name).ToList()
            };
            return View(model);
        }
        public ActionResult Details(int? id)
        {
            Product product;
            if (id != null)
                product = DB.Products.Single(x => x.ID == id);
            else
            {
                product = new Product();
                return RedirectToAction("ProductsList");
            }

            var model = new ProductDetailsModel { Product = product };
            return View(model);
        }

        [HttpPost]
        public ActionResult Details(int? id, ProductDetailsModel model)
        {
            if (ModelState.IsValid)
            {
                if (id != null)
                {
                    DB.Products.Attach(model.Product);
                    DB.Entry(model.Product).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    DB.Products.Add(model.Product);
                }
                DB.SaveChanges();
                return RedirectToAction("ProductsList");
            }
            return View(model);
        }
        public ActionResult Delete(int id)
        {
            var product = DB.Products.Single(x => x.ID == id);
            DB.Products.Remove(product);
            DB.SaveChanges();
            return RedirectToAction("ProductsList");
        }

        public ActionResult AddProduct(int? id)
        {
            ViewBag.id = id;
            var model = new ProductDetailsModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult AddProduct(ProductDetailsModel model)
        {
            if (ModelState.IsValid)
            {
                DB.Products.Add(model.Product);
                DB.SaveChanges();
                return RedirectToAction("ProductsList");
            }
            return View(model);
        }
    }
}