using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCEStoreData;
using MVCEStoreWeb.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MVCEStoreWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrators,ProductAdministrators,OrderAdministrators")]
    public class HomeController : Controller
    {
        private readonly AppDbContext context;

        public HomeController(
            AppDbContext context
            )
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var monthLySales =
                (await context.Orders.Where(p => p.Date.Year == DateTime.Today.Year).ToListAsync())
                .GroupBy(p => p.Date.Month)
                .Select(p => new { MonthNumber = p.Key, Sales = p.Sum(q => q.GrandTotal) })
                .ToList();

            var model = new DashboardDataViewModel
            {
                Users = await context.Users.CountAsync(),
                NewOrders = await context.Orders.Where(p => p.OrderStatus == OrderStatus.New).ToListAsync(),
                SalesTotal = (await context.Orders.Where(p => p.OrderStatus == OrderStatus.New || p.OrderStatus == OrderStatus.Shipped).ToListAsync()).Sum(p => p.GrandTotal),
                SalesThisMonth = (await context.Orders.Where(p => p.Date >= DateTime.Today.AddDays(-DateTime.Today.Day) && (p.OrderStatus == OrderStatus.New || p.OrderStatus == OrderStatus.Shipped)).ToListAsync()).Sum(p => p.GrandTotal),
                SalesData = 
                Enumerable.Range(1, 12)
                .GroupJoin(monthLySales, p => p, q => q.MonthNumber, (p, q) => new { MonthNumber = p, MonthlySales = q })
                .SelectMany(p => p.MonthlySales.DefaultIfEmpty(), (p, q) => new SalesDataViewModel { MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(p.MonthNumber), Sales = (q?.Sales ?? 0).ToString(CultureInfo.InvariantCulture) })
                .ToList()
            };
            return View(model);
        }
    }
}
