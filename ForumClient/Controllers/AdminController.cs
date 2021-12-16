using ForumClient.Models;
using ForumClient.Models.AppDBContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ForumClient.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDBContext _context;

        public AdminController(AppDBContext context)
        {
            _context = context;
        }

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Home()
        {
            return View();
        }
        [Route("Admin")]
        [HttpPost]
        public async Task<IActionResult> LoginAdmin(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userdetails = await _context.User.SingleOrDefaultAsync(m => m.UserName == model.UserName && m.Password == CreateMD5(model.Password));
                if (userdetails == null)
                {
                    TempData["Message"] = "Username and password is not correct";
                    return View("Index");
                }
                if (userdetails.Look == 0)
                {
                    TempData["Message"] = "Account has been locked.";
                    return View("Index");
                }
                if (userdetails.RoleId == "1")
                {
                    var is_admin = userdetails.Id.ToString();
                    HttpContext.Response.Cookies.Append("is_admin", is_admin);
                    HttpContext.Session.SetString("userId", userdetails.Name);
                    if (userdetails.Image == null)
                    {
                        HttpContext.Session.SetString("Image", "0");
                    }
                    else
                    {
                        HttpContext.Session.SetString("Image", userdetails.Image);
                    }
                    return View("Home");
                }
                else
                {
                    TempData["Message"] = "Username does not have access rights";
                    return View("Index");

                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return View("Index");
            }
        }

        public async Task<IActionResult> UserView()
        {
            var is_admin = Convert.ToInt32(HttpContext.Request.Cookies["is_admin"]);

            if (is_admin == 1)
            {
                var user = await _context.User.Select(c => new UserModel { Name = c.Name, Birthday = c.Birthday, Email = c.Email, Mobile = c.Mobile, Image = c.Image, RoleId = c.RoleId, Address = c.Address }).ToListAsync();
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            return View("Index");
        }
    }
}
