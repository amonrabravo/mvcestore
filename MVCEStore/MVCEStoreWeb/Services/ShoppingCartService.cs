using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MVCEStoreData;
using MVCEStoreWeb.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
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
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Today.AddMonths(-1),
                };
                httpContextAccessor.HttpContext.Response.Cookies.Delete("shoppingCart", cookieOptions);
            });
        }

        public ShoppingCart GetCart()
        {
            var cookie = httpContextAccessor.HttpContext.Request.Cookies["shoppingCart"];
            var shoppingCart = new ShoppingCart();
            if (!string.IsNullOrEmpty(cookie))
                shoppingCart = JsonConvert.DeserializeObject<ShoppingCart>(cookie);
            var products = context.Products.Where(p => shoppingCart.Items.Select(q => q.ProductId).Any(q => q == p.Id)).ToList();
            shoppingCart
                .Items
                .ToList()
                .ForEach(p => p.Product = products.Single(q => q.Id == p.ProductId));
            return shoppingCart;
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
