using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumClient.Models
{
    public class Role
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
