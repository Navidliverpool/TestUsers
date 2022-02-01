using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestUsers.Models;
using TestUsers.ViewModels;

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

        private int IsExisting(int id)
        {
            List<Item> cart = (List<Item>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].Product.MotorcycleId == id)
                    return i;
            return -1;
        }

        public ActionResult AddToCart(int id)
        {
            if(Session["cart"] == null)
            {
                List<Item> cart = new List<Item>();
                cart.Add(new Item(db.Motorcycles.Find(id), 1));
                Session["cart"] = cart;
            }
            else
            {
                List<Item> cart = (List<Item>)Session["cart"];
                int index = IsExisting(id);
                if (index == -1)
                    cart.Add(new Item(db.Motorcycles.Find(id), 1));
                else
                    cart[index].Quantity++;
                Session["cart"] = cart;
            }
            return View("Cart");
        }

        //[HttpPost]
        //public ActionResult OrderNow(OrderDetailVM orderDetailVM)
        //{
        //    OrderDetail orderDetail = new OrderDetail
        //    {
        //        MotorcycleId = orderDetailVM.OrderDatailProductWhere(odp => odp.MotorcycleId == )
        //    }
        //    return View("Cart");
        //}



        //private OrderDetail orderDetail = new OrderDetail();

        //public void SaveOrderDatail()
        //{
        //    List<Item> items = new List<Item>();
        //    items.Add();
        //    db.OrderDetails.Add();
        //    db.SaveChanges();
        //}
    }
}