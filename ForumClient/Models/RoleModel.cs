using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ForumClient.Models
{
    public partial class RoleModel
    {
        [Key]
        public int Id { get; set; }
        public string RoleInfo { get; set; }

    }
}
