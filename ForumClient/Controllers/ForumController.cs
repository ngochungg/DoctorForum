using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumClient.Controllers
{
    public class ForumController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
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
        public IActionResult User_Login()
        {
            return View();
        }
        public IActionResult User_Signup()
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
