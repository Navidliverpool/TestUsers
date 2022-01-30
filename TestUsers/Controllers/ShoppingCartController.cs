using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestUsers.Models;

namespace TestUsers.Controllers
{
    public class ShoppingCartController : Controller
    {
        private NavEcommerceDBfirst7Entities db = new NavEcommerceDBfirst7Entities();

        // GET: ShoppingCart
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult OrderNow(int? id)
        {
            if(Session["cart"] == null)
            {
                List<Item> cart = new List<Item>();
                cart.Add(new Item(db.Motorcycles.Find(id), 1));
                Session["cart"] = cart;
            }
            return View("Cart");
        }
    }
}