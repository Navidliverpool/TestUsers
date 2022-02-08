using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestUsers.Models
{
    public class Tamrin
    {
        NavEcommerceDBfirstEntities17 storeDB = new NavEcommerceDBfirstEntities17();
        HttpContextBase ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                var cart = new Tamrin();
                List<HttpContextBase> shoppingCartIdList = new List<HttpContextBase>();

                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] =
                       context.User.Identity.Name;
                    
                    cart.ShoppingCartId = context;
                    shoppingCartIdList.Add(context);
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();
                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();

                    cart.ShoppingCartId = context;
                    shoppingCartIdList.Add(context);

                }
            }
            return context.Session[CartSessionKey].ToString();
        }

        public void AddToCart(Motorcycle product)
        {
            // Get the matching cart and album instances
            var cartItem = storeDB.Carts.SingleOrDefault(
                c => c.CartId == ShoppingCartId.User.Identity.Name
                && c.motorcycleId == product.MotorcycleId);

            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new Cart
                {
                    motorcycleId = product.MotorcycleId,
                    CartId = ShoppingCartId.User.Identity.Name,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                storeDB.Carts.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart, 
                // then add one to the quantity
                cartItem.Count++;
            }
            // Save changes
            storeDB.SaveChanges();
        }
    }
}