﻿//using System.Linq;
//using System.Web.Mvc;
//using TestUsers.Models;
//using TestUsers.ViewModels;

//namespace TestUsers.Controllers
//{
//    public class ShoppingCartController : Controller
//    {
//        NavEcommerceDBfirstEntities17 storeDB = new NavEcommerceDBfirstEntities17();
//        //
//        // GET: /ShoppingCart/
//        public ActionResult Index()
//        {
//            var cart = ShoppingCart.GetCart(this.HttpContext);

//            // Set up our ViewModel
//            var viewModel = new ShoppingCartViewModel
//            {
//                CartItems = cart.GetCartItems(),
//                CartTotal = cart.GetTotal(),
//            };
//            // Return the view
//            return View(viewModel);
//        }
//        //
//        // GET: /Store/AddToCart/5
//        public ActionResult AddToCart(int id)
//        {
//            // Retrieve the album from the database
//            var addedAlbum = storeDB.Motorcycles
//                .Single(product => product.MotorcycleId == id);

//            // Add it to the shopping cart
//            var cart = ShoppingCart.GetCart(this.HttpContext);

//            cart.AddToCart(addedAlbum);

//            // Go back to the main store page for more shopping
//            return RedirectToAction("Index");
//        }
//        //
//        // AJAX: /ShoppingCart/RemoveFromCart/5
//        [HttpPost]
//        public ActionResult RemoveFromCart(int id)
//        {
//            // Remove the item from the cart
//            var cart = ShoppingCart.GetCart(this.HttpContext);

//            // Get the name of the album to display confirmation
//            string productName = storeDB.Carts
//                .Single(item => item.RecordId == id).Motorcycle.Model;

//            // Remove from cart
//            int itemCount = cart.RemoveFromCart(id);

//            // Display the confirmation message
//            var results = new ShoppingCartRemoveViewModel
//            {
//                Message = Server.HtmlEncode(productName) +
//                    " has been removed from your shopping cart.",
//                CartTotal = cart.GetTotal(),
//                CartCount = cart.GetCount(),
//                ItemCount = itemCount,
//                DeleteId = id
//            };
//            return Json(results);
//        }
//        //
//        // GET: /ShoppingCart/CartSummary
//        [ChildActionOnly]
//        public ActionResult CartSummary()
//        {
//            var cart = ShoppingCart.GetCart(this.HttpContext);

//            ViewData["CartCount"] = cart.GetCount();
//            return PartialView("CartSummary");
//        }
//    }
//}