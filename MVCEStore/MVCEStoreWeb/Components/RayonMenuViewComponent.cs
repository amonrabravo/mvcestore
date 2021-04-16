using Microsoft.AspNetCore.Mvc;
using MVCEStoreData;
using System.Linq;

namespace MVCEStoreWeb.Components
{
    public class RayonMenuViewComponent : ViewComponent
    {
        private readonly AppDbContext context;

        public RayonMenuViewComponent(
            AppDbContext context
            )
        {
            this.context = context;
        }

        public IViewComponentResult Invoke()
        {
            var model = context.Rayons.Where(p => p.Enabled && p.Categories.Any()).OrderBy(p => p.SortOrder).ToList();
            return View(model);
        }
    }
}
