using Microsoft.AspNetCore.Http;
using MvcEStoreData;
using MVCEStoreWeb.Models;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace MVCEStoreWeb.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly AppDbContext context;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ShoppingCartService(
            AppDbContext context,
            IHttpContextAccessor httpContextAccessor
            )
        {
            this.context = context;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task AddToCart(int productId, int quantity = 1)
        {
            var product = await context.Products.FindAsync(productId);
            var cookie = httpContextAccessor.HttpContext.Request.Cookies["shoppingCart"];
            var shoppingCart = new ShoppingCart();
            if (!string.IsNullOrEmpty(cookie))
                shoppingCart = JsonConvert.DeserializeObject<ShoppingCart>(cookie);

            shoppingCart.Add(product, quantity);

            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Today.AddMonths(1),
            };
            httpContextAccessor.HttpContext.Response.Cookies.Append("shoppingCart", JsonConvert.SerializeObject(shoppingCart), cookieOptions);
        }

        public async Task ClearCart()
        {
            await Task.Run(() =>
            {
                httpContextAccessor.HttpContext.Response.Cookies.Delete("shoppingCart");
            });
        }

        public async Task RemoveFromCart(int productId)
        {
            await Task.Run(() =>
            {
                var cookie = httpContextAccessor.HttpContext.Request.Cookies["shoppingCart"];
                var shoppingCart = JsonConvert.DeserializeObject<ShoppingCart>(cookie);
                shoppingCart.Remove(productId);

                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Today.AddMonths(1),
                };
                httpContextAccessor.HttpContext.Response.Cookies.Append("shoppingCart", JsonConvert.SerializeObject(shoppingCart), cookieOptions);
            });
        }
    }
}
