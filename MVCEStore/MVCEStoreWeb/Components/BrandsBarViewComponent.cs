using Microsoft.AspNetCore.Mvc;
using MvcEStoreData;
using System.Linq;

namespace MVCEStoreWeb.Components
{
    public class BrandsBarViewComponent : ViewComponent
    {
        private readonly AppDbContext context;

        public BrandsBarViewComponent(
            AppDbContext context
            )
        {
            this.context = context;
        }

        public IViewComponentResult Invoke()
        {
            var model = context.Brands.Where(p => p.Enabled).OrderBy(p => p.SortOrder).ToList();
            return View(model);
        }
    }
}
