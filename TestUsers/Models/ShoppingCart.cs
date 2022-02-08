using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestUsers.Models
{
    public partial class ShoppingCart
    {
        NavEcommerceDBfirstEntities17 storeDB = new NavEcommerceDBfirstEntities17();
        string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";
        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        // Helper method to simplify shopping cart calls
        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        public void AddToCart(Motorcycle product)
        {
                var checkAnyItemExistInCart = storeDB.Carts.Any();

                if  (checkAnyItemExistInCart == false)
                {
                    var item = new Cart
                    {
                        RecordId = 0,
                        motorcycleId = product.MotorcycleId,
                        CartId = ShoppingCartId,
                        Count = 1,
                        DateCreated = DateTime.Now
                    };
                    storeDB.Carts.Add(item);
                }
                else
                {
                    // Get the matching cart and product instances
                    var cartItem = storeDB.Carts.FirstOrDefault(c => c.CartId == ShoppingCartId && c.motorcycleId == product.MotorcycleId);

                //az in link estefade kardam baraye neveshtane recordId: https://stackoverflow.com/questions/7293639/linq-to-entities-does-not-recognize-the-method-last-really
                //recordId etelaate aakharin item ro az sql migire
                var lastCartItemId = storeDB.Carts.OrderByDescending(c => c.RecordId).Select(c => c.RecordId).FirstOrDefault();
                
                if (cartItem == null)
                    {

                    var item = new Cart
                    {
                        RecordId = lastCartItemId++,
                        motorcycleId = product.MotorcycleId,
                        CartId = ShoppingCartId,
                        Count = 1,
                        DateCreated = DateTime.Now
                    };
                    storeDB.Carts.Add(item);
                    }
                    else
                    {
                    var itemReplacement = storeDB.Carts.Where(i => i.RecordId == lastCartItemId).Select(i => i).Single();
                    storeDB.Carts.Remove(itemReplacement);
                    var item = new Cart
                    {
                        motorcycleId = product.MotorcycleId,
                        CartId = ShoppingCartId,
                        Count = cartItem.Count++,
                        DateCreated = DateTime.Now
                    };
                    storeDB.Carts.Add(item);
                    }
                }
            storeDB.SaveChanges();
        }

        public int RemoveFromCart(int id)
        {
            // Get the cart
            var cartItem = storeDB.Carts.Single(
                cart => cart.CartId == ShoppingCartId
                && cart.RecordId == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = (int)cartItem.Count;
                }
                else
                {
                    storeDB.Carts.Remove(cartItem);
                }
                // Save changes
                storeDB.SaveChanges();
            }
            return itemCount;
        }

        public void EmptyCart()
        {
            var cartItems = storeDB.Carts.Where(
                cart => cart.CartId == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                storeDB.Carts.Remove(cartItem);
            }
            // Save changes
            storeDB.SaveChanges();
        }

        public List<Cart> GetCartItems()
        {
            return storeDB.Carts.Where(
                cart => cart.CartId == ShoppingCartId).ToList();
        }

        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in storeDB.Carts
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Count).Sum();
            // Return 0 if all entries are null
            return count ?? 0;
        }

        public decimal GetTotal()
        {
            // Multiply album price by count of that album to get 
            // the current price for each of those albums in the cart
            // sum all album price totals to get the cart total
            decimal? total = (decimal?)(from cartItems in storeDB.Carts
                              where cartItems.CartId == ShoppingCartId
                              select (int?)cartItems.Count *
                              cartItems.Motorcycle.Price).Sum();

            return total ?? decimal.Zero;
        }

        public int CreateOrder(Order order)
        {
            decimal orderTotal = 0;

            var cartItems = GetCartItems();
            // Iterate over the items in the cart, 
            // adding the order details for each
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    MotorcycleId = item.motorcycleId,
                    OrderId = order.OrderID,
                    UnitPrice = (decimal?)item.Motorcycle.Price,
                    Quantity = item.Count
                };
                // Set the order total of the shopping cart
               orderTotal += (decimal)(item.Count * item.Motorcycle.Price);

                storeDB.OrderDetails.Add(orderDetail);

            }
            // Set the order's total to the orderTotal count
            order.Total = orderTotal;

            // Save the order
            storeDB.SaveChanges();
            // Empty the shopping cart
            EmptyCart();
            // Return the OrderId as the confirmation number
            return order.OrderID;
        }

        // We're using HttpContextBase to allow access to cookies.
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] =
                        context.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();
                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        }

        // When a user has logged in, migrate their shopping cart to
        // be associated with their username
        public void MigrateCart(string userName)
        {
            var shoppingCart = storeDB.Carts.Where(
                c => c.CartId == ShoppingCartId);

            foreach (Cart item in shoppingCart)
            {
                item.CartId = userName;
            }
            storeDB.SaveChanges();
        }
    }
}