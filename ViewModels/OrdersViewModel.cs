using RamsOnlineShoppingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RamsOnlineShoppingSystem.ViewModels
{
    public class OrdersViewModel
    {
        public List<Cart> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }
}