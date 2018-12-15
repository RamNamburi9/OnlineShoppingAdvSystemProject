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
using Microsoft.AspNet.Identity;


namespace RamsOnlineShoppingSystem.Controllers
{
        [Authorize]
    public class CheckoutController : Controller
    {
        private RamOnlineShppngDataContext db = new RamOnlineShppngDataContext();


        const String PromoCode = "FREE";
        public ActionResult AddressAndPayment()
        {
            string username = User.Identity.GetUserName();

            var query = from custrd in db.CustomerOrders
                        where custrd.Email == User.Identity.Name 
                        select custrd;

            var custorder = db.CustomerOrders.Where(p => p.CustomerUserName == username).FirstOrDefault();


            return View(custorder);
        }

        [HttpPost]
        public ActionResult SendEmail(int orderid)
        {
            ShoppingCart.SendEmail(0, orderid, User.Identity.Name); // sending an email 
            return RedirectToAction("Complete", new { id = orderid, Amout = 0 });
        }

        [HttpPost]
        public ActionResult AddressAndPayment(FormCollection values)
        {
            var order = new CustomerOrder();

            TryUpdateModel(order);

            try
            {
                if (string.Equals(values["PromoCode"], PromoCode, StringComparison.OrdinalIgnoreCase) == false)
                {
                    return View(order);
                }
                else
                {
                    order.CustomerUserName = User.Identity.Name;
                    order.DateCreated = DateTime.Now;

                    db.CustomerOrders.Add(order);
                    db.SaveChanges();

                    var cart = ShoppingCart.GetCart(this.HttpContext);
                    cart.CreateOrder(order);

                    db.SaveChanges();

                    ShoppingCart.SendEmail(order.Amount, order.Id, User.Identity.Name); // sending an email 

                    return RedirectToAction("Complete", new { id = order.Id , Amout = order.Amount});
                }
            }
            catch (Exception ex)
            {
                ex.InnerException.ToString();
                return View(order);
            }
        }

        public ActionResult Complete(int id)
        {
            bool isValid = db.CustomerOrders.Any(
                o => o.Id == id &&
                     o.CustomerUserName == User.Identity.Name
                );

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }
    }
}
