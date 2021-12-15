using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ForumClient.Models
{
    public class UpdateUserView
    {
        public int id { get; set; }
        //[StringLength(15, ErrorMessage = "Name length can't be more than 15.")]
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }
        public string Birthday { get; set; }
        public string Address { get; set; }
        public string Image { get; set; }
        public IFormFile ProfileImage { get; set; }
        public string Experience { get; set; }
        public string Qualification { get; set; }
        public string Professional { get; set; }
        public string CreatedAt { get; set; }
        public string RoleId { get; set; }
        public int Status { get; set; }
        public string Mess { get; set; }
    }
}
