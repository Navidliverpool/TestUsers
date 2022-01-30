using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestUsers.Models;

namespace TestUsers.Controllers
{
    public class ProductController : Controller
    {
        private NavEcommerceDBfirst7Entities db = new NavEcommerceDBfirst7Entities();

        // GET: Product
        public ActionResult Index()
        {
            ViewBag.listProducts = db.Motorcycles.ToList();
            return View();
        }
    }
}