using System;
using System.Collections.Generic;

#nullable disable

namespace ForumAPI.Models
{
    public partial class Doctor
    {
        public int DocId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Birthday { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int? RoleId { get; set; }
        public string Image { get; set; }
        public string Experience { get; set; }
        public string Qualification { get; set; }
        public string Professional { get; set; }
        public int? Status { get; set; }
        public string CreatedAt { get; set; }

        public virtual Role Role { get; set; }
    }
}
