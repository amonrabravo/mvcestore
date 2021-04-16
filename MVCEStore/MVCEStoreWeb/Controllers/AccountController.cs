using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCEStoreData;
using MVCEStoreWeb.Models;
using MVCEStoreWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCEStoreWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly IMessageService messageService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AccountController(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IMessageService messageService,
            IHttpContextAccessor httpContextAccessor
            )
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.messageService = messageService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl, IsPersistent = true });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, model.IsPersistent, false);
            if (result.Succeeded)
                return Redirect(model.ReturnUrl ?? "/");
            else
            {
                ModelState.AddModelError("", "Geçersiz kullanıcı girişi.");
                return View(model);
            }
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var user = new User
            {
                UserName = model.UserName,
                Name = model.Name,
                Gender = model.Gender
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
            else
            {
                await userManager.AddToRoleAsync(user, "Members");
                var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                var link = Url.RouteUrl("emailconfirmation", new { id = user.Id, token = token }, httpContextAccessor.HttpContext.Request.Scheme);
                await messageService.SendEmailConfirmationMessage(model.UserName, model.Name, link);
                return View("RegisterSuccess");
            }

        }

        public async Task<IActionResult> EmailConfirmation(string id, string token)
        {
            token = System.Net.WebUtility.UrlDecode(token);
            var user = await userManager.FindByIdAsync(id);
            var result = await userManager.ConfirmEmailAsync(user, token);
            return View(result.Succeeded ? "EmailConfirmationSuccess" : "EmailConfirmationFail");
        }

        [Authorize]
        public async Task<IActionResult> Orders()
        {
            var model = await userManager.FindByNameAsync(User.Identity.Name);
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var model = await userManager.FindByNameAsync(User.Identity.Name);
            return View(model);
        }
    }
}
