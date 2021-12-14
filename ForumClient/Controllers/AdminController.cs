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

        [Route("Admin")]
        [HttpPost]
        public async Task<IActionResult> LoginAdmin(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userdetails = await _context.User.SingleOrDefaultAsync(m => m.UserName == model.UserName && m.Password == CreateMD5(model.Password));
                if (userdetails == null)
                {
                    ModelState.AddModelError("Password", "Invalid login attempt.");
                    return View("Index");
                }
                if (userdetails.RoleId == "1")
                {
                    return View("Home");
                }
                else
                {
                    return View("Index");

                }
                //HttpContext.Session.SetString("userId", userdetails.Name);
                //HttpContext.Session.SetString("Role", userdetails.RoleId);
                //var id = userdetails.Id.ToString();
                //HttpContext.Session.SetString("Id", id);
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                //return Page();
                return View("Index");
            }
        }

        public IActionResult Home()
        {
            return View();
        }
    }
}
