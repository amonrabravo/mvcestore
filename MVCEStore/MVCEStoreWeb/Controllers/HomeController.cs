using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
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
