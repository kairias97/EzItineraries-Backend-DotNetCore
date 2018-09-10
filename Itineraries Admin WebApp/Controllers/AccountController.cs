using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ItinerariesAdminWebApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItinerariesAdminWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAdministratorRepository administratorRepository;

        public AccountController(IAdministratorRepository adminRepository)
        {
            administratorRepository = adminRepository;
        }
        public IActionResult Login()
        {
            ViewBag.AccountCreated = TempData["accountCreated"];
            return View(new Credentials());
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(Credentials credentials)
        {
            if (!ModelState.IsValid)
            {
                return View(credentials);
            }
            var foundUser = administratorRepository.ValidateCredentials(credentials.Email, credentials.Password);
            //null means not found
            if (foundUser == null)
            {
                ViewBag.InvalidCredentials = true;
                return View(credentials);
            } else
            {
                //bool saved = await SaveCookies(user);
                var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, foundUser.Email),
                            new Claim(ClaimTypes.NameIdentifier, foundUser.Id.ToString()),
                            new Claim(ClaimTypes.Role, foundUser.IsGlobal ? "GlobalAdmin" : "AttractionsAdmin")
                        };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                return RedirectToAction("Index", "Home");
            }
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}