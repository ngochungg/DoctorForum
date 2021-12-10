using System;
using System.Collections.Generic;

#nullable disable

namespace ForumAPI.Models
{
    public partial class Role
    {
        public Role()
        {
            Doctors = new HashSet<Doctor>();
        }

        public int RoleId { get; set; }
        public string RoleInfo { get; set; }

        public virtual ICollection<Doctor> Doctors { get; set; }
    }
}
