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
using System.Security.Cryptography;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace ForumClient.Controllers
{
    public class ForumController : Controller
    {
        private readonly AppDBContext _context;

        private readonly IWebHostEnvironment webHostEnvironment;

        public ForumController(AppDBContext context,IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
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
                var userdetails = await _context.User.SingleOrDefaultAsync(m => m.UserName == model.UserName && m.Password == CreateMD5(model.Password));
                if (userdetails == null)
                {
                    ModelState.AddModelError("Password", "Invalid login attempt.");
                    return View("User_Login");
                }

                userdetails.Status += 1;
                await _context.SaveChangesAsync();
                HttpContext.Session.SetString("userId", userdetails.Name);
                HttpContext.Session.SetString("Role", userdetails.RoleId);
                if (userdetails.Image == null)
                {
                    HttpContext.Session.SetString("Image", "0");
                }
                else
                {
                    HttpContext.Session.SetString("Image", userdetails.Image);
                }

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
                string uniqueFileName = UploadedFile(model);

                UserModel user = new UserModel
                {
                    UserName = model.UserName,
                    Name = model.Name,
                    Email = model.Email,
                    Password = CreateMD5(model.Password),
                    Mobile = model.Mobile,
                    Birthday = model.Birthday,
                    Address = model.Address,
                    Image = uniqueFileName,
                    Status = 0,
                    RoleId = "3",
                    CreatedAt = DateTime.Now.ToString(),
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
        public string UploadedFile(RegistrationViewModel model)
        {
            string uniqueFileName = null;

            if (model.ProfileImage != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "update_images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProfileImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ProfileImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
