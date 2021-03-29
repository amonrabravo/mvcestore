﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcEStoreData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCEStoreWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrators,ProductAdministrators")]
    public class RayonsController : Controller
    {
        private readonly string entity = "Reyon";

        private readonly AppDbContext context;
        private readonly UserManager<User> userManager;

        public RayonsController(AppDbContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await context.Rayons.OrderBy(p => p.SortOrder).ToListAsync());
        }

        public IActionResult Create()
        {
            return View(new Rayon { Enabled = true });
        }

        [HttpPost]
        public async Task<IActionResult> Create(Rayon model)
        {
            model.Date = DateTime.Now;
            model.UserId = (await userManager.FindByNameAsync(User.Identity.Name)).Id;

            var lastOrder = context.Rayons.OrderByDescending(p => p.SortOrder).FirstOrDefault()?.SortOrder ?? 0;
            model.SortOrder = lastOrder + 1;

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
                return View(model);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            return View(await context.Rayons.FindAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Rayon model)
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
                return View(model);
            }
        }


        public async Task<IActionResult> Remove(int id)
        {
            var model = await context.Rayons.FindAsync(id);

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

        public async Task<IActionResult> MoveUp(int id)
        {
            var subject = await context.Rayons.FindAsync(id);
            var target = await context.Rayons.Where(p => p.SortOrder < subject.SortOrder).OrderBy(p => p.SortOrder).LastOrDefaultAsync();
            if (target != null)
            {
                var m = subject.SortOrder;
                subject.SortOrder = target.SortOrder;
                target.SortOrder = m;

                context.Entry(subject).State = EntityState.Modified;
                context.Entry(target).State = EntityState.Modified;
                await context.SaveChangesAsync();
                TempData["success"] = $"{entity} silme işlemi başarıyla tamamlanmıştır.";
            }
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> MoveDn(int id)
        {
            var subject = await context.Rayons.FindAsync(id);
            var target = await context.Rayons.Where(p => p.SortOrder > subject.SortOrder).OrderBy(p => p.SortOrder).FirstOrDefaultAsync();
            if (target != null)
            {
                var m = subject.SortOrder;
                subject.SortOrder = target.SortOrder;
                target.SortOrder = m;

                context.Entry(subject).State = EntityState.Modified;
                context.Entry(target).State = EntityState.Modified;
                await context.SaveChangesAsync();
                TempData["success"] = $"{entity} silme işlemi başarıyla tamamlanmıştır.";
            }
            return RedirectToAction("Index");

        }
    }
}
