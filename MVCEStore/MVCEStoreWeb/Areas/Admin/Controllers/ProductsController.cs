using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcEStoreData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theia.Areas.Admin.Models.DataTables;

namespace MVCEStoreWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrators,ProductAdministrators")]
    public class ProductsController : Controller
    {
        private readonly string entity = "Ürün";

        private readonly AppDbContext context;
        private readonly UserManager<User> userManager;

        public ProductsController(AppDbContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> List(Parameters parameters)
        {
            //TODO: result hazırlanacak
            return Json(default(Result<Product>));
        }

        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View(new Product { Enabled = true });
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product model)
        {
            model.Date = DateTime.Now;
            model.UserId = (await userManager.FindByNameAsync(User.Identity.Name)).Id;

            context.Entry(model).State = EntityState.Added;
            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = $"{entity} ekleme işlemi başarıyla tamamlanmıştır.";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                TempData["error"] = $"Aynı isimli birden fazla {entity.ToLower()} olamaz.";
                await PopulateDropdowns();
                return View(model);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            await PopulateDropdowns();
            return View(await context.Products.FindAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product model)
        {
            model.UserId = (await userManager.FindByNameAsync(User.Identity.Name)).Id;

            context.Entry(model).State = EntityState.Modified;
            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = $"{entity} güncelleme işlemi başarıyla tamamlanmıştır.";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                TempData["error"] = $"Aynı isimli birden fazla {entity.ToLower()} olamaz.";
                await PopulateDropdowns();
                return View(model);
            }
        }


        public async Task<IActionResult> Remove(int id)
        {
            var model = await context.Products.FindAsync(id);

            context.Entry(model).State = EntityState.Deleted;
            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = $"{entity} silme işlemi başarıyla tamamlanmıştır.";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                TempData["error"] = $"{model.Name} isimli {entity.ToLower()}, bir ya da daha fazla kayıt ile ilişkili olduğu için silme işlemi tamamlanamıyor.";
                return View(model);
            }
        }

        private async Task PopulateDropdowns()
        {
            ViewBag.Brands = new SelectList(await context.Brands.OrderBy(p => p.Name).ToListAsync(), "Id", "Name");
            ViewBag.Categories = new SelectList(await context.Categories.OrderBy(p => p.Name).ToListAsync(), "Id", "Name");
        }
    }
}
