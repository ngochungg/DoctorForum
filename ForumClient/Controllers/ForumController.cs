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

        public ForumController(AppDBContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }
        #region User
        #region change_password
        public async Task<IActionResult> Change_Password_View(int id, string mess)
        {
            var is_login = HttpContext.Request.Cookies["is_Login"];
            if (is_login != null)
            {
                var login_id = Convert.ToInt16(is_login);
                if (login_id == id)
                {
                    UserModel MyUser = await _context.User.SingleOrDefaultAsync(c => c.Id == id);

                    Change_pass_view update = new Change_pass_view
                    {
                        id = MyUser.Id
                    };
                    if (mess != null)
                    {
                        update.Mess = mess;
                    }
                    return View("Change_pass", update);
                }
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Change_Password(Change_pass_view model, int id)
        {
            if (ModelState.IsValid)
            {
                UserModel old_user = await _context.User.SingleOrDefaultAsync(c => c.Id == id);
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
                        int sid = id;
                        return RedirectToAction("Change_Password_View", new { id = sid, mess = model.Mess });
                    }
                }
                model.Mess = "Change Fail";
                int uid = id;
                return RedirectToAction("Change_Password_View", new { id = uid, mess = model.Mess });
            }
            else
            {
                model.Mess = "The new password is not the same...!";
                int uid = id;
                return RedirectToAction("Change_Password_View", new { id = uid, mess = model.Mess });
            }
        }
        #endregion

        #region confirmed_docter
        public async Task<IActionResult> Confirmed_docter_view(int id, string mess)
        {
            var is_login = HttpContext.Request.Cookies["is_Login"];
            if (is_login != null)
            {
                var login_id = Convert.ToInt16(is_login);
                if (login_id == id)
                {
                    UserModel MyUser = await _context.User.SingleOrDefaultAsync(c => c.Id == id);

                    Confirmed_docter_view update = new Confirmed_docter_view
                    {
                        id = MyUser.Id,
                        Name = MyUser.Name,
                        Image = MyUser.Image,
                        Experience = MyUser.Experience,
                        Qualification = MyUser.Qualification,
                        Professional = MyUser.Professional,
                    };
                    if (mess != null)
                    {
                        update.Mess = mess;
                    }
                    return View("Confirmed_docter_view", update);
                }
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Confirmed_docter(Confirmed_docter_view model, int id)
        {
            UserModel old_user = await _context.User.SingleOrDefaultAsync(c => c.Id == id);
            if (old_user != null)
            {
                if (old_user.Password == CreateMD5(model.Password))
                {
                    old_user.Experience = model.Experience;
                    old_user.Qualification = model.Qualification;
                    old_user.Professional = model.Professional;
                    await _context.SaveChangesAsync();
                    return View("waitAdminConfirmed");
                }
                else
                {
                    model.Mess = "Wrong password...!";
                    int sid = id;
                    return RedirectToAction("Confirmed_docter_view", new { id = sid, mess = model.Mess });
                }
            }
            model.Mess = "Update failed...!";
            int uid = id;
            return RedirectToAction("Confirmed_docter_view", new { id = uid, mess = model.Mess });
        }
        #endregion
        #region update_infor
        public async Task<IActionResult> Update_User_View(int id, string mess)
        {
            var is_login = HttpContext.Request.Cookies["is_Login"];
            if (is_login != null)
            {
                var login_id = Convert.ToInt16(is_login);
                if (login_id == id)
                {
                    UserModel MyUser = await _context.User.SingleOrDefaultAsync(c => c.Id == id);

                    UpdateUserView update = new UpdateUserView
                    {
                        id = MyUser.Id,
                        Name = MyUser.Name,
                        Address = MyUser.Address,
                        Image = MyUser.Image,
                        Experience = MyUser.Experience,
                        Qualification = MyUser.Qualification,
                        Professional = MyUser.Professional,
                        Email = MyUser.Email,
                        Mobile = MyUser.Mobile,
                        RoleId = MyUser.RoleId
                    };
                    if (mess != null)
                    {
                        update.Mess = mess;
                    }
                    return View("User_Update", update);
                }
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Update_User(UpdateUserView model, int id)
        {
            UserModel old_user = await _context.User.SingleOrDefaultAsync(c => c.Id == id);
            if (model.ProfileImage != null)
            {
                string uniqueFileName = UpdateFile(model);
                old_user.Image = uniqueFileName;
            }
            if (model.Birthday != null)
            {
                old_user.Birthday = model.Birthday;
            }
            if (old_user != null)
            {
                if (old_user.Password == CreateMD5(model.Password))
                {
                    old_user.Name = model.Name;
                    old_user.Email = model.Email;
                    old_user.Address = model.Address;
                    old_user.Mobile = model.Mobile;
                    old_user.Experience = model.Experience;
                    old_user.Qualification = model.Qualification;
                    old_user.Professional = model.Professional;
                    await _context.SaveChangesAsync();
                    model.Mess = "Update successful...!";
                    int sid = id;
                    return RedirectToAction("Update_User_View", new { id = sid, mess = model.Mess });
                }
                else
                {
                    model.Mess = "Update failed...!";
                    int sid = id;
                    return RedirectToAction("Update_User_View", new { id = sid, mess = model.Mess });
                }
            }
            model.Mess = "Update failed...!";
            int uid = id;
            return RedirectToAction("Update_User_View", new { id = uid, mess = model.Mess });
        }
        #endregion
        #region Proflie
        public async Task<IActionResult> User_View(int id)
        {
            var is_login = HttpContext.Request.Cookies["is_Login"];
            if (is_login != null)
            {
                var login_id = Convert.ToInt16(is_login);
                if (login_id == id)
                {
                    UserModel MyUser = await _context.User.SingleOrDefaultAsync(c => c.Id == id);
                    return View("User_View", MyUser);
                }
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }

        }
        #endregion
        #region Login
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
                var is_login = userdetails.Id.ToString();
                HttpContext.Response.Cookies.Append("is_Login", is_login);
                var id = userdetails.Id.ToString();
                HttpContext.Session.SetString("Id", id);
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
        #endregion
        #region Registar_And_logout
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
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            return View("Index");
        }
        #endregion
        #endregion
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



        public IActionResult Trending()
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
        public async Task<ActionResult> Categories()
        {
            var category = await _context.Categories.ToListAsync();
            return View(category);
        }
        public IActionResult Create_Topic()
        {
            var cate = _context.Categories;
            ViewBag.cate = cate;
            return View();
        }
        public async Task<ActionResult> CreatePost(TopicModel request)
        {
            if (ModelState.IsValid && HttpContext.Session.GetString("userId") != null)
            {
                TopicModel topic = new TopicModel
                {
                    Categogies_name = request.Categogies_name,
                    Contents = request.Contents,
                    Created_at = DateTime.Now.ToString(),
                    Title = request.Title,
                    Username = HttpContext.Session.GetString("userId")
                };
                _context.Topic.Add(topic);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Success";
            }
            else
            {
                TempData["Message"] = "Could not create maybe because you are not logged in";
            }
            return RedirectToAction("Create_Topic");
        }
        public async Task<ActionResult> UpdatePost(TopicModel request)
        {
            if (ModelState.IsValid)
            {
                _context.Topic.Update(request);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Create_Post");
        }
        public async Task<ActionResult> DeletePost(int id)
        {
            var category = await _context.Topic.FindAsync(id);
            if (category == null) return RedirectToAction("Index");
            await _context.SaveChangesAsync();
            _context.Topic.Remove(category);
            return RedirectToAction("Create_Post");
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
