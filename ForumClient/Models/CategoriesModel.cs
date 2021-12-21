using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ForumClient.Models
{
    public partial class CategoriesModel
    {
        [Key]
        public int cate_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string created_by { get; set; }
        public string created_at { get; set; }
    }
}
