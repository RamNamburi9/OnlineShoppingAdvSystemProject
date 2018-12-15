using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RamsOnlineShoppingSystem.DAL;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNet.Identity;

namespace RamsOnlineShoppingSystem.Models
{
    public partial class ShoppingCart
    {
        RamOnlineShppngDataContext db = new RamOnlineShppngDataContext();

        public string ShoppingCartId { get; set; }

        public const string CartSessionKey = "cartId";

        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();

            cart.ShoppingCartId = cart.GetCartId(context);

            return cart;
        }

        public static void SendEmail(decimal orderamount , int orderid, string toemail )
        {
            RamOnlineShppngDataContext db = new RamOnlineShppngDataContext();
            var query = from prd in db.Orderedproducts
                        where prd.CustomerOrderId == orderid
                        select prd;
            var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
            foreach ( var item in query)
            {
                var product = db.Products.Single(p => p.Id == item.ProductId);
                body += "<p>" + product.Name + " : "+ product.Price+" $</p>";
            }
            body += "<p><a href='http://localhost:55324/ShoppingCart/Chekorders/" + orderid+"'>Please check your Orders here</a>";
            var message = new MailMessage();
            message.To.Add(new MailAddress(toemail));  // replace with valid value 
            message.From = new MailAddress("nramakrishna547@gmail.com");  // replace with valid value
            message.Subject = "Your Order Details On Ram online Shopping";
            message.Body = string.Format(body, "Ram Online Shopping", "nramakrishna547@gmail.com", "Your order has been successfull, plese check your order details."); 
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "nramakrishna547@gmail.com",  // replace with valid value
                    Password = "Ramkinewgmail@"  // replace with valid value
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
               smtp.Send(message);
                //return RedirectToAction("Sent");
            }
        }

      


        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        public void AddToCart(Product product)
        {
            var cartItem = db.Carts.SingleOrDefault(c => c.CartId == ShoppingCartId && c.ProductId == product.Id);

            if (cartItem == null)
            {
                cartItem = new Cart
                {
                    ProductId = product.Id,
                    CartId = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                db.Carts.Add(cartItem);
            }
            else
            {
                cartItem.Count++;
            }

            db.SaveChanges();
        }


        public void AddToCartorder(Product product, int shipped)
        {
            var cartItem = db.Carts.SingleOrDefault(c => c.CartId == ShoppingCartId && c.ProductId == product.Id);

            if (cartItem == null)
            {
                cartItem = new Cart
                {
                    ProductId = product.Id,
                    CartId = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now,
                    shipped = shipped
                };
                    
                db.Carts.Add(cartItem);
            }
            else
            {
                cartItem.Count++;
            }

            db.SaveChanges();
        }

        public int RemoveFromCart(int id)
        {
            var cartItem = db.Carts.SingleOrDefault(cart => cart.CartId == ShoppingCartId && cart.ProductId == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    db.Carts.Remove(cartItem);
                }

                db.SaveChanges();
            }
            return itemCount;
        }

        public void EmptyCart()
        {
            var cartItems = db.Carts.Where(cart => cart.CartId == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                db.Carts.Remove(cartItem);
            }
            db.SaveChanges();
        }


        public void EmptyAllCart()
        {
            var cartItems = db.Carts;

            foreach (var cartItem in cartItems)
            {
                db.Carts.Remove(cartItem);
            }
            db.SaveChanges();
        }



        public List<Cart> GetCartItems()
        {
            return db.Carts.Where(cart => cart.CartId == ShoppingCartId).ToList();
        }

        public int GetCount()
        {
            int? count =
                (from cartItems in db.Carts where cartItems.CartId == ShoppingCartId select (int?)cartItems.Count).Sum();

            return count ?? 0;
        }

        public decimal GetTotal()
        {
            decimal? total = (from cartItems in db.Carts
                              where cartItems.CartId == ShoppingCartId
                              select (int?)cartItems.Count * cartItems.Product.Price).Sum();

            return total ?? decimal.Zero;
        }

        public int CreateOrder(CustomerOrder customerOrder)
        {
            decimal orderTotal = 0;

            var cartItems = GetCartItems();

            foreach (var item in cartItems)
            {
                var orderedProduct = new OrderedProduct
                {
                    ProductId = item.ProductId,
                    CustomerOrderId = customerOrder.Id,
                    Quantity = item.Count
                };

                orderTotal += (item.Count * item.Product.Price);

                db.Orderedproducts.Add(orderedProduct);

               

            }

            customerOrder.Amount = orderTotal;

            db.SaveChanges();

            EmptyCart();

            return customerOrder.Id;
        }

        public int RemoveOrder(int orderid, int productid)
        {
            var orderItem = db.Orderedproducts.SingleOrDefault(op => op.CustomerOrderId == orderid && op.ProductId == productid );
            if (orderItem != null)
            {
                db.Orderedproducts.Remove(orderItem);
                db.SaveChanges();
            }
            return productid;
        }

        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] = context.User.Identity.Name;
                }

                else
                {
                    Guid tempCartId = Guid.NewGuid();
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }

            return context.Session[CartSessionKey].ToString();
        }

        public void MigrateCart(string userName)
        {
            var shoppingCart = db.Carts.Where(c => c.CartId == ShoppingCartId);
            foreach (Cart item in shoppingCart)
            {
                item.CartId = userName;
            }

            db.SaveChanges();
        }

    }
}