using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShareMeet.Models;
using Mood.ViewModels;

namespace ShareMeet.Controllers
{
    public class AccountController : Controller
    {
        private UsersContext db;
        public AccountController(UsersContext context)
        {
            db = context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                string source = model.Password;
                byte[] ByteData = Encoding.ASCII.GetBytes(source);
                //MD5 creating MD5 object.
                MD5 oMd5 = MD5.Create();
                //Hash değerini hesaplayalım.
                byte[] HashData = oMd5.ComputeHash(ByteData);

                //convert byte array to hex format
                StringBuilder oSb = new StringBuilder();

                for (int x = 0; x < HashData.Length; x++)
                {
                    //hexadecimal string value
                    oSb.Append(HashData[x].ToString("x2"));
                }
                User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == oSb.ToString());

                if (user != null)
                {
                    await Authenticate(model.Email); // аутентификация

                    return RedirectToAction("Index", "Home");

                }

                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {


            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null)
                {
                    string source = model.Password;
                    byte[] ByteData = Encoding.ASCII.GetBytes(source);
                    //MD5 creating MD5 object.
                    MD5 oMd5 = MD5.Create();
                    //Hash değerini hesaplayalım.
                    byte[] HashData = oMd5.ComputeHash(ByteData);

                    //convert byte array to hex format
                    StringBuilder oSb = new StringBuilder();

                    for (int x = 0; x < HashData.Length; x++)
                    {
                        //hexadecimal string value
                        oSb.Append(HashData[x].ToString("x2"));
                    }

                    // добавляем пользователя в бд
                    db.Users.Add(new User
                    {
                        Email = model.Email,
                        Password = oSb.ToString(),
                        FirstName = model.FirstName,
                        Name = model.Name,
                        LastName = model.LastName,
                        Birth = model.Birth,
                        Gender=model.Gender,
                        StatusId = model.StatusId
                    });
                    await db.SaveChangesAsync();

                    await Authenticate(model.Email); // аутентификация

                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        private async Task Authenticate(string userName)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }


    }
}
