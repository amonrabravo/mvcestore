using Microsoft.AspNetCore.Mvc;
using MVCEStoreWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCEStoreWeb.Components
{
    public class ShoppingCartButtonViewComponent : ViewComponent
    {
        private readonly IShoppingCartService shoppingCartService;

        public ShoppingCartButtonViewComponent(
            IShoppingCartService shoppingCartService
            )
        {
            this.shoppingCartService = shoppingCartService;
        }

        public IViewComponentResult Invoke()
        {
            var model = shoppingCartService.GetCart().Items.Sum(p => p.Quantity);
            return View(model);
        }
    }
}
