using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MVCEStoreData;
using MVCEStorePayment;
using MVCEStoreWeb.Models;
using MVCEStoreWeb.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MVCEStoreWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext context;
        private readonly IShoppingCartService shoppingCartService;
        private readonly IPaymentService paymentService;
        private readonly UserManager<User> userManager;

        public HomeController(
            ILogger<HomeController> logger,
            AppDbContext context,
            IShoppingCartService shoppingCartService,
            IPaymentService paymentService,
            UserManager<User> userManager
            )
        {
            _logger = logger;
            this.context = context;
            this.shoppingCartService = shoppingCartService;
            this.paymentService = paymentService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.FeaturedProducts = await context.Products.Where(p => p.Enabled).OrderBy(p => Guid.NewGuid()).Take(16).ToListAsync();
            return View();
        }

        public async Task<IActionResult> Category(int id)
        {
            var model = await context.Categories.FindAsync(id);
            return View(model);
        }

        public async Task<IActionResult> Brand(int id)
        {
            var model = await context.Brands.FindAsync(id);
            return View(model);
        }

        public async Task<IActionResult> Product(int id)
        {
            var model = await context.Products.FindAsync(id);
            return View(model);
        }

        public async Task<IActionResult> AddToCart(int id)
        {
            await shoppingCartService.AddToCart(id, 1);
            TempData["success"] = "Ürün başarıyla sepetinize eklenmiştir...";
            return RedirectToAction("Checkout");
        }

        public async Task<IActionResult> RemoveFromCart(int id)
        {
            await shoppingCartService.RemoveFromCart(id);
            TempData["success"] = "Ürün başarıyla sepetinizden çıkartılmıştır...";
            return RedirectToAction("Checkout");
        }

        public async Task<IActionResult> ClearCart()
        {
            await shoppingCartService.ClearCart();
            TempData["success"] = "Sepetiniz boşaltılmıştır...";
            return RedirectToAction("Checkout");
        }

        public IActionResult Checkout()
        {
            var model = shoppingCartService.GetCart();
            return View(model);
        }

        [Authorize]
        public IActionResult Payment()
        {
            ViewData["shoppingCart"] = shoppingCartService.GetCart();
            return View(new PaymentRequest());
        }

        [Authorize, HttpPost]
        public async Task<IActionResult> Payment(PaymentRequest model)
        {
            model.Amount = shoppingCartService.GetCart().GrandTotal;
            var result = await paymentService.Payment(model);
            if (result.Succeeded)
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                var shoppingCart = shoppingCartService.GetCart();

                var order = new Order
                {
                    Date = DateTime.Now,
                    Enabled = true,
                    OrderStatus = OrderStatus.New,
                    UserId = user.Id

                };
                shoppingCart
                    .Items
                    .ToList()
                    .ForEach(async p =>
                    {
                        var product = await context.Products.FindAsync(p.ProductId);

                        var orderItem = new OrderItem
                        {
                            Discount = product.Discount,
                            Price = product.Price,
                            ProductId = product.Id,
                            Quantity = p.Quantity
                        };
                        context.Entry(orderItem).State = EntityState.Added;
                        order.OrderItems.Add(orderItem);
                    });

                await shoppingCartService.ClearCart();

                context.Entry(order).State = EntityState.Added;
                await context.SaveChangesAsync();
                TempData["success"] = "Teşekkürler, ödeme işleminiz başarıyla tamamlanmıştır.";
                return RedirectToAction("Orders", "Account");
            }
            else
            {
                TempData["error"] = result.Error;
                return View(model);
            }
        }

        public async Task<IActionResult> GetBankData(string binNumber)
        {
            return Json(await BinQuery.CreateQuery(binNumber));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("/home/error/{code:int}")]
        public IActionResult Error(int code)
        {
            switch (code)
            {
                case 404:
                default:
                    return View("~/Views/Shared/Error404.cshtml");
            }
        }
    }
}
