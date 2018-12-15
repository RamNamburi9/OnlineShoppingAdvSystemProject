using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RamsOnlineShoppingSystem.DAL;
using RamsOnlineShoppingSystem.Models;
using RamsOnlineShoppingSystem.ViewModels;

namespace RamsOnlineShoppingSystem.Controllers
{
    public class ShoppingCartController : Controller
    {
        private RamOnlineShppngDataContext db = new RamOnlineShppngDataContext();

        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };

            return View(viewModel);
        }


        public ActionResult modifyorders()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };

            return View(viewModel);
        }



        public ActionResult AddToCart(int id)
        {
            var addedProduct = db.Products.Single(product => product.Id == id);

            var query = from prd in db.Products
                        where prd.Id == id & prd.numberofitems > 0
                        select prd;



            foreach (Product prd in query)
            {
                if (prd.numberofitems > 0)
                {
                    prd.numberofitems = prd.numberofitems - 1;
                    var cart = ShoppingCart.GetCart(this.HttpContext);

                    cart.AddToCart(addedProduct);
                }
                else
                {

                }
            }
            db.SaveChanges();


            return RedirectToAction("Index");
        }

        public ActionResult AddToCartorder(int id)
        {
            var addedProduct = db.Products.Single(product => product.Id == id);

            var query = from prd in db.Products
                        where prd.Id == id & prd.numberofitems > 0
                        select prd;



            foreach (Product prd in query)
            {
                if (prd.numberofitems > 0)
                {
                    prd.numberofitems = prd.numberofitems - 1;
                    var cart = ShoppingCart.GetCart(this.HttpContext);

                    cart.AddToCart(addedProduct);
                }
                else
                {

                }
            }
            db.SaveChanges();


            return RedirectToAction("modifyorders");
        }

        public ActionResult Chekorders(int id)
        {
                   

            var query = from prd in db.Orderedproducts
                        where prd.CustomerOrderId == id
                        select prd;

           

            foreach (var prditem in query)
            {
                var addedProduct = db.Products.Single(product => product.Id == prditem.ProductId);
                var cart = ShoppingCart.GetCart(this.HttpContext);
                cart.AddToCartorder(addedProduct,prditem.shipped);
                db.SaveChanges();
            }
            
           


                return RedirectToAction("modifyorders",id);
        }

        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            string productName = db.Carts.FirstOrDefault(item => item.ProductId == id).Product.Name;

            int itemCount = cart.RemoveFromCart(id);



            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(productName) + " has been removed from your shopping cart",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };

            return Json(results);
        }


        public ActionResult SendEmail(int? id)
        {


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                int orid = (int)id;
                ShoppingCart.SendEmail(0, orid, User.Identity.Name); // sending an email 
                return View("Saveorder", new { id = id });
            }



        }

        //public  Action Saveorder(int? id)
        //{
        //    ViewBag.id = id;
        //    return View();
        //}



        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }

    }
}
