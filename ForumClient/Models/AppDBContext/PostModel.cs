using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ForumClient.Models.AppDBContext
{
    public partial class PostModel
    {
        [Key]
        public int Post_Id { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Content { set; get; }
    }
}
