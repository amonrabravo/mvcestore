using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MvcEStoreData;
using MVCEStoreWeb.Models;
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

        public HomeController(
            ILogger<HomeController> logger,
            AppDbContext context
            )
        {
            _logger = logger;
            this.context = context;
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
