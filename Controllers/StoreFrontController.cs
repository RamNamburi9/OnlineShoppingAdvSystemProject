using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RamsOnlineShoppingSystem.Models;
using RamsOnlineShoppingSystem.DAL;

namespace RamsOnlineShoppingSystem.Controllers
{
    public class StoreFrontController : Controller
    {
        private RamOnlineShppngDataContext db = new RamOnlineShppngDataContext();
        // GET: StoreFront
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
    }
}