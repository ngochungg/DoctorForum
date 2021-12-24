using ForumClient.Models;
using ForumClient.Models.AppDBContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ForumClient.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDBContext _context;

        private readonly IWebHostEnvironment webHostEnvironment;

        public AdminController(AppDBContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
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
                   
                    HttpContext.Response.Cookies.Append("is_admin", "1");
                    HttpContext.Session.SetString("userId", userdetails.Name);
                    HttpContext.Session.SetString("userName", userdetails.UserName);
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
   
        //Page cus
        [Route("Customerr")]
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
        public async Task<IActionResult> Enable_Cuss(string id)
        {
            UserModel Enable_U = await _context.User.SingleOrDefaultAsync(c => c.UserName == id);
            Enable_U.Look = 1;
            await _context.SaveChangesAsync();
            return RedirectToAction("CusUser");
        }
        public async Task<IActionResult> Disable_Cuss(string id)
        {
            UserModel Disable_U = await _context.User.SingleOrDefaultAsync(c => c.UserName == id);
            Disable_U.Look = 0;
            await _context.SaveChangesAsync();
            return RedirectToAction("CusUser");
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


        //page add user
        public IActionResult Add_admin_user()
        {
            var is_admin = Convert.ToInt32(HttpContext.Request.Cookies["is_admin"]);

            if (is_admin == 1)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public async Task<ActionResult> Add_Admin_Post(RegistrationViewModel model)
        {
            var u = await _context.User.SingleOrDefaultAsync(m => m.UserName == model.UserName);
            var g = await _context.User.SingleOrDefaultAsync(m => m.Email == model.Email);
            if (u != null)
            {
                TempData["Message"] = "User already exists";
                return View("Add_admin_user");
            }
            if (g != null)
            {
                TempData["Message"] = "Email already exists";
                return View("Add_admin_user");
            }
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
                    Image = uniqueFileName,
                    Status = 0,
                    RoleId = "1",
                    Look = 1,
                    Share = 1,
                    CreatedAt = DateTime.Now.ToString(),
                };
                _context.Add(user);
                await _context.SaveChangesAsync();
            }
            else
            {
                return View("Add_admin_user");
            }
            return RedirectToAction("UserView");
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

        //profile
        public async Task<ActionResult> Profile_Admin(string id)
        {
            var is_admin = Convert.ToInt32(HttpContext.Request.Cookies["is_admin"]);

            if (is_admin == 1)
            {
                var profile = await _context.User.SingleOrDefaultAsync(c => c.UserName == id);
                return View(profile);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        //change Pass
        public async Task<IActionResult> Change_Pass_Admin(string id, string mess)
        {
            var is_admin = Convert.ToInt32(HttpContext.Request.Cookies["is_admin"]);
            if (is_admin == 1)
            {
                UserModel MyUser = await _context.User.SingleOrDefaultAsync(c => c.UserName == id);
                Change_pass_view update = new Change_pass_view
                {
                    id = MyUser.Id
                };
                if (mess != null)
                {
                    update.Mess = mess;
                }
                return View(update);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> OldPassword(Change_pass_view model, int id)
        {
            UserModel old_user = await _context.User.SingleOrDefaultAsync(c => c.Id == id);
            if (ModelState.IsValid)
            {
                if (old_user != null)
                {
                    string passOld = CreateMD5(model.OldPassword);
                    if (old_user.Password == passOld)
                    {
                        old_user.Password = CreateMD5(model.Password);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Logout");
                    }
                    else
                    {
                        model.Mess = "Wrong password...!";
                        string sid = old_user.UserName;
                        return RedirectToAction("Change_Pass_Admin", new { id = sid, mess = model.Mess });
                    }
                }
                model.Mess = "Change Fail";
                string uid = old_user.UserName;
                return RedirectToAction("Change_Pass_Admin", new { id = uid, mess = model.Mess });
            }
            else
            {
                model.Mess = "The new password is not the same...!";
                string uid = old_user.UserName;
                return RedirectToAction("Change_Pass_Admin", new { id = uid, mess = model.Mess });
            }
        }

        //topic
        public async Task<IActionResult> TopicView()
        {
            var category = await _context.Topic.ToListAsync();
            return View(category);
        }


        public async Task<ActionResult> TopicDelete(int id)
        {
            var category = await _context.Topic.FindAsync(id);
            if (category == null)
            {
                TempData["Message"] = "Delete Error";
            }
            else
            {
                _context.Topic.Remove(category);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Delete Success";
            }
            return RedirectToAction("TopicView");
        }




        //category
        public IActionResult Categories()
        {
            return View();
        }
        public async Task<IActionResult> CategoriesView()
        {
            var category = await _context.Categories.ToListAsync();
            if (category == null)
            {
                TempData["Message"] = "Have 0 Category";
            }
            return View(category);
        }
        public async Task<ActionResult> CreateCategory(CategoriesModel request)
        {
            if (ModelState.IsValid)
            {
                if (request.created_by == null)
                {
                    request.created_by = HttpContext.Session.GetString("userId");
                }
                request.created_at = DateTime.Now.ToString();
                _context.Categories.Add(request);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Create Success";
            }
            else
            {
                TempData["Message"] = "Create Fail";
            }
            return RedirectToAction(nameof(CategoriesView));
        }
        public async Task<ActionResult> UpdateCategory(CategoriesModel request)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Update(request);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("CategoriesView");
        }
        public async Task<ActionResult> DeleteCategory(string name)
        {
            var category = await _context.Categories.SingleOrDefaultAsync(x=>x.name == name);
            if (category == null)
            {
                TempData["Message"] = "Delete Errr";
            }
            else
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Delete Success";
            }
            var topics = await _context.Topic.ToListAsync();
            foreach (var topic in topics)
            {
                if (topic.Categogies_name == name)
                {
                    _context.Topic.Remove(topic);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("CategoriesView");
        }
        public async Task<ActionResult> DetailCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            return View(category);
        }
        //user Disable
        [Route("Disable_Cus")]
        public async Task<IActionResult> Disable_Cus()
        {
            var is_admin = Convert.ToInt32(HttpContext.Request.Cookies["is_admin"]);

            if (is_admin == 1)
            {
                var disable_Cus = await _context.User.Where(c => c.Look == 0).ToListAsync();
                return View(disable_Cus);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        public async Task<IActionResult> Enable_Users(string id)
        {
            UserModel Enable_U = await _context.User.SingleOrDefaultAsync(c => c.UserName == id);
            Enable_U.Look = 1;
            await _context.SaveChangesAsync();
            return RedirectToAction("Disable_Cus");
        }
        public async Task<IActionResult> Remove_Cus(string id)
        {
            UserModel Cus = await _context.User.SingleOrDefaultAsync(c => c.UserName == id);
            _context.User.Remove(Cus);
            await _context.SaveChangesAsync();
            return RedirectToAction("Disable_Cus");
        }

        //Edit Infomation
        public async Task<IActionResult> EditAdmin(string id, string mess)
        {
            var is_admin = Convert.ToInt32(HttpContext.Request.Cookies["is_admin"]);

            if (is_admin == 1)
            {
                UserModel MyUser = await _context.User.SingleOrDefaultAsync(c => c.UserName == id);
                if (MyUser.Look == 0)
                {
                    return RedirectToAction("Index");
                }
                UpdateUserView update = new UpdateUserView
                {
                    UserName = MyUser.UserName,
                    Name = MyUser.Name,
                    Address = MyUser.Address,
                    Image = MyUser.Image,
                    Email = MyUser.Email,
                    Mobile = MyUser.Mobile,
                    RoleId = MyUser.RoleId,
 
                };
                if (mess != null)
                {
                    update.Mess = mess;
                }
                return View("Update_Admin", update);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditAdmin_Post(UpdateUserView model, string id)
        {
            UserModel old_user = await _context.User.SingleOrDefaultAsync(c => c.UserName == id);
            if (model.ProfileImage != null)
            {
                string uniqueFileName = UpdateFile(model);
                old_user.Image = uniqueFileName;
            }
            if (old_user != null)
            {
                if (old_user.Password == CreateMD5(model.Password))
                {
                    old_user.Name = model.Name;
                    old_user.Email = model.Email;
                    old_user.Address = model.Address;
                    old_user.Mobile = model.Mobile;
                    await _context.SaveChangesAsync();
                    model.Mess = "Update successful...!";
                    string sid = id;
                    return RedirectToAction("EditAdmin", new { id = sid, mess = model.Mess });
                }
                else
                {
                    model.Mess = "Update failed...!";
                    string sid = id;
                    return RedirectToAction("EditAdmin", new { id = sid, mess = model.Mess });
                }
            }
            model.Mess = "Update failed...!";
            string uid = id;
            return RedirectToAction("EditAdmin", new { id = uid, mess = model.Mess });
        }
        public string UpdateFile(UpdateUserView model)
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
