using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ForumClient.Models
{
    public class RegistrationViewModel
    {
        [Required]
        [StringLength(10, MinimumLength = 6)]
        public string UserName { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 3)]
        //[StringLength(15, ErrorMessage = "Name length can't be more than 15.")]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Mobile no not valid")]
        public string Mobile { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 6)]
        public string Password { get; set; }
        [Required]
        [NotMapped] // Does not effect with your database
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
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
        public int Look { get; set; }
        public int Share { get; set; }
    }
}
