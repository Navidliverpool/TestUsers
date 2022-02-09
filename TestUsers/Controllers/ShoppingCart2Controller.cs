//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using TestUsers.Models;
//using TestUsers.ViewModels;

//namespace TestUsers.Controllers
//{
//    public class ShoppingCart2Controller : Controller
//    {
//        private NavEcommerceDBfirstEntities18 db = new NavEcommerceDBfirstEntities18();


//        //This code for creating a shopping cart from:
//        //Here is the link: https://www.youtube.com/watch?v=ZBd0MnKb7u0
//        // GET: ShoppingCart
//        public ActionResult Index()
//        {

//            return View();
//        }

//        private int IsExisting(int id)
//        {
//            List<Item> cart = (List<Item>)Session["cart"];
//            for (int i = 0; i < cart.Count; i++)
//                if (cart[i].Product.MotorcycleId == id)
//                    return i;
//            return -1;
//        }

//        public ActionResult Delete(int id)
//        {
//            int index = IsExisting(id);
//            List<Item> cart = (List<Item>)Session["cart"];
//            cart.RemoveAt(index);
//            Session["cart"] = cart;
//            return View("Cart");
//        }

//        public ActionResult AddToCart(int id)
//        {
//            if (Session["cart"] == null)
//            {
//                List<Item> cart = new List<Item>();
//                cart.Add(new Item(db.Motorcycles.Find(id), 1));
//                Session["cart"] = cart;
//            }
//            else
//            {
//                List<Item> cart = (List<Item>)Session["cart"];
//                int index = IsExisting(id);
//                if (index == -1)
//                    cart.Add(new Item(db.Motorcycles.Find(id), 1));
//                else
//                    cart[index].Quantity++;
//                Session["cart"] = cart;
//            }
//            return View("Cart");
//        }
//    }
//}