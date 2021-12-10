using System;
using System.Collections.Generic;

#nullable disable

namespace ForumAPI.Models
{
    public partial class Customer
    {
        public int CusId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Birthday { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Image { get; set; }
        public int? Status { get; set; }
    }
}
