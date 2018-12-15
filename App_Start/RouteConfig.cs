using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OnlineShoppingAdvSysProject
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Remove Order",
                url: "{Controller}/RemoveOrder/{id}/{orid}",
                defaults: new { controller = "CustomerOrders", action = "RemoveOrder", id = UrlParameter.Optional , orid = UrlParameter.Optional }
           );

            routes.MapRoute(
              name: "Default",
              url: "{controller}/{action}/{id}",
              defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
          );
        }
    }
}


// @"\d+"
