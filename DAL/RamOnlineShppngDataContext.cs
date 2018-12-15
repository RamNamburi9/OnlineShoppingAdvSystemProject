using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using RamsOnlineShoppingSystem.Models;


namespace RamsOnlineShoppingSystem.DAL
{
    public class RamOnlineShppngDataContext : DbContext
    {

        public RamOnlineShppngDataContext() : base("RamsOnlineshoppingDB")
        {

        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Customer> Customers { get; set; }

       

        public DbSet<CustomerOrder> CustomerOrders { get; set; }

        public DbSet<OrderedProduct> Orderedproducts { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public System.Data.Entity.DbSet<RamsOnlineShoppingSystem.Models.ShoppingCart> ShoppingCarts { get; set; }
    }
}