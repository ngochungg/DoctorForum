using ForumAPI.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration _configuration;

        public AuthenticationController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterModel input)
        {
            var userExist = await userManager.FindByNameAsync(input.Username);
            if(userExist != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message ="Username have already!" });
            }
            var user = new ApplicationUser()
            {
                Email = input.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = input.Username
                //PasswordHash = input.Password
            };
            var result = await userManager.CreateAsync(user, input.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Passwords must have at least one non alphanumeric character(!@#$...) and at least one uppercase ('A'-'Z')." });
            }
            return Ok(new Response { Status = "Success", Message = "User created successfully" });
        }
    }
}
