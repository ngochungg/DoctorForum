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


        //page User
        public async Task<IActionResult> UserView()
        {
            var is_admin = Convert.ToInt32(HttpContext.Request.Cookies["is_admin"]);

            if (is_admin == 1)
            {
                var user = await _context.User.Select(c => new UserModel { Name = c.Name, UserName = c.UserName, Email = c.Email, Mobile = c.Mobile, Image = c.Image, RoleId = c.RoleId, Status = c.Status, CreatedAt = c.CreatedAt}).ToListAsync();
                var AllUser = user.Count();
                ViewBag.AllUser = AllUser;
                //user docter
                var docter = await _context.User.Where(c => c.RoleId == "2").ToListAsync();
                ViewBag.Docter = docter.Count();
                //customer
                var cus = await _context.User.Where(c => c.RoleId == "3").ToListAsync();
                ViewBag.Cus = cus.Count();
                //confirmDoc
                var confirm = await _context.User.Where(c => c.Experience != null && c.RoleId == "3").ToListAsync();
                ViewBag.confirm = confirm.Count();
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        public async Task<IActionResult> SearchByUName(string sname)
        {
            var is_admin = Convert.ToInt32(HttpContext.Request.Cookies["is_admin"]);

            if (is_admin == 1)
            {
                if (sname == null)
                {
                    return RedirectToAction("UserView");
                }
                var user = await _context.User.Where(c => c.UserName.Contains(sname)).Select(c => new UserModel { Name = c.Name, UserName = c.UserName, Email = c.Email, Mobile = c.Mobile, Image = c.Image, RoleId = c.RoleId, Status = c.Status, CreatedAt = c.CreatedAt }).ToListAsync();
                return View("UserView",user);
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
        public async Task<IActionResult> ProfileUser(string id)
        {
            var is_admin = Convert.ToInt32(HttpContext.Request.Cookies["is_admin"]);

            if (is_admin == 1)
            {
                UserModel user = await _context.User.SingleOrDefaultAsync(c => c.UserName == id);
                return View("UserProfile", user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        
        //page Docter
        public async Task<IActionResult> DocterUser()
        {
            var is_admin = Convert.ToInt32(HttpContext.Request.Cookies["is_admin"]);

            if (is_admin == 1)
            {
                var docter = await _context.User.Where(c => c.RoleId == "2").ToListAsync();
                return View(docter);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        public async Task<IActionResult> Enable_User(string id)
        {
                UserModel Enable_U = await _context.User.SingleOrDefaultAsync(c => c.UserName == id);
                Enable_U.Look = 1;
                await _context.SaveChangesAsync();
                return RedirectToAction("DocterUser");
        }
        public async Task<IActionResult> Disable_User(string id)
        {
            UserModel Disable_U = await _context.User.SingleOrDefaultAsync(c => c.UserName == id);
            Disable_U.Look = 0;
            await _context.SaveChangesAsync();
            return RedirectToAction("DocterUser");
        }
        public async Task<IActionResult> SearchByUName_Doctor(string sname)
        {
            var is_admin = Convert.ToInt32(HttpContext.Request.Cookies["is_admin"]);

            if (is_admin == 1)
            {
                if (sname == null)
                {
                    return RedirectToAction("DocterUser");
                }
                var user = await _context.User.Where(c => c.UserName.Contains(sname)).Select(c => new UserModel { Name = c.Name, UserName = c.UserName, Email = c.Email, Mobile = c.Mobile, Image = c.Image, RoleId = c.RoleId, Status = c.Status, CreatedAt = c.CreatedAt }).ToListAsync();
                return View("DocterUser", user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        //Page cus
        public async Task<IActionResult> CusUser()
        {
            var is_admin = Convert.ToInt32(HttpContext.Request.Cookies["is_admin"]);

            if (is_admin == 1)
            {
                var docter = await _context.User.Where(c => c.RoleId == "3" ).ToListAsync();
                return View(docter);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        public async Task<IActionResult> Enable_Cus(string id)
        {
            UserModel Enable_U = await _context.User.SingleOrDefaultAsync(c => c.UserName == id);
            Enable_U.Look = 1;
            await _context.SaveChangesAsync();
            return RedirectToAction("CusUser");
        }
        public async Task<IActionResult> Disable_Cus(string id)
        {
            UserModel Disable_U = await _context.User.SingleOrDefaultAsync(c => c.UserName == id);
            Disable_U.Look = 0;
            await _context.SaveChangesAsync();
            return RedirectToAction("CusUser");
        }
        public async Task<IActionResult> SearchByUName_Cus(string sname)
        {
            var is_admin = Convert.ToInt32(HttpContext.Request.Cookies["is_admin"]);

            if (is_admin == 1)
            {
                if(sname == null)
                {
                    return RedirectToAction("CusUser");
                }
                var user = await _context.User.Where(c => c.UserName.Contains(sname)).Select(c => new UserModel { Name = c.Name, UserName = c.UserName, Email = c.Email, Mobile = c.Mobile, Image = c.Image, RoleId = c.RoleId, Status = c.Status, CreatedAt = c.CreatedAt }).ToListAsync();
                return View("CusUser", user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        //Page Confirm_Doctor
        public async Task<IActionResult> Confirm_Doctor()
        {
            var is_admin = Convert.ToInt32(HttpContext.Request.Cookies["is_admin"]);

            if (is_admin == 1)
            {
                var confirm_Doctor = await _context.User.Where(c => c.RoleId == "3" && c.Experience !=null ).ToListAsync();
                return View(confirm_Doctor);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        public async Task<IActionResult> Accept_User(string id)
        {
            UserModel Enable_U = await _context.User.SingleOrDefaultAsync(c => c.UserName == id);
            Enable_U.RoleId = "2";
            await _context.SaveChangesAsync();
            return RedirectToAction("Confirm_Doctor");
        }
        public async Task<IActionResult> Not_Accept_User(string id)
        {
            UserModel Disable_U = await _context.User.SingleOrDefaultAsync(c => c.UserName == id);
            Disable_U.Experience = null;
            Disable_U.Professional = null;
            Disable_U.Qualification = null;
            await _context.SaveChangesAsync();
            return RedirectToAction("Confirm_Doctor");
        }

    }
}
