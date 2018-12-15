using OnlineShoppingAdvSysProject.ViewModels;
using RamsOnlineShoppingSystem.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RamsOnlineShoppingSystem.Models;

namespace OnlineShoppingAdvSysProject.Controllers
{
    public class CustomerOrdersController : Controller
    {
        private RamOnlineShppngDataContext db = new RamOnlineShppngDataContext();
        // GET: CustomerOrders
        public ActionResult Index()
        {
            var viewModel =
        from a in db.Orderedproducts
        join b in db.CustomerOrders on a.CustomerOrderId equals b.Id
        join c in db.Products on a.ProductId equals c.Id
        where a.shipped == 0
        select new CustomerOrdersViewModel { OrderedProduct = a, CustomerOrder = b, Products = c };

            return View(viewModel);
        }

        public ActionResult Shipped(int id)
        {
            var ordercustomers = db.Orderedproducts.FirstOrDefault(op => op.ProductId == id);

            ordercustomers.shipped = 1;

            var caritem = db.Carts.FirstOrDefault(ci => ci.ProductId == id);

            if (caritem != null)
            { 

            caritem.shipped = caritem.shipped + 1;
             }


            db.SaveChanges();

            return RedirectToAction("index");
        }

        public ActionResult RemoveOrder(int id, int orid)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            int itemCount = cart.RemoveOrder(orid, id);
            var viewModel1 =
       from a in db.Orderedproducts
       join b in db.CustomerOrders on a.CustomerOrderId equals b.Id
       join c in db.Products on a.ProductId equals c.Id
       where b.Id == orid
       select new CustomerOrdersViewModel { OrderedProduct = a, CustomerOrder = b, Products = c };

            return View(viewModel1);


        }

    }
}