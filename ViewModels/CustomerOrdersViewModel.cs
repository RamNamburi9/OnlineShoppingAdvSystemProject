using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RamsOnlineShoppingSystem.Models;

namespace OnlineShoppingAdvSysProject.ViewModels
{
    public class CustomerOrdersViewModel
    {
        public  Product Products { get; set; }
        public CustomerOrder CustomerOrder { get; set; }
        public OrderedProduct OrderedProduct { get; set; }
    }
}   