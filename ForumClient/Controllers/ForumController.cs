using ForumClient.Models;
using ForumClient.Models.AppDBContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ForumClient.Controllers
{
    public class ForumController : Controller
    {
        private readonly AppDBContext _context;

        public ForumController(AppDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult User_Login()
        {
            return View();
        }
        [Route("Login")]
        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userdetails = await _context.User.SingleOrDefaultAsync(m => m.UserName == model.UserName && m.Password == model.Password);
                if (userdetails == null)
                {
                    ModelState.AddModelError("Password", "Invalid login attempt.");
                    return View("User_Login");
                }
                HttpContext.Session.SetString("userId", userdetails.Name);
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                //return Page();
                return View("User_Login");
            }
            return View("Index");
        }
        public IActionResult User_Signup()
        {
            return View();
        }
        [Route("Register")]
        [HttpPost]
        public async Task<ActionResult> Registar(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    UserName = model.UserName,
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password,
                    Mobile = model.Mobile
                };
                _context.Add(user);
                await _context.SaveChangesAsync();
            }
            else
            {
                foreach (var modelStateKey in ModelState.Keys)
                {
                    var modelStateVal = ModelState[modelStateKey];
                    foreach (var error in modelStateVal.Errors)
                    {
                        var key = modelStateKey;
                        var errorMessage = error.ErrorMessage;
                    }
                }
                return View("User_Signup");
            }
            return RedirectToAction("User_Login");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }
        public void ValidationMessage(string key, string alert, string value)
        {
            try
            {
                TempData.Remove(key);
                TempData.Add(key, value);
                TempData.Add("alertType", alert);
            }
            catch
            {
                Debug.WriteLine("TempDataMessage Error");
            }

        }

        ////////////////

        public IActionResult Categories()
        {
            return View();
        }
        public IActionResult Trending()
        {
            return View();
        }
        public IActionResult New()
        {
            return View();
        }

        public IActionResult Doctor_Login()
        {
            return View();
        }
        public IActionResult Doctor_Signup()
        {
            return View();
        }
        public IActionResult Create_Post()
        {
            return View();
        }
    }
}
