using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DannyPieShop.Models
{
    public class ShoppingCart
    {
        private readonly AppDbContext appDbContext;

        public ShoppingCart(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public string ShoppingCartId { get; set; }
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }
        /// <summary>
        /// This method called at Startup file time and get the cart value for a particular user session if exists
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static ShoppingCart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;

            var context = services.GetService<AppDbContext>();

            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();

            session.SetString("CartId", cartId);

            return new ShoppingCart(context) { ShoppingCartId = cartId };   // set shopping cart id here further it will use at add and remove cart item
        }

        /// <summary>
        /// getting shopping cart items from db on the basis of shopping card id
        /// </summary>
        /// <returns></returns>
        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            return ShoppingCartItems ??
                   (ShoppingCartItems =
                       appDbContext.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                           .Include(s => s.Pie)
                           .ToList());
        }

        /// <summary>
        /// getting total cart value gere through pie value and amount
        /// </summary>
        /// <returns></returns>
        public decimal GetShoppingCartTotal()
        {
            var total = appDbContext.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                .Select(c => c.Pie.Price * c.Amount).Sum();
            return total;
        }

        /// <summary>
        /// add item to cart
        /// </summary>
        /// <param name="pie"></param>
        /// <param name="amount"></param>
        public void AddToCart(Pie pie, int amount)
        {
            var shoppingCartItem =
                    appDbContext.ShoppingCartItems.SingleOrDefault(
                        s => s.Pie.PieId == pie.PieId && s.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId,
                    Pie = pie,
                    Amount = 1
                };

                appDbContext.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount++;
            }
            appDbContext.SaveChanges();
        }

        /// <summary>
        /// remove cart item
        /// </summary>
        /// <param name="pie"></param>
        /// <returns></returns>
        public int RemoveFromCart(Pie pie)
        {
            var shoppingCartItem =
                    appDbContext.ShoppingCartItems.SingleOrDefault(
                        s => s.Pie.PieId == pie.PieId && s.ShoppingCartId == ShoppingCartId);

            var localAmount = 0;

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;
                    localAmount = shoppingCartItem.Amount;
                }
                else
                {
                    appDbContext.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }

            appDbContext.SaveChanges();

            return localAmount;
        }

        /// <summary>
        /// clear cart on the basis of shopping Id
        /// </summary>
        public void ClearCart()
        {
            var cartItems = appDbContext
                .ShoppingCartItems
                .Where(cart => cart.ShoppingCartId == ShoppingCartId);

            appDbContext.ShoppingCartItems.RemoveRange(cartItems);

            appDbContext.SaveChanges();
        }

    }
}
