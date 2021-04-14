﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MvcEStoreData;

namespace MVCEStoreWeb.Components
{
    public class UserBarViewComponent : ViewComponent
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<User> userManager;

        public UserBarViewComponent(
            IHttpContextAccessor httpContextAccessor,
            UserManager<User> userManager
            )
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
        }

        public IViewComponentResult Invoke()
        {
            var userName = httpContextAccessor.HttpContext.User.Identity.Name;
            if (string.IsNullOrEmpty(userName))
                return View();
            else
            {
                var model = userManager.FindByNameAsync(userName).Result;
                return View(model);
            }
        }
    }
}